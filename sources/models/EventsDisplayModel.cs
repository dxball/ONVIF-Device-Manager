using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.onvif;
using System.Collections;
using System.Drawing;

namespace nvc.models {
	public class EventsDisplayModel {
		public EventsDisplayModel(ChannelDescription channel) { }
		Queue<EventDescriptor> events{get;set;}
	}

	public class EventDescriptor {
		public Image screen { get; set; }
		public string id { get; set; }
		public DateTime datetime { get; set; }
		public string type { get; set; }
		public string details { get; set; }
	}
}
