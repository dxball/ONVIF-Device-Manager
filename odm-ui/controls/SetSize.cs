using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nvc.controls {
	public partial class SetSize : Form {

		PropertyDepthCalibrationStrings _strings = new PropertyDepthCalibrationStrings();

		public SetSize(Size size, Point location, bool Is2D) {
			InitializeComponent();

			_location = location;
			Load += new EventHandler(SetSize_Load);
			_heigth.Value = size.Height;
			_width.Value = size.Width;

			_lblWidth.Visible = Is2D;
			_width.Visible = Is2D;

			this.CreateBinding(x => x.Text, _strings, x => x.title);
			_lblSize.CreateBinding(x => x.Text, _strings, x => x.physHeight);
		}
		Point _location;
		void SetSize_Load(object sender, EventArgs e) {
			Location = _location;			
		}

		private void _btnSetSize_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
