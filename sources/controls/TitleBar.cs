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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using nvc.localization;
using nvc.controls.utils;

namespace nvc.controls {
	public partial class TitleBar : UserControl {
		public TitleBar() {
			InitializeComponent();
			InitControls();
		}

		//public event EventHandler LocalizationSelected;

		void InitControls() {
			var list = Language.AvailableLanguages.Select(x => ListItem.Wrap(x, y => y.DisplayName)).Where(u => u.Unwrap().iso3 != null).ToArray();

			var defItem = list.Where(x => x.Unwrap().iso3 == Language.Default.iso3).FirstOrDefault();

			_cmbLocale.Items.AddRange(list);
			if (defItem == null) {
				defItem = ListItem.Wrap(Language.Default, x => "english");
				_cmbLocale.Items.Add(defItem);
			}
			_cmbLocale.SelectedItem = defItem;
		}

		private void _pbAbout_Click(object sender, EventArgs e) {
			AboutBox about = new AboutBox();
			about.ShowDialog(this);
		}

		private void _cmbLocale_SelectedIndexChanged(object sender, EventArgs e) {
			var selection = (ListItem<Language>)_cmbLocale.SelectedItem;
			if (selection == null) {
				Language.Current = null;
			} else {
				Language.Current = selection.Unwrap();
			}
		}
	}
}