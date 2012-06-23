module odm.ui.activities.ProfileDescription
    open System
    open System.Linq
    open System.Collections.ObjectModel
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    open System.Collections.Generic
    //open System.Disposables
    
    //open Microsoft.Practices.Unity
    open onvif.services
    open onvif.utils

    open odm.core
    open odm.infra
    open utils
    //open utils.extensions
    //open odm.models
    open utils.fsharp

    ///<summary></summary>
    let CreateProp(name:string, value:obj, childs:ItemSelectorView.ItemProp[]) = 
        let valStr = 
            if value<> null then value.ToString()
            else null
        new ItemSelectorView.ItemProp(name, valStr, childs)
    
    ///<summary></summary>
    let CreateSimpleProp(name, value:obj) = 
        CreateProp(name, value, null)        

    ///<summary></summary>
    let GetMulticastDetails(mc:MulticastConfiguration) = Seq.toList(seq{
            if mc.Address<>null then
                if mc.Address.Type = IPType.IPv4 then
                    yield CreateProp("ip address", mc.Address.IPv4Address, null)
                else
                    yield CreateProp("ip address", mc.Address.IPv6Address, null)                         
            yield CreateProp("port", mc.Port, null)
            yield CreateProp("TTL", mc.TTL, null)
            yield CreateProp("auto start", mc.AutoStart, null)
    })

    ///<summary></summary>
    let GetPtzDetails(ptz:PTZConfiguration, nodes:seq<PTZNode>) = Seq.toList(seq{
        yield CreateProp("name", ptz.Name, null)
        yield CreateProp("token", ptz.token, null)
        yield CreateProp("use count", ptz.UseCount, null)

        if ptz.PanTiltLimits<>null && ptz.PanTiltLimits.Range<> null && ptz.PanTiltLimits.Range.XRange<>null then
            yield CreateProp("pan tilt limits", ptz.PanTiltLimits.Range.XRange, null)
        if ptz.ZoomLimits<>null && ptz.ZoomLimits.Range<> null && ptz.ZoomLimits.Range.XRange<>null then
            yield CreateProp("zoom limits", ptz.ZoomLimits.Range.XRange, null)
                                                                                    
        yield CreateProp("default ptz speed", ptz.DefaultPTZSpeed, null)
        yield CreateProp("default ptz timeout", ptz.DefaultPTZTimeout, null)
        yield CreateProp("default absolute pant tilt position space", ptz.DefaultAbsolutePantTiltPositionSpace, null)
        yield CreateProp("default absolute zoom position space", ptz.DefaultAbsoluteZoomPositionSpace, null)
        yield CreateProp("default continuous pant tilt velocity space", ptz.DefaultContinuousPanTiltVelocitySpace, null)
        yield CreateProp("default continuous zoom velocity space", ptz.DefaultContinuousZoomVelocitySpace, null)
        yield CreateProp("default relative pant tilt translation space", ptz.DefaultRelativePanTiltTranslationSpace, null)
        yield CreateProp("default relative zoom translation space", ptz.DefaultRelativeZoomTranslationSpace, null)

        let node = 
            if nodes<>null then
                nodes |> Seq.tryFind(fun x->x.token = ptz.NodeToken)
            else
                None
        match node with
        |Some node->
            let childs = Seq.toList(seq{
                yield CreateProp("name", node.Name, null)
                yield CreateProp("token", node.token, null)
                yield CreateProp("home supported", node.HomeSupported, null)
                yield CreateProp("maximum number of presets", node.MaximumNumberOfPresets, null)
                let auxCommands = 
                    if node.AuxiliaryCommands=null then [||]
                    else node.AuxiliaryCommands                     
                yield CreateProp("auxiliary commands", String.Join("; ", auxCommands), null)
                yield CreateProp("supported ptz spaces", node.SupportedPTZSpaces, null)
            })
            yield CreateProp("Node", node.Name, childs |> List.toArray)
        |None ->
            yield CreateProp("node token", ptz.NodeToken, null)
    })

    ///<summary></summary>
    let GetVideoSourceDetails(vsrc:VideoSource) = Seq.toList(seq{
        yield CreateProp("token", vsrc.token, null)
        yield CreateProp("framerate", vsrc.Framerate, null)
        yield CreateProp("resolution", vsrc.Resolution, null)
        if vsrc.Imaging <> null then
            let img = vsrc.Imaging
            let childs = Seq.toList(seq{
                if img.BrightnessSpecified then
                    yield CreateProp("brightness", img.Brightness, null)
                if img.ColorSaturationSpecified then
                    yield CreateProp("color saturation", img.ColorSaturation, null)
                if img.ContrastSpecified then
                    yield CreateProp("contrast", img.Contrast, null)
                if img.SharpnessSpecified then
                    yield CreateProp("sharpness", img.Sharpness, null)
                if img.Exposure <> null then
                    yield CreateProp("exposure", img.Exposure, null)
                if img.Focus <> null then
                    yield CreateProp("focus", img.Focus, null)
                if img.IrCutFilterSpecified then
                    yield CreateProp("IrCut filter", img.IrCutFilter, null)
                if img.BacklightCompensation <> null then
                    let level = img.BacklightCompensation.Level
                    let mode = img.BacklightCompensation.Mode
                    let value = sprintf "level=%g, mode=%A" level mode
                    yield CreateProp("backlight compensation", value, null)
                if img.WhiteBalance<>null then
                    let cbGain = img.WhiteBalance.CbGain
                    let crGain = img.WhiteBalance.CrGain
                    let mode = img.WhiteBalance.Mode
                    let value = sprintf "CbGain=%g, CrGain=%g, mode=%A" cbGain crGain mode
                    yield CreateProp("white balance", value, null)
                if img.WideDynamicRange <> null then
                    let level = img.WideDynamicRange.Level
                    let mode = img.WhiteBalance.Mode
                    let value = sprintf "level=%g, mode=%A" level mode
                    yield CreateProp("wide dynamic range", value, null)
            })
            yield CreateProp("Imaging", null, childs |> List.toArray)
    })

    ///<summary></summary>
    let GetAudioSourceDetails(asrc:AudioSource) = Seq.toList(seq{
        yield CreateProp("token", asrc.token, null)
        yield CreateProp("channels", asrc.Channels, null)
    })
        
    ///<summary></summary>
    let GetVscDetails(vsc:VideoSourceConfiguration, videoSrcs:seq<VideoSource>) = Seq.toList(seq{
        yield CreateProp("name", vsc.Name, null)
        yield CreateProp("token", vsc.token, null)
        yield CreateProp("use count", vsc.UseCount, null)
        yield CreateProp("bounds", vsc.Bounds.ToString(), null)
        let vs = 
            if videoSrcs<>null then
                videoSrcs |> Seq.tryFind(fun vs->vs.token = vsc.SourceToken)
            else
                None
        match vs with
        | Some vs ->
            let childs = GetVideoSourceDetails(vs)
            yield  CreateProp("Video Source", vs.token, childs |> List.toArray)
        | None ->
            yield CreateProp("video source token", vsc.SourceToken, null)
    })

    ///<summary></summary>
    let GetAscDetails(asc:AudioSourceConfiguration, audioSrcs:seq<AudioSource>) = Seq.toList(seq{
        yield CreateProp("name", asc.Name, null)
        yield CreateProp("token", asc.token, null)
        yield CreateProp("use count", asc.UseCount, null)
        let audioSrc = 
            if audioSrcs<>null then
                audioSrcs |> Seq.tryFind(fun x->x.token = asc.SourceToken)
            else
                None
        match audioSrc with
        | Some x ->
            let childs = GetAudioSourceDetails(x)
            yield  CreateProp("Audio Source", x.token, childs |> List.toArray)
        | None ->
            yield CreateProp("audio source token", asc.SourceToken, null)
    })

    ///<summary></summary>
    let GetVecDetails(vec:VideoEncoderConfiguration) = Seq.toList(seq{
        yield CreateProp("name", vec.Name, null)
        yield CreateProp("token", vec.token, null)
        yield CreateProp("use count", vec.UseCount, null)
        yield CreateProp("encoding", vec.Encoding, null)
        yield CreateProp("resolution", vec.Resolution, null)
        yield CreateProp("session timeout", vec.SessionTimeout, null)
        yield CreateProp("quality", vec.Quality, null)
        if vec.RateControl <> null then
            yield CreateProp("frame rate", vec.RateControl.FrameRateLimit, null)
            yield CreateProp("bitrate", vec.RateControl.BitrateLimit, null)
            yield CreateProp("encoding interval", vec.RateControl.EncodingInterval, null)                
    })

    ///<summary></summary>
    let GetAecDetails(aec:AudioEncoderConfiguration) = Seq.toList(seq{
        yield CreateProp("name", aec.Name, null)
        yield CreateProp("token", aec.token, null)
        yield CreateProp("use count", aec.UseCount, null)
        yield CreateProp("encoding", aec.Encoding, null)
        yield CreateProp("bitrate", aec.Bitrate, null)
        yield CreateProp("sample rate", aec.SampleRate, null)
        yield CreateProp("session timeout", aec.SessionTimeout, null)
        if aec.Multicast <> null then
            let mc = aec.Multicast
            let childs = GetMulticastDetails(mc)
            yield CreateProp("multicast", null, childs |> List.toArray)
    })

    ///<summary></summary>
    let GetVacDetails(vac:VideoAnalyticsConfiguration) = Seq.toList(seq{
        yield CreateProp("name", vac.Name, null)
        yield CreateProp("token", vac.token, null)
        yield CreateProp("use count", vac.UseCount, null)
        if vac.AnalyticsEngineConfiguration <> null then
            let anEng = vac.AnalyticsEngineConfiguration
            if anEng.AnalyticsModule<>null then
                for m in anEng.AnalyticsModule do
                    yield CreateProp("name", m.Name, null)
        if vac.RuleEngineConfiguration <> null then
            let ruleEng = vac.RuleEngineConfiguration
            if ruleEng.Rule<>null then
                for r in ruleEng.Rule do
                    yield CreateProp("name", r.Name, null)
    })

    ///<summary></summary>
    let GetMetaDetails(meta:MetadataConfiguration) = Seq.toList(seq{
        yield CreateProp("name", meta.Name, null)
        yield CreateProp("token", meta.token, null)
        yield CreateProp("use count", meta.UseCount, null)
        yield CreateProp("session timeout", meta.SessionTimeout, null)
        if meta.AnalyticsSpecified then
            yield CreateProp("analytics", meta.Analytics, null)
            
        if meta.PTZStatus<>null then
            let ptzStatus = meta.PTZStatus
            yield CreateProp("ptz position", ptzStatus.Position, null)
            yield CreateProp("ptz status", ptzStatus.Status, null)
            
        if meta.Events <>null then
            let events = meta.Events
            yield CreateProp("events filter", events.Filter, null)
            yield CreateProp("events subscription policy", events.SubscriptionPolicy, null)
                
        if meta.Multicast <> null then
            let mc = meta.Multicast
            let childs = GetMulticastDetails(mc)
            yield CreateProp("multicast", null, childs |> List.toArray)
            
    })

    ///<summary></summary>
    let GetProfileDetails(profile:Profile, videoSources:seq<VideoSource>, audioSources:seq<AudioSource>, ptzNodes:seq<PTZNode>) = Seq.toList(seq{
        yield CreateProp("name", profile.Name, null)
        yield CreateProp("token", profile.token, null)
        if profile.fixedSpecified then
            yield CreateProp("fixed", profile.``fixed``, null)

        if profile.VideoSourceConfiguration <> null then
            let vsc = profile.VideoSourceConfiguration
            let childs = GetVscDetails(vsc, videoSources)
            yield  CreateProp("Video Source Configuration", vsc.token, childs |> List.toArray)

        if profile.AudioSourceConfiguration <> null then
            let asc = profile.AudioSourceConfiguration
            let childs = GetAscDetails(asc, audioSources)
            yield  CreateProp("Audio Source Configuration", asc.token, childs |> List.toArray)

        if profile.VideoEncoderConfiguration <> null then
            let vec = profile.VideoEncoderConfiguration
            let childs = GetVecDetails(vec)
            yield CreateProp("Video Encoder Configuration", vec.ToString(), childs |> List.toArray)

        if profile.AudioEncoderConfiguration <> null then
            let aec = profile.AudioEncoderConfiguration
            let childs = GetAecDetails(aec)
            yield CreateProp("Audio Encoder Configuration", aec.ToString(), childs |> List.toArray)

        if profile.PTZConfiguration <> null then
            let ptz = profile.PTZConfiguration
            let childs = GetPtzDetails(ptz, ptzNodes)
            yield  CreateProp("PTZ Configuration", ptz.ToString(), childs |> List.toArray)

        if profile.VideoAnalyticsConfiguration <> null then
            let vac = profile.VideoAnalyticsConfiguration
            let childs = GetVacDetails(vac)
            yield  CreateProp("Video Analytics Configuration", vac.ToString(), childs |> List.toArray)

        if profile.MetadataConfiguration <> null then
            let meta = profile.MetadataConfiguration
            let childs = GetMetaDetails(meta)
            yield  CreateProp("Metadata Configuration", meta.ToString(), childs |> List.toArray)
    })