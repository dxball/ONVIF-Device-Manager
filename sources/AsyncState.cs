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
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace nvc {

	public class AsyncStateValue {
		public override string ToString() {
			var type = GetType();
			var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
			var val = fields
				.Where(x => type.IsAssignableFrom(x.FieldType))
				.Where(x => x.GetValue(null) == this)
				.FirstOrDefault();

			if (val != null) {
				return val.Name;
			}

			return base.ToString();
		}
	}

	[DebuggerDisplay("{value}")]
	public class AsyncState<T> where T : class {
		public AsyncState(T initState) {
			m_value = initState;
		}
		private T m_value;
		public T value {
			get {
				return m_value;
			}
		}

		public virtual bool transit(T from, T to) {
			return Interlocked.CompareExchange(ref m_value, to, from) == from;
		}

		public virtual T transit(T to) {
			return Interlocked.Exchange(ref m_value, to);
		}
	}

}
