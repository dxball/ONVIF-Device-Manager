using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.onvif;
using System.Collections;
using System.Drawing;

namespace odm.models {
	//public class EventsDisplayModel {
	//    public EventsDisplayModel(ChannelDescription channel) { }
	//    public Queue<mEventDescriptor> events{get;set;}
	//}

	public class mEventDescriptor {
		public Image screen { get; set; }
		public string id { get; set; }
		public DateTime datetime { get; set; }
		public string type { get; set; }
		public string details { get; set; }
	}
}
