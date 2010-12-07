using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.onvif;
using nvc.models;
using nvc.controlsUIProvider;
using onvifdm.utils;

namespace nvc.controllers {
	public abstract class BasePropertyController {//: IRelesable, IPropertyController {
		protected Session CurrentSession {get;set;}
		protected ChannelDescription CurrentChannel { get; set; }

		protected abstract void ApplyChanges();
		protected abstract void CancelChanges();
		public abstract void ReleaseAll();
		protected abstract void LoadControl();

		//Display custom information
		protected virtual void InformationBox(string info, Action linkedAction) {
			UIProvider.Instance.MainWindowProvider.EnableControls();
			UIProvider.Instance.InfoFormProvider.DisplayInformationForm(info, linkedAction);
		}

		//Display apply changes notification
		protected virtual void OnApply(string message) {
			UIProvider.Instance.MainWindowProvider.DisableControls();
			UIProvider.Instance.InfoFormProvider.DisplayOnApplyChangesForm(message);
		}
		//Display apply changes error
		protected virtual void ApplyError(Exception err) {
			UIProvider.Instance.MainWindowProvider.EnableControls();
			UIProvider.Instance.InfoFormProvider.DisplayErrorForm(err, ReturnToMainFrame);
		}
		protected virtual void OnCriticalError(Exception err) {
			UIProvider.Instance.MainWindowProvider.EnableControls();
			UIProvider.Instance.InfoFormProvider.DisplayErrorForm(err, ReturnToDeviceList);
		}
		protected virtual void OnMinorError(Exception err) {
			UIProvider.Instance.MainWindowProvider.EnableControls();
			UIProvider.Instance.InfoFormProvider.DisplayErrorForm(err, ReturnToMainFrame);
		}
		//Display Apply changes compleate
		protected virtual void ApplyCompleate() {
			UIProvider.Instance.MainWindowProvider.EnableControls();
			UIProvider.Instance.InfoFormProvider.ReleaseUI();
		}

		protected virtual void ReturnToMainFrame() {
			UIProvider.Instance.MainFrameProvider.ReturnToMainFrame();
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
			UIProvider.Instance.MainFrameProvider.PropertyLoadingControl();

			LoadControl();
		}
	}
}
