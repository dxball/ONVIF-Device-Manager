using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System;

namespace utils {

	public static class XmlExtensions {

		public static Object Deserialize(this XmlNode xmlNode, Type type) {
			var xroot = type.GetCustomAttribute<XmlRootAttribute>();
			if (xroot == null) {
				var xtype = type.GetCustomAttribute<XmlTypeAttribute>();
				if (xtype != null && xmlNode.NodeType == XmlNodeType.Element) {
					return Deserialize(xmlNode, type, new XmlQualifiedName(xmlNode.LocalName, xmlNode.NamespaceURI));
				}
			}

			var serializer = new XmlSerializer(type);
			using (var reader = new XmlNodeReader(xmlNode)) {
				return Deserialize(reader, type);
			}
		}

		public static Object Deserialize(this XmlNode xmlNode, Type type, XmlQualifiedName root) {
			using (var reader = new XmlNodeReader(xmlNode)) {
				return Deserialize(reader, type, root);
			}
		}

		public static T Deserialize<T>(this XmlNode xmlNode) {
			return (T)Deserialize(xmlNode, typeof(T));
		}

		public static T Deserialize<T>(this XmlNode xmlNode, XmlQualifiedName root) {
			using (var reader = new XmlNodeReader(xmlNode)) {
				return Deserialize<T>(reader, root);
			}
		}

		public static Object Deserialize(this XNode xNode, Type type) {
			var xroot = type.GetCustomAttribute<XmlRootAttribute>();
			if (xroot == null) {
				var xtype = type.GetCustomAttribute<XmlTypeAttribute>();
				if (xtype != null && xNode.NodeType == XmlNodeType.Element) {
					var xElement = (XElement)xNode;
					return Deserialize(xNode, type, xElement.Name);
				}
			}
			using (var reader = xNode.CreateReader()) {
				return Deserialize(reader, type);
			}
		}

		public static Object Deserialize(this XNode xNode, Type type, XName root) {
			using (var reader = xNode.CreateReader()) {
				return Deserialize(reader, type, root);
			}
		}

		public static T Deserialize<T>(this XNode xNode) {
			return (T)Deserialize(xNode, typeof(T));
		}

		public static T Deserialize<T>(this XNode xNode, XName root) {
			return (T)Deserialize(xNode, typeof(T), root);
		}

		public static Object Deserialize(this XmlReader xmlReader, Type type) {
			var serializer = new XmlSerializer(type);
			return serializer.Deserialize(xmlReader);
		}

		public static Object Deserialize(this XmlReader xmlReader, Type type, string rootName, string rootNamespace) {
			var rootAttr = new XmlRootAttribute(rootName);
			rootAttr.Namespace = rootNamespace;
			var serializer = new XmlSerializer(type, rootAttr);
			return serializer.Deserialize(xmlReader);
		}

		public static Object Deserialize(this XmlReader xmlReader, Type type, XmlQualifiedName root) {
			if (root != null) {
				return Deserialize(xmlReader, type, root.Name, root.Namespace);
			}
			return Deserialize(xmlReader, type);
		}

		public static Object Deserialize(this XmlReader xmlReader, Type type, XName root) {
			if (root != null) {
				return Deserialize(xmlReader, type, root.LocalName, root.NamespaceName);
			}
			return Deserialize(xmlReader, type);
		}

		public static T Deserialize<T>(this XmlReader xmlReader) {
			return (T)Deserialize(xmlReader, typeof(T));
		}

		public static T Deserialize<T>(this XmlReader xmlReader, XmlQualifiedName root) {
			return (T)Deserialize(xmlReader, typeof(T), root);
		}

		public static T Deserialize<T>(this XmlReader xmlReader, XName root) {
			return (T)Deserialize(xmlReader, typeof(T), root.LocalName, root.NamespaceName);
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
			var ns = new XmlSerializerNamespaces();
			var xroot = typeof(T).GetCustomAttribute<XmlRootAttribute>();
			if (xroot != null) {
				ns.Add("", xroot.Namespace);
			} else {
				ns.Add("", "");
			}
			return obj.Serialize(ns);
		}

		public static XElement SerializeAsXElement<T>(this T obj) {
			var ns = new XmlSerializerNamespaces();
			var xroot = typeof(T).GetCustomAttribute<XmlRootAttribute>();
			if (xroot != null) {
				ns.Add("", xroot.Namespace);
			} else {
				ns.Add("", "");
			}
			return obj.SerializeAsXElement(ns);
		}

		public static XElement SerializeAsXElement<T>(this T obj, XmlSerializerNamespaces ns) {
			var serializer = new XmlSerializer(typeof(T));
			var xd = new XDocument();
			using (var writer = xd.CreateWriter()) {
				serializer.Serialize(writer, obj, ns);
			}
			var root = xd.Root;
			root.Remove();
			return root;
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

		public static XmlElement ToXmlElement(this XElement xelement){
			var xmlDoc = new XmlDocument();
			using (var reader = xelement.CreateReader()) {
				xmlDoc.Load(reader);
			}
			return xmlDoc.DocumentElement;
		}
		public static XName ToXName(this XmlQualifiedName qname) {
			return XName.Get(qname.Name, qname.Namespace);
		}

		public static IDictionary<string, string> GetNamespacesInScope(this XmlReader reader) {
			if (reader == null) {
				return null;
			}
			var xmlResolver = reader as IXmlNamespaceResolver;
			if (xmlResolver != null) {
				return xmlResolver.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);
			}
			var asmSysRuntimeSerialization = Assembly.Load("System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			var t = asmSysRuntimeSerialization.GetType("System.Xml.XmlBaseReader");
			var readerType = reader.GetType();
			if (t.IsAssignableFrom(readerType)) {
				var nsMgr = ExposedObject.From(ExposedObject.From(reader).nsMgr);
				var namespaces = nsMgr.namespaces as object[];
				if (namespaces == null) {
					return null;
				}
				var dic = new Dictionary<string, string>();
				for (int i = nsMgr.nsCount - 1; i >= 0; i--) {
					var ns = ExposedObject.From(namespaces[i]);
					if (ns == null) {
						continue;
					}
					var prefix = ExposedObject.From(ns.Prefix);
					if (prefix == null || prefix.IsEmpty || prefix.IsXml || prefix.IsXmlns) {
						continue;
					}
					var key = prefix.GetString() as string;
					if(String.IsNullOrEmpty(key)){
						continue;
					}
					var uri = ExposedObject.From(ns.Uri);
					string value = uri != null ? uri.GetString() : null;
					dic[key] = value ?? String.Empty;
				}
				return dic;
			}
			return null;
		}
	}
}
