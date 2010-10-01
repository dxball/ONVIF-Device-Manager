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
//
//----------------------------------------------------------------------------------------------------------------

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

		void Localisation(){
			_title.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOTitle"));
			_grpDigitalInputs.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOInputs"));
			_grpDigitalOutputs.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOOutputs"));
			_lblInputCurrentStatus.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOInputCurrentStatus"));
			_lblInputName.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOInputName"));
			_lblInputNormalStatus.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOInputNormalStatus"));
			_lblOutputCurrentStatus.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOOutputCurrentStatus"));
			_lblOutputIdleStatus.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOOutputIdleStatus"));
			_lblOutputName.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOOutputName"));
			_btnTriggerRelay.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOButtonTriggerRelay"));
			_checkRecordChannel.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIORecordChannel"));
			_checkSendMessage.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOEventSendONVIFmessage"));
			_checkSwitchVideo.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOSwitchAnalogue"));
			_checkTriggerRelay.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDigitalIOTriggerRelay"));
		}


        void InitControls()
        {
			Localisation();
            //Colors
            _title.BackColor = ColorDefinition.colTitleBackground;
            _grpDigitalInputs.BackColor = ColorDefinition.colControlBackground;
            _grpDigitalOutputs.BackColor = ColorDefinition.colControlBackground;
            BackColor = ColorDefinition.colControlBackground;
        }
    }
}
