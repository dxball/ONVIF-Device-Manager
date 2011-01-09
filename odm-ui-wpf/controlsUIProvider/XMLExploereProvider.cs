using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.models;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class XMLExploereProvider : BaseUIProvider, IXMLExplorerProvider {
		PropertyXMLExplorer _propXmpExpl;
		public void InitView(DumpModel devModel) {
			_propXmpExpl = new PropertyXMLExplorer(devModel);
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_propXmpExpl);
		}
		public override void ReleaseUI() {
			if (_propXmpExpl != null) {
				_propXmpExpl.ReleaseAll();
				_propXmpExpl = null;
			}
		}
	}
}
