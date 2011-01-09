using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using System.Windows.Forms;
using odm.controllers;
using odm.utils.controls;

namespace odm.controls.UIProvider {
	public class DepthCalibrationProvider : BaseUIProvider {
		//PropertyDepthCalibration _depthCalibration;
		PropertyDepthCalibrationSTA _depthCalibrationSTA;
		public void InitView(DepthCalibrationModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			//_depthCalibration = new PropertyDepthCalibration(devModel) { Dock = DockStyle.Fill, Save = ApplyChanges, 
			//    Cancel = CancelChanges, onBindingError = BindingError };
			//if (datProcInfo != null)
			//    _depthCalibration.memFile = datProcInfo.VideoProcessFile;
			
			//UIProvider.Instance.MainFrameProvider.AddPropertyControl(_depthCalibration);

			_depthCalibrationSTA = new PropertyDepthCalibrationSTA(devModel){Save = ApplyChanges, 
				Cancel = CancelChanges, onBindingError = BindingError };
			if (datProcInfo != null)
				_depthCalibrationSTA.memFile = datProcInfo.VideoProcessFile;

			UIProvider.Instance.MainFrameProvider.StartSTAForm(_depthCalibrationSTA);
		}
		public override void ReleaseUI() {
			//if(_depthCalibration != null && !_depthCalibration.IsDisposed)
			//    _depthCalibration.ReleaseAll();
			if (_depthCalibrationSTA != null && !_depthCalibrationSTA.IsDisposed){
				_depthCalibrationSTA.ReleaseAll();
				_depthCalibrationSTA.Dispose();
			}
		}
	}
}
