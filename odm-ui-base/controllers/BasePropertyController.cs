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
		protected ChannelDescription CurrentChannel { get; set; }

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
			UIProvider.Instance.GetInfoFormProvider().DisplayErrorForm(err, ReturnToMainFrame);
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

		protected virtual void ReturnToMainFrame() {
			UIProvider.Instance.GetMainFrameProvider().ReturnToMainFrame();
			//UIProvider.Instance.MainFrameProvider.ReleaseLinkSelection();
			ReleaseAll();
		}
		protected virtual void ReturnToDeviceList() {
			WorkflowController.Instance.ReleaseControllers();
		}
		protected virtual void RefreshDevicesList() {
			WorkflowController.Instance.KillEveryBody();
		}

		public virtual void CreateController(Session session, ChannelDescription chan) {
			CurrentSession = session;
			CurrentChannel = chan;

			//Display "property loading" control
			UIProvider.Instance.GetMainFrameProvider().PropertyLoadingControl();

			LoadControl();
		}
	}
}
