using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using odm.controllers;
using System.Windows.Forms;

namespace odm.controls.UIProvider {
	public class ImagingSettingsProvider : BaseUIProvider {
		PropertyImagingSettings _imgSettings;
		public void InitView(ImagingSettingsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_imgSettings = new PropertyImagingSettings(devModel) {
				Dock = DockStyle.Fill,
				Save = ApplyChanges,
				Cancel = CancelChanges,
				onBindingError = BindingError
			};
			if (datProcInfo != null)
				_imgSettings.memFile = datProcInfo.VideoProcessFile;

			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_imgSettings);
		}
		public override void ReleaseUI() {
			if (_imgSettings != null && !_imgSettings.IsDisposed)
				_imgSettings.ReleaseAll();
		}
	}
}
