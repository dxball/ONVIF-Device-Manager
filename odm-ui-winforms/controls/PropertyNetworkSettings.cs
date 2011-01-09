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
using odm.models;
using odm.utils.entities;
using System.Net;
using odm.utils;

namespace odm.controls
{
    public partial class PropertyNetworkSettings : BasePropertyControl
    {
		protected PropertyNetworkSettingsStrings strings = PropertyNetworkSettingsStrings.Instance;
		public override void ReleaseUnmanaged() {}
		DeviceNetworkSettingsModel _devMod;

		public PropertyNetworkSettings(DeviceNetworkSettingsModel devMod)
        {
			_devMod = devMod;
		    InitializeComponent();
			this.SetDoubleBufferedRecursive(true);
			
			Load += new EventHandler(PropertyNetworkSettings_Load);
        }

		void PropertyNetworkSettings_Load(object sender, EventArgs e) {
			BindData(_devMod);
			InitControls();
		}

		public Action Save { get; set; }
		public Action Cancel { get; set; }

		void BindData(DeviceNetworkSettingsModel devModel) {
			try{
			_tbMACaddr.CreateBinding(x => x.Text, devModel, x => x.mac);
			} catch (Exception err) {
				string strValue;
				if (devModel.mac == null)
					strValue = "Null";
				else
					strValue = devModel.mac;
				BindingError(err, ExceptionStrings.Instance.errBindNetworkSetMac + strValue);
			}
			try{
			_tbmDNS.CreateBinding(x => x.IPAddress, devModel, x => x.staticDns, DataSourceUpdateMode.OnPropertyChanged);
			_tbmDNS.CreateBinding(x => x.Enabled, devModel, x => x.dhcp, x=>!x);
			} catch (Exception err) {
				string strValue;
				if (devModel.staticDns == null)
					strValue = "Null";
				else
					strValue = devModel.staticDns.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindStaticDns + strValue);
			}
			try{
			_tbmGate.CreateBinding(x => x.IPAddress, devModel, x => x.staticGateway, DataSourceUpdateMode.OnPropertyChanged);
			_tbmGate.CreateBinding(x => x.Enabled, devModel, x => x.dhcp, x => !x);
			} catch (Exception err) {
				string strValue;
				if (devModel.staticGateway == null)
					strValue = "Null";
				else
					strValue = devModel.staticGateway.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindStaticGateway + strValue);
			}
			try{
			_tbmSubnet.CreateBinding(x => x.IPAddress, devModel, x => x.subnetMask, DataSourceUpdateMode.OnPropertyChanged);
			_tbmSubnet.CreateBinding(x => x.Enabled, devModel, x => x.dhcp, x => !x);
			} catch (Exception err) {
				string strValue;
				if (devModel.subnetMask == null)
					strValue = "Null";
				else
					strValue = devModel.subnetMask.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindSubnetMask + strValue);
			}
			try{
			_tbmIPaddr.CreateBinding(x => x.IPAddress, devModel, x => x.staticIp, DataSourceUpdateMode.OnPropertyChanged);
			_tbmIPaddr.CreateBinding(x => x.Enabled, devModel, x => x.dhcp, x => !x);
			} catch (Exception err) {
				string strValue;
				if (devModel.staticIp == null)
					strValue = "Null";
				else
					strValue = devModel.staticIp.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindStaticIp + strValue);
			} 

			_cbDHCP.Items.Add(true);
			_cbDHCP.Items.Add(false);
			_cbDHCP.Format += (sender, cevent) => {
				dbg.Assert(typeof(bool)==cevent.Value.GetType());
				var value = (bool)cevent.Value;
				cevent.Value = value ? "On" : "Off";
			};

			try{
			_cbDHCP.CreateBinding(x => x.SelectedItem, devModel, x => x.dhcp, DataSourceUpdateMode.OnPropertyChanged);
			_cbDHCP.SelectedValueChanged += (sender, args) => {
				if (_cbDHCP.SelectedItem != null) {
					devModel.dhcp = (bool)_cbDHCP.SelectedItem;
				}
			};
			} catch (Exception err) {
				string strValue;
				if (devModel.dhcp == null)
					strValue = "Null";
				else
					strValue = devModel.dhcp.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindDhcp + strValue);
			}
			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}
		void Localization(){
			_title.CreateBinding(x => x.Text,strings, x => x.title);
			_lblDHCP.CreateBinding(x => x.Text,strings, x => x.dhcp);
			_lblDNSaddr.CreateBinding(x => x.Text,strings, x => x.dnsAddr);
			_lblGateAddr.CreateBinding(x => x.Text,strings, x => x.gateAddr);
			_lblIPaddress.CreateBinding(x => x.Text, strings, x => x.ipAddr);
			_lblMACaddr.CreateBinding(x => x.Text, strings, x => x.macAddr);
			_lblSubnetMask.CreateBinding(x => x.Text, strings, x => x.subnetMask);
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
