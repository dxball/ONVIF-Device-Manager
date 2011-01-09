using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.controllers;
using odm.models;

namespace odm.controls.UIProvider {
	public class InfoFormProvider : BaseUIProvider{
		InfoFormStrings _strings = new InfoFormStrings();
		InfoPageNotification _infoNotification;
		
		public void DisplayOnCheckBoxApplyChangesForm(string message) {
			_infoNotification = new InfoPageNotification() { Message = message, Title = _strings.applyChangesTitle, IsButtonVisible = false };
			UIProvider.Instance.MainFrameProvider.AddInfoControl(_infoNotification);
		}
		public void DisplayOnApplyChangesForm(string message) {
			ReleaseUI();
			_infoNotification = new InfoPageNotification() { Message = message, Title = _strings.applyChangesTitle, IsButtonVisible = false };
			UIProvider.Instance.MainFrameProvider.AddOnApplyInfo(_infoNotification);
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
			UIProvider.Instance.MainFrameProvider.AddInfoControl(_infoNotification);
		}
		public void DisplayErrorForm(Exception err, Action linkedAction) {
			ReleaseUI();
			_infoNotification = new InfoPageNotification() { Message = err.Message, Title = _strings.errorTitle, IsButtonVisible = false};
			UIProvider.Instance.MainFrameProvider.AddInfoControl(_infoNotification);
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
			_infoNotification._DetailBrowser.DocumentText = ErrorDescr.htmlError;
			_infoNotification.IsDetails = true;

			_infoNotification.OnClickAction = linkedAction;
			UIProvider.Instance.MainFrameProvider.AddDeviceLoadInfoControl(_infoNotification);
		}
		//to use during device capabilities loading, while common forms didin't initialised
		void ReleaseDeviceLoadingUI() {
			if (_infoNotification != null && !_infoNotification.IsDisposed) {
				UIProvider.Instance.MainFrameProvider.RemoveInfoControl(_infoNotification);

				_infoNotification.Dispose();
				_infoNotification = null;
			}
		}

		public override void ReleaseUI() {
			if (_infoNotification != null && !_infoNotification.IsDisposed) {
				UIProvider.Instance.MainFrameProvider.RemoveInfoControl(_infoNotification);

				_infoNotification.Dispose();
				_infoNotification = null;
			}
		}
	}
}
