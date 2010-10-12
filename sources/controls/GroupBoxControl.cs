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

namespace nvc.controls
{
    public partial class GroupBoxControl : UserControl
    {
        public event EventHandler eMouseEnter;
        public event EventHandler eMouseLeave;
        public GroupBoxControl()
        {
            InitializeComponent();

            _lblTitle.MouseEnter += new EventHandler(_panelTitle_MouseEnter);
            _lblTitle.MouseLeave += new EventHandler(_panelTitle_MouseLeave);
            _panelTitle.MouseEnter += new EventHandler(_panelTitle_MouseEnter);
            _panelTitle.MouseLeave += new EventHandler(_panelTitle_MouseLeave);
        }

        void _panelTitle_MouseLeave(object sender, EventArgs e)
        {
            if (eMouseLeave != null)
                eMouseLeave(sender, e);
        }

        void _panelTitle_MouseEnter(object sender, EventArgs e)
        {
            if (eMouseEnter != null)
                eMouseEnter(sender, e);            
        }
		public override string Text {
			get {
				return _lblTitle.Text;
			}
			set {
				_lblTitle.Text = value;
			}
		}
		//string Text { set { 
		//    _lblTitle.Text = value; } 
		//}
    }
}
