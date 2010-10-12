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
using nvc.controllers;
using nvc.onvif;
using nvc.models;

namespace nvc.controls
{
    public partial class LinkCheckButton : UserControl
    {
        public event EventHandler linkClicked;
        public event EventHandler eMouseEnter;
        public event EventHandler eMouseLeave;

        protected bool _isClicked = false;
        protected Panel _propertyContainer;

		public bool IsChecked { get { return _checkBox.Checked; } }
		public Func<IPropertyController> CreatePropertyAction { get; set; }
		public Action ReleasePropertyAction { get; set; }
		public Action<LinkCheckButton> Click { get; set; }
		public ChannelDescription Channel {
			get;
			set;
		}
		public Session ModelSession { get; set; }
		public Label NameLable {
			get { return _linkLabel; }
		}
        public LinkCheckButton(bool isCheckable, Panel propContainer)//LinkButtonSetting lbtnSet)
        {
            InitializeComponent();
            
            _checkBox.Checked = true;

            _linkLabel.Click += new EventHandler(_linkLabel_Click);

			_checkBox.Visible = isCheckable;//lbtnSet.IsCheckable;
			

            MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            _linkLabel.MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            _checkBox.MouseEnter += new EventHandler(LinkCheckButton_MouseEnter);
            MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);
            _linkLabel.MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);
            _checkBox.MouseLeave += new EventHandler(LinkCheckButton_MouseLeave);
			_propertyContainer = propContainer;

            _linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }
		
        void _linkLabel_Click(object sender, EventArgs e)
        {
			if(!_isClicked)
				Click(this);
        }

        void LinkCheckButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
				if (!_isClicked) {
					_linkLabel.ForeColor = ColorDefinition.colLinkButtonsIitial;
					_linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
				}
            }
            if (eMouseLeave != null)
                eMouseLeave(sender, e);
        }

        void LinkCheckButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
				if (!_isClicked) {
					_linkLabel.ForeColor = ColorDefinition.colLinkButtonsHovered;
					_linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
				}
            }
            if (eMouseEnter != null)
                eMouseEnter(sender, e);
        }

        public LinkCheckButton()
        {
            InitializeComponent();
        }

		public void SetUnclicked() {
			_isClicked = false;
			_linkLabel.ForeColor = ColorDefinition.colLinkButtonsIitial;
			_linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
		}
		public void SetClicked() {
			_isClicked = true;
			_linkLabel.ForeColor = ColorDefinition.colLinkButtonsClicked;
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
