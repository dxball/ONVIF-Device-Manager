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
using nvc.models;
using nvc.onvif;
using System.Windows.Forms;
using nvc.controls;

namespace nvc.controllers {
	public class PropertyRuleEngineController : IRelesable, IPropertyController {
		RuleEngineModel _devModel;
		Session CurrentSession { get; set; }
		Panel _propertyPanel;
		BasePropertyControl _currentControl;
		InformationForm _infoForm;
		IDisposable _subscription;

		public PropertyRuleEngineController() { 
		}

		public void ReleaseAll() {
			if (_subscription != null) _subscription.Dispose();
		}

		void LoadControl() {
			_propertyPanel.Controls.ForEach(x => ((Control)x).Dispose());
			_propertyPanel.Controls.Clear();
			_currentControl = new PropertyRuleEngine(_devModel) {Dock = DockStyle.Fill, RemoveRule = RemoveRule , AddRule = AddRule , Save = ApplyChanges, Cancel = CancelChanges };
			_propertyPanel.Controls.Add(_currentControl);

			//_subscription = _devModel.Load(CurrentSession)
			//    .Subscribe(arg => {
			//        _devModel = arg;
			//        _propertyPanel.SuspendLayout();
			//        _propertyPanel.Controls.ForEach(x => ((Control)x).Dispose());
			//        _propertyPanel.Controls.Clear();
			//        _currentControl = new PropertyEvents(_devModel) { Dock = DockStyle.Fill, Save = ApplyChanges, Cancel = CancelChanges };
			//        _propertyPanel.Controls.Add(_currentControl);
			//        _propertyPanel.ResumeLayout();
			//    }, err => {
			//        _infoForm = new InformationForm("ERROR");
			//        _infoForm.SetErrorMessage(err.Message);
			//        _infoForm.SetEttorXML(err);
			//        _infoForm.ShowCloseButton(ReturnToMainFrame);
			//        _infoForm.ShowDialog(_propertyPanel);
			//    });
		}
		public BasePropertyControl CreateController(Panel propertyPanel, Session session, ChannelDescription chan) {
			_propertyPanel = propertyPanel;
			CurrentSession = session;
			_devModel = new RuleEngineModel(chan);

			_currentControl = new LoadingPropertyPage();
			_currentControl.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(_currentControl);

			LoadControl();

			return null;
		}
		void CancelChanges() {
			//_devModel.RevertChanges();
		}
		void AddRule() {
			RuleDescriptor descr = new RuleDescriptor() { name = "Rule", enabld = true };
			_devModel.Rules.Add(descr);
		}
		void RemoveRule(RuleDescriptor descr) {
			if(_devModel.Rules.Where(x=> x== descr).Any())
				_devModel.Rules.Remove(descr);
		}

		void ApplyChanges() {
			//_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
			//    .Subscribe(devMod => {
			//        _devModel = devMod;
			//    }, err => {
			//        SaveError(err.Message, err);
			//    }, () => {
			//        SaveComplete();
			//    });
			//_infoForm = new InformationForm();
			//_infoForm.ShowDialog(_currentControl);
		}
		void SaveComplete() {
			_infoForm.Close();
		}
		void SaveError(string Message, Exception err) {
			_infoForm.SetErrorMessage(err.Message);
			_infoForm.SetEttorXML(err);
			_infoForm.ShowCloseButton(null);
			//_infoForm.Close();
		}
		public void ReturnToMainFrame() {
			_propertyPanel.Dispose();
			WorkflowController.Instance.GetMainFrameController().ReleaseLinkSelection();
			WorkflowController.Instance.ReleaseIdentificationController();
		}
	}
}
