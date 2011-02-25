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

using odm.utils;

namespace odm.ui.controls {
	public class ListItem {
		public static ListItem<T> Wrap<T>(T obj, Func<T, string> display) where T : class {
			return new ListItem<T>(obj, display);
		}
	}
	public class ListItem<T>{
		public ListItem(T obj, Func<T, string> display) {
			m_object = obj;
			m_display = display;
		}
		T m_object;
		Func<T, string> m_display = null;
		public override string ToString() {
			if (m_display != null) {
				return m_display(m_object);
			}
			return m_object.ToString();
		}
		public override bool Equals(object obj) {
			if (m_object == null && obj == null) {
				return true;
			}
			if (obj == null || obj.GetType() != typeof(ListItem<T>)) {
				return false;
			}
			var result = m_object.Equals((obj as ListItem<T>).m_object);
			return result;
		}
		public override int GetHashCode() {
			if (m_object == null) {
				return 0;
			}
			return m_object.GetHashCode();
		}
		public T Unwrap() {
			return m_object;
		}
	}
}
