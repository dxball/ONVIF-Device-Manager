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

using onvif.services.device;
using onvif.services.media;
using nvc.utils;
using nvc.models;

using dev = global::onvif.services.device;
using med = global::onvif.services.media;
using net = global::System.Net;
using System.Net.NetworkInformation;
namespace nvc.onvif {

	public static class ext {
		public static IObservable<Object> Handle<T>(this IObservable<T> observable, Action<T> act) {
			return observable.Do(act).TakeLast(0).Select(x => new object());
		}

		public static IObservable<Object> Idle<T>(this IObservable<T> observable) {
			return Observable.CreateWithDisposable<Object>(observer => {
				return observable.Subscribe(_ => {}, observer.OnError, observer.OnCompleted);
			});
		}
		//public static IObservable<Unit> Process<T>(this IObservable<T> observable, Action<T> act) {
		//    return observable.Do(act).TakeLast(0).Select(x => new Unit());
		//}
		public static IObservable<T> IgnoreError<T>(this IObservable<T> observable) {
			return Observable.CreateWithDisposable<T>(observer => {
				return observable.Subscribe(observer.OnNext, err => observer.OnCompleted(), observer.OnCompleted);
			});
		}
	}


	public class Session {
		public Session(DeviceDescription deviceDescription) {
			this.m_deviceDescription = deviceDescription;
			this.m_transportUri = deviceDescription.Uris.LastOrDefault();
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

		private static ChannelFactory<Device> m_deviceFactory =
			new ChannelFactory<Device>(new WSHttpBinding(SecurityMode.None) {
				TextEncoding = Encoding.UTF8
			});
		private static ChannelFactory<Media> m_mediaFactory = new ChannelFactory<Media>(new WSHttpBinding(SecurityMode.None));

		private IEnumerable<IObservable<Object>> GetCapabilitiesImpl(IObserver<Capabilities> observer) {
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

		private IEnumerable<IObservable<Object>> GetScopesImpl(IObserver<Scope[]> observer) {
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

		private IEnumerable<IObservable<Object>> GetDeviceInformationImpl(IObserver<GetDeviceInformationResponse> observer) {
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


		private IEnumerable<IObservable<Object>> GetDeviceInfoImpl(IObserver<DeviceInfo> observer) {
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

		private IEnumerable<IObservable<Object>> GetProfilesImpl(IObserver<Profile[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			Profile[] profiles = null;
			yield return media.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			if (observer != null) {
				observer.OnNext(profiles);
			}
		}

		public IObservable<Profile[]> GetProfiles() {
			return Observable.Iterate<Profile[]>(observer => GetProfilesImpl(observer));
		}

		private IEnumerable<IObservable<Object>> CreateProfileImpl(string profileName, string profileToken, IObserver<Profile> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			Profile profile = null;
			yield return media.CreateProfile(profileName, profileToken).Handle(x => profile = x);
			DebugHelper.Assert(profile != null);

			if (observer != null) {
				observer.OnNext(profile);
			}
		}

		public IObservable<Profile> CreateProfile(string profileName, string profileToken) {
			return Observable.Iterate<Profile>(observer => CreateProfileImpl(profileName, profileToken, observer));
		}

		private IEnumerable<IObservable<Object>> AddVideoEncoderConfigurationImpl(string profileToken, string vecToken) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			yield return media.AddVideoEncoderConfiguration(profileToken, vecToken).Idle();
		}

		public IObservable<Profile> AddVideoEncoderConfiguration(string profileToken, string vecToken) {
			return Observable.Iterate<Profile>(observer => AddVideoEncoderConfigurationImpl(profileToken, vecToken));
		}


		private IEnumerable<IObservable<Object>> AddVideoSourceConfigurationImpl(string profileToken, string videoSourceToken) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			yield return media.AddVideoSourceConfiguration(profileToken, videoSourceToken).Idle();
		}

		public IObservable<Profile> AddVideoSourceConfiguration(string profileToken, string videoSourceToken) {
			return Observable.Iterate<Profile>(observer => AddVideoSourceConfigurationImpl(profileToken, videoSourceToken));
		}

		private IEnumerable<IObservable<Object>> GetVideoSourcesImpl(IObserver<med::VideoSource[]> observer) {
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


		private IEnumerable<IObservable<Object>> GetVideoSourceConfigurationsImpl(IObserver<med::VideoSourceConfiguration[]> observer) {
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


		private IEnumerable<IObservable<Object>> GetVideoEncoderConfigurationsImpl(IObserver<med::VideoEncoderConfiguration[]> observer) {
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

		public IObservable<med::VideoEncoderConfiguration[]> GetVideoEncoderConfigurations() {
			return Observable.Iterate<med::VideoEncoderConfiguration[]>(observer => GetVideoEncoderConfigurationsImpl(observer));
		}

		private IEnumerable<IObservable<Object>> GetCompatibleVideoEncoderConfigurationsImpl(string profileToken, IObserver<med::VideoEncoderConfiguration[]> observer) {
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

		public IObservable<med::VideoEncoderConfiguration[]> GetCompatibleVideoEncoderConfigurations(string profileToken) {
			return Observable.Iterate<med::VideoEncoderConfiguration[]>(observer => GetCompatibleVideoEncoderConfigurationsImpl(profileToken, observer));
		}


		private IEnumerable<IObservable<Object>> GetNetworkInterfacesImpl(IObserver<dev::NetworkInterface[]> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			dev::NetworkInterface[] nics = null;
			yield return device.GetNetworkInterfaces().Handle(x => nics = x);
			DebugHelper.Assert(nics != null);

			if (observer != null) {
				observer.OnNext(nics);
			}
		}

		public IObservable<dev::NetworkInterface[]> GetNetworkInterfaces() {
			return Observable.Iterate<dev::NetworkInterface[]>(observer => GetNetworkInterfacesImpl(observer));
		}

		private IEnumerable<IObservable<Object>> GetDNSImpl(IObserver<dev::DNSInformation> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			dev::DNSInformation dns = null;
			yield return device.GetDNS().Handle(x => dns = x);
			DebugHelper.Assert(dns != null);

			if (observer != null) {
				observer.OnNext(dns);
			}
		}

		public IObservable<dev::DNSInformation> GetDNS() {
			return Observable.Iterate<dev::DNSInformation>(observer => GetDNSImpl(observer));
		}


		private IEnumerable<IObservable<Object>> SetDNSImpl(bool fromDHCP, dev::IPAddress[] DNSManual, IObserver<Unit> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			yield return device.SetDNS(fromDHCP, null, DNSManual).Idle();

			//if (observer != null) {
			//    observer.OnNext(new Unit());
			//}
		}

		public IObservable<Unit> SetDNS(bool fromDHCP, dev::IPAddress[] DNSManual) {
			return Observable.Iterate<Unit>(observer => SetDNSImpl(fromDHCP, DNSManual, observer));
		}


		private IEnumerable<IObservable<Object>> SetNetworkDefaultGatewayImpl(string[] ipv4Addresses, string[] ipv6Addresses, IObserver<Unit> observer) {
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

		private IEnumerable<IObservable<Object>> SetNetworkInterfacesImpl(string nicToken, dev::NetworkInterfaceSetConfiguration nicConfig, IObserver<bool> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			bool isRebootNeeded = false;
			yield return device.SetNetworkInterfaces(nicToken, nicConfig).Handle(x => isRebootNeeded = x);

			if (observer != null) {
				observer.OnNext(isRebootNeeded);
			}
		}

		public IObservable<bool> SetNetworkInterfaces(string nicToken, dev::NetworkInterfaceSetConfiguration nicConfig) {
			return Observable.Iterate<bool>(observer => SetNetworkInterfacesImpl(nicToken, nicConfig, observer));
		}

		private IEnumerable<IObservable<Object>> SystemRebootImpl(IObserver<Unit> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			yield return device.SystemReboot().Idle();

			//if (observer != null) {
			//    observer.OnNext(new Unit());
			//}
		}

		public IObservable<Unit> SystemReboot() {
			return Observable.Iterate<Unit>(observer => SystemRebootImpl(observer));
		}


		private IEnumerable<IObservable<Object>> GetStreamUriImpl(StreamSetup streamSetup, string profileToken, IObserver<string> observer) {
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

		public IObservable<string> GetStreamUri(StreamSetup streamSetup, string profileToken) {
			return Observable.Iterate<string>(observer => GetStreamUriImpl(streamSetup, profileToken, observer));
		}

		private IEnumerable<IObservable<Object>> GetNetworkDefaultGatewayImpl(IObserver<dev::NetworkGateway> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			DebugHelper.Assert(device != null);

			dev::NetworkGateway gateway = null;
			yield return device.GetNetworkDefaultGateway().Handle(x => gateway = x);
			DebugHelper.Assert(gateway != null);

			if (observer != null) {
				observer.OnNext(gateway);
			}
		}

		public IObservable<dev::NetworkGateway> GetNetworkDefaultGateway() {
			return Observable.Iterate<dev::NetworkGateway>(observer => GetNetworkDefaultGatewayImpl(observer));
		}


		private IEnumerable<IObservable<Object>> GetNetworkStatusImpl(IObserver<NetworkStatus> observer) {

			dev::NetworkInterface[] nics = null;
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

		private IEnumerable<IObservable<Object>> GetNetworkSettingsImpl(IObserver<NetworkSettings> observer) {

			dev::NetworkGateway gateway = null;
			dev::DNSInformation dns = null;
			dev::NetworkInterface[] nics = null;

			yield return Observable.Merge(
				GetNetworkDefaultGateway().Handle(x => gateway = x),
				GetDNS().Handle(x => dns = x),
				GetNetworkInterfaces().Handle(x => nics = x)
			);

			DebugHelper.Assert(gateway != null);
			DebugHelper.Assert(dns != null);
			DebugHelper.Assert(nics != null);

			var netSettngs = new NetworkSettings();

			if (gateway.IPv4Address != null && gateway.IPv4Address.Count() > 0) {
				netSettngs.defaultGateway = net::IPAddress.Parse(gateway.IPv4Address[0]);
			}

			if (dns.DNSManual != null && dns.DNSManual.Count() > 0) {
				netSettngs.staticDns = net::IPAddress.Parse(dns.DNSManual[0].IPv4Address);
			} else if (dns.DNSFromDHCP != null && dns.DNSFromDHCP.Count() > 0) {
				netSettngs.staticDns = net::IPAddress.Parse(dns.DNSFromDHCP[0].IPv4Address);
			}

			var nic = nics.Where(x => x.Enabled).FirstOrDefault();
			if (nic != null) {
				var nic_cfg = nic.IPv4.Config;
				//networkSettngs.m_token = t[0].token;
				//networkSettngs.m_mac = PhysicalAddress.Parse(nic.Info.HwAddress.Replace(':','-'));
				netSettngs.dhcp = nic.IPv4.Config.DHCP;

				if (nic_cfg.Manual.Count() > 0) {
					netSettngs.staticIp = net::IPAddress.Parse(nic_cfg.Manual[0].Address);
					netSettngs.subnetPrefix = nic_cfg.Manual[0].PrefixLength;
				} else if (nic_cfg.FromDHCP != null) {
					netSettngs.staticIp = net::IPAddress.Parse(nic_cfg.FromDHCP.Address);
					netSettngs.subnetPrefix = nic_cfg.FromDHCP.PrefixLength;
				}
			}

			if (observer != null) {
				observer.OnNext(netSettngs);
			}
		}

		public IObservable<NetworkSettings> GetNetworkSettings() {
			return Observable.Iterate<NetworkSettings>(observer => GetNetworkSettingsImpl(observer));
		}

		public static IObservable<DeviceObservable> GetDeviceClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = m_deviceFactory.CreateChannel(endpointAddr);
			return Observable.Return(new DeviceObservable(proxy));
		}

		public static IObservable<MediaObservable> GetMediaClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = m_mediaFactory.CreateChannel(endpointAddr);
			return Observable.Return(new MediaObservable(proxy));
		}

		public IObservable<DeviceObservable> GetDeviceClient() {
			return GetDeviceClient(m_transportUri);
		}

		private IEnumerable<IObservable<Object>> GetMediaClientImpl(IObserver<MediaObservable> observer) {
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

		public IObservable<MediaObservable> GetMediaClient() {
			return Observable.Iterate<MediaObservable>(observer => GetMediaClientImpl(observer));
		}



		private IEnumerable<IObservable<Object>> CreateDefaultProfileImpl(string videoSourceToken, IObserver<med::Profile> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			VideoSourceConfiguration[] vscs = null;
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

		private IEnumerable<IObservable<Object>> AddDefaultVideoEncoderImpl(string profileToken, IObserver<med::VideoEncoderConfiguration> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			VideoEncoderConfiguration[] vecs = null;
			yield return GetCompatibleVideoEncoderConfigurations(profileToken).Handle(x => vecs = x);

			VideoEncoderConfiguration vec = null;
			vec = vecs.Where(x => x.Encoding == med::VideoEncoding.H264).FirstOrDefault();
			if (vec == null && vecs.Length > 0) {
				vec = vecs[0];
			}
			if (vec != null) {
				yield return AddVideoEncoderConfiguration(profileToken, vec.token);
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
