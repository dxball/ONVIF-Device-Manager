using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.Drawing.Imaging;
using System.Concurrency;
using System.Disposables;

namespace onvifdm.player {

	public class Dispatcher {
		bool m_canceled = false;
		ManualResetEvent m_waitEvt = new ManualResetEvent(false);
		bool m_isProcessed = false;
		object m_gate = new object();
		Queue<Action> m_queue = new Queue<Action>();

		public void Cancel() {
			Invoke(() => {
				lock (m_gate) {
					m_canceled = true;
					m_queue.Clear();
				}
			});
		}

		public bool Invoke(Action action) {
			lock (m_gate) {
				if (m_canceled) {
					return false;
				}
				m_queue.Enqueue(action);
				if (!m_isProcessed) {
					m_waitEvt.Set();
				}
			}
			return true;
		}

		public void Run() {
			while (!m_canceled) {
				m_waitEvt.WaitOne();
				Action action = null;
				lock (m_gate) {
					m_isProcessed = true;
					action = m_queue.Dequeue();
				}
				try {
					action();
				} catch {
					//TODO: handle error
				}
				lock (m_gate) {
					m_isProcessed = false;
					if (m_queue.Count == 0) {
						m_waitEvt.Reset();
					}
				}
			}
		}
	}
}


