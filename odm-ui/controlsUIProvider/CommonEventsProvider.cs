using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using nvc.controllers;

namespace nvc.controlsUIProvider {
	public class CommonEventsProvider : BaseUIProvider {
		DeviceEventsControl _commonEvents;
		public void InitView(List<EventDescriptor> eventList) {
			_commonEvents = new DeviceEventsControl();
			_commonEvents.FillListView(eventList);
			UIProvider.Instance.MainFrameProvider.AddPropertyControl(_commonEvents);
		}
		public void AddEvent(EventDescriptor evDescr) {
			_commonEvents.AddListItem(evDescr);
		}
		public void RemoveEvent(EventDescriptor evDescr) {
			_commonEvents.RemoveListViewItem(evDescr);
		}

		public override void ReleaseUI() {
			if (_commonEvents != null && !_commonEvents.IsDisposed)
				_commonEvents.ReleaseAll();
		}
	}
}
