using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using odm.controls;
using odm.models;
using System.Drawing;
using odm.utils;
using System.Linq.Expressions;

namespace odm.controls.UIProvider {
    public class UIProvider{
		#region Singletone
        protected static UIProvider _instance;
        protected static Object _syncObj = new Object();
		UIProvider() { }
		public static UIProvider Instance{
            get{
                lock (_syncObj){
                    if (_instance == null)
						_instance = new UIProvider();
                }
                return _instance;
            }
        } 
        #endregion Singletone

		public enum UIFramework {
			WINFORMS,
			WPF
		}
		public void SetWPF() {
			_currentFrameWork = UIFramework.WPF;
		}
		public void SetWinForms() {
			_currentFrameWork = UIFramework.WINFORMS;
		}
		UIFramework _currentFrameWork = UIFramework.WINFORMS;
		public UIFramework CurrentFrameWork {
			get {
				return _currentFrameWork;
			}
		}
		
		public void ReleaseAll() {
			ReleaseMainFrameContainer();

			ReleaseDevicesListProvider();
			ReleaseMainFrameProvider();
		}
		public void ReleaseMainFrameContainer() {
			ReleaseInfoFormProvider();
			ReleaseAnalogueOutProvider();
			ReleaseCommonEventsProvider();
			ReleaseDepthCalibrationProvider();
			ReleaseDigitalIOProvider();
			ReleaseDisplayAnnotationProvider();
			ReleaseEventsProvider();
			ReleaseIdentificationProvider();
			ReleaseTimeSettingsProvider();
			ReleaseLiveVideoProvider();
			ReleaseMaintenanceProvider();
			ReleaseNetworkSettingsProvider();
			ReleaseObjectTrakkerProvider();
			ReleaseRotationProvider();
			ReleaseRuleEngineProvider();
			ReleaseTamperingDetectorsProvider();
			ReleaseVideoStreamingProvider();
			ReleaseMetadataProvider();
			ReleaseAntishakerProvider(); 
			ReleaseSystemLogProvider();
			ReleaseImagingSettingsProvider();
		}

		ImagingSettingsProvider _imgSett;
		public ImagingSettingsProvider ImagingSettingsProvider {
			get {
				if (_imgSett == null)
					_imgSett = new ImagingSettingsProvider();
				return _imgSett;
			}
		}
		public void ReleaseImagingSettingsProvider() {
			if (_imgSett != null) {
				_imgSett.ReleaseUI();
				_imgSett = null;
			}
		}

