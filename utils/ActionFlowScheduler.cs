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


