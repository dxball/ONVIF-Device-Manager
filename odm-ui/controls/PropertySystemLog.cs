using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nvc.controls {
	public partial class PropertySystemLog : BasePropertyControl {
		SystemLogStrings _strings = new SystemLogStrings();
		public PropertySystemLog() {
			InitializeComponent();

			Localization();
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
		}
	}
}
