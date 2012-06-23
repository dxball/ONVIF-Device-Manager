namespace odm.ui.activities
    open System
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
    open odm.infra
    open utils
    //open utils.extensions
    //open odm.models
    open utils.fsharp
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs

    type PtzActivity private(ctx:IUnityContainer, profToken:string) = class
        do if profToken=null then raise( new ArgumentNullException("profToken") )
        let session = ctx.Resolve<INvtSession>()
        let ptz = session :> IPtzAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            
            let! profile, nodes, presets = Async.Parallel(
                session.GetProfile(profToken),
                ptz.GetNodes(),
                ptz.GetPresets(profToken)
            )
            
            let cfg = profile.PTZConfiguration
            
            let! node = 
                if cfg<>null then
                    ptz.GetNode(cfg.NodeToken)
                else
                    async{return null}

            let model = new PtzView.Model(
                profToken = profile.token,
                panMin = -1.0f,
                panMax = 1.0f,
                tiltMin = -1.0f,
                tiltMax = 1.0f,
                zoomMin = 0.0f,
                zoomMax = 1.0f,
                nodes = nodes,
                presets = presets
            )

            model.currentNode <- node

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
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    do! PtzView.Show(ctx, model) |> Async.Ignore
                    return this.Complete()
                with err -> 
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        member private this.Complete(res) = async{
            return res
        }
        
        static member Run(ctx:IUnityContainer, profToken:string) = 
            let act = new PtzActivity(ctx, profToken)
            act.Main()
    end
