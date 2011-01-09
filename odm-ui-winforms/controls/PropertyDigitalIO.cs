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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using odm.utils;

namespace odm.controls {
	public partial class PropertyDigitalIO : BasePropertyControl {
		protected PropertyDigitalIOStrings strings = PropertyDigitalIOStrings.Instance;
		public override void ReleaseUnmanaged() { }

		public PropertyDigitalIO() {
			InitializeComponent();

			Load += new EventHandler(PropertyDigitalIO_Load);
		}

		void PropertyDigitalIO_Load(object sender, EventArgs e) {
			InitControls();
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, strings, x => x.title);
			_grpDigitalInputs.CreateBinding(x => x.Text, strings, x => x.inputs);
			_grpDigitalOutputs.CreateBinding(x => x.Text, strings, x => x.outputs);
			_lblInputCurrentStatus.CreateBinding(x => x.Text, strings, x => x.inputCurrentStatus);
			_lblInputName.CreateBinding(x => x.Text, strings, x => x.inputName);
			_lblInputNormalStatus.CreateBinding(x => x.Text, strings, x => x.inputNormalStatus);
			_lblOutputCurrentStatus.CreateBinding(x => x.Text, strings, x => x.outputCurrentStatus);
			_lblOutputIdleStatus.CreateBinding(x => x.Text, strings, x => x.outputIdleStatus);
			_lblOutputName.CreateBinding(x => x.Text, strings, x => x.outputName);
			_btnTriggerRelay.CreateBinding(x => x.Text, strings, x => x.buttonTriggerRelay);
			_checkRecordChannel.CreateBinding(x => x.Text, strings, x => x.recordChannel);
			_checkSendMessage.CreateBinding(x => x.Text, strings, x => x.eventSendONVIFmessage);
			_checkSwitchVideo.CreateBinding(x => x.Text, strings, x => x.switchAnalogue);
			_checkTriggerRelay.CreateBinding(x => x.Text, strings, x => x.triggerRelay);
		}


		void InitControls() {
			Localization();
			//Colors
			_title.BackColor = ColorDefinition.colTitleBackground;
			_grpDigitalInputs.BackColor = ColorDefinition.colControlBackground;
			_grpDigitalOutputs.BackColor = ColorDefinition.colControlBackground;
			BackColor = ColorDefinition.colControlBackground;
		}
	}
}
