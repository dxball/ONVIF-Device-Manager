using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.controls;
using System.Windows.Forms;
using nvc.controllers;

namespace nvc.controlsUIProvider {
	public class RotationProvider : BaseUIProvider {
		PropertyRotation _rotation;
		public void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo) {
			_rotation = new PropertyRotation(devModel) { Dock = DockStyle.Fill, onBindingError = BindingError };
			if (datProcInfo != null)
				_rotation.memFile = datProcInfo.VideoProcessFile;

			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_rotation);
		}
		public override void ReleaseUI() {
			if (_rotation != null && !_rotation.IsDisposed)
				_rotation.ReleaseAll();
		}
	}
}
