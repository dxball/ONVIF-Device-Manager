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
using nvc.entities;
using nvc.models;
using nvc.onvif;
using System.Threading;
using onvifdm.utils;
using nvc.controlsUIProvider;

namespace nvc.controllers {
	public class PropertyIdentificationController : BasePropertyController {
		DeviceIdentificationModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new DeviceIdentificationModel();
			_subscription = _devModel.Load(CurrentSession)
				.Subscribe(arg => {
					UIProvider.Instance.IdentificationProvider.InitView(_devModel, ApplyChanges, CancelChanges);
				}, err => {
					OnCriticalError(err);
				});
		}

		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseIdentificationProvider();
			if (_subscription != null) _subscription.Dispose();
		}
		protected override void CancelChanges() {
			_devModel.RevertChanges();
		}
		protected override void ApplyChanges() {
			UIProvider.Instance.ReleaseIdentificationProvider();
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
					_devModel = devMod;
					UIProvider.Instance.IdentificationProvider.InitView(_devModel, ApplyChanges, CancelChanges);
				}, err => {
					ApplyError(err);
				}, () => {
					ApplyCompleate();
				});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
	}
}
