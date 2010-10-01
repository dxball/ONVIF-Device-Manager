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
using nvc.entities;

namespace nvc.controls
{
    public partial class ErrorMessageControl : UserControl
    {
		DeviceModelInfo _devInfo;
        public ErrorMessageControl(DeviceModelInfo devInfo)
        {
			_devInfo = devInfo;
            InitializeComponent();
            InitializeControl_i();
        }
		void Localisation(){
			_title.DataBindings.Add(new Binding("Text", Constants.Instance, "sErrorDevInfoNull"));
			_tbErrorMsg.Text = Environment.NewLine + "device address: " + _devInfo.IpAddress + Environment.NewLine + Environment.NewLine + _devInfo.ErrorMsg;
		}
        public ErrorMessageControl()
        {
            InitializeComponent();
            InitializeControl_i();
        }

        private void InitializeControl_i()
        {
			Localisation();
            _title.BackColor = ColorDefinition.colTitleBackground;
            BackColor = ColorDefinition.colControlBackground;
            _tbErrorMsg.BackColor = ColorDefinition.colControlBackground;
        }

        public string Title
        {
            set
            {
                _title.Text = value;
            }
        }

        public string Message
        {
            set
            {
                _tbErrorMsg.Text = value;
            }
        }
    }
}
