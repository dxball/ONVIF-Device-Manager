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
using System.Linq;
using System.Text;
using nvc.controls;
using System.Windows.Forms;
using nvc.entities;
using nvc.utils;

namespace nvc.controllers {
	public class MainFrameController : IRelesable {
		//public class MyPanel : Panel {
		//    public MyPanel() {
		//        //DoubleBuffered = true;
		//        this.SetDoubleBuffered(true);
		//    }
		//}

		DeviceModel _devItem;
		MainFrame _mainFrame;
		Panel _settingsPanel;
		UserControl _tempControl;
		FlowLayoutPanel _devicePanel;
		protected DeviceModel DevItem { get { return _devItem; } set { _devItem = value; } }

		DeviceControl _devCtrl = null;
		List<DeviceChannelControl> _lstDevCannelsCtrl;

		public MainFrameController() {
			_lstDevCannelsCtrl = new List<DeviceChannelControl>();
		}

		#region Init section
		public UserControl CreateMainFrame(DeviceModel devItem) {
			DevItem = devItem;
			_mainFrame = new MainFrame();
			_mainFrame.Dock = DockStyle.Fill;

			CreatePanels();
			_mainFrame.AddDevicePanel(_devicePanel);
			_mainFrame.AddPropertyPanel(_settingsPanel);

			DevItem.ChannelsInitialised += new EventHandler(DevItem_ChannelsInitialised);
			WorkflowController.Instance.SubscribeToChannelsDescription();
			_tempControl = new LoadingPropertyPage();
			_devicePanel.Controls.Add(_tempControl);

			return _mainFrame;
		}

		void DevItem_ChannelsInitialised(object sender, EventArgs e) {
			_devicePanel.SuspendLayout();
			_settingsPanel.SuspendLayout();
			FillChannelsControl(_devicePanel, _settingsPanel);
			_devicePanel.ResumeLayout();
			_settingsPanel.ResumeLayout();
		}

		void CreateDeviceControlsLinkButtons(DeviceControl ctrl) {
			var res = from ln in WorkflowController.Instance.DictLinkButtonsSet
					  where ln.Value.ID < Defaults.constDeviceControlLinksIDBorder
					  orderby ln.Value.ID
					  select ln;
			foreach (var val in res) {
				LinkCheckButton lbtn = WorkflowController.Instance.CreateLinkCheckButton(val.Key, ctrl.SettingsFrame);
				lbtn.linkClicked += new EventHandler(lbtn_Click);
				ctrl.AddLinkButton(lbtn);
			}
		}
		protected DeviceControl CreateDeviceControl(Panel settingsFrame) {
			var ctrl = new DeviceControl(DevItem);
			_devCtrl = ctrl;
			ctrl.SettingsFrame = settingsFrame;

			CreateDeviceControlsLinkButtons(ctrl);
			return ctrl;
		}
		void CreateChannelControlLinkButtons(DeviceChannelControl ctrl) {
			var res = from ln in WorkflowController.Instance.DictLinkButtonsSet
					  where ln.Value.ID < Defaults.constDeviceChannelLinksIDBorder && ln.Value.ID > Defaults.constDeviceControlLinksIDBorder
					  orderby ln.Value.ID
					  select ln;
			foreach (var val in res) {
				if (DevItem.DeviceCapabilities.Contains(val.Value.ID)) {
					LinkCheckButton lbtn = WorkflowController.Instance.CreateLinkCheckButton(val.Key, ctrl.SettingsFrame);
					ctrl.AddLinkButton(lbtn);
				}
			}
		}
		protected DeviceChannelControl CreateChannelControl(Panel settingsFrame, DeviceChannel channel) {
			var devchannelControl = new DeviceChannelControl(channel);
			devchannelControl.SettingsFrame = settingsFrame;

			devchannelControl.ChannelSelected += new channelEventHandlerDelegate(devchannelControl_ChannelSelected);

			_lstDevCannelsCtrl.Add(devchannelControl);

			CreateChannelControlLinkButtons(devchannelControl);
			return devchannelControl;
		}

		protected void FillChannelsControl(Control channelscontainer, Panel settingFrame) {
			if (_tempControl != null) {
				channelscontainer.Controls.Remove(_tempControl);
				_tempControl.Dispose();
				_tempControl = null;
			}
			foreach (var chan in WorkflowController.Instance.GetCurrentDevice().ChannelsList) {
				var devChCtrl = CreateChannelControl(settingFrame, chan.Value);
				channelscontainer.Controls.Add(devChCtrl);
			}
		}
		private void CreatePanels() {
			CreatePropertyPanel();
			_devicePanel = new FlowLayoutPanel();
			_devicePanel.Dock = DockStyle.Fill;

			_settingsPanel.SuspendLayout();

			var ctrl = CreateDeviceControl(_settingsPanel);
			_devicePanel.Controls.Add(ctrl);

			_settingsPanel.ResumeLayout();
		}
		private void CreatePropertyPanel() {
			_settingsPanel = new Panel();
			_settingsPanel.Dock = DockStyle.Fill;
			_settingsPanel.SetDoubleBuffered(true);
		}
		#endregion

		#region EventsHandlers
		void devchannelControl_ChannelSelected(DeviceChannel devChannel, LinkButtonSetting settings) {
			DevItem.SetCurrentChannel(devChannel.GetChannelID());
			if (settings != null) {
				IPropertyController propCtrl = settings.func();
				DisposeChilds(_settingsPanel);
				
				//WorkflowController.Instance.SubscribeToDefaultProfile();
				_settingsPanel.Controls.Add(propCtrl.CreateController(_settingsPanel));
			}
		}
		void DisposeChilds(Panel control) {
			if (control != null)
				foreach (UserControl value in control.Controls) {
					value.Dispose();
				}
		}
		void lbtn_Click(object sender, EventArgs e) {
			LinkButtonSetting lbtnSet = e as LinkButtonSetting;

			IPropertyController propCtrl = lbtnSet.func();

			_settingsPanel.SuspendLayout();

			DisposeChilds(_settingsPanel);
			_settingsPanel.Controls.Add(propCtrl.CreateController(_settingsPanel));
			
			_settingsPanel.ResumeLayout();
		}
		#endregion

		#region release resources
		public void ReleaseAll() {
			DevItem.ChannelsInitialised -= DevItem_ChannelsInitialised;
			_devCtrl.UnsubscribeLinkButton(UnsubscribeLinkButton);
			foreach (var value in _lstDevCannelsCtrl) {
				value.UnsubscribeLinkButton(UnsubscribeLinkButton);
			}
			UnsubscribeDeviceChannel();
			DisposeChilds(_settingsPanel);
			_mainFrame.Dispose();
		}
		protected void UnsubscribeLinkButton(LinkCheckButton lbtn) {
			lbtn.linkClicked -= lbtn_Click;
		}
		protected void UnsubscribeDeviceChannel() {
			foreach (var value in _lstDevCannelsCtrl) {
				value.ChannelSelected -= devchannelControl_ChannelSelected;
			}
		}
		#endregion
	}
}
