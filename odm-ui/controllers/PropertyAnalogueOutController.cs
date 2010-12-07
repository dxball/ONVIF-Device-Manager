using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.onvif;
using nvc.controlsUIProvider;

namespace nvc.controllers {
	public class PropertyAnalogueOutController : BasePropertyController {
		ChannelDescription CurrentChannel { get; set; }
		DeviceIdentificationModel _devModel;
		Session _session;
		IDisposable _subscription;

		public PropertyAnalogueOutController() {
		}

		protected override void LoadControl() {
			_devModel = new DeviceIdentificationModel();
			_subscription = _devModel.Load(_session).Subscribe(arg => {
				UIProvider.Instance.AnalogueOutProvider.InitView(_devModel);
			}, err => {
				OnCriticalError(err);
			});
		}
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseAnalogueOutProvider();
			if (_subscription != null) _subscription.Dispose();
		}
		protected override void ApplyChanges() { }
		protected override void CancelChanges() { }
	}
}
