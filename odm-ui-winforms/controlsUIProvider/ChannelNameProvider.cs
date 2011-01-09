using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using nvc.controls;
using System.Windows.Forms;
using odm.models;
using odm.controls.UIProvider;
//using nvc.models;

namespace nvc.controlsl.UIProvider {
	public class ChannelNameProvider : BaseUIProvider {
		//PropertyChannelName _channelName;
		public void InitView(ChannelDescription devChannel) {
			//_channelName = new PropertyChannelName(devChannel) { Dock = DockStyle.Fill};
			//UIProvider.Instance.MainFrameProvider.AddPropertyControl(_channelName);
		}

		public override void ReleaseUI() {
			//if (_channelName != null && !_channelName.IsDisposed)
			//    _channelName.ReleaseAll();
		}
	}
}