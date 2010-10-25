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
using nvc.models;
using nvc.onvif;
using System.Windows.Forms;
using System.Threading;

namespace nvc.controllers {
	public class PropertyDepthCalibrationController : IRelesable, IPropertyController {
		DepthCalibrationModel _devModel;
		Session CurrentSession { get; set; }
		Panel _propertyPanel;
		BasePropertyControl _currentControl;
		InformationForm _infoForm;
		IDisposable _subscription;
		
		public void ReleaseAll() {
			if (_subscription != null) _subscription.Dispose();
		}

		void LoadControl() {
			_subscription = _devModel.Load(CurrentSession)
				.Subscribe(arg => {
					_devModel = arg;
					_propertyPanel.SuspendLayout();
					_propertyPanel.Controls.ForEach(x => ((Control)x).Dispose());
					_propertyPanel.Controls.Clear();
					_currentControl = new PropertyDepthCalibration(_devModel) { Dock = DockStyle.Fill, Save = ApplyChanges, Cancel = CancelChanges };
					_propertyPanel.Controls.Add(_currentControl);
					_propertyPanel.ResumeLayout();
				}, err => {
					_infoForm = new InformationForm("ERROR");
					_infoForm.SetErrorMessage(err.Message);
					_infoForm.SetEttorXML(err);
					_infoForm.ShowCloseButton(null);
					_infoForm.ShowDialog(_currentControl);
				});
		}
		public BasePropertyControl CreateController(Panel propertyPanel, Session session, ChannelDescription chan) {
			_propertyPanel = propertyPanel;
			CurrentSession = session;
			_devModel = new DepthCalibrationModel(chan);

			_currentControl = new LoadingPropertyPage();
			_currentControl.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(_currentControl);

			LoadControl();

			return null;
		}
		void CancelChanges() {
			_devModel.RevertChanges();
		}
		void ApplyChanges() {
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
					_devModel = devMod;
				}, err => {
					SaveError(err.Message, err);
				}, () => {
					SaveComplete();
				});
			_infoForm = new InformationForm();
			_infoForm.ShowDialog(_currentControl);
		}

		void SaveComplete() {
			_infoForm.Close();
		}
		void SaveError(string Message, Exception err) {
			_infoForm.SetErrorMessage(err.Message);
			_infoForm.SetEttorXML(err);
			_infoForm.ShowCloseButton(null);
		}
	}
}
