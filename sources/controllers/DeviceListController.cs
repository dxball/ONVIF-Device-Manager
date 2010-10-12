#region License and Terms
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
//----------------------------------------------------------------------------------------------------------------
#endregion

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
using nvc.models;

namespace nvc.controllers {
	public class DeviceListController {
		public DeviceListController() {
			_deviceDescriptionModels = new List<DeviceDescriptionModel>();

		}

		List<DeviceDescriptionModel> _deviceDescriptionModels;	// List of device descriptions from WD discovery to fill in UI list view
		List<DeviceDescriptionModel> DeviceDescriptionModels {
			get {
				if (_deviceDescriptionModels == null)
					_deviceDescriptionModels = new List<DeviceDescriptionModel>();
				return _deviceDescriptionModels;
			}
		}
		protected DevicesListControl _devLsrCtrl;				// UI control
		IDisposable _discoverySubscription;						// Returned IDisposable for cancel operation

		void AddDeviceDescription(DeviceDescriptionModel devModel) {
			DeviceDescriptionModels.Add(devModel);
			_devLsrCtrl.AddItem(devModel);
		}
		//Create UI control
		public DevicesListControl CreateDeviceListControl() {
			_devLsrCtrl = new DevicesListControl();
			_devLsrCtrl.Dock = DockStyle.Fill;

			SubscribeToEvents();
			//[TODO] REMOVE!!!
			//_devLsrCtrl.SubscribeToManualAdding(AddDeviceManually);

			//Subscribe to WSDiscovery for devices
			FillDeviceList();

			return _devLsrCtrl;
		}
		protected void FillDeviceList() {
			UnsubscribeFromWSDiscovery();
			_discoverySubscription = SubscribeToWSDiscovery();
		}
		protected IDisposable SubscribeToWSDiscovery() {
			var syncCtx = SynchronizationContext.Current;
			var isActive = true;
			var subscription = new MutableDisposable();

			if (_discoverySubscription != null) {
				_discoverySubscription.Dispose();
			}
			_discoverySubscription = Disposable.Create(() => {
				isActive = false;
				subscription.Dispose();
			});

			GlobalWorkItemQueue.Enqueue(() => {
				if (!isActive) {
					return;
				}
				var discoveryClient = new DeviceDiscovery() {
					Duration = TimeSpan.FromSeconds(10)
				}.Find();

				subscription.Disposable = discoveryClient
					.ObserveOn(syncCtx)
					.Subscribe(devDescr => {
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DebugHelper.Assert(isActive);

						DeviceDescriptionModel devModel = new DeviceDescriptionModel(devDescr);
						devModel.Load(Session.Create(devDescr)).Subscribe(dModel => {

						}, err => {
							//DebugHelper.Error(err);
						});

						AddDeviceDescription(devModel);
					}, err => {
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DebugHelper.Assert(isActive);
						DebugHelper.Error(err);
					});
			});
			return subscription;
		}

		void UnsubscribeFromWSDiscovery() {
			if (_discoverySubscription != null) {
				_discoverySubscription.Dispose();
			}
		}

		//Subscribe to UI events
		void SubscribeToEvents() {
			_devLsrCtrl._onDeviceItemSelected += new DeviceListItemSelectedDelegate(devListctrl__onDeviceItemSelected);
			_devLsrCtrl.RefreshDeviceList += new EventHandler(devListctrl_RefreshDeviceList);
			//WorkflowController.Instance.ModelInfoAdded += new EventHandler(Instance_ModelInfoAdded);
		}
		//Subscribe from UI events
		void UnsubscribeFromEvents() {
			_devLsrCtrl._onDeviceItemSelected -= devListctrl__onDeviceItemSelected;
			_devLsrCtrl.RefreshDeviceList -= devListctrl_RefreshDeviceList;
		}

		void ReleaseResources() {
			WorkflowController.Instance.ReleaseControllers();
			//WorkflowController.Instance.ReleaseDeviceModel();
		}

		public void RemoveDevListControlItem(DeviceDescriptionModel devModel) {
			if (_devLsrCtrl != null) {
				_devLsrCtrl.RemoveItem(devModel);
			}
		}

		void devListctrl__onDeviceItemSelected(DeviceDescriptionModel devModel) {
			ReleaseResources();

			//Check if device valid
			WorkflowController.Instance.GetMainWindowController().RunMainFrame(devModel);
			SetStatusText(devModel);
		}

		void SetStatusText(DeviceDescriptionModel devModel) {
			string text1 = devModel.Name + "/" + devModel.Address + "/" + devModel.Firmware;
			WorkflowController.Instance.GetMainWindowController().SetStatusBarText1(text1);
		}

		public void RefreshDevicesList() {
			WorkflowController.Instance.ReleaseMainFrameController();
			_devLsrCtrl.RefreshItems();
			DeviceDescriptionModels.Clear();
			FillDeviceList();
		}
		void devListctrl_RefreshDeviceList(object sender, EventArgs e) {
			RefreshDevicesList();
		}
	}
}
