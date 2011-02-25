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
using odm.utils;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public abstract class BasePropertyController {//: IRelesable, IPropertyController {
		protected Session CurrentSession {get;set;}
		protected ChannelModel CurrentChannel { get; set; }

		protected abstract void ApplyChanges();
		protected abstract void CancelChanges();
		public abstract void ReleaseAll();
		protected abstract void LoadControl();

		//Display custom information
		protected virtual void InformationBox(string info, Action linkedAction) {
			UIProvider.Instance.GetMainWindowProvider().EnableControls();
			UIProvider.Instance.GetInfoFormProvider().DisplayInformationForm(info, linkedAction);
		}

		//Display apply changes notification
		protected virtual void OnApply(string message) {
			UIProvider.Instance.GetMainWindowProvider().DisableControls();
			UIProvider.Instance.GetInfoFormProvider().DisplayOnApplyChangesForm(message);
		}
		//Display apply changes error
		protected virtual void ApplyError(Exception err) {
			UIProvider.Instance.GetMainWindowProvider().EnableControls();
			UIProvider.Instance.GetInfoFormProvider().DisplayErrorForm(err, ReturnToProperty);
			//UIProvider.Instance.GetInfoFormProvider().DisplayErrorForm(err, ReturnToMainFrame);
		}
		protected virtual void OnCriticalError(Exception err) {
			UIProvider.Instance.GetMainWindowProvider().EnableControls();
			UIProvider.Instance.GetInfoFormProvider().DisplayErrorForm(err, ReturnToDeviceList);
		}
		protected virtual void OnMinorError(Exception err) {
			UIProvider.Instance.GetMainWindowProvider().EnableControls();
			UIProvider.Instance.GetInfoFormProvider().DisplayErrorForm(err, ReturnToMainFrame);
		}
		//Display Apply changes compleate
		protected virtual void ApplyCompleate() {
			UIProvider.Instance.GetMainWindowProvider().EnableControls();
			UIProvider.Instance.GetInfoFormProvider().ReleaseUI();
		}
		protected virtual void ReturnToProperty() {
			UIProvider.Instance.GetMainFrameProvider().ReturnToMainFrame();
			LoadControl();
		}
		protected virtual void ReturnToMainFrame() {
			UIProvider.Instance.GetMainFrameProvider().ReturnToMainFrame();
			//UIProvider.Instance.MainFrameProvider.ReleaseLinkSelection();
			ReleaseAll();
		}
		protected virtual void ReturnToDeviceList() {
			WorkflowController.Instance.ReleaseMainFrameController();
			UIProvider.Instance.ReleaseMainFrameContainer();
		}
		protected virtual void RefreshDevicesList() {
			WorkflowController.Instance.KillEveryBody();
		}

		public virtual void CreateController(Session session, ChannelModel chan) {
			CurrentSession = session;
			CurrentChannel = chan;

			//Display "property loading" control
			UIProvider.Instance.GetMainFrameProvider().PropertyLoadingControl();

			LoadControl();
		}
	}
}
