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
using nvc.models;

namespace nvc.entities {
	public class DeviceChannel {
		public DeviceChannel(string name, string id) {
			Name = name;
			Id = id;
		}
		public void ReleaseAll() {
		}
		//Video URL initialisation
		public event EventHandler VideoURLInitialised;
		public void RiseVideoURLInitialisedEvent() {
			if (VideoURLInitialised != null)
				VideoURLInitialised(this, new EventArgs());
		}
		public bool IsVideoURLInitialised = false;
		string _mediaStreamUri;
		public string MediaStreamUri{
			get {
				return _mediaStreamUri;
			}
			set {
				_mediaStreamUri = value;
				IsVideoURLInitialised = true;
				RiseVideoURLInitialisedEvent();
			}
		}
		
		//public VideoResolution GetResolutionByString(string res) {
		//    return _channelModel.encoder.resolutions.Where(x => x.ToString() == res).FirstOrDefault();
		//}

		Channel _channelModel;
		public Channel ChannelModel {
			get {
				return _channelModel;
			}
			set {
				_channelModel = value;
			}
		}

		public Channel GetModelChannel() {
			return ChannelModel;
		}
		public void SetModelChannel(Channel channel) {
			ChannelModel = channel;
		}

		public string Id { get; set; }
		public string GetChannelID() { return Id; }
		public string Name { get; set; }
		public string GetChannelName() { return Name; }

		#region Events section
		protected Queue<EventDescriptor> _eventsQueue;
		public Queue<EventDescriptor> EventsQueue {
			get {
				if (_eventsQueue == null)
					_eventsQueue = new Queue<EventDescriptor>();
				return _eventsQueue;
			}

		}
		public EventDescriptor AddEvent(EventDescriptor eventDescr) {
			EventsQueue.Enqueue(eventDescr);
			if (EventsQueue.Count > Defaults.iEventsMaxCount)
				return EventsQueue.Dequeue();
			else
				return null;
		}
		public List<EventDescriptor> GetEventsList() {
			return EventsQueue.ToList();
		}
		#endregion
	}
}
