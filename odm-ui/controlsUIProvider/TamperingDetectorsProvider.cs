using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.controls;
using System.Windows.Forms;
using nvc.controllers;

namespace nvc.controlsUIProvider {
	public class TamperingDetectorsProvider : BaseUIProvider {
		PropertyTamperingDetectors _tampering;
		public void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_tampering = new PropertyTamperingDetectors(devModel) { Dock = DockStyle.Fill, Save = ApplyChanges,
																	Cancel = CancelChanges,
																	onBindingError = BindingError
			};
			if (datProcInfo != null)
				_tampering.memFile = datProcInfo.VideoProcessFile;
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_tampering);
		}
		public override void ReleaseUI() {
			if (_tampering != null && !_tampering.IsDisposed)
				_tampering.ReleaseAll();
		}
	}
}
