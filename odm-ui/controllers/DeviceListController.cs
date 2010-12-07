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
using nvc.entities;
using nvc.onvif;
using onvifdm.utils;
using nvc.models;
using System.Xml.XPath;
using System.Diagnostics;
using nvc.controlsUIProvider;
using nvc;

namespace nvc.controllers {
	public class DeviceListController {
		public DeviceListController() {
		}
		// Returned IDisposable for cancel operation
		IDisposable _discoverySubscription;

		//Create UI control
		public void CreateDeviceListControl() {
			//Call to UI provider to create Devices List control
			UIProvider.Instance.DevicesListProvider.CreateDeviceListControl(DeviceItemSelected, RefreshDevicesList, CreateDumpViewer);
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
						DebugHelper.Assert(SynchronizationContext.Current == syncCtx);
						DebugHelper.Assert(isActive);

						DeviceDescriptionModel devModel = new DeviceDescriptionModel(devDescr);
						try {
							var session = Session.Create(devDescr);
							Guid guid = Guid.NewGuid();
							session.SetContext<Guid>(guid);
							devModel.Load(session).Subscribe(dModel => {
								
							}, err => {
								DescoveryError(err);
							});
							WorkflowController.Instance.WSDicroveryAddDevice(guid.ToString());
							UIProvider.Instance.DevicesListProvider.AddDeviceDescription(devModel);

						} catch (Exception err) {
							DescoveryError(err);
						}

						devDescr.removal
							.ObserveOn(syncCtx).Subscribe(un => {
								UIProvider.Instance.DevicesListProvider.RemoveDeviceDescription(devModel);
							});
						
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
			string text1 = devModel.Name + "/" + devModel.Address + "/" + devModel.Firmware;
			UIProvider.Instance.MainWindowProvider.SetStatusBarText1(text1);
		}

		public void RefreshDevicesList() {
			WorkflowController.Instance.ReleaseMainFrameController();
			UIProvider.Instance.DevicesListProvider.RefreshDevicesList();
			WSDiscoveryStartup();
		}

		XmlExplorer.Controls.TabbedXmlExplorerWindow _dumpWnd;
		DeviceDescriptionModel _currentSelection;
		public void CreateDumpViewer() {
			if (_dumpWnd == null || _dumpWnd.IsDisposed)
				_dumpWnd = new XmlExplorer.Controls.TabbedXmlExplorerWindow();
			_dumpWnd.Show();
			DumpModel onvifDump = new DumpModel();
			if (_currentSelection != null) {
				onvifDump.Load(_currentSelection.session).Subscribe(arg => {

					_dumpWnd.Open(arg.xmlDump.CreateNavigator(), arg.name);

				}, err => {
				});

			}
		}
	}
}
