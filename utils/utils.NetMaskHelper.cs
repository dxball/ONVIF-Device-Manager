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
using System.Net;

namespace odm.utils {
	public class NetMaskHelper {		
		//Subnet mask
		public static IPAddress PrefixToMask(int prefix) {
			if (prefix == 0) {
				return new IPAddress(0);
			}

			if ((prefix < 0) || (prefix > 32)) {
				return null;
			}

			String retValue = "";

			uint mask = 0xFFFFFFFF;
			prefix = 32 - prefix;

			mask <<= prefix;
			byte[] bytes = new byte[4];

			for (int i = 0; i < 4; i++) {
				byte lastbyte = (byte)(mask & 0x000000FF);
				bytes[3-i] = lastbyte;
				retValue = lastbyte.ToString() + retValue;
				if (i < 3)
				    retValue = "." + retValue;
				mask >>= 8;
			}
			var ipAddr = new IPAddress(bytes);
			dbg.Assert(ipAddr.ToString() == retValue);
			return ipAddr;
		}
		
		public static int MaskToPrefix(IPAddress mask) {
			byte[] maskbytes = mask.GetAddressBytes();
			if (maskbytes.Length != 4)
				return -1;

			uint intmask = 0;

			for (int i = 0; i < 4; i++) {
				intmask |= (uint)(maskbytes[i] << (8 * (3 - i)));
			}

			int prefix = 32;

			while ((intmask & 1) == 0) {
				intmask >>= 1;
				prefix--;
			}

			return prefix;
		}

		public static int MaskToPrefix(String mask) {
			return MaskToPrefix(global::System.Net.IPAddress.Parse(mask));			
		}
	}
}
