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
using System.Diagnostics;
using System.Reflection;

namespace odm.utils.rx {
	
	public sealed class ObserverState : AsyncStateValue {
		private ObserverState() { }
		public static readonly ObserverState delayed = new ObserverState();
		public static readonly ObserverState subscribed = new ObserverState();
		public static readonly ObserverState disposed = new ObserverState();
		public static readonly ObserverState completed = new ObserverState();
		public static readonly ObserverState failed = new ObserverState();
		public static AsyncState<ObserverState> Create() {
			return new AsyncState<ObserverState>(ObserverState.subscribed);
		}
		public static AsyncState<ObserverState> Create(ObserverState initState) {
			return new AsyncState<ObserverState>(initState);
		}
	}

	public static class ObserverStateExtensions {
		public static bool isDisposed(this AsyncState<ObserverState> state) {
			return state.value == ObserverState.disposed;
		}
		public static bool isCompleted(this AsyncState<ObserverState> state) {
			return state.value == ObserverState.completed;
		}
		public static bool isFailed(this AsyncState<ObserverState> state) {
			return state.value == ObserverState.failed;
		}
		public static bool isSubscribed(this AsyncState<ObserverState> state) {
			return state.value == ObserverState.subscribed;
		}
	}

}
