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
using nvc.entities;

namespace nvc.controls
{
    public delegate void channelEventHandlerDelegate(DeviceChannel devChannel, LinkButtonSetting settings);
    public partial class DeviceChannelControl : BaseControl
    {
        public Panel SettingsFrame { get; set; }

		DeviceChannel _devChannel;
        public event channelEventHandlerDelegate ChannelSelected;

		public DeviceChannelControl(DeviceChannel devChannel)
        {
            InitializeComponent();

            _devChannel = devChannel;

            InitControls();
            InitForModelEvents();
        }

        void InitForModelEvents()
        {
            //subscribe for change last event
        }

        public void InitControls()
        {
            _title.Text = _devChannel.GetChannelName();

            //_imgBox.Image = _devModel.GetChannelsLastEvent(_chanelName).Thumbnail;
            //_imgBox.SizeMode = PictureBoxSizeMode.Zoom;

			_imgBox.BackColor = ColorDefinition.colControlBackground;
			BackColor = ColorDefinition.colActiveControlBackground;

			SetUnActiveColors();

            InitForSelectionEvents();
        }

        protected void InitForSelectionEvents()
        {
            _imgBox.MouseEnter += new EventHandler(DeviceControl_MouseEnter);
            _imgBox.MouseLeave += new EventHandler(DeviceControl_MouseLeave);
            _title.eMouseEnter += new EventHandler(DeviceControl_MouseEnter);
            _title.eMouseLeave += new EventHandler(DeviceControl_MouseLeave);
            this.MouseEnter += new EventHandler(DeviceControl_MouseEnter);
            this.MouseLeave +=new EventHandler(DeviceControl_MouseLeave);
            _flowLayoutPanel.MouseEnter += new EventHandler(DeviceControl_MouseEnter);
            _flowLayoutPanel.MouseLeave += new EventHandler(DeviceControl_MouseLeave);

            //User select channel
            _title.MouseClick += new MouseEventHandler(DeviceChannelControl_MouseClick);
            MouseClick += new MouseEventHandler(DeviceChannelControl_MouseClick);
        }

        void DeviceChannelControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (ChannelSelected != null)
            {
                ChannelSelected(_devChannel, null);
            }
        }

        #region Set color for active state
        private void DeviceControl_MouseEnter(object sender, EventArgs e){
			SetActiveColors();
        }

		void SetActiveColors() {
			BackColor = ColorDefinition.colActiveControlBackground;
			_imgBox.BackColor = BackColor;
			_flowLayoutPanel.BackColor = BackColor;
			_title.BackColor = ColorDefinition.colActiveTitleBackground;
		}
		void SetUnActiveColors() {
			BackColor = ColorDefinition.colControlBackground;
			_imgBox.BackColor = BackColor;
			_flowLayoutPanel.BackColor = BackColor;
			_title.BackColor = ColorDefinition.colTitleBackground;
		}

        private void DeviceControl_MouseLeave(object sender, EventArgs e){
			SetUnActiveColors();
        }
        public override void SetActiveControl()
        {
            _isActive = true;
            _currentColor = BackColor;
            BackColor = Color.GhostWhite;
        }
        public override void SetUnActiveControl()
        {
            _isActive = false;
            BackColor = _currentColor;
        }
        #endregion

        public void AddLinkButton(LinkCheckButton lbtn)
        {
            lbtn.eMouseEnter += new EventHandler(DeviceControl_MouseEnter);
            lbtn.eMouseLeave += new EventHandler(DeviceControl_MouseLeave);

            lbtn.linkClicked += new EventHandler(lbtn_linkClicked);
			_lBtnsList.Add(lbtn);
            _flowLayoutPanel.Controls.Add(lbtn);
        }
		List<LinkCheckButton> _lBtnsList = new List<LinkCheckButton>();
		public void UnsubscribeLinkButton(Action<LinkCheckButton> func) {
			foreach (var value in _lBtnsList) {
				func(value);
			}
		}
		public void RemoveLinkButtons() {
			foreach (var value in _lBtnsList) {
				value.Dispose();
			}
			_lBtnsList.Clear();
			
			_flowLayoutPanel.Controls.Clear();
		}

        void lbtn_linkClicked(object sender, EventArgs e)
        {
            if (ChannelSelected != null)
            {
                ChannelSelected(_devChannel, e as LinkButtonSetting);
            }
        }
    }

    
}
