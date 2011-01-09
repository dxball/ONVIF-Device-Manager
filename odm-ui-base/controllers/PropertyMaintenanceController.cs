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
using odm.onvif;
using odm.models;
using System.Threading;
using odm.utils;
using odm.controls;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class PropertyMaintenanceController : BasePropertyController {
		DeviceMaintenanceModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new DeviceMaintenanceModel();
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				UIProvider.Instance.GetMaintenanceProvider().InitView(_devModel, UpgradeFirmware, SoftReset);
			}, err => {
				OnCriticalError(err);
			});
		}

		public void SoftReset() {
			WorkflowController.Instance.GetMainFrameController().UnSubscribeToEvents();
			_devModel.Reboot().Subscribe(message => {
				InformationBox(message, ReturnToDeviceList);
			}, err => {
				OnMinorError(err);
			});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
		public void UpgradeFirmware(string path) {
			_devModel.firmwarePath = path;
			WorkflowController.Instance.GetMainFrameController().UnSubscribeToEvents();
			_devModel.ApplyChanges().Subscribe(devMod => {
			}, err => {
				OnMinorError(err);
			}, () => {
				UpgradeCompleate();
			});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}

		void UpgradeCompleate() {
			UIProvider.Instance.GetMainWindowProvider().EnableControls();
			UIProvider.Instance.GetInfoFormProvider().DisplayInformationForm(PropertyMaintenanceStrings.Instance.updateCompleate, Close);			
		}
		protected override void ApplyChanges() {
		}
		void Close() {
			WorkflowController.Instance.ReleaseControllers();
		}

		protected override void CancelChanges() { }
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseMaintenanceProvider();
		}
	}
}
