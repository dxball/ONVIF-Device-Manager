
using sys=global::System;
using System.ServiceModel;
using System.Xml.Serialization;
using onvif.types;
namespace onvif.services.device{
		
		[ServiceContract(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		public interface Device{
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDeviceInformation", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetDeviceInformation(MsgGetDeviceInformationRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetDeviceInformationResponse EndGetDeviceInformation(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetSystemDateAndTime", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetSystemDateAndTime(MsgSetSystemDateAndTimeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetSystemDateAndTimeResponse EndSetSystemDateAndTime(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetSystemDateAndTime", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetSystemDateAndTime(MsgGetSystemDateAndTimeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetSystemDateAndTimeResponse EndGetSystemDateAndTime(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetSystemFactoryDefault", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetSystemFactoryDefault(MsgSetSystemFactoryDefaultRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetSystemFactoryDefaultResponse EndSetSystemFactoryDefault(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/UpgradeSystemFirmware", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginUpgradeSystemFirmware(MsgUpgradeSystemFirmwareRequest request, sys::AsyncCallback callback, object asyncState);
			MsgUpgradeSystemFirmwareResponse EndUpgradeSystemFirmware(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/StartFirmwareUpgrade", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginStartFirmwareUpgrade(MsgStartFirmwareUpgradeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgStartFirmwareUpgradeResponse EndStartFirmwareUpgrade(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SystemReboot", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSystemReboot(MsgSystemRebootRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSystemRebootResponse EndSystemReboot(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/RestoreSystem", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginRestoreSystem(MsgRestoreSystemRequest request, sys::AsyncCallback callback, object asyncState);
			MsgRestoreSystemResponse EndRestoreSystem(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetSystemBackup", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetSystemBackup(MsgGetSystemBackupRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetSystemBackupResponse EndGetSystemBackup(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetSystemLog", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetSystemLog(MsgGetSystemLogRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetSystemLogResponse EndGetSystemLog(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetSystemSupportInformation", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetSystemSupportInformation(MsgGetSystemSupportInformationRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetSystemSupportInformationResponse EndGetSystemSupportInformation(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetScopes", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetScopes(MsgGetScopesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetScopesResponse EndGetScopes(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetScopes", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetScopes(MsgSetScopesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetScopesResponse EndSetScopes(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/AddScopes", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginAddScopes(MsgAddScopesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgAddScopesResponse EndAddScopes(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/RemoveScopes", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginRemoveScopes(MsgRemoveScopesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgRemoveScopesResponse EndRemoveScopes(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDiscoveryMode", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetDiscoveryMode(MsgGetDiscoveryModeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetDiscoveryModeResponse EndGetDiscoveryMode(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetDiscoveryMode", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetDiscoveryMode(MsgSetDiscoveryModeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetDiscoveryModeResponse EndSetDiscoveryMode(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetRemoteDiscoveryMode", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetRemoteDiscoveryMode(MsgGetRemoteDiscoveryModeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetRemoteDiscoveryModeResponse EndGetRemoteDiscoveryMode(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetRemoteDiscoveryMode", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetRemoteDiscoveryMode(MsgSetRemoteDiscoveryModeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetRemoteDiscoveryModeResponse EndSetRemoteDiscoveryMode(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDPAddresses", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetDPAddresses(MsgGetDPAddressesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetDPAddressesResponse EndGetDPAddresses(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetDPAddresses", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetDPAddresses(MsgSetDPAddressesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetDPAddressesResponse EndSetDPAddresses(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetUsers", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetUsers(MsgGetUsersRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetUsersResponse EndGetUsers(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/CreateUsers", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginCreateUsers(MsgCreateUsersRequest request, sys::AsyncCallback callback, object asyncState);
			MsgCreateUsersResponse EndCreateUsers(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/DeleteUsers", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginDeleteUsers(MsgDeleteUsersRequest request, sys::AsyncCallback callback, object asyncState);
			MsgDeleteUsersResponse EndDeleteUsers(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetUser", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetUser(MsgSetUserRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetUserResponse EndSetUser(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetWsdlUrl", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetWsdlUrl(MsgGetWsdlUrlRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetWsdlUrlResponse EndGetWsdlUrl(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetCapabilities", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetCapabilities(MsgGetCapabilitiesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetCapabilitiesResponse EndGetCapabilities(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetHostname", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetHostname(MsgGetHostnameRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetHostnameResponse EndGetHostname(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetHostname", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetHostname(MsgSetHostnameRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetHostnameResponse EndSetHostname(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDNS", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetDNS(MsgGetDNSRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetDNSResponse EndGetDNS(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetDNS", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetDNS(MsgSetDNSRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetDNSResponse EndSetDNS(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetNTP", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetNTP(MsgGetNTPRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetNTPResponse EndGetNTP(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetNTP", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetNTP(MsgSetNTPRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetNTPResponse EndSetNTP(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetDynamicDNS", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetDynamicDNS(MsgGetDynamicDNSRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetDynamicDNSResponse EndGetDynamicDNS(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetDynamicDNS", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetDynamicDNS(MsgSetDynamicDNSRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetDynamicDNSResponse EndSetDynamicDNS(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetNetworkInterfaces", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetNetworkInterfaces(MsgGetNetworkInterfacesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetNetworkInterfacesResponse EndGetNetworkInterfaces(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetNetworkInterfaces", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetNetworkInterfaces(MsgSetNetworkInterfacesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetNetworkInterfacesResponse EndSetNetworkInterfaces(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetNetworkProtocols", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetNetworkProtocols(MsgGetNetworkProtocolsRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetNetworkProtocolsResponse EndGetNetworkProtocols(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetNetworkProtocols", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetNetworkProtocols(MsgSetNetworkProtocolsRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetNetworkProtocolsResponse EndSetNetworkProtocols(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetNetworkDefaultGateway", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetNetworkDefaultGateway(MsgGetNetworkDefaultGatewayRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetNetworkDefaultGatewayResponse EndGetNetworkDefaultGateway(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetNetworkDefaultGateway", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetNetworkDefaultGateway(MsgSetNetworkDefaultGatewayRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetNetworkDefaultGatewayResponse EndSetNetworkDefaultGateway(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetZeroConfiguration", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetZeroConfiguration(MsgGetZeroConfigurationRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetZeroConfigurationResponse EndGetZeroConfiguration(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetZeroConfiguration", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetZeroConfiguration(MsgSetZeroConfigurationRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetZeroConfigurationResponse EndSetZeroConfiguration(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetIPAddressFilter", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetIPAddressFilter(MsgGetIPAddressFilterRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetIPAddressFilterResponse EndGetIPAddressFilter(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetIPAddressFilter", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetIPAddressFilter(MsgSetIPAddressFilterRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetIPAddressFilterResponse EndSetIPAddressFilter(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/AddIPAddressFilter", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginAddIPAddressFilter(MsgAddIPAddressFilterRequest request, sys::AsyncCallback callback, object asyncState);
			MsgAddIPAddressFilterResponse EndAddIPAddressFilter(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/RemoveIPAddressFilter", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginRemoveIPAddressFilter(MsgRemoveIPAddressFilterRequest request, sys::AsyncCallback callback, object asyncState);
			MsgRemoveIPAddressFilterResponse EndRemoveIPAddressFilter(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetAccessPolicy", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetAccessPolicy(MsgGetAccessPolicyRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetAccessPolicyResponse EndGetAccessPolicy(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetAccessPolicy", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetAccessPolicy(MsgSetAccessPolicyRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetAccessPolicyResponse EndSetAccessPolicy(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/CreateCertificate", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginCreateCertificate(MsgCreateCertificateRequest request, sys::AsyncCallback callback, object asyncState);
			MsgCreateCertificateResponse EndCreateCertificate(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetCertificates", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetCertificates(MsgGetCertificatesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetCertificatesResponse EndGetCertificates(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetCertificatesStatus", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetCertificatesStatus(MsgGetCertificatesStatusRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetCertificatesStatusResponse EndGetCertificatesStatus(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetCertificatesStatus", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetCertificatesStatus(MsgSetCertificatesStatusRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetCertificatesStatusResponse EndSetCertificatesStatus(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/DeleteCertificates", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginDeleteCertificates(MsgDeleteCertificatesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgDeleteCertificatesResponse EndDeleteCertificates(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetPkcs10Request", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetPkcs10Request(MsgGetPkcs10RequestRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetPkcs10RequestResponse EndGetPkcs10Request(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/LoadCertificates", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginLoadCertificates(MsgLoadCertificatesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgLoadCertificatesResponse EndLoadCertificates(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetClientCertificateMode", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetClientCertificateMode(MsgGetClientCertificateModeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetClientCertificateModeResponse EndGetClientCertificateMode(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetClientCertificateMode", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetClientCertificateMode(MsgSetClientCertificateModeRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetClientCertificateModeResponse EndSetClientCertificateMode(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/GetRelayOutputs", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetRelayOutputs(MsgGetRelayOutputsRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetRelayOutputsResponse EndGetRelayOutputs(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetRelayOutputSettings", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetRelayOutputSettings(MsgSetRelayOutputSettingsRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetRelayOutputSettingsResponse EndSetRelayOutputSettings(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/device/wsdl/SetRelayOutputState", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetRelayOutputState(MsgSetRelayOutputStateRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetRelayOutputStateResponse EndSetRelayOutputState(sys::IAsyncResult result);			
		
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDeviceInformationRequest{
			
				[MessageBodyMember]
				[XmlElement("GetDeviceInformation", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDeviceInformation parameters;
			
			public MsgGetDeviceInformationRequest(){
				parameters = new GetDeviceInformation();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDeviceInformationResponse{
			
				[MessageBodyMember]
				[XmlElement("GetDeviceInformationResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDeviceInformationResponse parameters;
			
			public MsgGetDeviceInformationResponse(){
				parameters = new GetDeviceInformationResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetSystemDateAndTimeRequest{
			
				[MessageBodyMember]
				[XmlElement("SetSystemDateAndTime", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetSystemDateAndTime parameters;
			
			public MsgSetSystemDateAndTimeRequest(){
				parameters = new SetSystemDateAndTime();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetSystemDateAndTimeResponse{
			
				[MessageBodyMember]
				[XmlElement("SetSystemDateAndTimeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetSystemDateAndTimeResponse parameters;
			
			public MsgSetSystemDateAndTimeResponse(){
				parameters = new SetSystemDateAndTimeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetSystemDateAndTimeRequest{
			
				[MessageBodyMember]
				[XmlElement("GetSystemDateAndTime", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetSystemDateAndTime parameters;
			
			public MsgGetSystemDateAndTimeRequest(){
				parameters = new GetSystemDateAndTime();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetSystemDateAndTimeResponse{
			
				[MessageBodyMember]
				[XmlElement("GetSystemDateAndTimeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetSystemDateAndTimeResponse parameters;
			
			public MsgGetSystemDateAndTimeResponse(){
				parameters = new GetSystemDateAndTimeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetSystemFactoryDefaultRequest{
			
				[MessageBodyMember]
				[XmlElement("SetSystemFactoryDefault", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetSystemFactoryDefault parameters;
			
			public MsgSetSystemFactoryDefaultRequest(){
				parameters = new SetSystemFactoryDefault();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetSystemFactoryDefaultResponse{
			
				[MessageBodyMember]
				[XmlElement("SetSystemFactoryDefaultResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetSystemFactoryDefaultResponse parameters;
			
			public MsgSetSystemFactoryDefaultResponse(){
				parameters = new SetSystemFactoryDefaultResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgUpgradeSystemFirmwareRequest{
			
				[MessageBodyMember]
				[XmlElement("UpgradeSystemFirmware", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public UpgradeSystemFirmware parameters;
			
			public MsgUpgradeSystemFirmwareRequest(){
				parameters = new UpgradeSystemFirmware();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgUpgradeSystemFirmwareResponse{
			
				[MessageBodyMember]
				[XmlElement("UpgradeSystemFirmwareResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public UpgradeSystemFirmwareResponse parameters;
			
			public MsgUpgradeSystemFirmwareResponse(){
				parameters = new UpgradeSystemFirmwareResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgStartFirmwareUpgradeRequest{
			
				[MessageBodyMember]
				[XmlElement("StartFirmwareUpgrade", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public StartFirmwareUpgrade parameters;
			
			public MsgStartFirmwareUpgradeRequest(){
				parameters = new StartFirmwareUpgrade();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgStartFirmwareUpgradeResponse{
			
				[MessageBodyMember]
				[XmlElement("StartFirmwareUpgradeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public StartFirmwareUpgradeResponse parameters;
			
			public MsgStartFirmwareUpgradeResponse(){
				parameters = new StartFirmwareUpgradeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSystemRebootRequest{
			
				[MessageBodyMember]
				[XmlElement("SystemReboot", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SystemReboot parameters;
			
			public MsgSystemRebootRequest(){
				parameters = new SystemReboot();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSystemRebootResponse{
			
				[MessageBodyMember]
				[XmlElement("SystemRebootResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SystemRebootResponse parameters;
			
			public MsgSystemRebootResponse(){
				parameters = new SystemRebootResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetSystemBackupRequest{
			
				[MessageBodyMember]
				[XmlElement("GetSystemBackup", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetSystemBackup parameters;
			
			public MsgGetSystemBackupRequest(){
				parameters = new GetSystemBackup();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetSystemBackupResponse{
			
				[MessageBodyMember]
				[XmlElement("GetSystemBackupResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetSystemBackupResponse parameters;
			
			public MsgGetSystemBackupResponse(){
				parameters = new GetSystemBackupResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgRestoreSystemRequest{
			
				[MessageBodyMember]
				[XmlElement("RestoreSystem", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public RestoreSystem parameters;
			
			public MsgRestoreSystemRequest(){
				parameters = new RestoreSystem();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgRestoreSystemResponse{
			
				[MessageBodyMember]
				[XmlElement("RestoreSystemResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public RestoreSystemResponse parameters;
			
			public MsgRestoreSystemResponse(){
				parameters = new RestoreSystemResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetSystemSupportInformationRequest{
			
				[MessageBodyMember]
				[XmlElement("GetSystemSupportInformation", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetSystemSupportInformation parameters;
			
			public MsgGetSystemSupportInformationRequest(){
				parameters = new GetSystemSupportInformation();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetSystemSupportInformationResponse{
			
				[MessageBodyMember]
				[XmlElement("GetSystemSupportInformationResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetSystemSupportInformationResponse parameters;
			
			public MsgGetSystemSupportInformationResponse(){
				parameters = new GetSystemSupportInformationResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetSystemLogRequest{
			
				[MessageBodyMember]
				[XmlElement("GetSystemLog", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetSystemLog parameters;
			
			public MsgGetSystemLogRequest(){
				parameters = new GetSystemLog();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetSystemLogResponse{
			
				[MessageBodyMember]
				[XmlElement("GetSystemLogResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetSystemLogResponse parameters;
			
			public MsgGetSystemLogResponse(){
				parameters = new GetSystemLogResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetScopesRequest{
			
				[MessageBodyMember]
				[XmlElement("GetScopes", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetScopes parameters;
			
			public MsgGetScopesRequest(){
				parameters = new GetScopes();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetScopesResponse{
			
				[MessageBodyMember]
				[XmlElement("GetScopesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetScopesResponse parameters;
			
			public MsgGetScopesResponse(){
				parameters = new GetScopesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetScopesRequest{
			
				[MessageBodyMember]
				[XmlElement("SetScopes", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetScopes parameters;
			
			public MsgSetScopesRequest(){
				parameters = new SetScopes();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetScopesResponse{
			
				[MessageBodyMember]
				[XmlElement("SetScopesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetScopesResponse parameters;
			
			public MsgSetScopesResponse(){
				parameters = new SetScopesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgAddScopesRequest{
			
				[MessageBodyMember]
				[XmlElement("AddScopes", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public AddScopes parameters;
			
			public MsgAddScopesRequest(){
				parameters = new AddScopes();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgAddScopesResponse{
			
				[MessageBodyMember]
				[XmlElement("AddScopesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public AddScopesResponse parameters;
			
			public MsgAddScopesResponse(){
				parameters = new AddScopesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgRemoveScopesRequest{
			
				[MessageBodyMember]
				[XmlElement("RemoveScopes", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public RemoveScopes parameters;
			
			public MsgRemoveScopesRequest(){
				parameters = new RemoveScopes();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgRemoveScopesResponse{
			
				[MessageBodyMember]
				[XmlElement("RemoveScopesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public RemoveScopesResponse parameters;
			
			public MsgRemoveScopesResponse(){
				parameters = new RemoveScopesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDiscoveryModeRequest{
			
				[MessageBodyMember]
				[XmlElement("GetDiscoveryMode", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDiscoveryMode parameters;
			
			public MsgGetDiscoveryModeRequest(){
				parameters = new GetDiscoveryMode();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDiscoveryModeResponse{
			
				[MessageBodyMember]
				[XmlElement("GetDiscoveryModeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDiscoveryModeResponse parameters;
			
			public MsgGetDiscoveryModeResponse(){
				parameters = new GetDiscoveryModeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetDiscoveryModeRequest{
			
				[MessageBodyMember]
				[XmlElement("SetDiscoveryMode", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetDiscoveryMode parameters;
			
			public MsgSetDiscoveryModeRequest(){
				parameters = new SetDiscoveryMode();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetDiscoveryModeResponse{
			
				[MessageBodyMember]
				[XmlElement("SetDiscoveryModeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetDiscoveryModeResponse parameters;
			
			public MsgSetDiscoveryModeResponse(){
				parameters = new SetDiscoveryModeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetRemoteDiscoveryModeRequest{
			
				[MessageBodyMember]
				[XmlElement("GetRemoteDiscoveryMode", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetRemoteDiscoveryMode parameters;
			
			public MsgGetRemoteDiscoveryModeRequest(){
				parameters = new GetRemoteDiscoveryMode();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetRemoteDiscoveryModeResponse{
			
				[MessageBodyMember]
				[XmlElement("GetRemoteDiscoveryModeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetRemoteDiscoveryModeResponse parameters;
			
			public MsgGetRemoteDiscoveryModeResponse(){
				parameters = new GetRemoteDiscoveryModeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetRemoteDiscoveryModeRequest{
			
				[MessageBodyMember]
				[XmlElement("SetRemoteDiscoveryMode", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetRemoteDiscoveryMode parameters;
			
			public MsgSetRemoteDiscoveryModeRequest(){
				parameters = new SetRemoteDiscoveryMode();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetRemoteDiscoveryModeResponse{
			
				[MessageBodyMember]
				[XmlElement("SetRemoteDiscoveryModeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetRemoteDiscoveryModeResponse parameters;
			
			public MsgSetRemoteDiscoveryModeResponse(){
				parameters = new SetRemoteDiscoveryModeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDPAddressesRequest{
			
				[MessageBodyMember]
				[XmlElement("GetDPAddresses", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDPAddresses parameters;
			
			public MsgGetDPAddressesRequest(){
				parameters = new GetDPAddresses();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDPAddressesResponse{
			
				[MessageBodyMember]
				[XmlElement("GetDPAddressesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDPAddressesResponse parameters;
			
			public MsgGetDPAddressesResponse(){
				parameters = new GetDPAddressesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetDPAddressesRequest{
			
				[MessageBodyMember]
				[XmlElement("SetDPAddresses", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetDPAddresses parameters;
			
			public MsgSetDPAddressesRequest(){
				parameters = new SetDPAddresses();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetDPAddressesResponse{
			
				[MessageBodyMember]
				[XmlElement("SetDPAddressesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetDPAddressesResponse parameters;
			
			public MsgSetDPAddressesResponse(){
				parameters = new SetDPAddressesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetUsersRequest{
			
				[MessageBodyMember]
				[XmlElement("GetUsers", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetUsers parameters;
			
			public MsgGetUsersRequest(){
				parameters = new GetUsers();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetUsersResponse{
			
				[MessageBodyMember]
				[XmlElement("GetUsersResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetUsersResponse parameters;
			
			public MsgGetUsersResponse(){
				parameters = new GetUsersResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgCreateUsersRequest{
			
				[MessageBodyMember]
				[XmlElement("CreateUsers", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public CreateUsers parameters;
			
			public MsgCreateUsersRequest(){
				parameters = new CreateUsers();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgCreateUsersResponse{
			
				[MessageBodyMember]
				[XmlElement("CreateUsersResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public CreateUsersResponse parameters;
			
			public MsgCreateUsersResponse(){
				parameters = new CreateUsersResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgDeleteUsersRequest{
			
				[MessageBodyMember]
				[XmlElement("DeleteUsers", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public DeleteUsers parameters;
			
			public MsgDeleteUsersRequest(){
				parameters = new DeleteUsers();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgDeleteUsersResponse{
			
				[MessageBodyMember]
				[XmlElement("DeleteUsersResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public DeleteUsersResponse parameters;
			
			public MsgDeleteUsersResponse(){
				parameters = new DeleteUsersResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetUserRequest{
			
				[MessageBodyMember]
				[XmlElement("SetUser", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetUser parameters;
			
			public MsgSetUserRequest(){
				parameters = new SetUser();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetUserResponse{
			
				[MessageBodyMember]
				[XmlElement("SetUserResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetUserResponse parameters;
			
			public MsgSetUserResponse(){
				parameters = new SetUserResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetWsdlUrlRequest{
			
				[MessageBodyMember]
				[XmlElement("GetWsdlUrl", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetWsdlUrl parameters;
			
			public MsgGetWsdlUrlRequest(){
				parameters = new GetWsdlUrl();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetWsdlUrlResponse{
			
				[MessageBodyMember]
				[XmlElement("GetWsdlUrlResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetWsdlUrlResponse parameters;
			
			public MsgGetWsdlUrlResponse(){
				parameters = new GetWsdlUrlResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetCapabilitiesRequest{
			
				[MessageBodyMember]
				[XmlElement("GetCapabilities", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetCapabilities parameters;
			
			public MsgGetCapabilitiesRequest(){
				parameters = new GetCapabilities();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetCapabilitiesResponse{
			
				[MessageBodyMember]
				[XmlElement("GetCapabilitiesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetCapabilitiesResponse parameters;
			
			public MsgGetCapabilitiesResponse(){
				parameters = new GetCapabilitiesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetHostnameRequest{
			
				[MessageBodyMember]
				[XmlElement("GetHostname", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetHostname parameters;
			
			public MsgGetHostnameRequest(){
				parameters = new GetHostname();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetHostnameResponse{
			
				[MessageBodyMember]
				[XmlElement("GetHostnameResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetHostnameResponse parameters;
			
			public MsgGetHostnameResponse(){
				parameters = new GetHostnameResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetHostnameRequest{
			
				[MessageBodyMember]
				[XmlElement("SetHostname", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetHostname parameters;
			
			public MsgSetHostnameRequest(){
				parameters = new SetHostname();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetHostnameResponse{
			
				[MessageBodyMember]
				[XmlElement("SetHostnameResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetHostnameResponse parameters;
			
			public MsgSetHostnameResponse(){
				parameters = new SetHostnameResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDNSRequest{
			
				[MessageBodyMember]
				[XmlElement("GetDNS", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDNS parameters;
			
			public MsgGetDNSRequest(){
				parameters = new GetDNS();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDNSResponse{
			
				[MessageBodyMember]
				[XmlElement("GetDNSResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDNSResponse parameters;
			
			public MsgGetDNSResponse(){
				parameters = new GetDNSResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetDNSRequest{
			
				[MessageBodyMember]
				[XmlElement("SetDNS", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetDNS parameters;
			
			public MsgSetDNSRequest(){
				parameters = new SetDNS();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetDNSResponse{
			
				[MessageBodyMember]
				[XmlElement("SetDNSResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetDNSResponse parameters;
			
			public MsgSetDNSResponse(){
				parameters = new SetDNSResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetNTPRequest{
			
				[MessageBodyMember]
				[XmlElement("GetNTP", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetNTP parameters;
			
			public MsgGetNTPRequest(){
				parameters = new GetNTP();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetNTPResponse{
			
				[MessageBodyMember]
				[XmlElement("GetNTPResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetNTPResponse parameters;
			
			public MsgGetNTPResponse(){
				parameters = new GetNTPResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetNTPRequest{
			
				[MessageBodyMember]
				[XmlElement("SetNTP", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetNTP parameters;
			
			public MsgSetNTPRequest(){
				parameters = new SetNTP();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetNTPResponse{
			
				[MessageBodyMember]
				[XmlElement("SetNTPResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetNTPResponse parameters;
			
			public MsgSetNTPResponse(){
				parameters = new SetNTPResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDynamicDNSRequest{
			
				[MessageBodyMember]
				[XmlElement("GetDynamicDNS", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDynamicDNS parameters;
			
			public MsgGetDynamicDNSRequest(){
				parameters = new GetDynamicDNS();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetDynamicDNSResponse{
			
				[MessageBodyMember]
				[XmlElement("GetDynamicDNSResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetDynamicDNSResponse parameters;
			
			public MsgGetDynamicDNSResponse(){
				parameters = new GetDynamicDNSResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetDynamicDNSRequest{
			
				[MessageBodyMember]
				[XmlElement("SetDynamicDNS", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetDynamicDNS parameters;
			
			public MsgSetDynamicDNSRequest(){
				parameters = new SetDynamicDNS();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetDynamicDNSResponse{
			
				[MessageBodyMember]
				[XmlElement("SetDynamicDNSResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetDynamicDNSResponse parameters;
			
			public MsgSetDynamicDNSResponse(){
				parameters = new SetDynamicDNSResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetNetworkInterfacesRequest{
			
				[MessageBodyMember]
				[XmlElement("GetNetworkInterfaces", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetNetworkInterfaces parameters;
			
			public MsgGetNetworkInterfacesRequest(){
				parameters = new GetNetworkInterfaces();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetNetworkInterfacesResponse{
			
				[MessageBodyMember]
				[XmlElement("GetNetworkInterfacesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetNetworkInterfacesResponse parameters;
			
			public MsgGetNetworkInterfacesResponse(){
				parameters = new GetNetworkInterfacesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetNetworkInterfacesRequest{
			
				[MessageBodyMember]
				[XmlElement("SetNetworkInterfaces", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetNetworkInterfaces parameters;
			
			public MsgSetNetworkInterfacesRequest(){
				parameters = new SetNetworkInterfaces();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetNetworkInterfacesResponse{
			
				[MessageBodyMember]
				[XmlElement("SetNetworkInterfacesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetNetworkInterfacesResponse parameters;
			
			public MsgSetNetworkInterfacesResponse(){
				parameters = new SetNetworkInterfacesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetNetworkProtocolsRequest{
			
				[MessageBodyMember]
				[XmlElement("GetNetworkProtocols", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetNetworkProtocols parameters;
			
			public MsgGetNetworkProtocolsRequest(){
				parameters = new GetNetworkProtocols();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetNetworkProtocolsResponse{
			
				[MessageBodyMember]
				[XmlElement("GetNetworkProtocolsResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetNetworkProtocolsResponse parameters;
			
			public MsgGetNetworkProtocolsResponse(){
				parameters = new GetNetworkProtocolsResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetNetworkProtocolsRequest{
			
				[MessageBodyMember]
				[XmlElement("SetNetworkProtocols", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetNetworkProtocols parameters;
			
			public MsgSetNetworkProtocolsRequest(){
				parameters = new SetNetworkProtocols();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetNetworkProtocolsResponse{
			
				[MessageBodyMember]
				[XmlElement("SetNetworkProtocolsResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetNetworkProtocolsResponse parameters;
			
			public MsgSetNetworkProtocolsResponse(){
				parameters = new SetNetworkProtocolsResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetNetworkDefaultGatewayRequest{
			
				[MessageBodyMember]
				[XmlElement("GetNetworkDefaultGateway", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetNetworkDefaultGateway parameters;
			
			public MsgGetNetworkDefaultGatewayRequest(){
				parameters = new GetNetworkDefaultGateway();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetNetworkDefaultGatewayResponse{
			
				[MessageBodyMember]
				[XmlElement("GetNetworkDefaultGatewayResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetNetworkDefaultGatewayResponse parameters;
			
			public MsgGetNetworkDefaultGatewayResponse(){
				parameters = new GetNetworkDefaultGatewayResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetNetworkDefaultGatewayRequest{
			
				[MessageBodyMember]
				[XmlElement("SetNetworkDefaultGateway", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetNetworkDefaultGateway parameters;
			
			public MsgSetNetworkDefaultGatewayRequest(){
				parameters = new SetNetworkDefaultGateway();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetNetworkDefaultGatewayResponse{
			
				[MessageBodyMember]
				[XmlElement("SetNetworkDefaultGatewayResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetNetworkDefaultGatewayResponse parameters;
			
			public MsgSetNetworkDefaultGatewayResponse(){
				parameters = new SetNetworkDefaultGatewayResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetZeroConfigurationRequest{
			
				[MessageBodyMember]
				[XmlElement("GetZeroConfiguration", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetZeroConfiguration parameters;
			
			public MsgGetZeroConfigurationRequest(){
				parameters = new GetZeroConfiguration();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetZeroConfigurationResponse{
			
				[MessageBodyMember]
				[XmlElement("GetZeroConfigurationResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetZeroConfigurationResponse parameters;
			
			public MsgGetZeroConfigurationResponse(){
				parameters = new GetZeroConfigurationResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetZeroConfigurationRequest{
			
				[MessageBodyMember]
				[XmlElement("SetZeroConfiguration", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetZeroConfiguration parameters;
			
			public MsgSetZeroConfigurationRequest(){
				parameters = new SetZeroConfiguration();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetZeroConfigurationResponse{
			
				[MessageBodyMember]
				[XmlElement("SetZeroConfigurationResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetZeroConfigurationResponse parameters;
			
			public MsgSetZeroConfigurationResponse(){
				parameters = new SetZeroConfigurationResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetIPAddressFilterRequest{
			
				[MessageBodyMember]
				[XmlElement("GetIPAddressFilter", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetIPAddressFilter parameters;
			
			public MsgGetIPAddressFilterRequest(){
				parameters = new GetIPAddressFilter();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetIPAddressFilterResponse{
			
				[MessageBodyMember]
				[XmlElement("GetIPAddressFilterResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetIPAddressFilterResponse parameters;
			
			public MsgGetIPAddressFilterResponse(){
				parameters = new GetIPAddressFilterResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetIPAddressFilterRequest{
			
				[MessageBodyMember]
				[XmlElement("SetIPAddressFilter", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetIPAddressFilter parameters;
			
			public MsgSetIPAddressFilterRequest(){
				parameters = new SetIPAddressFilter();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetIPAddressFilterResponse{
			
				[MessageBodyMember]
				[XmlElement("SetIPAddressFilterResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetIPAddressFilterResponse parameters;
			
			public MsgSetIPAddressFilterResponse(){
				parameters = new SetIPAddressFilterResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgAddIPAddressFilterRequest{
			
				[MessageBodyMember]
				[XmlElement("AddIPAddressFilter", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public AddIPAddressFilter parameters;
			
			public MsgAddIPAddressFilterRequest(){
				parameters = new AddIPAddressFilter();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgAddIPAddressFilterResponse{
			
				[MessageBodyMember]
				[XmlElement("AddIPAddressFilterResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public AddIPAddressFilterResponse parameters;
			
			public MsgAddIPAddressFilterResponse(){
				parameters = new AddIPAddressFilterResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgRemoveIPAddressFilterRequest{
			
				[MessageBodyMember]
				[XmlElement("RemoveIPAddressFilter", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public RemoveIPAddressFilter parameters;
			
			public MsgRemoveIPAddressFilterRequest(){
				parameters = new RemoveIPAddressFilter();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgRemoveIPAddressFilterResponse{
			
				[MessageBodyMember]
				[XmlElement("RemoveIPAddressFilterResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public RemoveIPAddressFilterResponse parameters;
			
			public MsgRemoveIPAddressFilterResponse(){
				parameters = new RemoveIPAddressFilterResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetAccessPolicyRequest{
			
				[MessageBodyMember]
				[XmlElement("GetAccessPolicy", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetAccessPolicy parameters;
			
			public MsgGetAccessPolicyRequest(){
				parameters = new GetAccessPolicy();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetAccessPolicyResponse{
			
				[MessageBodyMember]
				[XmlElement("GetAccessPolicyResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetAccessPolicyResponse parameters;
			
			public MsgGetAccessPolicyResponse(){
				parameters = new GetAccessPolicyResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetAccessPolicyRequest{
			
				[MessageBodyMember]
				[XmlElement("SetAccessPolicy", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetAccessPolicy parameters;
			
			public MsgSetAccessPolicyRequest(){
				parameters = new SetAccessPolicy();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetAccessPolicyResponse{
			
				[MessageBodyMember]
				[XmlElement("SetAccessPolicyResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetAccessPolicyResponse parameters;
			
			public MsgSetAccessPolicyResponse(){
				parameters = new SetAccessPolicyResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgCreateCertificateRequest{
			
				[MessageBodyMember]
				[XmlElement("CreateCertificate", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public CreateCertificate parameters;
			
			public MsgCreateCertificateRequest(){
				parameters = new CreateCertificate();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgCreateCertificateResponse{
			
				[MessageBodyMember]
				[XmlElement("CreateCertificateResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public CreateCertificateResponse parameters;
			
			public MsgCreateCertificateResponse(){
				parameters = new CreateCertificateResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetCertificatesRequest{
			
				[MessageBodyMember]
				[XmlElement("GetCertificates", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetCertificates parameters;
			
			public MsgGetCertificatesRequest(){
				parameters = new GetCertificates();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetCertificatesResponse{
			
				[MessageBodyMember]
				[XmlElement("GetCertificatesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetCertificatesResponse parameters;
			
			public MsgGetCertificatesResponse(){
				parameters = new GetCertificatesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetCertificatesStatusRequest{
			
				[MessageBodyMember]
				[XmlElement("GetCertificatesStatus", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetCertificatesStatus parameters;
			
			public MsgGetCertificatesStatusRequest(){
				parameters = new GetCertificatesStatus();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetCertificatesStatusResponse{
			
				[MessageBodyMember]
				[XmlElement("GetCertificatesStatusResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetCertificatesStatusResponse parameters;
			
			public MsgGetCertificatesStatusResponse(){
				parameters = new GetCertificatesStatusResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetCertificatesStatusRequest{
			
				[MessageBodyMember]
				[XmlElement("SetCertificatesStatus", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetCertificatesStatus parameters;
			
			public MsgSetCertificatesStatusRequest(){
				parameters = new SetCertificatesStatus();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetCertificatesStatusResponse{
			
				[MessageBodyMember]
				[XmlElement("SetCertificatesStatusResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetCertificatesStatusResponse parameters;
			
			public MsgSetCertificatesStatusResponse(){
				parameters = new SetCertificatesStatusResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgDeleteCertificatesRequest{
			
				[MessageBodyMember]
				[XmlElement("DeleteCertificates", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public DeleteCertificates parameters;
			
			public MsgDeleteCertificatesRequest(){
				parameters = new DeleteCertificates();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgDeleteCertificatesResponse{
			
				[MessageBodyMember]
				[XmlElement("DeleteCertificatesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public DeleteCertificatesResponse parameters;
			
			public MsgDeleteCertificatesResponse(){
				parameters = new DeleteCertificatesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetPkcs10RequestRequest{
			
				[MessageBodyMember]
				[XmlElement("GetPkcs10Request", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetPkcs10Request parameters;
			
			public MsgGetPkcs10RequestRequest(){
				parameters = new GetPkcs10Request();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetPkcs10RequestResponse{
			
				[MessageBodyMember]
				[XmlElement("GetPkcs10RequestResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetPkcs10RequestResponse parameters;
			
			public MsgGetPkcs10RequestResponse(){
				parameters = new GetPkcs10RequestResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgLoadCertificatesRequest{
			
				[MessageBodyMember]
				[XmlElement("LoadCertificates", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public LoadCertificates parameters;
			
			public MsgLoadCertificatesRequest(){
				parameters = new LoadCertificates();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgLoadCertificatesResponse{
			
				[MessageBodyMember]
				[XmlElement("LoadCertificatesResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public LoadCertificatesResponse parameters;
			
			public MsgLoadCertificatesResponse(){
				parameters = new LoadCertificatesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetClientCertificateModeRequest{
			
				[MessageBodyMember]
				[XmlElement("GetClientCertificateMode", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetClientCertificateMode parameters;
			
			public MsgGetClientCertificateModeRequest(){
				parameters = new GetClientCertificateMode();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetClientCertificateModeResponse{
			
				[MessageBodyMember]
				[XmlElement("GetClientCertificateModeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetClientCertificateModeResponse parameters;
			
			public MsgGetClientCertificateModeResponse(){
				parameters = new GetClientCertificateModeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetClientCertificateModeRequest{
			
				[MessageBodyMember]
				[XmlElement("SetClientCertificateMode", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetClientCertificateMode parameters;
			
			public MsgSetClientCertificateModeRequest(){
				parameters = new SetClientCertificateMode();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetClientCertificateModeResponse{
			
				[MessageBodyMember]
				[XmlElement("SetClientCertificateModeResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetClientCertificateModeResponse parameters;
			
			public MsgSetClientCertificateModeResponse(){
				parameters = new SetClientCertificateModeResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetRelayOutputsRequest{
			
				[MessageBodyMember]
				[XmlElement("GetRelayOutputs", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetRelayOutputs parameters;
			
			public MsgGetRelayOutputsRequest(){
				parameters = new GetRelayOutputs();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetRelayOutputsResponse{
			
				[MessageBodyMember]
				[XmlElement("GetRelayOutputsResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public GetRelayOutputsResponse parameters;
			
			public MsgGetRelayOutputsResponse(){
				parameters = new GetRelayOutputsResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetRelayOutputSettingsRequest{
			
				[MessageBodyMember]
				[XmlElement("SetRelayOutputSettings", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetRelayOutputSettings parameters;
			
			public MsgSetRelayOutputSettingsRequest(){
				parameters = new SetRelayOutputSettings();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetRelayOutputSettingsResponse{
			
				[MessageBodyMember]
				[XmlElement("SetRelayOutputSettingsResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetRelayOutputSettingsResponse parameters;
			
			public MsgSetRelayOutputSettingsResponse(){
				parameters = new SetRelayOutputSettingsResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetRelayOutputStateRequest{
			
				[MessageBodyMember]
				[XmlElement("SetRelayOutputState", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetRelayOutputState parameters;
			
			public MsgSetRelayOutputStateRequest(){
				parameters = new SetRelayOutputState();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetRelayOutputStateResponse{
			
				[MessageBodyMember]
				[XmlElement("SetRelayOutputStateResponse", Namespace="http://www.onvif.org/ver10/device/wsdl")]
				public SetRelayOutputStateResponse parameters;
			
			public MsgSetRelayOutputStateResponse(){
				parameters = new SetRelayOutputStateResponse();
				
			}
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDeviceInformation{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDeviceInformationResponse{
		
			[XmlElement]
			public string Manufacturer;
			
			[XmlElement]
			public string Model;
			
			[XmlElement]
			public string FirmwareVersion;
			
			[XmlElement]
			public string SerialNumber;
			
			[XmlElement]
			public string HardwareId;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetSystemDateAndTime{
		
			[XmlElement]
			public SetDateTimeType DateTimeType;
			
			[XmlElement]
			public bool DaylightSavings;
			
			[XmlElement]
			public TimeZone TimeZone;
			
			[XmlElement]
			public DateTime UTCDateTime;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetSystemDateAndTimeResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetSystemDateAndTime{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetSystemDateAndTimeResponse{
		
			[XmlElement]
			public SystemDateTime SystemDateAndTime;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetSystemFactoryDefault{
		
			[XmlElement]
			public string FactoryDefault;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetSystemFactoryDefaultResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class UpgradeSystemFirmware{
		
			[XmlElement]
			public AttachmentData Firmware;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class UpgradeSystemFirmwareResponse{
		
			[XmlElement]
			public string Message;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class StartFirmwareUpgrade{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class StartFirmwareUpgradeResponse{
		
			[XmlElement]
			public string UploadUri;
			
			[XmlElement]
			public string UploadDelay;
			
			[XmlElement]
			public string ExpectedDownTime;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SystemReboot{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SystemRebootResponse{
		
			[XmlElement]
			public string Message;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class RestoreSystem{
		
			[XmlElement("BackupFiles")]
			public BackupFile[] BackupFiles;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class RestoreSystemResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetSystemBackup{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetSystemBackupResponse{
		
			[XmlElement("BackupFiles")]
			public BackupFile[] BackupFiles;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetSystemSupportInformation{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetSystemSupportInformationResponse{
		
			[XmlElement]
			public SupportInformation SupportInformation;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetSystemLog{
		
			[XmlElement]
			public string LogType;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetSystemLogResponse{
		
			[XmlElement]
			public SystemLog SystemLog;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetScopes{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetScopesResponse{
		
			[XmlElement("Scopes")]
			public Scope[] Scopes;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetScopes{
		
			[XmlElement]
			public string[] Scopes;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetScopesResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class AddScopes{
		
			[XmlElement]
			public string[] ScopeItem;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class AddScopesResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class RemoveScopes{
		
			[XmlElement]
			public string[] ScopeItem;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class RemoveScopesResponse{
		
			[XmlElement]
			public string[] ScopeItem;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDiscoveryMode{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDiscoveryModeResponse{
		
			[XmlElement]
			public string DiscoveryMode;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetDiscoveryMode{
		
			[XmlElement]
			public string DiscoveryMode;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetDiscoveryModeResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetRemoteDiscoveryMode{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetRemoteDiscoveryModeResponse{
		
			[XmlElement]
			public string RemoteDiscoveryMode;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetRemoteDiscoveryMode{
		
			[XmlElement]
			public string RemoteDiscoveryMode;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetRemoteDiscoveryModeResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDPAddresses{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDPAddressesResponse{
		
			[XmlElement("DPAddress")]
			public NetworkHost[] DPAddress;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetDPAddresses{
		
			[XmlElement("DPAddress")]
			public NetworkHost[] DPAddress;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetDPAddressesResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetUsers{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetUsersResponse{
		
			[XmlElement("User")]
			public User[] User;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class CreateUsers{
		
			[XmlElement("User")]
			public User[] User;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class CreateUsersResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class DeleteUsers{
		
			[XmlElement]
			public string[] Username;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class DeleteUsersResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetUser{
		
			[XmlElement("User")]
			public User[] User;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetUserResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetWsdlUrl{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetWsdlUrlResponse{
		
			[XmlElement]
			public string WsdlUrl;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetCapabilities{
		
			[XmlElement("Category")]
			public CapabilityCategory[] Category;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetCapabilitiesResponse{
		
			[XmlElement]
			public Capabilities Capabilities;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetHostname{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetHostnameResponse{
		
			[XmlElement]
			public HostnameInformation HostnameInformation;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetHostname{
		
			[XmlElement]
			public string Name;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetHostnameResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDNS{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDNSResponse{
		
			[XmlElement]
			public DNSInformation DNSInformation;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetDNS{
		
			[XmlElement]
			public bool FromDHCP;
			
			[XmlElement]
			public string[] SearchDomain;
			
			[XmlElement("DNSManual")]
			public IPAddress[] DNSManual;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetDNSResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetNTP{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetNTPResponse{
		
			[XmlElement]
			public NTPInformation NTPInformation;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetNTP{
		
			[XmlElement]
			public bool FromDHCP;
			
			[XmlElement("NTPManual")]
			public NetworkHost[] NTPManual;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetNTPResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDynamicDNS{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetDynamicDNSResponse{
		
			[XmlElement]
			public DynamicDNSInformation DynamicDNSInformation;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetDynamicDNS{
		
			[XmlElement]
			public DynamicDNSType Type;
			
			[XmlElement]
			public string Name;
			
			[XmlElement]
			public string TTL;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetDynamicDNSResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetNetworkInterfaces{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetNetworkInterfacesResponse{
		
			[XmlElement("NetworkInterfaces")]
			public NetworkInterface[] NetworkInterfaces;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetNetworkInterfaces{
		
			[XmlElement]
			public string InterfaceToken;
			
			[XmlElement]
			public NetworkInterfaceSetConfiguration NetworkInterface;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetNetworkInterfacesResponse{
		
			[XmlElement]
			public bool RebootNeeded;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetNetworkProtocols{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetNetworkProtocolsResponse{
		
			[XmlElement("NetworkProtocols")]
			public NetworkProtocol[] NetworkProtocols;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetNetworkProtocols{
		
			[XmlElement("NetworkProtocols")]
			public NetworkProtocol[] NetworkProtocols;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetNetworkProtocolsResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetNetworkDefaultGateway{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetNetworkDefaultGatewayResponse{
		
			[XmlElement]
			public NetworkGateway NetworkGateway;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetNetworkDefaultGateway{
		
			[XmlElement]
			public string[] IPv4Address;
			
			[XmlElement]
			public string[] IPv6Address;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetNetworkDefaultGatewayResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetZeroConfiguration{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetZeroConfigurationResponse{
		
			[XmlElement]
			public NetworkZeroConfiguration ZeroConfiguration;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetZeroConfiguration{
		
			[XmlElement]
			public string InterfaceToken;
			
			[XmlElement]
			public bool Enabled;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetZeroConfigurationResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetIPAddressFilter{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetIPAddressFilterResponse{
		
			[XmlElement]
			public IPAddressFilter IPAddressFilter;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetIPAddressFilter{
		
			[XmlElement]
			public IPAddressFilter IPAddressFilter;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetIPAddressFilterResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class AddIPAddressFilter{
		
			[XmlElement]
			public IPAddressFilter IPAddressFilter;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class AddIPAddressFilterResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class RemoveIPAddressFilter{
		
			[XmlElement]
			public IPAddressFilter IPAddressFilter;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class RemoveIPAddressFilterResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetAccessPolicy{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetAccessPolicyResponse{
		
			[XmlElement]
			public BinaryData PolicyFile;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetAccessPolicy{
		
			[XmlElement]
			public BinaryData PolicyFile;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetAccessPolicyResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class CreateCertificate{
		
			[XmlElement]
			public string CertificateID;
			
			[XmlElement]
			public string Subject;
			
			[XmlElement]
			public System.DateTime ValidNotBefore;
			
			[XmlElement]
			public System.DateTime ValidNotAfter;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class CreateCertificateResponse{
		
			[XmlElement]
			public Certificate NvtCertificate;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetCertificates{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetCertificatesResponse{
		
			[XmlElement("NvtCertificate")]
			public Certificate[] NvtCertificate;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetCertificatesStatus{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetCertificatesStatusResponse{
		
			[XmlElement("CertificateStatus")]
			public CertificateStatus[] CertificateStatus;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetCertificatesStatus{
		
			[XmlElement("CertificateStatus")]
			public CertificateStatus[] CertificateStatus;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetCertificatesStatusResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class DeleteCertificates{
		
			[XmlElement]
			public string[] CertificateID;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class DeleteCertificatesResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetPkcs10Request{
		
			[XmlElement]
			public string CertificateID;
			
			[XmlElement]
			public string Subject;
			
			[XmlElement]
			public BinaryData Attributes;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetPkcs10RequestResponse{
		
			[XmlElement]
			public BinaryData Pkcs10Request;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class LoadCertificates{
		
			[XmlElement("NVTCertificate")]
			public Certificate[] NVTCertificate;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class LoadCertificatesResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetClientCertificateMode{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetClientCertificateModeResponse{
		
			[XmlElement]
			public bool Enabled;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetClientCertificateMode{
		
			[XmlElement]
			public bool Enabled;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetClientCertificateModeResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetRelayOutputs{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class GetRelayOutputsResponse{
		
			[XmlElement("RelayOutputs")]
			public RelayOutput[] RelayOutputs;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetRelayOutputSettings{
		
			[XmlElement]
			public string RelayOutputToken;
			
			[XmlElement]
			public RelayOutputSettings Properties;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetRelayOutputSettingsResponse{
		
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetRelayOutputState{
		
			[XmlElement]
			public string RelayOutputToken;
			
			[XmlElement]
			public string LogicalState;
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/device/wsdl")]
		
		public class SetRelayOutputStateResponse{
		
		}
	
}
	