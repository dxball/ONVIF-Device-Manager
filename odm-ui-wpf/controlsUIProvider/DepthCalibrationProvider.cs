using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using System.Windows.Forms;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class DepthCalibrationProvider : BaseUIProvider, IDepthCalibrationProvider {
		PropertyDepthCalibration _depthCalibration;
		public void InitView(DepthCalibrationModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			if (datProcInfo != null) {
				_depthCalibration = new PropertyDepthCalibration(devModel, datProcInfo.VideoProcessFile) {
					Save = ApplyChanges,
					Cancel = CancelChanges,
					onBindingError = BindingError
				};
			} else {
				_depthCalibration = new PropertyDepthCalibration(devModel, null) {
					Save = ApplyChanges,
					Cancel = CancelChanges,
					onBindingError = BindingError
				};
			}

			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_depthCalibration);
		}
		public override void ReleaseUI() {
			if(_depthCalibration != null)
			  _depthCalibration.ReleaseAll();
		}
	}
}
