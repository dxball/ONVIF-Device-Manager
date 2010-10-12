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
using System.Windows.Forms;
using nvc.controls;
using nvc.entities;
using nvc.models;
using nvc.onvif;
using System.Threading;
using nvc.utils;

namespace nvc.controllers {
	public class PropertyIdentificationController : IRelesable, IPropertyController {
		DeviceIdentificationModel _devIdentificationModel;
		Session _session;
		Panel _propertyPanel;
		PropertyDeviceIdentificationAndStatus _propertyIdentification;
		InformationForm _savingSettingsForm;
		IDisposable _subscription;

		public PropertyIdentificationController() {

		}

		public void ReleaseAll() {
			if (_subscription != null) _subscription.Dispose();
		}

		void LoadControl() {
			_subscription = _devIdentificationModel.Load(_session)
				.Subscribe(arg => {
					_devIdentificationModel = arg;
					_propertyPanel.SuspendLayout();
					_propertyPanel.Controls.ForEach(x=>((Control)x).Dispose());
					_propertyPanel.Controls.Clear();
					_propertyIdentification = new PropertyDeviceIdentificationAndStatus(_devIdentificationModel) { Dock = DockStyle.Fill, Save = ApplyChanges, Cancel = CancelChanges};
					_propertyPanel.Controls.Add(_propertyIdentification);
					_propertyPanel.ResumeLayout();
				}, err => {
					_savingSettingsForm = new InformationForm("ERROR");
					_savingSettingsForm.SetErrorMessage(err.Message);
					_savingSettingsForm.ShowCloseButton(null);
					_savingSettingsForm.ShowDialog(_propertyIdentification);
				});
		}
		public BasePropertyControl CreateController(Panel propertyPanel, Session session, ChannelDescription chan) {
			_propertyPanel = propertyPanel;
			_session = session;
			_devIdentificationModel = new DeviceIdentificationModel();			

			_propertyPanel.Controls.Clear();
			_propertyPanel.Controls.Add(new LoadingPropertyPage() { Dock = DockStyle.Fill});

			LoadControl();

			return null;
		}

		void _devModel_IdentificationInitialised(object sender, EventArgs e) {
			var control = CreatePropertyIdentification();
			_propertyPanel.Controls.Clear();
			control.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(control);
		}
		public BasePropertyControl CreatePropertyIdentification() {
			_propertyIdentification = new PropertyDeviceIdentificationAndStatus(new DeviceIdentificationModel());
			_propertyIdentification.Dock = DockStyle.Fill;
			return _propertyIdentification;
		}

		void CancelChanges() {
			_devIdentificationModel.RevertChanges();
		}
		void ApplyChanges() {
			_devIdentificationModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => { 
					_devIdentificationModel = devMod;
				}, err => { 
					SaveDeviceNameError(err.Message);
				}, () => { 
					SaveDeviceNameComplete();
				});
			_savingSettingsForm = new InformationForm();
			_savingSettingsForm.ShowDialog(_propertyIdentification);
		}

		void propertyIdentification_SaveData(string name) {
			_savingSettingsForm = new InformationForm();
			_savingSettingsForm.ShowDialog(_propertyIdentification);
		}

		void SaveDeviceNameError(string error) {
			_savingSettingsForm.SetErrorMessage(error);
			_savingSettingsForm.ShowCloseButton(KillEveryOne);
		}
		public void KillEveryOne() {
			WorkflowController.Instance.KillEveryBody();
		}
		void SaveDeviceNameComplete() {
			_savingSettingsForm.Close();
		}

	}
}
