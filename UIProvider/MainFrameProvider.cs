using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using System.Windows.Forms;
using odm.utils;
using System.Drawing;
using odm.models;
using odm.controllers;

namespace odm.controls.UIProvider {
	public class MainFrameProvider : BaseUIProvider {
		//Frame with all UI functionality for selected device
		MainFrame _mainFrameControl;
		public MainFrame MainFrameControl {
			get {
				if (_mainFrameControl == null) {
					_mainFrameControl = new MainFrame();
					_mainFrameControl.Dock = DockStyle.Fill;
				}
				return _mainFrameControl;
			}
		}
		//panel for all settings controls, LiveVideo, NetworkSettings etc..
		Panel _propertyPanel;
		UserControl _infoControl;
		DeviceControl _devCtrl = null;

		FlowLayoutPanel _devicePanel;
		LinkButtonsStrings _buttonStrings = new LinkButtonsStrings();
		List<DeviceChannelControl> _chanCtrlList = new List<DeviceChannelControl>();

		public void ReturnToMainFrame() {
			if (_propertyPanel != null && !_propertyPanel.IsDisposed) {
				_propertyPanel.Controls.ForEach(x => { ((UserControl)x).Dispose(); });
				_propertyPanel.Controls.Clear();
			}
		}

		//Init all controlls ant put on temp LoadingProperty Page
		public void InitView(DeviceCapabilityModel devModel, Action<ChannelDescription> ChanelSelected, Image devImg) {
			ClearDeviceLoadingControl();
			CreatePropertyPanel();

			CreateDeviceControl(devModel, devImg);
			FillChannelsControl(devModel, ChanelSelected);
		}
		//Clete information control "Device Loading"
		public void DeviceLoadingControl() {
			CreateDevicePanel();

			_infoControl = new LoadingPropertyPage();
			_devicePanel.Controls.Add(_infoControl);
		}
		protected void ClearDeviceLoadingControl() {
			if (_devicePanel != null && !_devicePanel.IsDisposed) {
				_devicePanel.Controls.ForEach(x => {
					if (!((UserControl)x).IsDisposed) {
						((UserControl)x).Dispose();
					}
				});
			}
		}


		#region Info form
		public void ShowPropertyContainer() {
			if (_propertyPanel != null && !_propertyPanel.IsDisposed) {
				_propertyPanel.Controls.ForEach(x => {
					if (!((BasePropertyControl)x).IsDisposed) {
						((BasePropertyControl)x).Visible = true;
					}
				});
			}
		}
		public void HidePropertyContainer() {
			if (_propertyPanel != null && !_propertyPanel.IsDisposed) {
				_propertyPanel.Controls.ForEach(x => {
					if (!((BasePropertyControl)x).IsDisposed) {
						((BasePropertyControl)x).Visible = false;
					}
				});
			}
		}
		public void RemoveOnApplyInfo(BasePropertyControl userControl) {
			if (_propertyPanel != null && !_propertyPanel.IsDisposed) {
				_propertyPanel.Controls.ForEach(x => {
					if (!((BasePropertyControl)x).IsDisposed) {
						((BasePropertyControl)x).Dispose();
					}
				});
			}
		}
		public void AddOnApplyInfo(BasePropertyControl userControl) {
			if (_propertyPanel != null && !_propertyPanel.IsDisposed) {
				_propertyPanel.Controls.ForEach(x => {
					if (!((BasePropertyControl)x).IsDisposed) {
						((BasePropertyControl)x).Dispose();
					}
				});
			}
			if (_propertyPanel != null && !_propertyPanel.IsDisposed)
				_propertyPanel.Controls.Add(userControl);
		}
		public void AddInfoControl(BasePropertyControl userControl) {
			HidePropertyContainer();
			if (_propertyPanel != null && !_propertyPanel.IsDisposed)
				_propertyPanel.Controls.Add(userControl);
		}
		public void RemoveInfoControl(BasePropertyControl userControl) {
			if (_propertyPanel != null && !_propertyPanel.IsDisposed)
				_propertyPanel.Controls.ForEach(x => { if (x == userControl) ((BasePropertyControl)x).Dispose(); });
			ShowPropertyContainer();
		} 

