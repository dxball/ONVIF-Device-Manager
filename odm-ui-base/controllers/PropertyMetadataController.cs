using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.onvif;
using odm.models;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO.Pipes;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class PropertyMetadataController : BasePropertyController {

		DataProcessInfo dprocinfo;
		public override void CreateController(Session session, ChannelDescription chan) {
			dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(chan);
			
			UIProvider.Instance.GetMetadataProvider().InitView(dprocinfo);
		}

		protected override void ApplyChanges() { }
		protected override void CancelChanges() { }
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseMetadataProvider();
		}
		protected override void LoadControl() { }
	}
}
