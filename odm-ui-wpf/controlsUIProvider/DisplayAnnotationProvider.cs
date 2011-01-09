using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using System.Windows.Forms;
using odm.models;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class DisplayAnnotationProvider : BaseUIProvider, IDisplayAnnotationProvider {
		PropertyDisplayAnnotation _displayAnnotation;
		public void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			if (datProcInfo == null)
				return;
			_displayAnnotation = new PropertyDisplayAnnotation(devModel) {Save = ApplyChanges,
																		   Cancel = CancelChanges,
																		   onBindingError = BindingError,
																		   memFile = datProcInfo.VideoProcessFile
			};

			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_displayAnnotation);
		}
		public override void ReleaseUI() {
			if (_displayAnnotation != null) {
				_displayAnnotation.ReleaseAll();
				_displayAnnotation = null;
			}
		}
	}
}
