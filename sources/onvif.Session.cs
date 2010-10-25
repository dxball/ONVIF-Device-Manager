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
using System.ServiceModel;
using System.ComponentModel;
using System.Disposables;
using System.Net.NetworkInformation;
using System.ServiceModel.Channels;

using onvifdm.utils;
using nvc.models;

using onvif.types;
using onvif.services.device;
using onvif.services.media;
using onvif.services.events;
using onvif.services.analytics;

using dev = global::onvif.services.device;
using med = global::onvif.services.media;
using evt = global::onvif.services.events;
using anc = global::onvif.services.analytics;
using tt = global::onvif.types;

using net = global::System.Net;

namespace nvc.onvif {
	
	public class Session {
		private static ChannelFactory<Device> s_deviceFactory = null;
		private static ChannelFactory<Media> s_mediaFactory = null;
		private static ChannelFactory<AnalyticsEnginePort> s_analyticsFactory = null;
		private static ChannelFactory<RuleEnginePort> s_ruleEngineFactory = null;
		//private static ChannelFactory<> s_eventsFactory = null;

		public Session(DeviceDescription deviceDescription) {
			this.m_deviceDescription = deviceDescription;
			this.m_transportUri = deviceDescription.Uris.LastOrDefault();
		}
		
		static Session() {
			var binding = new CustomBinding();
			binding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8));
			binding.Elements.Add(new HttpTransportBindingElement() {
				MaxReceivedMessageSize  = 10 * 1024 * 1024
			});

			//var binding = new WSHttpBinding(SecurityMode.None) {
			//    TextEncoding = Encoding.UTF8
			//};
			//binding.MaxReceivedMessageSize = 10 * 1024 * 1024;

