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

namespace odm.controllers
{
    using System.Threading;
    using global::onvif.services.media;
    using dev=global::onvif.services.device;
    using odm.onvif;
	using odm.models;
	using System.Disposables;
	using odm.utils.entities;
	using odm.utils;
	using System.IO;
	using odm.utils.controlsUIProvider;

	
	public class DeviceState {
		public string DeviceId { get; set; }
		public LinkButtonsChannelID LastSelectedChannelLink { get; set; }
		public LinkButtonsDeviceID LastSelectedDeviceLink { get; set; }
		public ChannelModel Channel { get; set; }
	}

    public class WorkflowController
    {

        #region Singletone
        protected static WorkflowController _instance;
        protected static Object _syncObj = new Object();
        WorkflowController() { 
			//Delete all meta on startup

			if (Directory.Exists("meta"))
				Directory.GetFiles(@"meta\", "*.meta").ForEach(x=>File.Delete(x));

			SystemPath = Directory.GetCurrentDirectory();
		}
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

		public string SystemPath { get; set; }

		//Release all UI controllers
		public void KillEveryBody() {
			ReleaseControllers();
			//ReleaseDeviceModel();
			RefreshDevicesList();
		}

		public int tick;

		//Implementation for save last open link for device
		List<DeviceState> _devicesCookie = new List<DeviceState>();
		public void WSDicroveryAddDevice(string DevID){
			DeviceState devSt;
			devSt = _devicesCookie.Find(x => x.DeviceId == DevID);
			if (devSt == null) {
				devSt = new DeviceState() { DeviceId = DevID };
				_devicesCookie.Add(devSt);
			}
		}
		public void LinkChannelClicked(string DevID, LinkButtonsChannelID linkId, ChannelModel Channel) {
			var devSt = _devicesCookie.Find(x => x.DeviceId == DevID);
			if (devSt == null)
				devSt = new DeviceState() { DeviceId = DevID };
			devSt.Channel = Channel;
			devSt.LastSelectedChannelLink = linkId;
			devSt.LastSelectedDeviceLink = LinkButtonsDeviceID.NONE;
		}
		public void LinkDeviceClicked(string DevID, LinkButtonsDeviceID linkId) {
			var devSt = _devicesCookie.Find(x => x.DeviceId == DevID);
			if (devSt == null)
				devSt = new DeviceState() { DeviceId = DevID };
			devSt.Channel = null;
			devSt.LastSelectedDeviceLink = linkId;
			devSt.LastSelectedChannelLink = LinkButtonsChannelID.NONE;
		}
		public DeviceState GetLastDeviceState(string DevID) {
			return _devicesCookie.Find(x => x.DeviceId == DevID);
		}

		public void RefreshDevicesList() {
			GetDeviceListController().RefreshDevicesList();
		}

		#region Controls factory
		public void ReleasePropertyControllers() {
			ReleaseIdentificationController();
			ReleaseLiveVideoController();
			ReleaseDigitalIOController();
			ReleaseMaintenanceController();
			ReleaseCommonEventsController();
			ReleaseNetworkSettingsController();
			ReleaseVideoStreamingController();
			ReleaseMetadataController();
			ReleaseRotationController();
			ReleaseAnalogueOutController();
			ReleaseyDisplayAnnotationController();
			ReleaseTemperingController();
			ReleaseDepthCalibrationController();
			ReleaseObjectTrackerController();
			ReleaseApproMotionDetectorController();
			ReleaseEventController();
			ReleaseRuleEngineController();
			ReleaseAntishakerController();
			ReleaseTimeSettingsController();
			ReleaseSystemLogController();
			ReleaseImagingSettingsController();
			ReleaseXMLExplorerController();
			ReleaseProfileEditorController();
		}
		public void ReleaseControllers()
        {
			ReleasePropertyControllers();
			ReleaseMainFrameController();
        }

		PropertyProfileEditorController _profileEditorController;
		public PropertyProfileEditorController GetPropProfileEditorController() {
			if (_profileEditorController == null)
				_profileEditorController = new PropertyProfileEditorController();
			return _profileEditorController;
		}
		public void ReleaseProfileEditorController() {
			if (_profileEditorController != null) {
				_profileEditorController.ReleaseAll();
				_profileEditorController = null;
			}
		}

		PropertyXMLExplorerController _xmlExplorerController;
		public PropertyXMLExplorerController GetPropXmlExplorerController() {
			if (_xmlExplorerController == null) {
				_xmlExplorerController = new PropertyXMLExplorerController();
			}
			return _xmlExplorerController;
		}
		public void ReleaseXMLExplorerController() {
			if (_xmlExplorerController != null) {
				_xmlExplorerController.ReleaseAll();
				_xmlExplorerController = null;
			}
		}
		PropertyImagingSettingsController _imgSettingsController;
		public PropertyImagingSettingsController GetPropImagingSettingsController(){
			if (_imgSettingsController == null)
				_imgSettingsController = new PropertyImagingSettingsController();
			return _imgSettingsController;
		}
		public void ReleaseImagingSettingsController() {
			if (_imgSettingsController != null) {
				_imgSettingsController.ReleaseAll();
				_imgSettingsController = null;
			}
		}
		PropertySystemLogController _sysLogController;
		public PropertySystemLogController GetPropPropertySystemLogController() {
			if (_sysLogController == null)
				_sysLogController = new PropertySystemLogController();
			return _sysLogController;
		}
		public void ReleaseSystemLogController() {
			if (_sysLogController != null) {
				_sysLogController.ReleaseAll();
				_sysLogController = null;
			}
		}
		PropertyTimeSettingsController _timeSettingsController;
		public PropertyTimeSettingsController GetPropTimeSettingsController() {
			if (_timeSettingsController == null)
				_timeSettingsController = new PropertyTimeSettingsController();
			return _timeSettingsController;
		}
		public void ReleaseTimeSettingsController() {
			if (_timeSettingsController != null) {
				_timeSettingsController.ReleaseAll();
				_timeSettingsController = null;
			}
		}

		InformationController _infoController;
		public InformationController InfoController {
			get {
				if (_infoController == null)
					_infoController = new InformationController();
				return _infoController;
			}
		}
        ErrorFrameController _errorFrameController;
        public ErrorFrameController GetErrorFrameController(){
            if (_errorFrameController == null)
                _errorFrameController = new ErrorFrameController();
            return _errorFrameController;
        }
        MainWindowController _mainWindowController;
        public MainWindowController GetMainWindowController(){
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
		public void ReleaseMainFrameController() {
			if (_mainFrameController != null) {
				_mainFrameController.ReleaseAll();
				_mainFrameController = null;

				UIProvider.Instance.GetMainWindowProvider().Refrersh();
			}
		}
        PropertyIdentificationController _propIdentificationController;
        public PropertyIdentificationController GetPropertyIdentificationController()
        {
            if (_propIdentificationController == null)
                _propIdentificationController = new PropertyIdentificationController();
            return _propIdentificationController;
        }
		public void ReleaseIdentificationController() {
			if (_propIdentificationController != null) {
				_propIdentificationController.ReleaseAll();
				_propIdentificationController = null;
			}
		}
        PropertyNetworkSettingsController _propNetworkSettingsController;
        public PropertyNetworkSettingsController GetPropertyNetworkSettingsController()
        {
            if (_propNetworkSettingsController == null)
                _propNetworkSettingsController = new PropertyNetworkSettingsController();
            return _propNetworkSettingsController;
        }
		public void ReleaseNetworkSettingsController() {
			if (_propNetworkSettingsController != null) {
				_propNetworkSettingsController.ReleaseAll();
				_propNetworkSettingsController = null;
			}
		}
		PropertyMaintenanceController _propMaintenanceController;
		public PropertyMaintenanceController GetPropertyMaintenanceController() {
			if (_propMaintenanceController == null)
				_propMaintenanceController = new PropertyMaintenanceController();
			return _propMaintenanceController;
		}
		public void ReleaseMaintenanceController() {
			if (_propMaintenanceController != null) {
				_propMaintenanceController.ReleaseAll();
				_propMaintenanceController = null;
			}
		}
		PropertyDigitalIOController _propDigitalIOController;
		public PropertyDigitalIOController GetPropertyDigitalIOController() {
			if (_propDigitalIOController == null)
				_propDigitalIOController = new PropertyDigitalIOController();
			return _propDigitalIOController;
		}
		public void ReleaseDigitalIOController() {
			if (_propDigitalIOController != null) {
				_propDigitalIOController.ReleaseAll();
				_propDigitalIOController = null;
			}
		}

		PropertyCommonEventsController _propCommonEventsController;
		public PropertyCommonEventsController GetPropCommonEventsController() {
			if (_propCommonEventsController == null)
				_propCommonEventsController = new PropertyCommonEventsController();
			return _propCommonEventsController;
		}
		public void ReleaseCommonEventsController() {
			if (_propCommonEventsController != null) {
				_propCommonEventsController.ReleaseAll();
				_propCommonEventsController = null;
			}
		}
        PropertyLiveVideoController _propLiveVideoController;
        public PropertyLiveVideoController GetPropLiveVideoController()
        {
            if (_propLiveVideoController == null)
                _propLiveVideoController = new PropertyLiveVideoController();
            return _propLiveVideoController;
        }
		public void ReleaseLiveVideoController() {
			if (_propLiveVideoController != null) {
				_propLiveVideoController.ReleaseAll();
				_propLiveVideoController = null;
			}
		}
		PropertyRotationController _propRotationController;
		public PropertyRotationController GetPropRotationController() {
			if (_propRotationController == null)
				_propRotationController = new PropertyRotationController();
			return _propRotationController;
		}
		public void ReleaseRotationController() {
			if (_propRotationController != null) {
				_propRotationController.ReleaseAll();
				_propRotationController = null;
			}
		}
		PropertyAnalogueOutController _propAnalogueOutController;
		public PropertyAnalogueOutController GetPropAnalogueOutController() {
			if (_propAnalogueOutController == null)
				_propAnalogueOutController = new PropertyAnalogueOutController();
			return _propAnalogueOutController;
		}
		public void ReleaseAnalogueOutController() {
			if (_propAnalogueOutController != null) {
				_propAnalogueOutController.ReleaseAll();
				_propAnalogueOutController = null;
			}
		}
		PropertyDisplayAnnotationController _propyDisplayAnnotationController;
		public PropertyDisplayAnnotationController GetPropDisplayAnnotationController() {
			if (_propyDisplayAnnotationController == null)
				_propyDisplayAnnotationController = new PropertyDisplayAnnotationController();
			return _propyDisplayAnnotationController;
		}
		public void ReleaseyDisplayAnnotationController() {
			if (_propyDisplayAnnotationController != null) {
				_propyDisplayAnnotationController.ReleaseAll();
				_propyDisplayAnnotationController = null;
			}
		}
		PropertyTamperingDetectorsController _propyTemperingController;
		public PropertyTamperingDetectorsController GetPropTemperingController() {
			if (_propyTemperingController == null)
				_propyTemperingController = new PropertyTamperingDetectorsController();
			return _propyTemperingController;
		}
		public void ReleaseTemperingController() {
			if (_propyTemperingController != null) {
				_propyTemperingController.ReleaseAll();
				_propyTemperingController = null;
			}
		}
        PropertyVideoStreamingController _propVideoStreamingController;
        public PropertyVideoStreamingController GetPropVideoStreamingController()
        {
            if (_propVideoStreamingController == null)
                _propVideoStreamingController = new PropertyVideoStreamingController();
            return _propVideoStreamingController;
        }
		public void ReleaseVideoStreamingController() {
			if (_propVideoStreamingController != null) {
				_propVideoStreamingController.ReleaseAll();
				_propVideoStreamingController = null;
			}
		}
		PropertyMetadataController _propMetadataController;
		public PropertyMetadataController GetPropMetadataController() {
			if (_propMetadataController == null)
				_propMetadataController = new PropertyMetadataController();
			return _propMetadataController;
		}
		public void ReleaseMetadataController() {
			if (_propMetadataController != null) {
				_propMetadataController.ReleaseAll();
				_propMetadataController = null;
			}
		}
		PropertyDepthCalibrationController _propDepthCalibrationController;
		public PropertyDepthCalibrationController GetPropDepthCalibrationController() {
			if (_propDepthCalibrationController == null)
				_propDepthCalibrationController = new PropertyDepthCalibrationController();
			return _propDepthCalibrationController;
		}
		public void ReleaseDepthCalibrationController() {
			if (_propDepthCalibrationController != null) {
				_propDepthCalibrationController.ReleaseAll();
				_propDepthCalibrationController = null;
			}
		}
		PropertyEventsController _propEventController;
		public PropertyEventsController GetPropEventController() {
			if (_propEventController == null)
				_propEventController = new PropertyEventsController();
			return _propEventController;
		}
		public void ReleaseEventController() {
			if (_propEventController != null) {
				_propEventController.ReleaseAll();
				_propEventController = null;
			}
		}
		PropertyRuleEngineController _propRuleEngineController;
		public PropertyRuleEngineController GetPropRuleEngineController() {
			if (_propRuleEngineController == null)
				_propRuleEngineController = new PropertyRuleEngineController();
			return _propRuleEngineController;
		}
		public void ReleaseRuleEngineController() {
			if (_propRuleEngineController != null) {
				_propRuleEngineController.ReleaseAll();
				_propRuleEngineController = null;
			}
		}
		PropertyAntishakerController _propAntishakerController;
		public PropertyAntishakerController GetPropAntishakerController() {
			if (_propAntishakerController == null) 
				_propAntishakerController = new PropertyAntishakerController();
			return _propAntishakerController;
		}
		public void ReleaseAntishakerController(){
			if (_propAntishakerController != null) {
				_propAntishakerController.ReleaseAll();
				_propAntishakerController = null;
			}
		}
		PropertyObjectTrackerController _propObjectTrackerController;
		public PropertyObjectTrackerController GetPropObjectTrackerController() {
			if (_propObjectTrackerController == null)
				_propObjectTrackerController = new PropertyObjectTrackerController();
			return _propObjectTrackerController;
		}
		public void ReleaseObjectTrackerController() {
			if (_propObjectTrackerController != null) {
				_propObjectTrackerController.ReleaseAll();
				_propObjectTrackerController = null;
			}
		}
		PropertyApproMotionDetectorController _propApproMotionDetectorController;
		public PropertyApproMotionDetectorController GetPropApproMotionDetectorController() {
			if (_propApproMotionDetectorController == null)
				_propApproMotionDetectorController = new PropertyApproMotionDetectorController();
			return _propApproMotionDetectorController;
		}
		public void ReleaseApproMotionDetectorController() {
			if (_propApproMotionDetectorController != null) {
				_propApproMotionDetectorController.ReleaseAll();
				_propApproMotionDetectorController = null;
			}
		}		
        #endregion
    }
}