		SystemLogProvider _sysLog;
		public SystemLogProvider SystemLogProvider {
			get {
				if (_sysLog == null)
					_sysLog = new SystemLogProvider();
				return _sysLog;
			}
		}
		public void ReleaseSystemLogProvider() {
			if (_sysLog != null) {
				_sysLog.ReleaseUI();
				_sysLog = null;
			}
		}
		InfoFormProvider _infoForm;
		public InfoFormProvider InfoFormProvider {
			get {
				if (_infoForm == null)
					_infoForm = new InfoFormProvider();
				return _infoForm;
			}
		}
		public void ReleaseInfoFormProvider() {
			if (_infoForm != null) {
				_infoForm.ReleaseUI();
				_infoForm = null;
			}
		}
		AnalogueOutProvider _analogueOutProvider;
		public AnalogueOutProvider AnalogueOutProvider {
			get {
				if (_analogueOutProvider == null)
					_analogueOutProvider = new AnalogueOutProvider();
				return _analogueOutProvider;
			}
		}
		public void ReleaseAnalogueOutProvider() {
			if (_analogueOutProvider != null) {
				_analogueOutProvider.ReleaseUI();
				_analogueOutProvider = null;
			}
		}
		CommonEventsProvider _commonEventsProvider;
		public CommonEventsProvider CommonEventsProvider {
			get {
				if (_commonEventsProvider == null)
					_commonEventsProvider = new CommonEventsProvider();
				return _commonEventsProvider;
			}
		}
		public void ReleaseCommonEventsProvider() {
			if (_commonEventsProvider != null) {
				_commonEventsProvider.ReleaseUI();
				_commonEventsProvider = null;
			}
		}
		DepthCalibrationProvider _depthCalibrationProvider;
		public DepthCalibrationProvider DepthCalibrationProvider {
			get {
				if (_depthCalibrationProvider == null)
					_depthCalibrationProvider = new DepthCalibrationProvider();
				return _depthCalibrationProvider;
			}
		}
		public void ReleaseDepthCalibrationProvider() {
			if (_depthCalibrationProvider != null) {
				_depthCalibrationProvider.ReleaseUI();
				_depthCalibrationProvider = null;
			}
		}
		DevicesListProvider _devListProvider;
		public DevicesListProvider DevicesListProvider {
			get {
				if (_devListProvider == null)
					_devListProvider = new DevicesListProvider();
				return _devListProvider;
			}
		}
		public void ReleaseDevicesListProvider() {
			if (_devListProvider!=null) {
				_devListProvider.ReleaseUI();
				_devListProvider = null;
			}
		}
		DigitalIOProvider _digitalIOProvider;
		public DigitalIOProvider DigitalIOProvider {
			get {
				if (_digitalIOProvider == null)
					_digitalIOProvider = new DigitalIOProvider();
				return _digitalIOProvider;
			}
		}
		public void ReleaseDigitalIOProvider() {
			if(_digitalIOProvider != null){
				_digitalIOProvider.ReleaseUI();
				_digitalIOProvider = null;
			}
		}
		DisplayAnnotationProvider _displayAnnotationProvider;
		public DisplayAnnotationProvider DisplayAnnotationProvider {
			get {
				if (_displayAnnotationProvider == null)
					_displayAnnotationProvider = new DisplayAnnotationProvider();
				return _displayAnnotationProvider;
			}
		}
		public void ReleaseDisplayAnnotationProvider() {
			if(_displayAnnotationProvider != null){
				_displayAnnotationProvider.ReleaseUI();
				_displayAnnotationProvider = null;
			}
		}
		EventsProvider _eventsProvider;
		public EventsProvider EventsProvider {
			get {
				if (_eventsProvider == null)
					_eventsProvider = new EventsProvider();
				return _eventsProvider;
			}
		}
		public void ReleaseEventsProvider() {
			if(_eventsProvider != null){
				_eventsProvider.ReleaseUI();
				_eventsProvider = null;
			}
		}
		IdentificationProvider _identificationProvider;
		public IdentificationProvider IdentificationProvider {
			get {
				if (_identificationProvider == null)
					_identificationProvider = new IdentificationProvider();
				return _identificationProvider;
			}
		}
		public void ReleaseIdentificationProvider() {
			if(_identificationProvider != null){
				_identificationProvider.ReleaseUI();
				_identificationProvider = null;
			}
		}
		TimeSettingsProvider _timesettingsProvider;
		public TimeSettingsProvider TimeSettingsProvider {
			get {
				if (_timesettingsProvider == null)
					_timesettingsProvider = new TimeSettingsProvider();
				return _timesettingsProvider;
			}
		}
		public void ReleaseTimeSettingsProvider() {
			if(_timesettingsProvider != null){
				_timesettingsProvider.ReleaseUI();
				_timesettingsProvider = null;
			}
		}
		LiveVideoProvider _liveVideoProvider;
		public LiveVideoProvider LiveVideoProvider {
			get {
				if (_liveVideoProvider == null)
					_liveVideoProvider = new LiveVideoProvider();
				return _liveVideoProvider;
			}
		}
		public void ReleaseLiveVideoProvider() {
			if(_liveVideoProvider != null){
				_liveVideoProvider.ReleaseUI();
				_liveVideoProvider = null;
			}
		}
		MainFrameProvider _mainFrameProvider;
		public MainFrameProvider MainFrameProvider {
			get {
				if (_mainFrameProvider == null)
					_mainFrameProvider = new MainFrameProvider();
				return _mainFrameProvider;
			}
		}
		public void ReleaseMainFrameProvider() {
			if(_mainFrameProvider != null){
				_mainFrameProvider.ReleaseUI();
				_mainFrameProvider = null;
			}
		}
		MaintenanceProvider _maintenanceProvider;
		public MaintenanceProvider MaintenanceProvider {
			get {
				if (_maintenanceProvider == null)
					_maintenanceProvider = new MaintenanceProvider();
				return _maintenanceProvider;
			}
		}
		public void ReleaseMaintenanceProvider() {
			if(_maintenanceProvider != null){
				_maintenanceProvider.ReleaseUI();
				_maintenanceProvider = null;
			}
		}
		MainWindowProvider _mainWindowProvider;
		public MainWindowProvider MainWindowProvider {
			get {
				if (_mainWindowProvider == null)
					_mainWindowProvider = new MainWindowProvider();
				return _mainWindowProvider;
			}
		}
		public void ReleaseMainWindowProvider() {
			if(_mainWindowProvider != null){
				_mainWindowProvider.ReleaseUI();
				_mainWindowProvider = null;
			}
		}
		NetworkSettingsProvider _networkSettingsProvider;
		public NetworkSettingsProvider NetworkSettingsProvider {
			get {
				if (_networkSettingsProvider == null)
					_networkSettingsProvider = new NetworkSettingsProvider();
				return _networkSettingsProvider;
			}
		}
		public void ReleaseNetworkSettingsProvider() {
			if(_networkSettingsProvider != null){
				_networkSettingsProvider.ReleaseUI();
				_networkSettingsProvider = null;
			}
		}
		ObjectTrakkerProvider _objectTrakkerProvider;
		public ObjectTrakkerProvider ObjectTrakkerProvider {
			get {
				if (_objectTrakkerProvider == null)
					_objectTrakkerProvider = new ObjectTrakkerProvider();
				return _objectTrakkerProvider;
			}
		}
		public void ReleaseObjectTrakkerProvider() {
			if(_objectTrakkerProvider != null){
				_objectTrakkerProvider.ReleaseUI();
				_objectTrakkerProvider = null;
			}
		}
		RotationProvider _rotationProvider;
		public RotationProvider RotationProvider {
			get {
				if (_rotationProvider == null)
					_rotationProvider = new RotationProvider();
				return _rotationProvider;
			}
		}
		public void ReleaseRotationProvider() {
			if(_rotationProvider != null){
				_rotationProvider.ReleaseUI();
				_rotationProvider = null;
			}
		}
		RuleEngineProvider _ruleEngineProvider;
		public RuleEngineProvider RuleEngineProvider {
			get {
				if (_ruleEngineProvider == null)
					_ruleEngineProvider = new RuleEngineProvider();
				return _ruleEngineProvider;
			}
		}
		public void ReleaseRuleEngineProvider() {
			if(_ruleEngineProvider != null){
				_ruleEngineProvider.ReleaseUI();
				_ruleEngineProvider = null;
			}
		}
		TamperingDetectorsProvider _tamperingDetectorsProvider;
		public TamperingDetectorsProvider TamperingDetectorsProvider {
			get {
				if (_tamperingDetectorsProvider == null)
					_tamperingDetectorsProvider = new TamperingDetectorsProvider();
				return _tamperingDetectorsProvider;
			}
		}
		public void ReleaseTamperingDetectorsProvider() {
			if(_tamperingDetectorsProvider != null){
				_tamperingDetectorsProvider.ReleaseUI();
				_tamperingDetectorsProvider = null;
			}
		}
		VideoStreamingProvider _videoStreamingProvider;
		public VideoStreamingProvider VideoStreamingProvider {
			get {
				if (_videoStreamingProvider == null)
					_videoStreamingProvider = new VideoStreamingProvider();
				return _videoStreamingProvider;
			}
		}
		public void ReleaseVideoStreamingProvider() {
			if(_videoStreamingProvider != null){
				_videoStreamingProvider.ReleaseUI();
				_videoStreamingProvider = null;
			}
		}
		MetadataProvider _metadataProvider;
		public MetadataProvider MetadataProvider {
			get {
				if (_metadataProvider == null)
					_metadataProvider = new MetadataProvider();
				return _metadataProvider;
			}
		}
		public void ReleaseMetadataProvider() {
			if (_metadataProvider != null) {
				_metadataProvider.ReleaseUI();
				_metadataProvider = null;
			}
		}
		AntishakerProvider _antishakerProvider;
		public AntishakerProvider AntishakerProvider {
			get {
				if (_antishakerProvider == null)
					_antishakerProvider = new AntishakerProvider();
				return _antishakerProvider;
			}
		}
		public void ReleaseAntishakerProvider() {
			if (_antishakerProvider != null) {
				_antishakerProvider.ReleaseUI();
				_antishakerProvider = null;
			}
		}
    }
}
