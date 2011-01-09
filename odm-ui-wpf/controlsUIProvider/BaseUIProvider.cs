using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace odm.controlsUIProvider {
	public abstract class BaseUIProvider {
		public abstract void ReleaseUI();
		public virtual void BindingError(Exception err, string message) {
			WPFUIProvider.Instance.MainFrameProvider.ClearPropertyContainer();
			WPFUIProvider.Instance.MainFrameProvider.ReleaseLinkSelection();
			WPFUIProvider.Instance.InfoFormProvider.DisplayInformationForm(message, null);
		}

	}
}
