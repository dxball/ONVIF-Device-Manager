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
using System.IO;
using System.Diagnostics;

namespace nvc.controls {
	public partial class PropertyMaintenance : BasePropertyControl {
		public override void ReleaseUnmanaged() { }
		public Action<string> Upgrade {get; set;}
		public Action SoftReset { get; set; }

		public PropertyMaintenance(DeviceMaintenanceModel devModel) {
			InitializeComponent();
			_btnSoftReset.Enabled = false;
			BindData(devModel);
			InitControls();
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceTitle);
			_lblConfig.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceConfiguration);
			_lblDiagnostics.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceDiagnostics);
			_lblFactoryReset.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceFactoryReset);
			_lblFirmwareVer.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceFirmwareVer);
			_lblUpgrade.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceUpgrateFirmware);
			//_linkSupport.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyMaintenanceBTNSupportLink);
			_btnBackup.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceBTNBackup);
			_btnHardReset.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceBTNHardReset);
			_btnRestore.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceBTNRestore);
			_btnSoftReset.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceBTNSoftReset);
			_btnUpgrade.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceBTNUpgrate);
			_btnDiagnostics.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyMaintenanceBTNDownloadDump);
		}
		[Conditional("DEBUG")]
		void SetEnables() {
			_btnSoftReset.Enabled = true;
		}
		protected void InitControls() {
			Localization();
			SetEnables();
			_btnUpgrade.Click += new EventHandler(_btnUpgrade_Click);
			_btnSoftReset.Click += new EventHandler(_btnSoftReset_Click);

			//Color
			_title.BackColor = ColorDefinition.colTitleBackground;
			BackColor = ColorDefinition.colControlBackground;
		}

		void _btnSoftReset_Click(object sender, EventArgs e) {
			if (SoftReset != null) {
				SoftReset();
			}
		}

		void BindData(DeviceMaintenanceModel devModel) {
			_tbFirmwareVer.CreateBinding(x => x.Text, devModel, x => x.currentFirmwareVersion);
			_btnUpgrade.CreateBinding(x => x.Enabled, devModel, x => x.firmwareUpgradeSupported);
		}

		void _btnUpgrade_Click(object sender, EventArgs e) {
			var openDialog = new OpenFileDialog();
			openDialog.Filter = "Binary files (*.bin)|*.bin";
			openDialog.InitialDirectory = Directory.GetCurrentDirectory();
			var results = openDialog.ShowDialog(this);
			if (results == DialogResult.OK) {
				string path = openDialog.FileName;
				if (Upgrade != null)
					Upgrade(path);
			}
		}

	}
}
