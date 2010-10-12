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

	//public class DeviceModelArg : EventArgs
	//{
	//    public DeviceModelArg(DeviceModel devmod)
	//    {
	//        DevModel = devmod;
	//    }
	//    public DeviceModel DevModel;
	//}

    public class WorkflowController
    {

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

		//Release all UI controllers
		public void KillEveryBody() {
			ReleaseControllers();
			//ReleaseDeviceModel();
			GetMainWindowController().ClearMainFrame();
			GetDeviceListController().RefreshDevicesList();
		}

		public void RefreshDevicesList() {
			GetDeviceListController().RefreshDevicesList();
		}

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
		public void ReleaseMainFrameController() {
			if (_mainFrameController != null) {
				_mainFrameController.ReleaseAll();
				_mainFrameController = null;

				GetMainWindowController().Refrersh();
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
		
        #endregion
    }
}
