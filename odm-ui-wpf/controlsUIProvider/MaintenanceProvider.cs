using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using System.Windows.Forms;
using odm.models;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class MaintenanceProvider : BaseUIProvider, IMaintenanceProvider {
		PropertyMaintenance _maintenance;
		public void InitView(DeviceMaintenanceModel devModel, Action<string> UpgradeFirmware, Action SoftReset) {
			_maintenance = new PropertyMaintenance(devModel) { Upgrade = UpgradeFirmware,
															   SoftReset = SoftReset,
															   onBindingError = BindingError
			};
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_maintenance);
		}
		public override void ReleaseUI() {
			if (_maintenance != null) {
				_maintenance.ReleaseAll();
				_maintenance = null;
			}
		}
	}
}
