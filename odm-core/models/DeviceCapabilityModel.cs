using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using odm.onvif;
using odm.utils;

using onvif.types;
using onvif.services.device;
using onvif.services.media;

using dev = global::onvif.services.device;
using med = global::onvif.services.media;
using tt = global::onvif.types;
using syn = global::synesis.onvif.extensions;

using System.Xml;

namespace odm.models {

	public class AnalyticsModules {
		public bool? SceneCalibrator = null;
		public bool? ServiceDetectors = null;
		public bool? DisplayAnnotation = null;
		public bool? ObjectTracker = null;
		public bool? RuleEngine = null;
		public bool? DigitalAntishaker = null;
	}

	public partial class DeviceCapabilityModel : ModelBase<DeviceCapabilityModel> {
		private class Channel:ChannelDescription{
			public string m_videoSourceToken;
			public string m_name;
			public Image m_snapshot;
			public Size m_encoderResolution;
			public string m_mediaUri;
			public AnalyticsModules m_modules;
			
			public override string Id {
				get {
					return m_videoSourceToken;
				}
			}

			public override string Name {
				get {
					return m_name;
				}
			}

			public override Size encoderResolution {
				get {
					return m_encoderResolution;
				}
			}

			public override string mediaUri {
				get {
					return m_mediaUri;
				}
			}

			public override AnalyticsModules modules {
				get {
					return m_modules;
				}
			}

			//public override Image snapshot {
			//    get {
			//        return m_snapshot;
			//    }
			//}
		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<DeviceCapabilityModel> observer) {
			
			Capabilities caps = null;
			med::VideoSource[] vsources = null;
			med::Profile[] profiles = null;
			var channels = new List<Channel>();
			
			yield return Observable.Merge(
				session.GetVideoSources().Handle(x => vsources = x ?? new med::VideoSource[] { }),
				session.GetProfiles().Handle(x => profiles = x),
				session.GetCapabilities().Handle(x => caps = x),
				session.GetDeviceInfo().Handle(x => devInfo = x)
			);
			
			dbg.Assert(profiles != null);
			foreach (var v in vsources) {
				var profile_token = NvcHelper.GetChannelProfileToken(v.token);
				//var profile = profiles.Where(x => x.token == profile_token).FirstOrDefault();
				//if (profile == null) {
				//    yield return session.CreateDefaultProfile(v.token).Handle(x => profile = x);					
				//}
				med::Profile profile = null;
				yield return session.CreateDefaultProfile(v.token).Handle(x => profile = x);
				//Image snapshot = null;
				//yield return session.GetSnapshot(profile.token).Handle(x=>snapshot = x);
				var modules = new AnalyticsModules();

				if (caps.Analytics != null && caps.Analytics.AnalyticsModuleSupport) {
					yield return session.AddDefaultVideoAnalytics(profile).Idle();
					var vac = profile.VideoAnalyticsConfiguration;
					
					if (vac != null) {
						foreach (var m in vac.AnalyticsEngineConfiguration.AnalyticsModule.Select(x => x.Type.Name)) {
							switch (m) {
								case "SceneCalibrator":
									modules.SceneCalibrator = true;
									break;
								case "ObjectTracker":
									modules.ObjectTracker = true;
									break;
								case "RuleEngine":
									modules.RuleEngine = true;
									break;
								case "ServiceDetectors":
									modules.ServiceDetectors = true;
									break;
								case "DigitalAntishaker":
									modules.DigitalAntishaker = true;
									break;
								case "Display":
									modules.DisplayAnnotation = true;
									break;
							}
						}
						foreach (var m in vac.AnalyticsEngineConfiguration.Extension.Any.Select(x => x.Deserialize<syn::DefaultModule>()).Select(x => x.type.Name)) {
							switch (m) {
								case "SceneCalibrator":
									if (modules.SceneCalibrator == null) {
										modules.SceneCalibrator = false;
									}
									break;
								case "ObjectTracker":
									if (modules.ObjectTracker == null) {
										modules.ObjectTracker = false;
									}
									break;
								case "RuleEngine":
									if (modules.RuleEngine == null) {
										modules.RuleEngine = false;
									}
									break;
								case "ServiceDetectors":
									if (modules.ServiceDetectors == null) {
										modules.ServiceDetectors = false;
									}
									break;
								case "DigitalAntishaker":
									if (modules.DigitalAntishaker == null) {
										modules.DigitalAntishaker = false;
									}
									break;
								case "Display":
									if (modules.DisplayAnnotation == null) {
										modules.DisplayAnnotation = false;
									}
									break;
							}
						}
					}
				}

				var res = new Size() {
					Width = profile.VideoEncoderConfiguration.Resolution.Width,
					Height = profile.VideoEncoderConfiguration.Resolution.Height
				};

				var streamSetup = new med::StreamSetup();
				streamSetup.Stream = med::StreamType.RTPUnicast;
				streamSetup.Transport = new med::Transport();
				streamSetup.Transport.Protocol = med::TransportProtocol.UDP;
				streamSetup.Transport.Tunnel = null;

				string uri = null;
				yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => uri = x);
				dbg.Assert(uri != null);

				channels.Add(new Channel() {
					m_videoSourceToken = v.token,
					m_name = profile.Name,
					m_modules = modules,
					m_encoderResolution = res,
					m_mediaUri = uri
					//m_snapshot = snapshot
					//m_Capabilities = caps
				});


				

			}
			capabilities = caps;
			//AnalyticsCapabilities analyticsField;
			//DeviceCapabilities deviceField;
			//EventCapabilities eventsField;
			//ImagingCapabilities imagingField;
			//MediaCapabilities mediaField;
			//PTZCapabilities pTZField;
			
