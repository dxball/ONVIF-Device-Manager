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
using odm.models;
using odm.controllers;
using System.Windows.Media;
using onvif.services.media;
using onvif;

namespace odm.utils.controlsUIProvider {
	public interface IAnalogueOutProvider {
		void InitView(DeviceIdentificationModel devmodel);
		void ReleaseUI();
	}
	public interface IAntishakerProvider {
		void InitView(AntishakerModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface ICommonEventsProvider {
		void InitView(List<EventDescriptor> eventList);
		void AddEvent(EventDescriptor evDescr);
		void RemoveEvent(EventDescriptor evDescr);
		void ReleaseUI();
	}
	public interface IDepthCalibrationProvider {
		void InitView(DepthCalibrationModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface IDevicesListProvider {
		void CreateDeviceListControl(Action<DeviceDescriptionModel> itemSelected, Action refreshDevicesList, Action CreateD);
		void FillDeviceList();
		void RefreshDevicesList();
		void RemoveDeviceDescription(IDeviceDescriptionModel devModel);
		void AddDeviceDescription(IDeviceDescriptionModel devModel);
		void ReleaseUI();
	}
	public interface IDigitalIOProvider {
		void InitView();
		void ReleaseUI();
	}
	public interface IDisplayAnnotationProvider {
		void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface IEventsProvider {
		void InitView(List<EventDescriptor> eventList, ChannelModel chan);
		void AddEvent(EventDescriptor evDescr);
		void RemoveEvent(EventDescriptor evDescr);
		void ReleaseUI();
	}
	public interface IIdentificationProvider {
		void InitView(DeviceIdentificationModel devModel, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface IImagingSettingsProvider {
		void InitView(ImagingSettingsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface IInfoFormProvider {
		void DisplayOnCheckBoxApplyChangesForm(string message);
		void DisplayOnApplyChangesForm(string message);
		void DisplayInformationForm(string message, Action linkedAction);
		void DisplayErrorForm(Exception err, Action linkedAction);
		void DisplayLoadingDeviceErrorForm(Exception err, Action linkedAction);
		//void ReleaseDeviceLoadingUI();
		void ReleaseUI();
	}
	public interface ILiveVideoProvider {
		void InitView(LiveVideoModel devModel, DataProcessInfo datProcInfo, Action<string>SetRecordingPath, Action StartRecording, Action StopRecording);
		void ReleaseUI();
	}
	public interface IMainFrameProvider {
		void ReturnToMainFrame();
		void InitView(DeviceCapabilityModel devModel, ImageSource devImg);
		void AddChannelControl(VideoSourceToken chan, Action<ChannelModel> chanSelect);
		void InitChannelControl(ChannelModel chan);
		void DeviceLoadingControl();
		void HidePropertyContainer();
		void ShowPropertyContainer();
		//void RefreshDeviceContainer();
		void PropertyLoadingControl();
		void ClearPropertyContainer();
		void ClearUI();
		void ReleaseAll();
		void SetChannelImage(System.Drawing.Bitmap CurrentImage, VideoSourceToken SourceToken);
		void CreateDeviceControl(DeviceCapabilityModel devModel, ImageSource devImg);
		//void FillChannelsControl(DeviceCapabilityModel devModel, Action<ChannelModel> chanSelect);
		void SetChannelName(ChannelModel chan, string name);
		void ReleaseUI();
		void SetLinkSelection();
		void ReleaseLinkSelection();
		void AddDeviceLinkButton(Action click, bool IsChackable, bool Enabled, LinkButtonsDeviceID linkID);
		void AddChannelLinkButton(Action click, ChannelModel channel, bool IsChackable,
											bool IsChecked, bool Enabled, LinkButtonsChannelID linkID,
											Action<bool, Action> chBoxSwitched);
	}
	public interface IMaintenanceProvider {
		void InitView(MaintenanceModel devModel, Action<string> UpgradeFirmware, Action SoftReset, Action<string> backup, Action<string> restore);
		void ReleaseUI();
	}
	public interface IMainWindowProvider {
		void InitLeftFrame();
		void Refrersh();
		void DisableControls();
		void EnableControls();
		void InitFrame();
		void SetStatusBarText1(string value);
		void SetStatusBarText2(string value);
		void SetStatusBarText3(string value);
		void ReleaseUI();
	}
	public interface IMetadataProvider {
		void InitView(DataProcessInfo dataProc);
		void ApendData(string data);
		void ReleaseUI();
	}
	public interface INetworkSettingsProvider {
		void InitView(DeviceNetworkSettingsModel devModel, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface IObjectTrakkerProvider {
		void InitView(ObjectTrackerModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges); 
		void ReleaseUI();
	}
	public interface IApproMotionDetectorProvider {
		void InitView(ApproMotionDetectorModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface IRotationProvider {
		void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo);
		void ReleaseUI();
	}
	public interface IRuleEngineProvider {
		void InitView(RuleEngineModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface ISystemLogProvider {
		void InitView(SystemLogModel model);
		void ReleaseUI();
	}
	public interface ITamperingDetectorsProvider {
		void InitView(TamperingDetectorsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface ITimeSettingsProvider {
		void InitView(DateTimeSettingsModel devModel, Action apply);
		void ReleaseUI();
	}
	public interface IVideoStreamingProvider {
		void InitView(VideoStreamingModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void RefreshStream();
		void ReleaseUI();
	}
	public interface IXMLExplorerProvider {
		void InitView(DumpModel devModel);
		void ReleaseUI();
	}
	public interface IProfileEditorProvider {
		void InitView(List<Profile> plst, ProfileToken profileToken, Action createNew, Action<Profile> renameProfile, Action<Profile> selectProfile, Action<Profile> deleteProfile);
		void AddProfile(Profile prof);
		void DeleteProfile(Profile prof);
		void ReleaseUI();
	}
	public interface IUIProvider {
		void ReleaseAll();
		void ReleaseMainFrameContainer();
		IProfileEditorProvider GetProfileEditorProvider();
		void ReleaseProfileEditorProvider();
		IXMLExplorerProvider GetXMLExplorerProvider();
		void ReleaseXMLExplorerProvider();
		IImagingSettingsProvider GetImagingSettingsProvider();
		void ReleaseImagingSettingsProvider();
		ISystemLogProvider GetSystemLogProvider();
		void ReleaseSystemLogProvider();
		IInfoFormProvider GetInfoFormProvider();
		void ReleaseInfoFormProvider();
		IAnalogueOutProvider GetAnalogueOutProvider();
		void ReleaseAnalogueOutProvider();
		ICommonEventsProvider GetCommonEventsProvider();
		void ReleaseCommonEventsProvider();
		IDepthCalibrationProvider GetDepthCalibrationProvider();
		void ReleaseDepthCalibrationProvider();
		IDevicesListProvider GetDevicesListProvider();
		void ReleaseDevicesListProvider();
		IDigitalIOProvider GetDigitalIOProvider();
		void ReleaseDigitalIOProvider();
		IDisplayAnnotationProvider GetDisplayAnnotationProvider();
		void ReleaseDisplayAnnotationProvider();
		IEventsProvider GetEventsProvider();
		void ReleaseEventsProvider();
		IIdentificationProvider GetIdentificationProvider();
		void ReleaseIdentificationProvider();
		ITimeSettingsProvider GetTimeSettingsProvider();
		void ReleaseTimeSettingsProvider();
		ILiveVideoProvider GetLiveVideoProvider();
		void ReleaseLiveVideoProvider();
		IMainFrameProvider GetMainFrameProvider();
		void ReleaseMainFrameProvider();
		IMaintenanceProvider GetMaintenanceProvider();
		void ReleaseMaintenanceProvider();
		IMainWindowProvider GetMainWindowProvider();
		void ReleaseMainWindowProvider();
		INetworkSettingsProvider GetNetworkSettingsProvider();
		void ReleaseNetworkSettingsProvider();
		IObjectTrakkerProvider GetObjectTrakkerProvider();
		void ReleaseObjectTrakkerProvider();
		IApproMotionDetectorProvider GetApproMotionDetectorProvider();
		void ReleaseApproMotionDetectorProvider();
		IRotationProvider GetRotationProvider();
		void ReleaseRotationProvider();
		IRuleEngineProvider GetRuleEngineProvider();
		void ReleaseRuleEngineProvider();
		ITamperingDetectorsProvider GetTamperingDetectorsProvider();
		void ReleaseTamperingDetectorsProvider();
		IVideoStreamingProvider GetVideoStreamingProvider();
		void ReleaseVideoStreamingProvider();
		IMetadataProvider GetMetadataProvider();
		void ReleaseMetadataProvider();
		IAntishakerProvider GetAntishakerProvider();
		void ReleaseAntishakerProvider();
	}

	public class UIProvider {
		public static IUIProvider provider;
		public static IUIProvider Instance {
			get {
				return provider;
			}
		}
	}
}
