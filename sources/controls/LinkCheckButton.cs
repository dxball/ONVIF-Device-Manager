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
//
//----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nvc.controllers;

namespace nvc.controls
{
    public partial class LinkCheckButton : UserControl
    {
        public event EventHandler linkClicked;
        public event EventHandler eMouseEnter;
        public event EventHandler eMouseLeave;

        protected bool _isClicked = false;
        protected Panel _propertyContainer;
        //protected Func<BasePropertyControl> _func;
        protected Func<IPropertyController> _func;
        LinkButtonSetting _lbtnSet;

        public LinkCheckButton(LinkButtonSetting lbtnSet)
        {
            InitializeComponent();
            _lbtnSet = lbtnSet;

            _checkBox.Checked = true;

            _linkLabel.Click += new EventHandler(_linkLabel_Click);

            _checkBox.Visible = lbtnSet.IsCheckable;
			_linkLabel.DataBindings.Add(new Binding("Text", Constants.Instance , lbtnSet.Name));

            MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            _linkLabel.MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            _checkBox.MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);
            _linkLabel.MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);
            _checkBox.MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);

            _func = lbtnSet.func;

            _propertyContainer = lbtnSet.PropertyFrame;

            _linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }
		
        void _linkLabel_Click(object sender, EventArgs e)
        {
            Clicked(sender, e);
        }

        void LinkCheckButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                _linkLabel.ForeColor = ColorDefinition.colLinkButtonsIitial;
                _linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
            if (eMouseLeave != null)
                eMouseLeave(sender, e);
        }

        void LinkCheckButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                _linkLabel.ForeColor = ColorDefinition.colLinkButtonsHovered;
                _linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
            if (eMouseEnter != null)
                eMouseEnter(sender, e);
        }

        public string LinkText { set { _linkLabel.Text = value; } }
        public LinkCheckButton()
        {
            InitializeComponent();
        }

        public bool IsChecked{ get { return _checkBox.Checked; } }
        protected void Clicked(object sender, EventArgs e)
        {
            _isClicked = true;

            //Rise enet
            if (linkClicked != null)
                linkClicked(sender, _lbtnSet);

            ////Resreshing panel (removing recent controls) and add new one
            //foreach (var ctrl in _propertyContainer.Controls)
            //    ((BasePropertyControl)ctrl).Dispose();
            //_propertyContainer.Controls.Clear();
            //_propertyContainer.Controls.Add(_func());
            //foreach (var ctrl in _propertyContainer.Controls)
            //    ((BasePropertyControl)ctrl).Dock = DockStyle.Fill;
        }
        public void ResetLink()
        {
            _isClicked = false;
            _linkLabel.Enabled = true;
        }          

        private void _checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if(!IsChecked)
                _linkLabel.Enabled = false;
            else
                _linkLabel.Enabled = true;
        }

        private void LinkCheckButton_Load(object sender, EventArgs e)
        {
        }
    }
}