		// to use during device capability loading, while common forms did not initialized
		protected void RefreshDeviceContainer() {
			if (_devicePanel != null && !_devicePanel.IsDisposed) {
				_devicePanel.Controls.ForEach(x => {
					if (!((BasePropertyControl)x).IsDisposed) {
						((BasePropertyControl)x).Dispose();
						_devicePanel.Controls.Remove(((BasePropertyControl)x));
					}
				});
			}
		}
		// to use during device capability loading, while common forms did not initialized
		public void AddDeviceLoadInfoControl(BasePropertyControl userControl) {
			RefreshDeviceContainer();
			if (_devicePanel != null && !_devicePanel.IsDisposed)
				_devicePanel.Controls.Add(userControl);
		}
		// to use during device capability loading, while common forms did not initialized
		public void RemoveDeviceLoadInfoControl(BasePropertyControl userControl) {
			if (_devicePanel != null && !_propertyPanel.IsDisposed)
				_devicePanel.Controls.ForEach(x => { if (x == userControl) ((BasePropertyControl)x).Dispose(); });
			ShowPropertyContainer();
		} 
		#endregion

		public void ClearPropertyContainer() {
			if (_propertyPanel != null && !_propertyPanel.IsDisposed) {
				_propertyPanel.Controls.ForEach(x => {
					if (!((BasePropertyControl)x).IsDisposed) {
						((BasePropertyControl)x).ReleaseAll();
						((BasePropertyControl)x).Dispose();
					}
				});
			}
		}
		public void PropertyLoadingControl() {
			_infoControl = new LoadingPropertyPage() { Dock = DockStyle.Top};
			ClearPropertyContainer();
			_propertyPanel.Controls.Clear();
			_propertyPanel.Controls.Add(_infoControl);
		}

		private void CreateDevicePanel() {
			_devicePanel = new FlowLayoutPanel();
			_devicePanel.Dock = DockStyle.Fill;
			_devicePanel.FlowDirection = FlowDirection.TopDown;
			MainFrameControl.AddDevicePanel(_devicePanel);
		}
		private void CreatePropertyPanel() {
			if (_propertyPanel == null) {
				_propertyPanel = new Panel();
				_propertyPanel.Dock = DockStyle.Fill;
				_propertyPanel.SetDoubleBuffered(true);
				MainFrameControl.AddPropertyPanel(_propertyPanel);
			}
		}
		public void StartSTAForm(Form frm) {
			ClearPropertyContainer();
			frm.Show();
		}
		public void AddPropertyControl(BasePropertyControl userControl) {
			ClearPropertyContainer();
			if (_propertyPanel != null && !_propertyPanel.IsDisposed)
				_propertyPanel.Controls.Add(userControl);
		}

		public void ClearUI() {
			_devicePanel.Controls.ForEach(x=>{
				if (!((Control)x).IsDisposed)
					((Control)x).Dispose();
			});
			_devicePanel.Controls.Clear();

			_chanCtrlList.ForEach(x=>{
				if (!x.IsDisposed)
					x.Dispose();
			});
			_chanCtrlList.Clear();
		}
		public void ReleaseAll() {
			_chanCtrlList.Clear();
			if (_mainFrameControl != null && !_mainFrameControl.IsDisposed)
				_mainFrameControl.Dispose();
			_mainFrameControl = null;
		}

		public void SetChannelImage(Image CurrentImage, string SourceToken) {
			_chanCtrlList.ForEach(ctrl => {
				if (ctrl._devChannel.Id == SourceToken)
					ctrl.SetChannelImage(CurrentImage);
			});
		}

		public void CreateDeviceControl(DeviceCapabilityModel devModel, Image devImg) {
			_devCtrl = new DeviceControl(devModel, devImg);
			_devCtrl.SettingsFrame = _propertyPanel;

			_devicePanel.Controls.Add(_devCtrl);
		}

