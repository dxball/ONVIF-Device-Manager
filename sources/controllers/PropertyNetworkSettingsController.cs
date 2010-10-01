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
using System.Windows.Forms;
using nvc.controls;
using nvc.models;
using nvc.entities;

namespace nvc.controllers {
	public class PropertyNetworkSettingsController : IRelesable, IPropertyController {
		DeviceModel _devModel = WorkflowController.Instance.GetCurrentDevice();
		Panel _propertyPanel;
		PropertyNetworkSettings _propNetSett;
		BasePropertyControl _currentControl;
		IDisposable _subscriptionNetworkStatus;
		IDisposable _subscriptionNetworkSettings;

		public PropertyNetworkSettingsController() {

		}

		#region IRelesable
		public bool disposed = false;
		void ReleaseUIEvents() { 
			WorkflowController.Instance.SaveNetworkSettingsCompleate -= Instance_SaveNetworkSettingsCompleate;
			WorkflowController.Instance.SaveNetworkSettingsError -= Instance_SaveNetworkSettingsError;
			WorkflowController.Instance.SaveNetworkSettingsCompleateRebootNeedet -= Instance_SaveNetworkSettingsCompleateRebootNeedet;

			_devModel.NetworkSettingsInitialised -= _devModel_NetworkSettingsInitialised;
			if(_propNetSett != null)
				_propNetSett.SaveData -= property_SaveData;
		}
		void ReleaseSubscriptions() {
			if (_subscriptionNetworkStatus != null)
				_subscriptionNetworkStatus.Dispose();
			if (_subscriptionNetworkSettings != null)
				_subscriptionNetworkSettings.Dispose();
		}
		public void ReleaseAll() {
			disposed = true;
			ReleaseSubscriptions();
			ReleaseUIEvents();
		}
		#endregion IRelesable

		public BasePropertyControl CreateController(Panel propertyPanel) {
			_propertyPanel = propertyPanel;

			//_propertyPanel.SuspendLayout();
			
			_propertyPanel.Controls.Clear();
			
			if (_devModel.IsPropertyNetworkSettingsReady != true) {
				_subscriptionNetworkStatus = WorkflowController.Instance.SubscribeToNetworkStatus();
				_subscriptionNetworkSettings = WorkflowController.Instance.SubscribeToNetworkSettings();
				_devModel.NetworkSettingsInitialised += new EventHandler(_devModel_NetworkSettingsInitialised);
				_currentControl = new LoadingPropertyPage();
			} else {
				_currentControl = CreateProperty();				
			}

			//_propertyPanel.ResumeLayout();

			_currentControl.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(_currentControl);

			return _currentControl;
		}

		void _devModel_NetworkSettingsInitialised(object sender, EventArgs e) {
			var control = CreateProperty();
			_propertyPanel.Controls.Clear();
			_propertyPanel.Controls.Add(control);

		}
		public BasePropertyControl CreateProperty() {
			_propNetSett = new PropertyNetworkSettings(_devModel);
			_propNetSett.Dock = DockStyle.Fill;
			_propNetSett.SaveData += new SaveNetworkSettingsEvent(property_SaveData);
			return _propNetSett;
		}
		SavingSettingsForm _savingSettingsForm;
		void property_SaveData(NetworkSettings netSett, NetworkStatus netStat) {
			WorkflowController.Instance.SaveNetworkSettingsCompleate += new EventHandler(Instance_SaveNetworkSettingsCompleate);
			WorkflowController.Instance.SaveNetworkSettingsError += new EventHandler(Instance_SaveNetworkSettingsError);
			WorkflowController.Instance.SaveNetworkSettingsCompleateRebootNeedet += new EventHandler(Instance_SaveNetworkSettingsCompleateRebootNeedet);
			ReleaseSubscriptions();
			WorkflowController.Instance.SubscribeToSaveNetworkSettings(netSett);
			_savingSettingsForm = new SavingSettingsForm();
			_savingSettingsForm.ShowDialog(_propNetSett);
		}

		void Instance_SaveNetworkSettingsCompleateRebootNeedet(object sender, EventArgs e) {
			WorkflowController.Instance.SaveNetworkSettingsCompleateRebootNeedet -= Instance_SaveNetworkSettingsCompleateRebootNeedet;
			_savingSettingsForm.SetErrorMessage(Constants.Instance.sSaveSettingsNeedReboot);
			_savingSettingsForm.ShowCloseButton();
			WorkflowController.Instance.KillEveryBody();
		}

		void Instance_SaveNetworkSettingsError(object sender, EventArgs e) {
			WorkflowController.Instance.SaveNetworkSettingsError -= Instance_SaveNetworkSettingsError;
			_savingSettingsForm.SetErrorMessage(Constants.Instance.sErrorSaveNetworkSettings);
			_savingSettingsForm.ShowCloseButton();
			WorkflowController.Instance.KillEveryBody();
		}

		void Instance_SaveNetworkSettingsCompleate(object sender, EventArgs e) {
			WorkflowController.Instance.SaveNetworkSettingsCompleate -= Instance_SaveNetworkSettingsCompleate;
			_savingSettingsForm.Close();
			
			ReleaseUIEvents();
			var control = CreateProperty();
			_propertyPanel.Controls.Clear();
			_propertyPanel.Controls.Add(control);
		}
	}
}
