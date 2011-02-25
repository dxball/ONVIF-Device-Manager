using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Concurrency;
using System.Threading;

using onvif.services.device;
using onvif.types;
using tt=onvif.types;

using odm.onvif;
using odm.utils;

namespace odm.models {
	public partial class DeviceIdentificationModel : ModelBase<DeviceIdentificationModel> {
		public DeviceIdentificationModel() {
			
		}
		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<DeviceIdentificationModel> observer) {
			DeviceInfo info = null;
			NetworkStatus netstat = null;
			DeviceObservable device = null;
			SystemDateTime time = null;
			Scope[] scopes = null;
			
			yield return session.GetDeviceClient().Handle(x => device = x);
			yield return Observable.Merge(
				session.GetDeviceInfo().Handle(x => info = x),
				session.GetNetworkStatus().Handle(x => netstat = x),
				device.GetSystemDateAndTime().Handle(x => time = x),
				session.GetScopes().Handle(x => scopes = x)
			);
			
			dbg.Assert(info != null);
			dbg.Assert(netstat != null);

			m_name.SetBoth(info.Name);
			m_location.SetBoth(NvcHelper.GetLocation(scopes.Select(x=>x.ScopeItem)));
			
			NotifyPropertyChanged(x => x.location);
			NotifyPropertyChanged(x => x.Name);
			
			HardwareVer = info.HardwareId;
			FirmwareVer = info.FirmwareVersion;
			DeviceID = info.SerialNumber;
			NetworkIPAddress = netstat.ip.ToString();
			MACAddress = BitConverter.ToString(netstat.mac.GetAddressBytes());
			//m_dateTime.SetBoth(System.TimeZone.CurrentTimeZone.ToLocalTime(
			//    new System.DateTime(time.UTCDateTime.Date.Year, time.UTCDateTime.Date.Month, time.UTCDateTime.Date.Day, time.UTCDateTime.Time.Hour, time.UTCDateTime.Time.Minute, time.UTCDateTime.Time.Second, DateTimeKind.Utc)
			//));
			m_dateTime.SetBoth(
				new System.DateTime(time.UTCDateTime.Date.Year, time.UTCDateTime.Date.Month, time.UTCDateTime.Date.Day, time.UTCDateTime.Time.Hour, time.UTCDateTime.Time.Minute, time.UTCDateTime.Time.Second, DateTimeKind.Utc)
			);
			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public override void RevertChanges() {
			m_name.Revert();
			m_dateTime.Revert();
			NotifyPropertyChanged(x => x.Name);
			NotifyPropertyChanged(x => x.dateTime);
			NotifyPropertyChanged(x => x.isModified);
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<DeviceIdentificationModel> observer) {
			DeviceObservable device = null;
			SystemDateTime time = null;

			if (m_location.isModified) {
				yield return session.SetLocation(m_location.current).Idle();
			}
			
			if (m_name.isModified) {
				yield return session.SetName(m_name.current).Idle();
			}

			if (m_dateTime.isModified) {
				yield return session.GetDeviceClient().Handle(x => device = x);
				yield return device.GetSystemDateAndTime().Handle(x => time = x);
				//var t = System.TimeZone.CurrentTimeZone.ToUniversalTime(dateTime);
				var t = dateTime;
				var utcTime = new tt::DateTime();
				utcTime.Date = new tt::Date();
				utcTime.Date.Year = t.Year;
				utcTime.Date.Month = t.Month;
				utcTime.Date.Day = t.Day;
				utcTime.Time = new tt::Time();
				utcTime.Time.Hour = t.Hour;
				utcTime.Time.Minute = t.Minute;
				utcTime.Time.Second = t.Second;
				yield return device.SetSystemDateAndTime(SetDateTimeType.Manual, time.DaylightSavings, time.TimeZone, utcTime).Idle();
			}

			yield return Observable.Concat(LoadImpl(session, observer)).Idle();
			//if (observer != null) {
			//    observer.OnNext(this);
			//}
		}


		private ChangeTrackingProperty<string> m_name = new ChangeTrackingProperty<string>();
		private ChangeTrackingProperty<string> m_location = new ChangeTrackingProperty<string>();
		private ChangeTrackingProperty<System.DateTime> m_dateTime = new ChangeTrackingProperty<System.DateTime>();
		
		private string m_HardwareVer;
		private string m_FirmwareVer;
		private string m_DeviceID;
		private string m_NetworkIPAddress;
		private string m_MACAddress;
		//private System.DateTime m_dateTime;

		public string Name {
			get {

				return m_name.current;
			}
			set {
				if (m_name.current != value) {
					m_name.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.Name);
				}
			}
		}
		public string location {
			get {

				return m_location.current;
			}
			set {
				if (m_location.current != value) {
					m_location.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.location);
				}
			}
		}
		public string HardwareVer {
			get {
				return m_HardwareVer;
			}
			private set {
				if (m_HardwareVer != value) {
					m_HardwareVer = value;
					NotifyPropertyChanged(x => x.HardwareVer);
				}
			}
		}
		public string FirmwareVer {
			get {
				return m_FirmwareVer;
			}
			private set {
				if (m_FirmwareVer != value) {
					m_FirmwareVer = value;
					NotifyPropertyChanged(x => x.FirmwareVer);
				}
			}
		}
		public string DeviceID {
			get {
				return m_DeviceID;
			}
			private set {
				if (m_DeviceID != value) {
					m_DeviceID = value;
					NotifyPropertyChanged(x => x.DeviceID);
				}
			}
		}
		public string NetworkIPAddress {
			get {
				return m_NetworkIPAddress;
			}
			private set {
				if (m_NetworkIPAddress != value) {
					m_NetworkIPAddress = value;
					NotifyPropertyChanged(x => x.NetworkIPAddress);
				}
			}
		}
		public string MACAddress {
			get {
				return m_MACAddress;
			}
			private set {
				if (m_MACAddress != value) {
					m_MACAddress = value;
					NotifyPropertyChanged(x => x.MACAddress);
				}
			}
		}

		public System.DateTime dateTime {
			get {
				return m_dateTime.current;
			}
			set {
				if (m_dateTime.current != value) {
					m_dateTime.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.dateTime);
				}
			}
		}
		
	}
}
