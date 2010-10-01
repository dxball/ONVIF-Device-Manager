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
//
//----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Disposables;
using onvif.services.device;
using System.ServiceModel.Discovery;
using System.Threading;
using System.ServiceModel;
using nvc.utils;

namespace nvc.onvif {
	public class  DeviceObservable:IDisposable{
		private Device m_proxy;
		public DeviceObservable(Device proxy) {
			DebugHelper.Assert(proxy != null);
			m_proxy = proxy;
		}
		public Uri uri {
			get {
				return (m_proxy as IClientChannel).RemoteAddress.Uri;
			}
		}
				
		//public void Open(){
		//    m_proxy.Open();
		//}

		public IObservable<GetDeviceInformationResponse> GetDeviceInformation() {
			var request = new GetDeviceInformationRequest();
			var asyncOp = Observable.FromAsyncPattern<GetDeviceInformationRequest, GetDeviceInformationResponse>(m_proxy.BeginGetDeviceInformation, m_proxy.EndGetDeviceInformation);
			return asyncOp(request);
		}

		public IObservable<Capabilities> GetCapabilities(params CapabilityCategory[] categories) {
			if (categories == null || categories.IsEmpty()) {
				categories = new CapabilityCategory[] { CapabilityCategory.All };
			}
			var request = new GetCapabilitiesRequest(categories);
			var asyncOp = Observable.FromAsyncPattern<GetCapabilitiesRequest, GetCapabilitiesResponse>(m_proxy.BeginGetCapabilities, m_proxy.EndGetCapabilities);
			return asyncOp(request).Select(x => x.Capabilities);		
		}

		public IObservable<NetworkInterface[]> GetNetworkInterfaces() {
			var request = new GetNetworkInterfacesRequest();
			var asyncOp = Observable.FromAsyncPattern<GetNetworkInterfacesRequest, GetNetworkInterfacesResponse>(m_proxy.BeginGetNetworkInterfaces, m_proxy.EndGetNetworkInterfaces);
			return asyncOp(request).Select(x => x.NetworkInterfaces);
		}

		public IObservable<bool> SetNetworkInterfaces(string interfaceToken, NetworkInterfaceSetConfiguration networkInterface) {
			var request = new SetNetworkInterfacesRequest(interfaceToken, networkInterface);
			var asyncOp = Observable.FromAsyncPattern<SetNetworkInterfacesRequest, SetNetworkInterfacesResponse>(m_proxy.BeginSetNetworkInterfaces, m_proxy.EndSetNetworkInterfaces);
			return asyncOp(request).Select(x => x.RebootNeeded);
		}

		public IObservable<NetworkGateway> GetNetworkDefaultGateway() {
			var request = new GetNetworkDefaultGatewayRequest();
			var asyncOp = Observable.FromAsyncPattern<GetNetworkDefaultGatewayRequest, GetNetworkDefaultGatewayResponse>(m_proxy.BeginGetNetworkDefaultGateway, m_proxy.EndGetNetworkDefaultGateway);
			return asyncOp(request).Select(x => x.NetworkGateway);
		}

		public IObservable<DNSInformation> GetDNS() {
			var request = new GetDNSRequest();
			var asyncOp = Observable.FromAsyncPattern<GetDNSRequest, GetDNSResponse>(m_proxy.BeginGetDNS, m_proxy.EndGetDNS);
			return asyncOp(request).Select(x => x.DNSInformation);	
		}

		public IObservable<Unit> SetDNS(bool fromDHCP, string[] searchDomain, IPAddress[] DNSManual) {
			var request = new SetDNSRequest(fromDHCP, searchDomain, DNSManual);
			var asyncOp = Observable.FromAsyncPattern<SetDNSRequest, SetDNSResponse>(m_proxy.BeginSetDNS, m_proxy.EndSetDNS);
			return asyncOp(request).Select(x => new Unit());		
		}

		public IObservable<Unit> SetNetworkDefaultGateway(string[] ipv4Addresses, string[] ipv6Addresses) {
			var request = new SetNetworkDefaultGatewayRequest(ipv4Addresses, ipv6Addresses);
			var asyncOp = Observable.FromAsyncPattern<SetNetworkDefaultGatewayRequest, SetNetworkDefaultGatewayResponse>(m_proxy.BeginSetNetworkDefaultGateway, m_proxy.EndSetNetworkDefaultGateway);
			return asyncOp(request).Select(x => new Unit());		
		}

		public IObservable<string> SystemReboot() {
			var request = new SystemRebootRequest();
			var asyncOp = Observable.FromAsyncPattern<SystemRebootRequest, SystemRebootResponse>(m_proxy.BeginSystemReboot, m_proxy.EndSystemReboot);
			return asyncOp(request).Select(x => x.Message);
		}

		public void Dispose() {
		   //m_proxy.Close();
		}
		public Device Services {
			get {
				return m_proxy;
			}
		}
	}
}
