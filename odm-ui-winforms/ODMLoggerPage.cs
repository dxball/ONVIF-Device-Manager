using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace odm.utils {
	public partial class ODMLoggerPage : UserControl {
		public ODMLoggerPage(string html) {
			InitializeComponent();

			InitBrouser(html);
		}

		void InitBrouser(string html) {
			_webBrowser.DocumentText = html;
		}
	}
}
