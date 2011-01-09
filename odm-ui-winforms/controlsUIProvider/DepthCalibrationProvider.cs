using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using System.Windows.Forms;
using odm.controllers;

namespace odm.controls.UIProvider {
	public class DepthCalibrationProvider : BaseUIProvider {
		PropertyDepthCalibration _depthCalibration;
		public void InitView(DepthCalibrationModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_depthCalibration = new PropertyDepthCalibration(devModel) { Dock = DockStyle.Fill, Save = ApplyChanges, 
				Cancel = CancelChanges, onBindingError = BindingError };
			if (datProcInfo != null)
				_depthCalibration.memFile = datProcInfo.VideoProcessFile;

			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_depthCalibration);
		}
		public override void ReleaseUI() {
			if(_depthCalibration != null && !_depthCalibration.IsDisposed)
				_depthCalibration.ReleaseAll();
		}
	}
}
