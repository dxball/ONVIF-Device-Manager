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

namespace nvc.models {
	public class AntishakerModel : ModelBase<AntishakerModel> {
		ChannelDescription m_channel;
		public AntishakerModel(ChannelDescription channel) {
			m_channel = channel;
		}
		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<AntishakerModel> observer) {
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
				yield return session.GetVideoAnalyticModule(profile, "DigitalAntishaker").Handle(x => module = x);
				DebugHelper.Assert(module != null);

				var crop_rect = module.Parameters.ElementItem
					.Where(x => x.Name == "crop_rect")
					.Select(x => x.Any.Deserialize<tt::IntRectangle>())
					.FirstOrDefault();
				if (crop_rect != null) {
					croppingRectangle.X = crop_rect.x;
					croppingRectangle.Y = crop_rect.y;
					croppingRectangle.Width = crop_rect.width;
					croppingRectangle.Height = crop_rect.height;
				} 
			}

			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => mediaUri = x);
			DebugHelper.Assert(mediaUri != null);
			NotifyPropertyChanged(x => x.mediaUri);

			NotifyPropertyChanged(x => x.isModified);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<AntishakerModel> observer) {

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
			yield return session.GetVideoAnalyticModule(profile, "DigitalAntishaker").Handle(x => module = x);
			DebugHelper.Assert(module != null);


			var crop_rect = module.Parameters.ElementItem
				.Where(x => x.Name == "crop_rect")
				.FirstOrDefault();
			
			if (crop_rect == null) {
				crop_rect = new media.ItemListElementItem();
				module.Parameters.ElementItem = module.Parameters.ElementItem.Append(crop_rect).ToArray();
			}

			crop_rect.Any = new tt::IntRectangle() {
				x = croppingRectangle.X,
				y = croppingRectangle.Y,
				width = croppingRectangle.Width,
				height = croppingRectangle.Height,
			}.Serialize();
			
			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();
			yield return Observable.Concat(LoadImpl(session, observer)).Idle();
		}

		public System.Drawing.Rectangle croppingRectangle;
		public string mediaUri {
			get;
			private set;
		}
	}
}
