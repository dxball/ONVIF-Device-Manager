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
using System.Linq;
using System.Text;
using nvc.controls;
using System.Windows.Forms;
using nvc.entities;
using nvc.utils;
using nvc.models;
using nvc.onvif;

namespace nvc.controllers {
	public class MainFrameController : IRelesable {
		DeviceCapabilityModel _devCapabilityModel;
		ChannelDescription _currentChannel;
		ChannelDescription CurrentChannel {
			get {
				return _currentChannel;
			}
			set { _currentChannel = value; }
		}
		MainFrame _mainFrame;
		Panel _settingsPanel;
		UserControl _tempControl;
		FlowLayoutPanel _devicePanel;
		List<DeviceChannelControl> _lstDevCannelsCtrl;
		List<LinkCheckButton> _buttonsList;
		IDisposable _subscription;

		DeviceControl _devCtrl = null;

		public MainFrameController() {
			_lstDevCannelsCtrl = new List<DeviceChannelControl>();
			_buttonsList = new List<LinkCheckButton>();
		}

		#region Init section
		InformationForm _savingSettingsForm;
		public void Init(DeviceCapabilityModel devModel, Session session) {
			_subscription = devModel.Load(session).Subscribe(arg => {
				_devCapabilityModel = arg;
				_devicePanel.SuspendLayout();
				_settingsPanel.SuspendLayout();

				CreateDeviceControl();
				CreateDeviceControlsLinkButtons(_devCtrl);

				FillChannelsControl(_devicePanel, _settingsPanel);

				_devicePanel.ResumeLayout();
				_settingsPanel.ResumeLayout();
			}, err => {
				_savingSettingsForm = new InformationForm("ERROR");
				_savingSettingsForm.SetErrorMessage(err.Message);
				_savingSettingsForm.ShowCloseButton(WorkflowController.Instance.ReleaseMainFrameController);
				_savingSettingsForm.ShowDialog(_mainFrame);
			});
		}
		public void KillEveryOne() {
			WorkflowController.Instance.KillEveryBody();
		}
		public UserControl CreateMainFrame(DeviceCapabilityModel devModel, Session session) {
			//DevItem = devItem;
			_mainFrame = new MainFrame();
			_mainFrame.Dock = DockStyle.Fill;
			
			//Create panels and add device control
			CreatePanels();

			_tempControl = new LoadingPropertyPage();
			_devicePanel.Controls.Add(_tempControl);


			_mainFrame.AddDevicePanel(_devicePanel);
			_mainFrame.AddPropertyPanel(_settingsPanel);

			//Load channels description
			Init(devModel, session);

			return _mainFrame;
		}

		void CreateDeviceControlsLinkButtons(DeviceControl ctrl) {
			//DeviceIdentification button
			LinkCheckButton link = new LinkCheckButton(false, ctrl.SettingsFrame);
			_buttonsList.Add(link);
			link.NameLable.CreateBinding(x => x.Text, Constants.Instance, x => x.constLinkButtonIdentificationAndStatus);
			link.ModelSession = _devCapabilityModel.session;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropertyIdentificationController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseIdentificationController;
			link.Click = LbtnClick;
			ctrl.AddLinkButton(link);
			
			//NetworkSettings button
			link = new LinkCheckButton(false, ctrl.SettingsFrame);
			_buttonsList.Add(link);
			link.NameLable.CreateBinding(x => x.Text, Constants.Instance, x => x.constLinkButtonNetworkSettings);
			link.ModelSession = _devCapabilityModel.session;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropertyNetworkSettingsController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseNetworkSettingsController;
			link.Click = LbtnClick;
			ctrl.AddLinkButton(link);
		}
		void CreateChannelControlLinkButtons(DeviceChannelControl ctrl, ChannelDescription channel) {
			//LiveVideo button
			LinkCheckButton link = new LinkCheckButton(false, ctrl.SettingsFrame);
			_buttonsList.Add(link);
			link.NameLable.CreateBinding(x => x.Text, Constants.Instance, x => x.constLinkButtonLiveVideo);
			link.ModelSession = _devCapabilityModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropLiveVideoController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseLiveVideoController;
			link.Click = LbtnClick;
			ctrl.AddLinkButton(link);

			//VideoStreaming button
			link = new LinkCheckButton(false, ctrl.SettingsFrame);
			_buttonsList.Add(link);
			link.NameLable.CreateBinding(x => x.Text, Constants.Instance, x => x.constLinkButtonVideoStreaming);
			link.ModelSession = _devCapabilityModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropVideoStreamingController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseVideoStreamingController;
			link.Click = LbtnClick;
			ctrl.AddLinkButton(link);
		}
		protected DeviceControl CreateDeviceControl(Panel settingsFrame) {
			var ctrl = new DeviceControl(_devCapabilityModel);
			_devCtrl = ctrl;
			ctrl.SettingsFrame = settingsFrame;
			return ctrl;
		}

