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
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;

using nvc;
using System.Xml.Linq;

namespace onvifdm.config {

	//<logger-config>
	//	<columns>
	//		<column name="id" xpath="/log-message/id"/>
	//		<column name="source" xpath="/log-message/source"/>
	//		<column name="eventType" xpath="/log-message/eventType"/>
	//		<column name="message" xpath="/log-message/message"/>
	//	</columns>
	//	<details>
	//		<detail name="XML" xpath="" xslt=""/>
	//		<detail name="XML" xpath="" xslt=""/>
	//		<detail name="XML" xpath="" xslt=""/>
	//	</details>			 
	//</logger-config>

	public static class LoggerConfigAPI {
		private static string s_xmlFilePath = "logger.config.xml";
		public static LoggerConfig Load(string xmlFilePath = null) {
			if (xmlFilePath == null) {
				xmlFilePath = s_xmlFilePath;
			}
			var doc = new XmlDocument();
			doc.Load(xmlFilePath);
			var nav = doc.CreateNavigator();
			var ns = nav.GetNamespacesInScope(new XmlNamespaceScope());
			return doc.Deserialize<LoggerConfig>();			
		}
		public static void Save(LoggerConfig loggerConig, string xmlFilePath = null) {
			if (xmlFilePath == null) {
				xmlFilePath = s_xmlFilePath;
			}
			var doc = new XmlDocument();
			using (var w = new XmlTextWriter(xmlFilePath, Encoding.UTF8)) {
				w.Formatting = Formatting.Indented;
				var ns = new XmlSerializerNamespaces();
				ns.Add("", "");
				loggerConig.Serialize(ns).WriteTo(w);
			}
		}
	}
	
	[Serializable]
	[XmlRoot("logger-config")]
	public class LoggerConfig{
		
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces xmlns;

		[XmlArray("columns")]
		[XmlArrayItem("column")]
		public LoggerColumnConfig[] columns;

		[XmlArray("tabs")]
		[XmlArrayItem("tab")]
		public LoggerTabConfig[] tabs;

		public System.Xml.Schema.XmlSchema GetSchema() {
			return null;
		}

		public void ReadXml(XmlReader reader) {
			var doc = new XmlDocument();
			doc.Load(reader);
			var nav = doc.CreateNavigator();
			var ns = nav.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);
			nav.ReadSubtree();
			//nav.ReadSubtree
			//doc.Deserialize<LoggerConfig>();

		}

		public void WriteXml(XmlWriter writer) {
			throw new NotImplementedException();
		}
	}


	[Serializable]
	[XmlRoot("column")]
	public class LoggerColumnConfig {

		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces xmlns;

		[XmlAttribute("name")]
		public string name;

		[XmlAttribute("xpath")]
		public string xpath;
	}


	[Serializable]
	public class LoggerTabConfig {
		
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces xmlns;

		[XmlAttribute("name")]
		public string name;

		[XmlAttribute("xpath")]
		public string xpath;

		[XmlAttribute("xslt")]
		public string xslt;
	}
}
