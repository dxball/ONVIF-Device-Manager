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
                localDateTime = dateTimeInfo.LocalDateTime.ToSystemDateTime()
            )
            //let! ntpInfo = session.GetNTP()

            //if ntpInfo<>null then
            //    model.useNtpFromDhcp <- ntpInfo.FromDHCP
            //    model.ntp <- String.Empty
            //    if ntpInfo.NTPManual<>null then
            //        model.ntp <- String.Join("; ", ntpInfo.NTPManual.Select(fun x -> NetHostToString(x)))
            //    if ntpInfo.NTPFromDHCP<>null then
            //        model.ntp <- String.Join("; ", ntpInfo.NTPFromDHCP.Select(fun x -> NetHostToString(x)))

            model.useDateTimeFromNtp <- dateTimeInfo.DateTimeType = SetDateTimeType.NTP
            model.timeZone <- dateTimeInfo.TimeZone.TZ
            model.daylightSavings <- dateTimeInfo.DaylightSavings
            
            // seconds can be in range 0..61 (leap seconds), see http://www.onvif.org/onvif/ver10/device/wsdl/devicemgmt.wsdl, section 38
            let seconds = 
                if dateTimeInfo.UTCDateTime.Time.Second <= 59 then 
                    dateTimeInfo.UTCDateTime.Time.Second
                else
                    59

            let dateTime = 
                let mutable date = new System.DateTime(
                    dateTimeInfo.UTCDateTime.Date.Year, 1, 1, 0, 0, seconds, DateTimeKind.Utc
                )
                date <- date.AddMonths(dateTimeInfo.UTCDateTime.Date.Month - 1)
                date <- date.AddDays(float(dateTimeInfo.UTCDateTime.Date.Day - 1))
                date <- date.AddHours(float(dateTimeInfo.UTCDateTime.Time.Hour))
                date <- date.AddMinutes(float(dateTimeInfo.UTCDateTime.Time.Minute))
                date

            //dateTime
            model.utcDateTime <- dateTime//dateTimeInfo.UTCDateTime.ToSystemDateTime()
            model.AcceptChanges()
            return model
        }

        let apply_changes(model:TimeSettingsView.Model) = async{
            let dateTime_changed = 
                model.origin.utcDateTime <> model.current.utcDateTime ||
                model.origin.timeZone <> model.current.timeZone ||
                model.origin.useDateTimeFromNtp <> model.current.useDateTimeFromNtp ||
                model.origin.daylightSavings <> model.current.daylightSavings
//            let ntp_changed = 
//                model.origin.useNtpFromDhcp <> model.current.useNtpFromDhcp ||
//                model.origin.ntp <> model.current.ntp
            
            if dateTime_changed then
                let timeZone = new TimeZone(
                    TZ = model.current.timeZone
                )

                if model.current.useDateTimeFromNtp then
                    do! session.SetSystemDateAndTime(SetDateTimeType.NTP, model.current.daylightSavings, timeZone, null)
                else
                    if model.origin.utcDateTime <> model.current.utcDateTime then
                        let dateTime = new DateTime()
                        dateTime.Date <- new Date(
                            Year = model.current.utcDateTime.Year,
                            Month = model.current.utcDateTime.Month,
                            Day = model.current.utcDateTime.Day
                        )
                        dateTime.Time <- new Time(
                            Hour = model.current.utcDateTime.Hour,
                            Minute = model.current.utcDateTime.Minute,
                            Second = model.current.utcDateTime.Second
                        )
                        do! session.SetSystemDateAndTime(SetDateTimeType.Manual, model.current.daylightSavings, timeZone, dateTime)
                    else
                        let! sysDateTime = session.GetSystemDateAndTime()
                        let dateTime = sysDateTime.UTCDateTime
                        do! session.SetSystemDateAndTime(SetDateTimeType.Manual, model.current.daylightSavings, timeZone, dateTime)

//            if ntp_changed then
//                do! session.SetNTP(model.current.useNtpFromDhcp, model.current.ntp.Split([|';'|], StringSplitOptions.RemoveEmptyEntries).Select(fun x -> NetHostFromString(x)).ToArray())
            
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
                        apply = (fun model-> 
                            if model.isModified then 
                                this.Apply(model)
                            else 
                                this.Main()
                        ),
                        close = (fun ()->this.Complete())
                    )
                with err -> 
                    dbg.Error(err)
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        ///<summary></summary>
        member private this.Apply(model) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        return! apply_changes(model)
                    }
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
