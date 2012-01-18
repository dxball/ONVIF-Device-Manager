namespace odm.ui.activities
    open System
    open System.Linq
    //open System.Disposables
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.Threading
    open System.Net
    open System.Net.Sockets
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    open onvif.services
    open onvif.utils

    open odm.core
    open odm.infra
    open odm.onvif
    open utils
    //open utils.extensions
    //open odm.models
    open utils.fsharp
    open odm.ui
//    open odm.ui.core
//    open odm.ui.controls
//    open odm.ui.views
//    open odm.ui.dialogs


    type NetworkSettingsActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        let facade = new OdmSession(session)
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }

        let load() = async{
            let! nics, ntpInfo, zeroConfSupported, zeroConf, host, gatewayInfo, dnsInfo, netProtocols = 
                Async.Parallel(
                    dev.GetNetworkInterfaces(),
                    dev.GetNTP(),
                    facade.IsZeroConfigurationSupported(),
                    async{
                        let! zeroConfSupported = facade.IsZeroConfigurationSupported()
                        if zeroConfSupported then
                            return! dev.GetZeroConfiguration()
                        else
                            return null
                    },
                    dev.GetHostname(),
                    dev.GetNetworkDefaultGateway(),
                    dev.GetDNS(),
                    async{
                        let! protocols = dev.GetNetworkProtocols()
                        if protocols = null then
                            return [||]
                        else
                            let res = Array.zeroCreate<NetworkSettingsView.NetworkProtocol>(protocols.Length)
                            for i in {0..res.Length-1} do
                                let ports = String.Join("; ", protocols.[i].Port.Select(fun x->x.ToString()))
                                res.[i] <- new NetworkSettingsView.NetworkProtocol(
                                    name = protocols.[i].Name,
                                    ports = ports,
                                    enabled = protocols.[i].Enabled
                                )
                            return res
                    }
                )

            let zeroConfIp = 
                if not(zeroConfSupported) || zeroConf=null || zeroConf.Addresses = null then
                    Seq.empty
                else
                    zeroConf.Addresses |> Seq.filter (fun x-> not (String.IsNullOrWhiteSpace(x)))
            

            let model = new NetworkSettingsView.Model(
                zeroConfIp = String.Join("; ", zeroConfIp),
                zeroConfSupported = zeroConfSupported
            )
            
            model.zeroConfEnabled <- zeroConfSupported && zeroConf<>null && zeroConf.Enabled
            model.useHostFromDhcp <- host.FromDHCP
            model.host <- host.Name
            model.netProtocols <- netProtocols
            
            if ntpInfo<>null then
                model.useNtpFromDhcp <- ntpInfo.FromDHCP
                let ntp = 
                    if ntpInfo.NTPManual<>null && ntpInfo.NTPManual.Length>0 then
                        ntpInfo.NTPManual
                    else
                        ntpInfo.NTPFromDHCP

                if ntp <> null then
                    model.ntpServers <- String.Join("; ", seq{
                        for n in ntp do
                            let s = OdmSession.NetHostToStr(n)
                            if not(String.IsNullOrWhiteSpace(s)) then
                                yield s
                    })
            
            if gatewayInfo<>null && gatewayInfo.IPv4Address<>null && gatewayInfo.IPv4Address.Length>0 then
                let addresses = seq{
                    if gatewayInfo.IPv4Address<>null then
                        for x in gatewayInfo.IPv4Address |> Seq.filter (fun x-> x<>null) do
                            let ip = x.Trim()
                            if not(String.IsNullOrWhiteSpace(ip)) then
                                yield ip
                    if gatewayInfo.IPv6Address<>null then
                        for x in gatewayInfo.IPv6Address |> Seq.filter (fun x-> x<>null) do
                            let ip = x.Trim()
                            if not(String.IsNullOrWhiteSpace(ip)) then
                                yield ip
                }
                model.gateway <-  String.Join(";", addresses)

            if dnsInfo<>null then
                model.useDnsFromDhcp <- dnsInfo.FromDHCP
                let dns = 
                    if dnsInfo.DNSManual<>null && dnsInfo.DNSManual.Length>0 then
                        dnsInfo.DNSManual
                    else
                        dnsInfo.DNSFromDHCP
                if dns <> null then
                    model.dns <- String.Join(";", dns |> Seq.filter (fun x-> x<>null && not(String.IsNullOrWhiteSpace(x.IPv4Address))) |> Seq.map (fun x->x.IPv4Address))
            
            match nics |> Seq.tryFind (fun x -> x.Enabled) with
            | Some nic ->
                
                let nic_cfg = nic.IPv4.Config
                model.dhcp <- nic.IPv4.Config.DHCP
                if nic_cfg.Manual<>null && nic_cfg.Manual.Length>0 then 
                    let ipInfo = nic_cfg.Manual.[0]
                    model.ip <- ipInfo.Address
                    model.subnet <- (ipInfo.PrefixLength |> NetMaskHelper.CidrToIpMask).ToString()
                else 
                    if nic_cfg.FromDHCP<>null then
                        model.ip <- nic_cfg.FromDHCP.Address
                        model.subnet <- (nic_cfg.FromDHCP.PrefixLength |> NetMaskHelper.CidrToIpMask).ToString()
            | None ->
                model.dhcp <- false
                model.ip <- "255.255.255.255"
                model.subnet <- "255.255.255.255"
                
            model.AcceptChanges()
            return model
        }

        let apply(model:NetworkSettingsView.Model) = async{
            //apply changes of network settings
            
            if not (model.isModified) then 
                //nothing has been changed
                return ()

            let dns_has_changed = 
                model.origin.dns <> model.current.dns ||
                model.origin.dhcp <> model.current.dhcp ||
                model.origin.useDnsFromDhcp <>  model.current.useDnsFromDhcp

            let host_has_changed = 
                model.origin.host <> model.current.host ||
                model.origin.useHostFromDhcp <> model.current.useHostFromDhcp

            let gateway_has_changed = 
                model.origin.gateway <> model.current.gateway
                            
            let ip_has_changed = 
                model.origin.ip <> model.current.ip ||
                model.origin.subnet <> model.current.subnet ||
                model.origin.dhcp <> model.current.dhcp
                            
            let ntp_has_changed = 
                model.origin.ntpServers <> model.current.ntpServers ||
                model.origin.dhcp <> model.current.dhcp ||
                model.origin.useNtpFromDhcp <> model.current.useNtpFromDhcp
                
            let zero_conf_has_changed = 
                model.zeroConfSupported && (model.origin.zeroConfEnabled <> model.current.zeroConfEnabled)
            
            let currentNetProtocols = 
                if model.netProtocols <> null then
                    model.netProtocols
                else
                    Seq.empty
            
            let protocols_has_changed = 
                
                let temp = new List<_>(model.origin.netProtocols)
                currentNetProtocols |> Seq.takeWhile(fun x->
                    let i = temp |> Seq.tryFind (fun y ->
                        y.name = x.name && y.enabled = x.enabled && y.ports = x.ports
                    )
                    match i with
                    | Some n -> temp.Remove(n)
                    | None -> true
                )|> Seq.iter (fun x->())
                temp.Count <> 0

            if protocols_has_changed then
                do! async{
                    use! progress = Progress.Show(ctx, "applying protocol settings...")
                    let netProtocols = [|
                        for x in currentNetProtocols do
                            let p = new NetworkProtocol()
                            p.Name <- x.name
                            p.Enabled <- x.enabled
                            p.Port <- [|
                                if x.ports <> null then
                                    let ports = 
                                        x.ports.Split([|';'|], StringSplitOptions.RemoveEmptyEntries)
                                        |> Seq.map(fun h-> if h<>null then h.Trim() else null)
                                        |> Seq.filter(fun h-> not(String.IsNullOrWhiteSpace(h)))
                                    for i in ports do
                                        yield Int32.Parse(i)
                            |]
                            yield p
                    |]
                    do! session.SetNetworkProtocols(netProtocols)
                }
            if ntp_has_changed then
                do! async{
                    use! progress = Progress.Show(ctx, LocalNetworkSettings.instance.applyindNtp)
                    let ntp_addresses = [| 
                        if model.current.ntpServers <> null then
                            for x in model.current.ntpServers.Split([|';'; ' '; ','|], StringSplitOptions.RemoveEmptyEntries) do
                                if not(String.IsNullOrWhiteSpace(x)) then
                                    yield OdmSession.NetHostFromStr(x)
                        
                    |]
                    let useDhcp = model.current.dhcp && model.current.useNtpFromDhcp
                    do! session.SetNTP(useDhcp, ntp_addresses)
                }

            if dns_has_changed then
                do! async{
                    use! progress = Progress.Show(ctx, LocalNetworkSettings.instance.applyindDns)
                    let dns_addresses = [| 
                        if model.current.dns <> null then
                            for x in model.current.dns.Split([|';'|], StringSplitOptions.RemoveEmptyEntries) do
                                let x = x.Trim()
                                if not(String.IsNullOrWhiteSpace(x)) then
                                    yield new IPAddress(
                                        Type = IPType.IPv4, 
                                        IPv4Address = x
                                    )
                    |]
                    let useDhcp = model.current.dhcp && model.current.useDnsFromDhcp
                    do! session.SetDNS(useDhcp, null, dns_addresses)
                }

            if gateway_has_changed then
                do! async{
                    use! view = Progress.Show(ctx, LocalNetworkSettings.instance.applyindGateway)
                    let ips = [
                        if model.current.gateway <> null then
                            for x in model.current.gateway.Split([|';'|], StringSplitOptions.RemoveEmptyEntries) do
                                let valid,ip = IPAddress.TryParse(x.Trim())
                                if not(valid) then
                                    failwith LocalNetworkSettings.instance.invalidIpForGateway
                                yield ip
                    ]
                    let ipv4_list = [
                        for x in ips do
                            if x.AddressFamily=AddressFamily.InterNetwork then
                                yield x.ToString()
                    ]
                    let ipv6_list = [
                        for x in ips do
                            if x.AddressFamily=AddressFamily.InterNetworkV6 then
                                yield x.ToString()
                    ]
                    do! session.SetNetworkDefaultGateway(ipv4_list |> List.toArray, ipv6_list |> List.toArray)
                }

            if zero_conf_has_changed then
                do! async{
                    use! view = Progress.Show(ctx, LocalNetworkSettings.instance.applyindZeroConf)
                    let! zeroConf = dev.GetZeroConfiguration()
                    do! dev.SetZeroConfiguration(zeroConf.InterfaceToken ,model.zeroConfEnabled)
                }

            if host_has_changed then
                do! async{
                    use! view = Progress.Show(ctx, LocalNetworkSettings.instance.applyindHostName)
                    if model.useHostFromDhcp then
                        do! dev.SetHostname(null)
                    else
                        do! dev.SetHostname(model.host)
                }

            if ip_has_changed then
                let! rebootIsNeeded = 
                    async{
                        use! progress = Progress.Show(ctx, LocalNetworkSettings.instance.applyindIp)
                        
                        //tt::NetworkInterface[] nics = null
                        let! nics = session.GetNetworkInterfaces()
                        let nic = nics |> Seq.find(fun x -> x.Enabled)
                        
                        let nic_set = new NetworkInterfaceSetConfiguration()
                        nic_set.Enabled <- true
                        nic_set.EnabledSpecified <- true
                        nic_set.MTUSpecified <- nic.Info.MTUSpecified
                        nic_set.MTU <- nic.Info.MTU
                        
                        nic_set.IPv4 <- new IPv4NetworkInterfaceSetConfiguration()
                        nic_set.IPv4.DHCP <- model.current.dhcp
                        nic_set.IPv4.DHCPSpecified <- true
                        nic_set.IPv4.Enabled <- true
                        nic_set.IPv4.EnabledSpecified <- true
                        nic_set.IPv4.Manual <- 
                            let ip = model.current.ip
                            if String.IsNullOrWhiteSpace(ip) then
                                null
                            else [|
                                new PrefixedIPv4Address(
                                    Address = model.current.ip.ToString(),
                                    PrefixLength = (model.current.subnet |> IPAddress.Parse |> NetMaskHelper.IpToCidrMask)
                                )
                            |]
                        
        //                if nic.Link <> null then
        //                    //nic_set_cfg.Link = new NetworkInterfaceConnectionSetting()
        //                    if nic.Link.AdminSettings <> null then
        //                        nic_set.Link <- nic.Link.AdminSettings
        //                    else if nic.Link.OperSettings <> null then
        //                        nic_set.Link <- nic.Link.OperSettings

                        return! session.SetNetworkInterfaces(nic.token, nic_set)
                    }
                if rebootIsNeeded then
                    do! async{
                        use! progress = Progress.Show(ctx, LocalMaintenance.instance.rebooting)
                        do! session.SystemReboot() |> Async.Ignore
                    }
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
                    let! res = NetworkSettingsView.Show(ctx, model)
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
            try
                use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                do! apply(model)
            with err ->
                do! show_error(err)

            return! this.Main()
        }

        member private this.Complete(result) = async{
            return result
        }

        static member Run(ctx) = 
            let act = new NetworkSettingsActivity(ctx)
            act.Main()
        
    end
