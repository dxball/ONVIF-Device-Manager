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
using odm.ui.controls;
using odm.controllers;
using odm.models;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class InfoFormProvider : BaseUIProvider, IInfoFormProvider {
		InfoFormStrings _strings = new InfoFormStrings();
		InfoPageNotification _infoNotification;

		public void OnCheckBoxApplyChangesComplete() {
			WPFUIProvider.Instance.MainFrameProvider.CloseInfoControl();
		}
		public void DisplayOnCheckBoxApplyChangesForm(string message) {
			_infoNotification = new InfoPageNotification() { Message = message, Title = _strings.applyChangesTitle, IsButtonVisible = false };
			WPFUIProvider.Instance.MainFrameProvider.AddInfoControl(_infoNotification);
		}
		public void DisplayOnApplyChangesForm(string message) {
			ReleaseUI();
			_infoNotification = new InfoPageNotification() { Message = message, Title = _strings.applyChangesTitle, IsButtonVisible = false };
			WPFUIProvider.Instance.MainFrameProvider.AddOnApplyInfo(_infoNotification);
		}
		public void DisplayInformationForm(string message, Action linkedAction) {
			ReleaseUI();
			if (linkedAction == null)
				_infoNotification = new InfoPageNotification() { Message = message, 
					Title = _strings.ifoTitle, 
					IsButtonVisible = false, 
					IsProgressLoadingVisible = false };
			else {
				_infoNotification = new InfoPageNotification() { Message = message, Title = _strings.ifoTitle };
				_infoNotification.OnClickAction = linkedAction;
			}
			WPFUIProvider.Instance.MainFrameProvider.AddInfoControl(_infoNotification);
		}
		public void DisplayErrorForm(Exception err, Action linkedAction) {
			ReleaseUI();
			_infoNotification = new InfoPageNotification() { Message = err.Message, 
				Title = _strings.errorTitle, 
				OnClickAction = linkedAction
			};
			WPFUIProvider.Instance.MainFrameProvider.AddInfoControl(_infoNotification);
		}
		ErrorInfoModel _errorDescr;
		ErrorInfoModel ErrorDescr {
			get {
				if (_errorDescr == null)
					_errorDescr = new ErrorInfoModel(new Exception());
				return _errorDescr;
			}
		}
		void InitDetailsInfo(Exception err) {
			_errorDescr = new ErrorInfoModel(err);
		}
		//to use during device capabilities loading, while common forms didin't initialised
		public void DisplayLoadingDeviceErrorForm(Exception err, Action linkedAction) {
			InitDetailsInfo(err);

			ReleaseDeviceLoadingUI();
			_infoNotification = new InfoPageNotification() { Message = err.Message, Title = _strings.errorTitle};
			_infoNotification.detailsView.NavigateToString(ErrorDescr.htmlError);
			_infoNotification.IsDetails = true;

			//_infoNotification.OnClickAction = linkedAction;
			WPFUIProvider.Instance.MainFrameProvider.AddDeviceLoadInfoControl(_infoNotification);
		}
		//to use during device capabilities loading, while common forms didin't initialised
		void ReleaseDeviceLoadingUI() {
			WPFUIProvider.Instance.MainFrameProvider.RemoveDeviceLoadingInfoControl();
			if (_infoNotification != null) {
				_infoNotification = null;
			}
		}

		public override void ReleaseUI() {
			if (_infoNotification != null) {
				WPFUIProvider.Instance.MainFrameProvider.RemoveInfoControl(_infoNotification);

				_infoNotification = null;
			}
		}
	}
}
