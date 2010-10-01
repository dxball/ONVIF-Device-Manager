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
	public class PropertyLiveVideoController : IRelesable, IPropertyController {
		DeviceChannel _devModel = WorkflowController.Instance.GetCurrentDevice().GetCurrentChannel();
		Panel _propertyPanel;
		BasePropertyControl _currentControl;
		IDisposable _subscriptionVideoURI;

		public PropertyLiveVideoController() {

		}
		#region Init section
		public BasePropertyControl CreateController(Panel propertyPanel) {
			_propertyPanel = propertyPanel;
			
			if (_devModel.IsVideoURLInitialised != true) {
				_subscriptionVideoURI = WorkflowController.Instance.SubscribeToVideoURI(_devModel);
			    _devModel.VideoURLInitialised += new EventHandler(_devModel_LiveVideoInitialised);
				_currentControl = new LoadingPropertyPage();
			} else {
				_currentControl = CreateProperty();
			}

			_propertyPanel.Controls.Clear();
			_currentControl.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(_currentControl);

			return _currentControl;
		}

		public BasePropertyControl CreateProperty() {
			var property = new PropertyLiveVideo(_devModel);
			property.Dock = DockStyle.Fill;
			return property;
		}
		#endregion

		#region EventsHandlers
		void _devModel_LiveVideoInitialised(object sender, EventArgs e) {
			var control = CreateProperty();
			_propertyPanel.Controls.Clear();
			control.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(control);
		}
		#endregion

		#region release resources
		public bool disposed = false;
		public void ReleaseAll() {
			if (_subscriptionVideoURI != null)
				_subscriptionVideoURI.Dispose();
			disposed = true;
		}
		#endregion
	}
}
