using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.controls;
using System.Windows.Forms;

namespace odm.controls.UIProvider {
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
