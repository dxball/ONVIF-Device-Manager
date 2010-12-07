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
using nvc.onvif;
using System.Threading;
using nvc.controlsUIProvider;

namespace nvc.controllers {
	public class PropertyEventsController : BasePropertyController {
		public void EventHandler(EventDescriptor evDescr){
			if (CurrentChannel.Id == evDescr.ChannelID)
				UIProvider.Instance.EventsProvider.AddEvent(evDescr);
		}
		public void RemoveEvent(EventDescriptor evDescr) {
			if (CurrentChannel.Id == evDescr.ChannelID)
				UIProvider.Instance.EventsProvider.RemoveEvent(evDescr);
		}

		public override void CreateController(Session session, ChannelDescription chan) {
			CurrentChannel = chan;

			var eventList = WorkflowController.Instance.GetMainFrameController().GetEventList(CurrentChannel.Id);
			UIProvider.Instance.EventsProvider.InitView(eventList, CurrentChannel);

			WorkflowController.Instance.GetMainFrameController().EventAction = EventHandler;
			WorkflowController.Instance.GetMainFrameController().RemoveEventAction = RemoveEvent;
		}

		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseEventsProvider();
		}

		protected override void ApplyChanges() { }
		protected override void CancelChanges() { }
		protected override void LoadControl() { }
	}
}
