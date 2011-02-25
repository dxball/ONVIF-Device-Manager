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
using odm.utils;

using onvif.types;
using tt=global::onvif.types;

namespace odm.onvif {
	public class  DeviceObservable:IDisposable{
		private Device m_proxy;
		public DeviceObservable(Device proxy) {
			dbg.Assert(proxy != null);
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

		public IObservable<Unit> SetDNS(bool fromDHCP, string[] searchDomain, IPAddress[] DNSManual) {
			var request = new MsgSetDNSRequest();
			request.parameters.DNSManual = DNSManual;
			request.parameters.FromDHCP = fromDHCP;
			request.parameters.SearchDomain = searchDomain;

			var asyncOp = Observable.FromAsyncPattern<MsgSetDNSRequest, MsgSetDNSResponse>(m_proxy.BeginSetDNS, m_proxy.EndSetDNS);
			return asyncOp(request).Select(x => new Unit());
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

		public IObservable<NTPInformation> GetNTP() {
			var request = new MsgGetNTPRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetNTPRequest, MsgGetNTPResponse>(m_proxy.BeginGetNTP, m_proxy.EndGetNTP);
			return asyncOp(request).Select(x => x.parameters.NTPInformation);
		}

		public IObservable<Unit> SetNTP(bool fromDhcp,  NetworkHost[] ntpManual) {

			var request = new MsgSetNTPRequest();
			request.parameters.FromDHCP = fromDhcp;
			request.parameters.NTPManual = ntpManual;

			var asyncOp = Observable.FromAsyncPattern<MsgSetNTPRequest, MsgSetNTPResponse>(m_proxy.BeginSetNTP, m_proxy.EndSetNTP);
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

		public IObservable<Unit> CreateUsers(User[] users) {
			var request = new MsgCreateUsersRequest();
			request.parameters.User = users;
			var asyncOp = Observable.FromAsyncPattern<MsgCreateUsersRequest, MsgCreateUsersResponse>(m_proxy.BeginCreateUsers, m_proxy.EndCreateUsers);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<SystemLog> GetSystemLog() {
			var request = new MsgGetSystemLogRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetSystemLogRequest, MsgGetSystemLogResponse>(m_proxy.BeginGetSystemLog, m_proxy.EndGetSystemLog);
			return asyncOp(request).Select(x => x.parameters.SystemLog);
		}

		public IObservable<BackupFile[]> GetSystemBackup() {
			var request = new MsgGetSystemBackupRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetSystemBackupRequest, MsgGetSystemBackupResponse>(m_proxy.BeginGetSystemBackup, m_proxy.EndGetSystemBackup);
			return asyncOp(request).Select(x => x.parameters.BackupFiles);
		}

		public IObservable<Unit> RestoreSystem(BackupFile[] backupFiles) {
			var request = new MsgRestoreSystemRequest();
			request.parameters.BackupFiles = backupFiles;
			var asyncOp = Observable.FromAsyncPattern<MsgRestoreSystemRequest, MsgRestoreSystemResponse>(m_proxy.BeginRestoreSystem, m_proxy.EndRestoreSystem);
			return asyncOp(request).Select(x => new Unit());
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

		public IObservable<Certificate[]> GetCertificates() {
			var request = new MsgGetCertificatesRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetCertificatesRequest, MsgGetCertificatesResponse>(m_proxy.BeginGetCertificates, m_proxy.EndGetCertificates);
			return asyncOp(request).Select(x => x.parameters.NvtCertificate);
		}

		public IObservable<Certificate> CreateCertificate(string certificateId, string subject, System.DateTime validNotBefore, System.DateTime validNotAfter) {
			var request = new MsgCreateCertificateRequest();
			request.parameters.CertificateID = certificateId;
			request.parameters.Subject = subject;
			request.parameters.ValidNotBefore = validNotBefore;
			request.parameters.ValidNotAfter = validNotAfter;
			var asyncOp = Observable.FromAsyncPattern<MsgCreateCertificateRequest, MsgCreateCertificateResponse>(m_proxy.BeginCreateCertificate, m_proxy.EndCreateCertificate);
			return asyncOp(request).Select(x => x.parameters.NvtCertificate);
		}

		public IObservable<CertificateStatus[]> GetCertificatesStatus() {
			var request = new MsgGetCertificatesStatusRequest();
			var asyncOp = Observable.FromAsyncPattern<MsgGetCertificatesStatusRequest, MsgGetCertificatesStatusResponse>(m_proxy.BeginGetCertificatesStatus, m_proxy.EndGetCertificatesStatus);
			return asyncOp(request).Select(x => x.parameters.CertificateStatus);
		}

		public IObservable<Unit> SetCertificatesStatus(CertificateStatus[] certificateStatuses) {
			var request = new MsgSetCertificatesStatusRequest();
			request.parameters.CertificateStatus = certificateStatuses;
			var asyncOp = Observable.FromAsyncPattern<MsgSetCertificatesStatusRequest, MsgSetCertificatesStatusResponse>(m_proxy.BeginSetCertificatesStatus, m_proxy.EndSetCertificatesStatus);
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
