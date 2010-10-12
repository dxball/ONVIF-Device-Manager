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
using System.Threading;
using nvc.utils;

namespace nvc {
	public class WorkItemQueue {
		
		private Queue<Action> m_queue = new Queue<Action>();
		private object m_gate = new object();
		private bool m_isProcessed = false;

		public void Enqueue(Action action) {
			bool startProcessing = false;
			lock (m_gate) {
				m_queue.Enqueue(action);
				if (!m_isProcessed) {
					m_isProcessed = true;
					startProcessing = true;
				}
			}
			if (startProcessing) {
				ThreadPool.QueueUserWorkItem(state => {
					Action _action = null;
					while (true) {
						lock (m_gate) {
							if (m_queue.Count == 0) {
								m_isProcessed = false;
								return;
							}
							_action = m_queue.Dequeue();
						}

						if (_action != null) {
							try {
								_action();
							} catch {
								DebugHelper.Assert(false);
							}
						}
					}

				});
			}
		}		
	}

	public class GlobalWorkItemQueue {
		private static WorkItemQueue s_workItemQueue = new WorkItemQueue();
		public static void Enqueue(Action action){
			s_workItemQueue.Enqueue(action);
		}
	}
}
