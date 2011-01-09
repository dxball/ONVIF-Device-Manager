using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.controls;
using System.Windows.Forms;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class ObjectTrakkerProvider : BaseUIProvider, IObjectTrakkerProvider {
		PropertyObjectTracker _objectTracker;
		public void InitView(ObjectTrackerModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			if (datProcInfo == null)
				return;

			_objectTracker = new PropertyObjectTracker(devModel) { Save = ApplyChanges,
																   Cancel = CancelChanges,
																   onBindingError = BindingError,
																   memFile = datProcInfo.VideoProcessFile
			};
			
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_objectTracker);
		}
		public override void ReleaseUI() {
			if (_objectTracker != null) {
				_objectTracker.ReleaseAll();
				_objectTracker = null;
			}
		}
	}
}
