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
using onvif.services.device;
using nvc.onvif;
using System.Threading;
using nvc.utils;

namespace nvc.models {
	public class DeviceDescriptionModel : ModelBase<DeviceDescriptionModel> {
		private string m_Name;
		private string m_Address;
		private string m_Location;
		private string m_Id;
		private string m_Firmware;
		
		public DeviceDescriptionModel(DeviceDescription description) {
			this.Name = description.Name;
			this.Location = description.Location;
			this.Address = String.Join(" ,", description.Uris.Select(x => x.Host));
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<DeviceDescriptionModel> observer) {
			Capabilities caps = null;
			DeviceInfo info = null;

			yield return Observable.Merge(
				session.GetCapabilities().Handle(x => caps = x),
				session.GetDeviceInfo().Handle(x => info = x)
			);

			DebugHelper.Assert(info != null);
			DebugHelper.Assert(caps != null);

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

		public string Id {
			get {
				return m_Id;
			}
			set {
				if (m_Id != value) {
					m_Id = value;
					NotifyPropertyChanged(x => x.Id);
				}
			}
		}

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

	}
}
