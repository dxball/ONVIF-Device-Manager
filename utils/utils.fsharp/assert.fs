[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Assert
    open System
    open System.Threading
    open System.Collections.Generic
    open System.Runtime.CompilerServices
    open System.Reactive.Concurrency
    open System.Reactive.Disposables

    type AssertionFailedException = class
        inherit Exception
        new () ={inherit Exception()}
        new (message:string) = {inherit Exception(message)}
        new (message:string, innerException:Exception) = {inherit Exception(message, innerException)}
    end
    
    let Fail() = 
        ThreadPool.QueueUserWorkItem(fun o->raise <| new AssertionFailedException()) |> ignore
        while true do ()
    
    let FailWithMessage(message) = 
        ThreadPool.QueueUserWorkItem(fun o->raise <| new AssertionFailedException(message)) |> ignore
        while true do ()

    let FailWithError(err) = 
        ThreadPool.QueueUserWorkItem(fun o->raise err) |> ignore
        while true do ()

    let Try cont = try cont() with err-> FailWithError err
    //let Faultless cont = fun()-> try cont() with err-> FailWithError err
    
    let Is x v = if x<>v then Fail()
    let IsNull x = if x<>null then Fail()
    let NotNull x = if x=null then Fail()
    let IsNone (x: 'T option) = if x.IsSome then Fail()
    let IsSome (x: 'T option) = if x.IsSome then Fail()
    let IsTrue cond = Is true cond
    let IsFalse cond = Is false cond
    let True pred expr = IsTrue (pred expr)
    let False pred expr = IsFalse (pred expr)