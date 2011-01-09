using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.controls;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class AnalogueOutProvider : BaseUIProvider, IAnalogueOutProvider {
		PropertyAnalogueOut _analogueOut;
		public void InitView(DeviceIdentificationModel devmodel) {
			_analogueOut = new PropertyAnalogueOut(devmodel);
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_analogueOut);
		}
		public override void ReleaseUI() {
			if (_analogueOut != null){
				_analogueOut.ReleaseAll();
				_analogueOut = null;
			}
		}
	}
}
