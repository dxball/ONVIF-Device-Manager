using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using System.Threading;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class PropertyTimeSettingsController : BasePropertyController {
		DateTimeSettingsModel _devModel;
		IDisposable _subscription;

		protected override void ApplyChanges() {
			UIProvider.Instance.ReleaseTimeSettingsProvider();
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
				}, err => {
					ApplyError(err);
				}, () => {
					ApplyCompleate();
				});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
		protected override void ApplyCompleate() {
			//base.ApplyCompleate();

			LoadControl();
		}
		protected override void CancelChanges() { }
		protected override void LoadControl() {
			_devModel = new DateTimeSettingsModel();
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				UIProvider.Instance.GetTimeSettingsProvider().InitView(_devModel, ApplyChanges);
			}, err => {
				OnCriticalError(err);
			});
		}

		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseTimeSettingsProvider();
			if (_subscription != null) _subscription.Dispose();
		}
	}
}
