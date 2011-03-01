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
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using odm.models;
using odm.utils;
using odm.utils.extensions;
using odm.utils.controlsUIProvider;
using odm.ui.controls;
using odm.controllers;
using onvif;

namespace odm.controlsUIProvider {
	public class MainFrameProvider : BaseUIProvider, IMainFrameProvider {
		//Frame with all UI functionality for selected device
		MainFrame _mainFrameControl;
		public MainFrame MainFrameControl {
			get {
				if (_mainFrameControl == null) {
					_mainFrameControl = new MainFrame();
				}
				return _mainFrameControl;
			}
		}
		//panel for all settings controls, LiveVideo, NetworkSettings etc..
		ContentControl _propertyPanel { 
			get {
				return MainFrameControl._propertyContainer;
			}
		}
		UserControl _infoControl;
		DeviceControl _devCtrl = null;

		StackPanel _devicePanel;
		LinkButtonsStrings _buttonStrings = new LinkButtonsStrings();
		List<DeviceChannelControl> _chanCtrlList = new List<DeviceChannelControl>();

		public void ReturnToMainFrame() {
			if (_propertyPanel != null) {
				//_propertyPanel = null;
				_propertyPanel.Content = null;//.Children.Clear();
			}
		}

		//Init all controlls ant put on temp LoadingProperty Page
		public void InitView(DeviceCapabilityModel devModel, ImageSource devImg) {
			ClearDeviceLoadingControl();
			CreatePropertyPanel();

			CreateDeviceControl(devModel, devImg);
			//FillChannelsControl(devModel, ChanelSelected);
		}
		//Clete information control "Device Loading"
		public void DeviceLoadingControl() {
			CreateDevicePanel();

			_infoControl = new LoadingPropertyPage();
			_devicePanel.Children.Add(_infoControl);
		}
		protected void ClearDeviceLoadingControl() {
			if (_devicePanel != null) {
				_devicePanel.Children.Clear();
				//_devicePanel.InvalidateVisual();
			}
		}


		#region Info form
		UserControl hidenControl;
		public void HidePropertyContainer() {
			//if (_propertyPanel != null && _propertyPanel.Children.Count != 0) {
				//hidenControl = (UserControl)_propertyPanel.Children[0];
				hidenControl = (UserControl)_propertyPanel.Content;
			//}
		}
		public void ShowPropertyContainer() {
			if (_propertyPanel != null) {
				//_propertyPanel.Children.Clear();
				//_propertyPanel.Children.Add(hidenControl);
				_propertyPanel.Content = hidenControl;
			}
		}

		UserControl lastControl;
		public void AddOnApplyInfo(UserControl userControl) {
			//if (_propertyPanel != null && _propertyPanel.Children.Count > 0) {
				//lastControl = (UserControl)_propertyPanel.Children[0];
				//_propertyPanel.Children.Clear();
				//_propertyPanel.Children.Add(userControl);
				lastControl = (UserControl)_propertyPanel.Content;
				_propertyPanel.Content = userControl;
			//}
		}
		public void RemoveOnApplyInfo(UserControl userControl) {
			if (_propertyPanel != null) {
				//_propertyPanel.Children.Clear();
				//_propertyPanel.Children.Add(userControl);
				_propertyPanel.Content = userControl;
			}
		}
		public void CloseInfoControl() {
			if (lastControl != null) {
				_propertyPanel.Content = lastControl;
				
			}
		}
		public void AddInfoControl(UserControl userControl) {
			//if (_propertyPanel != null && _propertyPanel.Children.Count >0) {
				//lastControl = (UserControl)_propertyPanel.Children[0];
				//_propertyPanel.Children.Clear();
				//_propertyPanel.Children.Add(userControl);
            if (_propertyPanel.Content != null)
            {
                if (_propertyPanel.Content.GetType() != typeof(InfoPageNotification))
                    lastControl = (UserControl)_propertyPanel.Content;
                _propertyPanel.Content = userControl;
            }
			//}
		}
		public void RemoveDeviceLoadingInfoControl() {
			_devicePanel.Children.Clear();
		}
		public void RemoveInfoControl(UserControl userControl) {
			if (_propertyPanel != null) {
				//_propertyPanel.Children.Clear();
				//_propertyPanel.Children.Add(userControl);
				_propertyPanel.Content = lastControl;
			}	
		} 

