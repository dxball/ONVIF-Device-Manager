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
    public partial class PropertyMaintenance : BasePropertyControl
    {
        public PropertyMaintenance()
        {
            InitializeComponent();

            InitControls();
        }
		void Localisation(){
			_title.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceTitle"));
			_lblConfig.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceConfiguration"));
			_lblDiagnostics.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceDiagnostics"));
			_lblFactoryReset.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceFactoryReset"));
			_lblFirmwareVer.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceFirmwareVer"));
			_lblUpgrade.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceUpgrateFirmware"));
			_linkSupport.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceBTNSupportLink"));
			_btnBackup.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceBTNBackup"));
			_btnHardReset.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceBTNHardReset"));
			_btnRestore.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceBTNRestore"));
			_btnSoftReset.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceBTNSoftReset"));
			_btnUpgrade.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceBTNUpgrate"));
			_btnDiagnostics.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyMaintenanceBTNDownloadDump"));
		}
        protected void InitControls()
        {
			Localisation();

            //[TODO] Get data from device
            //_tbFirmwareVer.Text = "00000001";
        }

    }
}
