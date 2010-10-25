using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.onvif;

using onvif.services.media;
using onvifdm.utils;
namespace nvc.models {
	public class LiveVideoModel : ModelBase<LiveVideoModel> {
		ChannelDescription m_channel;
		public LiveVideoModel(ChannelDescription channel) {
			m_channel = channel;
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<LiveVideoModel> observer) {
			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

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

			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => mediaUri = x);
			DebugHelper.Assert(mediaUri != null);
			NotifyPropertyChanged(x => x.mediaUri);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public string mediaUri {
			get;
			private set;
		}
	}
}
