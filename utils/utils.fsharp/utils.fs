namespace utils
    open System
    open System.Collections.Generic
    open System.Threading
    open System.Threading.Tasks
    open System.Reactive.Disposables
    open System.Reactive.Concurrency
    //open utils

    type Attempt<'T> = 
        |Success of 'T
        |Error of Exception

    type Trampoline(max_workload) = class
        let mutable acquired = false
        let queue = new Queue<(unit->unit)>()
        let rec process_queue_in_task() = 
            let cont = lock queue (fun()->
                if queue.Count > 0 then
                    let act = queue.Dequeue()
                    fun()->
                        try act() with err -> dbg.Error(err)
                        true
                else
                    acquired <- false
                    fun()->false
            )
            if cont() then process_queue_in_task()

        let rec process_queue(processedOperations) = 
            let cont = lock queue (fun()->
                if queue.Count > 0 then
                    let act = queue.Dequeue()
                    let workload = queue.Count + processedOperations
                    (fun()->
                        if workload > max_workload then
                            Async.StartAsTask(async{
                                try act() with err -> dbg.Error(err)
                                process_queue_in_task()
                            }) |> ignore
                            false
                        else
                            try act() with err -> dbg.Error(err)
                            true
                    )
                else
                    acquired <- false
                    (fun()->false)
            )
            if cont() then process_queue(processedOperations+1)

        new () = Trampoline(max_workload = 10)

        member this.Drop (act:Action) =
            let acquirer = lock queue (fun()->
                queue.Enqueue(fun()->act.Invoke())
                let acquirer = not acquired
                acquired <- true
                acquirer
            )
            if acquirer then process_queue(0)
    end

//    type TrampolineScheduler() = class
//        let tramp = new Trampoline()
//        
//        interface IScheduler with
//            member s.Schedule(act:Action) = 
//                let disp = new BooleanDisposable()
//                tramp.Drop(fun()->
//                    if not disp.IsDisposed then act.Invoke()
//                )
//                disp :> IDisposable
//            member s.Schedule(act:Action, span:TimeSpan) = 
//                let disp = new MutableDisposable()
//                tramp.Drop(fun()->
//                    disp.Disposable<-Scheduler.ThreadPool.Schedule((fun()->
//                        tramp.Drop(fun()->
//                            disp.Disposable<- (s:>IScheduler).Schedule(act)
//                        )
//                    ),span)
//                )
//                disp :> IDisposable
//            member s.Now = Scheduler.ThreadPool.Now
//        end        
//    end