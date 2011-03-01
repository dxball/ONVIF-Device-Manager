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
using System.Windows.Shapes;

using odm.ui.controls;
using System.Diagnostics;
using odm.utils.extensions;

namespace odm {
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class About : CustomWindow {
		public About() {
			InitializeComponent();
            Localization();
		}
        CommonApplicationStrings strings = new CommonApplicationStrings();
        void Localization()
        {
            commonCaption.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutCommon);
            russCaption.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutRus);
            belarusCaption.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutBelarus);
            russCaptionPhone.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutRusPhone);
            belarusCaptionPhone.CreateBinding(TextBlock.TextProperty, strings, x => x.aboutBelarusPhone);

            Title = strings.aboutTitle;
        }
		private void HandleRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
			var hl = sender as Hyperlink;
			//if (hl == null) {
			//    return;
			//}
			string navigateUri = hl.NavigateUri.ToString();
			// if the URI somehow came from an untrusted source, make sure to
			// validate it before calling Process.Start(), e.g. check to see
			// the scheme is HTTP, etc.
			Process.Start(new ProcessStartInfo(navigateUri));
			e.Handled = true;
		}

		
	}
}
