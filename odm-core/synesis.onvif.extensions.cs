using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using tt = onvif.types;
using med = global::onvif.services.media;


namespace synesis.onvif.extensions {
	
	[Serializable]
	[XmlType(Namespace = "http://www.onvif.org/ver10/schema")]
	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = true)]
	public class DefaultModule {

		[XmlElement("Parameters", Order = 0)]
		public med::ItemList parameters;

		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Type")]
		public XmlQualifiedName type;

		public static implicit operator med::Config(DefaultModule defMod) {
			return new med::Config() {
				Parameters = defMod.parameters,
				Name = defMod.name,
				Type = defMod.type
			};
		}
	}
}