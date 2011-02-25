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
using odm.models;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for DeviceControl.xaml
	/// </summary>
	public partial class DeviceControl : UserControl {
		public DeviceControl(DeviceCapabilityModel devModel, ImageSource devImg) {
			InitializeComponent();

			SetControlName(devModel.devInfo.Name);
			SetControlImage(devImg);
		}

		List<LinkCheckButton> _lBtnsList = new List<LinkCheckButton>();

		public void SetControlImage(ImageSource devImg) {
			_deviceImage.Source = devImg;
		}
		public void SetControlName(string name) {
			title.Title = name;
		}

		public void AddLinkButton(LinkCheckButton lbtn) {
			//lbtn.eMouseEnter += new EventHandler(DeviceControl_MouseEnter);
			//lbtn.eMouseLeave += new EventHandler(DeviceControl_MouseLeave);
			_lBtnsList.Add(lbtn);

			_deviceLinksPanel.Children.Add(lbtn);
		}
		public void ResetLinkSelection() {
			_lBtnsList.ForEach(x => { ((LinkCheckButton)x).SetUnclicked(); });
		}
	}
}
