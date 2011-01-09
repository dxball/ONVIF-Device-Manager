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
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Threading;
using System.Xml.XPath;
using System.Xml;

using odm.utils;
using System.Xml.Serialization;
using com=System.ComponentModel;
using System.Globalization;


namespace odm.utils {

	public static class EnumerableExtensions {

		public static T FirstOrDefault<T>(this IEnumerable<T> src, Func<T> factory) {
			if (src == null) {
				throw new ArgumentNullException("src");
			}
			
			using(var itor = src.GetEnumerator()) {
				if (itor.MoveNext()) {
					return itor.Current;
				}
			}
			return factory();
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> src, Func<T, bool> predicate, Func<T> factory) {
			if (src == null) {
				throw new ArgumentNullException("src");
			}
			using (var itor = src.Where(predicate).GetEnumerator()) {
				if (itor.MoveNext()) {
					return itor.Current;
				}
			}
			return factory();
		}

		public static void ForEach<T>(this IEnumerable<T> src, Action<T> action) {
			foreach (var element in src) {
				action(element);
			}
		}
		
		public static void ForEach<T>(this IEnumerable<T> src, Action<T, int> action) {
			int index = 0;
			foreach (var element in src) {
				action(element, index++);
			}
		}

		public static void ForEach(this IEnumerable src, Action<object> action) {
			foreach (var element in src) {
				action(element);
			}
		}

		public static void ForEach(this IEnumerable src, Action<object, int> action) {
			int index = 0;
			foreach (var element in src) {
				action(element, index);
			}
		}
		
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T head) {
			return Enumerable.Repeat(head, 1).Concat(source);
		}

		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T tail) {
			return source.Concat(Enumerable.Repeat(tail, 1));
		}
	}
}
