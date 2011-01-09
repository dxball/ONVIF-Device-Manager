using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using System.Windows.Forms;

namespace odm.controls.UIProvider {
	public class DevicesListProvider: BaseUIProvider {
		DevicesListControl _devLsrCtrl;
		public DevicesListControl DevListControl {
			get { return _devLsrCtrl; }
		}

		public void CreateDeviceListControl(Action<DeviceDescriptionModel> itemSelected, Action refreshDevicesList, Action CreateD) {
			_devLsrCtrl = new DevicesListControl(itemSelected, refreshDevicesList, CreateD);
			_devLsrCtrl.Dock = DockStyle.Fill;

			//Subscribe to WSDiscovery for devices
			FillDeviceList();
		}
		public void FillDeviceList() {
		}
		public void RefreshDevicesList() {
			_devLsrCtrl.RefreshItems();
			//_currentSelection = null;
			//DeviceDescriptionModels.Clear();
			FillDeviceList();
		}
		public void RemoveDeviceDescription(DeviceDescriptionModel devModel) {
			_devLsrCtrl.RemoveItem(devModel);
		}
		public void AddDeviceDescription(DeviceDescriptionModel devModel) {
			_devLsrCtrl.AddItem(devModel);
		}

		public override void ReleaseUI() {
			
		}
	}
}
