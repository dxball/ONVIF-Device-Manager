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

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for DirectionRose.xaml
	/// </summary>
	public partial class DirectionRose : UserControl {
		public DirectionRose() {
			InitializeComponent();
			InitControls();
		}
		public Action SelectionChanged;

		void InitControls() {
			bE.Click += Click;
			bN.Click += Click;
			bNE.Click += Click;
			bNW.Click += Click;
			bS.Click += Click;
			bSE.Click += Click;
			bSW.Click += Click;
			bW.Click += Click;

			btnAll.Click += new RoutedEventHandler(btnAll_Click);
			btnNone.Click += new RoutedEventHandler(btnNone_Click);
		}

		void btnNone_Click(object sender, RoutedEventArgs e) {
			bE.IsChecked = false;
			bN.IsChecked = false;
			bNE.IsChecked = false;
			bNW.IsChecked = false;
			bS.IsChecked = false;
			bSE.IsChecked = false;
			bSW.IsChecked = false;
			bW.IsChecked = false;	
		}

		void btnAll_Click(object sender, RoutedEventArgs e) {
			bE.IsChecked = true;
			bN.IsChecked = true;
			bNE.IsChecked = true;
			bNW.IsChecked = true;
			bS.IsChecked = true;
			bSE.IsChecked = true;
			bSW.IsChecked = true;
			bW.IsChecked = true;			
		}

		void Click(object sender, RoutedEventArgs e) {
			if (SelectionChanged != null)
				SelectionChanged();
		}
		public bool IfbE { get { return bE.IsChecked.Value; } set { bE.IsChecked = value; } }
		public bool IfbN { get { return bN.IsChecked.Value; } set { bN.IsChecked = value; } }
		public bool IfbNE { get { return bNE.IsChecked.Value; } set { bNE.IsChecked = value; } }
		public bool IfbNW { get { return bNW.IsChecked.Value; } set { bNW.IsChecked = value; } }
		public bool IfbS { get { return bS.IsChecked.Value; } set { bS.IsChecked = value; } }
		public bool IfbSE { get { return bSE.IsChecked.Value; } set { bSE.IsChecked = value; } }
		public bool IfbSW { get { return bSW.IsChecked.Value; } set { bSW.IsChecked = value; } }
		public bool IfbW { get { return bW.IsChecked.Value; } set { bW.IsChecked = value; } }
	}
}
