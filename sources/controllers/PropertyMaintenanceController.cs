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
using nvc.controls;
using System.Windows.Forms;
using nvc.onvif;
using nvc.models;
using System.Threading;
using onvifdm.utils;

namespace nvc.controllers {
	public class PropertyMaintenanceController : IRelesable, IPropertyController {
		
		public PropertyMaintenanceController() {
		}
		public void ReleaseAll() {
		}

		Panel _propertyPanel;
		Session CurrentSession{get;set;}
		BasePropertyControl _currentControl;
		IDisposable _subscription;
		InformationForm _infoForm;
		DeviceMaintenanceModel _devModel;

		void LoadControl() {
			_devModel = new DeviceMaintenanceModel();
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				_devModel = arg;
				_propertyPanel.SuspendLayout();
				_propertyPanel.Controls.ForEach(x => ((Control)x).Dispose());
				_propertyPanel.Controls.Clear();
				_currentControl = new PropertyMaintenance(_devModel) { Dock = DockStyle.Fill, Upgrade = UpgradeFirmware, SoftReset = SoftReset };
				_propertyPanel.Controls.Add(_currentControl);
				_propertyPanel.ResumeLayout();
			}, err => {
				_infoForm = new InformationForm("ERROR");
				_infoForm.SetErrorMessage(err.Message);
				_infoForm.SetEttorXML(err);
				_infoForm.ShowCloseButton(null);
				_infoForm.ShowDialog();
			});
		}
		public BasePropertyControl CreateController(Panel propertyPanel, Session session, ChannelDescription chan) {
			_propertyPanel = propertyPanel;
			_propertyPanel.Controls.Clear();
			CurrentSession = session;

			_currentControl = new LoadingPropertyPage();
			_currentControl.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(_currentControl);

			LoadControl();
			return _currentControl;
		}
		public void SoftReset() {
			_devModel.Reboot().Subscribe(message => {
				_infoForm.SetErrorMessage(message);
				_infoForm.ShowCloseButton(ReturnToBeginning);
			}, err => {
				_infoForm.SetErrorMessage(err.Message);
				_infoForm.SetEttorXML(err);
				_infoForm.ShowCloseButton(null);
			});
			_infoForm = new InformationForm();
			_infoForm.ShowDialog(_propertyPanel);
		}
		
		public void UpgradeFirmware(string path) {
			_devModel.firmwarePath = path;
			_devModel.ApplyChanges().Subscribe(devMod => {
					_devModel = devMod;
				}, err => {
					DebugHelper.Error(err);
					UpgradeFirmwareError(err.Message, err);
					
				}, () => {
					SaveNetworkSettingsComplete();
				});
			_infoForm = new InformationForm();
			_infoForm.ShowDialog(_propertyPanel);
		}

		void UpgradeFirmwareError(string message, Exception err) {
			//_infoForm = new InformationForm("ERROR");
			_infoForm.SetErrorMessage(err.Message);
			_infoForm.SetEttorXML(err);
			_infoForm.ShowCloseButton(null); 
		}

		void SaveNetworkSettingsComplete() {
			//_savingSettingsForm.Close();
			_infoForm.SetErrorMessage(SaveSettingsFormStrings.Instance.NeedToReboot);
			_infoForm.ShowCloseButton(ReturnToBeginning);
		}
		public void ReturnToBeginning() {
			WorkflowController.Instance.ReleaseMainFrameController();
			WorkflowController.Instance.RefreshDevicesList();
		}
	}
}
