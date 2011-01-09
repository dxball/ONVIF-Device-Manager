using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.controllers;
using System.Windows.Media;

namespace odm.utils.controlsUIProvider {
	public interface IAnalogueOutProvider {
		void InitView(DeviceIdentificationModel devmodel);
		void ReleaseUI();
	}
	public interface IAntishakerProvider {
		void InitView(LiveVideoModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
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
		void RemoveDeviceDescription(DeviceDescriptionModel devModel);
		void AddDeviceDescription(DeviceDescriptionModel devModel);
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
		void InitView(List<EventDescriptor> eventList, ChannelDescription chan);
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
		void InitView(LiveVideoModel devModel, DataProcessInfo datProcInfo, Action StartRecording, Action StopRecording);
		void ReleaseUI();
	}
	public interface IMainFrameProvider {
		void ReturnToMainFrame();
		void InitView(DeviceCapabilityModel devModel, Action<ChannelDescription> ChanelSelected, ImageSource devImg);
		void DeviceLoadingControl();
		void HidePropertyContainer();
		void ShowPropertyContainer();
		//void RefreshDeviceContainer();
		void PropertyLoadingControl();
		void ClearPropertyContainer();
		void ClearUI();
		void ReleaseAll();
		void SetChannelImage(System.Drawing.Bitmap CurrentImage, string SourceToken);
		void CreateDeviceControl(DeviceCapabilityModel devModel, ImageSource devImg);
		void FillChannelsControl(DeviceCapabilityModel devModel, Action<ChannelDescription> chanSelect);
		void SetChannelName(ChannelDescription chan, string name);
		void ReleaseUI();
		void SetLinkSelection();
		void ReleaseLinkSelection();
		void AddDeviceLinkButton(Action click, bool IsChackable, bool Enabled, LinkButtonsDeviceID linkID);
		void AddChannelLinkButton(Action click, ChannelDescription channel, bool IsChackable,
											bool IsChecked, bool Enabled, LinkButtonsChannelID linkID,
											Action<bool, Action> chBoxSwitched);
	}
	public interface IMaintenanceProvider {
		void InitView(DeviceMaintenanceModel devModel, Action<string> UpgradeFirmware, Action SoftReset);
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
	public interface IRotationProvider {
		void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo);
		void ReleaseUI();
	}
	public interface IRuleEngineProvider {
		void InitView(RuleEngineModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
		void ReleaseUI();
	}
	public interface ISystemLogProvider {
		void InitView();
		void ReleaseUI();
	}
	public interface ITamperingDetectorsProvider {
		void InitView(AnnotationsModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges);
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
	public interface IUIProvider {
		void ReleaseAll();
		void ReleaseMainFrameContainer();
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
