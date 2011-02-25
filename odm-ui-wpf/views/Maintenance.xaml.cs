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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

using odm.models;
using odm.utils.extensions;
using onvif.types;
using onvif.services.device;
using odm.utils;
using System.Threading;

namespace odm.ui.controls {

	/// <summary>
	/// Interaction logic for PropertyMaintenance.xaml
	/// </summary>
	public partial class Maintenance : BasePropertyControl {
		public override void ReleaseUnmanaged() {
		}
		public Action<string> Upgrade;
		public Action SoftReset;
		public Action<string> Backup;
		public Action<string> Restore;

		PropertyMaintenanceStrings strings = new PropertyMaintenanceStrings();
		LinkButtonsStrings titles = new LinkButtonsStrings();
		MaintenanceModel model;

		public Maintenance(MaintenanceModel devModel) {
			InitializeComponent();
			this.model = devModel;
			Loaded += PropertyMaintenance_Load;
		}

		void PropertyMaintenance_Load(object sender, EventArgs e) {
			softResetBtn.IsEnabled= false;
			BindData(model);
			InitControls();
		}

		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.maintenance);

			configCaption.CreateBinding(Label.ContentProperty, strings, x => x.configuration);
			diagnosticsCaption.CreateBinding(Label.ContentProperty, strings, x => x.diagnostics);
			factoryResetCaption.CreateBinding(Label.ContentProperty, strings, x => x.factoryReset);
			firmwareVersionCaption.CreateBinding(Label.ContentProperty, strings, x => x.firmwareVer);
			//upgradeCaption.CreateBinding(Label.ContentProperty, strings, x => x.upgrateFirmware);
			backupBtn.CreateBinding(Button.ContentProperty, strings, x => x.btnBackup);
			hardResetBtn.CreateBinding(Button.ContentProperty, strings, x => x.btnHardReset);
			restoreBtn.CreateBinding(Button.ContentProperty, strings, x => x.btnRestore);
			softResetBtn.CreateBinding(Button.ContentProperty, strings, x => x.btnSoftReset);
			upgradeBtn.CreateBinding(Button.ContentProperty, strings, x => x.btnUpgrate);
			diagnosticsBtn.CreateBinding(Button.ContentProperty, strings, x => x.diagnostics);
		}

		[Conditional("DEBUG")]
		void SetEnables() {
			softResetBtn.IsEnabled = true;
		}

		protected void InitControls() {
			Localization();
			SetEnables();
			upgradeBtn.Click += _btnUpgrade_Click;
			softResetBtn.Click += _btnSoftReset_Click;
			backupBtn.Click += new RoutedEventHandler(backupBtn_Click);
			restoreBtn.Click += new RoutedEventHandler(restoreBtn_Click);
			//Color
		}

		void restoreBtn_Click(object sender, RoutedEventArgs e) {
			var dlg = new OpenFileDialog();
			dlg.Filter = "Backup files|*.backup";
            dlg.Title = "Open backup file";
            if (dlg.ShowDialog() == true)// && dlg.CheckFileExists)
            {
                var ret = dlg.FileName;
                if (Restore != null)
                    Restore(ret);
            }
		}

		void backupBtn_Click(object sender, RoutedEventArgs e) {
			var dlg = new SaveFileDialog();
			dlg.Filter = "Backup files|*.backup";
            if (dlg.ShowDialog() == true)
            {
                var ret = dlg.FileName;

                if (Backup != null)
                    Backup(ret);
            }
		}

		void _btnSoftReset_Click(object sender, EventArgs e) {
			if (SoftReset != null) {
				SoftReset();
			}
		}

		void BindData(MaintenanceModel devModel) {
			try {
				firmwareTxt.CreateBinding(TextBox.TextProperty, devModel, x => x.currentFirmwareVersion);
			} catch (Exception err) {
				string strValue;
				if (devModel.currentFirmwareVersion == null)
					strValue = "Null";
				else
					strValue = devModel.currentFirmwareVersion;
				BindingError(err, ExceptionStrings.Instance.errBindCurrentFirmwareVersion + strValue);
			}
			try {
				upgradeBtn.CreateBinding(Button.IsEnabledProperty, devModel, x => x.firmwareUpgradeSupported);
			} catch (Exception err) {
				BindingError(err, ExceptionStrings.Instance.errBindFirmwareUpgradeSupported);
			}
		}

		void _btnUpgrade_Click(object sender, EventArgs e) {
			// Create OpenFileDialog
			var openDialog = new OpenFileDialog();
			// Set filter for file extension and default file extension
			openDialog.Filter = "Binary files (*.bin)|*.bin";
			openDialog.InitialDirectory = Directory.GetCurrentDirectory();
			
			// Display OpenFileDialog by calling ShowDialog method
			var result = openDialog.ShowDialog();
			// Get the selected file name and start upgrate firmware
			if (result == true) {
				string path = openDialog.FileName;
				if (Upgrade != null)
					Upgrade(path);
			}
		}

		private void _btnBackup_Click(object sender, RoutedEventArgs e) {
			var dev = model.session
				.GetDeviceClient()
				.First()
				.Services;
			BackupFile[] backups = null;
			var wh = new ManualResetEvent(false);
			dev.BeginGetSystemBackup(new MsgGetSystemBackupRequest(), ar => {
				try {
					backups = dev.EndGetSystemBackup(ar).parameters.BackupFiles;
				} catch (Exception err) {
					dbg.Error(err);
				}
				wh.Set();
			}, null);
			wh.WaitOne();
			dbg.Break();
		}

	}
}
