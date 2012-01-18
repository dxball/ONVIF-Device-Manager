namespace odm.ui.activities
    open System
    open System.Xml
    open System.Linq
    //open System.Disposables
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.core
    open odm.ui
    //open odm.infra
    open utils
    open utils.fsharp
    ////open odm.models
    //open utils.fsharp
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    type IdentificationActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err);
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            let! devInfo, scopes, zeroConf, netstat, caps = Async.Parallel(
                session.GetDeviceInformation(),
                session.GetScopes(),
                async{
                    let! zeofConfSupported = facade.IsZeroConfigurationSupported()
                    if zeofConfSupported then
                        return! session.GetZeroConfiguration()
                    else
                        return null
                },
                facade.GetNetworkStatus(),
                session.GetCapabilities()
            )

            let ips = seq{
                if netstat.nics <> null then
                    yield! netstat.nics |> Seq.map (fun nic -> nic.ip.ToString())
                if zeroConf<>null && zeroConf.Addresses <> null then
                    yield! zeroConf.Addresses |> Seq.filter(fun x-> not(String.IsNullOrEmpty(x)))
            }

            let onvifVersion = 
                if caps.Device<>null && caps.Device.System<>null && caps.Device.System.SupportedVersions<> null then
                    caps.Device.System.SupportedVersions.Max()
                else
                    null

            let model = new IdentificationView.Model(
                firmware = devInfo.FirmwareVersion,
                hardware = devInfo.HardwareId,
                serial = devInfo.SerialNumber,
                manufacturer = devInfo.Manufacturer,
                model = devInfo.Model,
                //model.host
                mac  = String.Join(", ", netstat.nics |> Seq.map (fun nic -> nic.mac)),
                ip  = String.Join(", ", ips.Distinct()),
                onvifVersion = onvifVersion
            )
            
            model.origin.name <- ScopeHelper.GetName(scopes |> Seq.map (fun x->x.ScopeItem))
            model.origin.location <- ScopeHelper.GetLocation(scopes |> Seq.map (fun x->x.ScopeItem))
            
            model.RevertChanges()
            return model
        }

        let apply(model:IdentificationView.Model) = async{
            let name_has_changed = model.origin.name <> model.current.name
            let location_has_changed = model.origin.location <> model.current.location
            let is_modified = name_has_changed || location_has_changed
            if not is_modified then return ()
            let! scopes = session.GetScopes()
            
            let scopes_to_set = seq{
                let changed_scopes = Seq.toList (seq{
                    if name_has_changed then
                        let prefix = 
                            let use_onvif_scope = 
                                scopes 
                                    |> Seq.filter (fun x -> x.ScopeDef = ScopeDefinition.Fixed)
                                    |> Seq.exists (fun x -> not(x.ScopeItem.StartsWith(ScopeHelper.onvifNameScope)))
                            if use_onvif_scope then
                                ScopeHelper.onvifNameScope
                            else 
                                ScopeHelper.odmNameScope
                        //let value = String.Concat(prefix, Uri.EscapeDataString(model.current.name))
                        let value = Uri.EscapeUriString(model.current.name)
                        yield (prefix, value)
                    
                    if location_has_changed then
                        let prefix = 
                            let use_onvif_scope = 
                                scopes 
                                    |> Seq.filter (fun x -> x.ScopeDef = ScopeDefinition.Fixed)
                                    |> Seq.forall (fun x -> not(x.ScopeItem.StartsWith(ScopeHelper.onvifLocationScope)))
                            if use_onvif_scope then
                                ScopeHelper.onvifLocationScope
                            else 
                                ScopeHelper.odmLocationScope
                        //let value = String.Concat(prefix, Uri.EscapeDataString(model.current.location))
                        let values = seq{
                            if model.location <> null then
                                for x in model.location.Split([|';'|]) do
                                    let v = x.Trim()
                                    if not(String.IsNullOrEmpty(v)) then
                                        yield Uri.EscapeUriString(v)
                        }
                        for value in values do
                            yield (prefix, value)
                })
                
                let filter (x:string) = changed_scopes |> Seq.forall (fun (prefix, value) -> not(x.StartsWith(prefix)))
                
                yield! scopes
                    |> Seq.filter (fun x -> x.ScopeDef = ScopeDefinition.Configurable)
                    |> Seq.map (fun x -> x.ScopeItem)
                    |> Seq.filter filter
                
                yield! changed_scopes |> Seq.map (fun (prefix, value) -> String.Concat(prefix, value))
            }
            do! session.SetScopes(scopes_to_set.ToArray())
        }

        member private this.Main() = async{
            let! cont = async{
                try
                    let! model = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! load()
                    }
                    return this.ShowForm(model)
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = IdentificationView.Show(ctx, model)
                    return res.Handle(
                        apply = (fun model->this.ApplyChanges(model)),
                        close = (fun ()->this.Complete())
                    )
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.ApplyChanges(model) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        return! apply(model)
                    }
                    return this.Main()
                with err ->
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx) = 
            let act = new IdentificationActivity(ctx)
            act.Main()
    end

