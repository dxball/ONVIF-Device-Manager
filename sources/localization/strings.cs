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
using System.Xml.XPath;
using System.Reflection;
using nvc.localization;

namespace nvc {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
	public class XPathAttribute : Attribute {
		public XPathAttribute(string xpath) {
			this.xpath = xpath;
		}
		public string xpath;
	}

	[XPath("/localized-strings")]
	public partial class Constants {

		private Constants() {
			Language.CurrentObservable.Subscribe(l => {
				Load(l.CreateNavigator());
			});
		}

		private static Constants _instance;
		public static Constants Instance {
			get {
				if (_instance == null)
					_instance = new Constants();
				return _instance;
			}
		}

		public void Load(XPathNavigator nav) {
			
			var sb = new StringBuilder();
			
			Func<string, string> xeval = xpath => {
				if (nav == null) {
					return null;
				}
				var t = nav.Select(xpath);
				var result = new StringBuilder();
				while (t.MoveNext()) {
					result.Append(t.Current);
				}
				return result.ToString();
			};


			GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.ForEach(t => {
					var x = xeval(String.Format(@"localized-strings/{0}", t.Name));
					t.SetValue(this, x, null);
				});
		}

		public void Load(string file) {
			
			var doc = new XPathDocument(file);			
			Load(doc.CreateNavigator());
				
		}
	}
}