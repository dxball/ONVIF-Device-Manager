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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nvc.controls {
	public partial class AddDeviceForm : Form {
		public AddDeviceForm() {
			InitializeComponent();
		}

		public Uri DeviceURI { get; set; }

		private void _btnOk_Click(object sender, EventArgs e) {
			bool isValid = true;
			try {
				DeviceURI = new Uri(_tbURI.Text);
			} catch {
				isValid = false;	
			}
			if (isValid) {
				Exit(DialogResult.OK);
			} else {
				MessageBox.Show("invalid uri");
			}
		}

		private void _btnCancel_Click(object sender, EventArgs e) {
			Exit(DialogResult.Cancel);
		}
		private void Exit(DialogResult result) {
			DialogResult = result;
			this.Close();
		}
	}
}
