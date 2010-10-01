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

namespace nvc.controls
{
	public delegate void SaveNetworkSettingsEvent(NetworkSettings netSett, NetworkStatus netStat);
    public partial class PropertyNetworkSettings : BasePropertyControl
    {
        public PropertyNetworkSettings(DeviceModel devMod)
        {
		    InitializeComponent();
			this.SetDoubleBufferedRecursive(true);
			//panel1.SetDoubleBuffered(true);

			//this.SetAutoScrollMargin			
			_netSet = devMod.GetModelNetworkSettings();
			_netStat = devMod.GetModelNetworkStatus();
			fResetNetworkSettings = devMod.ResetModelNetworkSettings;

            InitControls();
            FillData();
			_saveCancelControl.EnableCancel(false);
			_saveCancelControl.EnableSave(false);
        }

		public event SaveNetworkSettingsEvent SaveData;
		NetworkSettings _netSet;
		NetworkStatus _netStat;
		Action fResetNetworkSettings;

		void Localisation(){
			_title.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyDeviceInfoStatusTitle"));
			_lblDHCP.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyNetworkSettingsDHCP"));
			_lblDNSaddr.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyNetworkSettingsDNSaddr"));
			_lblGateAddr.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyNetworkSettingsGateAddr"));
			_lblIPaddress.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyNetworkSettingsIPaddr"));
			_lblMACaddr.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyNetworkSettingsMACaddr"));
			_lblSubnetMask.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyNetworkSettingsSubnetMask"));
		}

        void InitControls()
        {
			Localisation();
            _saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
            _saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);

			_cbDHCP.Items.Add(Constants.Instance.sCommonAppOn);
			_cbDHCP.Items.Add(Constants.Instance.sCommonAppOff);
			_cbDHCP.SelectedItem = Constants.Instance.sCommonAppOn;

            //Color
            _title.BackColor = ColorDefinition.colTitleBackground;
            BackColor = ColorDefinition.colControlBackground;

            _lblDHCP.BackColor = _lblDNSaddr.BackColor = _lblGateAddr.BackColor =
                _lblIPaddress.BackColor = _lblMACaddr.BackColor = _lblSubnetMask.BackColor = 
                _tbMACaddr.BackColor = ColorDefinition.colControlBackground;

			_saveCancelControl.EnableCancel(false);
			_saveCancelControl.EnableSave(false);
        }

        void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e)
        {
            //Cancel
			fResetNetworkSettings();
            FillData();
			_saveCancelControl.EnableCancel(false);
			_saveCancelControl.EnableSave(false);
        }

        void GetData()
        {
			_netSet.dhcp = ((string)_cbDHCP.SelectedItem) == Constants.Instance.sCommonAppOn ? true : false;
			_netSet.staticDns = _tbmDNS.GetIPAddress();
			_netSet.defaultGateway = _tbmGate.GetIPAddress();
			_netSet.staticIp = _tbmIPaddr.GetIPAddress();
			_netStat.mac = System.Net.NetworkInformation.PhysicalAddress.Parse(_tbMACaddr.Text);
			_netSet.subnetPrefix = DeviceModel.MaskToPrefix(_tbmSubnet.Text);
        }

        void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e)
        {
            //Save
			if (SaveData != null) {
				GetData();
				SaveData(_netSet, _netStat);
			}
        }
        void FillData()
        {
            if (_netSet.dhcp == true)
                SetDHCPOn();
            else
                SetDHCPOff();
			
			_tbmDNS.SetIPAddress(_netSet.staticDns);
			_tbmGate.SetIPAddress(_netSet.defaultGateway);
			_tbmIPaddr.SetIPAddress(_netSet.staticIp);
            _tbmSubnet.Text = DeviceModel.PrefixToMask(_netSet.subnetPrefix);
			_tbMACaddr.Text = _netStat.mac.ToString();
        }


        void SetDHCPOn()
        {
			_cbDHCP.SelectedItem = Constants.Instance.sCommonAppOn;

            _tbmDNS.Enabled = false;
            _tbmGate.Enabled = false;
            _tbmIPaddr.Enabled = false;
            _tbmSubnet.Enabled = false;
        }
        void SetDHCPOff()
        {
			_cbDHCP.SelectedItem = Constants.Instance.sCommonAppOff;

            _tbmDNS.Enabled = true;
            _tbmGate.Enabled = true;
            _tbmIPaddr.Enabled = true;
            _tbmSubnet.Enabled = true;
        }

        private void _cbDHCP_SelectedIndexChanged(object sender, EventArgs e)
        {
			_saveCancelControl.EnableCancel(true);
			_saveCancelControl.EnableSave(true);
			if (_cbDHCP.SelectedItem == (Object)Constants.Instance.sCommonAppOn)
                SetDHCPOn();
            else
                SetDHCPOff();
        }

		private void _tbmIPaddr_TextChanged(object sender, EventArgs e) {
			_saveCancelControl.EnableCancel(true);
			_saveCancelControl.EnableSave(true);
		}

		private void _tbmSubnet_TextChanged(object sender, EventArgs e) {
			_saveCancelControl.EnableCancel(true);
			_saveCancelControl.EnableSave(true);
		}

		private void _tbmGate_TextChanged(object sender, EventArgs e) {
			_saveCancelControl.EnableCancel(true);
			_saveCancelControl.EnableSave(true);
		}

		private void _tbmDNS_TextChanged(object sender, EventArgs e) {
			_saveCancelControl.EnableCancel(true);
			_saveCancelControl.EnableSave(true);
		}

    }
}
