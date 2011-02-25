#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.ui.controls;
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
