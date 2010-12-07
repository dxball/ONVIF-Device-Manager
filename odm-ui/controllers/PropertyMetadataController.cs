using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.onvif;
using nvc.models;
using nvc.controlsUIProvider;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO.Pipes;

namespace nvc.controllers {
	public class PropertyMetadataController : BasePropertyController {

		DataProcessInfo dprocinfo;
		public override void CreateController(Session session, ChannelDescription chan) {
			dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(chan);
			
			UIProvider.Instance.MetadataProvider.InitView(dprocinfo);
		}

		protected override void ApplyChanges() { }
		protected override void CancelChanges() { }
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseMetadataProvider();
		}
		protected override void LoadControl() { }
	}
}
