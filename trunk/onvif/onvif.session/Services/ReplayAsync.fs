namespace odm.onvif

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Text
    open System.Xml
    open System.ServiceModel

    open onvif.services
    open onvif10_replay
    open onvif
    
    open utils.fsharp

    [<AllowNullLiteral>]
    type IReplayAsync = interface
        //onvif 2.1
        abstract GetServiceCapabilities: request:GetServiceCapabilitiesRequest -> Async<GetServiceCapabilitiesResponse>
        abstract GetReplayUri: request:GetReplayUriRequest -> Async<GetReplayUriResponse>
        abstract GetReplayConfiguration: request:GetReplayConfigurationRequest -> Async<GetReplayConfigurationResponse>
        abstract SetReplayConfiguration: request:SetReplayConfigurationRequest -> Async<SetReplayConfigurationResponse>
    end

    type ReplayAsync(proxy:ReplayPort) = class
        do if proxy |> IsNull then raise <| new ArgumentNullException(paramName = "proxy")

        member this.uri =
            (proxy :?> IClientChannel).RemoteAddress.Uri

        member this.services =
            proxy
        
        interface IReplayAsync with
            member this.GetServiceCapabilities(request:GetServiceCapabilitiesRequest):Async<GetServiceCapabilitiesResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetServiceCapabilities, proxy.EndGetServiceCapabilities)
                return response
            }
            member this.GetReplayUri(request:GetReplayUriRequest):Async<GetReplayUriResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetReplayUri, proxy.EndGetReplayUri)
                return response
            }
            member this.GetReplayConfiguration(request:GetReplayConfigurationRequest):Async<GetReplayConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginGetReplayConfiguration, proxy.EndGetReplayConfiguration)
                return response
            }
            member this.SetReplayConfiguration(request:SetReplayConfigurationRequest):Async<SetReplayConfigurationResponse> = async{
                let! response = Async.FromBeginEnd(request, proxy.BeginSetReplayConfiguration, proxy.EndSetReplayConfiguration)
                return response
            }
        end
    end
