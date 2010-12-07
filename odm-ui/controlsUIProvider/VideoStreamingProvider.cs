using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.models;
using nvc.controls;
using System.Windows.Forms;
using nvc.controllers;

namespace nvc.controlsUIProvider {
	public class VideoStreamingProvider : BaseUIProvider {
		PropertyVideoStreaming _videoStreaming;
		public void InitView(VideoStreamingModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_videoStreaming = new PropertyVideoStreaming(devModel) { Dock = DockStyle.Fill, Save = ApplyChanges,
																	 Cancel = CancelChanges,
																	 onBindingError = BindingError
				};
			if (datProcInfo != null)
				_videoStreaming.memFile = datProcInfo.VideoProcessFile;
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_videoStreaming);
		}
		public void RefreshStream() {
			_videoStreaming.RefershBindings();
			_videoStreaming.InitUrl();
		}
		public override void ReleaseUI() {
			if (_videoStreaming != null && !_videoStreaming.IsDisposed)
				_videoStreaming.ReleaseAll();
		}
	}
}
