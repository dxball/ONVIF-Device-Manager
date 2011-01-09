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

	public static class XPathNavigable {
		private class AnonymousXPathNavigable : IXPathNavigable {
			private Func<XPathNavigator> m_factory;
			public AnonymousXPathNavigable(Func<XPathNavigator> factory) {
				if (factory == null) {
					throw new ArgumentNullException("factory");
				}
				m_factory = factory;
			}

			public XPathNavigator CreateNavigator() {
				return m_factory();
			}
		}
		public static IXPathNavigable Create(Func<XPathNavigator> factory) {
			return new AnonymousXPathNavigable(factory);
		}
	}

	public static class XPathExtensions {
		//private static Dictionary<string, XPathExpression> m_ExpressionCache = new Dictionary<string, XPathExpression>();

		public static Func<string, string> CreateEvaluator(this IXPathNavigable navigable) {
			return navigable.CreateNavigator().GetEvaluator();
		}
		public static Func<XPathExpression, string> CreateExprEvaluator(this IXPathNavigable navigable) {
			return navigable.CreateNavigator().GetExprEvaluator();
		}

		public static Func<string, string> GetEvaluator(this XPathNavigator navigator) {
			var xeval = GetExprEvaluator(navigator);
			return xpath => {
				XPathExpression expr = null;
				//lock (m_ExpressionCache) {
				//    if (!m_ExpressionCache.TryGetValue(xpath, out expr)) {
				expr = XPathExpression.Compile(xpath);
				//        m_ExpressionCache[xpath] = expr;
				//    }
				//}
				return xeval(expr);
			};
		}

		public static Func<XPathExpression, string> GetExprEvaluator(this XPathNavigator navigator) {
			return xpath => {
				if (navigator == null) {
					return null;
				}
				var t = navigator.Select(xpath);
				var sb = new StringBuilder();
				while (t.MoveNext()) {
					sb.Append(t.Current);
				}
				var result = sb.ToString();
				if (String.IsNullOrWhiteSpace(result)) {
					return null;
				}
				return result;
			};
		}
	}
}
