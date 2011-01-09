#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;

using odm.onvif;
using dev = onvif.services.device;
using odm.utils;

namespace odm.models {
	//public static class NetworkSettingsExtensions {
		

	//    public static IObservable<DeviceDescription> Save(this NetworkSettings netSettings, Session session) {

	//        var proxy = session.device;
	//        var id = session.deviceDescription.Id;

	//        var dhcp_enabled = netSettings.dhcp;

	//        var nic_set = new dev::NetworkInterfaceSetConfiguration();

	//        nic_set.EnabledSpecified = false;
	//        nic_set.MTUSpecified = false;
	//        nic_set.IPv4 = new dev.IPv4NetworkInterfaceSetConfiguration();
	//        nic_set.IPv4.DHCP = netSettings.dhcp;
	//        nic_set.IPv4.DHCPSpecified = true;
	//        nic_set.IPv4.Manual = new dev::PrefixedIPv4Address[]{
	//            new dev::PrefixedIPv4Address(){
	//                Address = netSettings.staticIp.ToString(),					
	//                PrefixLength = netSettings.subnetPrefix
	//            }
	//        };

	//        var dns_addresses = new dev::IPAddress[] { 
	//            new dev::IPAddress(){ 
	//                Type = dev::IPType.IPv4, 
	//                IPv4Address = netSettings.staticDns.ToString()
	//            } 
	//        };

	//        var gateway_addresses = new string[]{
	//            netSettings.defaultGateway.ToString()
	//        };
			
	//        return Observable.Start<DeviceDescription>(() => {
	//            proxy.SetDNS(dhcp_enabled, null, dns_addresses).First();
	//            var nics = proxy.GetNetworkInterfaces().First();
	//            var nic = nics.Where(x => x.Enabled).First();

	//            nic_set.MTUSpecified = nic.Info.MTUSpecified;
	//            nic_set.MTU = nic.Info.MTU;

	//            if (nic.Link != null) {
	//                //nic_set_cfg.Link = new NetworkInterfaceConnectionSetting();
	//                if (nic.Link.AdminSettings != null) {
	//                    nic_set.Link = nic.Link.AdminSettings;
	//                } else if (nic.Link.OperSettings != null) {
	//                    nic_set.Link = nic.Link.OperSettings;
	//                }
	//            }

	//            var isRebootNeeded = proxy.SetNetworkInterfaces(nic.token, nic_set).First();
	//            if (isRebootNeeded) {
	//                var message = proxy.SystemReboot().First();
	//                //DebugHelper.Break();
	//                return null;
	//            }
	//            var resolver = new DeviceDiscovery() {
	//                Duration = TimeSpan.FromSeconds(5)
	//            }.Resolve(id);
	//            try {
	//                var devDescr = resolver.First();
	//                return devDescr;
	//            } catch {
	//                return null;
	//            }
	//        }).OnError(err=>{
	//            DebugHelper.Error(err);
	//        });

	//    }
	//}
}
