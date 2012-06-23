//module VideoSettingsActivity
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
    //open odm.models
    open utils.fsharp
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    type VideoPlayerActivity(ctx:IUnityContainer, model: VideoPlayerActivityModel) = class
        do if model=null then raise(new ArgumentNullException("model"))
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        member private this.Main() = async{
            let! cont = async{
                try
                    let! profile = session.GetProfile(model.profileToken)
                    if profile.VideoEncoderConfiguration = null then
                        failwith "the profile has no video encoder configuration"
                    let! mediaUri = session.GetStreamUri(model.streamSetup, model.profileToken)
                    let viewModel = new VideoPlayerView.Model(
                        profileToken = model.profileToken,
                        streamSetup = model.streamSetup,
                        mediaUri = mediaUri,
                        encoderResolution = profile.VideoEncoderConfiguration.Resolution,
                        isUriEnabled = model.showStreamUrl
                    )
                    do! VideoPlayerView.Show(ctx, viewModel) |> Async.Ignore
                    return async{return ()}
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx, model: VideoPlayerActivityModel) = 
            let act = new VideoPlayerActivity(ctx, model)
            act.Main()

        static member Create() = {
            new IVideoPlayerActivity with
                member this.Run(ctx:IUnityContainer, model: VideoPlayerActivityModel) =
                    VideoPlayerActivity.Run(ctx, model)
            end
        }
        
    end
