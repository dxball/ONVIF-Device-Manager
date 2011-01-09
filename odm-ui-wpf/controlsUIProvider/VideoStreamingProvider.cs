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
	public class VideoStreamingProvider : BaseUIProvider, IVideoStreamingProvider {
		PropertyVideoStreaming _videoStreaming;
		public void InitView(VideoStreamingModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			if (datProcInfo == null)
				return;

			_videoStreaming = new PropertyVideoStreaming(devModel) { Save = ApplyChanges,
																	 Cancel = CancelChanges,
																	 onBindingError = BindingError,
																	 memFile = datProcInfo.VideoProcessFile
				};
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_videoStreaming);
		}
		public void RefreshStream() {
			_videoStreaming.RefershBindings();
			_videoStreaming.InitUrl();
		}
		public override void ReleaseUI() {
			if (_videoStreaming != null) {
				_videoStreaming.ReleaseAll();
				_videoStreaming = null;
			}
		}
	}
}
