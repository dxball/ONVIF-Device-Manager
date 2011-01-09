using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using System.Windows.Forms;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class SystemLogProvider : BaseUIProvider, ISystemLogProvider {

		PropertySystemLog _sysLog;
		public void InitView() {
			_sysLog = new PropertySystemLog();
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_sysLog);
		}
		public override void ReleaseUI() {
		}
	}
}
