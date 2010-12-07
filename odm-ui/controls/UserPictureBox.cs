using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nvc.controls{
	public class UserPictureBox :Control {
		public UserPictureBox():base() {
			DoubleBuffered = false;
		}

		public void RefresImage() {
			isOnPaint = true;
			Invalidate();
		}
		bool isOnPaint = false;
		protected override void OnPaintBackground(PaintEventArgs pevent) {

		}
		protected override void OnPaint(PaintEventArgs pe) {
			//if (isOnPaint) {
			//    base.OnPaint(pe);
			//    isOnPaint = false;
			//}
		}
	}
}
