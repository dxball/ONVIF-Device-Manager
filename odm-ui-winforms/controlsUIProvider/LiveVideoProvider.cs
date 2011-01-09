using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using System.Windows.Forms;
using odm.controllers;

namespace odm.controls.UIProvider {
	public class LiveVideoProvider : BaseUIProvider {
		PropertyLiveVideo _liveVideo;
		public void InitView(LiveVideoModel devModel, DataProcessInfo datProcInfo) {
			_liveVideo = new PropertyLiveVideo(devModel) { Dock = DockStyle.Fill };
			if (datProcInfo != null)
				_liveVideo.memFile = datProcInfo.VideoProcessFile;

			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_liveVideo);
		}
		public override void ReleaseUI() {
			if(_liveVideo != null && !_liveVideo.IsDisposed)
				_liveVideo.ReleaseAll();
		}
	}
}
