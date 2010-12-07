using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using nvc.models;
using System.Windows.Forms;

namespace nvc.controlsUIProvider {
	public class IdentificationProvider : BaseUIProvider {
		PropertyDeviceIdentificationAndStatus _identification;
		public void InitView(DeviceIdentificationModel devModel, Action ApplyChanges, Action CancelChanges) {
			_identification = new PropertyDeviceIdentificationAndStatus(devModel) { Dock = DockStyle.Fill,
																					Save = ApplyChanges,
																					Cancel = CancelChanges,
																					onBindingError = BindingError
			};
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_identification);
		}
		public override void ReleaseUI() {
			if (_identification != null && !_identification.IsDisposed)
				_identification.ReleaseAll();
		}
	}
}
