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
using nvc.onvif;
using nvc.models;
using System.Drawing;

namespace nvc.entities {
	public class DeviceModel {
		public bool IsDisposed { get; set; }
		public DeviceModel() {
			IsDisposed = false;
		}
		public void ReleaseAll() {
			IsDisposed = true;

			if (ChannelsList != null)
				foreach (var value in ChannelsList)
					value.Value.ReleaseAll();
			UnsubscribeAll();
		}
		#region IDeviceAndChannels
		protected Image DeviceImage { get { return Image.FromFile(@"Resources\Images\device.jpg"); } }
		protected Image EventImageThumbnail { get { return Image.FromFile(@"Resources\Images\scr.jpg"); } }
		protected Image EventImage { get { return Image.FromFile(@"Resources\Images\scr_big.jpg"); } }
		public Image GetDeviceImage() {
			return DeviceImage;
		}

		public EventDescriptor GetChannelsLastEvent() {
			return GetChannelsLastEvent(GetCurrentChannel().Name);
		}
		public EventDescriptor GetChannelsLastEvent(string channelname) {
			var eventdescr = new EventDescriptor();
			eventdescr.Details = "Details";
			eventdescr.Image = EventImage;
			eventdescr.Thumbnail = EventImageThumbnail;
			eventdescr.TimeDate = DateTime.Now;
			eventdescr.RuleID = 1;
			eventdescr.Type = "Intruder";
			return eventdescr;
		}

		protected string _currentChannelID;
		protected Dictionary<string, DeviceChannel> _channelsList;
		public Dictionary<string, DeviceChannel> ChannelsList {
			get {
				if (_channelsList == null)
					_channelsList = new Dictionary<string, DeviceChannel>();
				return _channelsList;
			}
		}

		public event EventHandler ChannelsInitialised;
		public void RiseChannelsInitialisedEvent() {
			if (ChannelsInitialised != null)
				ChannelsInitialised(this, new EventArgs());
		}

		public DeviceChannel GetCurrentChannel() {
			return _channelsList[_currentChannelID];
		}
		public void SetCurrentChannel(string id) {
			_currentChannelID = id;
		}
		public void AddChannel(DeviceChannel devCh) {
			ChannelsList.Add(devCh.Id, devCh);
		}
		public int GetChannelsCount() { return ChannelsList.Count; }
		#endregion IDeviceAndChannels

		#region IDisposable
		private bool disposed = false;

		public void Dispose() {
			UnsubscribeAll();
			disposed = true;
		}

		protected void UnsubscribeAll() {
		}

		#endregion IDisposable

		private void InitFakeCapabilities() {
			_deviceCapabilities = new List<int>();
			_deviceCapabilities.Add(Defaults.constLinkButtonAntishakerID);
			_deviceCapabilities.Add(Defaults.constLinkButtonDepthCalibrationID);
			_deviceCapabilities.Add(Defaults.constLinkButtonDisplayAnnotationID);
			_deviceCapabilities.Add(Defaults.constLinkButtonEventsID);
			_deviceCapabilities.Add(Defaults.constLinkButtonIdentificationAndStatusID);
			_deviceCapabilities.Add(Defaults.constLinkButtonLiveVideoID);
			_deviceCapabilities.Add(Defaults.constLinkButtonMaintenanceID);
			_deviceCapabilities.Add(Defaults.constLinkButtonNetworkSettingsID);
			_deviceCapabilities.Add(Defaults.constLinkButtonObjectTrackerID);
			_deviceCapabilities.Add(Defaults.constLinkButtonDigitalIOID);
			_deviceCapabilities.Add(Defaults.constLinkButtonRotationID);
			_deviceCapabilities.Add(Defaults.constLinkButtonTamperingDetectorsID);
			_deviceCapabilities.Add(Defaults.constLinkButtonVideoStreamingID);
		}
		public List<int> _deviceCapabilities;
		public List<int> DeviceCapabilities {
			get {
				if (_deviceCapabilities == null) {
					_deviceCapabilities = new List<int>();
				}
				InitFakeCapabilities();
				return _deviceCapabilities;
			}
		}

		#region AKModels
		public event EventHandler NetworkSettingsInitialised;
		public event EventHandler IdentificationInitialised;

		public bool IsPropertyIdentificationReady {
			get {
				return IsNetworkStatusLoaded;
			}
		}
		public bool IsPropertyNetworkSettingsReady {
			get {
				return IsNetworkStatusLoaded && IsNetworkSettingsLoaded;
			}
		}
		void CheckIdentificationCompleteness() {
			if (IsNetworkStatusLoaded)
				if (IdentificationInitialised != null)
					IdentificationInitialised(this, null);
		}
		void CheckNetworkSettingsCompleteness() {
			if (IsNetworkStatusLoaded && IsNetworkSettingsLoaded)
				if (NetworkSettingsInitialised != null)
					NetworkSettingsInitialised(this, null);
		}

