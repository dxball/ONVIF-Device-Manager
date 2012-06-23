namespace odm.ui.activities
    open System
    open System.Xml
    open System.Xml.Schema
    open System.Xml.Serialization
    open System.Xml.Linq
    open System.Linq
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Net.Mime
    open System.Windows
    open System.Windows.Threading
    open System.Linq
//    open System.Disposables
//    open System.Concurrency

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


    type AnalyticsActivity(ctx:IUnityContainer, profToken:string) = class
        do if profToken=null then raise( new ArgumentNullException("profile") )
        let session = ctx.Resolve<INvtSession>()
        let van = session :> IAnalyticsAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        let load() = async{
            let! profile, caps = Async.Parallel(
                session.GetProfile(profToken),
                session.GetCapabilities()
            )
            return! async{
                if caps.Analytics<>null && profile.VideoAnalyticsConfiguration <> null then
                    //TODO: do this in parallel

                    let! (modules, moduleTypes, moduleSchemes) = async{
                        if caps.Analytics.AnalyticsModuleSupport then
                            let! modules, moduleTypes = Async.Parallel(
                                van.GetAnalyticsModules(profile.VideoAnalyticsConfiguration.token),
                                van.GetSupportedAnalyticsModules(profile.VideoAnalyticsConfiguration.token)
                            )
                            let! moduleSchemes = facade.DownloadSchemes(moduleTypes.AnalyticsModuleContentSchemaLocation)
                            return (modules, moduleTypes.AnalyticsModuleDescription, moduleSchemes)
                        else
                            return ([||], [||], new XmlSchemaSet())
                    }

                    let! (rules, ruleTypes, ruleSchemes) = async{
                        if caps.Analytics.RuleSupport then
                            let! rules, ruleTypes = Async.Parallel(
                                van.GetRules(profile.VideoAnalyticsConfiguration.token),
                                van.GetSupportedRules(profile.VideoAnalyticsConfiguration.token)
                            )
                            let! ruleSchemes = facade.DownloadSchemes(ruleTypes.RuleContentSchemaLocation)
                            return (rules, ruleTypes.RuleDescription, ruleSchemes)
                        else
                            return ([||], [||], new XmlSchemaSet())
                    }
                    return new AnalyticsView.Model(
                        profile = profile,
                        rules = rules, 
                        ruleTypes = ruleTypes,
                        ruleSchemes = ruleSchemes, 
                        modules = modules,
                        moduleTypes = moduleTypes,
                        moduleSchemes = moduleSchemes
                    )
                else
                    return new AnalyticsView.Model(
                        profile = profile,
                        rules = [||], 
                        ruleTypes = [||],
                        ruleSchemes = new XmlSchemaSet(), 
                        modules = [||],
                        moduleTypes = [||],
                        moduleSchemes = new XmlSchemaSet()
                    )
            }
        }

        ///<summary></summary>
        member private this.Main() = async{
            let! cont = async{
                try
                    return! async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        let! model = load()
                        return this.ShowForm(model)
                    }
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        member private this.ShowForm(model) = async{
            let vac = model.profile.VideoAnalyticsConfiguration
            let! cont = async{
                try
                    let! res = AnalyticsView.Show(ctx, model)
                    return res.Handle(
                        createModule = (fun()  -> 
                            this.CreateModule(model)
                        ),
                        configureModule = (fun moduleCfg -> 
                            this.ConfigModule(model, moduleCfg)
                        ),
                        deleteModule = (fun moduleName -> 
                            this.DeleteModule(model, moduleName)
                        ),
                        createRule = (fun ruleName -> 
                            this.CreateRule(model)
                        ),
                        configureRule = (fun ruleCfg -> 
                            this.ConfigRule(model, ruleCfg)
                        ),
                        deleteRule = (fun ruleName ->
                            this.DeleteRule(model, ruleName)
                        ),
                        close = (fun () ->
                            this.Complete()
                        )
                    )
                with err -> 
                    do! show_error(err)
                    return this.Complete()
            }
            return! cont
        }
        member private this.CreateDefaultConfig(name:string, description:ConfigDescription, schemaSet: XmlSchemaSet) = 
            let cfg = new Config()
            cfg.Name <- name
            cfg.Type <- description.Name
            let simpleItems = Seq.toList(seq{
                if description.Parameters<>null && description.Parameters.SimpleItemDescription<>null then
                    for sid in description.Parameters.SimpleItemDescription do
                        let item = new ItemListSimpleItem()
                        item.Name <- sid.Name
                        item.Value <- 
                            if sid.Type.Namespace = XmlSchema.Namespace then
                                let simpleType = XmlSchemaSimpleType()
                                ProtoSchemeGenerator.CreateProtoXsdType(sid.Type.Name)
                            else
                                let simpleTypes = schemaSet.GlobalTypes.Values.OfType<XmlSchemaSimpleType>()
                                let simpleType = simpleTypes |> Seq.find(fun x->x.QualifiedName =  sid.Type)
                                ProtoSchemeGenerator.CreateProtoSimpleType(simpleType)
                        yield item
            })
            let elementItems = Seq.toList(seq{
                if description.Parameters<>null && description.Parameters.ElementItemDescription<>null then
                    for eid in description.Parameters.ElementItemDescription do
                        let item = new ItemListElementItem()
                        item.Name <- eid.Name
                        let schemaElements = schemaSet.GlobalElements.Values.OfType<XmlSchemaElement>()
                        let schemaElement = schemaElements |> Seq.tryFind(fun t-> t.QualifiedName = eid.Type)
                        item.Any <- 
                            match schemaElement with
                            | Some sel ->
                                ProtoSchemeGenerator.CreateProtoElement(sel).ToXmlElement()
                            |None ->
                                let err = new Exception(String.Format("scheme definition for element {0} is missing", eid.Type))
                                dbg.Error(err)
                                raise err
                        //item.Any <- get_element_default(eid.Type)
                                
                        yield item
            })
            cfg.Parameters <- new ItemList()
            cfg.Parameters.SimpleItem <- simpleItems |> List.toArray
            cfg.Parameters.ElementItem <- elementItems |> List.toArray
            cfg

        member private this.CreateModule(model) = 
            let rec set_name() = 
                async{
                    let! cont = async{
                        try
                            let vm = new AnalyticsSetNameView.Model(
                                types = model.moduleTypes
                            )
                            let! res = AnalyticsSetNameView.Show(ctx, vm)
                            return res.Handle(
                                configure = (fun name description ->
                                    configure(name, description)
                                ),
                                abort = (fun()-> 
                                    this.ShowForm(model)
                                )
                            )
                        with err ->
                            do! show_error(err)
                            return this.ShowForm(model)
                    }
                    return! cont
                } 
            and configure(name, description) = 
                async{
                    let! cont = async{
                        try
                            let vac = model.profile.VideoAnalyticsConfiguration
                            let vm = new ConfigureAnalyticView.Model(
                                config = this.CreateDefaultConfig(name, description, model.moduleSchemes),
                                configDescription = description,
                                profile = model.profile,
                                schemes = model.moduleSchemes
                            )
                            let! res = ConfigureAnalyticView.Show(ctx, vm)
                            return res.Handle(
                                apply = (fun m->
                                    create(m.config)
                                ),
                                abort = (fun()->
                                    this.ShowForm(model)
                                )
                            )
                        with err ->
                            do! show_error(err)
                            return this.ShowForm(model)
                    }
                    return! cont
                }
            and create(config) = 
                async{
                    try
                        let vac = model.profile.VideoAnalyticsConfiguration
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        do! van.CreateAnalyticsModules(vac.token, [|config|])
                    with err ->
                        do! show_error(err)

                    return! this.Main()
                }
                
            set_name()

        member private this.CreateRule(model) = 
            let rec set_name() = 
                async{
                    let! cont = async{
                        try
                            let vm = 
                                new AnalyticsSetNameView.Model(
                                    types = model.ruleTypes
                                )
                            let! res = AnalyticsSetNameView.Show(ctx, vm)
                            return res.Handle(
                                configure = (fun name description ->
                                    configure(name, description)
                                ),
                                abort = (fun()-> 
                                    this.ShowForm(model)
                                )
                            )
                        with err ->
                            do! show_error(err)
                            return this.ShowForm(model)
                    }
                    return! cont
                } 
            and configure(name, description) = 
                async{
                    let! cont = async{
                        try
                            let vac = model.profile.VideoAnalyticsConfiguration
                            let vm = new ConfigureAnalyticView.Model(
                                config = this.CreateDefaultConfig(name, description, model.ruleSchemes),
                                configDescription = description,
                                profile = model.profile,
                                schemes = model.ruleSchemes
                            )
                            let! res = ConfigureAnalyticView.Show(ctx, vm)
                            return res.Handle(
                                apply = (fun m->
                                    create(m.config)
                                ),
                                abort = (fun()->
                                    this.ShowForm(model)
                                )
                            )
                        with err ->
                            do! show_error(err)
                            return this.ShowForm(model)
                    }
                    return! cont
                }
            and create(config) = 
                async{
                    try
                        let vac = model.profile.VideoAnalyticsConfiguration
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        do! van.CreateRules(vac.token, [|config|])
                    with err ->
                        do! show_error(err)

                    return! this.Main()
                }

            set_name()
        member private this.ConfigModule(model, moduleCfg) = async{
            let! cont = async{
                try
                    let vac = model.profile.VideoAnalyticsConfiguration
                    let vm = new ConfigureAnalyticView.Model(
                        config = moduleCfg,
                        configDescription = (
                            model.moduleTypes |> Seq.find(fun x->
                                x.Name = moduleCfg.Type
                            )
                        ),
                        profile = model.profile,
                        schemes = model.moduleSchemes
                    )
                    let! res = ConfigureAnalyticView.Show(ctx, vm)
                    return res.Handle(
                        apply = (fun m->
                            this.ApplyModuleChanges(model, m.config)
                        ),
                        abort = (fun()->
                            this.ShowForm(model)
                        )
                    )
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        member private this.ConfigRule(model, ruleCfg) = async{
            let! cont = async{
                try
                    let vac = model.profile.VideoAnalyticsConfiguration
                    let vm = new ConfigureAnalyticView.Model(
                        config = ruleCfg,
                        configDescription = (
                            model.ruleTypes |> Seq.find(fun x-> x.Name = ruleCfg.Type)
                        ),
                        profile = model.profile,
                        schemes = model.ruleSchemes
                    )
                    let! res = ConfigureAnalyticView.Show(ctx, vm)
                    return res.Handle(
                        apply = (fun m->
                            this.ApplyRuleChanges(model, m.config)
                        ),
                        abort = (fun()->
                            this.ShowForm(model)
                        )
                    )
                with err ->
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        member private this.ApplyModuleChanges(model, config) = async{
            try
                let vac = model.profile.VideoAnalyticsConfiguration
                use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                do! van.ModifyAnalyticsModules(vac.token, [|config|])
            with err ->
                do! show_error(err)
            
            return! this.Main()
        }
        
        member private this.ApplyRuleChanges(model, config) = async{
            try
                let vac = model.profile.VideoAnalyticsConfiguration
                use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                do! van.ModifyRules(vac.token, [|config|])
            with err ->
                do! show_error(err)
            
            return! this.Main()
        }

        member private this.DeleteModule(model, moduleName) = async{
            let vac = model.profile.VideoAnalyticsConfiguration
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalDevice.instance.deleting)
                    do! van.DeleteAnalyticsModules(vac.token, [|moduleName|])
                    return this.Main()
                with err ->
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.DeleteRule(model, ruleName) = async{
            let vac = model.profile.VideoAnalyticsConfiguration
            let! cont = async{
                try
                    use! progress = Progress.Show(ctx, LocalDevice.instance.deleting)
                    do! van.DeleteRules(vac.token, [|ruleName|])
                    return this.Main()
                with err ->
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }

        member private this.Complete(res) = async{
            return res
        }

        static member Run(ctx, profToken) = 
            let act = new AnalyticsActivity(ctx, profToken)
            act.Main()
    end

