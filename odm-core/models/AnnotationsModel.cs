using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using odm.onvif;
using odm.utils;
using onvif.services.media;
using onvif.services.analytics;
using media = onvif.services.media;
using analytics = onvif.services.analytics;
using tt = onvif.types;
using onvif;

namespace odm.models {
	public class AnnotationsModel : ModelBase<AnnotationsModel> {
		//ChannelDescription m_channel;
		//public AnnotationsModel(ChannelDescription channel) {
		//    m_channel = channel;
		//}
		ProfileToken m_profileToken;
		public AnnotationsModel(ProfileToken profileToken) {
			this.m_profileToken = profileToken;
		}
		
		
		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<AnnotationsModel> observer) {
			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			tt::Capabilities caps = null;
			yield return session.GetCapabilities().Handle(x => caps = x);
			dbg.Assert(caps != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			//if (profile == null) {
			//    //create default profile
			//    yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			//}
			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();
			dbg.Assert(profile != null);

			//if (profile.VideoSourceConfiguration == null) {
			//    //add default video source configuration
			//    VideoSourceConfiguration[] vscs = null;
			//    yield return session.GetVideoSourceConfigurations().Handle(x => vscs = x);
			//    dbg.Assert(vscs != null);
			//    var vsc = vscs.Where(x => x.SourceToken == m_channel.Id).FirstOrDefault();
			//    yield return session.AddVideoSourceConfiguration(profile.token, vsc.token).Idle();
			//    profile.VideoSourceConfiguration = vsc;
			//}

			yield return session.AddDefaultVideoEncoder(profile).Idle();
			var vec = profile.VideoEncoderConfiguration;
			dbg.Assert(vec != null);

			if (caps.Analytics != null && caps.Analytics.AnalyticsModuleSupport) {
				yield return session.AddDefaultVideoAnalytics(profile).Idle();
			
				var vac = profile.VideoAnalyticsConfiguration;

				media::Config module = null;
				yield return session.GetVideoAnalyticModule(profile, "Display").Handle(x => module = x);
				dbg.Assert(module != null);

				m_timestamp.SetBoth(module.GetSimpleItemAsBool("timestamp"));
				m_tracking.SetBoth(module.GetSimpleItemAsBool("tracking"));
				m_movingRects.SetBoth(module.GetSimpleItemAsBool("moving_rects"));
				m_userRegion.SetBoth(module.GetSimpleItemAsBool("user_region"));
				m_speed.SetBoth(module.GetSimpleItemAsBool("speed"));
				m_calibrationResults.SetBoth(module.GetSimpleItemAsBool("calibration_results"));
				m_channelName.SetBoth(module.GetSimpleItemAsBool("channel_name"));
				m_systemInfo.SetBoth(module.GetSimpleItemAsBool("system_information"));

				NotifyPropertyChanged(x => x.channelName);
				NotifyPropertyChanged(x => x.systemInfo);
				NotifyPropertyChanged(x => x.calibrationResults);
				NotifyPropertyChanged(x => x.timestamp);
				NotifyPropertyChanged(x => x.tracking);
				NotifyPropertyChanged(x => x.movingRects);
				NotifyPropertyChanged(x => x.userRegion);
				NotifyPropertyChanged(x => x.speed);
			}

			encoderResolution = new Size() {
				Width = profile.VideoEncoderConfiguration.Resolution.Width,
				Height = profile.VideoEncoderConfiguration.Resolution.Height
			};

			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => mediaUri = x);
			dbg.Assert(mediaUri != null);

			NotifyPropertyChanged(x => x.encoderResolution);
			NotifyPropertyChanged(x => x.mediaUri);
			NotifyPropertyChanged(x => x.isModified);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<AnnotationsModel> observer) {

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();
			yield return session.AddDefaultVideoAnalytics(profile).Idle();
			var vac = profile.VideoAnalyticsConfiguration;
			
			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "Display").Handle(x => module = x);
			dbg.Assert(module != null);

			//module.SetSimpleItemAsBool("calibration_results", false);
			module.SetSimpleItemAsBool("calibration_results", calibrationResults);
			module.SetSimpleItemAsBool("timestamp", timestamp);
			module.SetSimpleItemAsBool("tracking", tracking);
			module.SetSimpleItemAsBool("moving_rects", movingRects);
			module.SetSimpleItemAsBool("user_region", userRegion);
			module.SetSimpleItemAsBool("speed", speed);
			module.SetSimpleItemAsBool("channel_name", channelName);
			module.SetSimpleItemAsBool("system_information", systemInfo);

			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();

			yield return Observable.Concat(LoadImpl(session, observer)).Idle();
		}

		private ChangeTrackingProperty<bool> m_timestamp = new ChangeTrackingProperty<bool>(false);
		public bool timestamp {
			get { return m_timestamp.current; }
			set {
				m_timestamp.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x=>x.timestamp);
			}
		}
		private ChangeTrackingProperty<bool> m_tracking = new ChangeTrackingProperty<bool>(false);
		public bool tracking {
			get { return m_tracking.current; }
			set {
				m_tracking.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.tracking);
			}
		}
		private ChangeTrackingProperty<bool> m_movingRects = new ChangeTrackingProperty<bool>(false);
		public bool movingRects{
			get { return m_movingRects.current; }
			set { m_movingRects.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x=>x.movingRects);}
		}
		private ChangeTrackingProperty<bool> m_userRegion = new ChangeTrackingProperty<bool>(false);
		public bool userRegion{
			get { return m_userRegion.current; }
			set { m_userRegion.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x=>x.userRegion);}
		}
		private ChangeTrackingProperty<bool> m_speed = new ChangeTrackingProperty<bool>(false);
		public bool speed{
			get { return m_speed.current; }
			set { m_speed.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x=>x.speed);}
		}

		private ChangeTrackingProperty<bool> m_calibrationResults = new ChangeTrackingProperty<bool>(false);
		public bool calibrationResults {
			get {
				return m_calibrationResults.current;
			}
			set {
				m_calibrationResults.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.calibrationResults);
			}
		}

		private ChangeTrackingProperty<bool> m_channelName = new ChangeTrackingProperty<bool>(false);
		public bool channelName {
			get {
				return m_channelName.current;
			}
			set {
				m_channelName.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.channelName);
			}
		}

		private ChangeTrackingProperty<bool> m_systemInfo = new ChangeTrackingProperty<bool>(false);
		public bool systemInfo {
			get {
				return m_systemInfo.current;
			}
			set {
				m_systemInfo.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.systemInfo);
			}
		}

		public Size encoderResolution{get; private set;}

		public string mediaUri {
			get;
			private set;
		}
	}
}
