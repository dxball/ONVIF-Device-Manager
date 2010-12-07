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
using System.IO;
using onvifdm.utils;
using System.Globalization;

namespace nvc.localization {
	
	public class Language:IXPathNavigable{
		private Language() {
		}
		private string m_FilePath = null;

		public string DisplayName = null;
		public string iso3 = null;

		public virtual XPathNavigator CreateNavigator() {
			if (String.IsNullOrEmpty(m_FilePath)) {
				return null;
			}
			return new XPathDocument(m_FilePath).CreateNavigator();
		}

		public static IEnumerable<Language> AvailableLanguages {
			get {
				//yield return Default;
				var langs = Directory
					.GetFiles(Program.MapPath("~/locales"), "*.xml")
					.Select(x => new FileInfo(x))
					.Select(x => new Language() {
						DisplayName = Path.GetFileNameWithoutExtension(x.Name),
						m_FilePath = x.FullName
					});
				foreach (var t in langs) {
					try {
						var xeval = t.CreateEvaluator();
						var lang_name = xeval("/localized-strings/@name");
						var lang_iso3 = xeval("/localized-strings/@lang-iso3");
						
						if (lang_name == null && lang_iso3!=null) {
							var ci = CultureInfo.GetCultures(CultureTypes.NeutralCultures)
								.Where(c => c.ThreeLetterISOLanguageName.Equals(lang_iso3, StringComparison.OrdinalIgnoreCase))
								.FirstOrDefault();
							lang_name = ci != null ? ci.NativeName.ToLower() : null;
						}

						t.iso3 = lang_iso3;
						t.DisplayName = lang_name;
					} catch(Exception err) {
						//swallow error
						DebugHelper.Error(err);
					}
					yield return t;
				}
				
			}
		}

		public static Language Default = new Language() {
			DisplayName = "english(default)",
			iso3 = "eng"
		};
		private static Language m_Current = null;
		public static Language Current {
			get {
				if (m_Current == null) {
					return Default;
				}
				return m_Current;
			}
			set {
				if(m_Current != value){					
					m_Current = value;
					m_CurrentObservable.OnNext(value);
				}
			}
		}
		private static Subject<Language> m_CurrentObservable = new Subject<Language>();
		public static IObservable<Language> CurrentObservable {
			get {
				return m_CurrentObservable;
			}
		}
	}

	//class LanguageManager {
	//    public static IEnumerable<LanguageData> AvailableLanguages;
	//    public static LanguageData GetCurrentLanguage() {
	//        throw new NotImplementedException();
	//    }
	//    public static void SetCurrentLanguage(LanguageData language) {
	//        throw new NotImplementedException();
	//    }
	//    public static IObservable<LanguageData> CurrentLanguage;
	//}
}
