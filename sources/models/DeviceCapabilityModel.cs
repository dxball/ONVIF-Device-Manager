using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using nvc.onvif;
using nvc.utils;

using dev=global::onvif.services.device;
using med = global::onvif.services.media;
using onvif.services.device;
using onvif.services.media;

namespace nvc.models {
	public partial class DeviceCapabilityModel : ModelBase<DeviceCapabilityModel> {
		private class Channel:ChannelDescription{
			public string m_VideoSourceToken;
			public string m_Name;
			public dev::Capabilities m_Capabilities;

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

			public override dev::Capabilities Capabilities {
				get {
					throw new NotImplementedException();
				}
			}
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<DeviceCapabilityModel> observer) {

			Capabilities caps = null;
			med::VideoSource[] vsources = null;
			Profile[] profiles = null;
			var channels = new List<Channel>();

			yield return Observable.Merge(
				session.GetVideoSources().Handle(x => vsources = x),
				session.GetProfiles().Handle(x => profiles = x),
				session.GetCapabilities().Handle(x => caps = x)
			);

			DebugHelper.Assert(vsources != null);
			DebugHelper.Assert(profiles != null);
			foreach(var v in vsources){
				var profile_token = NvcHelper.GetChannelProfileToken(v.token);
				var profile = profiles.Where(x=>x.token == profile_token).FirstOrDefault();
				if(profile == null){
					//profile = session.CreateDefaultProfile();					
				}
				//profile.MetadataConfiguration.
				channels.Add(new Channel() {
					m_VideoSourceToken = v.token,
					m_Capabilities = caps
				});
			}
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

		public Image DeviceImage {
			get {
				return null;
			}
		}
	}
}
