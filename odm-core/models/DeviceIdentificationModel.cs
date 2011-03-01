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

			serial = info.SerialNumber;
			manufacturer = info.Manufacturer;
			model = info.Model;
			hardwareVersion = info.HardwareId;
			firmwareVersion = info.FirmwareVersion;
			networkIpAddress = netstat.ip.ToString();
			mac = netstat.mac;
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

		private string m_manufacturer;
		private string m_model;
		private string m_serial;
		private string m_hardwareVersion;
		private string m_firmwareVersion;
		private string m_networkIpAddress;
		private string m_mac;
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

		public string serial {
			get {
				return m_serial;
			}
			private set {
				if (m_serial != value) {
					m_serial = value;
					NotifyPropertyChanged(x => x.serial);
				}
			}
		}

		public string manufacturer {
			get {
				return m_manufacturer;
			}
			private set {
				if (m_manufacturer != value) {
					m_manufacturer = value;
					NotifyPropertyChanged(x => x.manufacturer);
				}
			}
		}

		public string model {
			get {
				return m_model;
			}
			private set {
				if (m_model != value) {
					m_model = value;
					NotifyPropertyChanged(x => x.model);
				}
			}
		}
		public string hardwareVersion {
			get {
				return m_hardwareVersion;
			}
			private set {
				if (m_hardwareVersion != value) {
					m_hardwareVersion = value;
					NotifyPropertyChanged(x => x.hardwareVersion);
				}
			}
		}
		public string firmwareVersion {
			get {
				return m_firmwareVersion;
			}
			private set {
				if (m_firmwareVersion != value) {
					m_firmwareVersion = value;
					NotifyPropertyChanged(x => x.firmwareVersion);
				}
			}
		}
		
		public string networkIpAddress {
			get {
				return m_networkIpAddress;
			}
			private set {
				if (m_networkIpAddress != value) {
					m_networkIpAddress = value;
					NotifyPropertyChanged(x => x.networkIpAddress);
				}
			}
		}
		public string mac {
			get {
				return m_mac;
			}
			private set {
				if (m_mac != value) {
					m_mac = value;
					NotifyPropertyChanged(x => x.mac);
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
