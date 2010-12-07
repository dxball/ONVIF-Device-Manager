using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using System.Windows.Forms;
using nvc.models;

namespace nvc.controlsUIProvider {
	public class DigitalIOProvider : BaseUIProvider {
		PropertyDigitalIO _digitalIO;
		public void InitView() {
			_digitalIO = new PropertyDigitalIO() { Dock = DockStyle.Fill };
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_digitalIO);
		}
		public override void ReleaseUI() {
			if (_digitalIO != null && !_digitalIO.IsDisposed)
				_digitalIO.ReleaseAll();
		}
	}
}
