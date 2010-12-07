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
using System.Disposables;
using System.ServiceModel.Discovery;
using System.Threading;
using System.ServiceModel;

using onvif.services.device;
using onvifdm.utils;

using onvif.types;
using tt=onvif.types;

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
			var request = new MsgGetDeviceInformationRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetDeviceInformationRequest, MsgGetDeviceInformationResponse>(m_proxy.BeginGetDeviceInformation, m_proxy.EndGetDeviceInformation);
			return asyncOp(request).Select(x => x.parameters);
		}

		public IObservable<Scope[]> GetScopes() {
			var request = new MsgGetScopesRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetScopesRequest, MsgGetScopesResponse>(m_proxy.BeginGetScopes, m_proxy.EndGetScopes);
			return asyncOp(request).Select(x=>x.parameters.Scopes);
		}

		public IObservable<MsgSetScopesResponse> SetScopes(string[] scopes) {
			var request = new MsgSetScopesRequest();
			request.parameters.Scopes = scopes;

			var asyncOp = Observable.FromAsyncPattern<MsgSetScopesRequest, MsgSetScopesResponse>(m_proxy.BeginSetScopes, m_proxy.EndSetScopes);
			return asyncOp(request);
		}

		public IObservable<Capabilities> GetCapabilities(params CapabilityCategory[] categories) {
			if (categories == null || categories.IsEmpty()) {
				categories = new CapabilityCategory[] { CapabilityCategory.All };
			}
			var request = new MsgGetCapabilitiesRequest();
			request.parameters.Category = categories;
			
			var asyncOp = Observable.FromAsyncPattern<MsgGetCapabilitiesRequest, MsgGetCapabilitiesResponse>(m_proxy.BeginGetCapabilities, m_proxy.EndGetCapabilities);
			return asyncOp(request).Select(x => x.parameters.Capabilities);		
		}

		public IObservable<NetworkInterface[]> GetNetworkInterfaces() {
			var request = new MsgGetNetworkInterfacesRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetNetworkInterfacesRequest, MsgGetNetworkInterfacesResponse>(m_proxy.BeginGetNetworkInterfaces, m_proxy.EndGetNetworkInterfaces);
			return asyncOp(request).Select(x => x.parameters.NetworkInterfaces);
		}

		public IObservable<bool> SetNetworkInterfaces(string interfaceToken, NetworkInterfaceSetConfiguration networkInterface) {
			var request = new MsgSetNetworkInterfacesRequest();
			request.parameters.InterfaceToken = interfaceToken;
			request.parameters.NetworkInterface = networkInterface;

			var asyncOp = Observable.FromAsyncPattern<MsgSetNetworkInterfacesRequest, MsgSetNetworkInterfacesResponse>(m_proxy.BeginSetNetworkInterfaces, m_proxy.EndSetNetworkInterfaces);
			return asyncOp(request).Select(x => x.parameters.RebootNeeded);
		}

		public IObservable<NetworkGateway> GetNetworkDefaultGateway() {
			var request = new MsgGetNetworkDefaultGatewayRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetNetworkDefaultGatewayRequest, MsgGetNetworkDefaultGatewayResponse>(m_proxy.BeginGetNetworkDefaultGateway, m_proxy.EndGetNetworkDefaultGateway);
			return asyncOp(request).Select(x => x.parameters.NetworkGateway);
		}

		public IObservable<DNSInformation> GetDNS() {
			var request = new MsgGetDNSRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetDNSRequest, MsgGetDNSResponse>(m_proxy.BeginGetDNS, m_proxy.EndGetDNS);
			return asyncOp(request).Select(x => x.parameters.DNSInformation);	
		}

		public IObservable<SystemDateTime> GetSystemDateAndTime() {
			var request = new MsgGetSystemDateAndTimeRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetSystemDateAndTimeRequest, MsgGetSystemDateAndTimeResponse>(m_proxy.BeginGetSystemDateAndTime, m_proxy.EndGetSystemDateAndTime);
			return asyncOp(request).Select(x => x.parameters.SystemDateAndTime);	
		}

		public IObservable<Unit> SetSystemDateAndTime(SetDateTimeType dateTimeType, bool daylightSavings, tt::TimeZone timeZone, tt::DateTime utcDateTime) {

			var request = new MsgSetSystemDateAndTimeRequest();
			request.parameters.DateTimeType = dateTimeType;
			request.parameters.DaylightSavings = daylightSavings;
			request.parameters.TimeZone = timeZone;
			request.parameters.UTCDateTime = utcDateTime;

			var asyncOp = Observable.FromAsyncPattern<MsgSetSystemDateAndTimeRequest, MsgSetSystemDateAndTimeResponse>(m_proxy.BeginSetSystemDateAndTime, m_proxy.EndSetSystemDateAndTime);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<Unit> SetDNS(bool fromDHCP, string[] searchDomain, IPAddress[] DNSManual) {
			var request = new MsgSetDNSRequest();
			request.parameters.DNSManual = DNSManual;
			request.parameters.FromDHCP = fromDHCP;
			request.parameters.SearchDomain = searchDomain;
			
			var asyncOp = Observable.FromAsyncPattern<MsgSetDNSRequest, MsgSetDNSResponse>(m_proxy.BeginSetDNS, m_proxy.EndSetDNS);
			return asyncOp(request).Select(x => new Unit());		
		}

		public IObservable<Unit> SetNetworkDefaultGateway(string[] ipv4Addresses, string[] ipv6Addresses) {
			var request = new MsgSetNetworkDefaultGatewayRequest();
			request.parameters.IPv4Address = ipv4Addresses;
			request.parameters.IPv6Address = ipv6Addresses;
			
			var asyncOp = Observable.FromAsyncPattern<MsgSetNetworkDefaultGatewayRequest, MsgSetNetworkDefaultGatewayResponse>(m_proxy.BeginSetNetworkDefaultGateway, m_proxy.EndSetNetworkDefaultGateway);
			return asyncOp(request).Select(x => new Unit());		
		}

		public IObservable<string> SystemReboot() {
			var request = new MsgSystemRebootRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgSystemRebootRequest, MsgSystemRebootResponse>(m_proxy.BeginSystemReboot, m_proxy.EndSystemReboot);
			return asyncOp(request).Select(x => x.parameters.Message);
		}

		public IObservable<StartFirmwareUpgradeResponse> StartFirmwareUpgrade() {
			var request = new MsgStartFirmwareUpgradeRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgStartFirmwareUpgradeRequest, MsgStartFirmwareUpgradeResponse>(m_proxy.BeginStartFirmwareUpgrade, m_proxy.EndStartFirmwareUpgrade);
			return asyncOp(request).Select(x => x.parameters);
		}

		public IObservable<RelayOutput[]> GetRelayOutputs() {
			var request = new MsgGetRelayOutputsRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetRelayOutputsRequest, MsgGetRelayOutputsResponse>(m_proxy.BeginGetRelayOutputs, m_proxy.EndGetRelayOutputs);
			return asyncOp(request).Select(x => x.parameters.RelayOutputs);
		}

		public IObservable<User[]> GetUsers() {
			var request = new MsgGetUsersRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetUsersRequest, MsgGetUsersResponse>(m_proxy.BeginGetUsers, m_proxy.EndGetUsers);
			return asyncOp(request).Select(x => x.parameters.User);
		}

		//security

		public IObservable<bool> GetClientCertificateMode() {
			var request = new MsgGetClientCertificateModeRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetClientCertificateModeRequest, MsgGetClientCertificateModeResponse>(m_proxy.BeginGetClientCertificateMode, m_proxy.EndGetClientCertificateMode);
			return asyncOp(request).Select(x => x.parameters.Enabled);
		}

		public IObservable<Unit> SetClientCertificateMode(bool enabled) {
			var request = new MsgSetClientCertificateModeRequest();
			request.parameters.Enabled = enabled;
			var asyncOp = Observable.FromAsyncPattern<MsgSetClientCertificateModeRequest, MsgSetClientCertificateModeResponse>(m_proxy.BeginSetClientCertificateMode, m_proxy.EndSetClientCertificateMode);
			return asyncOp(request).Select(x => new Unit());
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
