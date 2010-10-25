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
		public SetSize(int size, Point location) {
			InitializeComponent();

			_location = location;
			Load += new EventHandler(SetSize_Load);
			_size.Value = size;

			this.CreateBinding(x => x.Text, DepthCalibrationStrings.Instance, x => x.title);
			_lblSize.CreateBinding(x => x.Text, DepthCalibrationStrings.Instance, x => x.physHeight);
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
