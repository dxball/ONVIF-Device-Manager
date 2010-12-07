using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.controls;
using System.Windows.Forms;

namespace nvc.controlsUIProvider {
	public class NetworkSettingsProvider : BaseUIProvider {
		PropertyNetworkSettings _netsettings;
		public void InitView(DeviceNetworkSettingsModel devModel, Action ApplyChanges, Action CancelChanges) {
			_netsettings = new PropertyNetworkSettings(devModel) { Dock = DockStyle.Fill, Save = ApplyChanges,
																   Cancel = CancelChanges,
																   onBindingError = BindingError
			};
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_netsettings);
		}
		public override void ReleaseUI() {
			if (_netsettings != null && !_netsettings.IsDisposed)
				_netsettings.ReleaseAll();
		}
	}
}
