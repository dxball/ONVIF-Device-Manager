using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.controlsUIProvider;
using System.Threading;

namespace nvc.controllers {
	public class PropertyImagingSettingsController : BasePropertyController {
		ImagingSettingsModel _devModel;
		IDisposable _subscription;
		
		protected override void LoadControl() {
			_devModel = new ImagingSettingsModel(CurrentChannel);
			_subscription = _devModel.Load(CurrentSession)
				.Subscribe(arg => {
					var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
					UIProvider.Instance.ImagingSettingsProvider.InitView(_devModel, dprocinfo, ApplyChanges, CancelChanges);
				}, err => {
					OnCriticalError(err);
				});
		}

		protected override void CancelChanges() {
			_devModel.RevertChanges();
		}
		protected override void ApplyChanges() {
			UIProvider.Instance.ReleaseImagingSettingsProvider();
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
					_devModel = devMod;
					var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
					UIProvider.Instance.ImagingSettingsProvider.InitView(_devModel, dprocinfo, ApplyChanges, CancelChanges);
				}, err => {
					ApplyError(err);
				}, () => {
					ApplyCompleate();
				});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseImagingSettingsProvider();
			if (_subscription != null) _subscription.Dispose();
		}
	}
}
