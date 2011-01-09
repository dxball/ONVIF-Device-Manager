using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using System.Windows.Forms;
using odm.models;
using odm.controllers;

namespace odm.controls.UIProvider {
	public class DisplayAnnotationProvider : BaseUIProvider {
		PropertyDisplayAnnotation _displayAnnotation;
		public void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_displayAnnotation = new PropertyDisplayAnnotation(devModel) { Dock = DockStyle.Fill,
																		   Save = ApplyChanges,
																		   Cancel = CancelChanges,
																		   onBindingError = BindingError
			};
			if (datProcInfo != null)
				_displayAnnotation.memFile = datProcInfo.VideoProcessFile;

			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_displayAnnotation);
		}
		public override void ReleaseUI() {
			if (_displayAnnotation != null && !_displayAnnotation.IsDisposed)
				_displayAnnotation.ReleaseAll();
		}
	}
}