			m_Channels = channels.ToArray();
			
			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected bool HasAnalytics(ChannelDescription channel){
			if(channel.modules == null){
				return false;
			}
			if (channel.modules.DigitalAntishaker.HasValue && channel.modules.DigitalAntishaker.Value) {
				return true;
			}
			if (channel.modules.DisplayAnnotation.HasValue && channel.modules.DisplayAnnotation.Value) {
				return true;
			}
			if (channel.modules.ObjectTracker.HasValue && channel.modules.ObjectTracker.Value) {
				return true;
			}
			if (channel.modules.RuleEngine.HasValue && channel.modules.RuleEngine.Value) {
				return true;
			}
			if (channel.modules.SceneCalibrator.HasValue && channel.modules.SceneCalibrator.Value) {
				return true;
			}
			if (channel.modules.ServiceDetectors.HasValue && channel.modules.ServiceDetectors.Value) {
				return true;
			}			
			return false;
		}

		
		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<DeviceCapabilityModel> observer) {

			MediaObservable media = null;
			Capabilities caps = null;
			med::VideoSource[] vsources = null;
			med::Profile[] profiles = null;
			var channels = new List<Channel>();
			
			
			//yield return Observable.Merge(
			yield return session.GetMediaClient().Handle(x => media = x);
			yield return session.GetVideoSources().Handle(x => vsources = x ?? new med::VideoSource[] { });
			yield return session.GetProfiles().Handle(x => profiles = x);
			yield return session.GetCapabilities().Handle(x => caps = x);
			//);
			
			dbg.Assert(profiles != null);
			foreach (var channel in m_Channels) {
				var profile_token = NvcHelper.GetChannelProfileToken(channel.Id);
				var profile = profiles.Where(x => x.token == profile_token).FirstOrDefault();
				if (profile == null) {
					yield return session.CreateDefaultProfile(channel.Id).Handle(x => profile = x);
				}

				//if (HasAnalytics(channel)) {
				//    yield return session.AddDefaultVideoAnalytics(profile).Idle();
				//} else if (profile.VideoAnalyticsConfiguration != null) {
				//    yield return media.RemoveVideoAnalyticsConfiguration(profile.token).Idle();
				//    profile.VideoAnalyticsConfiguration = null;
				//}
				var vac = profile.VideoAnalyticsConfiguration;

				if (vac != null) {
					if (channel.modules.DigitalAntishaker.GetValueOrDefault(false)) {
						yield return session.GetVideoAnalyticModule(profile, "DigitalAntishaker").Idle();
					} else if(channel.modules.DigitalAntishaker.HasValue) {
						yield return session.RemoveVideoAnalyticModule(profile, "DigitalAntishaker").Idle();
					}

					if (channel.modules.DisplayAnnotation.GetValueOrDefault(false)) {
						yield return session.GetVideoAnalyticModule(profile, "Display").Idle();
					} else if(channel.modules.DisplayAnnotation.HasValue){
						yield return session.RemoveVideoAnalyticModule(profile, "Display").Idle();
					}

					if (channel.modules.SceneCalibrator.GetValueOrDefault(false)) {
						yield return session.GetVideoAnalyticModule(profile, "SceneCalibrator").Idle();
					} else if(channel.modules.SceneCalibrator.HasValue) {
						yield return session.RemoveVideoAnalyticModule(profile, "SceneCalibrator").Idle();
					}

					if (channel.modules.ObjectTracker.GetValueOrDefault(false)) {
						yield return session.GetVideoAnalyticModule(profile, "ObjectTracker").Idle();
					} else if(channel.modules.ObjectTracker.HasValue) {
						yield return session.RemoveVideoAnalyticModule(profile, "ObjectTracker").Idle();
					}

					if (channel.modules.RuleEngine.GetValueOrDefault(false)) {
						yield return session.GetVideoAnalyticModule(profile, "RuleEngine").Idle();
					} else if(channel.modules.RuleEngine.HasValue) {
						yield return session.RemoveVideoAnalyticModule(profile, "RuleEngine").Idle();
					}

					if (channel.modules.ServiceDetectors.GetValueOrDefault(false)) {
						yield return session.GetVideoAnalyticModule(profile, "ServiceDetectors").Idle();
					} else if (channel.modules.ServiceDetectors.HasValue) {
						yield return session.RemoveVideoAnalyticModule(profile, "ServiceDetectors").Idle();
					}
				}
			}
			
			if (observer != null) {
				observer.OnNext(this);
			}
		}

		private ChannelDescription[] m_Channels;
		public IEnumerable<ChannelDescription> Channels { 
			get{
				return m_Channels;
			} 
		}
		public Capabilities capabilities;
		//Image _deviceImage;
		//public Image DeviceImage {
		//    get {
		//        return _deviceImage;
		//    }
		//    set {
		//        _deviceImage = value;
		//    }
		//}
		public DeviceInfo devInfo{get; private set;}
		
	}
}
