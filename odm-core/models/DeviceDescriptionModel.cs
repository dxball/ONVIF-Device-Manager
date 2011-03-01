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
using System.ComponentModel;

namespace odm.models {

	public interface IDeviceDescriptionModel : INotifyPropertyChanged {
		string name{get;set;}
		string address{get;set;}
		string location{get;set;}
		string firmware{get;set;}
		//public Exception error {get;set;}
	}

	public class DeviceDescriptionModel : ModelBase<DeviceDescriptionModel>, IDeviceDescriptionModel {
		private string m_name = String.Empty;
		private string m_address = String.Empty;
		private string m_location = String.Empty;
		//private string m_Id;
		private string m_firmware = String.Empty;
		private Exception m_error;
		
		public DeviceDescriptionModel(IDeviceDescription description) {
			this.name = description.name;
			this.location = description.location;
			this.address = String.Join(" ,", description.uris.Select(x => x.Host).Distinct());
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
				name = info.Name;
				location = info.Location;
				firmware = info.FirmwareVersion;
			}

			if (observer != null) {
				observer.OnNext(this);
			}
		}		

		public string name {
			get {
				return m_name;
			}
			set {
				if (m_name != value) {
					m_name = value;
					NotifyPropertyChanged(x => x.name);
				}
			}
		}

		public string address {
			get {
				return m_address;
			}
			set {
				if (m_address != value) {
					m_address = value;
					NotifyPropertyChanged(x => x.address);
				}
			}
		}

		public string location {
			get {
				return m_location;
			}
			set {
				if (m_location != value) {
					m_location = value;
					NotifyPropertyChanged(x => x.location);
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

		public string firmware {
			get {
				return m_firmware;
			}
			set {
				if (m_firmware != value) {
					m_firmware = value;
					NotifyPropertyChanged(x => x.firmware);
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
