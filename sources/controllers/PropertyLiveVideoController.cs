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
using nvc.onvif;
using nvc.models;
using nvc.utils;

namespace nvc.controllers {
	public class PropertyLiveVideoController : IRelesable, IPropertyController {
		ChannelDescription CurrentChannel { get; set; }
		LiveVideoModel _devModel;
		Session _session;
		Panel _propertyPanel;
		BasePropertyControl _currentControl;
		InformationForm _savingSettingsForm;
		IDisposable _subscription;

		public PropertyLiveVideoController() {
		}
	
		void LoadControl() {
			_devModel = new LiveVideoModel(CurrentChannel);
			_subscription = _devModel.Load(_session).Subscribe(arg => {
				_devModel = arg;
				_propertyPanel.SuspendLayout();
				_propertyPanel.Controls.ForEach(x=>((Control)x).Dispose());
				_propertyPanel.Controls.Clear();
				_currentControl = new PropertyLiveVideo(_devModel) { Dock = DockStyle.Fill};
				_propertyPanel.Controls.Add(_currentControl);
				_propertyPanel.ResumeLayout();
			}, err => {
				DebugHelper.Error(err);
				_savingSettingsForm = new InformationForm("ERROR");
				_savingSettingsForm.SetErrorMessage(err.Message);
				_savingSettingsForm.ShowCloseButton(null);
				_savingSettingsForm.ShowDialog(_propertyPanel);
			});
		}
		public void KillEveryOne() {
			WorkflowController.Instance.KillEveryBody();
		}
		public BasePropertyControl CreateController(Panel propertyPanel, Session session, ChannelDescription channel) {
			CurrentChannel = channel;
			_propertyPanel = propertyPanel;
			_session = session;

			_currentControl = new LoadingPropertyPage();
			_propertyPanel.Controls.Clear();
			_currentControl.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(_currentControl);
			
			LoadControl();

			return _currentControl;
		}
		#region release resources
		public void ReleaseAll() {
			if (_subscription != null) _subscription.Dispose();
		}
		#endregion
	}
}
