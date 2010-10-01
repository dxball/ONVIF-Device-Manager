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
using System.Drawing;
using nvc.onvif;

namespace nvc.entities {
	public class EventDescriptor {
		public EventDescriptor() {
		}
		public EventDescriptor(int id, DateTime date, string type, string details) {
			Type = type;
			Details = details;
			RuleID = id;
			TimeDate = date;
		}
		public string Type { get; set; }
		public string Details { get; set; }
		public int RuleID { get; set; }
		public DateTime TimeDate { get; set; }
		public Image Thumbnail { get; set; }
		public Image Image { get; set; }
	}

	public class EventsBehaviorDescriptor {
		public bool SendONVIFMessage { get; set; }
		public bool TriggerRelay { get; set; }
		public DigitalOutputDescriptor OutputRelay { get; set; }
		public bool RecordChannel { get; set; }
		public DeviceChannel ChannelForRecord { get; set; }
		public bool SwitchAnalogueVideoOn { get; set; }
	}

	public class DigitalInputDescriptor {
		public string Name { get; set; }
		public string NormalStatus { get; set; }
		public string CurrentStatus { get; set; }
		public string ID { get; set; }
	}

	public class DigitalOutputDescriptor {
		public string Name { get; set; }
		public string IdleStatus { get; set; }
		public string CurrentStatus { get; set; }
		public string ID { get; set; }
	}

	public class DeviceModelInfo : EventArgs {
		public DeviceDescription devDescr { get; set; }
		public string Name { get; set; }
		public string IpAddress { get; set; }
		public string Firmware { get; set; }
		public string DeviceId { get; set; }
		public string Manufacturer { get; set; }
		public string HardwareId { get; set; }

		public bool IsValid { get; set; }
		public string ErrorMsg { get; set; }
	}

	public class AvailableResolution {
		nvc.models.Channel.Resolution _resolution;
		public nvc.models.Channel.Resolution Resolution { 
			get{
				return _resolution;
			}
			set{
				_resolution = value;
			} 
		}
		public string ResolutionString{
			get{
				return Resolution.width + "x" + Resolution.height;
			}
		}
		List<AvailableEncoder> _encodersList;
		public List<AvailableEncoder> EncodersList {
			get {
				if (_encodersList == null)
					_encodersList = new List<AvailableEncoder>();
				return _encodersList;
			}
			set { _encodersList = value; }
		}
	}

	public class AvailableEncoder {
		nvc.models.VideoEncoder.Encoding _encoding;
		public nvc.models.VideoEncoder.Encoding Encoding {
			get { return _encoding; }
			set { _encoding = value; }
		}
		List<nvc.models.Channel.Resolution> _resolutionsList {
			get {
				if (_resolutionsList == null)
					_resolutionsList = new List<models.Channel.Resolution>();
				return _resolutionsList;
			}
			set { _resolutionsList = value; }
		}
	}
}
