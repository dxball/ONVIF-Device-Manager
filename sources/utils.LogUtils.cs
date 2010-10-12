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
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace nvc.utils {

	public interface ILogUtils {
		TraceListenerCollection listeners {get;}
		void WriteEvent(string evtMsg, string evtSrc, TraceEventType evtType);
		void Flush();
		void Close();
	}

	public class LogUtilsBase : ILogUtils {

		//--------------------------------------------------
		// public section
		//--------------------------------------------------

		public virtual Guid activityId {
			get {
				return Guid.Empty;
			}
		}

		//--------------------------------------------------
		// ILogUtils implementation
		//--------------------------------------------------

		public TraceListenerCollection listeners {
			get {
				return Trace.Listeners;				
			}
		}
		
		public void WriteEvent(string evtMsg, string evtSrc, TraceEventType evtType) {
			var ctx = BeforeWriteEvent();
			try {
				var evtId = Interlocked.Increment(ref _evtId);
				var evtCache = new TraceEventCache();
				foreach (TraceListener l in listeners) {
					lock (l) {
						l.TraceEvent(evtCache, evtSrc, evtType, evtId, evtMsg);
						if (Trace.AutoFlush) {
							l.Flush();
						}
					}
				}
			} finally {
				AfterWriteEvent(ctx);
			}
		}

		public void Flush() {
			foreach (TraceListener l in listeners) {
				lock (l) {
					l.Flush();
				}
			}
		}
		public void Close() {
			foreach (TraceListener l in listeners) {
				lock (l) {
					l.Close();
				}
			}
		}
		
		//--------------------------------------------------
		// protected section
		//--------------------------------------------------

		protected virtual Object BeforeWriteEvent() {
			var oldActivityId = Trace.CorrelationManager.ActivityId;
			if (oldActivityId != activityId) {
				Trace.CorrelationManager.ActivityId = activityId;
			}
			return oldActivityId;
		}

		protected virtual void AfterWriteEvent(Object context) {
			var oldActivityId = (Guid)context;
			if (oldActivityId != activityId) {
				Trace.CorrelationManager.ActivityId = activityId;
			}
		}

		//--------------------------------------------------
		// private section
		//--------------------------------------------------

		private int _evtId = 0;
	}


	public class LogUtilsDefault : LogUtilsBase {

		//--------------------------------------------------
		// public section
		//--------------------------------------------------
		
		public LogUtilsDefault() {
			_activityId = Guid.NewGuid();
		}

		public LogUtilsDefault(Guid activityId) {
			this._activityId = activityId;
		}

		//--------------------------------------------------
		// protected section
		//--------------------------------------------------

		protected Guid _activityId;
	}

	public class LogUtils {

		//--------------------------------------------------
		// public section
		//--------------------------------------------------
		
		static LogUtils() {
			Init(new LogUtilsDefault());
		}

		[Conditional("TRACE")]
		public static void Init(ILogUtils impl) {
			//lock (_staticLock) {
			LogUtils._impl = impl;
			//}
		}

		[Conditional("TRACE")]
		public static void WriteEvent(string evtMsg, string evtSrc, TraceEventType evtType) {
			_impl.WriteEvent(evtMsg, evtSrc, evtType);
		}

		[Conditional("TRACE")]
		public static void WriteError(string errMsg, string category) {
			WriteEvent(errMsg, category, TraceEventType.Error);
		}

		[Conditional("TRACE")]
		public static void WriteError(string errMsg) {
			WriteEvent(errMsg, null, TraceEventType.Error);
		}
		
		[Conditional("TRACE")]
		public static void WriteWarning(string warnMsg, string category) {
			WriteEvent(warnMsg, category, TraceEventType.Warning);
		}

		[Conditional("TRACE")]
		public static void WriteWarning(string warnMsg) {
			WriteEvent(warnMsg, null, TraceEventType.Warning);
		}

		[Conditional("TRACE")]
		public static void WriteInfo(string infoMsg, string category) {
			WriteEvent(infoMsg, category, TraceEventType.Information);
		}

		[Conditional("TRACE")]
		public static void WriteInfo(string infoMsg) {
			WriteEvent(infoMsg, null, TraceEventType.Information);
		}

		[Conditional("TRACE")]
		public static void Flush() {
			_impl.Flush();
		}

		[Conditional("TRACE")]
		public static void Close() {
			_impl.Close();
		}

		//--------------------------------------------------
		// private section
		//--------------------------------------------------

		//private static object _staticLock = new Object();
		private static ILogUtils _impl;

	}
}
