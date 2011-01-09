using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.controls;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class CommonEventsProvider : BaseUIProvider, ICommonEventsProvider {
		PropertyDeviceEvents _commonEvents;
		public void InitView(List<EventDescriptor> eventList) {
			_commonEvents = new PropertyDeviceEvents();
			//_commonEvents.FillListView(eventList);
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_commonEvents);
		}
		public void AddEvent(EventDescriptor evDescr) {
			//_commonEvents.AddListItem(evDescr);
		}
		public void RemoveEvent(EventDescriptor evDescr) {
			//_commonEvents.RemoveListViewItem(evDescr);
		}

		public override void ReleaseUI() {
			if (_commonEvents != null) {
				_commonEvents.ReleaseAll();
				_commonEvents = null;
			}
		}
	}
}
