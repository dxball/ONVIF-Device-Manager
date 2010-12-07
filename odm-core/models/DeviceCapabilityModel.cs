using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using nvc.onvif;
using onvifdm.utils;

using onvif.types;
using onvif.services.device;
using onvif.services.media;

using dev = global::onvif.services.device;
using med = global::onvif.services.media;
using tt = global::onvif.types;
using System.Xml;

namespace nvc.models {

	public class AnalyticsModules {
		public bool SceneCalibrator = false;
		public bool ServiceDetectors = false;
		public bool DisplayAnnotation = false;
		public bool ObjectTracker = false;
		public bool RuleEngine = false;
		public bool DigitalAntishaker = false;
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
			
			//yield return Observable.Merge(
				yield return session.GetVideoSources().Handle(x => vsources = x ?? new med::VideoSource[] { });
				yield return session.GetProfiles().Handle(x => profiles = x);
				yield return session.GetCapabilities().Handle(x => caps = x);
				yield return session.GetDeviceInfo().Handle(x => devInfo = x);
			//);
			
			DebugHelper.Assert(profiles != null);
			foreach (var v in vsources) {
				var profile_token = NvcHelper.GetChannelProfileToken(v.token);
				var profile = profiles.Where(x => x.token == profile_token).FirstOrDefault();
				if (profile == null) {
					yield return session.CreateDefaultProfile(v.token).Handle(x => profile = x);					
				}
				//Image snapshot = null;
				//yield return session.GetSnapshot(profile.token).Handle(x=>snapshot = x);
				var vac = profile.VideoAnalyticsConfiguration;
				var modules = new AnalyticsModules();
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
				DebugHelper.Assert(uri != null);

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
			if (channel.modules.DigitalAntishaker) {
				return true;
			}
			if (channel.modules.DisplayAnnotation) {
				return true;
			}
			if (channel.modules.ObjectTracker) {
				return true;
			}
			if (channel.modules.RuleEngine) {
				return true;
			}
			if (channel.modules.SceneCalibrator) {
				return true;
			}
			if (channel.modules.ServiceDetectors) {
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
			
			DebugHelper.Assert(profiles != null);
			foreach (var channel in m_Channels) {
				var profile_token = NvcHelper.GetChannelProfileToken(channel.Id);
				var profile = profiles.Where(x => x.token == profile_token).FirstOrDefault();
				if (profile == null) {
					yield return session.CreateDefaultProfile(channel.Id).Handle(x => profile = x);
				}

				if (HasAnalytics(channel)) {
					yield return session.AddDefaultVideoAnalytics(profile).Idle();
				} else if (profile.VideoAnalyticsConfiguration != null) {
					yield return media.RemoveVideoAnalyticsConfiguration(profile.token).Idle();
					profile.VideoAnalyticsConfiguration = null;
				}
				var vac = profile.VideoAnalyticsConfiguration;

				if (vac != null) {
					if (channel.modules.DigitalAntishaker) {
						yield return session.GetVideoAnalyticModule(profile, "DigitalAntishaker").Idle();
					} else {
						yield return session.RemoveVideoAnalyticModule(profile, "DigitalAntishaker").Idle();
					}

					if (channel.modules.DisplayAnnotation) {
						yield return session.GetVideoAnalyticModule(profile, "Display").Idle();
					} else {
						yield return session.RemoveVideoAnalyticModule(profile, "Display").Idle();
					}

					if (channel.modules.SceneCalibrator) {
						yield return session.GetVideoAnalyticModule(profile, "SceneCalibrator").Idle();
					} else {
						yield return session.RemoveVideoAnalyticModule(profile, "SceneCalibrator").Idle();
					}

					if (channel.modules.ObjectTracker) {
						yield return session.GetVideoAnalyticModule(profile, "ObjectTracker").Idle();
					} else {
						yield return session.RemoveVideoAnalyticModule(profile, "ObjectTracker").Idle();
					}

					if (channel.modules.RuleEngine) {
						yield return session.GetVideoAnalyticModule(profile, "RuleEngine").Idle();
					} else {
						yield return session.RemoveVideoAnalyticModule(profile, "RuleEngine").Idle();
					}

					if (channel.modules.ServiceDetectors) {
						yield return session.GetVideoAnalyticModule(profile, "ServiceDetectors").Idle();
					} else {
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