		void CreateDeviceControl() {
			var ctrl = CreateDeviceControl(_settingsPanel);
			_devicePanel.Controls.Add(ctrl);
		}
		protected DeviceChannelControl CreateChannelControl(Panel settingsFrame, ChannelDescription channel) {
			var devchannelControl = new DeviceChannelControl(channel);
			devchannelControl.SettingsFrame = settingsFrame;

			devchannelControl.ChannelSelected += new channelEventHandlerDelegate(devchannelControl_ChannelSelected);

			_lstDevCannelsCtrl.Add(devchannelControl);

			CreateChannelControlLinkButtons(devchannelControl, channel);

			return devchannelControl;
		}

		protected void FillChannelsControl(Control channelscontainer, Panel settingFrame) {
			if (_tempControl != null) {
				channelscontainer.Controls.Remove(_tempControl);
				_tempControl.Dispose();
				_tempControl = null;
			}
			foreach (var chan in _devCapabilityModel.Channels) {
			    var devChCtrl = CreateChannelControl(settingFrame, chan);
			    channelscontainer.Controls.Add(devChCtrl);
			}
		}
		private void CreatePanels() {
			CreatePropertyPanel();
			_devicePanel = new FlowLayoutPanel();
			_devicePanel.Dock = DockStyle.Fill;
			_devicePanel.FlowDirection = FlowDirection.TopDown;
		}
		private void CreatePropertyPanel() {
			_settingsPanel = new Panel();
			_settingsPanel.Dock = DockStyle.Fill;
			_settingsPanel.SetDoubleBuffered(true);
		}
		#endregion

		#region EventsHandlers
		void devchannelControl_ChannelSelected(ChannelDescription devChannel){//, LinkButtonSetting settings) {
			CurrentChannel = devChannel;
		}
		void DisposeChilds(Panel control) {
			if (control != null)
				foreach (UserControl value in control.Controls) {
					value.Dispose();
				}
		}
		public void LbtnClick(LinkCheckButton sender) {
			_buttonsList.ForEach(x => { x.SetUnclicked(); x.ReleasePropertyAction(); });

			IPropertyController propCtrl = sender.CreatePropertyAction();
			sender.SetClicked();
			_settingsPanel.SuspendLayout();

			//Refreshing panel (removing recent controls) and add new one
			DisposeChilds(_settingsPanel);
			_settingsPanel.Controls.Clear();

			_settingsPanel.Controls.Add(propCtrl.CreateController(_settingsPanel, sender.ModelSession, sender.Channel));
			
			_settingsPanel.ResumeLayout();
		}
		#endregion

		public void ReleaseAll() {
			if(_subscription != null) _subscription.Dispose();
			_buttonsList.ForEach(x => { x.ReleasePropertyAction(); });
			_buttonsList.Clear();

			UnsubscribeDeviceChannel();

			DisposeChilds(_settingsPanel);

			_mainFrame.Dispose();
		}
		protected void UnsubscribeDeviceChannel() {
			foreach (var value in _lstDevCannelsCtrl) {
				value.ChannelSelected -= devchannelControl_ChannelSelected;
			}
		}
	}
}
