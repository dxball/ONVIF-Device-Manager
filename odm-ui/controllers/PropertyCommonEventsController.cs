using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.onvif;
using nvc.models;
using nvc.controlsUIProvider;

namespace nvc.controllers {
	public class PropertyCommonEventsController : BasePropertyController {
		Session CurrentSession { get; set; }
		IDisposable _subscription;

		public PropertyCommonEventsController() { }

		public override void CreateController(Session session, ChannelDescription chan) {
			var eventList = WorkflowController.Instance.GetMainFrameController().GetEventList("device");
			UIProvider.Instance.CommonEventsProvider.InitView(eventList);

			WorkflowController.Instance.GetMainFrameController().EventAction = EventHandler;
			WorkflowController.Instance.GetMainFrameController().RemoveEventAction = RemoveEvent;
		}

		public void EventHandler(EventDescriptor evDescr) {
			if (evDescr.ChannelID == "device")
				UIProvider.Instance.CommonEventsProvider.AddEvent(evDescr);
		}
		public void RemoveEvent(EventDescriptor evDescr) {
			if (evDescr.ChannelID == "device")
				UIProvider.Instance.CommonEventsProvider.RemoveEvent(evDescr);
		}
		
		protected override void ApplyChanges() {}
		protected override void CancelChanges() { }
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseCommonEventsProvider();
			if (_subscription != null) _subscription.Dispose();
		}
		protected override void LoadControl() { }
	}
}