		public void FillChannelsControl(DeviceCapabilityModel devModel, Action<ChannelDescription> chanSelect) {
			foreach (var chan in devModel.Channels) {
				CreateChannelControl(chan, chanSelect);
			}
		}
		public void SetChannelName(ChannelDescription chan, string name) {
			_chanCtrlList.ForEach(x => {
				if (x._devChannel == chan)
					x.SetChannelName(name);
				});
		}
		protected void CreateChannelControl(ChannelDescription channel, Action<ChannelDescription> chanSelect) {
			var devchannelcontrol = new DeviceChannelControl(channel);
			devchannelcontrol.SettingsFrame = _propertyPanel;

			devchannelcontrol.ChannelSelected = chanSelect;

			//_lstdevcannelsctrl.add(devchannelcontrol);

			//createchannelcontrollinkbuttons(devchannelcontrol, channel);
			_devicePanel.Controls.Add(devchannelcontrol);
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
			var lbtn = new LinkCheckButton() { Click = click, SelectionChanged = onSelection, Enabled = Enabled, IsCheckable = IsChackable };
			switch (linkID) {
				case LinkButtonsDeviceID.CommonEvents:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.commonEvents);
					break;
				case LinkButtonsDeviceID.DigitalIO:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.digitalIO);
					break;
				case LinkButtonsDeviceID.Identification:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.identificationAndStatus);
					break;
				case LinkButtonsDeviceID.Maintenance:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.maintenance);
					break;
				case LinkButtonsDeviceID.SystemLog:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.systemLog);
					break;
				case LinkButtonsDeviceID.XMLExplorer:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.xmlExplorer);
					break;
				case LinkButtonsDeviceID.Network:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.networkSettings);
					break;
				case LinkButtonsDeviceID.TimeSettings:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.timesettings);
					break;
				default:
					lbtn.NameLable.Text = "Unknown";
					break;
			}
			_devCtrl.AddLinkButton(lbtn);
		}
		public void AddChannelLinkButton(Action click, ChannelDescription channel, bool IsChackable, 
											bool IsChecked, bool Enabled, LinkButtonsChannelID linkID, 
											Action<bool, Action> chBoxSwitched) {
			var lbtn = new LinkCheckButton() { Click = click, SelectionChanged = onSelection, 
				Enabled = Enabled, IsCheckable = IsChackable, IsChecked = IsChecked ,ChBoxSwitched =  chBoxSwitched};
			switch (linkID) {
				case LinkButtonsChannelID.AnalogueOut:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.analogueOutput);
					break;
				case LinkButtonsChannelID.Annotation:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.displayAnnotation);
					break;
				case LinkButtonsChannelID.Antishaker:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.cantishaker);
					break;
				case LinkButtonsChannelID.Depth:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.depthCalibration);
					break;
				case LinkButtonsChannelID.Events:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.events);
					break;
				case LinkButtonsChannelID.Metadata:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.metadata);
					break;
				case LinkButtonsChannelID.LiveVideo:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.liveVideo);
					break;
				case LinkButtonsChannelID.ImagingSettings:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.sensorSettings);
					break;
				case LinkButtonsChannelID.Rotation:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.rotation);
					break;
				case LinkButtonsChannelID.Rule:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.ruleEngine);
					break;
				case LinkButtonsChannelID.TamperingDetectors:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.tamperingDetectors);
					break;
				case LinkButtonsChannelID.Tracker:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.objectTracker);
					break;
				case LinkButtonsChannelID.VideoStreaming:
					lbtn.NameLable.CreateBinding(x => x.Text, _buttonStrings, x => x.videoStreaming);
					break;
				default:
					lbtn.NameLable.Text = "Unknown";
					break;
			}
			_chanCtrlList.ForEach(x => { if (x._devChannel == channel) x.AddLinkButton(lbtn); });
		}
	}
}
