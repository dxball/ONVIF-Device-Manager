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
using System.Disposables;
using System.Threading;

namespace nvc {

	public class BatchOperation: IDisposable {
		private object m_gate = new object();
		private LinkedList<IDisposable> m_queue = new LinkedList<IDisposable>();
		private Action m_completeHandler = null;
		private Action completeHandler {
			get {
				return m_completeHandler;
			}
			set {
				lock (m_gate) {
					if (!m_isCompleted) {
						m_completeHandler = value;
						return;
					}
				}
				if (value != null) {
					value();
				}
			}
		}
		private bool m_isCompleted = false;
		
		private BatchOperation() {			
		}
		private void Remove(LinkedListNode<IDisposable> node) {
			lock (m_gate) {
				if (m_isCompleted || node == null) {
					return;
				}
				m_queue.Remove(node);
			}
			CompleteIfNecessary();
		}
		private IDisposable Dequeue() {
			IDisposable node = null;
			lock (m_gate) {
				if (m_queue.First == null) {
					return null;
				}
				node = m_queue.First.Value;
				m_queue.RemoveFirst();
			}
			return node;
		}
		private void Cleanup() {
			m_completeHandler = null;
		}
		private LinkedListNode<IDisposable> Enqueue(IDisposable disposable) {
			lock (m_gate) {
				if (!m_isCompleted) {
					return m_queue.AddFirst(disposable);
				}								
			}
			disposable.Dispose();
			return null;
		}

		private void CompleteIfNecessary() {
			Action _completeHandler = null;
			lock (m_gate) {
				if (m_isCompleted || m_queue.First != null) {
					return;
				}
				_completeHandler = m_completeHandler;
				m_isCompleted = true;				
				Cleanup();
			}
			if (_completeHandler != null) {
				_completeHandler();
			}		
		}

		public void Dispose(){
			lock (m_gate) {
				if (m_isCompleted) {
					return;
				}
				m_isCompleted = true;
				Cleanup();
			}

			var node = Dequeue();
			while (node != null) {
				node.Dispose();
				node = Dequeue();
			}			
		}

		public static IObservable<T> Create<T>(Func<BatchOperation, T> init){
			return Observable.CreateWithDisposable<T>(observer => {

				//var subj = new AsyncSubject<T>();
				//var subscription = subj.Subscribe(observer);
				
				var batch = new BatchOperation();
				
				try {
					var res = init(batch);
					batch.completeHandler = () => {
						observer.OnNext(res);
						observer.OnCompleted();						
					};
				} catch(Exception err) {
					batch.Dispose();
					observer.OnError(err);			
					return Disposable.Empty;
				}
				batch.CompleteIfNecessary();

				return batch;
			});
		}

		public IObservable<T> Join<T>(IObservable<T> observable){
			return Observable.CreateWithDisposable<T>(observer => {
				
				var subscription = new MutableDisposable();
				var node = Enqueue(subscription);
				subscription.Disposable = observable
					.Finally(() => {
						Remove(node);
					})
					.Subscribe(t => {
						observer.OnNext(t);
					}, err => {
						//swallow error
					});

				return subscription;
			});
		}
	}	
}
