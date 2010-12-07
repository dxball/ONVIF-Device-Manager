using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using nvc.models;
using nvc.controllers;
using System.Windows.Forms;

namespace nvc.controlsUIProvider {
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
