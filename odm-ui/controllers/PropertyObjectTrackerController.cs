using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.onvif;
using System.Threading;
using nvc.controlsUIProvider;

namespace nvc.controllers {
	public class PropertyObjectTrackerController : BasePropertyController {
		ObjectTrackerModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new ObjectTrackerModel(CurrentChannel);
			_subscription = _devModel.Load(CurrentSession)
				.Subscribe(arg => {
					var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
					UIProvider.Instance.ObjectTrakkerProvider.InitView(_devModel, dprocinfo, ApplyChanges, CancelChanges);
				}, err => {
					OnCriticalError(err);
				});
		}

		protected override void CancelChanges() {
			_devModel.RevertChanges();
		}
		protected override void ApplyChanges() {
			UIProvider.Instance.ReleaseObjectTrakkerProvider();
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
					_devModel = devMod;
					var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
					UIProvider.Instance.ObjectTrakkerProvider.InitView(_devModel, dprocinfo, ApplyChanges, CancelChanges);
				}, err => {
					ApplyError(err);
				}, () => {
					ApplyCompleate();
				});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseObjectTrakkerProvider();
			if (_subscription != null) _subscription.Dispose();
		}

	}
}
