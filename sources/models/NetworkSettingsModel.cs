using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using nvc.onvif;

namespace nvc.models {
	public partial class NetworkSettingsModel {
		public Func<Action<DeviceDescriptionModel>, IObservable<Unit>> Load(Session session);
		Session session {
			public get;
			private set;
		}

		bool DHCP { get; set; }
		IPAddress DeviceIPAddress { get; set; }
		IPAddress SubnetMask { get; set; }
		IPAddress GatewayAdress { get; set; }
		IPAddress DNSAddress { get; set; }
		string MACAddress { get; }
	}
}
