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
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;

using nvc.onvif;
using dev = onvif.services.device;

namespace nvc.models {

	public static class DeviceInfoExtensions {

		public static IObservable<DeviceInfo> GetDeviceInfo(this Session session) {

			var dev_con = session.device;

			var asyncOp = dev_con.GetDeviceInformation()
				.Select(x => {
					return new DeviceInfo() {
						Manufacturer = x.Manufacturer,
						Model = x.Model,
						FirmwareVersion = x.FirmwareVersion,
						SerialNumber = x.SerialNumber,
						HardwareId = x.HardwareId
					};
				});

			return asyncOp;
		}
	};
}
