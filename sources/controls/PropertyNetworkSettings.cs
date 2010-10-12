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
using nvc.models;
using nvc.entities;
using System.Net;
using nvc.controls.utils;
using nvc.utils;

namespace nvc.controls
{
    public partial class PropertyNetworkSettings : BasePropertyControl
    {
		public PropertyNetworkSettings(DeviceNetworkSettingsModel devMod)
        {
		    InitializeComponent();
			this.SetDoubleBufferedRecursive(true);
			
			BindData(devMod);
			InitControls();
        }

		public Action Save { get; set; }
		public Action Cancel { get; set; }

		void BindData(DeviceNetworkSettingsModel devModel) {
			_tbMACaddr.CreateBinding(x => x.Text, devModel, x => x.mac);
			
			_tbmDNS.CreateBinding(x => x.IPAddress, devModel, x => x.staticDns, DataSourceUpdateMode.OnPropertyChanged);
			_tbmDNS.CreateBinding(x => x.Enabled, devModel, x => x.dhcp, x=>!x);

			_tbmGate.CreateBinding(x => x.IPAddress, devModel, x => x.staticGateway, DataSourceUpdateMode.OnPropertyChanged);
			_tbmGate.CreateBinding(x => x.Enabled, devModel, x => x.dhcp, x => !x);

			_tbmSubnet.CreateBinding(x => x.IPAddress, devModel, x => x.subnetMask, DataSourceUpdateMode.OnPropertyChanged);
			_tbmSubnet.CreateBinding(x => x.Enabled, devModel, x => x.dhcp, x => !x);

			_tbmIPaddr.CreateBinding(x => x.IPAddress, devModel, x => x.staticIp, DataSourceUpdateMode.OnPropertyChanged);
			_tbmIPaddr.CreateBinding(x => x.Enabled, devModel, x => x.dhcp, x => !x);
			
			_cbDHCP.Items.Add(true);
			_cbDHCP.Items.Add(false);
			_cbDHCP.Format += (sender, cevent) => {
				DebugHelper.Assert(typeof(bool)==cevent.Value.GetType());
				var value = (bool)cevent.Value;
				cevent.Value = value ? "On" : "Off";
			};

			_cbDHCP.CreateBinding(x => x.SelectedItem, devModel, x => x.dhcp, DataSourceUpdateMode.OnPropertyChanged);
			_cbDHCP.SelectedValueChanged += (sender, args) => {
				if (_cbDHCP.SelectedItem != null) {
					devModel.dhcp = (bool)_cbDHCP.SelectedItem;
				}
			};
			
			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}
		void Localization(){
			_title.CreateBinding(x => x.Text,Constants.Instance, x => x.sPropertyDeviceInfoStatusTitle);
			_lblDHCP.CreateBinding(x => x.Text,Constants.Instance, x => x.sPropertyNetworkSettingsDHCP);
			_lblDNSaddr.CreateBinding(x => x.Text,Constants.Instance, x => x.sPropertyNetworkSettingsDNSaddr);
			_lblGateAddr.CreateBinding(x => x.Text,Constants.Instance, x => x.sPropertyNetworkSettingsGateAddr);
			_lblIPaddress.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyNetworkSettingsIPaddr);
			_lblMACaddr.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyNetworkSettingsMACaddr);
			_lblSubnetMask.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyNetworkSettingsSubnetMask);
		}

        void InitControls()
        {
			Localization();

            //Color
            _title.BackColor = ColorDefinition.colTitleBackground;
            BackColor = ColorDefinition.colControlBackground;

            _lblDHCP.BackColor = _lblDNSaddr.BackColor = _lblGateAddr.BackColor =
                _lblIPaddress.BackColor = _lblMACaddr.BackColor = _lblSubnetMask.BackColor = 
                _tbMACaddr.BackColor = ColorDefinition.colControlBackground;

			_saveCancelControl.ButtonClickedCancel +=new EventHandler(_saveCancelControl_ButtonClickedCancel);
			_saveCancelControl.ButtonClickedSave +=new EventHandler(_saveCancelControl_ButtonClickedSave);
        }

		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			//Cancel
			Cancel();
		}
		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			//Save
			Save();
		}
    }
}
