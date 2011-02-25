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
using System.Disposables;
using odm.utils.entities;
using odm.onvif;
using odm.utils;
using odm.models;
using System.Xml.XPath;
using System.Diagnostics;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class DeviceListController {
		public DeviceListController() {
		}
		// Returned IDisposable for cancel operation
		IDisposable _discoverySubscription;

		//Create UI control
		public void CreateDeviceListControl() {
			//Call to UI provider to create Devices List control
			UIProvider.Instance.GetDevicesListProvider().CreateDeviceListControl(DeviceItemSelected, RefreshDevicesList, CreateDumpViewer);
			//Subscribe to WSDiscovery for devices
			WSDiscoveryStartup();
		}
		public void WSDiscoveryStartup() {
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
				//var discoveryClient = new DeviceDiscovery(TimeSpan.FromSeconds(10)).Find();
				var devMng = new DeviceManager();

				subscription.Disposable = devMng.Discover(TimeSpan.FromSeconds(10))
					.ObserveOn(syncCtx)
					.Subscribe(devDescr => {
						dbg.Assert(SynchronizationContext.Current == syncCtx);
						dbg.Assert(isActive);

						DeviceDescriptionModel devModel = new DeviceDescriptionModel(devDescr);
						try {
							var session = new Session(devDescr);//Session.Create(devDescr);
							Guid guid = Guid.NewGuid();
							session.SetContext<Guid>(guid);
							devModel.Load(session).Subscribe(dModel => {
								
							}, err => {
								DescoveryError(err);
							});
							WorkflowController.Instance.WSDicroveryAddDevice(guid.ToString());
							UIProvider.Instance.GetDevicesListProvider().AddDeviceDescription(devModel);

						} catch (Exception err) {
							DescoveryError(err);
						}

						devDescr.removal
							.ObserveOn(syncCtx).Subscribe(un => {
								UIProvider.Instance.GetDevicesListProvider().RemoveDeviceDescription(devModel);
							});
						
					}, err => {
						dbg.Assert(SynchronizationContext.Current == syncCtx);
						dbg.Assert(isActive);
						dbg.Error(err);
					});
			});
			return subscription;
		}
		[Conditional("DEBUG")]
		void DescoveryError(Exception err) {
			//InformationForm infoform = new InformationForm("ERROR");
			//infoform.SetErrorMessage(err.Message);
			//infoform.SetEttorXML(err);
			//infoform.ShowCloseButton(null);
			//infoform.ShowDialog(_devLsrCtrl);
		}

		void UnsubscribeFromWSDiscovery() {
			if (_discoverySubscription != null) {
				_discoverySubscription.Dispose();
			}
		}

		void ReleaseResources() {
			WorkflowController.Instance.ReleaseControllers();
		}

		void DeviceItemSelected(DeviceDescriptionModel devModel) {
			ReleaseResources();
			_currentSelection = devModel;
			//Check if device valid
			WorkflowController.Instance.GetMainWindowController().RunMainFrame(devModel);
			SetStatusText(devModel);
		}

		void SetStatusText(DeviceDescriptionModel devModel) {
			string text1 = devModel.name + "/" + devModel.address + "/" + devModel.firmware;
			UIProvider.Instance.GetMainWindowProvider().SetStatusBarText1(text1);
		}

		public void RefreshDevicesList() {
			WorkflowController.Instance.ReleaseMainFrameController();
			UIProvider.Instance.GetDevicesListProvider().RefreshDevicesList();
			WSDiscoveryStartup();
		}

		DeviceDescriptionModel _currentSelection;
		public void CreateDumpViewer() {
		}
	}
}
