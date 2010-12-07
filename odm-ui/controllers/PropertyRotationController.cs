using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.onvif;
using nvc.controlsUIProvider;

namespace nvc.controllers {
	public class PropertyRotationController : BasePropertyController {
		AnnotationsModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new AnnotationsModel(CurrentChannel);
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
				UIProvider.Instance.RotationProvider.InitView(_devModel, dprocinfo);
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
