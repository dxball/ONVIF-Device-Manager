using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using System.Windows.Forms;
using nvc.controllers;

namespace nvc.controlsUIProvider {
	public class SystemLogProvider : BaseUIProvider {

		PropertySystemLog _sysLog;
		public void InitView() {
			_sysLog = new PropertySystemLog() {
				Dock = DockStyle.Fill,
			};
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_sysLog);
		}
		public override void ReleaseUI() {
		}
	}
}
