using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.onvif;

using onvif.services.media;
using nvc.utils;
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

			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			string uri = null;
			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => uri = x);
			DebugHelper.Assert(uri != null);

			MediaUri = uri;
			NotifyPropertyChanged(x => x.MediaUri);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public string MediaUri {
			get;
			private set;
		}
	}
}
