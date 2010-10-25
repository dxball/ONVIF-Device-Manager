using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nvc.controls {
	class NumericTextBox : TextBox {

		protected override void OnKeyPress(KeyPressEventArgs e) {
			base.OnKeyPress(e);
		}
		protected override void OnValidating(System.ComponentModel.CancelEventArgs e) {
			base.OnValidating(e);
		}
		protected override void OnModifiedChanged(EventArgs e) {
			base.OnModifiedChanged(e);
		}
		
	}
}
