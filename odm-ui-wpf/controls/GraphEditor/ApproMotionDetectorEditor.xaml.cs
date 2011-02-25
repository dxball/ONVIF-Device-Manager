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

namespace odm.ui.controls.GraphEditor {
	/// <summary>
	/// Interaction logic for ApproMotionDetectorEditor.xaml
	/// </summary>
	public partial class ApproMotionDetectorEditor : UserControl {
		public ApproMotionDetectorEditor() {
			InitializeComponent();
		}

		public int MaskedValue { 
			get {
				return 0;
			}
			set {
				p001.IsChecked = ((value & 0x1) == 0x1);
				p010.IsChecked = ((value & 0x10) == 0x10);
				p100.IsChecked = ((value & 0x100) == 0x100);

				p002.IsChecked = ((value & 0x2) == 0x2);
				p020.IsChecked = ((value & 0x20) == 0x20);
				p200.IsChecked = ((value & 0x200) == 0x200);

				p004.IsChecked = ((value & 0x4) == 0x4);
				p040.IsChecked = ((value & 0x40) == 0x40);
				p400.IsChecked = ((value & 0x400) == 0x400);

				p008.IsChecked = ((value & 0x8) == 0x8);
				p080.IsChecked = ((value & 0x80) == 0x80);
				p800.IsChecked = ((value & 0x800) == 0x800);

			}
		}
	}
}
