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
    public partial class SaveCancelControl : UserControl
    {
		SaveCancelStrings _strings = new SaveCancelStrings();
        public SaveCancelControl()
        {
            InitializeComponent();

            InitControls();
        }

        public event EventHandler ButtonClickedSave;
        public event EventHandler ButtonClickedCancel;

		void Localization(){
			_btnSave.CreateBinding(x => x.Text, _strings, x => x.save);
			_btnCancel.CreateBinding(x => x.Text, _strings, x => x.cancel);
		}

        protected void InitControls()
        {
			Localization();

            BackColor = ColorDefinition.colControlBackground;
            _btnCancel.BackColor = ColorDefinition.colControlBackground;
            _btnSave.BackColor = ColorDefinition.colControlBackground;

            SetButtonsStatus();

            _btnCancel.Click += new EventHandler(_btnCancel_Click);
            _btnSave.Click += new EventHandler(_btnSave_Click);
        }

        void _btnSave_Click(object sender, EventArgs e)
        {
            if (ButtonClickedSave != null)
                ButtonClickedSave(sender, e);
        }

        void _btnCancel_Click(object sender, EventArgs e)
        {
            if (ButtonClickedCancel != null)
                ButtonClickedCancel(sender, e);            
        }

        protected void SetButtonsStatus()
        {

            _btnCancel.Enabled = false;
        }

        public void EnableCancel(bool val)
        {
            _btnCancel.Enabled = val;
        }
        public void EnableSave(bool val)
        {
            _btnSave.Enabled = val;
        }
    }
}
