
		
using System;
using System.ServiceModel;
using System.Xml.Serialization;
using onvif.types;
namespace onvif.services{
		

		[ServiceContract(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		public interface IDevice{
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDeviceInformation", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetDeviceInformation(GetDeviceInformationRequest request, AsyncCallback callback, object asyncState);
			GetDeviceInformationResponse EndGetDeviceInformation(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetSystemDateAndTime", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetSystemDateAndTime(SetSystemDateAndTimeRequest request, AsyncCallback callback, object asyncState);
			SetSystemDateAndTimeResponse EndSetSystemDateAndTime(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetSystemDateAndTime", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetSystemDateAndTime(GetSystemDateAndTimeRequest request, AsyncCallback callback, object asyncState);
			GetSystemDateAndTimeResponse EndGetSystemDateAndTime(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetSystemFactoryDefault", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetSystemFactoryDefault(SetSystemFactoryDefaultRequest request, AsyncCallback callback, object asyncState);
			SetSystemFactoryDefaultResponse EndSetSystemFactoryDefault(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/UpgradeSystemFirmware", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginUpgradeSystemFirmware(UpgradeSystemFirmwareRequest request, AsyncCallback callback, object asyncState);
			UpgradeSystemFirmwareResponse EndUpgradeSystemFirmware(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/StartFirmwareUpgrade", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginStartFirmwareUpgrade(StartFirmwareUpgradeRequest request, AsyncCallback callback, object asyncState);
			StartFirmwareUpgradeResponse EndStartFirmwareUpgrade(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SystemReboot", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSystemReboot(SystemRebootRequest request, AsyncCallback callback, object asyncState);
			SystemRebootResponse EndSystemReboot(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/RestoreSystem", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginRestoreSystem(RestoreSystemRequest request, AsyncCallback callback, object asyncState);
			RestoreSystemResponse EndRestoreSystem(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetSystemBackup", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetSystemBackup(GetSystemBackupRequest request, AsyncCallback callback, object asyncState);
			GetSystemBackupResponse EndGetSystemBackup(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetSystemLog", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetSystemLog(GetSystemLogRequest request, AsyncCallback callback, object asyncState);
			GetSystemLogResponse EndGetSystemLog(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetSystemSupportInformation", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetSystemSupportInformation(GetSystemSupportInformationRequest request, AsyncCallback callback, object asyncState);
			GetSystemSupportInformationResponse EndGetSystemSupportInformation(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetScopes", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetScopes(GetScopesRequest request, AsyncCallback callback, object asyncState);
			GetScopesResponse EndGetScopes(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetScopes", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetScopes(SetScopesRequest request, AsyncCallback callback, object asyncState);
			SetScopesResponse EndSetScopes(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/AddScopes", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginAddScopes(AddScopesRequest request, AsyncCallback callback, object asyncState);
			AddScopesResponse EndAddScopes(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/RemoveScopes", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginRemoveScopes(RemoveScopesRequest request, AsyncCallback callback, object asyncState);
			RemoveScopesResponse EndRemoveScopes(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDiscoveryMode", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetDiscoveryMode(GetDiscoveryModeRequest request, AsyncCallback callback, object asyncState);
			GetDiscoveryModeResponse EndGetDiscoveryMode(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetDiscoveryMode", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetDiscoveryMode(SetDiscoveryModeRequest request, AsyncCallback callback, object asyncState);
			SetDiscoveryModeResponse EndSetDiscoveryMode(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetRemoteDiscoveryMode", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetRemoteDiscoveryMode(GetRemoteDiscoveryModeRequest request, AsyncCallback callback, object asyncState);
			GetRemoteDiscoveryModeResponse EndGetRemoteDiscoveryMode(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetRemoteDiscoveryMode", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetRemoteDiscoveryMode(SetRemoteDiscoveryModeRequest request, AsyncCallback callback, object asyncState);
			SetRemoteDiscoveryModeResponse EndSetRemoteDiscoveryMode(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDPAddresses", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetDPAddresses(GetDPAddressesRequest request, AsyncCallback callback, object asyncState);
			GetDPAddressesResponse EndGetDPAddresses(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetDPAddresses", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetDPAddresses(SetDPAddressesRequest request, AsyncCallback callback, object asyncState);
			SetDPAddressesResponse EndSetDPAddresses(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetUsers", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetUsers(GetUsersRequest request, AsyncCallback callback, object asyncState);
			GetUsersResponse EndGetUsers(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/CreateUsers", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginCreateUsers(CreateUsersRequest request, AsyncCallback callback, object asyncState);
			CreateUsersResponse EndCreateUsers(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/DeleteUsers", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginDeleteUsers(DeleteUsersRequest request, AsyncCallback callback, object asyncState);
			DeleteUsersResponse EndDeleteUsers(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetUser", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetUser(SetUserRequest request, AsyncCallback callback, object asyncState);
			SetUserResponse EndSetUser(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetWsdlUrl", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetWsdlUrl(GetWsdlUrlRequest request, AsyncCallback callback, object asyncState);
			GetWsdlUrlResponse EndGetWsdlUrl(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetCapabilities", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetCapabilities(GetCapabilitiesRequest request, AsyncCallback callback, object asyncState);
			GetCapabilitiesResponse EndGetCapabilities(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetHostname", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetHostname(GetHostnameRequest request, AsyncCallback callback, object asyncState);
			GetHostnameResponse EndGetHostname(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetHostname", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetHostname(SetHostnameRequest request, AsyncCallback callback, object asyncState);
			SetHostnameResponse EndSetHostname(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDNS", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetDNS(GetDNSRequest request, AsyncCallback callback, object asyncState);
			GetDNSResponse EndGetDNS(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetDNS", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetDNS(SetDNSRequest request, AsyncCallback callback, object asyncState);
			SetDNSResponse EndSetDNS(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetNTP", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetNTP(GetNTPRequest request, AsyncCallback callback, object asyncState);
			GetNTPResponse EndGetNTP(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetNTP", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetNTP(SetNTPRequest request, AsyncCallback callback, object asyncState);
			SetNTPResponse EndSetNTP(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDynamicDNS", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetDynamicDNS(GetDynamicDNSRequest request, AsyncCallback callback, object asyncState);
			GetDynamicDNSResponse EndGetDynamicDNS(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetDynamicDNS", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetDynamicDNS(SetDynamicDNSRequest request, AsyncCallback callback, object asyncState);
			SetDynamicDNSResponse EndSetDynamicDNS(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetNetworkInterfaces", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetNetworkInterfaces(GetNetworkInterfacesRequest request, AsyncCallback callback, object asyncState);
			GetNetworkInterfacesResponse EndGetNetworkInterfaces(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetNetworkInterfaces", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetNetworkInterfaces(SetNetworkInterfacesRequest request, AsyncCallback callback, object asyncState);
			SetNetworkInterfacesResponse EndSetNetworkInterfaces(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetNetworkProtocols", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetNetworkProtocols(GetNetworkProtocolsRequest request, AsyncCallback callback, object asyncState);
			GetNetworkProtocolsResponse EndGetNetworkProtocols(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetNetworkProtocols", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetNetworkProtocols(SetNetworkProtocolsRequest request, AsyncCallback callback, object asyncState);
			SetNetworkProtocolsResponse EndSetNetworkProtocols(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetNetworkDefaultGateway", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetNetworkDefaultGateway(GetNetworkDefaultGatewayRequest request, AsyncCallback callback, object asyncState);
			GetNetworkDefaultGatewayResponse EndGetNetworkDefaultGateway(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetNetworkDefaultGateway", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetNetworkDefaultGateway(SetNetworkDefaultGatewayRequest request, AsyncCallback callback, object asyncState);
			SetNetworkDefaultGatewayResponse EndSetNetworkDefaultGateway(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetZeroConfiguration", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetZeroConfiguration(GetZeroConfigurationRequest request, AsyncCallback callback, object asyncState);
			GetZeroConfigurationResponse EndGetZeroConfiguration(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetZeroConfiguration", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetZeroConfiguration(SetZeroConfigurationRequest request, AsyncCallback callback, object asyncState);
			SetZeroConfigurationResponse EndSetZeroConfiguration(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetIPAddressFilter", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetIPAddressFilter(GetIPAddressFilterRequest request, AsyncCallback callback, object asyncState);
			GetIPAddressFilterResponse EndGetIPAddressFilter(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetIPAddressFilter", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetIPAddressFilter(SetIPAddressFilterRequest request, AsyncCallback callback, object asyncState);
			SetIPAddressFilterResponse EndSetIPAddressFilter(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/AddIPAddressFilter", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginAddIPAddressFilter(AddIPAddressFilterRequest request, AsyncCallback callback, object asyncState);
			AddIPAddressFilterResponse EndAddIPAddressFilter(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/RemoveIPAddressFilter", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginRemoveIPAddressFilter(RemoveIPAddressFilterRequest request, AsyncCallback callback, object asyncState);
			RemoveIPAddressFilterResponse EndRemoveIPAddressFilter(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetAccessPolicy", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetAccessPolicy(GetAccessPolicyRequest request, AsyncCallback callback, object asyncState);
			GetAccessPolicyResponse EndGetAccessPolicy(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetAccessPolicy", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetAccessPolicy(SetAccessPolicyRequest request, AsyncCallback callback, object asyncState);
			SetAccessPolicyResponse EndSetAccessPolicy(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/CreateCertificate", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginCreateCertificate(CreateCertificateRequest request, AsyncCallback callback, object asyncState);
			CreateCertificateResponse EndCreateCertificate(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetCertificates", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetCertificates(GetCertificatesRequest request, AsyncCallback callback, object asyncState);
			GetCertificatesResponse EndGetCertificates(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetCertificatesStatus", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetCertificatesStatus(GetCertificatesStatusRequest request, AsyncCallback callback, object asyncState);
			GetCertificatesStatusResponse EndGetCertificatesStatus(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetCertificatesStatus", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetCertificatesStatus(SetCertificatesStatusRequest request, AsyncCallback callback, object asyncState);
			SetCertificatesStatusResponse EndSetCertificatesStatus(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/DeleteCertificates", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginDeleteCertificates(DeleteCertificatesRequest request, AsyncCallback callback, object asyncState);
			DeleteCertificatesResponse EndDeleteCertificates(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetPkcs10Request", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetPkcs10Request(GetPkcs10RequestRequest request, AsyncCallback callback, object asyncState);
			GetPkcs10RequestResponse EndGetPkcs10Request(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/LoadCertificates", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginLoadCertificates(LoadCertificatesRequest request, AsyncCallback callback, object asyncState);
			LoadCertificatesResponse EndLoadCertificates(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetClientCertificateMode", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetClientCertificateMode(GetClientCertificateModeRequest request, AsyncCallback callback, object asyncState);
			GetClientCertificateModeResponse EndGetClientCertificateMode(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetClientCertificateMode", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetClientCertificateMode(SetClientCertificateModeRequest request, AsyncCallback callback, object asyncState);
			SetClientCertificateModeResponse EndSetClientCertificateMode(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetRelayOutputs", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginGetRelayOutputs(GetRelayOutputsRequest request, AsyncCallback callback, object asyncState);
			GetRelayOutputsResponse EndGetRelayOutputs(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetRelayOutputSettings", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetRelayOutputSettings(SetRelayOutputSettingsRequest request, AsyncCallback callback, object asyncState);
			SetRelayOutputSettingsResponse EndSetRelayOutputSettings(IAsyncResult result);
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetRelayOutputState", ReplyAction="*", AsyncPattern=true)]
			IAsyncResult BeginSetRelayOutputState(SetRelayOutputStateRequest request, AsyncCallback callback, object asyncState);
			SetRelayOutputStateResponse EndSetRelayOutputState(IAsyncResult result);
		
	}
	
}
		
		[MessageContract(WrapperName="GetDeviceInformationRequest", WrapperNamespace="http://www.onvif.org/ver10/device/wsdl", IsWrapped=true)]
		public class GetDeviceInformationRequest{
		