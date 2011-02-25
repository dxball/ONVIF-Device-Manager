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
using odm.ui.controls;
using odm.utils.controlsUIProvider;
using System.IO;

namespace odm.controllers {
	public class PropertyMaintenanceController : BasePropertyController {
		MaintenanceModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new MaintenanceModel();
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				UIProvider.Instance.GetMaintenanceProvider().InitView(_devModel, UpgradeFirmware, SoftReset, Backup, Restore);
			}, err => {
				OnCriticalError(err);
			});
		}

		public void Backup(string path) {
			Stream strim = new FileStream(path, FileMode.Create);
			_devModel.Backup(strim).ObserveOn(SynchronizationContext.Current).Subscribe(ret => {
			}, err => {
				OnMinorError(err);
				strim.Close();
			}, () => {
				BuckupCompleate();
				strim.Close();
			});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
		void BuckupCompleate() {
			base.ApplyCompleate();
			UIProvider.Instance.GetInfoFormProvider().DisplayInformationForm(PropertyMaintenanceStrings.Instance.backupCompleate, OnCompleate);
		}
		void RestoreCompleate() {
			base.ApplyCompleate();
            UIProvider.Instance.GetInfoFormProvider().DisplayInformationForm(PropertyMaintenanceStrings.Instance.restoreCompleate, ReturnToMainFrame);
		}
		void OnCompleate() {
			base.ApplyCompleate();
		}
		public void Restore(string path) {
			Stream strim = new FileStream(path, FileMode.Open);
			_devModel.Restore(strim).ObserveOn(SynchronizationContext.Current).Subscribe(ret => {
			}, err => {
				OnMinorError(err);
				strim.Close();
			}, () => {
				RestoreCompleate();
				strim.Close();
			});
			OnApply(InfoFormStrings.Instance.applyChanges);
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
			UIProvider.Instance.GetInfoFormProvider().DisplayInformationForm(PropertyMaintenanceStrings.Instance.updateCompleate, null);			
		}
		protected override void ApplyChanges() {
		}
		void Close() {
			WorkflowController.Instance.ReleaseControllers();
		}

		protected override void CancelChanges() { }
		public override void ReleaseAll() {
			if (_subscription != null)
				_subscription.Dispose();
			UIProvider.Instance.ReleaseMaintenanceProvider();
		}
	}
}
