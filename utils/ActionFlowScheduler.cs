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
using System.Threading;
using System.Concurrency;
using System.Disposables;

namespace odm.utils {

	public class ActionFlowScheduler : IScheduler {
		protected ActionFlow m_actionFlow;

		public ActionFlowScheduler(ActionFlow actionFlow) {
			if (actionFlow == null) {
				throw new ArgumentNullException("actionFlow");
			}
			m_actionFlow = actionFlow;
		}

		public DateTimeOffset Now {
			get {
				return DateTime.Now;
			}
		}

		public IDisposable Schedule(Action action, TimeSpan dueTime) {
			bool canceled = false;
			var timer = new Timer(t => {
				if (!canceled) {
					action();
				}
			});
			timer.Change(dueTime, TimeSpan.FromMilliseconds(-1));
			return Disposable.Create(() => {
				canceled = true;
				timer.Dispose();
			});	
		}

		public IDisposable Schedule(Action action) {
			bool canceled = false;
			m_actionFlow.Invoke(()=>{
				if(!canceled){
					action();
				}
			});
			return Disposable.Create(() => {
				canceled = true;
			});			
		}
	}
}


