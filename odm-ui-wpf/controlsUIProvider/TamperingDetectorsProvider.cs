using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.controls;
using System.Windows.Forms;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class TamperingDetectorsProvider : BaseUIProvider, ITamperingDetectorsProvider {
		PropertyTamperingDetectors _tampering;
		public void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_tampering = new PropertyTamperingDetectors(devModel) { Save = ApplyChanges,
																	Cancel = CancelChanges,
																	onBindingError = BindingError
			};
			//if (datProcInfo != null)
			//    _tampering.memFile = datProcInfo.VideoProcessFile;
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_tampering);
		}
		public override void ReleaseUI() {
			if (_tampering != null) {
				_tampering.ReleaseAll();
				_tampering = null;
			}
		}
	}
}
