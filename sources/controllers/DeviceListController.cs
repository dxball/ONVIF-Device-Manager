//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//
//----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Disposables;

using nvc.entities;
using nvc.controls;
using nvc.onvif;
using nvc.utils;

namespace nvc.controllers {
	public class DeviceListController {
		protected DevicesListControl _devLsrCtrl;
		public DeviceListController() {
		}

		#region ADD MANUALLY
		//Action 
		public void AddDeviceManually(Uri uri) {

			DeviceDescription.Load(uri)
				.ObserveOn(SynchronizationContext.Current)
				.Select(devDescr => {
					DeviceModelInfo devMod = new DeviceModelInfo();
					devMod.IpAddress = devDescr.IPAddress;
					if (devDescr.devInfo != null) {
						devMod.DeviceId = devDescr.devInfo.SerialNumber;
						devMod.Firmware = devDescr.devInfo.FirmwareVersion;
						devMod.Name = devDescr.devInfo.Model;
						devMod.Manufacturer = devDescr.devInfo.Manufacturer;
						devMod.HardwareId = devDescr.devInfo.HardwareId;
						devMod.IsValid = true;
					} else {
						devMod = WSDiscoveryDevInfoError(Constants.Instance.sErrorDevInfoNull, devMod);
					}

					devMod.devDescr = devDescr;

					return devMod;
				})
					.Subscribe(devMod => {
						WorkflowController.Instance.AddDeviceModelInfo(devMod);
					}, err => {
						WorkflowController.Instance.AddDeviceModelInfo(WSDiscoveryExceptionHandler(Constants.Instance.sExceptionWSDiscoveryTitle, err.Message, null));
					});
		}
		#endregion

		#region Init section
		public DevicesListControl CreateDeviceListControl() {
			_devLsrCtrl = new DevicesListControl();
			_devLsrCtrl.Dock = DockStyle.Fill;

			SubscribeToEvents();
			//[TODO] REMOVE!!!
			_devLsrCtrl.SubscribeToManualAdding(AddDeviceManually);

			FillDeviceList();
			return _devLsrCtrl;
		}
		void SubscribeToEvents() {

			_devLsrCtrl._onDeviceItemSelected += new DeviceListItemSelectedDelegate(devListctrl__onDeviceItemSelected);
			_devLsrCtrl.RefreshDeviceList += new EventHandler(devListctrl_RefreshDeviceList);
			WorkflowController.Instance.ModelInfoAdded += new EventHandler(Instance_ModelInfoAdded);
		}
		#endregion

		#region Data section
		public void RemoveDevListControlItem(DeviceModelInfo devInfo) {
			if (_devLsrCtrl != null) {
				_devLsrCtrl.RemoveItem(devInfo);
			}
		}
		public void AddDevListControlItem(DeviceModelInfo devInfo) {
			if (_devLsrCtrl != null) {
				_devLsrCtrl.AddItem(devInfo, false);
			}
		}
		public void SelectDevListControlItem(DeviceModelInfo devInfo) {
			if (_devLsrCtrl != null) {
				_devLsrCtrl.SelectItem(devInfo);
			}
		}

		protected void FillDeviceList() {
			SubscribeToWSDiscovery();
		}

