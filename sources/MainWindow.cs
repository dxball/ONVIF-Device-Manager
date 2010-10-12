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

namespace nvc {

    using nvc.controls;
    public partial class MainWindow : Form
    {
		public MainWindow() 
        {
			this.DoubleBuffered = true;

			InitializeComponent();
            InitControls();

			Localization();
            
            BackColor = ColorDefinition.colControlBackground;
            _langPanel.BackColor = ColorDefinition.colControlBackground;
            _mainStatusStrip.BackColor = ColorDefinition.colControlBackground;
            _lblStatus1.BackColor = ColorDefinition.colControlBackground;
            _lblStatus2.BackColor = ColorDefinition.colControlBackground;
            _lblStatus3.BackColor = ColorDefinition.colControlBackground;
		}

		void Localization(){
			Text = Constants.Instance.sApplicationName;
		}

		EmptyNotifierControl _emptyCtrl;
        public void InitControls()
        {
            //Fills some data if no devices founded
			_emptyCtrl = new EmptyNotifierControl();
			_emptyCtrl.Dock = DockStyle.Fill;
			_splitContainerA.Panel2.Controls.Add(_emptyCtrl);

            //Colors
            Color bckColor = ColorDefinition.colMainWindowBackkground;
            _splitContainerA.BackColor = bckColor;
        }

		public void InitFrame() {
			_splitContainerA.Panel2.Controls.ForEach(x => { ((UserControl)x).Dispose(); });
			_splitContainerA.Panel2.Controls.Clear();
		}
        public void InitFrame(UserControl ctrl)
        {
			_splitContainerA.Panel2.Controls.ForEach(x => { ((UserControl)x).Dispose(); });
            _splitContainerA.Panel2.Controls.Clear();
            _splitContainerA.Panel2.Controls.Add(ctrl);
        } 
        public void InitLeftFrame(UserControl ctrl)
        {
            _splitContainerA.Panel1.Controls.Add(ctrl);
        } 
	}
}
