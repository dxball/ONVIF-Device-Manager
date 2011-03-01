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
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Linq;

using odm.utils;
using odm.models;

using onvif.types;
using onvif.services.device;
using onvif.services.media;
using onvif.services.imaging;
using onvif.services.events;
using onvif.services.analytics;

using dev = global::onvif.services.device;
using med = global::onvif.services.media;
using img = global::onvif.services.imaging;
using evt = global::onvif.services.events;
using anc = global::onvif.services.analytics;
using tt = global::onvif.types;
using syn = synesis.onvif.extensions;

using net = global::System.Net;
using odm.utils.rx;
using onvif;

namespace odm.onvif {
	
	public class Session {
		private Uri m_transportUri;
		private IDeviceDescription m_deviceDescription;
		public static ChannelFactory<Device> s_deviceFactory = null;
		public static ChannelFactory<Device> s_deviceMtomFactory = null;
		public static ChannelFactory<Media> s_mediaFactory = null;
		public static ChannelFactory<ImagingPort> s_imagingFactory = null;
		public static ChannelFactory<AnalyticsEnginePort> s_analyticsFactory = null;
		public static ChannelFactory<RuleEnginePort> s_ruleEngineFactory = null;
		//public static ChannelFactory<> s_eventsFactory = null;

		public Session(IDeviceDescription deviceDescription) {
			this.m_deviceDescription = deviceDescription;
			this.m_transportUri = deviceDescription.uris.Where(x => x.Scheme == Uri.UriSchemeHttp).LastOrDefault();
			if (this.m_transportUri == null) {
				dbg.Break();
				throw new Exception("failed to create session");
			}
		}

		public Session(Uri uri) {
			if (uri == null) {
				throw new ArgumentNullException("uri");
			}
			this.m_deviceDescription = null;
			this.m_transportUri = uri;
		}

		public Session(string uri):this(new Uri(uri)) {			
		}
		protected object m_context;
		
		public void SetContext<T>(T context) {
			m_context = context;
		}

		public T GetContext<T>() {
			if (m_context == null) {
				if (typeof(T).IsValueType) {
					var err = new InvalidCastException();
					dbg.Error(err);
					throw err;
				}
				return default(T);
			}
			dbg.Assert(typeof(T).IsAssignableFrom(m_context.GetType()));
			return (T)m_context;
		}
		
		static Binding CreateBinding(bool mtomEncoding = false){
	
			//var binding = new WSHttpBinding(SecurityMode.None) {
			//    TextEncoding = Encoding.UTF8,
			//    MaxReceivedMessageSize = 10 * 1024 * 1024,
			//};

			var binding = new CustomBinding() {
				CloseTimeout = TimeSpan.FromMinutes(3),
				OpenTimeout = TimeSpan.FromMinutes(3),
				SendTimeout = TimeSpan.FromMinutes(3),
			};

			if (mtomEncoding) {
				var encoding = new MtomMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8);
				encoding.ReaderQuotas.MaxStringContentLength = 10 * 1024 * 1024;
				binding.Elements.Add(encoding);
			} else {
				var encoding = new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8);
				encoding.ReaderQuotas.MaxStringContentLength = 10 * 1024 * 1024;
				binding.Elements.Add(encoding);
			}
			
			binding.Elements.Add(new HttpTransportBindingElement() {
				MaxReceivedMessageSize = 10 * 1024 * 1024,
			});

