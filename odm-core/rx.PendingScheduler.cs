using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Concurrency;
using System.Disposables;

namespace nvc.rx {
	class PendingScheduler:IScheduler {
		bool m_isProcessing = false;
		object m_gate = new object();
		Queue<Action> m_queue = new Queue<Action>();


		public DateTimeOffset Now {
			get {
				return DateTimeOffset.Now;
			}
		}

		public IDisposable Schedule(Action action, TimeSpan dueTime) {
			bool canceled = false;
			lock (m_gate) {
				m_queue.Enqueue(() => {
					Thread.Sleep(dueTime);
					if(!canceled){
						action();
					}
				});
			}
			return Disposable.Create(() => {
				canceled = true;
			});
		}

		public IDisposable Schedule(Action action) {
			bool canceled = false;
			lock (m_gate) {
				m_queue.Enqueue(() => {
					if (!canceled) {
						action();
					}
				});
			}
			return Disposable.Create(() => {
				canceled = true;
			});
		}
		
		public void Process() {
			lock (m_gate) {
				if (m_isProcessing) {
					return;
				}
				m_isProcessing = true;
			}
			while (true) {
				Action action = null;
				lock (m_gate) {
					if (m_queue.Count == 0) {
						m_isProcessing = false;
						return;
					}
					action = m_queue.Dequeue();
				}
				try {
					action();
				} catch (Exception err) {
					lock (m_gate) {
						m_isProcessing = false;
					}
					throw err;
				}
			}

		}
	}
}
