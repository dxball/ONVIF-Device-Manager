using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using System.Windows.Forms;
using nvc.models;

namespace nvc.controlsUIProvider {
	public class MaintenanceProvider  : BaseUIProvider{
		PropertyMaintenance _maintenance;
		public void InitView(DeviceMaintenanceModel devModel, Action<string> UpgradeFirmware, Action SoftReset) {
			_maintenance = new PropertyMaintenance(devModel) { Dock = DockStyle.Fill,
															   Upgrade = UpgradeFirmware,
															   SoftReset = SoftReset,
															   onBindingError = BindingError
			};
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_maintenance);
		}
		public override void ReleaseUI() {
			if (_maintenance != null && !_maintenance.IsDisposed)
				_maintenance.ReleaseAll();
		}
	}
}
