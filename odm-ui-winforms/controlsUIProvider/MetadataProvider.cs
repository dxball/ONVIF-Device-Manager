using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.controllers;

namespace odm.controls.UIProvider {
	public class MetadataProvider : BaseUIProvider {
		PropertyMetadata _metadata;
		DataProcessInfo _dataProc;
		public void InitView(DataProcessInfo dataProc) {
			_metadata = new PropertyMetadata() { Dock = System.Windows.Forms.DockStyle.Fill};
			_dataProc = dataProc;
			if (_dataProc != null)
				_dataProc.callback.Append = _metadata.AppendData;
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_metadata);
		}
		public void ApendData(string data) {
			if (_metadata != null)
				_metadata.AppendData(data);
		}
		public override void ReleaseUI() {
			if(_dataProc != null)
				_dataProc.callback.Append = null;
			if (_metadata != null && !_metadata.IsDisposed)
				_metadata.ReleaseAll();
		}
	}
}
