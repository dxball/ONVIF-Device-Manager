using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using nvc.onvif;

using onvif.services.device;
using System.Concurrency;
using System.Threading;
using onvifdm.utils;

namespace nvc.models {
	public partial class DeviceIdentificationModel : ModelBase<DeviceIdentificationModel> {
		public DeviceIdentificationModel() {
			
		}
		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<DeviceIdentificationModel> observer) {
			DeviceInfo info = null;
			NetworkStatus netstat = null;
			
			//yield return Observable.Merge(
				yield return session.GetDeviceInfo().Handle(x => info = x);
				yield return session.GetNetworkStatus().Handle(x => netstat = x);
			//);
			DebugHelper.Assert(info != null);
			DebugHelper.Assert(netstat != null);

			m_name.SetBoth(info.Name);
			NotifyPropertyChanged(x => x.Name);
			
			HardwareVer = info.HardwareId;
			FirmwareVer = info.FirmwareVersion;
			DeviceID = info.SerialNumber;
			NetworkIPAddress = netstat.ip.ToString();
			MACAddress = BitConverter.ToString(netstat.mac.GetAddressBytes());

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public override void RevertChanges() {
			m_name.Revert();
			NotifyPropertyChanged(x => x.Name);
		}

		protected override IEnumerable<IObservable<Object>> ApplyChangesImpl(Session session, IObserver<DeviceIdentificationModel> observer) {
			if (!m_name.isModified) {
				yield break;
			}

			yield return session.SetName(m_name.current).Idle();

			if (observer != null) {
				observer.OnNext(this);
			}
		}


		private ChangeTrackingProperty<string> m_name = new ChangeTrackingProperty<string>();
		
		private string m_HardwareVer;
		private string m_FirmwareVer;
		private string m_DeviceID;
		private string m_NetworkIPAddress;
		private string m_MACAddress;
		
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
	}
}
