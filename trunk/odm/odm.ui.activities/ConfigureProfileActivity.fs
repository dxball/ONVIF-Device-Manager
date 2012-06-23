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
    ////open odm.models
    open utils.fsharp
    open ProfileDescription
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs

    type internal SelectItemModel<'T when 'T:equality>(items, selectedEntity:option<'T>) = class
        let selectedItem = 
            match selectedEntity with
            |Some entity->
                match items |> List.tryFind(fun(e,i)->e=entity) with
                |Some (e,i) -> i
                |None -> null
            |None->null
        
        new (items, selectedEntity:'T) = SelectItemModel<'T> (items, Some selectedEntity)
        new (items) = SelectItemModel<'T> (items, None)

        member this.items:list<'T*ItemSelectorView.Item> = items
        member this.selection:ItemSelectorView.Item = selectedItem
        member this.GetEntityFromItem(item:ItemSelectorView.Item) = 
            let entity = items |> List.tryFind(fun(e, i) -> i=item)
            match entity with
            | Some (e,i) -> Some e
            | None -> None
        member this.GetEntities() = 
            items |> List.map(fun(e, i)->e) |> List.toArray
        member this.GetItems() = 
            items |> List.map(fun(e, i)->i) |> List.toArray
    end

    type ConfigureProfileActivity(ctx:IUnityContainer, profile:Profile) = class
        do if profile=null then raise( new ArgumentNullException("profile") )
        let session = ctx.Resolve<INvtSession>()
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            let! caps = session.GetCapabilities()
            let! vecs, aecs, vacs, metcs, ptzcs = Async.Parallel(
                async{
                    if profile.VideoSourceConfiguration = null then
                        return [||]
                    else
                        let! vecs = session.GetCompatibleVideoEncoderConfigurations(profile.token)
                        if vecs<> null then 
                            return vecs
                        else 
                            return [||]
                },
                async{
                    if profile.AudioSourceConfiguration = null then
                        return [||]
                    else
                        let! aecs = session.GetCompatibleAudioEncoderConfigurations(profile.token)
                        if aecs<> null then 
                            return aecs
                        else 
                            return [||]
                },
                async{
                    if caps.Analytics=null || caps.Analytics.XAddr=null then
                        return [||]
                    else
                        let! vacs = session.GetCompatibleVideoAnalyticsConfigurations(profile.token)
                        if vacs<> null then 
                            return vacs
                        else 
                            return [||]
                },
                async{
                    let! metcs = session.GetCompatibleMetadataConfigurations(profile.token)
                    if metcs<> null then 
                        return metcs
                    else 
                        return [||]
                },
                async{
                    if caps.PTZ = null || caps.PTZ.XAddr = null then
                        return [||]
                    else
                        let ptz = session :> IPtzAsync 
                        let! ptzcs = ptz.GetConfigurations()
                        if ptzcs<> null then 
                            return ptzcs
                        else 
                            return [||]
                }
            )
            
            let model = new ProfileUpdatingView.Model(
                videoEncCfgs = vecs,
                audioEncCfgs = aecs,
                analyticsCfgs = vacs,
                metaCfgs = metcs,
                ptzCfgs = ptzcs
            )
            //model.videoEncCfgs <- vecs
            model.isVideoEncCfgEnabled <- profile.VideoEncoderConfiguration <> null
            model.videoEncCfg <- 
                if profile.VideoEncoderConfiguration <> null then
                    let cfgToken = profile.VideoEncoderConfiguration.token
                    match vecs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif vecs.Length > 0 then
                    vecs.[0]
                else
                    null
            
            //model.audioEncCfgs <- aecs
            model.isAudioEncCfgEnabled <- profile.AudioEncoderConfiguration <> null
            model.audioEncCfg <- 
                if profile.AudioEncoderConfiguration <> null then
                    let cfgToken = profile.AudioEncoderConfiguration.token
                    match aecs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif aecs.Length > 0 then
                    aecs.[0]
                else
                    null
            
            //model.analyticsCfgs <- vacs
            model.isAnalyticsCfgEnabled <- profile.VideoAnalyticsConfiguration <> null
            model.analyticsCfg <- 
                if profile.VideoAnalyticsConfiguration <> null then
                    let cfgToken = profile.VideoAnalyticsConfiguration.token
                    match vacs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif vacs.Length > 0 then
                    vacs.[0]
                else
                    null
            
            //model.metaCfgs <- metcs
            model.isMetaCfgEnabled <- profile.MetadataConfiguration <> null
            model.metaCfg <- 
                if profile.MetadataConfiguration <> null then
                    let cfgToken = profile.MetadataConfiguration.token
                    match metcs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif metcs.Length > 0 then
                    metcs.[0]
                else
                    null
            
            //model.ptzCfgs <- ptzcs
            model.isPtzCfgEnabled <- profile.PTZConfiguration <> null
            model.ptzCfg <- 
                if profile.PTZConfiguration <> null then
                    let cfgToken = profile.PTZConfiguration.token
                    match ptzcs |> Seq.tryFind (fun x -> x.token = cfgToken) with
                    | None -> null
                    | Some x -> x
                elif ptzcs.Length > 0 then
                    ptzcs.[0]
                else
                    null
            
            model.AcceptChanges();
            return model
        }

        let configure(model:ProfileUpdatingView.Model) = async{

            if model.isVideoEncCfgEnabled && model.videoEncCfg <> null then
                do! session.AddVideoEncoderConfiguration(profile.token, model.videoEncCfg.token)
                profile.VideoEncoderConfiguration <- model.videoEncCfg
            elif not(model.isVideoEncCfgEnabled) && profile.VideoEncoderConfiguration <> null then
                do! session.RemoveVideoEncoderConfiguration(profile.token)
                profile.VideoEncoderConfiguration <- null
            
            if model.isAudioEncCfgEnabled && model.audioEncCfg <> null then
                do! session.AddAudioEncoderConfiguration(profile.token, model.audioEncCfg.token)
                profile.AudioEncoderConfiguration <- model.audioEncCfg
            elif not(model.isAudioEncCfgEnabled) && profile.AudioEncoderConfiguration <> null then
                do! session.RemoveAudioEncoderConfiguration(profile.token)
                profile.AudioEncoderConfiguration <- null
            
            if model.isAnalyticsCfgEnabled && model.analyticsCfg <> null then
                do! session.AddVideoAnalyticsConfiguration(profile.token, model.analyticsCfg.token)
                profile.VideoAnalyticsConfiguration <- model.analyticsCfg
            elif not(model.isAnalyticsCfgEnabled) && profile.VideoAnalyticsConfiguration <> null then
                do! session.RemoveVideoAnalyticsConfiguration(profile.token)
                profile.VideoAnalyticsConfiguration <- null
            
            if model.isMetaCfgEnabled && model.metaCfg <> null then
                do! session.AddMetadataConfiguration(profile.token, model.metaCfg.token)
                profile.MetadataConfiguration <- model.metaCfg
            elif not(model.isMetaCfgEnabled) && profile.MetadataConfiguration <> null then
                do! session.RemoveMetadataConfiguration(profile.token)
                profile.MetadataConfiguration <- null
            
            if model.isPtzCfgEnabled && model.ptzCfg <> null then
                do! session.AddPTZConfiguration(profile.token, model.ptzCfg.token)
                profile.PTZConfiguration <- model.ptzCfg
            elif not(model.isPtzCfgEnabled) && profile.PTZConfiguration <> null then
                do! session.RemovePTZConfiguration(profile.token)
                profile.PTZConfiguration <- null 
            
            model.AcceptChanges()
            return ()
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
                    return this.Complete(true)
            }
            return! cont
        }

        ///<summary></summary>
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = ProfileUpdatingView.Show(ctx, model)
                    return res.Handle(
                        abort = (fun ()-> this.Complete(true)),
                        finish = (fun model-> this.Finish(model)),
                        selectVideoEncCfg = (fun model-> this.SelectVec(model)),
                        selectAudioEncCfg = (fun model-> this.SelectAec(model)),
                        selectMetaCfg = (fun model-> this.SelectMeta(model)),
                        selectPtzCfg = (fun model-> this.SelectPtz(model)),
                        selectAnalyticsCfg = (fun model-> this.SelectAnalytics(model))
                    )
                with err -> 
                    do! show_error err
                    return this.Complete(true)
            }
            return! cont
        }

        ///<summary></summary>
        member private this.Finish(model) = async{
            if model.isModified then
                do! async{
                    use! progress = Progress.Show(ctx, LocalDevice.instance.configuring)
                    return! configure(model)
                }
            return! this.Complete(false)
        }
        
        ///<summary></summary>
        member private this.SelectVec(model) = async{
            let! cont = async{
                try
                    let items = Seq.toList(seq{
                        for vec in model.videoEncCfgs do
                            let item = new ItemSelectorView.Item(vec.ToString(), GetVecDetails(vec) |> List.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable)
                            yield (vec, item)
                    })

                    let itemsModel = new SelectItemModel<VideoEncoderConfiguration>(items, model.videoEncCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)

                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some e -> model.videoEncCfg <- e
                        | None -> ()

                    return this.ShowForm(model)
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.SelectAec(model) = async{
            let! cont = async{
                try
                    let items = Seq.toList(seq{
                        for aec in model.audioEncCfgs do
                            let item = new ItemSelectorView.Item(aec.ToString(), GetAecDetails(aec) |> List.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable)
                            yield (aec, item)
                    })
                    let itemsModel = new SelectItemModel<AudioEncoderConfiguration>(items, model.audioEncCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some entity -> model.audioEncCfg <- entity
                        | None -> ()                                
                    return this.ShowForm(model)
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.SelectMeta(model) = async{
            let! cont = async{
                try
                    let items = Seq.toList(seq{
                        for meta in model.metaCfgs do
                            let item = new ItemSelectorView.Item(meta.ToString(), GetMetaDetails(meta) |> List.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable)
                            yield (meta, item)

                    })
                    let itemsModel = new SelectItemModel<MetadataConfiguration>(items, model.metaCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some entity -> model.metaCfg <- entity
                        | None -> ()
                    return this.ShowForm(model)
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        member private this.SelectPtz(model) = async{
            let! cont = async{
                try
                    let! nodes = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! session.GetNodes()
                    }
                    let items = Seq.toList(seq{
                        for ptz in model.ptzCfgs do
                            let item = new ItemSelectorView.Item(ptz.ToString(), GetPtzDetails(ptz, nodes) |> Seq.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable)
                            yield (ptz, item)
                    })
                    let itemsModel = new SelectItemModel<PTZConfiguration>(items, model.ptzCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some entity -> model.ptzCfg <- entity
                        | None -> () 
                    return this.ShowForm(model)
                with err ->
                    do! show_error err
                    return this.ShowForm(model)
            }
            return! cont
        }

        member private this.SelectAnalytics(model) = async{
            let! cont = async{
                try
                    let! nodes = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! session.GetNodes()
                    }
                    let items = Seq.toList(seq{
                        for analytics in model.analyticsCfgs do
                            let details = GetVacDetails(analytics)
                            yield (analytics, new ItemSelectorView.Item(analytics.ToString(), details |> Seq.toArray, ItemSelectorView.ItemFlags.AllOperationsAvailable))
                    })
                    let itemsModel = new SelectItemModel<VideoAnalyticsConfiguration>(items, model.analyticsCfg)
                    let! res = 
                        let viewModel = 
                            ItemSelectorView.Model.Create(
                                items = (itemsModel.GetItems()),
                                selection = (itemsModel.selection),
                                flags = (
                                    ItemSelectorView.Flags.CanClose ||| ItemSelectorView.Flags.CanSelect
                                )
                            )
                        ItemSelectorView.Show(ctx, viewModel)
                    if res.IsSelect() then
                        let item = res.AsSelect().item
                        match itemsModel.GetEntityFromItem(item) with
                        | Some entity -> model.analyticsCfg <- entity
                        | None -> () 
                    return this.ShowForm(model)
                with err ->
                    do! show_error err
                    return this.ShowForm(model)
            }
            return! cont
        }
        member private this.Complete(res) = async{
            return res
        }
        static member Run(ctx, profileToken) = 
            let act = new ConfigureProfileActivity(ctx,profileToken)
            act.Main()
    end

