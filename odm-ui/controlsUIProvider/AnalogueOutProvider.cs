using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.controls;
using System.Windows.Forms;

namespace nvc.controlsUIProvider {
	public class AnalogueOutProvider : BaseUIProvider {
		PropertyAnalogueOut _analogueOut;
		public void InitView(DeviceIdentificationModel devmodel) {
			_analogueOut = new PropertyAnalogueOut(devmodel) { Dock = DockStyle.Fill, 
				onBindingError = BindingError };
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_analogueOut);
		}
		public override void ReleaseUI() {
			if (_analogueOut != null && !_analogueOut.IsDisposed)
				_analogueOut.ReleaseAll();
		}
	}
}
