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

namespace odm.controls {

	/// <summary>
	/// Interaction logic for PropertyMaintenance.xaml
	/// </summary>
	public partial class PropertyMaintenance : BasePropertyControl {
		public override void ReleaseUnmanaged() {
		}
		public Action<string> Upgrade;
		public Action SoftReset;

		PropertyMaintenanceStrings _strings = new PropertyMaintenanceStrings();
		DeviceMaintenanceModel _devModel;

		public PropertyMaintenance(DeviceMaintenanceModel devModel) {
			InitializeComponent();
			_devModel = devModel;
			Loaded += PropertyMaintenance_Load;
		}

		void PropertyMaintenance_Load(object sender, EventArgs e) {
			_btnSoftReset.IsEnabled= false;
			BindData(_devModel);
			InitControls();
		}

		void Localization() {
			title.CreateBinding(Title.ContentProperty, _strings, x => x.title);

			configCaption.CreateBinding(Label.ContentProperty, _strings, x => x.configuration);
			diagnosticsCaption.CreateBinding(Label.ContentProperty, _strings, x => x.diagnostics);
			factoryResetCaption.CreateBinding(Label.ContentProperty, _strings, x => x.factoryReset);
			firmwareVersionCaption.CreateBinding(Label.ContentProperty, _strings, x => x.firmwareVer);
			upgradeCaption.CreateBinding(Label.ContentProperty, _strings, x => x.upgrateFirmware);
			_btnBackup.CreateBinding(Button.ContentProperty, _strings, x => x.btnBackup);
			_btnHardReset.CreateBinding(Button.ContentProperty, _strings, x => x.btnHardReset);
			_btnRestore.CreateBinding(Button.ContentProperty, _strings, x => x.btnRestore);
			_btnSoftReset.CreateBinding(Button.ContentProperty, _strings, x => x.btnSoftReset);
			_btnUpgrade.CreateBinding(Button.ContentProperty, _strings, x => x.btnUpgrate);
			_btnDiagnostics.CreateBinding(Button.ContentProperty, _strings, x => x.diagnostics);
		}

		[Conditional("DEBUG")]
		void SetEnables() {
			_btnSoftReset.IsEnabled = true;
		}

		protected void InitControls() {
			Localization();
			SetEnables();
			_btnUpgrade.Click += _btnUpgrade_Click;
			_btnSoftReset.Click += _btnSoftReset_Click;
				
			//Color
		}

		void _btnSoftReset_Click(object sender, EventArgs e) {
			if (SoftReset != null) {
				SoftReset();
			}
		}

		void BindData(DeviceMaintenanceModel devModel) {
			try {
				_lblFirmwareVer.CreateBinding(Label.ContentProperty, devModel, x => x.currentFirmwareVersion);
			} catch (Exception err) {
				string strValue;
				if (devModel.currentFirmwareVersion == null)
					strValue = "Null";
				else
					strValue = devModel.currentFirmwareVersion;
				BindingError(err, ExceptionStrings.Instance.errBindCurrentFirmwareVersion + strValue);
			}
			try {
				_btnUpgrade.CreateBinding(Button.IsEnabledProperty, devModel, x => x.firmwareUpgradeSupported);
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

	}
}
