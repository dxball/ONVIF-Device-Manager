using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.onvif;

using nvc;
using nvc.onvif;
using onvifdm.utils;
using onvif.services.media;
using onvif.services.analytics;
using media = onvif.services.media;
using analytics = onvif.services.analytics;
using tt = onvif.types;
using System.Xml;
using System.Drawing;

namespace nvc.models {
	public class AnnotationsModel : ModelBase<AnnotationsModel> {
		ChannelDescription m_channel;
		public AnnotationsModel(ChannelDescription channel) {
			m_channel = channel;
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<AnnotationsModel> observer) {
			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			tt::Capabilities caps = null;
			yield return session.GetCapabilities().Handle(x => caps = x);
			DebugHelper.Assert(caps != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			if (profile == null) {
				//create default profile
				yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			}
			DebugHelper.Assert(profile != null);

			if (profile.VideoSourceConfiguration == null) {
				//add default video source configuration
				VideoSourceConfiguration[] vscs = null;
				yield return session.GetVideoSourceConfigurations().Handle(x => vscs = x);
				DebugHelper.Assert(vscs != null);
				var vsc = vscs.Where(x => x.SourceToken == m_channel.Id).FirstOrDefault();
				yield return session.AddVideoSourceConfiguration(profile.token, vsc.token).Idle();
				profile.VideoSourceConfiguration = vsc;
			}

			var vec = profile.VideoEncoderConfiguration;
			if (vec == null) {
				//add default video encoder
				yield return session.AddDefaultVideoEncoder(profile.token).Handle(x => vec = x);
				DebugHelper.Assert(vec != null);
				profile.VideoEncoderConfiguration = vec;
			}

			if (caps.Analytics != null && caps.Analytics.AnalyticsModuleSupport) {
				yield return session.AddDefaultVideoAnalytics(profile).Idle();
			
				var vac = profile.VideoAnalyticsConfiguration;

				media::Config module = null;
				yield return session.GetVideoAnalyticModule(profile, "Display").Handle(x => module = x);
				DebugHelper.Assert(module != null);

				m_timestamp.SetBoth(module.GetSimpleItemAsBool("timestamp"));
				m_tracking.SetBoth(module.GetSimpleItemAsBool("tracking"));
				m_movingRects.SetBoth(module.GetSimpleItemAsBool("moving_rects"));
				m_userRegion.SetBoth(module.GetSimpleItemAsBool("user_region"));
				m_speed.SetBoth(module.GetSimpleItemAsBool("speed"));
				
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
			DebugHelper.Assert(mediaUri != null);

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
			DebugHelper.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			yield return session.AddDefaultVideoAnalytics(profile).Idle();
			var vac = profile.VideoAnalyticsConfiguration;
			
			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "Display").Handle(x => module = x);
			DebugHelper.Assert(module != null);
			
			module.SetSimpleItemAsBool("timestamp", timestamp);
			module.SetSimpleItemAsBool("tracking", tracking);
			module.SetSimpleItemAsBool("moving_rects", movingRects);
			module.SetSimpleItemAsBool("user_region", userRegion);
			module.SetSimpleItemAsBool("speed", speed);			

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

		public Size encoderResolution{get; private set;}

		public string mediaUri {
			get;
			private set;
		}
	}
}
