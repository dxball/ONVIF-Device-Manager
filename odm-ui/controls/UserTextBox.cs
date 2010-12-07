using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nvc.controls {
	public class UserTextBox: TextBox {
		public UserTextBox():base(){
			DoubleBuffered = true;
		}
		protected override void OnPaintBackground(PaintEventArgs pevent) {
			
		}
	}
}
