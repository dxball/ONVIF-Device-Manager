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

	public static class XmlExtensions {

		public static T Deserialize<T>(this XmlNode xmlNode) {
			var serializer = new XmlSerializer(typeof(T));
			using (var reader = new XmlNodeReader(xmlNode)) {
				return (T)serializer.Deserialize(reader);
			}
		}
		
		public static T Deserialize<T>(this XmlReader xmlReader) {
			var serializer = new XmlSerializer(typeof(T));
			return (T)serializer.Deserialize(xmlReader);			
		}

		public static XmlElement Serialize(this object obj) {
			var serializer = new XmlSerializer(obj.GetType());
			var xmlDoc = new XmlDocument();

			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj);
			}

			return xmlDoc.DocumentElement;
		}

		public static XmlElement Serialize<T>(this T obj) {
			var serializer = new XmlSerializer(typeof(T));
			var xmlDoc = new XmlDocument();

			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj);
			}

			return xmlDoc.DocumentElement;
		}
		public static XmlElement Serialize<T>(this T obj, XmlSerializerNamespaces ns) {

			var serializer = new XmlSerializer(typeof(T));
			var xmlDoc = new XmlDocument();

			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj, ns);
			}

			return xmlDoc.DocumentElement;
		}
		public static XmlElement Serialize<T>(this T obj, XmlQualifiedName root) {
			var rootAttr = new XmlRootAttribute(root.Name);
			rootAttr.Namespace = root.Namespace;
			var serializer = new XmlSerializer(typeof(T), rootAttr);
			var xmlDoc = new XmlDocument();

			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj);
			}

			return xmlDoc.DocumentElement;
		}
	}
}
