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

namespace nvc.controls
{
    public partial class EmptyNotifierControl : UserControl
    {
		EmptySelectionStrings _strings = new EmptySelectionStrings();
        public EmptyNotifierControl()
        {
            InitializeComponent();
            InitControls();
        }

		void Localization(){
			_tbContacts.CreateBinding(x => x.Text, _strings, x => x.textContacts);
			_tbNitifier.CreateBinding(x => x.Text, _strings, x => x.text);
			_textBoxTitle.CreateBinding(x => x.Text, _strings, x => x.title);
		}
        protected void InitControls()
        {
            BackColor = ColorDefinition.colControlBackground;
            _tbContacts.BackColor = ColorDefinition.colControlBackground;
            _tbNitifier.BackColor = ColorDefinition.colControlBackground;
            _textBoxTitle.BackColor = ColorDefinition.colControlBackground;

            Localization();

            string currentDir = Directory.GetCurrentDirectory();
			//if(File.Exists(Defaults.sNotifierImg))
			//    _imgLogo.Image = Image.FromFile(Defaults.sNotifierImg);
        }
    }
}
