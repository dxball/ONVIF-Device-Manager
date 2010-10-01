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
using nvc.models;
using nvc.entities;
using System.Concurrency;

namespace nvc.controls
{
    public partial class PropertyDeviceIdentificationAndStatus : BasePropertyControl
    {
        public PropertyDeviceIdentificationAndStatus(DeviceModel devMod)
        {
			this.SuspendLayout();
            InitializeComponent();

			this.SetDoubleBufferedRecursive(true);
			
			//panel1.SetDoubleBuffered(true);
			_devMod = devMod;

            InitControls();
            FillData();
			this.ResumeLayout();
        }

		DeviceModel _devMod;

		void Localisation(){
			_title.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDeviceInfoStatusTitle"));
			_lblDeviceId.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDeviceInfoStatusLableDeviceID"));
			_lblFirmware.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDeviceInfoStatusLableFirmware"));
			_lblHardware.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDeviceInfoStatusLableHardware"));
			_lblIP.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDeviceInfoStatusLableIPAddr"));
			_lblMac.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDeviceInfoStatusLableMACAddr"));
			_lblName.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDeviceInfoStatusLableName"));
		}

		void InitControls() {
			Localisation();
			_saveCancelControl.EnableSave(false);

			//set colors
			BackColor = ColorDefinition.colControlBackground;
			_title.BackColor = ColorDefinition.colTitleBackground;
			_lblDeviceId.BackColor = ColorDefinition.colControlBackground;
			_lblFirmware.BackColor = ColorDefinition.colControlBackground;
			_lblHardware.BackColor = ColorDefinition.colControlBackground;
			_lblIP.BackColor = ColorDefinition.colControlBackground;
			_lblMac.BackColor = ColorDefinition.colControlBackground;
			_lblName.BackColor = ColorDefinition.colControlBackground;
			_tbDeviceId.BackColor = ColorDefinition.colControlBackground;
			_tbFirmware.BackColor = ColorDefinition.colControlBackground;
			_tbHardware.BackColor = ColorDefinition.colControlBackground;
			_tbIP.BackColor = ColorDefinition.colControlBackground;
			_tbMac.BackColor = ColorDefinition.colControlBackground;
		}

        void FillData()
        {
			_tbDeviceId.Text = _devMod.GetDeviceId();
			_tbFirmware.Text = _devMod.GetDeviceFirmware();
			_tbHardware.Text = _devMod.GetDeviceHardware();
			_tbIP.SetIPAddress(_devMod.GetModelNetworkStatus().ip);
			_tbMac.Text = _devMod.GetModelNetworkStatus().mac.ToString();
			_tbName.Text = _devMod.GetDeviceName();
        }
    }
}
