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

namespace nvc.models {
	public partial class DeviceCapabilityModel : ModelBase<DeviceCapabilityModel> {
		private class Channel:ChannelDescription{
			public string m_VideoSourceToken;
			public string m_Name;
			
			public override string Id {
				get {
					return m_VideoSourceToken;
				}
			}

			public override string Name {
				get {
					return m_VideoSourceToken;
				}
			}

			public override Capabilities Capabilities {
				get {
					var err = new NotImplementedException();
					DebugHelper.Error(err);
					throw err;
				}
			}
		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<DeviceCapabilityModel> observer) {
			
			Capabilities caps = null;
			med::VideoSource[] vsources = null;
			med::Profile[] profiles = null;
			var channels = new List<Channel>();
			
			//yield return Observable.Merge(
				yield return session.GetVideoSources().Handle(x => vsources = x);
				yield return session.GetProfiles().Handle(x => profiles = x);
				yield return session.GetCapabilities().Handle(x => caps = x);
			//);

			if (vsources == null) {
				vsources = new med::VideoSource[] { };
			}

			DebugHelper.Assert(profiles != null);
			foreach (var v in vsources) {
				var profile_token = NvcHelper.GetChannelProfileToken(v.token);
				var profile = profiles.Where(x => x.token == profile_token).FirstOrDefault();
				if (profile == null) {
					//profile = session.CreateDefaultProfile();					
				}
				//profile.MetadataConfiguration.
				channels.Add(new Channel() {
					m_VideoSourceToken = v.token,
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
			

		private ChannelDescription[] m_Channels;
		public IEnumerable<ChannelDescription> Channels { 
			get{
				return m_Channels;
			} 
		}
		public Capabilities capabilities;
		public Image DeviceImage {
			get {
				return null;
			}
		}
	}
}
