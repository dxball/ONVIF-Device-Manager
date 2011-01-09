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

namespace odm.models {

	public class VideoStreamingSettings {
		public Tuple<uint,uint> resolution;
		public uint frameRate;
		public uint bitrate;
		public string encoder;
	}


	public partial class Channel {
		
		public string m_videoSourceToken;
		
		public string name;
		public VideoResolution sourceResolution;
		public int frameRate;
		public int bitrate;
		public VideoEncoder encoder;
	}

	public class ChannelSettings {
		public VideoStreamingSettings videoStreaming;
		public Uri streamUri;			
	}

	public class DeviceSettings {
		public ChannelSettings[] channels;
		public NetworkSettings nicSettings;
		public NetworkStatus nicStatus;
		public DeviceInfo devInfo;
	}

}
