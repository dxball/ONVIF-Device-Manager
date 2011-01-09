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
	public class RotationProvider : BaseUIProvider, IRotationProvider {
		PropertyRotation _rotation;
		public void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo) {
			_rotation = new PropertyRotation(devModel) { onBindingError = BindingError };
			//if (datProcInfo != null)
			//    _rotation.memFile = datProcInfo.VideoProcessFile;

			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_rotation);
		}
		public override void ReleaseUI() {
			if (_rotation != null) {
				_rotation.ReleaseAll();
				_rotation = null;
			}
		}
	}
}
