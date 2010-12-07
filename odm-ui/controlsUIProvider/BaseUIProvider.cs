using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nvc.controlsUIProvider {
	public abstract class BaseUIProvider {
		public abstract void ReleaseUI();
		public virtual void BindingError(Exception err, string message) {
			UIProvider.Instance.MainFrameProvider.ClearPropertyContainer();
			UIProvider.Instance.MainFrameProvider.ReleaseLinkSelection();
			UIProvider.Instance.InfoFormProvider.DisplayInformationForm(message, null);
		}
	}
}
