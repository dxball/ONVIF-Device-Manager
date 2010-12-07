using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nvc.controls {
	public partial class SelectableImage : UserControl {
		public SelectableImage() {
			InitializeComponent();

			_imgBox.Click += new EventHandler(_imgBox_Click);
		}
		void _imgBox_Click(object sender, EventArgs e) {
			BackColor = ColorDefinition.colHighlightedImage;
		}
	}
}
