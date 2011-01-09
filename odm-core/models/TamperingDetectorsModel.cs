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

namespace odm.models {

	public class TamperingDetectorsModel : ModelBase<TamperingDetectorsModel> {
		ChannelDescription m_channel;
		public TamperingDetectorsModel(ChannelDescription channel) {
			m_channel = channel;
		}
		
		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<TamperingDetectorsModel> observer) {
			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			tt::Capabilities caps = null;
			yield return session.GetCapabilities().Handle(x => caps = x);
			dbg.Assert(caps != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			if (profile == null) {
				//create default profile
				yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			}
			dbg.Assert(profile != null);

			if (profile.VideoSourceConfiguration == null) {
				//add default video source configuration
				VideoSourceConfiguration[] vscs = null;
				yield return session.GetVideoSourceConfigurations().Handle(x => vscs = x);
				dbg.Assert(vscs != null);
				var vsc = vscs.Where(x => x.SourceToken == m_channel.Id).FirstOrDefault();
				yield return session.AddVideoSourceConfiguration(profile.token, vsc.token).Idle();
				profile.VideoSourceConfiguration = vsc;
			}

			var vec = profile.VideoEncoderConfiguration;
			if (vec == null) {
				//add default video encoder
				yield return session.AddDefaultVideoEncoder(profile.token).Handle(x => vec = x);
				dbg.Assert(vec != null);
				profile.VideoEncoderConfiguration = vec;
			}

			if (caps.Analytics != null && caps.Analytics.AnalyticsModuleSupport) {
				yield return session.AddDefaultVideoAnalytics(profile).Idle();

				var vac = profile.VideoAnalyticsConfiguration;

				media::Config module = null;
				yield return session.GetVideoAnalyticModule(profile, "ServiceDetectors").Handle(x => module = x);
				dbg.Assert(module != null);
				
				m_blackout.SetBoth(module.GetSimpleItemAsBool("blackout"));
				m_overexposure.SetBoth(module.GetSimpleItemAsBool("overexposure"));
				m_outOfFocus.SetBoth(module.GetSimpleItemAsBool("out_of_focus"));
				m_displacement.SetBoth(module.GetSimpleItemAsBool("displacement"));
				m_obstruction.SetBoth(module.GetSimpleItemAsBool("obstruction"));
				m_channelstate.SetBoth(module.GetSimpleItemAsBool("channelstate"));
				
				NotifyPropertyChanged(x => x.blackout);
				NotifyPropertyChanged(x => x.overexposure);
				NotifyPropertyChanged(x => x.outOfFocus);
				NotifyPropertyChanged(x => x.displacement);
				NotifyPropertyChanged(x => x.obstruction);
				NotifyPropertyChanged(x => x.channelstate);
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

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<TamperingDetectorsModel> observer) {

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			yield return session.AddDefaultVideoAnalytics(profile).Idle();
			var vac = profile.VideoAnalyticsConfiguration;

			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "ServiceDetectors").Handle(x => module = x);
			dbg.Assert(module != null);

			module.SetSimpleItemAsBool("blackout", blackout);
			module.SetSimpleItemAsBool("overexposure", overexposure);
			module.SetSimpleItemAsBool("out_of_focus", outOfFocus);
			module.SetSimpleItemAsBool("displacement", displacement);
			module.SetSimpleItemAsBool("obstruction", obstruction);
			module.SetSimpleItemAsBool("channelstate", channelstate);

			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();

			yield return Observable.Concat(LoadImpl(session, observer)).Idle();
		}

		
		private ChangeTrackingProperty<bool> m_blackout = new ChangeTrackingProperty<bool>(false);
		public bool blackout {
			get {
				return m_blackout.current;
			}
			set {
				m_blackout.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.blackout);
			}
		}

		private ChangeTrackingProperty<bool> m_overexposure = new ChangeTrackingProperty<bool>(false);
		public bool overexposure {
			get {
				return m_overexposure.current;
			}
			set {
				m_overexposure.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.overexposure);
			}
		}

		private ChangeTrackingProperty<bool> m_outOfFocus = new ChangeTrackingProperty<bool>(false);
		public bool outOfFocus {
			get {
				return m_outOfFocus.current;
			}
			set {
				m_outOfFocus.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.outOfFocus);
			}
		}

		private ChangeTrackingProperty<bool> m_displacement = new ChangeTrackingProperty<bool>(false);
		public bool displacement {
			get {
				return m_displacement.current;
			}
			set {
				m_displacement.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.displacement);
			}
		}

		private ChangeTrackingProperty<bool> m_obstruction = new ChangeTrackingProperty<bool>(false);
		public bool obstruction {
			get {
				return m_obstruction.current;
			}
			set {
				m_obstruction.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.obstruction);
			}
		}

		private ChangeTrackingProperty<bool> m_channelstate = new ChangeTrackingProperty<bool>(false);
		public bool channelstate {
			get {
				return m_channelstate.current;
			}
			set {
				m_channelstate.SetCurrent(m_changeSet, value);
				NotifyPropertyChanged(x => x.channelstate);
			}
		}

		public Size encoderResolution {
			get;
			private set;
		}

		public string mediaUri {
			get;
			private set;
		}
	}
}
