using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using System.Windows.Forms;
using odm.models;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class DigitalIOProvider : BaseUIProvider, IDigitalIOProvider {
		PropertyDigitalIO _digitalIO;
		public void InitView() {
			_digitalIO = new PropertyDigitalIO();
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_digitalIO);
		}
		public override void ReleaseUI() {
			if (_digitalIO != null) {
				_digitalIO.ReleaseAll();
				_digitalIO = null;
			}
		}
	}
}
