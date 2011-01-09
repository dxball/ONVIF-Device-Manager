using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class PropertyXMLExplorerController : BasePropertyController {

		DumpModel _devModel;
		IDisposable _subscription;
		protected override void LoadControl() {
			_devModel = new DumpModel();
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				UIProvider.Instance.GetXMLExplorerProvider().InitView(_devModel);
			}, err => {
				OnCriticalError(err);
			});
		}
		protected override void ApplyChanges() {}
		protected override void CancelChanges() {}
		public override void ReleaseAll() { }
	}
}
