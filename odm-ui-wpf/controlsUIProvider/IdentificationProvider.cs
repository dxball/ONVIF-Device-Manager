using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class IdentificationProvider : BaseUIProvider, IIdentificationProvider {
		PropertyDeviceIdentificationAndStatus _identification;
		public void InitView(DeviceIdentificationModel devModel, Action ApplyChanges, Action CancelChanges) {
			_identification = new PropertyDeviceIdentificationAndStatus(devModel) { Save = ApplyChanges,
																					Cancel = CancelChanges,
																					onBindingError = BindingError
			};
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_identification);
		}
		public override void ReleaseUI() {
			if (_identification != null) {
				_identification.ReleaseAll();
				_identification = null;
			}
		}
	}
}
