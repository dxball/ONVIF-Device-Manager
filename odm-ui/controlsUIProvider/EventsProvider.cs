using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using nvc.controllers;
using nvc.models;

namespace nvc.controlsUIProvider {
	public class EventsProvider : BaseUIProvider {
		PropertyEvents _events;
		ChannelDescription CurrentChannel { get; set; }
		public void InitView(List<EventDescriptor> eventList, ChannelDescription chan) {
			CurrentChannel = chan;
			_events = new PropertyEvents() {onBindingError = BindingError};
			_events.FillListView(eventList);
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_events);
		}
		public void AddEvent(EventDescriptor evDescr) {
			_events.AddListItem(evDescr);
		}
		public void RemoveEvent(EventDescriptor evDescr) {
			_events.RemoveListViewItem(evDescr);
		}

		public override void ReleaseUI() {
			if (_events != null && !_events.IsDisposed)
				_events.ReleaseAll();
		}
	}
}
