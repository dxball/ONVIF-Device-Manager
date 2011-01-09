using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using System.Windows.Forms;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class DevicesListProvider : BaseUIProvider, IDevicesListProvider {
		DeviceListControl _devLsrCtrl;
		public DeviceListControl DevListControl {
			get { return _devLsrCtrl; }
		}

		public void CreateDeviceListControl(Action<DeviceDescriptionModel> itemSelected, Action refreshDevicesList, Action CreateD) {
			_devLsrCtrl = new DeviceListControl(itemSelected, refreshDevicesList, CreateD);
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