		// to use during device capability loading, while common forms did not initialized
		protected void RefreshDeviceContainer() {
			if (_devicePanel != null) {
				//_devicePanel.Children.Clear();
				_propertyPanel.Content = null;
			}
		}
		// to use during device capability loading, while common forms did not initialized
		public void AddDeviceLoadInfoControl(BasePropertyControl userControl) {
			RefreshDeviceContainer();
			if (_devicePanel != null)
				_devicePanel.Children.Add(userControl);
		}
		// to use during device capability loading, while common forms did not initialized
		public void RemoveDeviceLoadInfoControl(BasePropertyControl userControl) {
			if (_devicePanel != null)
				_devicePanel.Children.Remove(userControl);
				//_devicePanel.Children.ForEach(x => {
				//    if (x == userControl)
				//        _devicePanel.Children.Remove(x);	
				//});
			ShowPropertyContainer();
		} 
		#endregion

		public void PropertyLoadingControl() {
			_infoControl = new LoadingPropertyPage();
			//_propertyPanel.Children.Clear();
			//_propertyPanel.Children.Add(_infoControl);
			_propertyPanel.Content = _infoControl;
		}

		private void CreateDevicePanel() {
			_devicePanel = MainFrameControl._stackPanel;
		}
		private void CreatePropertyPanel() {
			//_propertyPanel.Children.Clear();
			_propertyPanel.Content = null;
		}
		public void ClearPropertyContainer(){
			//_propertyPanel.Children.Clear();
			_propertyPanel.Content = null;
		}
		public void AddPropertyControl(BasePropertyControl userControl) {
			if (_propertyPanel != null) {
				//_propertyPanel.Children.Clear();
				//_propertyPanel.Children.Add(userControl);
				_propertyPanel.Content = userControl;
			}
		}

		public void ClearUI() {
			_devicePanel.Children.Clear();
			_chanCtrlList.Clear();
		}
		public void ReleaseAll() {
			_chanCtrlList.Clear();
			_mainFrameControl._propertyContainer.Content = null;//.Children.Clear();
			_mainFrameControl._stackPanel.Children.Clear();
		}

		public void SetChannelImage(System.Drawing.Bitmap CurrentImage, VideoSourceToken SourceToken) {
			_chanCtrlList.ForEach(ctrl => {
                if (ctrl._devChannel != null){
				if (ctrl._devChannel.sourceToken == SourceToken)
					ctrl.SetChannelImage(CurrentImage);
                }
			});
		}

		public void CreateDeviceControl(DeviceCapabilityModel devModel, ImageSource devImg) {
			_devCtrl = new DeviceControl(devModel, devImg);
			//_devCtrl.SettingsFrame = _propertyPanel;

			_devicePanel.Children.Add(_devCtrl);
		}

		public void AddChannelControl(VideoSourceToken chantoken, Action<ChannelModel> chanSelect) {
			CreateChannelControl(chantoken, chanSelect);
		}
		public void InitChannelControl(ChannelModel chan) {
			_chanCtrlList.ForEach(x => {
				if (x._chanhelToken == chan.sourceToken)
					x.InitControls(chan);
			});
		}

		//public void FillChannelsControl(DeviceCapabilityModel devModel, Action<ChannelModel> chanSelect) {
		//    foreach (var chan in WorkflowController.Instance.GetMainFrameController().channelsList) {
		//        CreateChannelControl(chan, chanSelect);
		//    }
		//}
		public void SetChannelName(ChannelModel chan, string name) {
			_chanCtrlList.ForEach(x => {
				if (x._devChannel == chan)
					x.SetChannelName(name);
				});
		}
		protected void CreateChannelControl(VideoSourceToken chantoken, Action<ChannelModel> chanSelect) {
			var devchannelcontrol = new DeviceChannelControl(chantoken);
			//devchannelcontrol.SettingsFrame = _propertyPanel;

			devchannelcontrol.ChannelSelected = chanSelect;

			//_lstdevcannelsctrl.add(devchannelcontrol);

			//createchannelcontrollinkbuttons(devchannelcontrol, channel);
			_devicePanel.Children.Add(devchannelcontrol);
			_chanCtrlList.Add(devchannelcontrol);
		}

