using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using System.Windows.Forms;
using odm.models;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class TimeSettingsProvider : BaseUIProvider, ITimeSettingsProvider {
		PropertyTimeZone _timeSettings;
		public void InitView(DateTimeSettingsModel devModel, Action apply) {
			_timeSettings = new PropertyTimeZone(devModel) { onBindingError = BindingError, onApply = apply };
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_timeSettings);
		}

		public override void ReleaseUI() {
			if (_timeSettings != null) {
				_timeSettings.ReleaseAll();
				_timeSettings = null;
			}
		}
	}
}