			return binding;
		}

		static Session() {

			Binding binding = CreateBinding();
			
			s_deviceFactory = new ChannelFactory<Device>(binding);
			s_deviceMtomFactory = new ChannelFactory<Device>(CreateBinding(true));
			s_mediaFactory = new ChannelFactory<Media>(binding);
			s_imagingFactory = new ChannelFactory<ImagingPort>(binding);
			s_analyticsFactory = new ChannelFactory<AnalyticsEnginePort>(binding);
			s_ruleEngineFactory = new ChannelFactory<RuleEnginePort>(binding);
			//s_eventsFactory = new ChannelFactory<evt::>(new WSHttpBinding(SecurityMode.None) {
			//    TextEncoding = Encoding.UTF8
			//});
		}
		
		//public static Session Create(DeviceDescription deviceDescription) {
		//    return new Session(deviceDescription);
		//}

		//public static Session Create(Uri uri) {
		//    var session = new Session(null);
		//    session.m_transportUri = uri;
		//    return session;
		//}
		//public static Session Create(string uri) {
		//    return Create(new Uri(uri));
		//}
				
		public IObservable<IDeviceDescription> GetDeviceDescription() {
			if (m_deviceDescription == null) {
				DeviceObservable dev = null;
				GetDeviceClient().Handle(x => dev = x);
				//TODO: create and fulfill DeviceDescription
			}

			return Observable.Return(m_deviceDescription);
		}

		private IEnumerable<IObservable<object>> GetSystemBackupImpl(IObserver<BackupFile[]> observer) {
			DeviceObservable dev = null;
			yield return GetDeviceMtomClient().Handle(x => dev = x);
			dbg.Assert(dev != null);

			BackupFile[] backupFiles = null;
			yield return dev.GetSystemBackup().Handle(x => backupFiles = x);
			dbg.Assert(backupFiles != null);

			if (observer != null) {
				observer.OnNext(backupFiles);
			}
		}

		public IObservable<BackupFile[]> GetSystemBackup() {
			return Observable.Iterate<BackupFile[]>(observer => GetSystemBackupImpl(observer));
		}
		
		private IEnumerable<IObservable<object>> RestoreSystemImpl(BackupFile[] backupFiles, IObserver<Unit> observer) {
			DeviceObservable dev = null;
			yield return GetDeviceMtomClient().Handle(x => dev = x);
			dbg.Assert(dev != null);

			yield return dev.RestoreSystem(backupFiles).Idle();
			dbg.Assert(backupFiles != null);

			if (observer != null) {
				observer.OnNext(new Unit());
			}
		}

		public IObservable<Unit> RestoreSystem(BackupFile[] backupFiles) {
			return Observable.Iterate<Unit>(observer => RestoreSystemImpl(backupFiles, observer));
		}

		private class Cache{
			public AsyncState<ObserverState> capabilitiesState = ObserverState.Create(ObserverState.delayed);
			public AsyncSubject<Capabilities> capabilities = null;

			public AsyncState<ObserverState> deviceInformationState = ObserverState.Create(ObserverState.delayed);
			public AsyncSubject<GetDeviceInformationResponse> deviceInformation = null;

			public AsyncState<ObserverState> scopesState = ObserverState.Create(ObserverState.delayed);
			public AsyncSubject<Scope[]> scopes = null;
		}

		private Cache m_cache = new Cache();
		private bool m_useCahce = true;

		private IEnumerable<IObservable<object>> GetCapabilitiesImpl(IObserver<Capabilities> observer) {
			DeviceObservable dev = null;
			yield return GetDeviceClient().Handle(x => dev = x);
			dbg.Assert(dev != null);
			Capabilities capabilities = null;
			if (!m_useCahce) {				
				yield return dev.GetCapabilities().Handle(x => capabilities = x);
				dbg.Assert(capabilities != null);
				if (observer != null) {
					observer.OnNext(capabilities);
				}
				yield break;
			}

			if (m_cache.capabilitiesState.transit(ObserverState.delayed, ObserverState.subscribed)) {
				var subj = new AsyncSubject<Capabilities>();
				dev.GetCapabilities().Subscribe(
					subj.OnNext, 
					err => {
						//m_cache.capabilitiesState.transit(ObserverState.failed);
						subj.OnError(err);
					},
					() => {
						//m_cache.capabilitiesState.transit(ObserverState.completed);
						subj.OnCompleted();
					}
				);
				m_cache.capabilities = subj;
			}

			yield return m_cache.capabilities.Handle(x => capabilities = x);
			dbg.Assert(capabilities != null);
			
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
			dbg.Assert(dev != null);
			Scope[] scopes = null;

			if (!m_useCahce) {
				yield return dev.GetScopes().Handle(x => scopes = x);
				dbg.Assert(scopes != null);

				if (observer != null) {
					observer.OnNext(scopes);
				}
				yield break;
			}

			if (m_cache.scopesState.transit(ObserverState.delayed, ObserverState.subscribed)) {
				var subj = new AsyncSubject<Scope[]>();
				dev.GetScopes().Subscribe(
					subj.OnNext,
					err => {
						//m_cache.capabilitiesState.transit(ObserverState.failed);
						subj.OnError(err);
					},
					() => {
						//m_cache.capabilitiesState.transit(ObserverState.completed);
						subj.OnCompleted();
					}
				);
				m_cache.scopes = subj;
			}

			yield return m_cache.scopes.Handle(x => scopes = x);
			dbg.Assert(scopes != null);

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
			dbg.Assert(dev != null);
			GetDeviceInformationResponse info = null;


			if (!m_useCahce) {
				yield return dev.GetDeviceInformation().Handle(x => info = x);
				dbg.Assert(info != null);

				if (observer != null) {
					observer.OnNext(info);
				}
				yield break;
			}

			if (m_cache.deviceInformationState.transit(ObserverState.delayed, ObserverState.subscribed)) {
				var subj = new AsyncSubject<GetDeviceInformationResponse>();

				dev.GetDeviceInformation().Subscribe(
					subj.OnNext,
					err => {
						//m_cache.deviceInformationState.transit(ObserverState.failed);
						subj.OnError(err);
					},
					() => {
						//m_cache.deviceInformationState.transit(ObserverState.completed);
						subj.OnCompleted();
					}
				);
				m_cache.deviceInformation = subj;
			}

			yield return m_cache.deviceInformation.Handle(x => info = x);
			dbg.Assert(info != null);

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
			dbg.Assert(dev != null);

			yield return dev.SetScopes(scopes).Idle();

			m_cache.scopesState.transit(ObserverState.delayed);

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

			dbg.Assert(scopes != null);
			dbg.Assert(dev_info_response != null);

			info.Manufacturer = dev_info_response.Manufacturer;
			info.Model = dev_info_response.Model;
			info.FirmwareVersion = dev_info_response.FirmwareVersion;
			info.SerialNumber = dev_info_response.SerialNumber;
			info.HardwareId = dev_info_response.HardwareId;
			info.Name = NvcHelper.GetName(scopes.Select(y => y.ScopeItem));
			info.Location = NvcHelper.GetLocation(scopes.Select(y => y.ScopeItem));

			if (observer != null) {				
				observer.OnNext(info);
			}
		}

		public IObservable<DeviceInfo> GetDeviceInfo() {
			return Observable.Iterate<DeviceInfo>(observer => GetDeviceInfoImpl(observer));
		}

		private IEnumerable<IObservable<object>> GetProfilesImpl(IObserver<med::Profile[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::Profile[] profiles = null;
			yield return media.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			if (observer != null) {
				observer.OnNext(profiles);
			}
		}

		public IObservable<med::Profile[]> GetProfiles() {
			return Observable.Iterate<med::Profile[]>(observer => GetProfilesImpl(observer));
		}

		private IEnumerable<IObservable<object>> CreateProfileImpl(string profileName, ProfileToken profileToken, IObserver<med::Profile> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::Profile profile = null;
			yield return media.CreateProfile(profileName, profileToken).Handle(x => profile = x);
			dbg.Assert(profile != null);

			if (observer != null) {
				observer.OnNext(profile);
			}
		}

		private IEnumerable<IObservable<object>> CreateDefaultProfileImpl(string profileName, ProfileToken profileToken, VideoSourceToken videoSourceToken, IObserver<med::Profile> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			med::Profile profile = null;
			yield return CreateProfile(profileName, profileToken).Handle(x => profile = x);
			
			med::VideoSourceConfiguration[] vscs = null;
			yield return GetVideoSourceConfigurations().Handle(x => vscs = x);
			dbg.Assert(vscs != null);
			
			var vsc = vscs.Where(x => x.SourceToken == videoSourceToken).FirstOrDefault();

			if (vsc == null) {
				foreach (var v in vscs.Where(x => String.IsNullOrWhiteSpace(x.SourceToken.value))) {
					med::VideoSourceConfigurationOptions options = null;
					yield return media.GetVideoSourceConfigurationOptions(v.token, null).Handle(x => options = x);
					dbg.Assert(options != null);
					if (options != null && options.VideoSourceTokensAvailable.Contains(videoSourceToken)) {
						vsc = v;
						vsc.SourceToken = videoSourceToken;
						yield return media.SetVideoSourceConfiguration(vsc, true).Idle();
						break;
					}
				}
			}

			if (vsc == null) {
				dbg.Break();
				throw new Exception("video source configuration not found");
			}
			
			yield return AddVideoSourceConfiguration(profile.token, vsc.token);
			profile.VideoSourceConfiguration = vsc;
			
			yield return AddDefaultVideoEncoder(profile).Idle();
			
			if (observer != null) {
				observer.OnNext(profile);
			}
		}

		private IEnumerable<IObservable<object>> DeleteProfileImpl(ProfileToken profileToken, IObserver<Unit> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			yield return media.DeleteProfile(profileToken).Idle();
			
			if (observer != null) {
				observer.OnNext(new Unit());
			}
		}

		public IObservable<med::Profile> CreateProfile(string profileName, ProfileToken profileToken) {
			return Observable.Iterate<med::Profile>(observer => CreateProfileImpl(profileName, profileToken, observer));
		}

		public IObservable<med::Profile> CreateDefaultProfile(string profileName, ProfileToken profileToken, VideoSourceToken videoSourceToken) {
			return Observable.Iterate<med::Profile>(observer => CreateDefaultProfileImpl(profileName, profileToken, videoSourceToken, observer));
		}

		public IObservable<Unit> DeleteProfile(ProfileToken profileToken) {
			return Observable.Iterate<Unit>(observer => DeleteProfileImpl(profileToken, observer));
		}

		private IEnumerable<IObservable<object>> AddVideoSourceConfigurationImpl(ProfileToken profileToken, VideoSourceConfigurationToken vscToken) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			yield return media.AddVideoSourceConfiguration(profileToken, vscToken).Idle();
		}

		public IObservable<med::Profile> AddVideoSourceConfiguration(ProfileToken profileToken, VideoSourceConfigurationToken vscToken) {
			return Observable.Iterate<med::Profile>(observer => AddVideoSourceConfigurationImpl(profileToken, vscToken));
		}
		
		// video source functions

		public IObservable<med::VideoSource[]> GetVideoSources() {
			return Observable.Iterate<med::VideoSource[]>(observer => GetVideoSourcesImpl(observer));
		}

		public IObservable<med::VideoSourceConfiguration[]> GetCompatibleVideoSourceConfigurations(ProfileToken profileToken) {
			return Observable.Iterate<med::VideoSourceConfiguration[]>(observer => GetCompatibleVideoSourceConfigurationsImpl(profileToken, observer));
		}

		public IObservable<med::VideoSourceConfiguration[]> GetVideoSourceConfigurations() {
			return Observable.Iterate<med::VideoSourceConfiguration[]>(observer => GetVideoSourceConfigurationsImpl(observer));
		}

		private IEnumerable<IObservable<object>> GetVideoSourcesImpl(IObserver<med::VideoSource[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::VideoSource[] vsources = null;
			yield return media.GetVideoSources().Handle(x => vsources = x);
			dbg.Assert(vsources != null);

			if (observer != null) {
				observer.OnNext(vsources);
			}
		}
		private IEnumerable<IObservable<object>> GetVideoSourceConfigurationsImpl(IObserver<med::VideoSourceConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::VideoSourceConfiguration[] vscs = null;
			yield return media.GetVideoSourceConfigurations().Handle(x => vscs = x);
			dbg.Assert(vscs != null);

			if (observer != null) {
				observer.OnNext(vscs);
			}
		}

		private IEnumerable<IObservable<object>> GetCompatibleVideoSourceConfigurationsImpl(ProfileToken profileToken, IObserver<med::VideoSourceConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::VideoSourceConfiguration[] vscs = null;
			yield return media.GetCompatibleVideoSourceConfigurations(profileToken).Handle(x => vscs = x);
			
			//TODO: handle UnknownAction and ActionNotSupported faults
			if (vscs == null || vscs.Length == 0) {
				yield return GetVideoSourceConfigurations().Handle(x => vscs = x);
			}

			dbg.Assert(vscs != null);

			if (observer != null) {
				observer.OnNext(vscs);
			}
		}

		

		//Video Encoder functions

		public IObservable<med::VideoEncoderConfiguration[]> GetVideoEncoderConfigurations() {
			return Observable.Iterate<med::VideoEncoderConfiguration[]>(observer => GetVideoEncoderConfigurationsImpl(observer));
		}

		public IObservable<med::VideoEncoderConfiguration[]> GetCompatibleVideoEncoderConfigurations(ProfileToken profileToken) {
			return Observable.Iterate<med::VideoEncoderConfiguration[]>(observer => GetCompatibleVideoEncoderConfigurationsImpl(profileToken, observer));
		}

		public IObservable<Unit> AddVideoEncoderConfiguration(ProfileToken profileToken, VideoEncoderConfigurationToken vecToken) {
			return Observable.Iterate<Unit>(observer => AddVideoEncoderConfigurationImpl(profileToken, vecToken));
		}

		public IObservable<Unit> SetVideoEncoderConfiguration(med::VideoEncoderConfiguration vec, bool forcePersistance) {
			return Observable.Iterate<Unit>(observer => SetVideoEncoderConfigurationImpl(vec, forcePersistance));
		}

		public IObservable<med::VideoEncoderConfigurationOptions> GetVideoEncoderConfigurationOptions(VideoEncoderConfigurationToken vecToken = null, ProfileToken profileToken = null) {
			return Observable.Iterate<med::VideoEncoderConfigurationOptions>(observer => GetVideoEncoderConfigurationOptionsImpl(vecToken, profileToken, observer));
		}

		private IEnumerable<IObservable<object>> GetVideoEncoderConfigurationOptionsImpl(VideoEncoderConfigurationToken vecToken, ProfileToken profileToken, IObserver<med::VideoEncoderConfigurationOptions> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::VideoEncoderConfigurationOptions opts = null;
			yield return media.GetVideoEncoderConfigurationOptions(vecToken, profileToken)
				.Handle(x => opts = x)
				.HandleError(err=>{
					dbg.Error(err);
				});

			if (opts != null) {
				if (observer != null) {
					observer.OnNext(opts);
				}
				yield break;
			}
			
			med::VideoEncoderConfiguration[] vecs = null;
			yield return GetVideoEncoderConfigurations().Handle(x => vecs = x);
			
			if (vecToken != null) {
				opts = CreateOptionsFromVecs(vecs.Where(x => x.token == vecToken).FirstOrDefault());
				if (opts == null) {
					var err = new Exception("specified vec does not exist");
					dbg.Error(err);
					throw err;
				} else {
					if (observer != null) {
						observer.OnNext(opts);
					}
					yield break;
				}
			}

			opts = CreateOptionsFromVecs(vecs);
			if (observer != null) {
				observer.OnNext(opts);
			}
		}

		private IEnumerable<IObservable<object>> GetVideoEncoderConfigurationsImpl(IObserver<med::VideoEncoderConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::VideoEncoderConfiguration[] vecs = null;
			yield return media.GetVideoEncoderConfigurations().Handle(x => vecs = x);
			dbg.Assert(vecs != null);

			if (observer != null) {
				observer.OnNext(vecs);
			}
		}

		private IEnumerable<IObservable<object>> GetCompatibleVideoEncoderConfigurationsImpl(ProfileToken profileToken, IObserver<med::VideoEncoderConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::VideoEncoderConfiguration[] vecs = null;
			yield return media.GetCompatibleVideoEncoderConfigurations(profileToken).Handle(x => vecs = x);
			
			//TODO: handle UnknownAction and ActionNotSupported faults
			if (vecs == null || vecs.Length == 0) {
				yield return GetVideoEncoderConfigurations().Handle(x => vecs = x);
			}

			if (observer != null) {
				observer.OnNext(vecs);
			}
		}

		private IEnumerable<IObservable<object>> AddVideoEncoderConfigurationImpl(ProfileToken profileToken, VideoEncoderConfigurationToken vecToken) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			yield return media.AddVideoEncoderConfiguration(profileToken, vecToken).Idle();
		}

		private IEnumerable<IObservable<object>> SetVideoEncoderConfigurationImpl(med::VideoEncoderConfiguration vec, bool forcePersistance) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			yield return media.SetVideoEncoderConfiguration(vec, forcePersistance).Idle();
		}

		//Video Analytics functions

		public IObservable<med::VideoAnalyticsConfiguration[]> GetVideoAnalyticsConfigurations() {
			return Observable.Iterate<med::VideoAnalyticsConfiguration[]>(observer => GetVideoAnalyticsConfigurationsImpl(observer));
		}

		public IObservable<med::VideoAnalyticsConfiguration[]> GetCompatibleVideoAnalyticsConfigurations(ProfileToken profileToken) {
			return Observable.Iterate<med::VideoAnalyticsConfiguration[]>(observer => GetCompatibleVideoAnalyticsConfigurationsImpl(profileToken, observer));
		}

		public IObservable<med::Profile> AddVideoAnalyticsConfiguration(ProfileToken profileToken, VideoAnalyticsConfigurationToken vacToken) {
			return Observable.Iterate<med::Profile>(observer => AddVideoAnalyticsConfigurationImpl(profileToken, vacToken));
		}

		private IEnumerable<IObservable<object>> GetVideoAnalyticsConfigurationsImpl(IObserver<med::VideoAnalyticsConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::VideoAnalyticsConfiguration[] vecs = null;
			yield return media.GetVideoAnalyticsConfigurations().Handle(x => vecs = x);
			dbg.Assert(vecs != null);

			if (observer != null) {
				observer.OnNext(vecs);
			}
		}

		private IEnumerable<IObservable<object>> GetCompatibleVideoAnalyticsConfigurationsImpl(ProfileToken profileToken, IObserver<med::VideoAnalyticsConfiguration[]> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			med::VideoAnalyticsConfiguration[] vecs = null;
			yield return media.GetCompatibleVideoAnalyticsConfigurations(profileToken).Handle(x => vecs = x);
			dbg.Assert(vecs != null);

			if (observer != null) {
				observer.OnNext(vecs);
			}
		}

		private IEnumerable<IObservable<object>> AddVideoAnalyticsConfigurationImpl(ProfileToken profileToken, VideoAnalyticsConfigurationToken vacToken) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			yield return media.AddVideoAnalyticsConfiguration(profileToken, vacToken).Idle();
		}

		//Network functions

		private IEnumerable<IObservable<object>> GetNetworkInterfacesImpl(IObserver<tt::NetworkInterface[]> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			dbg.Assert(device != null);

			global::onvif.types.NetworkInterface[] nics = null;
			yield return device.GetNetworkInterfaces().Handle(x => nics = x);
			dbg.Assert(nics != null);

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
			dbg.Assert(device != null);

			DNSInformation dns = null;
			yield return device.GetDNS().Handle(x => dns = x);
			dbg.Assert(dns != null);

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
			dbg.Assert(device != null);

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
			dbg.Assert(device != null);

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
			dbg.Assert(device != null);

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
			dbg.Assert(device != null);
			string message = null;
			yield return device.SystemReboot().Handle(x=>message = x);

			if (observer != null) {
			    observer.OnNext(message);
			}
		}

		public IObservable<string> SystemReboot() {
			return Observable.Iterate<string>(observer => SystemRebootImpl(observer));
		}


		private IEnumerable<IObservable<object>> GetStreamUriImpl(med::StreamSetup streamSetup, ProfileToken profileToken, IObserver<string> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			string uri = null;
			yield return media.GetStreamUri(streamSetup, profileToken).Handle(x => uri = x.Uri);
			dbg.Assert(uri != null);

			if (observer != null) {
				observer.OnNext(uri);
			}
		}

		public IObservable<string> GetStreamUri(med::StreamSetup streamSetup, ProfileToken profileToken) {
			return Observable.Iterate<string>(observer => GetStreamUriImpl(streamSetup, profileToken, observer));
		}

		private IEnumerable<IObservable<object>> GetNetworkDefaultGatewayImpl(IObserver<NetworkGateway> observer) {
			DeviceObservable device = null;
			yield return GetDeviceClient().Handle(x => device = x);
			dbg.Assert(device != null);

			NetworkGateway gateway = null;
			yield return device.GetNetworkDefaultGateway().Handle(x => gateway = x);
			dbg.Assert(gateway != null);

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
			dbg.Assert(nics != null);
			dbg.Assert(dns != null);

			var netStat = new NetworkStatus();
			if (dns.FromDHCP && dns.DNSFromDHCP != null && dns.DNSFromDHCP.Count() > 0) {
				dbg.Assert(!String.IsNullOrWhiteSpace(dns.DNSFromDHCP[0].IPv4Address));
				netStat.dns = net::IPAddress.Parse(dns.DNSFromDHCP[0].IPv4Address);
			} else if (!dns.FromDHCP && dns.DNSManual != null && dns.DNSManual.Count() > 0 && !String.IsNullOrWhiteSpace(dns.DNSManual[0].IPv4Address)) {
				netStat.dns = net::IPAddress.Parse(dns.DNSManual[0].IPv4Address);
			}
			dbg.Assert(netStat.dns != null);

			var nic = nics.Where(x => x.Enabled).FirstOrDefault();
			if (nic != null) {
				var nic_cfg = nic.IPv4.Config;
				//networkSettngs.m_token = t[0].token;
				//netStat.mac = PhysicalAddress.Parse(nic.Info.HwAddress.Replace(':', '-'));
				netStat.mac = nic.Info.HwAddress.Replace(':', '-').ToUpper();
				//netStat.m_dhcp = nic.IPv4.Config.DHCP;

				if (nic_cfg.DHCP && nic_cfg.FromDHCP != null) {
					//DebugHelper.Assert(nic_cfg.FromDHCP != null);
					dbg.Assert(nic_cfg.FromDHCP.Address != null);
					dbg.Assert(!String.IsNullOrWhiteSpace(nic_cfg.FromDHCP.Address));
					netStat.ip = net::IPAddress.Parse(nic_cfg.FromDHCP.Address);
					netStat.subnetPrefix = nic_cfg.FromDHCP.PrefixLength;
				} else if (!nic_cfg.DHCP && nic_cfg.Manual != null && nic_cfg.Manual.Count() > 0) {
					dbg.Assert(!String.IsNullOrWhiteSpace(nic_cfg.Manual[0].Address));
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

		//private IEnumerable<IObservable<object>> GetNetworkSettingsImpl(IObserver<NetworkSettings> observer) {

		//    NetworkGateway gateway = null;
		//    DNSInformation dns = null;
		//    tt::NetworkInterface[] nics = null;

		//    yield return Observable.Merge(
		//        GetNetworkDefaultGateway().Handle(x => gateway = x).IgnoreError(),
		//        GetDNS().Handle(x => dns = x).IgnoreError(),
		//        GetNetworkInterfaces().Handle(x => nics = x)
		//    );

		//    dbg.Assert(gateway != null);
		//    dbg.Assert(dns != null);
		//    dbg.Assert(nics != null);

		//    var netSettings = new NetworkSettings();
			
		//    if (gateway!=null && gateway.IPv4Address != null && gateway.IPv4Address.Count() > 0) {
		//        net::IPAddress defaultGateway = net::IPAddress.None;
		//        net::IPAddress.TryParse(gateway.IPv4Address[0], out defaultGateway);
		//        netSettings.defaultGateway = defaultGateway;
		//    }

		//    if (dns!=null && dns.DNSManual != null && dns.DNSManual.Count() > 0 && !String.IsNullOrWhiteSpace(dns.DNSManual[0].IPv4Address)) {
		//        netSettings.staticDns = net::IPAddress.Parse(dns.DNSManual[0].IPv4Address);
		//    } else if (dns!=null && dns.DNSFromDHCP != null && dns.DNSFromDHCP.Count() > 0) {
		//        netSettings.staticDns = net::IPAddress.Parse(dns.DNSFromDHCP[0].IPv4Address);
		//    }

		//    var nic = nics.Where(x => x.Enabled).FirstOrDefault();
		//    if (nic != null) {
		//        var nic_cfg = nic.IPv4.Config;
		//        //networkSettngs.m_token = t[0].token;
		//        //networkSettngs.m_mac = PhysicalAddress.Parse(nic.Info.HwAddress.Replace(':','-'));
		//        netSettings.dhcp = nic.IPv4.Config.DHCP;

		//        if (nic_cfg.Manual.Count() > 0) {
		//            netSettings.staticIp = net::IPAddress.Parse(nic_cfg.Manual[0].Address);
		//            netSettings.subnetPrefix = nic_cfg.Manual[0].PrefixLength;
		//        } else if (nic_cfg.FromDHCP != null) {
		//            netSettings.staticIp = net::IPAddress.Parse(nic_cfg.FromDHCP.Address);
		//            netSettings.subnetPrefix = nic_cfg.FromDHCP.PrefixLength;
		//        }
		//    }
			
		//    if (observer != null) {
		//        observer.OnNext(netSettings);
		//    }
		//}

		//public IObservable<NetworkSettings> GetNetworkSettings() {
		//    return Observable.Iterate<NetworkSettings>(observer => GetNetworkSettingsImpl(observer));
		//}

		public static IObservable<DeviceObservable> GetDeviceClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = s_deviceFactory.CreateChannel(endpointAddr);
			return Observable.Return(new DeviceObservable(proxy));
		}

		public static IObservable<DeviceObservable> GetDeviceMtomClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = s_deviceMtomFactory.CreateChannel(endpointAddr);
			return Observable.Return(new DeviceObservable(proxy));
		}

		public static IObservable<MediaObservable> GetMediaClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = s_mediaFactory.CreateChannel(endpointAddr);
			return Observable.Return(new MediaObservable(proxy));
		}

		public static IObservable<ImagingObservable> GetImagingClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = s_imagingFactory.CreateChannel(endpointAddr);
			return Observable.Return(new ImagingObservable(proxy));
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
		public IObservable<DeviceObservable> GetDeviceMtomClient() {
			return GetDeviceMtomClient(m_transportUri);
		}
		
		public IObservable<MediaObservable> GetMediaClient() {
			return Observable.Iterate<MediaObservable>(observer => GetMediaClientImpl(observer));
		}

		public IObservable<ImagingObservable> GetImagingClient() {
			return Observable.Iterate<ImagingObservable>(observer => GetImagingClientImpl(observer));
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
			dbg.Assert(caps != null);

			if (caps.Media == null) {
				throw new Exception("not supported");
			}
			
			yield return GetMediaClient(new Uri(caps.Media.XAddr)).Handle(x => media = x);
			dbg.Assert(media != null);

			if (observer != null) {
				observer.OnNext(media);
			}
		}

		private IEnumerable<IObservable<object>> GetImagingClientImpl(IObserver<ImagingObservable> observer) {
			ImagingObservable imaging = null;
			Capabilities caps = null;
			yield return GetCapabilities().Handle(x => caps = x);
			dbg.Assert(caps != null);

			if (caps.Imaging == null) {
				throw new Exception("not supported");
			}

			yield return GetImagingClient(new Uri(caps.Imaging.XAddr)).Handle(x => imaging = x);
			dbg.Assert(imaging != null);

			if (observer != null) {
				observer.OnNext(imaging);
			}
		}

		private IEnumerable<IObservable<object>> GetAnalyticsClientImpl(IObserver<AnalyticsObservable> observer) {
			AnalyticsObservable analytics = null;
			Capabilities caps = null;
			yield return GetCapabilities().Handle(x => caps = x);
			dbg.Assert(caps != null);

			if (caps.Analytics == null) {
				throw new Exception("not supported");
			}

			yield return GetAnalyticsClient(new Uri(caps.Analytics.XAddr)).Handle(x => analytics = x);
			dbg.Assert(analytics != null);

			if (observer != null) {
				observer.OnNext(analytics);
			}
		}

		private IEnumerable<IObservable<object>> GetRuleEngineClientImpl(IObserver<RuleEngineObservable> observer) {
			RuleEngineObservable RuleEngine = null;
			Capabilities caps = null;
			yield return GetCapabilities().Handle(x => caps = x);
			dbg.Assert(caps != null);

			if (caps.Analytics == null) {
				throw new Exception("not supported");
			}

			yield return GetRuleEngineClient(new Uri(caps.Analytics.XAddr)).Handle(x => RuleEngine = x);
			dbg.Assert(RuleEngine != null);

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



		private IEnumerable<IObservable<object>> AddDefaultVideoEncoderImpl(med::Profile profile, IObserver<med::VideoEncoderConfiguration> observer) {
			if (profile.VideoEncoderConfiguration != null) {
				if (observer != null) {
					observer.OnNext(profile.VideoEncoderConfiguration);
				}
				yield break;
			}

			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			med::VideoEncoderConfiguration[] vecs = null;
			yield return GetCompatibleVideoEncoderConfigurations(profile.token).Handle(x => vecs = x).IgnoreError();
			if (vecs == null || vecs.Length == 0) {
				yield return GetVideoEncoderConfigurations().Handle(x => vecs = x);
			}

			med::VideoEncoderConfiguration vec = null;
			//vec = vecs.Where(x => x.Encoding == med::VideoEncoding.H264).FirstOrDefault();
			vec = vecs.FirstOrDefault();
			if (vec == null && vecs.Length > 0) {
				vec = vecs[0];
			}
			if (vec != null) {
				yield return AddVideoEncoderConfiguration(profile.token, vec.token).Idle();
				profile.VideoEncoderConfiguration = vec;
			}


			if (observer != null) {
				observer.OnNext(profile.VideoEncoderConfiguration);
			}
		}

		public IObservable<med::VideoEncoderConfiguration> AddDefaultVideoEncoder(med::Profile profile) {
			return Observable.Iterate<med::VideoEncoderConfiguration>(observer => AddDefaultVideoEncoderImpl(profile, observer));
		}


		private IEnumerable<IObservable<object>> AddDefaultVideoAnalyticsImpl(med::Profile profile, IObserver<med::VideoAnalyticsConfiguration> observer) {
			if (profile.VideoAnalyticsConfiguration != null) {
				if (observer != null) {
					observer.OnNext(profile.VideoAnalyticsConfiguration);
				}
				yield break;
			}
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			med::VideoAnalyticsConfiguration[] vacs = null;
			yield return GetCompatibleVideoAnalyticsConfigurations(profile.token).Handle(x => vacs = x);

			med::VideoAnalyticsConfiguration vac = vacs.OrderBy(x => x.UseCount).FirstOrDefault();
			
			if (vac != null) {
				yield return AddVideoAnalyticsConfiguration(profile.token, vac.token).Idle();
			}

			profile.VideoAnalyticsConfiguration = vac;
			if (observer != null) {
				observer.OnNext(vac);
			}
		}

		public IObservable<med::VideoAnalyticsConfiguration> AddDefaultVideoAnalytics(med::Profile profile) {
			return Observable.Iterate<med::VideoAnalyticsConfiguration>(observer => AddDefaultVideoAnalyticsImpl(profile, observer));
		}

		public IObservable<med::Config> GetVideoAnalyticModule(med::Profile profile, string moduleName) {
			return Observable.Iterate<med::Config>(observer => GetVideoAnalyticModuleImpl(profile, moduleName, observer));
		}

		public IObservable<object> RemoveVideoAnalyticModule(med::Profile profile, string moduleName) {
			return Observable.Concat(RemoveModuleVideoAnalyticImpl(profile, moduleName)).Idle();
		}

		private IEnumerable<IObservable<object>> RemoveModuleVideoAnalyticImpl(med::Profile profile, string moduleName) {
			if (profile == null || profile.VideoAnalyticsConfiguration == null) {
				yield break;
			}
			var vac = profile.VideoAnalyticsConfiguration;
			
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			vac.AnalyticsEngineConfiguration.AnalyticsModule = vac.AnalyticsEngineConfiguration.AnalyticsModule.Where(x => x.Type.Name != moduleName).ToArray();
			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();

		}
		

		private IEnumerable<IObservable<object>> GetVideoAnalyticModuleImpl(med::Profile profile, string moduleName, IObserver<med::Config> observer) {
			var vac = profile.VideoAnalyticsConfiguration;
			if (vac == null) {
				throw new Exception("specified profile does not contains analytics");
			}

			var mod = vac.AnalyticsEngineConfiguration.AnalyticsModule.Where(x => x.Type == new XmlQualifiedName(moduleName)).FirstOrDefault();
			if (mod == null) {
				MediaObservable media = null;
				yield return GetMediaClient().Handle(x => media = x);

				med::Config defMod = null;
				try {
					defMod = vac.AnalyticsEngineConfiguration.Extension.Any
								 .Where(x =>
									 x.LocalName == "DefaultModule" && x.NamespaceURI == "http://www.onvif.org/ver10/schema"
								 )
								 .Select(x =>
									 x.Deserialize<syn::DefaultModule>()
								 )
								 .Where(x =>
									 x.type.Name == moduleName
								 )
								 .FirstOrDefault();
				} catch(Exception err) {
					dbg.Error(err);
					throw;
				}
				vac.AnalyticsEngineConfiguration.AnalyticsModule = vac.AnalyticsEngineConfiguration.AnalyticsModule.Append(defMod).ToArray();
				yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();
				mod = defMod;
			}

			if (observer != null) {
				observer.OnNext(mod);
			}
		}

		private IEnumerable<IObservable<object>> AddDefaultMetadataImpl(med::Profile profile, IObserver<med::MetadataConfiguration> observer) {

			if (profile.MetadataConfiguration != null) {
				if (observer != null) {
					observer.OnNext(profile.MetadataConfiguration);
				}
				yield break;
			}
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);

			med::MetadataConfiguration[] mcfgs = null;
			yield return media.GetCompatibleMetadataConfigurations(profile.token).Handle(x => mcfgs = x);

			var metadata = mcfgs.OrderBy(x => x.UseCount).FirstOrDefault();

			if (metadata != null) {
				yield return media.AddMetadataConfiguration(profile.token, metadata.token).Idle();
				//med::MetadataConfigurationOptions options = null;
				//yield return media.GetMetadataConfigurationOptions(metadata.token, profile.token).Handle(x=> options = x);
				if (profile.VideoAnalyticsConfiguration != null) {
					metadata.AnalyticsSpecified = true;
					metadata.Analytics = true;
					yield return media.SetMetadataConfiguration(metadata, true).Idle();
				}
			}
			
			profile.MetadataConfiguration = metadata;
			if (observer != null) {
				observer.OnNext(metadata);
			}
			
		}


		public IObservable<med::MetadataConfiguration> AddDefaultMetadata(med::Profile profile) {
			dbg.Assert(profile.VideoSourceConfiguration != null);
			return Observable.Iterate<med::MetadataConfiguration>(observer => AddDefaultMetadataImpl(profile, observer));
		}

		//public IObservable<MediaObservable> GetMediaClient() {
		//    //return GetMediaClient(m_transportUri);
		//}

		//public MediaObservable CreateMediaClient() {
		//    return CreateMediaClient(new Uri(capabilities.Media.XAddr));
		//}

		public IDeviceDescription deviceDescription {
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


		public IObservable<OnvifEvent> GetEvents() {
			return Observable.Iterate<OnvifEvent>(_observer => GetEventsImpl(_observer));
		}

		[ServiceContract]
		interface ISubscriptionManager : evt::PullPointSubscription, evt::SubscriptionManager {
		}

		static XmlElement CreateTopic() {
			var t = new XElement("{http://docs.oasis-open.org/wsn/b-2}TopicExpression",
				//new XAttribute("Dialect", "http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete"),
				new XAttribute("Dialect", "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet"),
				new XAttribute(XNamespace.Xmlns + "tns1", "http://www.onvif.org/ver10/topics"),
				"tns1:Device//.|tns1:MediaControl//.|tns1:VideoAnalytics//."
				//"tns1:*"
			);
			//http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet
			//
			var sx = t.ToString();
			var x = new XmlDocument();
			using (var w = x.CreateNavigator().AppendChild()) {
				t.WriteTo(w);
			}
			//var sx = "<TopicExpression xmlns:tns1=\"http://www.onvif.org/ver10/topics\" Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">tns1:Device//.|tns1:MediaControl//.|tns1:VideoAnalytics//.</TopicExpression>";
			//var sx = "<TopicExpression xmlns:tns1=\"http://www.onvif.org/ver10/topics\" Dialect=\"http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet\" xmlns=\"http://docs.oasis-open.org/wsn/b-2\">tns1:*</TopicExpression>";
			var topic = new XmlDocument();
			topic.LoadXml(sx);

			//fill subscription topic
			//TopicHelper topicHelper = new TopicHelper();
			//if (!topicHelper.CheckSupportedDialects(_eventDescription.TopicExpressionDialect))
			//    throw new Exception("Required Filter Dialect not supported by camera!");

			//if (!topicHelper.InitTopicTree(_eventDescription.TopicSet.Any))
			//    throw new Exception("Failed to parse topic set!");

			//topicFilter = topicHelper.CreateTopicFilter(1);
			//if (null == topicFilter)
			//    throw new Exception("Failed to create Topic filter for subscription!");

			return x.DocumentElement;
		}

		public partial class OnvifEvent {
			public System.DateTime arrivalTime;
			public evt::EndpointReferenceType subscriptionReference;
			public evt::TopicExpressionType topic;
			public evt::EndpointReferenceType producerReference;
			public tt::Message message;
		}

		private IEnumerable<IObservable<object>> GetEventsImpl(IObserver<OnvifEvent> observer) {

			Capabilities caps = null;				
			yield return GetCapabilities().Handle(x=>caps = x);

			var evt_portType_factory = new ChannelFactory<evt::EventPortType>(new WSHttpBinding(SecurityMode.None) {
				TextEncoding = Encoding.UTF8
			});

			var evt_portType = evt_portType_factory.CreateChannel(new EndpointAddress(caps.Events.XAddr));
			var subscr = new CreatePullPointSubscriptionRequest();
			subscr.InitialTerminationTime = "P10S";

			var filter = new evt::FilterType();
			filter.Any = new System.Xml.XmlElement[] { CreateTopic() };
			subscr.Filter = filter;
			var subscrResponse = evt_portType.CreatePullPointSubscription(subscr);

			TimeSpan timeout = subscrResponse.TerminationTime.Value - subscrResponse.CurrentTime;
			var subscribeServiceUrl = subscrResponse.SubscriptionReference.Address.Value;

			var subscrManager = new ChannelFactory<ISubscriptionManager>(new WSHttpBinding(SecurityMode.None) {
				TextEncoding = Encoding.UTF8
			}).CreateChannel(new EndpointAddress(subscribeServiceUrl));

			var pullMessages = Observable.FromAsyncPattern<PullMessagesRequest, PullMessagesResponse>(subscrManager.BeginPullMessages, subscrManager.EndPullMessages);
			var unsubscribe = Observable.FromAsyncPattern<UnsubscribeRequest, UnsubscribeResponse1>(subscrManager.BeginUnsubscribe, subscrManager.EndUnsubscribe);

			var fin = Disposable.Create(() => {
				unsubscribe(new UnsubscribeRequest(new Unsubscribe()))
					.Subscribe(resp=>{

					},err=>{
						dbg.Error(err);
					});			
			});

			using (fin) {
				while (true) {
					evt::PullMessagesResponse resp = null;
					yield return pullMessages(new PullMessagesRequest("P3S", 5, null)).Handle(x => resp = x);
					if (observer != null) {
						foreach(var x in resp.NotificationMessage){
							dbg.Assert(x != null);
							var evt = new OnvifEvent(){
								arrivalTime = resp.CurrentTime,
								producerReference = x.ProducerReference,
								subscriptionReference = x.SubscriptionReference,
								topic = x.Topic,
								message = x.Message!=null?x.Message.Deserialize<tt::Message>():null,
							};
							observer.OnNext(evt);
						}						
					}
				}
			}				
		}



		public IObservable<med::MediaUri> GetSnapshotUri(ProfileToken profileToken) {
			return Observable.Iterate<med::MediaUri>(observer => GetSnapshotUriImpl(profileToken, observer));
		}
		private IEnumerable<IObservable<object>> GetSnapshotUriImpl(ProfileToken profileToken, IObserver<med::MediaUri> observer) {
			MediaObservable media = null;
			yield return GetMediaClient().Handle(x => media = x);
			med::MediaUri uri = null;
			yield return media.GetSnapshotUri(profileToken).Handle(x=>uri = x);
			
			if (observer != null) {
				observer.OnNext(uri);
			}			
		}


		static IEnumerable<IObservable<object>> DownloadImageImpl(Uri uri, IObserver<Image> observer) {
			var request = (HttpWebRequest)HttpWebRequest.Create(uri);

			request.Method = WebRequestMethods.Http.Get;
			request.MaximumResponseHeadersLength = 10 * 1024;
			request.ContentLength = 0;
			request.KeepAlive = false;
			//request.Accept = "image/jpeg";
			//request.UserAgent = "Mozilla/4.0 (compatible; VAMS)";
			//request.ContentType = MediaTypeNames.Application.Octet; //"application/octet-stream"
			//request.Headers.Add("Content-Transfer-Encoding: binary");
			request.ProtocolVersion = HttpVersion.Version11;
			Image img = null;
			HttpWebResponse response = null;
			yield return Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)().Handle(x => response = (HttpWebResponse)x);
			if (response.StatusCode != HttpStatusCode.OK) {
				throw new WebException("image download failed", null, WebExceptionStatus.ReceiveFailure, response);
			}
			var downStream = response.GetResponseStream();

			yield return Observable.Start(() => {
				return Image.FromStream(downStream);
			}).Handle(x => img = x);

			downStream.Close();
			response.Close();

			if (observer != null) {
				observer.OnNext(img);
			}
		}

		static IObservable<Image> DownloadImage(Uri uri) {
			return Observable.Iterate<Image>(observer => DownloadImageImpl(uri, observer));
		}


		public IObservable<Image> GetSnapshot(ProfileToken profileToken) {
			return Observable.Iterate<Image>(observer => GetSnapshotImpl(profileToken, observer));
		}

		private IEnumerable<IObservable<object>> GetSnapshotImpl(ProfileToken profileToken, IObserver<Image> observer) {
			
			med::MediaUri mediaLink = null;
			yield return GetSnapshotUri(profileToken).Handle(x => mediaLink = x);
			Image img = null;
			var uri = new Uri(m_transportUri, mediaLink.Uri);
			yield return DownloadImage(uri).Handle(x => img = x);
			if (observer != null) {
				observer.OnNext(img);
			}
		}

		// helper functions

		protected med::VideoEncoderConfigurationOptions CreateOptionsFromVecs(params med::VideoEncoderConfiguration[] vecs) {
			if (vecs == null) {
				return null;
			}

			med::VideoEncoderConfigurationOptions options = new med::VideoEncoderConfigurationOptions();

			foreach (var v in vecs) {
				if (v.H264 != null) {
					if (options.H264 == null) {
						options.H264 = new med::H264Options();
						options.H264.EncodingIntervalRange = new med::IntRange() {
							Min = v.RateControl.EncodingInterval,
							Max = v.RateControl.EncodingInterval
						};
						options.H264.FrameRateRange = new med::IntRange() {
							Min = v.RateControl.FrameRateLimit,
							Max = v.RateControl.FrameRateLimit
						};
						options.H264.GovLengthRange = new med::IntRange() {
							Min = v.H264.GovLength,
							Max = v.H264.GovLength
						};
						options.H264.ResolutionsAvailable = new med::VideoResolution[] { v.Resolution };

					} else {
						options.H264.EncodingIntervalRange.Min = Math.Min(options.H264.EncodingIntervalRange.Min, v.RateControl.EncodingInterval);
						options.H264.EncodingIntervalRange.Max = Math.Max(options.H264.EncodingIntervalRange.Max, v.RateControl.EncodingInterval);

						options.H264.FrameRateRange.Min = Math.Min(options.H264.FrameRateRange.Min, v.RateControl.FrameRateLimit);
						options.H264.FrameRateRange.Max = Math.Max(options.H264.FrameRateRange.Max, v.RateControl.FrameRateLimit);

						options.H264.GovLengthRange.Min = Math.Min(options.H264.GovLengthRange.Min, v.H264.GovLength);
						options.H264.GovLengthRange.Max = Math.Max(options.H264.GovLengthRange.Max, v.H264.GovLength);

						if (!options.H264.ResolutionsAvailable.Any(x => x.Width == v.Resolution.Width && x.Height == v.Resolution.Height)) {
							options.H264.ResolutionsAvailable = options.H264.ResolutionsAvailable.Append(v.Resolution).ToArray();
						}
					}
				}

				if (v.MPEG4 != null) {
					if (options.MPEG4 == null) {
						options.MPEG4 = new med::Mpeg4Options();
						options.MPEG4.EncodingIntervalRange = new med::IntRange() {
							Min = v.RateControl.EncodingInterval,
							Max = v.RateControl.EncodingInterval
						};
						options.MPEG4.FrameRateRange = new med::IntRange() {
							Min = v.RateControl.FrameRateLimit,
							Max = v.RateControl.FrameRateLimit
						};
						options.MPEG4.GovLengthRange = new med::IntRange() {
							Min = v.MPEG4.GovLength,
							Max = v.MPEG4.GovLength
						};
						options.MPEG4.ResolutionsAvailable = new med::VideoResolution[] { v.Resolution };

					} else {
						options.MPEG4.EncodingIntervalRange.Min = Math.Min(options.MPEG4.EncodingIntervalRange.Min, v.RateControl.EncodingInterval);
						options.MPEG4.EncodingIntervalRange.Max = Math.Max(options.MPEG4.EncodingIntervalRange.Max, v.RateControl.EncodingInterval);

						options.MPEG4.FrameRateRange.Min = Math.Min(options.MPEG4.FrameRateRange.Min, v.RateControl.FrameRateLimit);
						options.MPEG4.FrameRateRange.Max = Math.Max(options.MPEG4.FrameRateRange.Max, v.RateControl.FrameRateLimit);

						options.MPEG4.GovLengthRange.Min = Math.Min(options.MPEG4.GovLengthRange.Min, v.MPEG4.GovLength);
						options.MPEG4.GovLengthRange.Max = Math.Max(options.MPEG4.GovLengthRange.Max, v.MPEG4.GovLength);

						if (!options.MPEG4.ResolutionsAvailable.Any(x => x.Width == v.Resolution.Width && x.Height == v.Resolution.Height)) {
							options.MPEG4.ResolutionsAvailable = options.MPEG4.ResolutionsAvailable.Append(v.Resolution).ToArray();
						}
					}
				}

				if (v.Encoding == med::VideoEncoding.JPEG) {
					if (options.JPEG == null) {
						options.JPEG = new med::JpegOptions();
						options.JPEG.EncodingIntervalRange = new med::IntRange() {
							Min = v.RateControl.EncodingInterval,
							Max = v.RateControl.EncodingInterval
						};
						options.JPEG.FrameRateRange = new med::IntRange() {
							Min = v.RateControl.FrameRateLimit,
							Max = v.RateControl.FrameRateLimit
						};
						options.JPEG.ResolutionsAvailable = new med::VideoResolution[] { v.Resolution };

					} else {
						options.JPEG.EncodingIntervalRange.Min = Math.Min(options.JPEG.EncodingIntervalRange.Min, v.RateControl.EncodingInterval);
						options.JPEG.EncodingIntervalRange.Max = Math.Max(options.JPEG.EncodingIntervalRange.Max, v.RateControl.EncodingInterval);

						options.JPEG.FrameRateRange.Min = Math.Min(options.JPEG.FrameRateRange.Min, v.RateControl.FrameRateLimit);
						options.JPEG.FrameRateRange.Max = Math.Max(options.JPEG.FrameRateRange.Max, v.RateControl.FrameRateLimit);

					}

					if (!options.JPEG.ResolutionsAvailable.Any(x => x.Width == v.Resolution.Width && x.Height == v.Resolution.Height)) {
						options.JPEG.ResolutionsAvailable = options.JPEG.ResolutionsAvailable.Append(v.Resolution).ToArray();
					}
				}

			}

			return options;
		}


	}
}