module utils.fsharp
    open System
    open System.Collections
    open System.Collections.Generic
    open System.Threading
    open System.Reactive.Concurrency
    open System.Reactive.Disposables
    //open utils
    
    let inline CastAs(o:obj) = 
        match o with
        | :? ^T as res -> res
        | _ -> null
    
    ///<summary>Math extensions</summary>
    type Math = class
        static member Coerce min max value = 
            if value < min then min
            elif value > max then max
            else value
    end
    
    ///<summary></summary>
    type AsyncObserver<'T> = ('T->unit)*(Exception->unit)
    
    type internal MemoizedAsyncState<'TResult> =
        |Completed of 'TResult
        |Failed of Exception
        |Processing of LinkedList<('TResult->unit)*(Exception->unit)>
        |Idle
    
    ///<summary>Async extensions</summary>
    type Async with
    
        ///<summary></summary>
        static member SleepEx(t) = async{
            let disp = new SerialDisposable()
            use! ch = Async.OnCancel(fun()->
                disp.Dispose()
            )
            do! Async.FromContinuations(fun (success, error, cancel) ->
                let completed = ref 0
                let CompleteWith cont = 
                    let completor = Interlocked.Exchange(completed, 1) = 0
                    if completor then cont()
                
                let period = Timeout.Infinite
                let tmr = new Timer((fun (o:obj) -> CompleteWith(success)), null, t, period)
                disp.Disposable <- Disposable.Create(fun ()->
                    CompleteWith(fun()->
                        tmr.Dispose()
                        cancel(new OperationCanceledException())
                    )
                )
            )
        }
    
        ///<summary></summary>
        static member CreateWithDisposable(factory:AsyncObserver<'T>->IDisposable):Async<'T> = async{
            let cancel_disp = new SingleAssignmentDisposable()
            use! ch = Async.OnCancel(fun ()-> cancel_disp.Dispose())
                
            let complete_disp = new SingleAssignmentDisposable()
            let CompleteWith = 
                let completed = ref 0
                fun cont ->
                    if Interlocked.Exchange(completed,1)=0 then 
                        cont()
                        complete_disp.Dispose()
                    
            return! Async.FromContinuations(fun (success, error, cancel)->
                complete_disp.Disposable <- factory(
                    (fun res->CompleteWith (fun ()->success res)), 
                    (fun err->CompleteWith (fun ()->error err))
                )
                cancel_disp.Disposable <- Disposable.Create(fun()->
                    CompleteWith(fun()-> cancel (new OperationCanceledException()))
                )
            )
        }
    
        ///<summary></summary>
        static member StartChilds(comp1, comp2) = async{
            let! c1 = Async.StartChild(comp1)
            let! c2 = Async.StartChild(comp2)
            return async{
                let! r1 = c1
                let! r2 = c2
                return (r1, r2)
            }
        }
    
        ///<summary></summary>
        static member StartChilds(comp1, comp2, comp3) = async{
            let! c1 = Async.StartChild(comp1)
            let! c2 = Async.StartChild(comp2)
            let! c3 = Async.StartChild(comp3)
            return async{
                let! r1 = c1
                let! r2 = c2
                let! r3 = c3
                return (r1, r2, r3)
            }
        }
    
        ///<summary></summary>
        static member StartChilds(comp1, comp2, comp3, comp4) = async{
            let! c1 = Async.StartChild(comp1)
            let! c2 = Async.StartChild(comp2)
            let! c3 = Async.StartChild(comp3)
            let! c4 = Async.StartChild(comp4)
            return async{
                let! r1 = c1
                let! r2 = c2
                let! r3 = c3
                let! r4 = c4
                return (r1, r2, r3, r4)
            }
        }
    
        ///<summary></summary>
        static member StartChilds(comp1, comp2, comp3, comp4, comp5) = async{
            let! c1 = Async.StartChild(comp1)
            let! c2 = Async.StartChild(comp2)
            let! c3 = Async.StartChild(comp3)
            let! c4 = Async.StartChild(comp4)
            let! c5 = Async.StartChild(comp5)
            return async{
                let! r1 = c1
                let! r2 = c2
                let! r3 = c3
                let! r4 = c4
                let! r5 = c5
                return (r1, r2, r3, r4, r5)
            }
        }
    
        ///<summary></summary>
        static member StartChilds(comp1, comp2, comp3, comp4, comp5, comp6) = async{
            let! c1 = Async.StartChild(comp1)
            let! c2 = Async.StartChild(comp2)
            let! c3 = Async.StartChild(comp3)
            let! c4 = Async.StartChild(comp4)
            let! c5 = Async.StartChild(comp5)
            let! c6 = Async.StartChild(comp6)
            return async{
                let! r1 = c1
                let! r2 = c2
                let! r3 = c3
                let! r4 = c4
                let! r5 = c5
                let! r6 = c6
                return (r1, r2, r3, r4, r5, r6)
            }
        }
    
        ///<summary></summary>
        static member StartChilds(comp1, comp2, comp3, comp4, comp5, comp6, comp7) = async{
            let! c1 = Async.StartChild(comp1)
            let! c2 = Async.StartChild(comp2)
            let! c3 = Async.StartChild(comp3)
            let! c4 = Async.StartChild(comp4)
            let! c5 = Async.StartChild(comp5)
            let! c6 = Async.StartChild(comp6)
            let! c7 = Async.StartChild(comp7)
            return async{
                let! r1 = c1
                let! r2 = c2
                let! r3 = c3
                let! r4 = c4
                let! r5 = c5
                let! r6 = c6
                let! r7 = c7
                return (r1, r2, r3, r4, r5, r6, r7)
            }
        }
    
        ///<summary></summary>
        static member StartChilds(comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8) = async{
            let! c1 = Async.StartChild(comp1)
            let! c2 = Async.StartChild(comp2)
            let! c3 = Async.StartChild(comp3)
            let! c4 = Async.StartChild(comp4)
            let! c5 = Async.StartChild(comp5)
            let! c6 = Async.StartChild(comp6)
            let! c7 = Async.StartChild(comp7)
            let! c8 = Async.StartChild(comp8)
            return async{
                let! r1 = c1
                let! r2 = c2
                let! r3 = c3
                let! r4 = c4
                let! r5 = c5
                let! r6 = c6
                let! r7 = c7
                let! r8 = c8
                return (r1, r2, r3, r4, r5, r6, r7, r8)
            }
        }
    
        ///<summary></summary>
        static member Parallel(comp1, comp2) = async{
            let! comp = Async.StartChilds(comp1, comp2)
            return! comp
        }
        
        ///<summary></summary>
        static member Parallel(comp1, comp2, comp3) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3)
            return! comp
        }
    
        ///<summary></summary>
        static member Parallel(comp1, comp2, comp3, comp4) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4)
            return! comp
        }
    
        ///<summary></summary>
        static member Parallel(comp1, comp2, comp3, comp4, comp5) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5)
            return! comp
        }
    
        ///<summary></summary>
        static member Parallel(comp1, comp2, comp3, comp4, comp5, comp6) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5, comp6)
            return! comp
        }
    
        ///<summary></summary>
        static member Parallel(comp1, comp2, comp3, comp4, comp5, comp6, comp7) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5, comp6, comp7)
            return! comp
        }
    
        ///<summary></summary>
        static member Parallel(comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8) = async{
            let! comp = Async.StartChilds(comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8)
            return! comp
        }
        
        ///<summary></summary>
        static member Memoize(comp:Async<'T>):Async<'T> = 
            let tramp = new Trampoline()
            let state = ref MemoizedAsyncState<'T>.Idle
            let subscribe (observers:LinkedList<AsyncObserver<'T>>) (observer:AsyncObserver<'T>) =
                let node = observers.AddLast(observer)
                Disposable.Create(fun()->
                    tramp.Drop(fun()->
                        observers.Remove(node)
                    )
                )
            let StartComp() =
                let observers = new LinkedList<AsyncObserver<'T>>()
                let CompCompleted(result: 'T) = tramp.Drop(fun()->
                    state := Completed(result)
                    for (onSuccess, onError) in observers do
                        onSuccess(result)
                )
                let CompFailed(error: Exception) = tramp.Drop(fun()->
                    state := Failed(error)
                    for (onSuccess, onError) in observers do
                        onError(error)
                )
                Async.StartWithContinuations(
                    comp,
                    CompCompleted,
                    CompFailed,
                    CompFailed
                )
                state := Processing(observers)
                observers
    
            Async.CreateWithDisposable(fun (onSuccess, onError)->
                let disp = new SingleAssignmentDisposable()
                tramp.Drop(fun()->
                    match !state with
                    |Idle ->
                        let observers = StartComp()
                        disp.Disposable <- (onSuccess, onError) |> subscribe observers
                    |Processing observers ->
                        disp.Disposable <- (onSuccess, onError) |> subscribe observers
                    |Completed result ->
                        onSuccess result
                    |Failed err ->
                        onError err
                )
                disp :> IDisposable
            )
    end
    
    ///<summary>IScheduler extensions</summary>
    type IScheduler with
        
        ///<summary></summary>
        member this.InvokeAsync f = async{
            let disp = new SingleAssignmentDisposable()
            use! onCancel = Async.OnCancel(fun ()->
                disp.Dispose()
            )
            return! Async.FromContinuations(fun (success, error, cancel)->
                let complete_with =
                    let completed = ref 0
                    fun cont -> 
                        if Interlocked.Exchange(completed, 1) = 0 then
                            cont()
                let d = new SingleAssignmentDisposable()
                disp.Disposable <- Disposable.Create(fun()->
                    d.Dispose()
                    complete_with (fun ()-> cancel (new OperationCanceledException()))
                )
                d.Disposable <- this.Schedule(fun()->
                    complete_with (fun ()->
                        let cont = 
                            try
                                let res = f()
                                (fun ()-> success res)
                            with
                                err->(fun()->error err)
                        cont()
                    )
                )
            )
        }
    end
    
    type System.Windows.Threading.Dispatcher with
        member this.InvokeAsync f = async{
            let disp = new BooleanDisposable()
            use! cancellation = Async.OnCancel(fun()->
                disp.Dispose()
            )
            return! Async.FromContinuations(fun (success, error, cancel) ->
                this.BeginInvoke(
                    new System.Action(fun ()->
                        let cont = 
                            try
                                if not(disp.IsDisposed) then 
                                    let result = f()
                                    fun ()->success(result)
                                else
                                    fun()->cancel (new OperationCanceledException())
                            with err ->
                                fun()->error err
                        cont()
                    ),
                    null
                ) |> ignore
            )
        }
    end
