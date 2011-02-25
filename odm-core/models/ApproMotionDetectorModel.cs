using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Globalization;

using odm.onvif;
using odm.utils;
using onvif.services.media;
using onvif.services.analytics;
using media = global::onvif.services.media;
using analytics = global::onvif.services.analytics;
using syn = global::synesis.onvif.extensions;
using tt = global::onvif.types;
using onvif;

namespace odm.models {

	public partial class ApproMotionDetectorModel : ModelBase<ApproMotionDetectorModel> {
		//ChannelDescription m_channel;
		//public ApproMotionDetectorModel(ChannelDescription channel) {
		//    m_channel = channel;
		//}
		ProfileToken m_profileToken;
		public ApproMotionDetectorModel(ProfileToken profileToken) {
			this.m_profileToken = profileToken;
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<ApproMotionDetectorModel> observer) {
			AnalyticsObservable analytics = null;
			yield return session.GetAnalyticsClient().Handle(x => analytics = x);
			dbg.Assert(analytics != null);		

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			//if (profile == null) {
			//    yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			//}
			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();
			dbg.Assert(profile != null);

			yield return session.AddDefaultVideoAnalytics(profile).Idle();
			
			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "ApproMotionDetector").Handle(x => module = x);
			dbg.Assert(module != null);

			//if (module == null) {
			//    var vac = profile.VideoAnalyticsConfiguration;
			//    if (vac == null) {
			//        throw new Exception("specified profile does not contains analytics");
			//    }

			//    var mod = vac.AnalyticsEngineConfiguration.AnalyticsModule.Where(x=>x!=null).Where(x => x.Type == new XmlQualifiedName("ApproMotionDetector")).FirstOrDefault();
			//    if (mod == null) {
					
			//        media::Config defMod = new media::Config();
			//        defMod.Type = new XmlQualifiedName("ApproMotionDetector");
			//        defMod.Name = "Default ApproMotionDetector Module";
			//        defMod.SetSimpleItemAsInt("sensitivity", 8);
			//        defMod.SetSimpleItemAsInt("region_mask", 33825);
			//        vac.AnalyticsEngineConfiguration.AnalyticsModule = vac.AnalyticsEngineConfiguration.AnalyticsModule.Where(x => x != null).Append(defMod).ToArray();
			//        yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();
			//        mod = defMod;
			//    }
			//}

			m_sensitivity.SetBoth(module.GetSimpleItemAsInt("sensitivity"));
			m_regionMask.SetBoth(module.GetSimpleItemAsInt("region_mask"));
			
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

			NotifyPropertyChanged(x => x.sensitivity);
			NotifyPropertyChanged(x => x.regionMask);
			NotifyPropertyChanged(x => x.encoderResolution);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<ApproMotionDetectorModel> observer) {

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();
			var vac = profile.VideoAnalyticsConfiguration;
			if (vac == null) {
				VideoAnalyticsConfiguration[] vacs =null;
				yield return session.GetCompatibleVideoAnalyticsConfigurations(profile.token).Handle(x=>vacs = x);
				vac = vacs.OrderBy(x=>x.UseCount).FirstOrDefault();
				yield return session.AddVideoAnalyticsConfiguration(profile.token, vac.token).Idle();
				profile.VideoAnalyticsConfiguration = vac;
			}
			
			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "ApproMotionDetector").Handle(x => module = x);
			dbg.Assert(module != null);


			module.SetSimpleItemAsFloat("sensitivity", sensitivity);
			module.SetSimpleItemAsFloat("region_mask", regionMask);

			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();

			yield return Observable.Concat(LoadImpl(session, observer)).Idle();
		}

		public override void RevertChanges() {

			m_sensitivity.Revert();
			m_regionMask.Revert();

			NotifyPropertyChanged(x => x.sensitivity);
			NotifyPropertyChanged(x => x.regionMask);			
		}
		
		private ChangeTrackingProperty<int> m_sensitivity = new ChangeTrackingProperty<int>();
		public int sensitivity {
			get {
				return m_sensitivity.current;
			}
			set {
				if (m_sensitivity.current != value) {
					m_sensitivity.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.sensitivity);
				}
			}
		}

		private ChangeTrackingProperty<int> m_regionMask = new ChangeTrackingProperty<int>();
		public int regionMask {
			get {
				return m_regionMask.current;
			}
			set{
				if(m_regionMask.current!=value){
					m_regionMask.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.regionMask);
				}
			}
		}
		
		public Size encoderResolution{get; private set;}

		public string mediaUri {
			get;
			private set;
		}
	}
}