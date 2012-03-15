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
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    type VideoSettingsActivity(ctx:IUnityContainer, profToken:string) = class
        do if profToken=null then raise( new ArgumentNullException("profToken") )
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            //let origin = model.origin

            let! profile = session.GetProfile(profToken)
            //let! profiles = session.GetProfiles()
            //let profile = profiles |> Seq.find (fun p-> p.token = profileToken)
            
            //TODO: show modal dialog to chose VSC, in case if the profile doesn't have one
            //TODO: show modal dialog to chose VEC, in case if the profile doesn't have one
            
            let vec  = profile.VideoEncoderConfiguration
            let! options = session.GetVideoEncoderConfigurationOptions(vec.token, profile.token)
            
            let resolution = vec.Resolution
            let framerate = 
                if vec.RateControl <> null then
                    vec.RateControl.FrameRateLimit
                else
                    -1
            let encodingInterval = 
                if vec.RateControl <> null then
                    vec.RateControl.EncodingInterval
                else
                    -1
            let bitrate = 
                if vec.RateControl <> null then
                    vec.RateControl.BitrateLimit
                else
                    -1

            let resolutions = Set.ofSeq (seq{
                yield vec.Resolution
                if options.H264 <> null then
                    yield! options.H264.ResolutionsAvailable
                if options.JPEG <> null then
                    yield! options.JPEG.ResolutionsAvailable
                if options.MPEG4 <> null then
                    yield! options.MPEG4.ResolutionsAvailable
            })
            
            let encoders = Set.ofSeq (seq{
                yield vec.Encoding
                if options.H264 <> null then
                    yield VideoEncoding.H264
                if options.JPEG <> null then
                    yield VideoEncoding.JPEG
                if options.MPEG4 <> null then
                    yield VideoEncoding.MPEG4
            })

            let frameRateRanges = Seq.toList(seq{
                if options.H264 <> null then
                    yield options.H264.FrameRateRange
                if options.JPEG <> null then
                    yield options.JPEG.FrameRateRange
                if options.MPEG4 <> null then
                    yield options.MPEG4.FrameRateRange
            })

            let encIntervalRanges = Seq.toList(seq{
                if options.H264 <> null then
                    yield options.H264.EncodingIntervalRange
                if options.JPEG <> null then
                    yield options.JPEG.EncodingIntervalRange
                if options.MPEG4 <> null then
                    yield options.MPEG4.EncodingIntervalRange
            })
            
            let govLengthRanges = Seq.toList(seq{
                if options.H264 <> null then
                    yield options.H264.GovLengthRange
                if options.MPEG4 <> null then
                    yield options.MPEG4.GovLengthRange
            })
            let govLength = 
                if vec.Encoding = VideoEncoding.H264 && vec.H264<>null then
                    vec.H264.GovLength
                elif vec.Encoding = VideoEncoding.MPEG4 && vec.MPEG4 <>null then
                    vec.MPEG4.GovLength
                else
                    -1

            let bitrateRanges = Seq.toList(seq{
                if options.Extension<>null && options.Extension.Any<>null then
                    let tt = @"http://www.onvif.org/ver10/schema"
                    for x in options.Extension.Any |> Seq.filter (fun x->x.NamespaceURI = tt) do
                        if x.Name = @"JPEG" then
                            yield x.Deserialize<JpegOptions2>().BitrateRange
                        elif x.Name = @"MPEG4" then
                            yield x.Deserialize<Mpeg4Options2>().BitrateRange
                        elif x.Name = @"H264" then
                            yield x.Deserialize<H264Options2>().BitrateRange
            })
            
            let quality = vec.Quality
            let qualityRange = 
                if options.QualityRange<>null then
                    options.QualityRange
                else
                    new IntRange(Min = -1, Max= -1)
            
            let (minFrameRate, maxFrameRate) = 
                if frameRateRanges.Length > 0 then
                    let min = frameRateRanges |> Seq.map (fun x->x.Min) |> Seq.min
                    let max = frameRateRanges |> Seq.map (fun x->x.Max) |> Seq.max
                    (min, max)
                else
                    (framerate, framerate)

            let (minEncodingInterval, maxEncodingInterval) = 
                if encIntervalRanges.Length > 0 then
                    let min = encIntervalRanges |> Seq.map (fun x->x.Min) |> Seq.min
                    let max = encIntervalRanges |> Seq.map (fun x->x.Max) |> Seq.max
                    (min, max)
                else
                    (encodingInterval, encodingInterval)

            let (minBitrate, maxBitrate) = 
                if bitrateRanges.Length > 0 then
                    let min = bitrateRanges |> Seq.map (fun x->x.Min) |> Seq.min
                    let max = bitrateRanges |> Seq.map (fun x->x.Max) |> Seq.max
                    (min, max)
                else
                    (0, Int32.MaxValue)

            let (minGovLength, maxGovLength) = 
                if govLengthRanges.Length > 0 then
                    let min = govLengthRanges |> Seq.map (fun x->x.Min) |> Seq.min
                    let max = govLengthRanges |> Seq.map (fun x->x.Max) |> Seq.max
                    (min, max)
                else
                    (govLength, govLength)
            
            let model = new VideoSettingsView.Model(
                minQuality = qualityRange.Min,
                maxQuality = qualityRange.Max,
                minBitrate = minBitrate,
                maxBitrate = maxBitrate,
                minEncodingInterval = minEncodingInterval,
                maxEncodingInterval = maxEncodingInterval,
                minFrameRate = minFrameRate,
                maxFrameRate = maxFrameRate,
                minGovLength = minGovLength,
                maxGovLength = maxGovLength,
                encoders = (encoders |> Set.toArray),
                resolutions = (resolutions |> Set.toArray),
                encoderOptions = options,
                profToken = profToken
            )
            
            model.encoder <- vec.Encoding
            model.resolution <- resolution
            model.frameRate <- float(framerate)
            model.govLength <- govLength
            model.encodingInterval <- encodingInterval
            model.quality <- quality
            model.bitrate <- float(bitrate)

            model.AcceptChanges()
            return model
        }

        let apply_changes(model:VideoSettingsView.Model) = async{
            
            //let! profiles = session.GetProfiles()
            //let profile = profiles |> Seq.find (fun p-> p.token = profToken)
            let! profile = session.GetProfile(profToken)
            let vec = profile.VideoEncoderConfiguration
//            
//            do! session.RemoveVideoEncoderConfiguration(profile.token)
//            profile.VideoEncoderConfiguration <- null

            //let! vecs = session.GetCompatibleVideoEncoderConfigurations(profile.token)
            
            let! options = session.GetVideoEncoderConfigurationOptions(vec.token, null)
            let qualityMin = float32(options.QualityRange.Min)
            let qualityMax = float32(options.QualityRange.Max)
            //let quality = Math.Min(qualityMax, Math.Max(model.quality, qualityMin))
            let quality = model.quality |> Math.Coerce qualityMin qualityMax
            
            vec.Encoding <- model.encoder
            vec.Quality <- quality
            vec.Resolution <- model.resolution
                
            let inline CoerceGovLength (options:^TOpt) = 
                let range = (^TOpt: (member GovLengthRange:IntRange)(options))
                if range <> null then
                    Math.Coerce (range.Min) (range.Max)
                else
                    (fun(v)->v)

            let inline tryConfig(opts:^TOpt) = 
                if options<>null then
                    let resolutions = (^TOpt: (member ResolutionsAvailable:VideoResolution[])(opts))
                    if resolutions |> Array.exists (fun x->x=model.resolution) then
                        if vec.RateControl = null then
                            vec.RateControl <- new VideoRateControl()
                        let frameRateRange = (^TOpt: (member FrameRateRange:IntRange)(opts))
                        let frameRateMin = frameRateRange.Min
                        let frameRateMax = frameRateRange.Max
                        let frameRate = int(model.frameRate) |> Math.Coerce frameRateMin frameRateMax 
                        vec.RateControl.FrameRateLimit <- frameRate
                        vec.RateControl.BitrateLimit <- int(model.bitrate)

                        let encodingIntervalRange = (^TOpt: (member EncodingIntervalRange:IntRange)(opts))
                        let encodingIntervalMin = encodingIntervalRange.Min
                        let encodingIntervalMax = encodingIntervalRange.Max
                        let encodingInterval = model.encodingInterval |> Math.Coerce encodingIntervalMin encodingIntervalMax 
                        vec.RateControl.EncodingInterval <- encodingInterval
                        if model.encoder = VideoEncoding.H264 then
                            if vec.H264 = null then vec.H264 <- new H264Configuration()
                            vec.H264.GovLength <- model.govLength |> CoerceGovLength(options.H264)
                        elif model.encoder = VideoEncoding.MPEG4 then
                            if vec.MPEG4 = null then vec.MPEG4 <- new Mpeg4Configuration()
                            vec.MPEG4.GovLength <- model.govLength |> CoerceGovLength(options.MPEG4)
                        true
                    else
                        false
                else
                    false
            

            let isVecConfigured =
                match model.encoder with
                |VideoEncoding.H264 -> tryConfig(options.H264)
                |VideoEncoding.JPEG -> tryConfig(options.JPEG)
                |VideoEncoding.MPEG4 -> tryConfig(options.MPEG4)
                |_ -> raise (new ArgumentException(LocalVideoSettings.instance.errorEncoder))
            
            if isVecConfigured then 
                do! session.SetVideoEncoderConfiguration(vec, true)
                model.AcceptChanges()

            return isVecConfigured
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
                    let! res = VideoSettingsView.Show(ctx, model)
                    return res.Handle(
                        apply = (fun model-> 
                            if model.isModified then 
                                this.Apply(model) 
                            else 
                                this.Main()
                        ),
                        none = (fun ()-> this.Complete())
                    )
                with err -> 
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.Apply(model) = async{
            let! cont = async{
                try
                    let! vecWasConfigured = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        return! apply_changes(model)
                    }
                    if vecWasConfigured then
                        return this.Main()
                    else
                        do! show_error(new Exception(LocalVideoSettings.instance.errorSupportResolution))
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

        static member Run(ctx, profileToken) = 
            let act = new VideoSettingsActivity(ctx,profileToken)
            act.Main()
    end
