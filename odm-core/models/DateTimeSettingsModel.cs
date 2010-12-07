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

using nvc.onvif;
using onvifdm.utils;

namespace nvc.models {
	public partial class DateTimeSettingsModel : ModelBase<DateTimeSettingsModel> {
		
		public DateTimeSettingsModel() {
			
		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<DateTimeSettingsModel> observer) {
			DeviceObservable device = null;
			SystemDateTime time = null;
			
			yield return session.GetDeviceClient().Handle(x => device = x);
			yield return device.GetSystemDateAndTime().Handle(x => time = x);
			DebugHelper.Assert(time != null);
			DebugHelper.Assert(time.TimeZone != null);
			DebugHelper.Assert(time.UTCDateTime != null);
			DebugHelper.Assert(time.UTCDateTime.Date != null);
			DebugHelper.Assert(time.UTCDateTime.Time != null);

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
			
			NotifyPropertyChanged(x => x.timeZone);
			NotifyPropertyChanged(x => x.dateTime);
			NotifyPropertyChanged(x => x.isModified);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public override void RevertChanges() {
			m_timeZone.Revert();
			m_dateTime.Revert();
			NotifyPropertyChanged(x => x.timeZone);
			NotifyPropertyChanged(x => x.dateTime);
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

			yield return Observable.Concat(LoadImpl(session, observer)).Idle();
			if (observer != null) {
			    observer.OnNext(this);
			}
		}

		private ChangeTrackingProperty<string> m_timeZone = new ChangeTrackingProperty<string>();
		private ChangeTrackingProperty<System.DateTime> m_dateTime = new ChangeTrackingProperty<System.DateTime>();
		private ChangeTrackingProperty<bool> m_daylightSavings = new ChangeTrackingProperty<bool>();
		
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
		
	}
}
