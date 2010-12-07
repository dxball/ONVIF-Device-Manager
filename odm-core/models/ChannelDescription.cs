using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using onvif.services.device;
using onvif.types;
using System.Drawing;

namespace nvc.models {
	public abstract class ChannelDescription {
		public abstract string Id {get;}
		public abstract string Name {get;}
		public abstract Size encoderResolution {get;}
		public abstract string mediaUri {get;}
		//public abstract Capabilities Capabilities {get;}
		//public abstract Image snapshot{get;}
		public abstract AnalyticsModules modules{get;}
	}

	enum CapabilityID {
	}
	//public class Capabilities {
	//    CapabilityID ID { get; set; }
	//    bool Enable { get; set; }
	//}
}
