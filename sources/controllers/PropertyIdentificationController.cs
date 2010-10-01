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
using nvc.entities;

namespace nvc.controllers {
	public class PropertyIdentificationController : IRelesable, IPropertyController {
		DeviceModel _devModel = WorkflowController.Instance.GetCurrentDevice();
		Panel _propertyPanel;
		BasePropertyControl _currentControl;
		IDisposable _subscriptionNetworkStatus;

		public PropertyIdentificationController() {

		}

		#region IDisposable
		public bool disposed = false;
		public void ReleaseAll() {
			disposed = true;

			if (_subscriptionNetworkStatus != null)
				_subscriptionNetworkStatus.Dispose();
			_devModel.IdentificationInitialised -= _devModel_IdentificationInitialised;
		}
		#endregion IDisposable

		public BasePropertyControl CreateController(Panel propertyPanel) {
			_propertyPanel = propertyPanel;
			if (_devModel.IsPropertyIdentificationReady != true) {
				_subscriptionNetworkStatus = WorkflowController.Instance.SubscribeToNetworkStatus();
				_devModel.IdentificationInitialised += new EventHandler(_devModel_IdentificationInitialised);
				_currentControl = new LoadingPropertyPage();
			} else {
				_currentControl = CreatePropertyIdentification();
			}

			_propertyPanel.Controls.Clear();
			_currentControl.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(_currentControl);

			return _currentControl;
		}

		void _devModel_IdentificationInitialised(object sender, EventArgs e) {
			var control = CreatePropertyIdentification();
			_propertyPanel.Controls.Clear();
			control.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(control);
		}
		public BasePropertyControl CreatePropertyIdentification() {
			var propertyIdentification = new PropertyDeviceIdentificationAndStatus(_devModel);
			propertyIdentification.Dock = DockStyle.Fill;
			return propertyIdentification;
		}

	}
}
