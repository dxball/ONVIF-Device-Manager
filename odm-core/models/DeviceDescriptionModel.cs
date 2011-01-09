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
using System.Threading;

using odm.onvif;
using odm.utils;

using onvif.services.device;
using onvif.types;

namespace odm.models {
	public class DeviceDescriptionModel : ModelBase<DeviceDescriptionModel> {
		private string m_Name = String.Empty;
		private string m_Address = String.Empty;
		private string m_Location = String.Empty;
		//private string m_Id;
		private string m_Firmware = String.Empty;
		private Exception m_error;
		
		public DeviceDescriptionModel(DeviceDescription description) {
			this.Name = description.name;
			this.Location = description.location;
			this.Address = String.Join(" ,", description.uris.Select(x => x.Host));
		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<DeviceDescriptionModel> observer) {
			Capabilities caps = null;
			DeviceInfo info = null;
			//yield return Observable.Merge(
				yield return session.GetCapabilities().Handle(x => caps = x);
				yield return session.GetDeviceInfo().Handle(x => info = x);
			//).HandleError(x=>m_error = x);

			if (m_error != null) {
				if (observer != null) {
					observer.OnNext(this);
				}
				yield break;
			}

			dbg.Assert(info != null);
			dbg.Assert(caps != null);

			if (info != null) {
				Name = info.Name;
				Location = info.Location;
				Firmware = info.FirmwareVersion;
			}

			if (observer != null) {
				observer.OnNext(this);
			}
		}		

		public string Name {
			get {
				return m_Name;
			}
			set {
				if (m_Name != value) {
					m_Name = value;
					NotifyPropertyChanged(x => x.Name);
				}
			}
		}

		public string Address {
			get {
				return m_Address;
			}
			set {
				if (m_Address != value) {
					m_Address = value;
					NotifyPropertyChanged(x => x.Address);
				}
			}
		}

		public string Location {
			get {
				return m_Location;
			}
			set {
				if (m_Location != value) {
					m_Location = value;
					NotifyPropertyChanged(x => x.Location);
				}
			}
		}

		//public string Id {
		//    get {
		//        return m_Id;
		//    }
		//    set {
		//        if (m_Id != value) {
		//            m_Id = value;
		//            NotifyPropertyChanged(x => x.Id);
		//        }
		//    }
		//}

		public string Firmware {
			get {
				return m_Firmware;
			}
			set {
				if (m_Firmware != value) {
					m_Firmware = value;
					NotifyPropertyChanged(x => x.Firmware);
				}
			}
		}

		public Exception error {
			get {
				return m_error;
			}
			private set {
				if (m_error != value) {
					m_error = value;
					NotifyPropertyChanged(x => x.error);
				}
			}
		}


	}
}
