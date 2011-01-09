using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class LiveVideoProvider : BaseUIProvider, ILiveVideoProvider {
		PropertyLiveVideo _liveVideo;
		public void InitView(LiveVideoModel devModel, DataProcessInfo datProcInfo, Action StartRecording, Action StopRecording) {
			if (datProcInfo != null)
				_liveVideo = new PropertyLiveVideo(devModel, datProcInfo.VideoProcessFile, StartRecording, StopRecording);
			else
				_liveVideo = new PropertyLiveVideo(devModel, null, StartRecording, StopRecording);

			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_liveVideo);
		}
		public override void ReleaseUI() {
			if (_liveVideo != null) {
				_liveVideo.ReleaseAll();
				_liveVideo = null;
			}
		}
	}
}
