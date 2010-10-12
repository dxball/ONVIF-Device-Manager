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

namespace nvc.controls
{
    public partial class PropertyDigitalIO : BasePropertyControl
    {
        public PropertyDigitalIO()
        {
            InitializeComponent();

            InitControls();
        }

		void Localization(){
			_title.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOTitle);
			_grpDigitalInputs.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOInputs);
			_grpDigitalOutputs.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOOutputs);
			_lblInputCurrentStatus.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOInputCurrentStatus);
			_lblInputName.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOInputName);
			_lblInputNormalStatus.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOInputNormalStatus);
			_lblOutputCurrentStatus.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOOutputCurrentStatus);
			_lblOutputIdleStatus.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOOutputIdleStatus);
			_lblOutputName.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOOutputName);
			_btnTriggerRelay.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOButtonTriggerRelay);
			_checkRecordChannel.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIORecordChannel);
			_checkSendMessage.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOEventSendONVIFmessage);
			_checkSwitchVideo.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOSwitchAnalogue);
			_checkTriggerRelay.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyDigitalIOTriggerRelay);
		}


        void InitControls()
        {
			Localization();
            //Colors
            _title.BackColor = ColorDefinition.colTitleBackground;
            _grpDigitalInputs.BackColor = ColorDefinition.colControlBackground;
            _grpDigitalOutputs.BackColor = ColorDefinition.colControlBackground;
            BackColor = ColorDefinition.colControlBackground;
        }
    }
}
