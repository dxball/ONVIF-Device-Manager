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
using System.ComponentModel;

using odm.models;
using odm.utils.extensions;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyDeviceIdentificationAndStatus.xaml
	/// </summary>
	public partial class IdentificationAndStatus : BasePropertyControl {
		public IdentificationAndStatus(DeviceIdentificationModel devModel) {
			InitializeComponent();
			model = devModel;

			InitControls();
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
		LinkButtonsStrings titles = new LinkButtonsStrings();

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
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.identificationAndStatus);
			
			nameCaption.CreateBinding(Label.ContentProperty, strings, x => x.name);
			nameEditor.CreateBinding(TextBox.TextProperty, model, m=>m.Name, (m, v)=>{ m.Name = v;});

			locationCaption.CreateBinding(Label.ContentProperty, strings, x => x.location);
			locationEditor.CreateBinding(TextBox.TextProperty, model, m=>m.location, (m, v)=>{ m.location = v;});

			lbDevID.CreateBinding(Label.ContentProperty, strings, x => x.deviceID);
			tbDevID.CreateBinding(TextBox.TextProperty, model, m => m.DeviceID);

			lbFirmware.CreateBinding(Label.ContentProperty, strings, x => x.firmware);
			tbFirmware.CreateBinding(TextBox.TextProperty, model, m => m.FirmwareVer);

			lbHardware.CreateBinding(Label.ContentProperty, strings, x => x.hardware);
			tbHardware.CreateBinding(TextBox.TextProperty, model, m => m.HardwareVer);

			lbIPAddr.CreateBinding(Label.ContentProperty, strings, x => x.ipAddress);
			tbIPAddr.CreateBinding(TextBox.TextProperty, model, m => m.NetworkIPAddress);

			lbMACAddr.CreateBinding(Label.ContentProperty, strings, x => x.macAddress);
			tbMACAddr.CreateBinding(TextBox.TextProperty, model, m => m.MACAddress);
			
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
