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
using onvifdm.utils;
using nvc.models;
using System.Xml.XPath;
using System.Diagnostics;

namespace nvc.controllers {
	public class DeviceListController {
		public DeviceListController() {
			_deviceDescriptionModels = new List<DeviceDescriptionModel>();
		}

		DeviceDescriptionModel _currentSelection;
		InformationForm _infoForm;
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
			_devLsrCtrl = new DevicesListControl(DeviceItemSelected, RefreshDevicesList, CreateDumpViewer);
			_devLsrCtrl.Dock = DockStyle.Fill;

			//Subscribe to WSDiscovery for devices
			FillDeviceList();

			return _devLsrCtrl;
		}
		public void FillDeviceList() {
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
				var discoveryClient = new DeviceDiscovery(TimeSpan.FromSeconds(10)).Find();

				subscription.Disposable = discoveryClient
					.ObserveOn(syncCtx)
					.Subscribe(devDescr => {
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DebugHelper.Assert(isActive);

						DeviceDescriptionModel devModel = new DeviceDescriptionModel(devDescr);
						devModel.Load(Session.Create(devDescr)).Subscribe(dModel => {

						}, err => {
							DescoveryError(err);
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
		[Conditional("DEBUG")]
		void DescoveryError(Exception err) {
			InformationForm infoform = new InformationForm("ERROR");
			infoform.SetErrorMessage(err.Message);
			infoform.SetEttorXML(err);
			infoform.ShowCloseButton(null);
			infoform.ShowDialog(_devLsrCtrl);

		}

		void UnsubscribeFromWSDiscovery() {
			if (_discoverySubscription != null) {
				_discoverySubscription.Dispose();
			}
		}

		void ReleaseResources() {
			WorkflowController.Instance.ReleaseControllers();
			//WorkflowController.Instance.ReleaseDeviceModel();
		}

		public void RemoveDevListControlItem(DeviceDescriptionModel devModel) {
			if (_currentSelection == devModel)
				_currentSelection = null;
			if (_devLsrCtrl != null) {
				_devLsrCtrl.RemoveItem(devModel);
			}
		}

		void DeviceItemSelected(DeviceDescriptionModel devModel) {
			ReleaseResources();
			_currentSelection = devModel;
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
			_currentSelection = null;
			DeviceDescriptionModels.Clear();
			FillDeviceList();
		}

		XmlExplorer.Controls.TabbedXmlExplorerWindow _dumpWnd;
		public void CreateDumpViewer() {
			if (_dumpWnd == null || _dumpWnd.IsDisposed)
				_dumpWnd = new XmlExplorer.Controls.TabbedXmlExplorerWindow();
			_dumpWnd.Show();
			DumpModel onvifDump = new DumpModel();
			if (_currentSelection != null) {
				onvifDump.Load(_currentSelection.session).Subscribe(arg => {

					_dumpWnd.Open(arg.xmlDump.CreateNavigator(), arg.name);
					
				}, err => {
					//DebugHelper.Error(err);
					_infoForm = new InformationForm("ERROR");
					_infoForm.SetErrorMessage(err.Message);
					_infoForm.SetEttorXML(err);
					_infoForm.ShowCloseButton(null);
					_infoForm.ShowDialog(_devLsrCtrl);
				});
				
			}
		}

		/// <summary>
		/// Implementation for UI Independent controller
		/// </summary>
		/// <param name="devModel"></param>
		#region WPF
		public Action<DeviceDescriptionModel> addToUi;

		public void wpfRefreshDevicesList() {
			WorkflowController.Instance.ReleaseMainFrameController();
			_currentSelection = null;
			DeviceDescriptionModels.Clear();
			wpfFillDeviceList();
		}
		void wpfAddDeviceDescription(DeviceDescriptionModel devModel) {
			DeviceDescriptionModels.Add(devModel);

			if (addToUi != null) {
				addToUi(devModel);
			}
		}
		public void wpfFillDeviceList() {
			UnsubscribeFromWSDiscovery();
			_discoverySubscription = wpfSubscribeToWSDiscovery(wpfAddDeviceDescription);
		}
		protected IDisposable wpfSubscribeToWSDiscovery(Action<DeviceDescriptionModel> addDevDescr) {
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
				var discoveryClient = new DeviceDiscovery(TimeSpan.FromSeconds(10)).Find();

				subscription.Disposable = discoveryClient
					.ObserveOn(syncCtx)
					.Subscribe(devDescr => {
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DebugHelper.Assert(isActive);

						DeviceDescriptionModel devModel = new DeviceDescriptionModel(devDescr);
						devModel.Load(Session.Create(devDescr)).Subscribe(dModel => {

						}, err => {
							DescoveryError(err);
						});

						addDevDescr(devModel);
					}, err => {
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DebugHelper.Assert(isActive);
						DebugHelper.Error(err);
					});
			});
			return subscription;
		}
		#endregion WPF
	}
}
