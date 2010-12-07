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
using System.Concurrency;

namespace nvc.controls
{
    public partial class PropertyDeviceIdentificationAndStatus : BasePropertyControl
    {
		PropertyIdentificationStrings _strings = new PropertyIdentificationStrings();
		public override void ReleaseUnmanaged() { }
        public PropertyDeviceIdentificationAndStatus(DeviceIdentificationModel devMod)
        {
			this.SuspendLayout();
            InitializeComponent();
			_devModel = devMod;

			this.SetDoubleBufferedRecursive(true);
			Load += new EventHandler(PropertyDeviceIdentificationAndStatus_Load);
			this.ResumeLayout();

        }

		void PropertyDeviceIdentificationAndStatus_Load(object sender, EventArgs e) {
			this.SuspendLayout();
			BindData(_devModel);
			InitControls();
			this.ResumeLayout();
		}

		DeviceIdentificationModel _devModel;
		public Action Save { get; set; }
		public Action Cancel { get; set; }

		void BindData(DeviceIdentificationModel devModel) {
			try {
				_tbName.CreateBinding(x => x.Text, devModel, x => x.Name);
			} catch (Exception err) {
				string strValue;
				if (devModel.Name == null)
					strValue = "Null";
				else
					strValue = devModel.Name;
				BindingError(err, ExceptionStrings.Instance.errBindName + strValue);
			}
			try {
				_tbFirmware.CreateBinding(x => x.Text, devModel, x => x.FirmwareVer);
			} catch (Exception err) {
				string strValue;
				if (devModel.FirmwareVer == null)
					strValue = "Null";
				else
					strValue = devModel.FirmwareVer;
				BindingError(err, ExceptionStrings.Instance.errBindFirmwareVer + strValue);
			}
			try {
				_tbDeviceId.CreateBinding(x => x.Text, devModel, x => x.DeviceID);
			} catch (Exception err) {
				string strValue;
				if (devModel.DeviceID == null)
					strValue = "Null";
				else
					strValue = devModel.DeviceID;
				BindingError(err, ExceptionStrings.Instance.errBindDeviceID + strValue);
			}
			try {
			_tbHardware.CreateBinding(x => x.Text, devModel, x => x.HardwareVer);
			} catch (Exception err) {
				string strValue;
				if (devModel.HardwareVer == null)
					strValue = "Null";
				else
					strValue = devModel.HardwareVer;
				BindingError(err, ExceptionStrings.Instance.errBindHardwareVer + strValue);
			}
			try {
			_tbIP.CreateBinding(x => x.Text, devModel, x => x.NetworkIPAddress);
			} catch (Exception err) {
				string strValue;
				if (devModel.NetworkIPAddress == null)
					strValue = "Null";
				else
					strValue = devModel.NetworkIPAddress;
				BindingError(err, ExceptionStrings.Instance.errBindNetworkIPAddress + strValue);
			}
			try {
			_tbMac.CreateBinding(x => x.Text, devModel, x => x.MACAddress);
			} catch (Exception err) {
				string strValue;
				if (devModel.MACAddress == null)
					strValue = "Null";
				else
					strValue = devModel.MACAddress;
				BindingError(err, ExceptionStrings.Instance.errBindMACAddress + strValue);
			}

			_dtCurTime.Format = DateTimePickerFormat.Custom;
			_dtCurTime.CustomFormat = "HH':'mm tt dd'.'MM'.'yyyy";

			try {
			_dtCurTime.CreateBinding(x=>x.Value, devModel, x=>x.dateTime);
			} catch (Exception err) {
				string strValue;
				if (devModel.dateTime == null)
					strValue = "Null";
				else
					strValue = devModel.dateTime.ToString();
				BindingError(err, ExceptionStrings.Instance.errBinddateTime + strValue);
			}

			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}
		void Localization(){
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_lblDeviceId.CreateBinding(x => x.Text, _strings, x => x.deviceID);
			_lblFirmware.CreateBinding(x => x.Text, _strings, x => x.firmware);
			_lblHardware.CreateBinding(x => x.Text, _strings, x => x.hardware);
			_lblIP.CreateBinding(x => x.Text, _strings, x => x.ipAddress);
			_lblMac.CreateBinding(x => x.Text, _strings, x => x.macAddress);
			_lblName.CreateBinding(x => x.Text, _strings, x => x.name);
			_lblCutTime.CreateBinding(x => x.Text, _strings, x => x.curTime);
		}

		void InitControls() {
			Localization();

			_saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
			_saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
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
			_lblCutTime.BackColor = ColorDefinition.colControlBackground;
		}
		
		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			Cancel();
		}

		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			Save();
		}
    }
}