			s_deviceFactory = new ChannelFactory<Device>(binding);
			s_mediaFactory = new ChannelFactory<Media>(binding);
			s_analyticsFactory = new ChannelFactory<AnalyticsEnginePort>(binding);
			s_ruleEngineFactory = new ChannelFactory<RuleEnginePort>(binding);
			//s_eventsFactory = new ChannelFactory<evt::>(new WSHttpBinding(SecurityMode.None) {
			//    TextEncoding = Encoding.UTF8
			//});
		}
		
		public static Session Create(DeviceDescription deviceDescription) {
			return new Session(deviceDescription);
		}
		public static Session Create(Uri uri) {
			var session = new Session(null);
			session.m_transportUri = uri;
			return session;
		}

		private DeviceDescription m_deviceDescription;
		public DeviceDescription DeviceDescription {
			get {
				return m_deviceDescription;
			}
		}

		public IObservable<DeviceDescription> GetDeviceDescription() {
			if (m_deviceDescription == null) {
				DeviceObservable dev = null;
				GetDeviceClient().Handle(x => dev = x);
				//TODO: create and fulfill DeviceDescription
			}

			return Observable.Return(m_deviceDescription);
		}

		private Uri m_transportUri;
				
		private IEnumerable<IObservable<object>> GetCapabilitiesImpl(IObserver<Capabilities> observer) {
			DeviceObservable dev = null;
			yield return GetDeviceClient().Handle(x => dev = x);
			DebugHelper.Assert(dev != null);

			Capabilities capabilities = null;
			yield return dev.GetCapabilities().Handle(x => capabilities = x);
			DebugHelper.Assert(capabilities != null);

			if (observer != null) {
				observer.OnNext(capabilities);
			}
		}

		public IObservable<Capabilities> GetCapabilities() {
			return Observable.Iterate<Capabilities>(observer => GetCapabilitiesImpl(observer));
		}

		private IEnumerable<IObservable<object>> GetScopesImpl(IObserver<Scope[]> observer) {
			DeviceObservable dev = null;
			yield return GetDeviceClient().Handle(x => dev = x);
			DebugHelper.Assert(dev != null);

			Scope[] scopes = null;
			yield return dev.GetScopes().Handle(x => scopes = x);
			DebugHelper.Assert(scopes != null);

			if (observer != null) {
				observer.OnNext(scopes);
			}
		}

		public IObservable<Scope[]> GetScopes() {
			return Observable.Iterate<Scope[]>(observer => GetScopesImpl(observer));
		}

		private IEnumerable<IObservable<object>> GetDeviceInformationImpl(IObserver<GetDeviceInformationResponse> observer) {
			DeviceObservable dev = null;
			yield return GetDeviceClient().Handle(x => dev = x);
			DebugHelper.Assert(dev != null);

			GetDeviceInformationResponse info = null;
			yield return dev.GetDeviceInformation().Handle(x => info = x);
			DebugHelper.Assert(info != null);

			if (observer != null) {
				observer.OnNext(info);
			}
		}

		public IObservable<GetDeviceInformationResponse> GetDeviceInformation() {
			return Observable.Iterate<GetDeviceInformationResponse>(observer => GetDeviceInformationImpl(observer));
		}

		public IEnumerable<IObservable<object>> SetScopesImpl(string[] scopes, IObserver<Unit> observer) {
			DeviceObservable dev = null;
			yield return GetDeviceClient().Handle(x => dev = x);
			DebugHelper.Assert(dev != null);

			yield return dev.SetScopes(scopes).Idle();

			if (observer != null) {
				observer.OnNext(new Unit());
			}
		}
		public IObservable<Unit> SetScopes(string[] scopes) {
			return Observable.Iterate<Unit>(observer => SetScopesImpl(scopes, observer));
		}


		private IEnumerable<IObservable<object>> GetDeviceInfoImpl(IObserver<DeviceInfo> observer) {
			DeviceObservable proxy = null;
			yield return GetDeviceClient().Handle(x => proxy = x);

			DeviceInfo info = new DeviceInfo();
			Scope[] scopes = null;
			GetDeviceInformationResponse dev_info_response = null;

			yield return Observable.Merge(
				GetDeviceInformation().Handle(x => dev_info_response = x),
				GetScopes().Handle(x => scopes = x)
			);

			DebugHelper.Assert(scopes != null);
			DebugHelper.Assert(dev_info_response != null);

			if (observer != null) {
				info.Manufacturer = dev_info_response.Manufacturer;
				info.Model = dev_info_response.Model;
				info.FirmwareVersion = dev_info_response.FirmwareVersion;
				info.SerialNumber = dev_info_response.SerialNumber;
				info.HardwareId = dev_info_response.HardwareId;
				info.Name = NvcHelper.GetName(scopes.Select(y => y.ScopeItem));
				observer.OnNext(info);
			}
		}

		public IObservable<DeviceInfo> GetDeviceInfo() {
			return Observable.Iterate<DeviceInfo>(observer => GetDeviceInfoImpl(observer));
		}

		private IEnumerable<IObservable<object>> GetProfilesImpl(IObserver<med::Profile[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			med::Profile[] profiles = null;
			yield return media.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			if (observer != null) {
				observer.OnNext(profiles);
			}
		}

		public IObservable<med::Profile[]> GetProfiles() {
			return Observable.Iterate<med::Profile[]>(observer => GetProfilesImpl(observer));
		}

		private IEnumerable<IObservable<object>> CreateProfileImpl(string profileName, string profileToken, IObserver<med::Profile> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			med::Profile profile = null;
			yield return media.CreateProfile(profileName, profileToken).Handle(x => profile = x);
			DebugHelper.Assert(profile != null);

			if (observer != null) {
				observer.OnNext(profile);
			}
		}

		public IObservable<med::Profile> CreateProfile(string profileName, string profileToken) {
			return Observable.Iterate<med::Profile>(observer => CreateProfileImpl(profileName, profileToken, observer));
		}

		private IEnumerable<IObservable<object>> AddVideoSourceConfigurationImpl(string profileToken, string vscToken) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			yield return media.AddVideoSourceConfiguration(profileToken, vscToken).Idle();
		}

		public IObservable<med::Profile> AddVideoSourceConfiguration(string profileToken, string vscToken) {
			return Observable.Iterate<med::Profile>(observer => AddVideoSourceConfigurationImpl(profileToken, vscToken));
		}

		private IEnumerable<IObservable<object>> GetVideoSourcesImpl(IObserver<med::VideoSource[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			med::VideoSource[] vsources = null;
			yield return media.GetVideoSources().Handle(x => vsources = x);
			DebugHelper.Assert(vsources != null);

			if (observer != null) {
				observer.OnNext(vsources);
			}
		}

		public IObservable<med::VideoSource[]> GetVideoSources() {
			return Observable.Iterate<med::VideoSource[]>(observer => GetVideoSourcesImpl(observer));
		}

		private IEnumerable<IObservable<object>> GetVideoSourceConfigurationsImpl(IObserver<med::VideoSourceConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			med::VideoSourceConfiguration[] vscs = null;
			yield return media.GetVideoSourceConfigurations().Handle(x => vscs = x);
			DebugHelper.Assert(vscs != null);

			if (observer != null) {
				observer.OnNext(vscs);
			}
		}

		public IObservable<med::VideoSourceConfiguration[]> GetVideoSourceConfigurations() {
			return Observable.Iterate<med::VideoSourceConfiguration[]>(observer => GetVideoSourceConfigurationsImpl(observer));
		}

		//Video Encoder functions

		public IObservable<med::VideoEncoderConfiguration[]> GetVideoEncoderConfigurations() {
			return Observable.Iterate<med::VideoEncoderConfiguration[]>(observer => GetVideoEncoderConfigurationsImpl(observer));
		}

		public IObservable<med::VideoEncoderConfiguration[]> GetCompatibleVideoEncoderConfigurations(string profileToken) {
			return Observable.Iterate<med::VideoEncoderConfiguration[]>(observer => GetCompatibleVideoEncoderConfigurationsImpl(profileToken, observer));
		}

		public IObservable<Unit> AddVideoEncoderConfiguration(string profileToken, string vecToken) {
			return Observable.Iterate<Unit>(observer => AddVideoEncoderConfigurationImpl(profileToken, vecToken));
		}

		public IObservable<Unit> SetVideoEncoderConfiguration(med::VideoEncoderConfiguration vec, bool forcePersistance) {
			return Observable.Iterate<Unit>(observer => SetVideoEncoderConfigurationImpl(vec, forcePersistance));
		}

		private IEnumerable<IObservable<object>> GetVideoEncoderConfigurationsImpl(IObserver<med::VideoEncoderConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			med::VideoEncoderConfiguration[] vecs = null;
			yield return media.GetVideoEncoderConfigurations().Handle(x => vecs = x);
			DebugHelper.Assert(vecs != null);

			if (observer != null) {
				observer.OnNext(vecs);
			}
		}

		private IEnumerable<IObservable<object>> GetCompatibleVideoEncoderConfigurationsImpl(string profileToken, IObserver<med::VideoEncoderConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			med::VideoEncoderConfiguration[] vecs = null;
			yield return media.GetCompatibleVideoEncoderConfigurations(profileToken).Handle(x => vecs = x);
			DebugHelper.Assert(vecs != null);

			if (observer != null) {
				observer.OnNext(vecs);
			}
		}

		private IEnumerable<IObservable<object>> AddVideoEncoderConfigurationImpl(string profileToken, string vecToken) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			yield return media.AddVideoEncoderConfiguration(profileToken, vecToken).Idle();
		}

		private IEnumerable<IObservable<object>> SetVideoEncoderConfigurationImpl(med::VideoEncoderConfiguration vec, bool forcePersistance) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			yield return media.SetVideoEncoderConfiguration(vec, forcePersistance).Idle();
		}

		//Video Analytics functions

		public IObservable<med::VideoAnalyticsConfiguration[]> GetVideoAnalyticsConfigurations() {
			return Observable.Iterate<med::VideoAnalyticsConfiguration[]>(observer => GetVideoAnalyticsConfigurationsImpl(observer));
		}

		public IObservable<med::VideoAnalyticsConfiguration[]> GetCompatibleVideoAnalyticsConfigurations(string profileToken) {
			return Observable.Iterate<med::VideoAnalyticsConfiguration[]>(observer => GetCompatibleVideoAnalyticsConfigurationsImpl(profileToken, observer));
		}

		public IObservable<med::Profile> AddVideoAnalyticsConfiguration(string profileToken, string vacToken) {
			return Observable.Iterate<med::Profile>(observer => AddVideoAnalyticsConfigurationImpl(profileToken, vacToken));
		}

		private IEnumerable<IObservable<object>> GetVideoAnalyticsConfigurationsImpl(IObserver<med::VideoAnalyticsConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			med::VideoAnalyticsConfiguration[] vecs = null;
			yield return media.GetVideoAnalyticsConfigurations().Handle(x => vecs = x);
			DebugHelper.Assert(vecs != null);

			if (observer != null) {
				observer.OnNext(vecs);
			}
		}

		private IEnumerable<IObservable<object>> GetCompatibleVideoAnalyticsConfigurationsImpl(string profileToken, IObserver<med::VideoAnalyticsConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			med::VideoAnalyticsConfiguration[] vecs = null;
			yield return media.GetCompatibleVideoAnalyticsConfigurations(profileToken).Handle(x => vecs = x);
			DebugHelper.Assert(vecs != null);

			if (observer != null) {
				observer.OnNext(vecs);
			}
		}

		private IEnumerable<IObservable<object>> AddVideoAnalyticsConfigurationImpl(string profileToken, string vacToken) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			yield return media.AddVideoAnalyticsConfiguration(profileToken, vacToken).Idle();
		}

		//Network functions

		private IEnumerable<IObservable<object>> GetNetworkInterfacesImpl(IObserver<tt::NetworkInterface[]> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			global::onvif.types.NetworkInterface[] nics = null;
			yield return device.GetNetworkInterfaces().Handle(x => nics = x);
			DebugHelper.Assert(nics != null);

			if (observer != null) {
				observer.OnNext(nics);
			}
		}

		public IObservable<tt::NetworkInterface[]> GetNetworkInterfaces() {
			return Observable.Iterate<tt::NetworkInterface[]>(observer => GetNetworkInterfacesImpl(observer));
		}

		private IEnumerable<IObservable<object>> GetDNSImpl(IObserver<DNSInformation> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			DNSInformation dns = null;
			yield return device.GetDNS().Handle(x => dns = x);
			DebugHelper.Assert(dns != null);

			if (observer != null) {
				observer.OnNext(dns);
			}
		}

		public IObservable<DNSInformation> GetDNS() {
			return Observable.Iterate<DNSInformation>(observer => GetDNSImpl(observer));
		}


		private IEnumerable<IObservable<object>> SetDNSImpl(bool fromDHCP, tt::IPAddress[] DNSManual, IObserver<Unit> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			yield return device.SetDNS(fromDHCP, null, DNSManual).Idle();

			//if (observer != null) {
			//    observer.OnNext(new Unit());
			//}
		}

		public IObservable<Unit> SetDNS(bool fromDHCP, tt::IPAddress[] DNSManual) {
			return Observable.Iterate<Unit>(observer => SetDNSImpl(fromDHCP, DNSManual, observer));
		}


		private IEnumerable<IObservable<object>> SetNetworkDefaultGatewayImpl(string[] ipv4Addresses, string[] ipv6Addresses, IObserver<Unit> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			yield return device.SetNetworkDefaultGateway(ipv4Addresses, ipv6Addresses).Idle();

			//if (observer != null) {
			//    observer.OnNext(new Unit());
			//}
		}

		public IObservable<Unit> SetNetworkDefaultGateway(string[] ipv4Addresses, string[] ipv6Addresses) {
			return Observable.Iterate<Unit>(observer => SetNetworkDefaultGatewayImpl(ipv4Addresses, ipv6Addresses, observer));
		}

		private IEnumerable<IObservable<object>> SetNetworkInterfacesImpl(string nicToken, NetworkInterfaceSetConfiguration nicConfig, IObserver<bool> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			bool isRebootNeeded = false;
			yield return device.SetNetworkInterfaces(nicToken, nicConfig).Handle(x => isRebootNeeded = x);

			if (observer != null) {
				observer.OnNext(isRebootNeeded);
			}
		}

		public IObservable<bool> SetNetworkInterfaces(string nicToken, NetworkInterfaceSetConfiguration nicConfig) {
			return Observable.Iterate<bool>(observer => SetNetworkInterfacesImpl(nicToken, nicConfig, observer));
		}

		private IEnumerable<IObservable<object>> SystemRebootImpl(IObserver<string> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);
			string message = null;
			yield return device.SystemReboot().Handle(x=>message = x);

			if (observer != null) {
			    observer.OnNext(message);
			}
		}

		public IObservable<string> SystemReboot() {
			return Observable.Iterate<string>(observer => SystemRebootImpl(observer));
		}


		private IEnumerable<IObservable<object>> GetStreamUriImpl(med::StreamSetup streamSetup, string profileToken, IObserver<string> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			string uri = null;
			yield return media.GetStreamUri(streamSetup, profileToken).Handle(x => uri = x.Uri);
			DebugHelper.Assert(uri != null);

			if (observer != null) {
				observer.OnNext(uri);
			}
		}

		public IObservable<string> GetStreamUri(med::StreamSetup streamSetup, string profileToken) {
			return Observable.Iterate<string>(observer => GetStreamUriImpl(streamSetup, profileToken, observer));
		}

		private IEnumerable<IObservable<object>> GetNetworkDefaultGatewayImpl(IObserver<NetworkGateway> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			NetworkGateway gateway = null;
			yield return device.GetNetworkDefaultGateway().Handle(x => gateway = x);
			DebugHelper.Assert(gateway != null);

			if (observer != null) {
				observer.OnNext(gateway);
			}
		}

		public IObservable<NetworkGateway> GetNetworkDefaultGateway() {
			return Observable.Iterate<NetworkGateway>(observer => GetNetworkDefaultGatewayImpl(observer));
		}


		private IEnumerable<IObservable<object>> GetNetworkStatusImpl(IObserver<NetworkStatus> observer) {

			tt::NetworkInterface[] nics = null;
			DNSInformation dns = null;
			yield return Observable.Merge(
				GetNetworkInterfaces().Handle(x => nics = x),
				GetDNS().Handle(x => dns = x)
			);
			DebugHelper.Assert(nics != null);
			DebugHelper.Assert(dns != null);

			var netStat = new NetworkStatus();
			if (dns.FromDHCP && dns.DNSFromDHCP!=null && dns.DNSFromDHCP.Count() > 0) {
				netStat.dns = net::IPAddress.Parse(dns.DNSFromDHCP[0].IPv4Address);
			} else if (!dns.FromDHCP && dns.DNSManual != null && dns.DNSManual.Count() > 0) {
				netStat.dns = net::IPAddress.Parse(dns.DNSManual[0].IPv4Address);
			}
			DebugHelper.Assert(netStat.dns != null);

			var nic = nics.Where(x => x.Enabled).FirstOrDefault();
			if (nic != null) {
				var nic_cfg = nic.IPv4.Config;
				//networkSettngs.m_token = t[0].token;
				netStat.mac = PhysicalAddress.Parse(nic.Info.HwAddress.Replace(':', '-'));
				//netStat.m_dhcp = nic.IPv4.Config.DHCP;

				if (nic_cfg.FromDHCP != null) {
					//DebugHelper.Assert(nic_cfg.FromDHCP != null);
					DebugHelper.Assert(nic_cfg.FromDHCP.Address != null);
					netStat.ip = net::IPAddress.Parse(nic_cfg.FromDHCP.Address);
					netStat.subnetPrefix = nic_cfg.FromDHCP.PrefixLength;
				} else if (nic_cfg.Manual != null && nic_cfg.Manual.Count() > 0) {
					netStat.ip = net::IPAddress.Parse(nic_cfg.Manual[0].Address);
					netStat.subnetPrefix = nic_cfg.Manual[0].PrefixLength;
				}
			}
			
			if (observer != null) {
				observer.OnNext(netStat);
			}
		}

		public IObservable<NetworkStatus> GetNetworkStatus() {
			return Observable.Iterate<NetworkStatus>(observer => GetNetworkStatusImpl(observer));
		}

		private IEnumerable<IObservable<object>> GetNetworkSettingsImpl(IObserver<NetworkSettings> observer) {

			NetworkGateway gateway = null;
			DNSInformation dns = null;
			tt::NetworkInterface[] nics = null;

			yield return Observable.Merge(
				GetNetworkDefaultGateway().Handle(x => gateway = x).IgnoreError(),
				GetDNS().Handle(x => dns = x).IgnoreError(),
				GetNetworkInterfaces().Handle(x => nics = x)
			);

			DebugHelper.Assert(gateway != null);
			DebugHelper.Assert(dns != null);
			DebugHelper.Assert(nics != null);

			var netSettings = new NetworkSettings();
			
			if (gateway!=null && gateway.IPv4Address != null && gateway.IPv4Address.Count() > 0) {
				net::IPAddress defaultGateway = net::IPAddress.None;
				net::IPAddress.TryParse(gateway.IPv4Address[0], out defaultGateway);
				netSettings.defaultGateway = defaultGateway;
			}

			if (dns!=null && dns.DNSManual != null && dns.DNSManual.Count() > 0) {
				netSettings.staticDns = net::IPAddress.Parse(dns.DNSManual[0].IPv4Address);
			} else if (dns!=null && dns.DNSFromDHCP != null && dns.DNSFromDHCP.Count() > 0) {
				netSettings.staticDns = net::IPAddress.Parse(dns.DNSFromDHCP[0].IPv4Address);
			}

			var nic = nics.Where(x => x.Enabled).FirstOrDefault();
			if (nic != null) {
				var nic_cfg = nic.IPv4.Config;
				//networkSettngs.m_token = t[0].token;
				//networkSettngs.m_mac = PhysicalAddress.Parse(nic.Info.HwAddress.Replace(':','-'));
				netSettings.dhcp = nic.IPv4.Config.DHCP;

				if (nic_cfg.Manual.Count() > 0) {
					netSettings.staticIp = net::IPAddress.Parse(nic_cfg.Manual[0].Address);
					netSettings.subnetPrefix = nic_cfg.Manual[0].PrefixLength;
				} else if (nic_cfg.FromDHCP != null) {
					netSettings.staticIp = net::IPAddress.Parse(nic_cfg.FromDHCP.Address);
					netSettings.subnetPrefix = nic_cfg.FromDHCP.PrefixLength;
				}
			}
			
			if (observer != null) {
				observer.OnNext(netSettings);
			}
		}

		public IObservable<NetworkSettings> GetNetworkSettings() {
			return Observable.Iterate<NetworkSettings>(observer => GetNetworkSettingsImpl(observer));
		}

		public static IObservable<DeviceObservable> GetDeviceClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = s_deviceFactory.CreateChannel(endpointAddr);
			return Observable.Return(new DeviceObservable(proxy));
		}

		public static IObservable<MediaObservable> GetMediaClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = s_mediaFactory.CreateChannel(endpointAddr);
			return Observable.Return(new MediaObservable(proxy));
		}

		public static IObservable<AnalyticsObservable> GetAnalyticsClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = s_analyticsFactory.CreateChannel(endpointAddr);
			return Observable.Return(new AnalyticsObservable(proxy));
		}

		public static IObservable<RuleEngineObservable> GetRuleEngineClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = s_ruleEngineFactory.CreateChannel(endpointAddr);
			return Observable.Return(new RuleEngineObservable(proxy));
		}

		//public static IObservable<EventsObservable> GetEventsClient(Uri uri) {
		//    var endpointAddr = new EndpointAddress(uri);
		//    var proxy = s_eventsFactory.CreateChannel(endpointAddr);
		//    return Observable.Return(new EventsObservable(proxy));
		//}

		public IObservable<DeviceObservable> GetDeviceClient() {
			return GetDeviceClient(m_transportUri);
		}
		
		public IObservable<MediaObservable> GetMediaClient() {
			return Observable.Iterate<MediaObservable>(observer => GetMediaClientImpl(observer));
		}

		public IObservable<AnalyticsObservable> GetAnalyticsClient() {
			return Observable.Iterate<AnalyticsObservable>(observer => GetAnalyticsClientImpl(observer));
		}

		public IObservable<RuleEngineObservable> GetRuleEngineClient() {
			return Observable.Iterate<RuleEngineObservable>(observer => GetRuleEngineClientImpl(observer));
		}

		//public IObservable<EventsObservable> GetEventsClient() {
		//    return Observable.Iterate<EventsObservable>(observer => GetEventsClientImpl(observer));
		//}

		private IEnumerable<IObservable<object>> GetMediaClientImpl(IObserver<MediaObservable> observer) {
			MediaObservable media = null;
			Capabilities caps = null;
			yield return GetCapabilities().Handle(x => caps = x);
			DebugHelper.Assert(caps != null);

			if (caps.Media == null) {
				throw new Exception("not supported");
			}

			yield return GetMediaClient(new Uri(caps.Media.XAddr)).Handle(x => media = x);
			DebugHelper.Assert(media != null);

			if (observer != null) {
				observer.OnNext(media);
			}
		}

		private IEnumerable<IObservable<object>> GetAnalyticsClientImpl(IObserver<AnalyticsObservable> observer) {
			AnalyticsObservable analytics = null;
			Capabilities caps = null;
			yield return GetCapabilities().Handle(x => caps = x);
			DebugHelper.Assert(caps != null);

			if (caps.Analytics == null) {
				throw new Exception("not supported");
			}

			yield return GetAnalyticsClient(new Uri(caps.Analytics.XAddr)).Handle(x => analytics = x);
			DebugHelper.Assert(analytics != null);

			if (observer != null) {
				observer.OnNext(analytics);
			}
		}

		private IEnumerable<IObservable<object>> GetRuleEngineClientImpl(IObserver<RuleEngineObservable> observer) {
			RuleEngineObservable RuleEngine = null;
			Capabilities caps = null;
			yield return GetCapabilities().Handle(x => caps = x);
			DebugHelper.Assert(caps != null);

			if (caps.Analytics == null) {
				throw new Exception("not supported");
			}

			yield return GetRuleEngineClient(new Uri(caps.Analytics.XAddr)).Handle(x => RuleEngine = x);
			DebugHelper.Assert(RuleEngine != null);

			if (observer != null) {
				observer.OnNext(RuleEngine);
			}
		}

		//private IEnumerable<IObservable<object>> GetEventsClientImpl(IObserver<EventsObservable> observer) {
		//    EventsObservable events = null;
		//    Capabilities caps = null;
		//    yield return GetCapabilities().Handle(x => caps = x);
		//    DebugHelper.Assert(caps != null);

		//    if (caps.Events == null) {
		//        throw new Exception("not supported");
		//    }

		//    yield return GetEventsClient(new Uri(caps.Events.XAddr)).Handle(x => events = x);
		//    DebugHelper.Assert(events != null);

		//    if (observer != null) {
		//        observer.OnNext(events);
		//    }
		//}


		private IEnumerable<IObservable<object>> CreateDefaultProfileImpl(string videoSourceToken, IObserver<med::Profile> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			med::VideoSourceConfiguration[] vscs = null;
			yield return GetVideoSourceConfigurations().Handle(x => vscs = x);

			var vsc = vscs.Where(x => x.SourceToken == videoSourceToken).FirstOrDefault();

			if (vsc == null) {
				throw new Exception("video source configuration not found");

			}

			med::Profile profile = null;
			yield return CreateProfile(videoSourceToken, NvcHelper.GetChannelProfileToken(videoSourceToken)).Handle(x => profile = x);
			yield return AddVideoSourceConfiguration(profile.token, vsc.token);
			profile.VideoSourceConfiguration = vsc;

			yield return AddDefaultVideoEncoder(profile.token).Handle(x => profile.VideoEncoderConfiguration = x);

			if (observer != null) {
				observer.OnNext(profile);
			}
		}

		public IObservable<med::Profile> CreateDefaultProfile(string videoSourceToken) {
			return Observable.Iterate<med::Profile>(observer => CreateDefaultProfileImpl(videoSourceToken, observer));
		}

		private IEnumerable<IObservable<object>> AddDefaultVideoEncoderImpl(string profileToken, IObserver<med::VideoEncoderConfiguration> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			med::VideoEncoderConfiguration[] vecs = null;
			yield return GetCompatibleVideoEncoderConfigurations(profileToken).Handle(x => vecs = x);

			med::VideoEncoderConfiguration vec = null;
			//vec = vecs.Where(x => x.Encoding == med::VideoEncoding.H264).FirstOrDefault();
			vec = vecs.FirstOrDefault();
			if (vec == null && vecs.Length > 0) {
				vec = vecs[0];
			}
			if (vec != null) {
				yield return AddVideoEncoderConfiguration(profileToken, vec.token).Idle();
			}


			if (observer != null) {
				observer.OnNext(vec);
			}
		}

		public IObservable<med::VideoEncoderConfiguration> AddDefaultVideoEncoder(string profileToken) {
			return Observable.Iterate<med::VideoEncoderConfiguration>(observer => AddDefaultVideoEncoderImpl(profileToken, observer));
		}

		//public IObservable<MediaObservable> GetMediaClient() {
		//    //return GetMediaClient(m_transportUri);
		//}

		//public MediaObservable CreateMediaClient() {
		//    return CreateMediaClient(new Uri(capabilities.Media.XAddr));
		//}

		public DeviceDescription deviceDescription {
			get {
				return m_deviceDescription;
			}
		}

		//public DeviceObservable device{
		//    get{
		//        return CreateDeviceClient();
		//    }
		//}

		//public MediaObservable media{
		//    get{
		//        return CreateMediaClient();
		//    }
		//}
	}
}
