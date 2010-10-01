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
using System.Windows.Forms;

namespace nvc.controllers
{
    using nvc.controls;
    using System.Threading;
    using global::onvif.services.media;
    using dev=global::onvif.services.device;
    using nvc.onvif;
	using nvc.models;
	using System.Disposables;
	using nvc.entities;
	using nvc.utils;

    public class LinkButtonSetting: EventArgs
    {
        public LinkButtonSetting(string name, int id, bool isCheckable, Func<IPropertyController> f)
        {
            Name = name;
            IsCheckable = isCheckable;
            func = f;
            PropertyFrame = null;
            ID = id;
        }
        public string Name;
        public bool IsCheckable;
        public Func<IPropertyController> func;
        public Panel PropertyFrame;

        public int ID;
    }

    public class DeviceModelArg : EventArgs
    {
        public DeviceModelArg(DeviceModel devmod)
        {
            DevModel = devmod;
        }
        public DeviceModel DevModel;
    }

    public class WorkflowController
    {
        #region LinkButtons
        protected Dictionary<string, LinkButtonSetting> _dLinkButtons;
        public Dictionary<string, LinkButtonSetting> DictLinkButtonsSet
        {
            get
            {
                _dLinkButtons = new Dictionary<string, LinkButtonSetting>();
				_dLinkButtons.Add(Constants.Instance.constLinkButtonIdentificationAndStatus,
					new LinkButtonSetting("constLinkButtonIdentificationAndStatus", Defaults.constLinkButtonIdentificationAndStatusID, 
                        false, () => { return WorkflowController.Instance.GetPropertyIdentificationController(); }));
				_dLinkButtons.Add(Constants.Instance.constLinkButtonNetworkSettings,
					new LinkButtonSetting("constLinkButtonNetworkSettings", Defaults.constLinkButtonNetworkSettingsID,
                        false, () => { return WorkflowController.Instance.GetPropertyNetworkSettingsController(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonDigitalIO,
                //    new LinkButtonSetting(Constants.constLinkButtonDigitalIO, Constants.constLinkButtonDigitalIOID,
                //        false, () => { return new PropertyDigitalIO(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonMaintenance,
                //    new LinkButtonSetting(Constants.constLinkButtonMaintenance, Constants.constLinkButtonMaintenanceID,
                //        false, () => { return new PropertyMaintenance(); }));
				_dLinkButtons.Add(Constants.Instance.constLinkButtonLiveVideo,
					new LinkButtonSetting("constLinkButtonLiveVideo", Defaults.constLinkButtonLiveVideoID,
                        false, () => { return WorkflowController.Instance.GetPropLiveVideoController(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonEvents,
                //    new LinkButtonSetting(Constants.constLinkButtonEvents, Constants.constLinkButtonEventsID,
                //        false, () => { return new PropertyEvents(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonDepthCalibration,
                //    new LinkButtonSetting(Constants.constLinkButtonDepthCalibration, Constants.constLinkButtonDepthCalibrationID,
                //        false, () => { return new BasePropertyControl(); }));
				_dLinkButtons.Add(Constants.Instance.constLinkButtonVideoStreaming,
                    new LinkButtonSetting("constLinkButtonVideoStreaming", Defaults.constLinkButtonVideoStreamingID,
                        false, () => { return WorkflowController.Instance.GetPropVideoStreamingController(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonDisplayAnnotation,
                //    new LinkButtonSetting(Constants.constLinkButtonDisplayAnnotation, Constants.constLinkButtonDisplayAnnotationID,
                //        true, () => { return new BasePropertyControl(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonTamperingDetectors,
                //    new LinkButtonSetting(Constants.constLinkButtonTamperingDetectors, Constants.constLinkButtonTamperingDetectorsID,
                //        true, () => { return new BasePropertyControl(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonObjectTracker,
                //    new LinkButtonSetting(Constants.constLinkButtonObjectTracker, Constants.constLinkButtonObjectTrackerID,
                //        true, () => { return new BasePropertyControl(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonRuleEngine,
                //    new LinkButtonSetting(Constants.constLinkButtonRuleEngine, Constants.constLinkButtonRuleEngineID,
                //        true, () => { return new BasePropertyControl(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonAntishaker,
                //    new LinkButtonSetting(Constants.constLinkButtonAntishaker, Constants.constLinkButtonAntishakerID,
                //        true, () => { return new BasePropertyControl(); }));
                //_dLinkButtons.Add(Constants.constLinkButtonRotation,
                //    new LinkButtonSetting(Constants.constLinkButtonRotation, Constants.constLinkButtonRotationID,
                //        true, () => { return new BasePropertyControl(); }));
                
                return _dLinkButtons;
            }
        }

        public LinkCheckButton CreateLinkCheckButton(string name, Panel propFrame)
        {
            LinkCheckButton lnkBtn;
            if (DictLinkButtonsSet.ContainsKey(name))
            {
                LinkButtonSetting lset = DictLinkButtonsSet[name];
                lset.PropertyFrame = propFrame;
                lnkBtn = new LinkCheckButton(lset);
            }
            else
                throw new System.ArgumentException("LinkButton name did not found");

            return lnkBtn;
        }
        #endregion

        #region Singletone
        protected static WorkflowController _instance;
        protected static Object _syncObj = new Object();
        WorkflowController() { }
        public static WorkflowController Instance
        {
            get
            {
                lock (_syncObj)
                {
                    if (_instance == null)
                        _instance = new WorkflowController();
                }
                return _instance;
            }
        } 
        #endregion

        public void ReleaseDeviceModel()
        {
			if (_currentDevice != null) {
				_currentDevice.ReleaseAll();
				_currentDevice = null;
			}
        }

		public void KillEveryBody() {
			ReleaseControllers();
			ReleaseDeviceModel();
			GetMainWindowController().ClearMainFrame();
		}

        public void CreateDeviceModel(DeviceModelInfo devinfo)
        {
            DeviceModel devModel = new DeviceModel();
            
            devModel.Name = devinfo.Name;
            devModel.IPAddress = devinfo.IpAddress;
            devModel.Firmware = devinfo.Firmware;
            devModel.DeviceId = devinfo.DeviceId;
            devModel.Manufacturer = devinfo.Manufacturer;
            devModel.Hardware = devinfo.HardwareId;
            devModel.devDescr = devinfo.devDescr;

            SetCurrent(devModel);
            //SubscribeToNetworkSettings();
        }
        public event EventHandler ModelInfoAdded;
        public void AddDeviceModelInfo(DeviceModelInfo devinfo)
        {
            DeviceItemsInfoList.Add(devinfo);
            if (ModelInfoAdded != null)
                ModelInfoAdded(this, devinfo);
        }
        protected void SetCurrent(DeviceModel devModel)
        {
            _currentDevice = devModel;
        }

        protected List<DeviceModelInfo> _deviceItemsInfoList;
        public List<DeviceModelInfo> DeviceItemsInfoList
        {
            get
            {
                if (_deviceItemsInfoList == null)
                {
                    _deviceItemsInfoList = new List<DeviceModelInfo>();
                }
                return _deviceItemsInfoList;
            }
        }

        public int ClearDeviceItemList()
        {
            int count = DeviceItemsInfoList.Count;
            DeviceItemsInfoList.Clear();
            return count;
        }

        protected DeviceModel _currentDevice;
        protected DeviceModel CurrentDevice
        {
            get
            {
                if (_currentDevice == null)
                    throw new SystemException("Device Intem not initialised");
                return _currentDevice;
            }
            set { 
				_currentDevice = value; 
			}
        }
        public DeviceModel GetCurrentDevice()
        {
            return CurrentDevice;
        }

        public DeviceModelInfo GetDeviceInfoByID(string id)
        {
            DeviceModelInfo devItem;
            devItem = DeviceItemsInfoList.Single(x => x.IpAddress == id);

            return devItem;
        }

		public void RefreshDevice(DeviceModel depricatedModel, DeviceDescription nDevDescr) {
			//Remove from DeviceListControl
			if (DeviceItemsInfoList.Where(x => x.DeviceId == depricatedModel.DeviceId).Any()) {
				var devInfo = DeviceItemsInfoList.Where(x => x.devDescr == depricatedModel.devDescr).FirstOrDefault();
				DeviceItemsInfoList.Remove(devInfo);
				GetDeviceListController().RemoveDevListControlItem(devInfo);
			} else {
				//device list need to be refreshed
			}
			//Get list of initialised settings
			List<Func<IDisposable>> DeviceSubscriptionList = new List<Func<IDisposable>>();
			Dictionary<string, List<Func<DeviceChannel, IDisposable>>> channelsList = new Dictionary<string, List<Func<DeviceChannel, IDisposable>>>();
			//For device settings
			if (depricatedModel.IsPropertyIdentificationReady)
				DeviceSubscriptionList.Add(SubscribeToNetworkStatus);
			if (depricatedModel.IsPropertyNetworkSettingsReady)
				DeviceSubscriptionList.Add(SubscribeToNetworkSettings);
			//For channels settings
			foreach (var value in depricatedModel.ChannelsList) {
				List<Func<DeviceChannel, IDisposable>> ChannelsSubscriptionList = new List<Func<DeviceChannel, IDisposable>>();
				var devchan = depricatedModel.ChannelsList.ToList()[0].Value;
				if (devchan.IsVideoStreamingInitialised) {
					ChannelsSubscriptionList.Add(SubscribeToVideoURI);
					ChannelsSubscriptionList.Add(SubscribeToAvailableEncoders);
					ChannelsSubscriptionList.Add(SubscribeToAvailableResolutions);
				}
				if (devchan.IsVideoURLInitialised && !devchan.IsVideoStreamingInitialised) {
					ChannelsSubscriptionList.Add(SubscribeToVideoURI);
				}
				channelsList.Add(value.Key, ChannelsSubscriptionList);
			}

			//Remove from DeviceModelList
			depricatedModel.ReleaseAll();

			if (nDevDescr.devInfo != null) {
				var devModel = new DeviceModelInfo();
				devModel.devDescr = nDevDescr;
				devModel.DeviceId = nDevDescr.Id;
				devModel.Firmware = nDevDescr.devInfo.FirmwareVersion;
				devModel.HardwareId = nDevDescr.devInfo.HardwareId;
				devModel.IpAddress = nDevDescr.IPAddress;
				devModel.Manufacturer = nDevDescr.devInfo.Manufacturer;
				devModel.Name = nDevDescr.devInfo.Model;
				GetDeviceListController().AddDevListControlItem(devModel);
				//GetDeviceListController().SelectDevListControlItem(devModel);
				CreateDeviceModel(devModel);
			} else {
				//device list need to be refreshed
			}

			foreach (var value in DeviceSubscriptionList) {
				value();
			}
			SubscribeToChannelsDescription(channelsList);
		}

        #region Subscribe to ONVIF
		public IDisposable SubscribeToAvailableEncoders(DeviceChannel devChan) {
			var _dev = CurrentDevice;

			var subscription = devChan.ChannelModel.GetAvailableEncoders(_dev.devDescr.CreateSession())
				.ObserveOn(SynchronizationContext.Current)
				.OnError(err => { })
				.Subscribe(encoders => {
					var encodlst = devChan.EncodersList;
					foreach (var value in encoders) {
						encodlst.Add(value);
					}
					devChan.EncodersList = encodlst;
				});
			return subscription;
		}
		public IDisposable SubscribeToAvailableResolutions(DeviceChannel devChan) {
			var _dev = CurrentDevice;

			var subscription = devChan.ChannelModel.GetAvailableResolutions(_dev.devDescr.CreateSession())
				.ObserveOn(SynchronizationContext.Current)
				.OnError(err => { })
				.Subscribe(resolutions => {
					var resLst = devChan.ResolutionsList;
					foreach(var value in resolutions){
						var resolution = new AvailableResolution();
						resolution.Resolution = value;
						resLst.Add(resolution);
					}
					devChan.ResolutionsList = resLst;
				});
			return subscription;
		}
		public IDisposable SubscribeToVideoURI(DeviceChannel devChan) {
			var _dev = CurrentDevice;

			var subscription = devChan.ChannelModel.GetStreamUri(_dev.devDescr.CreateSession())
				.ObserveOn(SynchronizationContext.Current)
				.OnError(err =>{})
				.Subscribe(devUri => {
					devChan.MediaStreamUri = devUri;
				});
			return subscription;
		}
		public IDisposable SubscribeToChannelsDescription(Dictionary<string, List<Func<DeviceChannel, IDisposable>>> dictList) {
			var _dev = CurrentDevice;

			var subscription = Channel.GetChannels(_dev.devDescr.CreateSession())
				.ObserveOn(SynchronizationContext.Current)
				.OnError(err => { })
				.Subscribe(chanList => {
					DeviceChannel chan;
					foreach (var value in chanList) {
						chan = new DeviceChannel(value.name, value.m_videoSourceToken);
						chan.SetModelChannel(value);

						_dev.AddChannel(chan);

						//Subscription for reload all data from channel for save setting issue
						if (dictList != null) {
							if(dictList.ContainsKey(value.m_videoSourceToken))
							{
								var lst = dictList[value.m_videoSourceToken];
								foreach(var vallst in lst)
								{
									vallst(chan);
								}
							}
							else{
								//ERROR - release all
							}
						}
					}
					_dev.RiseChannelsInitialisedEvent();
				});
			return subscription;
		}
        public IDisposable SubscribeToChannelsDescription(){
			return SubscribeToChannelsDescription(null);
        }
		public IDisposable SubscribeToNetworkStatus() {
			var _dev = CurrentDevice;
			var _session = _dev.devDescr.CreateSession();

			var subscription = _session.GetNetworkStatus()
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(t => {
					_dev.SetModelNetworkStatus(t);
					_dev.ResetModelNetworkStatus();
				}, err=>{
					DebugHelper.Error(err);
				});

			return subscription;
		}
		
		public event EventHandler SaveNetworkSettingsCompleate;
		public event EventHandler SaveNetworkSettingsCompleateRebootNeedet;
		public event EventHandler SaveNetworkSettingsError;
		public IDisposable SubscribeToSaveNetworkSettings(NetworkSettings e)
        {
			var _dev = CurrentDevice;
			var _session = _dev.devDescr.CreateSession();

			var subscription = e.Save(_session)
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(devDesc => {
					//SubscribeToNetworkSettings();
					//get device description and recreate device with new settings
					if (devDesc != null) {
						RefreshDevice(_dev, devDesc);
						if (SaveNetworkSettingsCompleate != null)
							SaveNetworkSettingsCompleate(this, null);
					} else {
						if (SaveNetworkSettingsCompleateRebootNeedet != null) {
							SaveNetworkSettingsCompleateRebootNeedet(this, null);
						}
					}
				}, error => { 
					//get success and unsuccess saved settings list and notify user or recreate device
					if (SaveNetworkSettingsError != null) {
						SaveNetworkSettingsError(this, null);
					}
				});
			return subscription;
        }
		public IDisposable SubscribeToNetworkSettings()
        {
			var _dev = CurrentDevice;
			var _session = _dev.devDescr.CreateSession();

			var subscription = _session.GetNetworkSettings()
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(t => {
					_dev.SetModelNetworkSettings(t);
					_dev.ResetModelNetworkSettings();
				}, err => {
					DebugHelper.Error(err);
				});
			return subscription;
        }
        #endregion Subscribe to ONVIF

        #region Entities factory
		//public DeviceChannel CreateDeviceChannel(string name, )
		//{
		//    DeviceChannel devChannel = new DeviceChannel(name);
		//    return devChannel;
		//}
        public void AddChannelsList(DeviceModel devItem, List<DeviceChannel> chanList)
        {
            foreach(var val in chanList)
            {
                devItem.AddChannel(val);
            }
        }        
        #endregion

        #region Controls factory  
        public void ReleaseControllers()
        {
            if (_propIdentificationController != null)
            {
				_propIdentificationController.ReleaseAll();
                _propIdentificationController = null;
            }
            if (_propNetworkSettingsController != null)
            {
				_propNetworkSettingsController.ReleaseAll();
                _propNetworkSettingsController = null;
            }
            if (_propLiveVideoController != null)
            {
				_propLiveVideoController.ReleaseAll();
                _propLiveVideoController = null;
            }
            if (_propVideoStreamingController != null)
            {
				_propVideoStreamingController.ReleaseAll();
                _propVideoStreamingController = null;
            }
			if (_mainFrameController != null) {
				_mainFrameController.ReleaseAll();
				_mainFrameController = null;
			}
        }

        ErrorFrameController _errorFrameController;
        public ErrorFrameController GetErrorFrameController()
        {
            if (_errorFrameController == null)
                _errorFrameController = new ErrorFrameController();
            return _errorFrameController;
        }
        MainWindowController _mainWindowController;
        public MainWindowController GetMainWindowController()
        {
            if (_mainWindowController == null)
                _mainWindowController = new MainWindowController();
            return _mainWindowController;
        }
        DeviceListController _devListController;
        public DeviceListController GetDeviceListController()
        {
            if (_devListController == null)
                _devListController = new DeviceListController();
            return _devListController;
        }
        MainFrameController _mainFrameController;
        public MainFrameController GetMainFrameController()
        {
            if (_mainFrameController == null)
                _mainFrameController = new MainFrameController();
            return _mainFrameController;
        }
        PropertyIdentificationController _propIdentificationController;
        public PropertyIdentificationController GetPropertyIdentificationController()
        {
            if (_propIdentificationController == null)
                _propIdentificationController = new PropertyIdentificationController();
            return _propIdentificationController;
        }
        PropertyNetworkSettingsController _propNetworkSettingsController;
        public PropertyNetworkSettingsController GetPropertyNetworkSettingsController()
        {
            if (_propNetworkSettingsController == null)
                _propNetworkSettingsController = new PropertyNetworkSettingsController();
            return _propNetworkSettingsController;
        }
        PropertyLiveVideoController _propLiveVideoController;
        public PropertyLiveVideoController GetPropLiveVideoController()
        {
            if (_propLiveVideoController == null)
                _propLiveVideoController = new PropertyLiveVideoController();
            return _propLiveVideoController;
        }
        PropertyVideoStreamingController _propVideoStreamingController;
        public PropertyVideoStreamingController GetPropVideoStreamingController()
        {
            if (_propVideoStreamingController == null)
                _propVideoStreamingController = new PropertyVideoStreamingController();
            return _propVideoStreamingController;
        }
		
        #endregion
    }
}
