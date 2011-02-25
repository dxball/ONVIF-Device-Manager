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
	public partial class DateTimeSettingsModel : ModelBase<DateTimeSettingsModel> {
		
		public DateTimeSettingsModel() {
			
		}

		protected string NetHostToString(NetworkHost netHost) {
			if(netHost == null){
				throw new ArgumentNullException("netHost"); 
			}
			switch (netHost.Type) {
				case NetworkHostType.IPv4:
					return netHost.IPv4Address;
				case NetworkHostType.IPv6:
					return netHost.IPv6Address;
				case NetworkHostType.DNS:
					return netHost.DNSname;
			}
			throw new ArgumentOutOfRangeException("netHost.Type");
		}

		protected NetworkHost NetHostFromString(string netHost) {
			if (netHost == null) {
				throw new ArgumentNullException("netHost");
			}
			netHost = netHost.Trim();

			System.Net.IPAddress ipAddr;
			if (System.Net.IPAddress.TryParse(netHost, out ipAddr)) {
				if(ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork){
					return new NetworkHost() {
						Type = NetworkHostType.IPv4,
						IPv4Address = netHost
					};
				}else if (ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) {
					return new NetworkHost() {
						Type = NetworkHostType.IPv6,
						IPv4Address = netHost
					};
				}
			}
			
			return new NetworkHost() {
				Type = NetworkHostType.DNS,
				DNSname = netHost
			};
			
		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<DateTimeSettingsModel> observer) {
			DeviceObservable device = null;
			SystemDateTime time = null;
			NTPInformation ntpInfo = null;
			
			yield return session.GetDeviceClient().Handle(x => device = x);
			yield return device.GetSystemDateAndTime().Handle(x => time = x);
			dbg.Assert(time != null);
			dbg.Assert(time.TimeZone != null);
			dbg.Assert(time.UTCDateTime != null);
			dbg.Assert(time.UTCDateTime.Date != null);
			dbg.Assert(time.UTCDateTime.Time != null);

			yield return device.GetNTP().Handle(x => ntpInfo = x);
			dbg.Assert(ntpInfo != null);

			if (ntpInfo.NTPManual != null) {
				m_ntpServerManual.SetBoth(String.Join("; ", ntpInfo.NTPManual.Select(x => NetHostToString(x))));
			} else {
				m_ntpServerManual.SetBoth(String.Empty);
			}

			if (ntpInfo.NTPFromDHCP != null) {
				ntpServerFromDhcp = String.Join("; ", ntpInfo.NTPFromDHCP.Select(x => NetHostToString(x)));
			} else {
				ntpServerFromDhcp = String.Empty;
			}
						
			m_useNtpFromDhcp.SetBoth(ntpInfo.FromDHCP);

			var dt = new System.DateTime(
					time.UTCDateTime.Date.Year,
					time.UTCDateTime.Date.Month,
					time.UTCDateTime.Date.Day,
					time.UTCDateTime.Time.Hour,
					time.UTCDateTime.Time.Minute,
					time.UTCDateTime.Time.Second,
					DateTimeKind.Utc
			);

			m_dateTime.SetBoth(dt);
			m_timeZone.SetBoth(time.TimeZone.TZ);
			m_daylightSavings.SetBoth(time.DaylightSavings);

			NotifyPropertyChanged(x => x.ntpServerFromDhcp);
			NotifyPropertyChanged(x => x.ntpServerManual);
			NotifyPropertyChanged(x => x.useNtpFromDhcp);
			
			NotifyPropertyChanged(x => x.timeZone);
			NotifyPropertyChanged(x => x.dateTime);
			NotifyPropertyChanged(x => x.daylightSavings);
			NotifyPropertyChanged(x => x.isModified);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public override void RevertChanges() {
			m_timeZone.Revert();
			m_dateTime.Revert();
			m_daylightSavings.Revert();
			m_ntpServerManual.Revert();
			m_useNtpFromDhcp.Revert();

			NotifyPropertyChanged(x => x.timeZone);
			NotifyPropertyChanged(x => x.dateTime);
			NotifyPropertyChanged(x => x.daylightSavings);
			NotifyPropertyChanged(x => x.ntpServerManual);
			NotifyPropertyChanged(x => x.useNtpFromDhcp);
			NotifyPropertyChanged(x => x.isModified);
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<DateTimeSettingsModel> observer) {
			DeviceObservable device = null;
			SystemDateTime time = null;
			tt::TimeZone tz = null;

			if (m_dateTime.isModified || m_timeZone.isModified) {
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
				tz = new tt.TimeZone();
				tz.TZ = timeZone;
				yield return device.SetSystemDateAndTime(SetDateTimeType.Manual, daylightSavings, tz, utcTime).Idle();
			}

			yield return device.SetNTP(useNtpFromDhcp, ntpServerManual.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries).Select(x => NetHostFromString(x)).ToArray()).Idle();

			yield return Observable.Concat(LoadImpl(session, observer)).Idle();
			if (observer != null) {
			    observer.OnNext(this);
			}
		}

		private ChangeTrackingProperty<string> m_timeZone = new ChangeTrackingProperty<string>();
		private ChangeTrackingProperty<System.DateTime> m_dateTime = new ChangeTrackingProperty<System.DateTime>();
		private ChangeTrackingProperty<bool> m_daylightSavings = new ChangeTrackingProperty<bool>();
		private ChangeTrackingProperty<string> m_ntpServerManual = new ChangeTrackingProperty<string>();
		private ChangeTrackingProperty<bool> m_useNtpFromDhcp = new ChangeTrackingProperty<bool>();
		
		public string timeZone {
			get {

				return m_timeZone.current;
			}
			set {
				if (m_timeZone.current != value) {
					m_timeZone.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.timeZone);
				}
			}
		}

		public bool daylightSavings {
			get {

				return m_daylightSavings.current;
			}
			set {
				if (m_daylightSavings.current != value) {
					m_daylightSavings.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.timeZone);
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

		public string ntpServerManual {
			get {
				return m_ntpServerManual.current;
			}
			set {
				if (m_ntpServerManual.current != value) {
					m_ntpServerManual.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.ntpServerManual);
				}
			}
		}

		public string ntpServerFromDhcp {get; private set;}

		public bool useNtpFromDhcp {
			get {
				return m_useNtpFromDhcp.current;
			}
			set {
				if (m_useNtpFromDhcp.current != value) {
					m_useNtpFromDhcp.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.useNtpFromDhcp);
				}
			}
		}
	}
}
