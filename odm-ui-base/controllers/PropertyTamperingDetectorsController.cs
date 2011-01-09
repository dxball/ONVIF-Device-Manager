using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.onvif;
using System.Threading;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class PropertyTamperingDetectorsController : BasePropertyController {
		AnnotationsModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new AnnotationsModel(CurrentChannel);
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
				UIProvider.Instance.GetTamperingDetectorsProvider().InitView(_devModel, dprocinfo, ApplyChanges, CancelChanges);
			}, err => {
				OnCriticalError(err);
			});
		}

		protected override void CancelChanges() {
			_devModel.RevertChanges();
		}
		protected override void ApplyChanges() {
			UIProvider.Instance.ReleaseTamperingDetectorsProvider();
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
					var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
					UIProvider.Instance.GetTamperingDetectorsProvider().InitView(_devModel, dprocinfo, ApplyChanges, CancelChanges);
				}, err => {
					ApplyError(err);
				}, () => {
					ApplyCompleate();
				});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}

		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseTamperingDetectorsProvider();
			if (_subscription != null) _subscription.Dispose();
		}
	}
}