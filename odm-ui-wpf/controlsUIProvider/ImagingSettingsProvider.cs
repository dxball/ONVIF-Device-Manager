using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using odm.controllers;
using System.Windows.Forms;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class ImagingSettingsProvider : BaseUIProvider, IImagingSettingsProvider {
		PropertyImagingSettings _imgSettings;
		public void InitView(ImagingSettingsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_imgSettings = new PropertyImagingSettings(devModel) {
				Save = ApplyChanges,
				Cancel = CancelChanges,
				onBindingError = BindingError
			};
			//if (datProcInfo != null)
			//    _imgSettings.memFile = datProcInfo.VideoProcessFile;

			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_imgSettings);
		}
		public override void ReleaseUI() {
			if (_imgSettings != null) {
				_imgSettings.ReleaseAll();
				_imgSettings = null;
			}
		}
	}
}
