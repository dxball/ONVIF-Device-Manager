using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace odm.controls {
	public partial class IPAddressControl : UserControl {
		public IPAddressControl() {
			InitializeComponent();
			//_IPtextBox;
			//errorProvider1

			EnabledChanged += new EventHandler(IPAddressControl_EnabledChanged);
		}

		void IPAddressControl_EnabledChanged(object sender, EventArgs e) {
			if (!Enabled)
				_IPtextBox.BackColor = ColorDefinition.colControlBackground;
			else
				_IPtextBox.BackColor = Color.FromKnownColor(KnownColor.Window);
		}
		public override string Text {
			get {
				return _IPtextBox.Text;
			}
			set {
				_IPtextBox.Text = value;
			}
		}
		public System.Net.IPAddress IPAddress {
			get {
				System.Net.IPAddress ipaddr;
				return System.Net.IPAddress.TryParse(_IPtextBox.Text, out ipaddr) ? ipaddr : null;
			}
			set {
				_IPtextBox.Text = value == null ? "" : value.ToString();
			}
		}
		public void SetIPAddress(System.Net.IPAddress ipaddr) {
			_IPtextBox.Text = ipaddr.ToString();
		}
		public System.Net.IPAddress GetIPAddress() {
			System.Net.IPAddress ipaddr;
			return System.Net.IPAddress.TryParse(_IPtextBox.Text, out ipaddr) ? ipaddr : null;
		}
	}
}
