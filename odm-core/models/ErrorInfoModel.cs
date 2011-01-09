using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.Xml.Serialization;

using odm.onvif;
using odm.utils;
using System.Runtime.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace odm.models {
	[Serializable]
	[XmlRoot("exception")]
	public class ErrorInfo {
		public static ErrorInfo Create(Exception error) {
			var errInfo = new ErrorInfo();
			errInfo.type = error.GetType().FullName;
			errInfo.message = error.Message;
			errInfo.source = error.Source;
			errInfo.stack = error.StackTrace;
			if (error.InnerException != null) {
				errInfo.inner = Create(error.InnerException);
			}
			return errInfo;
		}
		[XmlAttribute("type")]
		public string type = null;
		[XmlElement("inner-exception")]
		public ErrorInfo inner = null;
		[XmlElement("message")]
		public string message = null;
		[XmlElement("source")]
		public string source = null;
		[XmlElement("stack")]
		public string stack = null;
	}

	public class ErrorInfoModel  {
		public ErrorInfoModel(Exception error) {
			if (error == null) {
				dbg.Break();
				return;
			}

			var xml = new XDocument();
			var ser = new XmlSerializer(typeof(ErrorInfo));
			//strip default namespaces
			var xsn = new XmlSerializerNamespaces();
			xsn.Add("","");
			using (var w = xml.CreateWriter()) {
				ser.Serialize(w, ErrorInfo.Create(error), xsn);
			}
			xmlError = XPathNavigable.Create(() => xml.CreateNavigator());
		}
		
		public IXPathNavigable xmlError {get;private set;}
		private static object gate = new object();
		private static XslCompiledTransform s_xml2html = null;
		private static XslCompiledTransform xml2html {
			get {
				lock(gate){
					if (s_xml2html == null) {
						var xslt = new XslCompiledTransform();
						
						var xmlReaderSettings = new XmlReaderSettings() {
							DtdProcessing = DtdProcessing.Parse							
						};
						XsltSettings xsltSettings = new XsltSettings() {
							EnableScript = false,
							EnableDocumentFunction = false
						};
						
						using (var xmlReader = XmlReader.Create(@"xml2html/XmlToHtml10Basic.xslt", xmlReaderSettings)) {
							xslt.Load(xmlReader, xsltSettings, new XmlUrlResolver());
							xmlReader.Close();
						}
						s_xml2html = xslt;
					}
				}
				return s_xml2html;
			}
		}
		private string m_htmlError = null;
		public string htmlError{
			get {
				if (m_htmlError == null) {
					var html = new StringBuilder();
					var writer = new StringWriter(html);
					xml2html.Transform(xmlError, null, writer);
					m_htmlError = html.ToString();
				}

				return m_htmlError;
			}
		}

	}
}
