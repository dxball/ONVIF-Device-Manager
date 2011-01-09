using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class MetadataProvider : BaseUIProvider, IMetadataProvider {
		PropertyMetadata _metadata;
		DataProcessInfo _dataProc;
		public void InitView(DataProcessInfo dataProc) {
			_metadata = new PropertyMetadata();
			_dataProc = dataProc;
			if (_dataProc != null)
				_dataProc.callback.Append = _metadata.AppendData;
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_metadata);
		}
		public void ApendData(string data) {
			if (_metadata != null)
				_metadata.AppendData(data);
		}
		public override void ReleaseUI() {
			if(_dataProc != null)
				_dataProc.callback.Append = null;
			if (_metadata != null) {
				_metadata.ReleaseAll();
				_metadata = null;
			}
		}
	}
}
