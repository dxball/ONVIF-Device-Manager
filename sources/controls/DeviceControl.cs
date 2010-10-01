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
using nvc.entities;

namespace nvc.controls
{
    public delegate void UserEventRisedDelegate();

    public partial class DeviceControl : BaseControl
    {
        public Panel SettingsFrame{get;set;}
        DeviceModel _deviceDescr;

		public DeviceControl(DeviceModel devicedescr)
        {
            InitializeComponent();
            _deviceDescr = devicedescr;
            InitControls();
        }
		void Localisation() {
			_title.DataBindings.Add(new Binding("Text", nvc.Constants.Instance, "sDeviceControlTitle"));
		}
        protected void InitControls()
        {
			Localisation();
            BackColor = ColorDefinition.colControlBackground;
            _title.BackColor = ColorDefinition.colControlBackground;

            _imgBox.Image = _deviceDescr.GetDeviceImage();
            _imgBox.SizeMode = PictureBoxSizeMode.Zoom;

            InitForSelectionEvents();

            SetUnActiveControl();

            Width = Defaults.iDeviceControlWidth;
        }

        protected void InitForSelectionEvents()
        {
            _imgBox.MouseEnter += new EventHandler(DeviceControl_MouseEnter);
            _imgBox.MouseLeave += new EventHandler(DeviceControl_MouseLeave);
            _title.eMouseEnter += new EventHandler(DeviceControl_MouseEnter);
            _title.eMouseLeave += new EventHandler(DeviceControl_MouseLeave);
            _flowPanelLinksList.MouseEnter += new EventHandler(DeviceControl_MouseEnter);
            _flowPanelLinksList.MouseLeave += new EventHandler(DeviceControl_MouseLeave);
        }

        void InitFrame(UserControl ctrl)
        {
        }

        public void AddLinkButton(LinkCheckButton lbtn)
        {
            lbtn.eMouseEnter += new EventHandler(DeviceControl_MouseEnter);
            lbtn.eMouseLeave += new EventHandler(DeviceControl_MouseLeave);
			_lBtnsList.Add(lbtn);
            _flowPanelLinksList.Controls.Add(lbtn);
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
			_flowPanelLinksList.Controls.Clear();
		}

        #region Set color for active state
        public override void SetActiveControl()
        {
            _isActive = true;
            BackColor = ColorDefinition.colActiveControlBackground;
            _imgBox.BackColor = BackColor;
            _flowPanelLinksList.BackColor = BackColor;
            _title.BackColor = ColorDefinition.colActiveTitleBackground;
        }
        public override void SetUnActiveControl()
        {
            _isActive = false;
            _imgBox.BackColor = BackColor;
            _flowPanelLinksList.BackColor = BackColor;
            _title.BackColor = ColorDefinition.colTitleBackground;
        } 
        #endregion

        private void DeviceControl_MouseEnter(object sender, EventArgs e)
        {
            BackColor = ColorDefinition.colActiveControlBackground;
            SetActiveControl();
        }

        private void DeviceControl_MouseLeave(object sender, EventArgs e)
        {
            BackColor = ColorDefinition.colControlBackground;
            SetUnActiveControl();
        }

    }
}
