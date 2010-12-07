using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.controls;
using System.Windows.Forms;
using nvc.controllers;

namespace nvc.controlsUIProvider {
	public class ObjectTrakkerProvider : BaseUIProvider {
		PropertyObjectTracker _objectTracker;
		public void InitView(ObjectTrackerModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_objectTracker = new PropertyObjectTracker(devModel) { Dock = DockStyle.Fill, Save = ApplyChanges,
																   Cancel = CancelChanges,
																   onBindingError = BindingError
			};
			if (datProcInfo != null)
				_objectTracker.memFile = datProcInfo.VideoProcessFile;

			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_objectTracker);
		}
		public override void ReleaseUI() {
			if (_objectTracker != null && !_objectTracker.IsDisposed)
				_objectTracker.ReleaseAll();
		}
	}
}
