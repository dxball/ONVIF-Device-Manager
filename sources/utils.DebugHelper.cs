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
//
//----------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Diagnostics;

namespace nvc.utils {
	
	public class DebugHelper{

		//--------------------------------------------------
		// public section
		//--------------------------------------------------

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Error(string errMsg) {
			_ErrorInternal(2, errMsg, null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Error(string errMsg, string errSrc) {
			_ErrorInternal(2, errMsg, errSrc);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Error(Exception exception) {
			_ErrorInternal(2, exception.Message, null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Error(Exception exception, string errSrc) {
			_ErrorInternal(2, exception.Message, errSrc);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Assert(bool expr, string errMsg, string category) {
			_AssertInternal(2, expr, errMsg, category);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Assert(bool expr, string errMsg) {
			_AssertInternal(2, expr, errMsg, null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Assert(bool expr) {
			_AssertInternal(2, expr, null, null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Info(string errMsg, string errSrc) {
			_InfoInternal(2, errMsg, errSrc);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Info(string errMsg) {
			_InfoInternal(2, errMsg, null);
		}

		public class AssertionFailureException : Exception {
		}

		[DebuggerHidden]
		[Conditional("DEBUG")]
		public static void Break() {
			if (Debugger.IsAttached) {
				Debugger.Break();
			}
		}

		[DebuggerHidden]
		[Conditional("DEBUG")]
		public static void BreakIf(bool Condition) {
			if (Debugger.IsAttached && Condition) {
				Debugger.Break();
			}
		}

		//--------------------------------------------------
		// protected section
		//--------------------------------------------------

		protected static string _ResolveSourceIfNone(StackFrame sf, string src) {
			if (String.IsNullOrEmpty(src)) {
				return sf.GetMethod().DeclaringType.Namespace;
			}
			return src;
		}

		[DebuggerHidden]
		protected static void _AssertInternal(int framesToSkip, bool expr, string errMsg, string src) {
			if (expr) {
				return;
			}

			StackTrace stack = new StackTrace(framesToSkip, true);
			var evtSb = new StringBuilder();
			evtSb.Append("ASSERT FAILED:");
			if (!String.IsNullOrEmpty(errMsg)) {
				evtSb.Append(" \"");
				evtSb.Append(errMsg);
				evtSb.Append("\"");
			}
			evtSb.Append(" at ");
			var sf = stack.GetFrame(0);
			AppendStackFrame(evtSb, sf);
			src = _ResolveSourceIfNone(sf, src);
			LogUtils.WriteEvent(evtSb.ToString(), src, TraceEventType.Critical);
			LogUtils.Flush();
			Break();
		}

		[DebuggerHidden]
		protected static void _ErrorInternal(int framesToSkip, string errMsg, string src) {

			StackTrace stack = new StackTrace(framesToSkip, true);
			var evtSb = new StringBuilder();
			if (!String.IsNullOrEmpty(errMsg)) {
				evtSb.Append(" \"");
				evtSb.Append(errMsg);
				evtSb.Append("\"");
			}
			evtSb.Append(" at ");
			var sf = stack.GetFrame(0);
			AppendStackFrame(evtSb, sf);
			src = _ResolveSourceIfNone(sf, src);
			LogUtils.WriteEvent(evtSb.ToString(), src, TraceEventType.Error);
			LogUtils.Flush();
			Break();
		}

		protected static void _InfoInternal(int framesToSkip, string errMsg, string src) {

			StackTrace stack = new StackTrace(framesToSkip, true);
			var evtSb = new StringBuilder();
			//if (!String.IsNullOrEmpty(errMsg)) {
			//    evtSb.Append(" \"");
			    evtSb.Append(errMsg);
			//    evtSb.Append("\"");
			//}
			//evtSb.Append(" at ");
			var sf = stack.GetFrame(0);
			//AppendStackFrame(evtSb, sf);
			src = _ResolveSourceIfNone(sf, src);
			LogUtils.WriteEvent(evtSb.ToString(), src, TraceEventType.Information);
			LogUtils.Flush();
			//Break();
		}

		private static void AppendStackFrame(StringBuilder sb, StackFrame sf) {
			sb.Append(sf.GetMethod());
			var fileName = sf.GetFileName();
			if (!String.IsNullOrEmpty(fileName)) {
				sb.Append(" in ");
				sb.Append(sf.GetFileName());
				var lineNumber = sf.GetFileLineNumber();
				if (lineNumber > 0) {
					sb.Append(": line ");
					sb.Append(lineNumber);
				}
			}
		}

	}
}
