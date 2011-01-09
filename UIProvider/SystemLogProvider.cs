using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using System.Windows.Forms;
using odm.controllers;

namespace odm.controls.UIProvider {
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
