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
using odm.utils.entities;
using odm.utils;
using odm.models;
using odm.onvif;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using tt=onvif.types;
using odm.controllers;
using System.IO;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using Microsoft.Win32.SafeHandles;
using odm.player;
using System.ServiceModel;
using System.Drawing.Imaging;
using System.Xml;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class MainFrameController : BasePropertyController {
		DeviceCapabilityModel _devModel;

		List<LinkCheckButtonController> _buttonsList;
		IDisposable _subscription;
		IDisposable _eventsDeviceSubscription;
		IDisposable _screenSourceSubscription;
		IDisposable _eventsImageSubscription;

		IMainFrameProvider _mainFrameProvider = UIProvider.Instance.GetMainFrameProvider();

		public string DeviceID { get; set; }
		
		//Events
		Dictionary<string, bool> ImageLoading = new Dictionary<string,bool>();
		System.Drawing.Image CurrentImage;
		public Action<EventDescriptor> EventAction;
		public Action<EventDescriptor> RemoveEventAction;
		public Action<EventDescriptor> DeviceEventAction;
		public Action<EventDescriptor> DeviceRemoveEventAction;
		int _eventQueueLength = 15;
		List<DataProcessInfo> _dataProcesses = new List<DataProcessInfo>();
		Dictionary<string, Queue<EventDescriptor>> EventsPool;
		public List<EventDescriptor> GetEventList(string tag) {
			if (EventsPool.ContainsKey(tag))
				return EventsPool[tag].ToList();
			else
				return null;
		}

		public MainFrameController() {
			_buttonsList = new List<LinkCheckButtonController>();

			//UIProvider.Instance.MainWindowProvider.MainView.FormClosing += new FormClosingEventHandler(MainFrameController_FormClosing);
		}

		public void CreateController(Session session) {
			//Create panels and add device control
			CurrentSession = session;
			_mainFrameProvider.DeviceLoadingControl();
			//Load model and fill UI
			LoadControl();
		}

		Image _deviceImage;
		void SetDeviceImage() {
			switch (_devModel.devInfo.Manufacturer) {
				case "Synesis":
					if (_devModel.devInfo.Model.Contains("DK-6467-CAM")) {
						_deviceImage = odm.utils.properties.Resources.mc;
					}
					if (_devModel.devInfo.Model.Contains("DK-6467-ENC2")) {
						_deviceImage = odm.utils.properties.Resources.mb;
					}
					break;
				case "AXIS":
					if (_devModel.devInfo.Model.Contains("AXIS P3301")) {
						_deviceImage = odm.utils.properties.Resources.axis_p_3301;
					}
					break;
				default:
					_deviceImage = odm.utils.properties.Resources.other;
					break;
			}
		}

		public DataProcessInfo GetProcessByChannel(ChannelDescription chan) {
			DataProcessInfo datProc = null;
			try {
				datProc = _dataProcesses.Find(x => x.Channel.Id == chan.Id);
			} catch(Exception err) {
				dbg.Error(err);
			}

			return datProc;
		}
		public void StopVideoStreaming() {
			_dataProcesses.ForEach(x => {
				x.ReleaseAll();
				_dataProcesses.Remove(x);
			});
		}
		void CheckVideoStreaming(){
			_dataProcesses.ForEach(x => { 
				try{
					Process.GetProcessById(x.VideoProcess.Id);
				}
				catch(Exception err){
					Start(x);
				}
			});
		}
		public void StartVideoStreaming(ChannelDescription chan) {
			DataProcessInfo dataProcInfo = new DataProcessInfo();

			dataProcInfo.Resolution = chan.encoderResolution;
			try {
				dataProcInfo.VideoProcess = new Process();

				dataProcInfo.VideoProcess.StartInfo.FileName = "odm-player-host.exe";
				dataProcInfo.VideoProcess.StartInfo.CreateNoWindow = false;
				///!!!!!!!!!!!!!!!!
				dataProcInfo.VideoProcess.StartInfo.UseShellExecute = true;

				int PID = Process.GetCurrentProcess().Id;
				dataProcInfo.VideoProcessFileName = Guid.NewGuid().ToString();

				dataProcInfo.Channel = chan;
				dataProcInfo.MetadataPipeName = "net.pipe://localhost/" + dataProcInfo.VideoProcessFileName;

				dataProcInfo.MetadataFileLog = @"meta\"+ dataProcInfo.Channel.Name + ".meta";

				dataProcInfo.VideoProcess.StartInfo.Arguments = String.Format("/server-pipe:{0} /parent-pid:{1}",
					dataProcInfo.MetadataPipeName, PID);

				dataProcInfo.VideoProcessFrameSize = dataProcInfo.Resolution.Width * dataProcInfo.Resolution.Height * 4;

				dataProcInfo.VideoProcessFile = MemoryMappedFile.CreateNew(dataProcInfo.VideoProcessFileName,
					dataProcInfo.VideoProcessFrameSize, MemoryMappedFileAccess.ReadWriteExecute);

				Start(dataProcInfo);

				_dataProcesses.Add(dataProcInfo);

			} catch (Exception err) {
				//LogUtils.WriteError(err.Message);
				dbg.Error(err);
				dataProcInfo.ReleaseAll();
			}
		}
		void Start(DataProcessInfo dataProcInfo) {
			dataProcInfo.VideoProcess.Start();
			log.WriteInfo("dataProcInfo.VideoProcess.Start");

			Thread.Sleep(1000);

			var epAddr = new EndpointAddress(dataProcInfo.MetadataPipeName);
			var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
			binding.OpenTimeout = TimeSpan.FromSeconds(10);
			dataProcInfo.iPlayer = DuplexChannelFactory<IPlayer>.CreateChannel(dataProcInfo.callback, binding, epAddr);
			log.WriteInfo("dataProcInfo.iPlayer = DuplexChannelFactory<IPlayer>.CreateChannel");
			dataProcInfo.iPlayer.SetVideoBuffer(dataProcInfo.VideoProcessFileName, dataProcInfo.Resolution.Width,
				dataProcInfo.Resolution.Height, dataProcInfo.Resolution.Width * 4, PixelFormat.Format32bppArgb);
			log.WriteInfo("dataProcInfo.iPlayer.SetVideoBuffer");
			dataProcInfo.iPlayer.Play(dataProcInfo.Channel.mediaUri);
			log.WriteInfo("dataProcInfo.iPlayer.Play");
			dataProcInfo.iPlayer.Subscribe();
			log.WriteInfo("dataProcInfo.iPlayer.Subscribe");
		}

		public void ReloadModel() {
			ReleaseContainer();
			UIProvider.Instance.GetMainFrameProvider().ClearPropertyContainer();
			UIProvider.Instance.GetMainFrameProvider().ClearUI();
			UIProvider.Instance.GetMainFrameProvider().PropertyLoadingControl();
			LoadControl();
		}
		bool IsMediaError;
		void LoadMediaErrorMessage() {
			UIProvider.Instance.GetInfoFormProvider().DisplayErrorForm(new Exception("Media URI is null"), null);
		}
		protected override void LoadControl() {
			_devModel = new DeviceCapabilityModel();
			WorkflowController.Instance.tick = Environment.TickCount;
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				SetDeviceImage();

				//Create controls for Device settings and channels settings
				_mainFrameProvider.InitView(_devModel, ChannelSelected, ImageConversion.ToImageSource(_deviceImage));
				
				//Init Links
				CreateDeviceControlsLinkButtons();
				_devModel.Channels.ForEach(chan => { 
					CreateChannelControlLinkButtons(chan);
					if (chan.mediaUri != "") {
						//Init separate processes for all video sources
						StartVideoStreaming(chan);
					} else {
						IsMediaError = true;
					}
				});
				
				//Events initialisetion
				EventsPool = new Dictionary<string, Queue<EventDescriptor>>();
				EventsPool.Add("device", new Queue<EventDescriptor>());
				_devModel.Channels.ForEach(ch => {
					EventsPool.Add(ch.Id, new Queue<EventDescriptor>());
					ImageLoading.Add(ch.Id, false);
					SnapShotSubscription(CurrentSession, ch.Id);
				});

				if (_devModel.capabilities.Events != null && _devModel.capabilities.Events.WSPullPointSupport) {
					SubscribeToEvents(CurrentSession);
				}

				_devModel.session.deviceDescription.removal
					.ObserveOn(SynchronizationContext.Current).Subscribe(un => {
								ReleaseAll();
							});
				//Open default control
				if (IsMediaError) {
					LoadMediaErrorMessage();
				} else {
					OpenLastOrDefaultControl();
				}
			}, err => {
				OnLoadingError(err);
			});
		}
		void OpenIdentification() {
			BasePropertyController propCtrl = WorkflowController.Instance.GetPropertyIdentificationController();
			propCtrl.CreateController(CurrentSession, null);
		}
		public void OpenLiveVideo(ChannelDescription chan) {
			if (chan != null) {
				WorkflowController.Instance.LinkChannelClicked(DeviceID, LinkButtonsChannelID.LiveVideo, chan);
				WorkflowController.Instance.ReleasePropertyControllers();
				BasePropertyController propCtrl = WorkflowController.Instance.GetPropLiveVideoController();

				CheckVideoStreaming();

				propCtrl.CreateController(CurrentSession, chan);
			}
		}
		void OpenLastOrDefaultControl(){
			var devState = WorkflowController.Instance.GetLastDeviceState(DeviceID);
			if (devState == null) {
				OpenIdentification();
				return;
			}
			if (devState.Channel == null) {
				if (devState.LastSelectedDeviceLink == LinkButtonsDeviceID.NONE) {
					OpenIdentification();
					return;
				}
				var linkBtn = _buttonsList.Find(x => x.LinkDeviceID == devState.LastSelectedDeviceLink);
				if (linkBtn == null) {
					OpenIdentification();
					return;
				}
				BasePropertyController propCtrl = linkBtn.CreatePropertyAction();
				propCtrl.CreateController(linkBtn.ModelSession, null);
			} else {
				if (devState.LastSelectedChannelLink == LinkButtonsChannelID.NONE) {
					OpenIdentification();
					return;
				}
				var linkBtn = _buttonsList.Find(x =>x.Channel != null && x.Channel.Id == devState.Channel.Id && x.LinkChannelID == devState.LastSelectedChannelLink);

				if (linkBtn == null) {
					OpenIdentification();
					return;
				}
				BasePropertyController propCtrl = linkBtn.CreatePropertyAction();
				UIProvider.Instance.GetMainFrameProvider().SetLinkSelection();
				propCtrl.CreateController(linkBtn.ModelSession, linkBtn.Channel);
			}
		}
		void OnLoadingError(Exception err) {
			UIProvider.Instance.GetInfoFormProvider().DisplayLoadingDeviceErrorForm(err, ReturnToDeviceList);
		}

		void CreateDeviceControlsLinkButtons() {
			//DeviceIdentification button
			LinkCheckButtonController link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropertyIdentificationController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseIdentificationController;
			link.Click = LbtnClick;
			link.Enabled = true;
			link.AddLinkDeviceButton(LinkButtonsDeviceID.Identification);
			_buttonsList.Add(link);

			//Time Settings button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropTimeSettingsController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseTimeSettingsController;
			link.Click = LbtnClick;
			link.Enabled = true;
			link.AddLinkDeviceButton(LinkButtonsDeviceID.TimeSettings);
			_buttonsList.Add(link);

			//NetworkSettings button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropertyNetworkSettingsController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseNetworkSettingsController;
			link.Click = LbtnClick; 
			link.Enabled = true;
			link.AddLinkDeviceButton(LinkButtonsDeviceID.Network);
			_buttonsList.Add(link);

			//DigitalIO button
			//link = new LinkCheckButtonController(false);
			//link.ModelSession = _devModel.session;
			//link.CreatePropertyAction = WorkflowController.Instance.GetPropertyDigitalIOController;
			//link.ReleasePropertyAction = WorkflowController.Instance.ReleaseDigitalIOController;
			//link.Click = LbtnClick;
			//link.Enabled = true;
			//link.AddLinkDeviceButton(LinkButtonsDeviceID.DigitalIO);
			//_buttonsList.Add(link);

			//Maintenance button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropertyMaintenanceController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseMaintenanceController;
			link.Click = LbtnClick;
			link.Enabled = true;
			link.AddLinkDeviceButton(LinkButtonsDeviceID.Maintenance);
			_buttonsList.Add(link);

			////System log button
			//link = new LinkCheckButtonController(false);
			//link.ModelSession = _devModel.session;
			//link.CreatePropertyAction = WorkflowController.Instance.GetPropPropertySystemLogController;
			//link.ReleasePropertyAction = WorkflowController.Instance.ReleaseSystemLogController;
			//link.Click = LbtnClick;
			//link.Enabled = true;
			//link.AddLinkDeviceButton(LinkButtonsDeviceID.SystemLog);
			//_buttonsList.Add(link);

			//Xml explorer button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropXmlExplorerController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseXMLExplorerController;
			link.Click = LbtnClick;
			link.Enabled = true;
			link.AddLinkDeviceButton(LinkButtonsDeviceID.XMLExplorer);
			_buttonsList.Add(link);

			//Device Events button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropCommonEventsController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseCommonEventsController;
			if (_devModel.capabilities.Events == null)
				link.Enabled = false;
			else
				link.Enabled = _devModel.capabilities.Events.WSPullPointSupport;
			link.Click = LbtnClick;
			link.AddLinkDeviceButton(LinkButtonsDeviceID.CommonEvents);
			_buttonsList.Add(link);

		}
		public void RefershLikButtonsEnabledState(ChannelDescription chan) {
			_buttonsList.ForEach(x => {
				if (x.Channel == chan) {
					if (x.LinkChannelID == LinkButtonsChannelID.Annotation)
						x.IsChecked = chan.modules.DisplayAnnotation.GetValueOrDefault(false);
					if (x.LinkChannelID == LinkButtonsChannelID.Tracker)
						x.IsChecked = chan.modules.ObjectTracker.GetValueOrDefault(false);
				}
			});
		}
		void CreateChannelControlLinkButtons(ChannelDescription channel) {
			//LiveVideo button
			LinkCheckButtonController link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropLiveVideoController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseLiveVideoController;
			link.Click = LbtnClick;
			link.Enabled = true;
			link.AddLinkChannelButton(LinkButtonsChannelID.LiveVideo);
			_buttonsList.Add(link);

			//ImagingSettings button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropImagingSettingsController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseImagingSettingsController;
			link.Click = LbtnClick;
			if (_devModel.capabilities.Imaging == null)
				link.Enabled = false;
			else
				link.Enabled = true;
			link.AddLinkChannelButton(LinkButtonsChannelID.ImagingSettings);
			_buttonsList.Add(link);

			//VideoStreaming button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropVideoStreamingController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseVideoStreamingController;
			link.Click = LbtnClick;
			link.Enabled = true;
			link.AddLinkChannelButton(LinkButtonsChannelID.VideoStreaming);
			_buttonsList.Add(link);

			//Events button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropEventController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseEventController;
			if (_devModel.capabilities.Events == null)
				link.Enabled = false;
			else
				link.Enabled = _devModel.capabilities.Events.WSPullPointSupport;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.Events);
			_buttonsList.Add(link);

			//Metadata button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropMetadataController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseMetadataController;
			//if (_devModel.cap == null)
			//    link.Enabled = false;
			//else
			//    link.Enabled = _devModel.capabilities.Events.WSPullPointSupport;
			link.Enabled = true;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.Metadata);
			_buttonsList.Add(link);

			//DepthCalibration button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropDepthCalibrationController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseDepthCalibrationController;
			link.Click = LbtnClick;
			if (channel.modules.SceneCalibrator == null)
				link.Enabled = false;
			else
				link.Enabled = true;
			link.AddLinkChannelButton(LinkButtonsChannelID.Depth);
			_buttonsList.Add(link);

			//Display annotation button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropDisplayAnnotationController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseyDisplayAnnotationController;
			link.IsCheckable = false;
			if (channel.modules.DisplayAnnotation == null)
				link.Enabled = false;
			else
				link.Enabled = true;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.Annotation);
			_buttonsList.Add(link);

			//Tampering Detector button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropTemperingController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseTemperingController;
			link.IsCheckable = false;
			if (channel.modules.DisplayAnnotation == null)
				link.Enabled = false;
			else
				link.Enabled = true;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.TamperingDetectors);
			_buttonsList.Add(link);

			//ObjectTracker button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropObjectTrackerController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseObjectTrackerController;
			link.CheckBoxSwitchedAcion = CheckBoxSwitched;
			link.IsCheckable = true;
			link.IsChecked = channel.modules.ObjectTracker.GetValueOrDefault(false);
			if (channel.modules.ObjectTracker == null)
				link.Enabled = false;
			else
				link.Enabled = true;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.Tracker);
			_buttonsList.Add(link);

			//Rule Engine button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropRuleEngineController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseRuleEngineController;
			link.IsCheckable = false;
			if (channel.modules.RuleEngine == null)
				link.Enabled = false;
			else
				link.Enabled = true;
			//Disable Rule engine
			link.Enabled = false;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.Rule);
			_buttonsList.Add(link);

			//Antishaker
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropAntishakerController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseAntishakerController;
			link.CheckBoxSwitchedAcion = CheckBoxSwitched;
			link.IsCheckable = true;
			link.IsChecked = channel.modules.DigitalAntishaker.GetValueOrDefault(false);
			if (channel.modules.DigitalAntishaker == null)
				link.Enabled = false;
			else
				link.Enabled = true;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.Antishaker);
			_buttonsList.Add(link);

			//Rotation button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropRotationController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseRotationController;
			link.IsCheckable = false;
			link.Enabled = false;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.Rotation);
			_buttonsList.Add(link);

			//Analogue Out button
			link = new LinkCheckButtonController(false);
			link.ModelSession = _devModel.session;
			link.Channel = channel;
			link.CreatePropertyAction = WorkflowController.Instance.GetPropAnalogueOutController;
			link.ReleasePropertyAction = WorkflowController.Instance.ReleaseAnalogueOutController;
			link.IsCheckable = false;
			link.Enabled = false;
			link.Click = LbtnClick;
			link.AddLinkChannelButton(LinkButtonsChannelID.AnalogueOut);
			_buttonsList.Add(link);

		}

		protected void CheckBoxSwitched(LinkCheckButtonController link, Action onSwitchError) {
			switch (link.LinkChannelID) {
				case LinkButtonsChannelID.Tracker:
					link.Channel.modules.ObjectTracker = link.IsChecked;
					ApplyChBoxChanges(link, onSwitchError);
					break;
				case LinkButtonsChannelID.Antishaker:
					link.Channel.modules.DigitalAntishaker = link.IsChecked;
					ApplyChBoxChanges(link, onSwitchError);
					break;
			}
		}
		protected void ApplyChBoxChanges(LinkCheckButtonController link, Action onSwitchError) {
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
					_devModel = devMod;
				}, err => {
					if (onSwitchError != null)
						onSwitchError();
					link.IsChecked = !link.IsChecked;
					ChBApplyError(err);
				}, () => {
					ChBApplyCompleate();
				});
			ChBOnApply(InfoFormStrings.Instance.applyChanges);
		}
		void ChBApplyError(Exception err) {
			UIProvider.Instance.GetMainWindowProvider().EnableControls();
			UIProvider.Instance.GetInfoFormProvider().DisplayErrorForm(err, ReturnToMainFrame);
		}
		void ChBApplyCompleate() {
			UIProvider.Instance.GetMainWindowProvider().EnableControls();
			UIProvider.Instance.GetInfoFormProvider().ReleaseUI();
		}
		void ChBOnApply(string message) {
			UIProvider.Instance.GetMainWindowProvider().DisableControls();
			UIProvider.Instance.GetInfoFormProvider().DisplayOnCheckBoxApplyChangesForm(message);
		}
		public void UnSubscribeToEvents(){
			if (_eventsImageSubscription != null) _eventsImageSubscription.Dispose();
			if (_screenSourceSubscription != null) _screenSourceSubscription.Dispose();
			if (_eventsDeviceSubscription != null) {
				_eventsDeviceSubscription.Dispose();
			}
		}

		void SubscribeToEvents(Session currenSession){
			_eventsDeviceSubscription = currenSession.GetEvents().ObserveOn(SynchronizationContext.Current)
				.Subscribe(arg => {
					if (arg == null || arg.topic == null || arg.message == null || arg.arrivalTime == null)
						return;
					try {
						string type = String.Concat(arg.topic.Any.Select(x => x.InnerText));
						var strngs = type.Split('/');
						var shortType = strngs.Last();
						if (shortType != null && shortType != "ChannelState" && arg.message.Source != null && arg.message.Data != null) {

							string details = "";
							if (arg.message.Data != null)
								details = String.Join(", ", (arg.message.Data.SimpleItem ?? new tt::ItemListSimpleItem[0]).Select(x => String.Format("{0} = {1}", x.Name, x.Value)));

							var eventDscr = new EventDescriptor() {
								Date = arg.message.UtcTime.ToString(),
								Type = shortType,
								Details = details
							};

							var value = arg.message.Source.SimpleItem.Where(x => x.Name == "VideoSourceConfigurationToken").Select(x => x.Value).FirstOrDefault();
							if (value == null) {
								//Device Event
								EventsPool["device"].Enqueue(eventDscr);
								if (EventsPool["device"].Count > _eventQueueLength) {
									if (DeviceRemoveEventAction != null)
										DeviceRemoveEventAction(EventsPool["device"].Dequeue());
									else
										EventsPool["device"].Dequeue();
								}
								if (DeviceEventAction != null) { DeviceEventAction(eventDscr); }
							} else {
								//Channel event
								ChannelEventSubscription(currenSession, eventDscr, value);
							}
						}
					} catch(Exception err) {
						OnMinorError(err);
					}
				}, err => {
					OnMinorError(err);
				});
		}

		void ChannelEventSubscription(Session currenSession, EventDescriptor eventDscr, string VidSrcCnfToken) {
			_screenSourceSubscription = currenSession.GetVideoSourceConfigurations().ObserveOn(SynchronizationContext.Current)
				.Subscribe(ret => {
					try {
						var retval = ret.Where(vidS => vidS.token == VidSrcCnfToken).Select(vidS => vidS.SourceToken).FirstOrDefault();
						if (retval != null) {
							//Channel specified
							eventDscr.ChannelID = retval;
							eventDscr.WaitingForImage = true;
							if (ImageLoading[retval] == false) {
								ImageLoading[retval] = true;

								SnapShotSubscription(currenSession, retval);
							}
							//else {
							//    eventDscr.Screen = (Image)CurrentImage.Clone();
							//}
							if (EventsPool.ContainsKey(retval)) {
								//Add event description to correspondent queue
								EventsPool[retval].Enqueue(eventDscr);

								if (EventsPool[retval].Count > _eventQueueLength) {
									if (RemoveEventAction != null)
										RemoveEventAction(EventsPool[retval].Dequeue());
									else
										EventsPool[retval].Dequeue();
								}
								if (EventAction != null) { EventAction(eventDscr); }
							}
						}
					} catch (Exception err) {
						OnMinorError(err);
					}
				}, err => {
					OnMinorError(err);
				});
		}
		void SnapShotSubscription(Session currenSession, string SourceToken) {
			_eventsImageSubscription = currenSession.GetSnapshot(NvcHelper.GetChannelProfileToken(SourceToken)).ObserveOn(SynchronizationContext.Current)
				.Subscribe(img => {
					CurrentImage = (Image)img.Clone();
					UIProvider.Instance.GetMainFrameProvider().SetChannelImage((Bitmap)CurrentImage, SourceToken);

					EventsPool.ForEach(pl => {
						pl.Value.ForEach(que => {
							if (que.WaitingForImage && que.ChannelID == SourceToken) {
								que.Screen = (Image)CurrentImage.Clone();
								que.WaitingForImage = false;
							};
						});
					});
					ImageLoading[SourceToken] = false;
				}, err => {
					//DebugHelper.Error(err);
					log.WriteError(err.Message);
				});
		}

		void ChannelSelected(ChannelDescription devChannel){
			CurrentChannel = devChannel;
			OpenLiveVideo(devChannel);
		}
		
		public void LbtnClick(LinkCheckButtonController sender) {
			//_mainFrameProvider.ReleaseLinkSelection();
			//_mainFrameProvider.SetLinkButtonSelection(sender);
			if(sender.Channel != null)
				WorkflowController.Instance.LinkChannelClicked(DeviceID, sender.LinkChannelID, sender.Channel);
			else
				WorkflowController.Instance.LinkDeviceClicked(DeviceID, sender.LinkDeviceID);
			WorkflowController.Instance.ReleasePropertyControllers();
			BasePropertyController propCtrl = sender.CreatePropertyAction();

			CheckVideoStreaming();

			propCtrl.CreateController(sender.ModelSession, sender.Channel);
		}

		public override void  ReleaseAll(){
			ReleaseContainer();
			UIProvider.Instance.ReleaseMainFrameContainer();
			UIProvider.Instance.ReleaseMainFrameProvider();
		}
		void ReleaseContainer() {
			//?
			if (ImageLoading != null)
				ImageLoading.Clear();
			if (_buttonsList != null) {
				_buttonsList.ForEach(x => { x.ReleasePropertyAction(); });
				_buttonsList.Clear();
			}
			if (EventsPool != null)
				EventsPool.Clear();
			//?

			if (_subscription != null) _subscription.Dispose();
			UnSubscribeToEvents();

			//Kill all data processes
			_dataProcesses.ForEach(x => x.ReleaseAll());
			_dataProcesses.Clear();
		}

		protected override void ApplyChanges() { }
		protected override void CancelChanges() { }
	}
	public class PlayerCallbacks : IPlayerCallbacks {
		public PlayerCallbacks(DataProcessInfo dataInfo) {
			_procInfo = dataInfo;
		}
		DataProcessInfo _procInfo;
		public Action<string> Append;

		protected void ParseMetadata(string metadataStr) {
			var doc = new XmlDocument();
			using (var reader = new StringReader(metadataStr)) {
				//reader.
				doc.Load(reader);
			};
			var metadata = doc.DocumentElement.Deserialize<tt::MetadataStream>();
			if (metadata.Items != null && metadata.Items.Length > 0) {
				var sb = new StringBuilder();
				using (var sw = new StringWriter(sb)) {
					using (var writer = new XmlTextWriter(sw)) {
						writer.Formatting = Formatting.Indented;
						doc.DocumentElement.WriteTo(writer);
					}
				}
				sb.AppendLine();
				sb.AppendLine();
				if (Append != null) {
					Append(sb.ToString());
				}
				File.AppendAllText(_procInfo.MetadataFileLog, sb.ToString());
			}
		}
		public void MetadataReceived(string metadataStr) {
			try {
#if PARSE_METADATA
				ParseMetadata(metadataStr);
#else
				var sb = new StringBuilder(metadataStr);
				sb.AppendLine();
				sb.AppendLine();
				if (Append != null) {
					Append(sb.ToString());
				}
				File.AppendAllText(_procInfo.MetadataFileLog, sb.ToString());
#endif	
			} catch (Exception err) {
				//if (Append != null) {
				//    Append(new StringBuilder().AppendLine().AppendLine().ToString());
				//    Append(metadataStr);
				//}
				dbg.Error(err);
			}
		}


		public void LogMessageAcquired(LogMessage logMessage) {
			log.WriteEvent(logMessage.message, logMessage.source, logMessage.eventType);
		}
	}
	public class DataProcessInfo {
		public DataProcessInfo(){
			callback = new PlayerCallbacks(this);
		}
		public PlayerCallbacks callback;
		public IPlayer iPlayer {get;set;}
		public Process VideoProcess { get; set; }
		public string VideoProcessFileName { get; set; }
		public MemoryMappedFile VideoProcessFile { get; set; }
		public int VideoProcessFrameSize { get;set; }

		public string MetadataPipeName { get; set; }

		public string MetadataFileLog {get;set;}

		public ChannelDescription Channel { get; set; }
		public Size Resolution { get; set; }

		public void ReleaseAll(){
			if (VideoProcess != null) {
				try { 
					VideoProcess.Kill(); 
				} 
				catch(Exception err) {
					dbg.Error(err);
				}
				VideoProcess.Dispose();
			}
			if (VideoProcessFile != null)
				VideoProcessFile.Dispose();
		}
	}
	public class EventDescriptor : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		public string ChannelID = "device";
		public bool WaitingForImage = true;
		public string EventID { get; set; }
		public string Type { get; set; }
		public string Details { get; set; }
		public string Date { get; set; }
		public Image m_screen = new System.Drawing.Bitmap(1,1);
		public Image Screen {
			get { return m_screen; }
			set { m_screen = (Image)value.Clone();
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs("Screen"));
			} 
		}
	}
}
