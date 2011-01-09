using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.controllers;
using odm.models;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class AntishakerProvider : BaseUIProvider, IAntishakerProvider {
		PropertyAntishaker _antishaker;

		public void InitView(LiveVideoModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_antishaker = new PropertyAntishaker(devModel) { 
				Save = ApplyChanges, 
				Cancel = CancelChanges, 
				onBindingError = BindingError 
			};
			if (datProcInfo != null)
				//_antishaker.memFile = datProcInfo.VideoProcessFile;

			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_antishaker);
		}
		public override void ReleaseUI() {
			if (_antishaker != null) {
				_antishaker.ReleaseAll();
				_antishaker = null;
			}
		}
	}
}
