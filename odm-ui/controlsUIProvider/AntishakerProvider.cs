using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using nvc.controllers;
using nvc.models;
using System.Windows.Forms;

namespace nvc.controlsUIProvider {
	public class AntishakerProvider : BaseUIProvider {
		PropertyAntishaker _antishaker;
		//public void InitView(AntishakerModel devModel, Action ApplyChanges, Action CancelChanges) {
		public void InitView(LiveVideoModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_antishaker = new PropertyAntishaker(devModel) { Dock = DockStyle.Fill, Save = ApplyChanges, 
				Cancel = CancelChanges, onBindingError = BindingError };
			if (datProcInfo != null)
				_antishaker.memFile = datProcInfo.VideoProcessFile;

			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_antishaker);
		}
		public override void ReleaseUI() {
			if (_antishaker != null && !_antishaker.IsDisposed)
				_antishaker.ReleaseAll();
		}
	}
}