		#region Network Status
		NetworkStatus _networkStatusTemp;
		NetworkStatus NetworkStatusTemp {
			get {
				if (_networkStatusTemp == null)
					_networkStatusTemp = NetworkStatus;
				return _networkStatusTemp;
			}
			set { _networkStatusTemp = value; }
		}
		NetworkStatus _networkStatus;
		NetworkStatus NetworkStatus {
			get { return _networkStatus; }
			set {
				_networkStatus = value;
				IsNetworkStatusLoaded = true;
				CheckNetworkSettingsCompleteness();
				CheckIdentificationCompleteness();
			}
		}
		public NetworkStatus GetModelNetworkStatus() {
			return NetworkStatusTemp;
		}
		public void SetModelNetworkStatus(NetworkStatus netStat) {
			NetworkStatus = netStat;
		}
		public void ResetModelNetworkStatus() {
			NetworkStatusTemp = NetworkStatus;
		}
		#endregion Network Status

		#region Network Settings
		protected bool IsNetworkStatusLoaded = false;
		NetworkSettings _networkSettingsTemp;
		NetworkSettings NetworkSettingsTemp {
			get {
				if (_networkSettingsTemp == null)
					_networkSettingsTemp = NetworkSettings;
				return _networkSettingsTemp;
			}
			set {
				_networkSettingsTemp = value;
			}
		}
		NetworkSettings _networkSettings;
		NetworkSettings NetworkSettings {
			get { return _networkSettings; }
			set {
				_networkSettings = value;
				IsNetworkSettingsLoaded = true;
				CheckNetworkSettingsCompleteness();
			}
		}
		public NetworkSettings GetModelNetworkSettings() {
			return NetworkSettingsTemp;
		}
		public void SetModelNetworkSettings(NetworkSettings netSet) {
			NetworkSettings = netSet;
		}
		public void ResetModelNetworkSettings() {
			NetworkSettingsTemp = NetworkSettings;
		}
		protected bool IsNetworkSettingsLoaded = false;
		#endregion Network Settings

		#endregion AKModels

		#region Tokens;
		public string TokenNetworkInterface;
		#endregion

		public DeviceDescription devDescr { get; set; }
		public string Manufacturer { get; set; }

		// IP address
		public string IPAddress { get; set; }
		// Name
		protected string _name = null;
		public string Name {
			get { return _name; }
			set {
				_name = value;
			}
		}
		public string GetDeviceName() {
			return Name;
		}
		public void SetDeviceName(string value) {
			Name = value;
		}
		//Firmware
		protected string _firmware;
		public string Firmware {
			get { return _firmware; }
			set {
				_firmware = value;
			}
		}
		public string GetDeviceFirmware() {
			return Firmware;
		}
		//DeviceID
		protected string _deviceId;
		public string DeviceId {
			get { return _deviceId; }
			set {
				_deviceId = value;
			}
		}
		public string GetDeviceId() {
			return DeviceId;
		}

		//Hardware
		protected string _hardware = null;
		public string Hardware {
			get { return _hardware; }
			set {
				_hardware = value;
			}
		}
		public string GetDeviceHardware() {
			return Hardware;
		}
		//Hardware version
		public string HardwareVersion { get; set; }
		public string GetDeviceHardwareVersion() {
			return HardwareVersion;
		}
		//Firmware version
		public string FirmwareVersion { get; set; }
		public string GetDeviceFirmwareVersion() {
			return FirmwareVersion;
		}
		//Subnet mask
		public static String PrefixToMask(int prefix) {
			if ((prefix <= 0) || (prefix > 32))
				return null;

			String retValue = "";

			uint mask = 0xFFFFFFFF;
			prefix = 32 - prefix;

			mask <<= prefix;

			for (int i = 0; i < 4; i++) {
				byte lastbyte = (byte)(mask & 0x000000FF);

				retValue = lastbyte.ToString() + retValue;
				if (i < 3)
					retValue = "." + retValue;
				mask >>= 8;
			}

			return retValue;
		}
		public static int MaskToPrefix(String mask) {
			String[] maskbytes = mask.Split(".".ToCharArray());
			if (maskbytes.Length != 4)
				return -1;

			uint intmask = 0;

			for (int i = 0; i < 4; i++) {
				intmask |= (uint)(Convert.ToByte(maskbytes[i]) << (8 * (3 - i)));
			}

			int prefix = 32;

			while ((intmask & 1) == 0) {
				intmask >>= 1;
				prefix--;
			}

			return prefix;
		}
	}
}
