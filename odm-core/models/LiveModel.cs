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
	public class LiveVideoModel : ModelBase<LiveVideoModel> {
		ChannelDescription m_channel;
		public LiveVideoModel(ChannelDescription channel) {
			m_channel = channel;
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<LiveVideoModel> observer) {
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

			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => mediaUri = x);
			dbg.Assert(mediaUri != null);

			encoderResolution = new Size() {
				Width = profile.VideoEncoderConfiguration.Resolution.Width,
				Height = profile.VideoEncoderConfiguration.Resolution.Height
			};

			NotifyPropertyChanged(x => x.encoderResolution);
			NotifyPropertyChanged(x => x.mediaUri);
			NotifyPropertyChanged(x => x.isModified);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public string mediaUri {
			get;
			private set;
		}
		public Size encoderResolution {get; private set;}
	}
}
