using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using onvif.services.media;

namespace odm.controllers {
	public class ChannelDescription {
		public ChannelDescription(string id){
			Id = id;
		}
		List<Profile> profiles;
		public List<Profile> Profiles {
			get {
				if (profiles == null)
					profiles = new List<Profile>();
				return profiles;
			}
		}
		public string sourceToken { get { return CurrentProfile.VideoSourceConfiguration.SourceToken; } private set { } } 
		public Profile CurrentProfile { get; set; }
		public string Id { get;private set;}
		public string Name {
			get {
				return CurrentProfile.Name;
			}
		}
		public System.Drawing.Size encoderResolution {
			get {
				return new System.Drawing.Size(CurrentProfile.VideoEncoderConfiguration.Resolution.Width, CurrentProfile.VideoEncoderConfiguration.Resolution.Height);
			}
		}
		public string mediaUri { get; private set; }

		public AnalyticsModules modules { get; private set; }
	}

	enum CapabilityID {
	}
	public class Capabilities {
		CapabilityID ID { get; set; }
		bool Enable { get; set; }
	}
}