		IDisposable discoverySubscription;
		protected void SubscribeToWSDiscovery() {
			var syncCtx = SynchronizationContext.Current;
			var controller = WorkflowController.Instance;
			var isActive = true;
			var subscription = new MutableDisposable();
			DebugHelper.Assert(controller.DeviceItemsInfoList.IsEmpty());

			if (discoverySubscription != null) {
				discoverySubscription.Dispose();
			}
			discoverySubscription = Disposable.Create(() => {
				isActive = false;
				subscription.Dispose();
			});


			GlobalWorkItemQueue.Enqueue(() => {
				DebugHelper.Assert(controller.DeviceItemsInfoList.IsEmpty());
				if (!isActive) {
					return;
				}
				var discoveryClient = new DeviceDiscovery() {
					Duration = TimeSpan.FromSeconds(10)
				}.Find();

				subscription.Disposable = discoveryClient.ObserveOn(syncCtx)
					.Select(devDescr => {
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DeviceModelInfo devMod = new DeviceModelInfo();
						devMod.IpAddress = devDescr.IPAddress;
						if (devDescr.devInfo != null) {
							devMod.DeviceId = devDescr.devInfo.SerialNumber;
							devMod.Firmware = devDescr.devInfo.FirmwareVersion;
							devMod.Name = devDescr.devInfo.Model;
							devMod.Manufacturer = devDescr.devInfo.Manufacturer;
							devMod.HardwareId = devDescr.devInfo.HardwareId;
							devMod.IsValid = true;
						} else {
							devMod = WSDiscoveryDevInfoError(Constants.Instance.sErrorDevInfoNull, devMod);
						}

						devMod.devDescr = devDescr;

						return devMod;
					})
					.Subscribe(devMod => {
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DebugHelper.Assert(isActive);
						controller.AddDeviceModelInfo(devMod);

					}, err => {
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DebugHelper.Assert(isActive);
						controller.AddDeviceModelInfo(WSDiscoveryExceptionHandler(Constants.Instance.sExceptionWSDiscoveryTitle, err.Message, null));
					});
			});

		}
		#endregion

		#region ErrorHandler
		protected DeviceModelInfo WSDiscoveryDevInfoError(string Description, DeviceModelInfo devinfo) {
			return WSDiscoveryExceptionHandler(Constants.Instance.sExceptionWSDiscoveryTitle, Description, devinfo);
		}
		protected DeviceModelInfo WSDiscoveryExceptionHandler(string title, string description, DeviceModelInfo devinfo) {
			//ErrorMessageForm errorForm = new ErrorMessageForm(title, description);

			if (devinfo == null) {
				devinfo = new DeviceModelInfo();
				//devinfo.IpAddress = Constants.Instance.sErrorDeviceIPAddress;
			}

			devinfo.IsValid = false;
			devinfo.Firmware = Constants.Instance.sErrorDeviceFirmware;
			devinfo.Name = Constants.Instance.sErrorDeviceName;

			devinfo.ErrorMsg = description;

			return devinfo;
		}
		#endregion

		#region EventsHandlers
		void Instance_ModelInfoAdded(object sender, EventArgs e) {
			_devLsrCtrl.AddItem((DeviceModelInfo)e, true);
		}
		void devListctrl_RefreshDeviceList(object sender, EventArgs e) {
			_devLsrCtrl.Refresh();
			WorkflowController.Instance.DeviceItemsInfoList.Clear();
			FillDeviceList();
		}
		void devListctrl__onDeviceItemSelected(string id) {
			ReleaseResources();

			DeviceModelInfo devInfo = WorkflowController.Instance.GetDeviceInfoByID(id);

			//Check if device valid
			if (devInfo.IsValid) {
				WorkflowController.Instance.CreateDeviceModel(devInfo);
				WorkflowController.Instance.GetMainWindowController().RunMainFrame();
				SetStatusText(devInfo);
			} else {
				var controller = WorkflowController.Instance.GetErrorFrameController();
				var ctrl = controller.CreateErrorFrame(devInfo);
				WorkflowController.Instance.GetMainWindowController().RunErrorFrame(ctrl);
			}
		}
		void SetStatusText(DeviceModelInfo devInfo) {
			string text1 = devInfo.Name + "/" + devInfo.IpAddress + "/" + devInfo.Firmware;
			WorkflowController.Instance.GetMainWindowController().SetStatusBarText1(text1);
		}
		#endregion

		#region release resources
		void ReleaseResources() {
			WorkflowController.Instance.ReleaseControllers();
			WorkflowController.Instance.ReleaseDeviceModel();
		}
		#endregion
	}
}
