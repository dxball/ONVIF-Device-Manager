using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.controls;
using System.Windows.Forms;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class NetworkSettingsProvider : BaseUIProvider, INetworkSettingsProvider {
		PropertyNetworkSettings _netsettings;
		public void InitView(DeviceNetworkSettingsModel devModel, Action ApplyChanges, Action CancelChanges) {
			_netsettings = new PropertyNetworkSettings(devModel) { Save = ApplyChanges,
																   Cancel = CancelChanges,
																   onBindingError = BindingError
			};
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_netsettings);
		}
		public override void ReleaseUI() {
			if (_netsettings != null) {
				_netsettings.ReleaseAll();
				_netsettings = null;
			}
		}
	}
}
