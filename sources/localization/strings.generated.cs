
using System;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Reflection;
using System.Xml.Linq;
using System.ComponentModel;

namespace nvc{
	public partial class Constants : INotifyPropertyChanged{
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName) {
		if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private const string s_sExceptionWSDiscoveryTitle = @"WSDiscovery error";
		private string m_sExceptionWSDiscoveryTitle=null;
		public string sExceptionWSDiscoveryTitle {
			get { 
				if( m_sExceptionWSDiscoveryTitle == null){
					return s_sExceptionWSDiscoveryTitle;
				}
				return m_sExceptionWSDiscoveryTitle; 
			}
			set { 
				if( value != m_sExceptionWSDiscoveryTitle){
					m_sExceptionWSDiscoveryTitle = value;
					NotifyPropertyChanged("sExceptionWSDiscoveryTitle");
				}
			}  
		}
	
		private const string s_sErrorDevInfoNull = @"Error was during device configuration reading. Please refresh devices list";
		private string m_sErrorDevInfoNull=null;
		public string sErrorDevInfoNull {
			get { 
				if( m_sErrorDevInfoNull == null){
					return s_sErrorDevInfoNull;
				}
				return m_sErrorDevInfoNull; 
			}
			set { 
				if( value != m_sErrorDevInfoNull){
					m_sErrorDevInfoNull = value;
					NotifyPropertyChanged("sErrorDevInfoNull");
				}
			}  
		}
	
		private const string s_sErrorDeviceName = @"none";
		private string m_sErrorDeviceName=null;
		public string sErrorDeviceName {
			get { 
				if( m_sErrorDeviceName == null){
					return s_sErrorDeviceName;
				}
				return m_sErrorDeviceName; 
			}
			set { 
				if( value != m_sErrorDeviceName){
					m_sErrorDeviceName = value;
					NotifyPropertyChanged("sErrorDeviceName");
				}
			}  
		}
	
		private const string s_sErrorDeviceFirmware = @"none";
		private string m_sErrorDeviceFirmware=null;
		public string sErrorDeviceFirmware {
			get { 
				if( m_sErrorDeviceFirmware == null){
					return s_sErrorDeviceFirmware;
				}
				return m_sErrorDeviceFirmware; 
			}
			set { 
				if( value != m_sErrorDeviceFirmware){
					m_sErrorDeviceFirmware = value;
					NotifyPropertyChanged("sErrorDeviceFirmware");
				}
			}  
		}
	
		private const string s_sErrorSaveNetworkSettings = @"Error was during saving network settings. Please refresh device.";
		private string m_sErrorSaveNetworkSettings=null;
		public string sErrorSaveNetworkSettings {
			get { 
				if( m_sErrorSaveNetworkSettings == null){
					return s_sErrorSaveNetworkSettings;
				}
				return m_sErrorSaveNetworkSettings; 
			}
			set { 
				if( value != m_sErrorSaveNetworkSettings){
					m_sErrorSaveNetworkSettings = value;
					NotifyPropertyChanged("sErrorSaveNetworkSettings");
				}
			}  
		}
	
		private const string s_sErrorVlc = @"Media stream error";
		private string m_sErrorVlc=null;
		public string sErrorVlc {
			get { 
				if( m_sErrorVlc == null){
					return s_sErrorVlc;
				}
				return m_sErrorVlc; 
			}
			set { 
				if( value != m_sErrorVlc){
					m_sErrorVlc = value;
					NotifyPropertyChanged("sErrorVlc");
				}
			}  
		}
	
		private const string s_sErrorVlcMediaUriIsEmpty = @"Stream uri is emtpy";
		private string m_sErrorVlcMediaUriIsEmpty=null;
		public string sErrorVlcMediaUriIsEmpty {
			get { 
				if( m_sErrorVlcMediaUriIsEmpty == null){
					return s_sErrorVlcMediaUriIsEmpty;
				}
				return m_sErrorVlcMediaUriIsEmpty; 
			}
			set { 
				if( value != m_sErrorVlcMediaUriIsEmpty){
					m_sErrorVlcMediaUriIsEmpty = value;
					NotifyPropertyChanged("sErrorVlcMediaUriIsEmpty");
				}
			}  
		}
	
		private const string s_sErrorVlcMediaUriIsBad = @"Stream uri is unknown";
		private string m_sErrorVlcMediaUriIsBad=null;
		public string sErrorVlcMediaUriIsBad {
			get { 
				if( m_sErrorVlcMediaUriIsBad == null){
					return s_sErrorVlcMediaUriIsBad;
				}
				return m_sErrorVlcMediaUriIsBad; 
			}
			set { 
				if( value != m_sErrorVlcMediaUriIsBad){
					m_sErrorVlcMediaUriIsBad = value;
					NotifyPropertyChanged("sErrorVlcMediaUriIsBad");
				}
			}  
		}
	
		private const string s_sSaveSettingsFormTitle = @"Saving changes...";
		private string m_sSaveSettingsFormTitle=null;
		public string sSaveSettingsFormTitle {
			get { 
				if( m_sSaveSettingsFormTitle == null){
					return s_sSaveSettingsFormTitle;
				}
				return m_sSaveSettingsFormTitle; 
			}
			set { 
				if( value != m_sSaveSettingsFormTitle){
					m_sSaveSettingsFormTitle = value;
					NotifyPropertyChanged("sSaveSettingsFormTitle");
				}
			}  
		}
	
		private const string s_sSaveSettingsFormText = @"Please wait until changes will be applied";
		private string m_sSaveSettingsFormText=null;
		public string sSaveSettingsFormText {
			get { 
				if( m_sSaveSettingsFormText == null){
					return s_sSaveSettingsFormText;
				}
				return m_sSaveSettingsFormText; 
			}
			set { 
				if( value != m_sSaveSettingsFormText){
					m_sSaveSettingsFormText = value;
					NotifyPropertyChanged("sSaveSettingsFormText");
				}
			}  
		}
	
		private const string s_sSaveSettingsNeedReboot = @"Device need to reboot. Please refresh devices list.";
		private string m_sSaveSettingsNeedReboot=null;
		public string sSaveSettingsNeedReboot {
			get { 
				if( m_sSaveSettingsNeedReboot == null){
					return s_sSaveSettingsNeedReboot;
				}
				return m_sSaveSettingsNeedReboot; 
			}
			set { 
				if( value != m_sSaveSettingsNeedReboot){
					m_sSaveSettingsNeedReboot = value;
					NotifyPropertyChanged("sSaveSettingsNeedReboot");
				}
			}  
		}
	
		private const string s_sCommonAppOn = @"On";
		private string m_sCommonAppOn=null;
		public string sCommonAppOn {
			get { 
				if( m_sCommonAppOn == null){
					return s_sCommonAppOn;
				}
				return m_sCommonAppOn; 
			}
			set { 
				if( value != m_sCommonAppOn){
					m_sCommonAppOn = value;
					NotifyPropertyChanged("sCommonAppOn");
				}
			}  
		}
	
		private const string s_sCommonAppOff = @"Off";
		private string m_sCommonAppOff=null;
		public string sCommonAppOff {
			get { 
				if( m_sCommonAppOff == null){
					return s_sCommonAppOff;
				}
				return m_sCommonAppOff; 
			}
			set { 
				if( value != m_sCommonAppOff){
					m_sCommonAppOff = value;
					NotifyPropertyChanged("sCommonAppOff");
				}
			}  
		}
	
		private const string s_sCommonAppYes = @"Yes";
		private string m_sCommonAppYes=null;
		public string sCommonAppYes {
			get { 
				if( m_sCommonAppYes == null){
					return s_sCommonAppYes;
				}
				return m_sCommonAppYes; 
			}
			set { 
				if( value != m_sCommonAppYes){
					m_sCommonAppYes = value;
					NotifyPropertyChanged("sCommonAppYes");
				}
			}  
		}
	
		private const string s_sCommonAppNo = @"No";
		private string m_sCommonAppNo=null;
		public string sCommonAppNo {
			get { 
				if( m_sCommonAppNo == null){
					return s_sCommonAppNo;
				}
				return m_sCommonAppNo; 
			}
			set { 
				if( value != m_sCommonAppNo){
					m_sCommonAppNo = value;
					NotifyPropertyChanged("sCommonAppNo");
				}
			}  
		}
	
		private const string s_sVideoPriorityStreaming = @"Streaming";
		private string m_sVideoPriorityStreaming=null;
		public string sVideoPriorityStreaming {
			get { 
				if( m_sVideoPriorityStreaming == null){
					return s_sVideoPriorityStreaming;
				}
				return m_sVideoPriorityStreaming; 
			}
			set { 
				if( value != m_sVideoPriorityStreaming){
					m_sVideoPriorityStreaming = value;
					NotifyPropertyChanged("sVideoPriorityStreaming");
				}
			}  
		}
	
		private const string s_sVideoPriorityAnalytics = @"Analytics";
		private string m_sVideoPriorityAnalytics=null;
		public string sVideoPriorityAnalytics {
			get { 
				if( m_sVideoPriorityAnalytics == null){
					return s_sVideoPriorityAnalytics;
				}
				return m_sVideoPriorityAnalytics; 
			}
			set { 
				if( value != m_sVideoPriorityAnalytics){
					m_sVideoPriorityAnalytics = value;
					NotifyPropertyChanged("sVideoPriorityAnalytics");
				}
			}  
		}
	
		private const string s_sLoadingData = @"Loading data ...";
		private string m_sLoadingData=null;
		public string sLoadingData {
			get { 
				if( m_sLoadingData == null){
					return s_sLoadingData;
				}
				return m_sLoadingData; 
			}
			set { 
				if( value != m_sLoadingData){
					m_sLoadingData = value;
					NotifyPropertyChanged("sLoadingData");
				}
			}  
		}
	
		private const string s_sApplicationName = @"Network Video Console";
		private string m_sApplicationName=null;
		public string sApplicationName {
			get { 
				if( m_sApplicationName == null){
					return s_sApplicationName;
				}
				return m_sApplicationName; 
			}
			set { 
				if( value != m_sApplicationName){
					m_sApplicationName = value;
					NotifyPropertyChanged("sApplicationName");
				}
			}  
		}
	
		private const string s_sDeviceControlTitle = @"Device configuration";
		private string m_sDeviceControlTitle=null;
		public string sDeviceControlTitle {
			get { 
				if( m_sDeviceControlTitle == null){
					return s_sDeviceControlTitle;
				}
				return m_sDeviceControlTitle; 
			}
			set { 
				if( value != m_sDeviceControlTitle){
					m_sDeviceControlTitle = value;
					NotifyPropertyChanged("sDeviceControlTitle");
				}
			}  
		}
	
		private const string s_sDeviceControlLanguageEng = @"English";
		private string m_sDeviceControlLanguageEng=null;
		public string sDeviceControlLanguageEng {
			get { 
				if( m_sDeviceControlLanguageEng == null){
					return s_sDeviceControlLanguageEng;
				}
				return m_sDeviceControlLanguageEng; 
			}
			set { 
				if( value != m_sDeviceControlLanguageEng){
					m_sDeviceControlLanguageEng = value;
					NotifyPropertyChanged("sDeviceControlLanguageEng");
				}
			}  
		}
	
		private const string s_sDeviceControlLanguageRus = @"Русский";
		private string m_sDeviceControlLanguageRus=null;
		public string sDeviceControlLanguageRus {
			get { 
				if( m_sDeviceControlLanguageRus == null){
					return s_sDeviceControlLanguageRus;
				}
				return m_sDeviceControlLanguageRus; 
			}
			set { 
				if( value != m_sDeviceControlLanguageRus){
					m_sDeviceControlLanguageRus = value;
					NotifyPropertyChanged("sDeviceControlLanguageRus");
				}
			}  
		}
	
		private const string s_sDevicesListControlTitle = @"Network";
		private string m_sDevicesListControlTitle=null;
		public string sDevicesListControlTitle {
			get { 
				if( m_sDevicesListControlTitle == null){
					return s_sDevicesListControlTitle;
				}
				return m_sDevicesListControlTitle; 
			}
			set { 
				if( value != m_sDevicesListControlTitle){
					m_sDevicesListControlTitle = value;
					NotifyPropertyChanged("sDevicesListControlTitle");
				}
			}  
		}
	
		private const string s_sDevicesListControlRefresh = @"Refresh";
		private string m_sDevicesListControlRefresh=null;
		public string sDevicesListControlRefresh {
			get { 
				if( m_sDevicesListControlRefresh == null){
					return s_sDevicesListControlRefresh;
				}
				return m_sDevicesListControlRefresh; 
			}
			set { 
				if( value != m_sDevicesListControlRefresh){
					m_sDevicesListControlRefresh = value;
					NotifyPropertyChanged("sDevicesListControlRefresh");
				}
			}  
		}
	
		private const string s_sDevicesListControlColumnName = @"Name";
		private string m_sDevicesListControlColumnName=null;
		public string sDevicesListControlColumnName {
			get { 
				if( m_sDevicesListControlColumnName == null){
					return s_sDevicesListControlColumnName;
				}
				return m_sDevicesListControlColumnName; 
			}
			set { 
				if( value != m_sDevicesListControlColumnName){
					m_sDevicesListControlColumnName = value;
					NotifyPropertyChanged("sDevicesListControlColumnName");
				}
			}  
		}
	
		private const string s_sDevicesListControlColumnIPadress = @"IP Adress";
		private string m_sDevicesListControlColumnIPadress=null;
		public string sDevicesListControlColumnIPadress {
			get { 
				if( m_sDevicesListControlColumnIPadress == null){
					return s_sDevicesListControlColumnIPadress;
				}
				return m_sDevicesListControlColumnIPadress; 
			}
			set { 
				if( value != m_sDevicesListControlColumnIPadress){
					m_sDevicesListControlColumnIPadress = value;
					NotifyPropertyChanged("sDevicesListControlColumnIPadress");
				}
			}  
		}
	
		private const string s_sDevicesListControlColumnType = @"Firmware";
		private string m_sDevicesListControlColumnType=null;
		public string sDevicesListControlColumnType {
			get { 
				if( m_sDevicesListControlColumnType == null){
					return s_sDevicesListControlColumnType;
				}
				return m_sDevicesListControlColumnType; 
			}
			set { 
				if( value != m_sDevicesListControlColumnType){
					m_sDevicesListControlColumnType = value;
					NotifyPropertyChanged("sDevicesListControlColumnType");
				}
			}  
		}
	
		private const string s_constLinkButtonIdentificationAndStatus = @"Identification and status";
		private string m_constLinkButtonIdentificationAndStatus=null;
		public string constLinkButtonIdentificationAndStatus {
			get { 
				if( m_constLinkButtonIdentificationAndStatus == null){
					return s_constLinkButtonIdentificationAndStatus;
				}
				return m_constLinkButtonIdentificationAndStatus; 
			}
			set { 
				if( value != m_constLinkButtonIdentificationAndStatus){
					m_constLinkButtonIdentificationAndStatus = value;
					NotifyPropertyChanged("constLinkButtonIdentificationAndStatus");
				}
			}  
		}
	
		private const string s_constLinkButtonNetworkSettings = @"Network settings";
		private string m_constLinkButtonNetworkSettings=null;
		public string constLinkButtonNetworkSettings {
			get { 
				if( m_constLinkButtonNetworkSettings == null){
					return s_constLinkButtonNetworkSettings;
				}
				return m_constLinkButtonNetworkSettings; 
			}
			set { 
				if( value != m_constLinkButtonNetworkSettings){
					m_constLinkButtonNetworkSettings = value;
					NotifyPropertyChanged("constLinkButtonNetworkSettings");
				}
			}  
		}
	
		private const string s_constLinkButtonDigitalIO = @"Digital IO";
		private string m_constLinkButtonDigitalIO=null;
		public string constLinkButtonDigitalIO {
			get { 
				if( m_constLinkButtonDigitalIO == null){
					return s_constLinkButtonDigitalIO;
				}
				return m_constLinkButtonDigitalIO; 
			}
			set { 
				if( value != m_constLinkButtonDigitalIO){
					m_constLinkButtonDigitalIO = value;
					NotifyPropertyChanged("constLinkButtonDigitalIO");
				}
			}  
		}
	
		private const string s_constLinkButtonMaintenance = @"Maintenance";
		private string m_constLinkButtonMaintenance=null;
		public string constLinkButtonMaintenance {
			get { 
				if( m_constLinkButtonMaintenance == null){
					return s_constLinkButtonMaintenance;
				}
				return m_constLinkButtonMaintenance; 
			}
			set { 
				if( value != m_constLinkButtonMaintenance){
					m_constLinkButtonMaintenance = value;
					NotifyPropertyChanged("constLinkButtonMaintenance");
				}
			}  
		}
	
		private const string s_constLinkButtonLiveVideo = @"Live video";
		private string m_constLinkButtonLiveVideo=null;
		public string constLinkButtonLiveVideo {
			get { 
				if( m_constLinkButtonLiveVideo == null){
					return s_constLinkButtonLiveVideo;
				}
				return m_constLinkButtonLiveVideo; 
			}
			set { 
				if( value != m_constLinkButtonLiveVideo){
					m_constLinkButtonLiveVideo = value;
					NotifyPropertyChanged("constLinkButtonLiveVideo");
				}
			}  
		}
	
		private const string s_constLinkButtonEvents = @"Events";
		private string m_constLinkButtonEvents=null;
		public string constLinkButtonEvents {
			get { 
				if( m_constLinkButtonEvents == null){
					return s_constLinkButtonEvents;
				}
				return m_constLinkButtonEvents; 
			}
			set { 
				if( value != m_constLinkButtonEvents){
					m_constLinkButtonEvents = value;
					NotifyPropertyChanged("constLinkButtonEvents");
				}
			}  
		}
	
		private const string s_constLinkButtonDepthCalibration = @"Depth calibration";
		private string m_constLinkButtonDepthCalibration=null;
		public string constLinkButtonDepthCalibration {
			get { 
				if( m_constLinkButtonDepthCalibration == null){
					return s_constLinkButtonDepthCalibration;
				}
				return m_constLinkButtonDepthCalibration; 
			}
			set { 
				if( value != m_constLinkButtonDepthCalibration){
					m_constLinkButtonDepthCalibration = value;
					NotifyPropertyChanged("constLinkButtonDepthCalibration");
				}
			}  
		}
	
		private const string s_constLinkButtonVideoStreaming = @"Video streaming";
		private string m_constLinkButtonVideoStreaming=null;
		public string constLinkButtonVideoStreaming {
			get { 
				if( m_constLinkButtonVideoStreaming == null){
					return s_constLinkButtonVideoStreaming;
				}
				return m_constLinkButtonVideoStreaming; 
			}
			set { 
				if( value != m_constLinkButtonVideoStreaming){
					m_constLinkButtonVideoStreaming = value;
					NotifyPropertyChanged("constLinkButtonVideoStreaming");
				}
			}  
		}
	
		private const string s_constLinkButtonDisplayAnnotation = @"Display  annotation";
		private string m_constLinkButtonDisplayAnnotation=null;
		public string constLinkButtonDisplayAnnotation {
			get { 
				if( m_constLinkButtonDisplayAnnotation == null){
					return s_constLinkButtonDisplayAnnotation;
				}
				return m_constLinkButtonDisplayAnnotation; 
			}
			set { 
				if( value != m_constLinkButtonDisplayAnnotation){
					m_constLinkButtonDisplayAnnotation = value;
					NotifyPropertyChanged("constLinkButtonDisplayAnnotation");
				}
			}  
		}
	
		private const string s_constLinkButtonTamperingDetectors = @"Tampering detectors";
		private string m_constLinkButtonTamperingDetectors=null;
		public string constLinkButtonTamperingDetectors {
			get { 
				if( m_constLinkButtonTamperingDetectors == null){
					return s_constLinkButtonTamperingDetectors;
				}
				return m_constLinkButtonTamperingDetectors; 
			}
			set { 
				if( value != m_constLinkButtonTamperingDetectors){
					m_constLinkButtonTamperingDetectors = value;
					NotifyPropertyChanged("constLinkButtonTamperingDetectors");
				}
			}  
		}
	
		private const string s_constLinkButtonObjectTracker = @"Object tracker";
		private string m_constLinkButtonObjectTracker=null;
		public string constLinkButtonObjectTracker {
			get { 
				if( m_constLinkButtonObjectTracker == null){
					return s_constLinkButtonObjectTracker;
				}
				return m_constLinkButtonObjectTracker; 
			}
			set { 
				if( value != m_constLinkButtonObjectTracker){
					m_constLinkButtonObjectTracker = value;
					NotifyPropertyChanged("constLinkButtonObjectTracker");
				}
			}  
		}
	
		private const string s_constLinkButtonRuleEngine = @"Rule engine";
		private string m_constLinkButtonRuleEngine=null;
		public string constLinkButtonRuleEngine {
			get { 
				if( m_constLinkButtonRuleEngine == null){
					return s_constLinkButtonRuleEngine;
				}
				return m_constLinkButtonRuleEngine; 
			}
			set { 
				if( value != m_constLinkButtonRuleEngine){
					m_constLinkButtonRuleEngine = value;
					NotifyPropertyChanged("constLinkButtonRuleEngine");
				}
			}  
		}
	
		private const string s_constLinkButtonAntishaker = @"Antishaker";
		private string m_constLinkButtonAntishaker=null;
		public string constLinkButtonAntishaker {
			get { 
				if( m_constLinkButtonAntishaker == null){
					return s_constLinkButtonAntishaker;
				}
				return m_constLinkButtonAntishaker; 
			}
			set { 
				if( value != m_constLinkButtonAntishaker){
					m_constLinkButtonAntishaker = value;
					NotifyPropertyChanged("constLinkButtonAntishaker");
				}
			}  
		}
	
		private const string s_constLinkButtonRotation = @"Rotation";
		private string m_constLinkButtonRotation=null;
		public string constLinkButtonRotation {
			get { 
				if( m_constLinkButtonRotation == null){
					return s_constLinkButtonRotation;
				}
				return m_constLinkButtonRotation; 
			}
			set { 
				if( value != m_constLinkButtonRotation){
					m_constLinkButtonRotation = value;
					NotifyPropertyChanged("constLinkButtonRotation");
				}
			}  
		}
	
		private const string s_sButtonSave = @"Save";
		private string m_sButtonSave=null;
		public string sButtonSave {
			get { 
				if( m_sButtonSave == null){
					return s_sButtonSave;
				}
				return m_sButtonSave; 
			}
			set { 
				if( value != m_sButtonSave){
					m_sButtonSave = value;
					NotifyPropertyChanged("sButtonSave");
				}
			}  
		}
	
		private const string s_sButtonCancel = @"Cancel";
		private string m_sButtonCancel=null;
		public string sButtonCancel {
			get { 
				if( m_sButtonCancel == null){
					return s_sButtonCancel;
				}
				return m_sButtonCancel; 
			}
			set { 
				if( value != m_sButtonCancel){
					m_sButtonCancel = value;
					NotifyPropertyChanged("sButtonCancel");
				}
			}  
		}
	
		private const string s_sButtonClose = @"Close";
		private string m_sButtonClose=null;
		public string sButtonClose {
			get { 
				if( m_sButtonClose == null){
					return s_sButtonClose;
				}
				return m_sButtonClose; 
			}
			set { 
				if( value != m_sButtonClose){
					m_sButtonClose = value;
					NotifyPropertyChanged("sButtonClose");
				}
			}  
		}
	
		private const string s_sNotifierTextTitle = @"No connected devices.";
		private string m_sNotifierTextTitle=null;
		public string sNotifierTextTitle {
			get { 
				if( m_sNotifierTextTitle == null){
					return s_sNotifierTextTitle;
				}
				return m_sNotifierTextTitle; 
			}
			set { 
				if( value != m_sNotifierTextTitle){
					m_sNotifierTextTitle = value;
					NotifyPropertyChanged("sNotifierTextTitle");
				}
			}  
		}
	
		private const string s_sNotifierText = @"Please check the device is properly connected, or contact support.";
		private string m_sNotifierText=null;
		public string sNotifierText {
			get { 
				if( m_sNotifierText == null){
					return s_sNotifierText;
				}
				return m_sNotifierText; 
			}
			set { 
				if( value != m_sNotifierText){
					m_sNotifierText = value;
					NotifyPropertyChanged("sNotifierText");
				}
			}  
		}
	
		private const string s_sNotifierTextContacts = @"";
		private string m_sNotifierTextContacts=null;
		public string sNotifierTextContacts {
			get { 
				if( m_sNotifierTextContacts == null){
					return s_sNotifierTextContacts;
				}
				return m_sNotifierTextContacts; 
			}
			set { 
				if( value != m_sNotifierTextContacts){
					m_sNotifierTextContacts = value;
					NotifyPropertyChanged("sNotifierTextContacts");
				}
			}  
		}
	
		private const string s_sPropertyDeviceInfoStatusTitle = @"Device identification and status";
		private string m_sPropertyDeviceInfoStatusTitle=null;
		public string sPropertyDeviceInfoStatusTitle {
			get { 
				if( m_sPropertyDeviceInfoStatusTitle == null){
					return s_sPropertyDeviceInfoStatusTitle;
				}
				return m_sPropertyDeviceInfoStatusTitle; 
			}
			set { 
				if( value != m_sPropertyDeviceInfoStatusTitle){
					m_sPropertyDeviceInfoStatusTitle = value;
					NotifyPropertyChanged("sPropertyDeviceInfoStatusTitle");
				}
			}  
		}
	
		private const string s_sPropertyDeviceInfoStatusLableName = @"Name";
		private string m_sPropertyDeviceInfoStatusLableName=null;
		public string sPropertyDeviceInfoStatusLableName {
			get { 
				if( m_sPropertyDeviceInfoStatusLableName == null){
					return s_sPropertyDeviceInfoStatusLableName;
				}
				return m_sPropertyDeviceInfoStatusLableName; 
			}
			set { 
				if( value != m_sPropertyDeviceInfoStatusLableName){
					m_sPropertyDeviceInfoStatusLableName = value;
					NotifyPropertyChanged("sPropertyDeviceInfoStatusLableName");
				}
			}  
		}
	
		private const string s_sPropertyDeviceInfoStatusLableDeviceID = @"Device ID";
		private string m_sPropertyDeviceInfoStatusLableDeviceID=null;
		public string sPropertyDeviceInfoStatusLableDeviceID {
			get { 
				if( m_sPropertyDeviceInfoStatusLableDeviceID == null){
					return s_sPropertyDeviceInfoStatusLableDeviceID;
				}
				return m_sPropertyDeviceInfoStatusLableDeviceID; 
			}
			set { 
				if( value != m_sPropertyDeviceInfoStatusLableDeviceID){
					m_sPropertyDeviceInfoStatusLableDeviceID = value;
					NotifyPropertyChanged("sPropertyDeviceInfoStatusLableDeviceID");
				}
			}  
		}
	
		private const string s_sPropertyDeviceInfoStatusLableFirmware = @"Firmware";
		private string m_sPropertyDeviceInfoStatusLableFirmware=null;
		public string sPropertyDeviceInfoStatusLableFirmware {
			get { 
				if( m_sPropertyDeviceInfoStatusLableFirmware == null){
					return s_sPropertyDeviceInfoStatusLableFirmware;
				}
				return m_sPropertyDeviceInfoStatusLableFirmware; 
			}
			set { 
				if( value != m_sPropertyDeviceInfoStatusLableFirmware){
					m_sPropertyDeviceInfoStatusLableFirmware = value;
					NotifyPropertyChanged("sPropertyDeviceInfoStatusLableFirmware");
				}
			}  
		}
	
		private const string s_sPropertyDeviceInfoStatusLableHardware = @"Hardware";
		private string m_sPropertyDeviceInfoStatusLableHardware=null;
		public string sPropertyDeviceInfoStatusLableHardware {
			get { 
				if( m_sPropertyDeviceInfoStatusLableHardware == null){
					return s_sPropertyDeviceInfoStatusLableHardware;
				}
				return m_sPropertyDeviceInfoStatusLableHardware; 
			}
			set { 
				if( value != m_sPropertyDeviceInfoStatusLableHardware){
					m_sPropertyDeviceInfoStatusLableHardware = value;
					NotifyPropertyChanged("sPropertyDeviceInfoStatusLableHardware");
				}
			}  
		}
	
		private const string s_sPropertyDeviceInfoStatusLableVersion = @"Version";
		private string m_sPropertyDeviceInfoStatusLableVersion=null;
		public string sPropertyDeviceInfoStatusLableVersion {
			get { 
				if( m_sPropertyDeviceInfoStatusLableVersion == null){
					return s_sPropertyDeviceInfoStatusLableVersion;
				}
				return m_sPropertyDeviceInfoStatusLableVersion; 
			}
			set { 
				if( value != m_sPropertyDeviceInfoStatusLableVersion){
					m_sPropertyDeviceInfoStatusLableVersion = value;
					NotifyPropertyChanged("sPropertyDeviceInfoStatusLableVersion");
				}
			}  
		}
	
		private const string s_sPropertyDeviceInfoStatusLableIPAddr = @"IP address";
		private string m_sPropertyDeviceInfoStatusLableIPAddr=null;
		public string sPropertyDeviceInfoStatusLableIPAddr {
			get { 
				if( m_sPropertyDeviceInfoStatusLableIPAddr == null){
					return s_sPropertyDeviceInfoStatusLableIPAddr;
				}
				return m_sPropertyDeviceInfoStatusLableIPAddr; 
			}
			set { 
				if( value != m_sPropertyDeviceInfoStatusLableIPAddr){
					m_sPropertyDeviceInfoStatusLableIPAddr = value;
					NotifyPropertyChanged("sPropertyDeviceInfoStatusLableIPAddr");
				}
			}  
		}
	
		private const string s_sPropertyDeviceInfoStatusLableMACAddr = @"MAC address";
		private string m_sPropertyDeviceInfoStatusLableMACAddr=null;
		public string sPropertyDeviceInfoStatusLableMACAddr {
			get { 
				if( m_sPropertyDeviceInfoStatusLableMACAddr == null){
					return s_sPropertyDeviceInfoStatusLableMACAddr;
				}
				return m_sPropertyDeviceInfoStatusLableMACAddr; 
			}
			set { 
				if( value != m_sPropertyDeviceInfoStatusLableMACAddr){
					m_sPropertyDeviceInfoStatusLableMACAddr = value;
					NotifyPropertyChanged("sPropertyDeviceInfoStatusLableMACAddr");
				}
			}  
		}
	
		private const string s_sPropertyNetworkSettingsTitle = @"Network settings";
		private string m_sPropertyNetworkSettingsTitle=null;
		public string sPropertyNetworkSettingsTitle {
			get { 
				if( m_sPropertyNetworkSettingsTitle == null){
					return s_sPropertyNetworkSettingsTitle;
				}
				return m_sPropertyNetworkSettingsTitle; 
			}
			set { 
				if( value != m_sPropertyNetworkSettingsTitle){
					m_sPropertyNetworkSettingsTitle = value;
					NotifyPropertyChanged("sPropertyNetworkSettingsTitle");
				}
			}  
		}
	
		private const string s_sPropertyNetworkSettingsDHCP = @"DHCP";
		private string m_sPropertyNetworkSettingsDHCP=null;
		public string sPropertyNetworkSettingsDHCP {
			get { 
				if( m_sPropertyNetworkSettingsDHCP == null){
					return s_sPropertyNetworkSettingsDHCP;
				}
				return m_sPropertyNetworkSettingsDHCP; 
			}
			set { 
				if( value != m_sPropertyNetworkSettingsDHCP){
					m_sPropertyNetworkSettingsDHCP = value;
					NotifyPropertyChanged("sPropertyNetworkSettingsDHCP");
				}
			}  
		}
	
		private const string s_sPropertyNetworkSettingsIPaddr = @"Device IP address ";
		private string m_sPropertyNetworkSettingsIPaddr=null;
		public string sPropertyNetworkSettingsIPaddr {
			get { 
				if( m_sPropertyNetworkSettingsIPaddr == null){
					return s_sPropertyNetworkSettingsIPaddr;
				}
				return m_sPropertyNetworkSettingsIPaddr; 
			}
			set { 
				if( value != m_sPropertyNetworkSettingsIPaddr){
					m_sPropertyNetworkSettingsIPaddr = value;
					NotifyPropertyChanged("sPropertyNetworkSettingsIPaddr");
				}
			}  
		}
	
		private const string s_sPropertyNetworkSettingsSubnetMask = @"Subnet mask";
		private string m_sPropertyNetworkSettingsSubnetMask=null;
		public string sPropertyNetworkSettingsSubnetMask {
			get { 
				if( m_sPropertyNetworkSettingsSubnetMask == null){
					return s_sPropertyNetworkSettingsSubnetMask;
				}
				return m_sPropertyNetworkSettingsSubnetMask; 
			}
			set { 
				if( value != m_sPropertyNetworkSettingsSubnetMask){
					m_sPropertyNetworkSettingsSubnetMask = value;
					NotifyPropertyChanged("sPropertyNetworkSettingsSubnetMask");
				}
			}  
		}
	
		private const string s_sPropertyNetworkSettingsGateAddr = @"Gateway address";
		private string m_sPropertyNetworkSettingsGateAddr=null;
		public string sPropertyNetworkSettingsGateAddr {
			get { 
				if( m_sPropertyNetworkSettingsGateAddr == null){
					return s_sPropertyNetworkSettingsGateAddr;
				}
				return m_sPropertyNetworkSettingsGateAddr; 
			}
			set { 
				if( value != m_sPropertyNetworkSettingsGateAddr){
					m_sPropertyNetworkSettingsGateAddr = value;
					NotifyPropertyChanged("sPropertyNetworkSettingsGateAddr");
				}
			}  
		}
	
		private const string s_sPropertyNetworkSettingsDNSaddr = @"DNS address";
		private string m_sPropertyNetworkSettingsDNSaddr=null;
		public string sPropertyNetworkSettingsDNSaddr {
			get { 
				if( m_sPropertyNetworkSettingsDNSaddr == null){
					return s_sPropertyNetworkSettingsDNSaddr;
				}
				return m_sPropertyNetworkSettingsDNSaddr; 
			}
			set { 
				if( value != m_sPropertyNetworkSettingsDNSaddr){
					m_sPropertyNetworkSettingsDNSaddr = value;
					NotifyPropertyChanged("sPropertyNetworkSettingsDNSaddr");
				}
			}  
		}
	
		private const string s_sPropertyNetworkSettingsMACaddr = @"MAC address";
		private string m_sPropertyNetworkSettingsMACaddr=null;
		public string sPropertyNetworkSettingsMACaddr {
			get { 
				if( m_sPropertyNetworkSettingsMACaddr == null){
					return s_sPropertyNetworkSettingsMACaddr;
				}
				return m_sPropertyNetworkSettingsMACaddr; 
			}
			set { 
				if( value != m_sPropertyNetworkSettingsMACaddr){
					m_sPropertyNetworkSettingsMACaddr = value;
					NotifyPropertyChanged("sPropertyNetworkSettingsMACaddr");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOTitle = @"Digital IO";
		private string m_sPropertyDigitalIOTitle=null;
		public string sPropertyDigitalIOTitle {
			get { 
				if( m_sPropertyDigitalIOTitle == null){
					return s_sPropertyDigitalIOTitle;
				}
				return m_sPropertyDigitalIOTitle; 
			}
			set { 
				if( value != m_sPropertyDigitalIOTitle){
					m_sPropertyDigitalIOTitle = value;
					NotifyPropertyChanged("sPropertyDigitalIOTitle");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOInputName = @"Name";
		private string m_sPropertyDigitalIOInputName=null;
		public string sPropertyDigitalIOInputName {
			get { 
				if( m_sPropertyDigitalIOInputName == null){
					return s_sPropertyDigitalIOInputName;
				}
				return m_sPropertyDigitalIOInputName; 
			}
			set { 
				if( value != m_sPropertyDigitalIOInputName){
					m_sPropertyDigitalIOInputName = value;
					NotifyPropertyChanged("sPropertyDigitalIOInputName");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOInputNormalStatus = @"Normal status";
		private string m_sPropertyDigitalIOInputNormalStatus=null;
		public string sPropertyDigitalIOInputNormalStatus {
			get { 
				if( m_sPropertyDigitalIOInputNormalStatus == null){
					return s_sPropertyDigitalIOInputNormalStatus;
				}
				return m_sPropertyDigitalIOInputNormalStatus; 
			}
			set { 
				if( value != m_sPropertyDigitalIOInputNormalStatus){
					m_sPropertyDigitalIOInputNormalStatus = value;
					NotifyPropertyChanged("sPropertyDigitalIOInputNormalStatus");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOInputCurrentStatus = @"Current status";
		private string m_sPropertyDigitalIOInputCurrentStatus=null;
		public string sPropertyDigitalIOInputCurrentStatus {
			get { 
				if( m_sPropertyDigitalIOInputCurrentStatus == null){
					return s_sPropertyDigitalIOInputCurrentStatus;
				}
				return m_sPropertyDigitalIOInputCurrentStatus; 
			}
			set { 
				if( value != m_sPropertyDigitalIOInputCurrentStatus){
					m_sPropertyDigitalIOInputCurrentStatus = value;
					NotifyPropertyChanged("sPropertyDigitalIOInputCurrentStatus");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOOutputName = @"Name";
		private string m_sPropertyDigitalIOOutputName=null;
		public string sPropertyDigitalIOOutputName {
			get { 
				if( m_sPropertyDigitalIOOutputName == null){
					return s_sPropertyDigitalIOOutputName;
				}
				return m_sPropertyDigitalIOOutputName; 
			}
			set { 
				if( value != m_sPropertyDigitalIOOutputName){
					m_sPropertyDigitalIOOutputName = value;
					NotifyPropertyChanged("sPropertyDigitalIOOutputName");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOOutputIdleStatus = @"Idle status";
		private string m_sPropertyDigitalIOOutputIdleStatus=null;
		public string sPropertyDigitalIOOutputIdleStatus {
			get { 
				if( m_sPropertyDigitalIOOutputIdleStatus == null){
					return s_sPropertyDigitalIOOutputIdleStatus;
				}
				return m_sPropertyDigitalIOOutputIdleStatus; 
			}
			set { 
				if( value != m_sPropertyDigitalIOOutputIdleStatus){
					m_sPropertyDigitalIOOutputIdleStatus = value;
					NotifyPropertyChanged("sPropertyDigitalIOOutputIdleStatus");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOOutputCurrentStatus = @"Current status";
		private string m_sPropertyDigitalIOOutputCurrentStatus=null;
		public string sPropertyDigitalIOOutputCurrentStatus {
			get { 
				if( m_sPropertyDigitalIOOutputCurrentStatus == null){
					return s_sPropertyDigitalIOOutputCurrentStatus;
				}
				return m_sPropertyDigitalIOOutputCurrentStatus; 
			}
			set { 
				if( value != m_sPropertyDigitalIOOutputCurrentStatus){
					m_sPropertyDigitalIOOutputCurrentStatus = value;
					NotifyPropertyChanged("sPropertyDigitalIOOutputCurrentStatus");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOInputs = @"Digital inputs";
		private string m_sPropertyDigitalIOInputs=null;
		public string sPropertyDigitalIOInputs {
			get { 
				if( m_sPropertyDigitalIOInputs == null){
					return s_sPropertyDigitalIOInputs;
				}
				return m_sPropertyDigitalIOInputs; 
			}
			set { 
				if( value != m_sPropertyDigitalIOInputs){
					m_sPropertyDigitalIOInputs = value;
					NotifyPropertyChanged("sPropertyDigitalIOInputs");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOOutputs = @"Digital outputs (relays)";
		private string m_sPropertyDigitalIOOutputs=null;
		public string sPropertyDigitalIOOutputs {
			get { 
				if( m_sPropertyDigitalIOOutputs == null){
					return s_sPropertyDigitalIOOutputs;
				}
				return m_sPropertyDigitalIOOutputs; 
			}
			set { 
				if( value != m_sPropertyDigitalIOOutputs){
					m_sPropertyDigitalIOOutputs = value;
					NotifyPropertyChanged("sPropertyDigitalIOOutputs");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOEventAction = @"Event action";
		private string m_sPropertyDigitalIOEventAction=null;
		public string sPropertyDigitalIOEventAction {
			get { 
				if( m_sPropertyDigitalIOEventAction == null){
					return s_sPropertyDigitalIOEventAction;
				}
				return m_sPropertyDigitalIOEventAction; 
			}
			set { 
				if( value != m_sPropertyDigitalIOEventAction){
					m_sPropertyDigitalIOEventAction = value;
					NotifyPropertyChanged("sPropertyDigitalIOEventAction");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOEventSendONVIFmessage = @"Send ONVIF message";
		private string m_sPropertyDigitalIOEventSendONVIFmessage=null;
		public string sPropertyDigitalIOEventSendONVIFmessage {
			get { 
				if( m_sPropertyDigitalIOEventSendONVIFmessage == null){
					return s_sPropertyDigitalIOEventSendONVIFmessage;
				}
				return m_sPropertyDigitalIOEventSendONVIFmessage; 
			}
			set { 
				if( value != m_sPropertyDigitalIOEventSendONVIFmessage){
					m_sPropertyDigitalIOEventSendONVIFmessage = value;
					NotifyPropertyChanged("sPropertyDigitalIOEventSendONVIFmessage");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOTriggerRelay = @"Trigger relay";
		private string m_sPropertyDigitalIOTriggerRelay=null;
		public string sPropertyDigitalIOTriggerRelay {
			get { 
				if( m_sPropertyDigitalIOTriggerRelay == null){
					return s_sPropertyDigitalIOTriggerRelay;
				}
				return m_sPropertyDigitalIOTriggerRelay; 
			}
			set { 
				if( value != m_sPropertyDigitalIOTriggerRelay){
					m_sPropertyDigitalIOTriggerRelay = value;
					NotifyPropertyChanged("sPropertyDigitalIOTriggerRelay");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIORecordChannel = @"Record channel";
		private string m_sPropertyDigitalIORecordChannel=null;
		public string sPropertyDigitalIORecordChannel {
			get { 
				if( m_sPropertyDigitalIORecordChannel == null){
					return s_sPropertyDigitalIORecordChannel;
				}
				return m_sPropertyDigitalIORecordChannel; 
			}
			set { 
				if( value != m_sPropertyDigitalIORecordChannel){
					m_sPropertyDigitalIORecordChannel = value;
					NotifyPropertyChanged("sPropertyDigitalIORecordChannel");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOSwitchAnalogue = @"Switch analogue video on";
		private string m_sPropertyDigitalIOSwitchAnalogue=null;
		public string sPropertyDigitalIOSwitchAnalogue {
			get { 
				if( m_sPropertyDigitalIOSwitchAnalogue == null){
					return s_sPropertyDigitalIOSwitchAnalogue;
				}
				return m_sPropertyDigitalIOSwitchAnalogue; 
			}
			set { 
				if( value != m_sPropertyDigitalIOSwitchAnalogue){
					m_sPropertyDigitalIOSwitchAnalogue = value;
					NotifyPropertyChanged("sPropertyDigitalIOSwitchAnalogue");
				}
			}  
		}
	
		private const string s_sPropertyDigitalIOButtonTriggerRelay = @"Trigger relay";
		private string m_sPropertyDigitalIOButtonTriggerRelay=null;
		public string sPropertyDigitalIOButtonTriggerRelay {
			get { 
				if( m_sPropertyDigitalIOButtonTriggerRelay == null){
					return s_sPropertyDigitalIOButtonTriggerRelay;
				}
				return m_sPropertyDigitalIOButtonTriggerRelay; 
			}
			set { 
				if( value != m_sPropertyDigitalIOButtonTriggerRelay){
					m_sPropertyDigitalIOButtonTriggerRelay = value;
					NotifyPropertyChanged("sPropertyDigitalIOButtonTriggerRelay");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceTitle = @"Maintenance";
		private string m_sPropertyMaintenanceTitle=null;
		public string sPropertyMaintenanceTitle {
			get { 
				if( m_sPropertyMaintenanceTitle == null){
					return s_sPropertyMaintenanceTitle;
				}
				return m_sPropertyMaintenanceTitle; 
			}
			set { 
				if( value != m_sPropertyMaintenanceTitle){
					m_sPropertyMaintenanceTitle = value;
					NotifyPropertyChanged("sPropertyMaintenanceTitle");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceConfiguration = @"Configuration";
		private string m_sPropertyMaintenanceConfiguration=null;
		public string sPropertyMaintenanceConfiguration {
			get { 
				if( m_sPropertyMaintenanceConfiguration == null){
					return s_sPropertyMaintenanceConfiguration;
				}
				return m_sPropertyMaintenanceConfiguration; 
			}
			set { 
				if( value != m_sPropertyMaintenanceConfiguration){
					m_sPropertyMaintenanceConfiguration = value;
					NotifyPropertyChanged("sPropertyMaintenanceConfiguration");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceBTNBackup = @"Backup";
		private string m_sPropertyMaintenanceBTNBackup=null;
		public string sPropertyMaintenanceBTNBackup {
			get { 
				if( m_sPropertyMaintenanceBTNBackup == null){
					return s_sPropertyMaintenanceBTNBackup;
				}
				return m_sPropertyMaintenanceBTNBackup; 
			}
			set { 
				if( value != m_sPropertyMaintenanceBTNBackup){
					m_sPropertyMaintenanceBTNBackup = value;
					NotifyPropertyChanged("sPropertyMaintenanceBTNBackup");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceBTNRestore = @"Restore";
		private string m_sPropertyMaintenanceBTNRestore=null;
		public string sPropertyMaintenanceBTNRestore {
			get { 
				if( m_sPropertyMaintenanceBTNRestore == null){
					return s_sPropertyMaintenanceBTNRestore;
				}
				return m_sPropertyMaintenanceBTNRestore; 
			}
			set { 
				if( value != m_sPropertyMaintenanceBTNRestore){
					m_sPropertyMaintenanceBTNRestore = value;
					NotifyPropertyChanged("sPropertyMaintenanceBTNRestore");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceFactoryReset = @"Factory reset";
		private string m_sPropertyMaintenanceFactoryReset=null;
		public string sPropertyMaintenanceFactoryReset {
			get { 
				if( m_sPropertyMaintenanceFactoryReset == null){
					return s_sPropertyMaintenanceFactoryReset;
				}
				return m_sPropertyMaintenanceFactoryReset; 
			}
			set { 
				if( value != m_sPropertyMaintenanceFactoryReset){
					m_sPropertyMaintenanceFactoryReset = value;
					NotifyPropertyChanged("sPropertyMaintenanceFactoryReset");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceBTNSoftReset = @"Soft reset";
		private string m_sPropertyMaintenanceBTNSoftReset=null;
		public string sPropertyMaintenanceBTNSoftReset {
			get { 
				if( m_sPropertyMaintenanceBTNSoftReset == null){
					return s_sPropertyMaintenanceBTNSoftReset;
				}
				return m_sPropertyMaintenanceBTNSoftReset; 
			}
			set { 
				if( value != m_sPropertyMaintenanceBTNSoftReset){
					m_sPropertyMaintenanceBTNSoftReset = value;
					NotifyPropertyChanged("sPropertyMaintenanceBTNSoftReset");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceBTNHardReset = @"Hard reset";
		private string m_sPropertyMaintenanceBTNHardReset=null;
		public string sPropertyMaintenanceBTNHardReset {
			get { 
				if( m_sPropertyMaintenanceBTNHardReset == null){
					return s_sPropertyMaintenanceBTNHardReset;
				}
				return m_sPropertyMaintenanceBTNHardReset; 
			}
			set { 
				if( value != m_sPropertyMaintenanceBTNHardReset){
					m_sPropertyMaintenanceBTNHardReset = value;
					NotifyPropertyChanged("sPropertyMaintenanceBTNHardReset");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceDiagnostics = @"Diagnostics and support";
		private string m_sPropertyMaintenanceDiagnostics=null;
		public string sPropertyMaintenanceDiagnostics {
			get { 
				if( m_sPropertyMaintenanceDiagnostics == null){
					return s_sPropertyMaintenanceDiagnostics;
				}
				return m_sPropertyMaintenanceDiagnostics; 
			}
			set { 
				if( value != m_sPropertyMaintenanceDiagnostics){
					m_sPropertyMaintenanceDiagnostics = value;
					NotifyPropertyChanged("sPropertyMaintenanceDiagnostics");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceBTNDownloadDump = @"Download Dump";
		private string m_sPropertyMaintenanceBTNDownloadDump=null;
		public string sPropertyMaintenanceBTNDownloadDump {
			get { 
				if( m_sPropertyMaintenanceBTNDownloadDump == null){
					return s_sPropertyMaintenanceBTNDownloadDump;
				}
				return m_sPropertyMaintenanceBTNDownloadDump; 
			}
			set { 
				if( value != m_sPropertyMaintenanceBTNDownloadDump){
					m_sPropertyMaintenanceBTNDownloadDump = value;
					NotifyPropertyChanged("sPropertyMaintenanceBTNDownloadDump");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceFirmwareVer = @"Current firmware version";
		private string m_sPropertyMaintenanceFirmwareVer=null;
		public string sPropertyMaintenanceFirmwareVer {
			get { 
				if( m_sPropertyMaintenanceFirmwareVer == null){
					return s_sPropertyMaintenanceFirmwareVer;
				}
				return m_sPropertyMaintenanceFirmwareVer; 
			}
			set { 
				if( value != m_sPropertyMaintenanceFirmwareVer){
					m_sPropertyMaintenanceFirmwareVer = value;
					NotifyPropertyChanged("sPropertyMaintenanceFirmwareVer");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceUpgrateFirmware = @"Upgrade firmware";
		private string m_sPropertyMaintenanceUpgrateFirmware=null;
		public string sPropertyMaintenanceUpgrateFirmware {
			get { 
				if( m_sPropertyMaintenanceUpgrateFirmware == null){
					return s_sPropertyMaintenanceUpgrateFirmware;
				}
				return m_sPropertyMaintenanceUpgrateFirmware; 
			}
			set { 
				if( value != m_sPropertyMaintenanceUpgrateFirmware){
					m_sPropertyMaintenanceUpgrateFirmware = value;
					NotifyPropertyChanged("sPropertyMaintenanceUpgrateFirmware");
				}
			}  
		}
	
		private const string s_sPropertyMaintenanceBTNUpgrate = @"Upgrate";
		private string m_sPropertyMaintenanceBTNUpgrate=null;
		public string sPropertyMaintenanceBTNUpgrate {
			get { 
				if( m_sPropertyMaintenanceBTNUpgrate == null){
					return s_sPropertyMaintenanceBTNUpgrate;
				}
				return m_sPropertyMaintenanceBTNUpgrate; 
			}
			set { 
				if( value != m_sPropertyMaintenanceBTNUpgrate){
					m_sPropertyMaintenanceBTNUpgrate = value;
					NotifyPropertyChanged("sPropertyMaintenanceBTNUpgrate");
				}
			}  
		}
	
		private const string s_sPropertyLiveVideoTitle = @"Live video";
		private string m_sPropertyLiveVideoTitle=null;
		public string sPropertyLiveVideoTitle {
			get { 
				if( m_sPropertyLiveVideoTitle == null){
					return s_sPropertyLiveVideoTitle;
				}
				return m_sPropertyLiveVideoTitle; 
			}
			set { 
				if( value != m_sPropertyLiveVideoTitle){
					m_sPropertyLiveVideoTitle = value;
					NotifyPropertyChanged("sPropertyLiveVideoTitle");
				}
			}  
		}
	
		private const string s_sPropertyEventsTitle = @"Events";
		private string m_sPropertyEventsTitle=null;
		public string sPropertyEventsTitle {
			get { 
				if( m_sPropertyEventsTitle == null){
					return s_sPropertyEventsTitle;
				}
				return m_sPropertyEventsTitle; 
			}
			set { 
				if( value != m_sPropertyEventsTitle){
					m_sPropertyEventsTitle = value;
					NotifyPropertyChanged("sPropertyEventsTitle");
				}
			}  
		}
	
		private const string s_sPropertyEventsColumnID = @"Rule ID";
		private string m_sPropertyEventsColumnID=null;
		public string sPropertyEventsColumnID {
			get { 
				if( m_sPropertyEventsColumnID == null){
					return s_sPropertyEventsColumnID;
				}
				return m_sPropertyEventsColumnID; 
			}
			set { 
				if( value != m_sPropertyEventsColumnID){
					m_sPropertyEventsColumnID = value;
					NotifyPropertyChanged("sPropertyEventsColumnID");
				}
			}  
		}
	
		private const string s_sPropertyEventsColumnDate = @"Date/Time";
		private string m_sPropertyEventsColumnDate=null;
		public string sPropertyEventsColumnDate {
			get { 
				if( m_sPropertyEventsColumnDate == null){
					return s_sPropertyEventsColumnDate;
				}
				return m_sPropertyEventsColumnDate; 
			}
			set { 
				if( value != m_sPropertyEventsColumnDate){
					m_sPropertyEventsColumnDate = value;
					NotifyPropertyChanged("sPropertyEventsColumnDate");
				}
			}  
		}
	
		private const string s_sPropertyEventsColumnType = @"Type";
		private string m_sPropertyEventsColumnType=null;
		public string sPropertyEventsColumnType {
			get { 
				if( m_sPropertyEventsColumnType == null){
					return s_sPropertyEventsColumnType;
				}
				return m_sPropertyEventsColumnType; 
			}
			set { 
				if( value != m_sPropertyEventsColumnType){
					m_sPropertyEventsColumnType = value;
					NotifyPropertyChanged("sPropertyEventsColumnType");
				}
			}  
		}
	
		private const string s_sPropertyEventsColumnDetails = @"Details";
		private string m_sPropertyEventsColumnDetails=null;
		public string sPropertyEventsColumnDetails {
			get { 
				if( m_sPropertyEventsColumnDetails == null){
					return s_sPropertyEventsColumnDetails;
				}
				return m_sPropertyEventsColumnDetails; 
			}
			set { 
				if( value != m_sPropertyEventsColumnDetails){
					m_sPropertyEventsColumnDetails = value;
					NotifyPropertyChanged("sPropertyEventsColumnDetails");
				}
			}  
		}
	
		private const string s_sPropertyVideoStreamingTitle = @"Video streaming";
		private string m_sPropertyVideoStreamingTitle=null;
		public string sPropertyVideoStreamingTitle {
			get { 
				if( m_sPropertyVideoStreamingTitle == null){
					return s_sPropertyVideoStreamingTitle;
				}
				return m_sPropertyVideoStreamingTitle; 
			}
			set { 
				if( value != m_sPropertyVideoStreamingTitle){
					m_sPropertyVideoStreamingTitle = value;
					NotifyPropertyChanged("sPropertyVideoStreamingTitle");
				}
			}  
		}
	
		private const string s_sPropertyVideoStreamingResolution = @"Resolution, pixels";
		private string m_sPropertyVideoStreamingResolution=null;
		public string sPropertyVideoStreamingResolution {
			get { 
				if( m_sPropertyVideoStreamingResolution == null){
					return s_sPropertyVideoStreamingResolution;
				}
				return m_sPropertyVideoStreamingResolution; 
			}
			set { 
				if( value != m_sPropertyVideoStreamingResolution){
					m_sPropertyVideoStreamingResolution = value;
					NotifyPropertyChanged("sPropertyVideoStreamingResolution");
				}
			}  
		}
	
		private const string s_sPropertyVideoStreamingFrameRate = @"Frame rate, fps";
		private string m_sPropertyVideoStreamingFrameRate=null;
		public string sPropertyVideoStreamingFrameRate {
			get { 
				if( m_sPropertyVideoStreamingFrameRate == null){
					return s_sPropertyVideoStreamingFrameRate;
				}
				return m_sPropertyVideoStreamingFrameRate; 
			}
			set { 
				if( value != m_sPropertyVideoStreamingFrameRate){
					m_sPropertyVideoStreamingFrameRate = value;
					NotifyPropertyChanged("sPropertyVideoStreamingFrameRate");
				}
			}  
		}
	
		private const string s_sPropertyVideoStreamingEncoder = @"Encoder";
		private string m_sPropertyVideoStreamingEncoder=null;
		public string sPropertyVideoStreamingEncoder {
			get { 
				if( m_sPropertyVideoStreamingEncoder == null){
					return s_sPropertyVideoStreamingEncoder;
				}
				return m_sPropertyVideoStreamingEncoder; 
			}
			set { 
				if( value != m_sPropertyVideoStreamingEncoder){
					m_sPropertyVideoStreamingEncoder = value;
					NotifyPropertyChanged("sPropertyVideoStreamingEncoder");
				}
			}  
		}
	
		private const string s_sPropertyVideoStreamingBitrate = @"Target bitrate, kbps";
		private string m_sPropertyVideoStreamingBitrate=null;
		public string sPropertyVideoStreamingBitrate {
			get { 
				if( m_sPropertyVideoStreamingBitrate == null){
					return s_sPropertyVideoStreamingBitrate;
				}
				return m_sPropertyVideoStreamingBitrate; 
			}
			set { 
				if( value != m_sPropertyVideoStreamingBitrate){
					m_sPropertyVideoStreamingBitrate = value;
					NotifyPropertyChanged("sPropertyVideoStreamingBitrate");
				}
			}  
		}
	
		private const string s_sPropertyVideoStreamingPrioriy = @"Priority";
		private string m_sPropertyVideoStreamingPrioriy=null;
		public string sPropertyVideoStreamingPrioriy {
			get { 
				if( m_sPropertyVideoStreamingPrioriy == null){
					return s_sPropertyVideoStreamingPrioriy;
				}
				return m_sPropertyVideoStreamingPrioriy; 
			}
			set { 
				if( value != m_sPropertyVideoStreamingPrioriy){
					m_sPropertyVideoStreamingPrioriy = value;
					NotifyPropertyChanged("sPropertyVideoStreamingPrioriy");
				}
			}  
		}
	
	}

}    

	