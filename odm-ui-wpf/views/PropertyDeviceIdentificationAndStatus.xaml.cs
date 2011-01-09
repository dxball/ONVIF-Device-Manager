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
using System.ComponentModel;

using odm.models;
using odm.utils.extensions;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyDeviceIdentificationAndStatus.xaml
	/// </summary>
	public partial class PropertyDeviceIdentificationAndStatus : BasePropertyControl {
		public PropertyDeviceIdentificationAndStatus(DeviceIdentificationModel devModel) {
			InitializeComponent();
			model = devModel;

			InitControls();
			Localization();
			BindData();
		}
		DeviceIdentificationModel model;
		public Action Save { get; set; }
		public Action Cancel { get; set; }
		public PropertyIdentificationStrings strings {
			get {
				return PropertyIdentificationStrings.Instance;
			}
		}

		void Localization() {
			title.CreateBinding(Title.ContentProperty, strings, x => x.title);
			lbDevID.CreateBinding(Label.ContentProperty, strings, x => x.deviceID);
			lbFirmware.CreateBinding(Label.ContentProperty, strings, x => x.firmware);
			lbHardware.CreateBinding(Label.ContentProperty, strings, x => x.hardware);
			lbIPAddr.CreateBinding(Label.ContentProperty, strings, x => x.ipAddress);
			lbMACAddr.CreateBinding(Label.ContentProperty, strings, x => x.macAddress);
			lbName.CreateBinding(Label.ContentProperty, strings, x => x.name);
		}

		void InitControls() {
			tbDevID.IsReadOnly = true;
			tbFirmware.IsReadOnly = true;
			tbHardware.IsReadOnly = true;
			tbIPAddr.IsReadOnly = true;
			tbMACAddr.IsReadOnly = true;

			saveCancel.Save.Click += Save_Click;
			saveCancel.Cancel.Click += Cancel_Click;
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			if (Cancel != null) {
				Cancel();
			}
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			if (Save != null) {
				Save();
			}
		}

		void BindData() {
			tbDevID.CreateBinding(TextBox.TextProperty, model, m=>m.DeviceID);
			tbFirmware.CreateBinding(TextBox.TextProperty, model, m=>m.FirmwareVer);
			tbHardware.CreateBinding(TextBox.TextProperty, model, m=>m.HardwareVer);
			tbIPAddr.CreateBinding(TextBox.TextProperty, model, m=>m.NetworkIPAddress);
			tbMACAddr.CreateBinding(TextBox.TextProperty, model, m=>m.MACAddress);
			tbName.CreateBinding(TextBox.TextProperty, model, m=>m.Name, (m, v)=>{ m.Name = v;});

			//tbName
			//    .GetPropertyChangedEvents(TextBox.TextProperty)
			//    .Subscribe(v=>{
			//        MessageBox.Show(v.ToString());
			//    });

			saveCancel.Cancel.CreateBinding(Button.IsEnabledProperty, model, m => m.isModified);
			saveCancel.Save.CreateBinding(Button.IsEnabledProperty, model, m => m.isModified);
		}

	}
}
