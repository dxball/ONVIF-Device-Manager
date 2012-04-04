namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Diagnostics
    open System.Linq
    open System.Net
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.core
    open odm.infra
    open utils
    //open odm.models
    open utils.fsharp
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    type TimeSettingsActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        let load() = async{
            let! dateTimeInfo =  session.GetSystemDateAndTime()
            let model = new TimeSettingsView.Model(
                timestamp = Stopwatch.GetTimestamp(),
                localDateTime = dateTimeInfo.LocalDateTime.ToSystemDateTime(),
                utcDateTime = dateTimeInfo.UTCDateTime.ToSystemDateTime(DateTimeKind.Utc),
                useDateTimeFromNtp = (dateTimeInfo.DateTimeType = SetDateTimeType.NTP)
            )
            
            model.timeZone <- 
                if dateTimeInfo.TimeZone <> null then 
                    dateTimeInfo.TimeZone.TZ
                else
                    null
            model.daylightSavings <- dateTimeInfo.DaylightSavings
            
            model.AcceptChanges()
            return model
        }
        
        ///<summary></summary>
        member private this.Main() = async{
            let! cont = async{
                try
                    let! model = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! load()
                    }
                    return this.ShowForm(model)
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        ///<summary></summary>
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = TimeSettingsView.Show(ctx, model)
                    return res.Handle(
                        syncWithSystem = (fun m -> this.SyncWithSystem(m)),
                        syncWithNtp = (fun m -> this.SyncWithNtp(m)),
                        setManual = (fun m t -> this.SetManual(m,t)),
                        close = (fun () -> this.Complete())
                    )
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        ///<summary></summary>
        member private this.SyncWithSystem(model) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                    let utcNow = System.DateTime.UtcNow;
                    let newUtc = new DateTime()
                    newUtc.Date <- new Date(
                        Year = utcNow.Year,
                        Month = utcNow.Month,
                        Day = utcNow.Day
                    )
                    newUtc.Time <- new Time(
                        Hour = utcNow.Hour,
                        Minute = utcNow.Minute,
                        Second = utcNow.Second
                    )
                    let newTz = new TimeZone(TZ = model.timeZone)
                    do! session.SetSystemDateAndTime(SetDateTimeType.Manual, model.daylightSavings, newTz, newUtc)
                    return async{
                        do! InfoView.Show(ctx, LocalTimeZone.instance.applySuccess) |> Async.Ignore
                        return! this.Main()
                    }
                with err ->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        ///<summary></summary>
        member private this.SyncWithNtp(model) = async{
            let! cont = async{
                try
                    if model.useDateTimeFromNtp && not(model.isModified) then
                        return this.Main()
                    else
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        let newTz = new TimeZone(TZ = model.timeZone)
                        do! session.SetSystemDateAndTime(SetDateTimeType.NTP, model.daylightSavings, newTz, null)
                        return async{
                            do! InfoView.Show(ctx, LocalTimeZone.instance.applySuccess) |> Async.Ignore
                            return! this.Main()
                        }
                with err ->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

                ///<summary></summary>
        member private this.SetManual(model, utcDateTime) = async{
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                    let newUtc = new DateTime()
                    newUtc.Date <- new Date(
                        Year = utcDateTime.Year,
                        Month = utcDateTime.Month,
                        Day = utcDateTime.Day
                    )
                    newUtc.Time <- new Time(
                        Hour = utcDateTime.Hour,
                        Minute = utcDateTime.Minute,
                        Second = utcDateTime.Second
                    )
                    let newTz = new TimeZone(TZ = model.timeZone)
                    do! session.SetSystemDateAndTime(SetDateTimeType.Manual, model.daylightSavings, newTz, newUtc)
                    return async{
                        do! InfoView.Show(ctx, LocalTimeZone.instance.applySuccess) |> Async.Ignore
                        return! this.Main()
                    }
                with err ->
                    dbg.Error(err)
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        ///<summary></summary>
        member private this.Complete(result) = async{
            return result
        }

        ///<summary></summary>
        static member Run(ctx) = 
            let act = new TimeSettingsActivity(ctx)
            act.Main()
    end
