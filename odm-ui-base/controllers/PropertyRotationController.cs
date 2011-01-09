using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.onvif;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class PropertyRotationController : BasePropertyController {
		AnnotationsModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new AnnotationsModel(CurrentChannel);
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
				UIProvider.Instance.GetRotationProvider().InitView(_devModel, dprocinfo);
			}, err => {
				OnCriticalError(err);
			});
		}

		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseRotationProvider();
			if (_subscription != null) _subscription.Dispose();
		}

		protected override void ApplyChanges() { }
		protected override void CancelChanges() { }

	}
}
