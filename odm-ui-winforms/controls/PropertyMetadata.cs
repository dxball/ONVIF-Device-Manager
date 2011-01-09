using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using odm.utils;

namespace odm.controls {
	public partial class PropertyMetadata : BasePropertyControl {
		PropertyMetadataStrings _strings = new PropertyMetadataStrings();
		public PropertyMetadata() {
			InitializeComponent();

			InitControl();
		}
		void InitControl() {
			Localization();
		}
		void Localization() {
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
		}

		public void AppendData(string data) {
			if (_tbMetadata.Text.Length > 19048)
				_tbMetadata.Text = "";
			_tbMetadata.Text += _tbMetadata.Text + data;
		}
	}
}