		public override void ReleaseUI() {
			ReleaseAll();
		}
		
		//Link buttons operations
		public void SetLinkSelection() {
			//[TODO]
		}
		public void ReleaseLinkSelection() {
			_devCtrl.ResetLinkSelection();
			_chanCtrlList.ForEach(x => { ((DeviceChannelControl)x).ResetLinkSelection(); });
		}

		protected void onSelection(LinkCheckButton sender) {
			ReleaseLinkSelection();
			sender.SetClicked();
		}

		//Methodth to initialise linkbuttons
		public void AddDeviceLinkButton(Action click, bool IsChackable, bool Enabled, LinkButtonsDeviceID linkID) {
			var lbtn = new LinkCheckButton() { Click = click, SelectionChanged = onSelection, IsEnabled = Enabled, IsCheckable = IsChackable?Visibility.Visible:Visibility.Hidden };
			switch (linkID) {
				case LinkButtonsDeviceID.CommonEvents:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.commonEvents);
					break;
				case LinkButtonsDeviceID.DigitalIO:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.digitalIO);
					break;
				case LinkButtonsDeviceID.Identification:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.identificationAndStatus);
					break;
				case LinkButtonsDeviceID.Maintenance:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.maintenance);
					break;
				case LinkButtonsDeviceID.SystemLog:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.systemLog);
					break;
				case LinkButtonsDeviceID.OnvifExplorer:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.onvifExplorer);
					break;
				case LinkButtonsDeviceID.Network:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.networkSettings);
				    break;
				case LinkButtonsDeviceID.TimeSettings:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.timesettings);
					break;
				default:
					lbtn.NameLable.Content = "Unknown";
					break;
			}

			try {
				_devCtrl.AddLinkButton(lbtn);
			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		public void AddChannelLinkButton(Action click, ChannelModel channel, bool IsCheckable, 
											bool IsChecked, bool Enabled, LinkButtonsChannelID linkID, 
											Action<bool, Action> chBoxSwitched) {
			var lbtn = new LinkCheckButton() { Click = click, SelectionChanged = onSelection,
											   IsEnabled = Enabled,
											   IsCheckable = IsCheckable ? Visibility.Visible : Visibility.Hidden,
											   IsChecked = IsChecked,
											   ChBoxSwitched = chBoxSwitched
			};

			switch (linkID) {
				case LinkButtonsChannelID.ProfileEditor:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.profileEditor);
					break;
				case LinkButtonsChannelID.AnalogueOut:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.analogueOutput);
					break;
				case LinkButtonsChannelID.Annotation:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.displayAnnotation);
					break;
				case LinkButtonsChannelID.Antishaker:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.cantishaker);
					break;
				case LinkButtonsChannelID.Depth:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.depthCalibration);
					break;
				case LinkButtonsChannelID.Events:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.events);
					break;
				case LinkButtonsChannelID.Metadata:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.metadata);
					break;
				case LinkButtonsChannelID.LiveVideo:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.liveVideo);
					break;
				case LinkButtonsChannelID.ImagingSettings:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.sensorSettings);
					break;
				case LinkButtonsChannelID.Rotation:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.rotation);
					break;
				case LinkButtonsChannelID.Rule:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.ruleEngine);
					break;
				case LinkButtonsChannelID.TamperingDetectors:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.tamperingDetectors);
					break;
				case LinkButtonsChannelID.Tracker:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.objectTracker);
					break;
				case LinkButtonsChannelID.ApproMotionDetector:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.objectTracker);
					break;
				case LinkButtonsChannelID.VideoStreaming:
					lbtn.NameLable.CreateBinding(TextButton.ContentProperty, _buttonStrings, x => x.videoStreaming);
					break;
				default:
					lbtn.NameLable.Content = "Unknown";
					break;
			}
			_chanCtrlList.ForEach(x => { if (x._devChannel == channel) x.AddLinkButton(lbtn); });
		}
	}
}
