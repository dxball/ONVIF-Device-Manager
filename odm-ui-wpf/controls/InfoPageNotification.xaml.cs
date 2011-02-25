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
	/// Interaction logic for InfoPageNotification.xaml
	/// </summary>
	public partial class InfoPageNotification : BasePropertyControl {
		public InfoPageNotification() {
			InitializeComponent();

			details.Click += new RoutedEventHandler(details_Click);
			_btnClose.Click += new RoutedEventHandler(_btnClose_Click);
		}

		void _btnClose_Click(object sender, RoutedEventArgs e) {
			if (_onClickAction != null) {
				_onClickAction();
			}
		}

		void details_Click(object sender, RoutedEventArgs e) {
			if(detailsView.Visibility == System.Windows.Visibility.Hidden)
				detailsView.Visibility = System.Windows.Visibility.Visible;
			else
				detailsView.Visibility = System.Windows.Visibility.Hidden;
		}
		public enum InfoType {
			ErroreDtails,
			Other
		};
		public InfoType infotype = InfoType.Other;
		
		Action _onClickAction;
		public Action OnClickAction {
			get {
				return _onClickAction;
			}
			set {
				IsButtonVisible = true;
				_progressBar.Visibility = System.Windows.Visibility.Hidden;
				_onClickAction = value;
			}
		}
		public string Title {
			set {
				column.Title = value;
			}
		}
		public bool IsDetails {
			set {
				details.Visibility = System.Windows.Visibility.Visible;
			}
		}
		public bool IsProgressLoadingVisible {
			set {
				if (value)
					_progressBar.Visibility = System.Windows.Visibility.Visible;
				else
					_progressBar.Visibility = System.Windows.Visibility.Hidden;
			}
		}
		public bool IsButtonVisible {
			set {
				if(value)
					_btnClose.Visibility = System.Windows.Visibility.Visible;
				else
					_btnClose.Visibility = System.Windows.Visibility.Hidden;
			}
		}
		
		public string Message {
			get {
				return "info";//_infoTextBox.Text;
			}
			set {
				lText.Text = value; 
				//temp = value;
			}
		}
	}
}
