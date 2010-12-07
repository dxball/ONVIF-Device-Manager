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
using System.Xml.XPath;
using System.Reflection;
using System.ComponentModel;
using onvifdm.utils;

namespace nvc.localization {	

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
	public class XPathAttribute : Attribute {
		public XPathAttribute(string xpath) {
			this.xpath = xpath;
		}
		public string xpath;
	}

	public class LocalizedStringsBase<T> : NotifyPropertyChangedBase where T : LocalizedStringsBase<T>, new() {
		protected LocalizedStringsBase() {
			SetLocale(Language.Current.CreateEvaluator());
			Language.CurrentObservable.Subscribe(l => {
				SetLocale(l.CreateEvaluator());
			}, err => {
				DebugHelper.Error(err);
			});
		}
		
		private static T _instance;
		public static T Instance {
			get {
				if (_instance == null)
					_instance = new T();
				return _instance;
			}
		}

		public virtual void SetLocale(Func<string, string> xeval) {

			var _xeval = xeval.Catch(err => {
			    DebugHelper.Error(err);
			    return null;
			});

			GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.ForEach(t => {
					var attr = Attribute.GetCustomAttribute(t, typeof(XPathAttribute)) as XPathAttribute;
					if (attr == null) {
						t.SetValue(this, null, null);
						return;
					}
					t.SetValue(this, _xeval(attr.xpath), null);
				});
		}

		public void SetLocale(string file) {
			var doc = new XPathDocument(file);
			SetLocale(doc.CreateEvaluator());
		}
	}
}
