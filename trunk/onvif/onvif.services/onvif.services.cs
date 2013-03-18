namespace onvif.services {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.ServiceModel;
	using System.Text;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.Serialization;
	using System.Xml.Linq;
	using System.Reflection;
	using System.Dynamic;
	using utils;

	public interface IBaseVideoEncoderOptions {

		/// <summary>
		/// List of supported image sizes.
		/// </summary>
		VideoResolution[] resolutionsAvailable { get; set; }
	
		/// <summary>
		/// Supported frame rate in fps (frames per second).
		/// </summary>
		IntRange frameRateRange { get; set; }

		/// <summary>
		/// Supported encoding interval range. The encoding interval corresponds to the number of frames devided by the encoded frames. An encoding interval value of "1" means that all frames are encoded.
		/// </summary>
		IntRange encodingIntervalRange { get; set; }
	}
	public partial class JpegOptions : IBaseVideoEncoderOptions {
	}
	public partial class Mpeg4Options : IBaseVideoEncoderOptions {
	}
	public partial class H264Options : IBaseVideoEncoderOptions {
	}
	public partial class JpegOptions2 : IBaseVideoEncoderOptions {
	}
	public partial class Mpeg4Options2 : IBaseVideoEncoderOptions {
	}
	public partial class H264Options2 : IBaseVideoEncoderOptions {
	}


	public interface IBaseVideoDecoderOptions {
		/// <summary>
		/// List of supported Video Resolutions
		/// </summary>
		VideoResolution[] resolutionsAvailable { get; set; }

		/// <summary>
		/// Supported bitrate range in kbps
		/// </summary>
		IntRange supportedInputBitrate { get; set; }

		/// <summary>
		/// Supported framerate range in fps
		/// </summary>
		IntRange supportedFrameRate { get; set; }
	}
	public partial class JpegDecOptions : IBaseVideoDecoderOptions {
	}
	public partial class H264DecOptions : IBaseVideoDecoderOptions {
	}
	public partial class Mpeg4DecOptions : IBaseVideoDecoderOptions {
	}

	public interface IBaseAudioDecoderOptions {
		/// <summary>
		/// List of supported bitrates in kbps
		/// </summary>
		IntList bitrate { get; set; }

		/// <summary>
		/// List of supported sample rates in kHz
		/// </summary>
		IntList sampleRateRange { get; set; }
	}
	public partial class AACDecOptions : IBaseAudioDecoderOptions {
	}
	public partial class G711DecOptions : IBaseAudioDecoderOptions {
	}
	public partial class G726DecOptions : IBaseAudioDecoderOptions {
	}

	public interface IDeviceEntity {
		string token { get; set; }
	}
	public partial class VideoOutput : IDeviceEntity {
	}
	public partial class AudioOutput : IDeviceEntity {
	}
	public partial class NetworkInterface : IDeviceEntity {
	}
	public partial class RelayOutput : IDeviceEntity {
	}
	public partial class PTZNode : IDeviceEntity {
	}

	public interface IConfigurationEntity {
		string token { get; set; }
		string name { get; set; }
		int useCount { get; set; }
	}
	public static class ConfigurationEntityExtensions {
		public static string GetName(this IConfigurationEntity cgfEntity) {
			var name = cgfEntity.name;
			if (!String.IsNullOrWhiteSpace(name)) {
				return name;
			}
			return cgfEntity.token;
		}
	}

	public partial class ConfigurationEntity : IConfigurationEntity {
	}
	public partial class VideoSourceConfiguration : IConfigurationEntity {
	}
	public partial class VideoEncoderConfiguration : IConfigurationEntity {
	}
	public partial class AudioSourceConfiguration : IConfigurationEntity {
	}
	public partial class AudioEncoderConfiguration : IConfigurationEntity {
	}
	public partial class MetadataConfiguration : IConfigurationEntity {
	}
	public partial class VideoOutputConfiguration : IConfigurationEntity {
	}
	//public partial class AudioOutputConfiguration : IConfigurationEntity {
	//}
	//public partial class AudioDecoderConfiguration : IConfigurationEntity {
	//}
	public partial class PTZConfiguration : IConfigurationEntity {
	}
	public partial class VideoAnalyticsConfiguration : IConfigurationEntity {
	}
	public partial class AnalyticsEngine : IConfigurationEntity {
	}
	public partial class AnalyticsEngineInput : IConfigurationEntity {
	}
	public partial class AnalyticsEngineControl : IConfigurationEntity {
	}


	public class QNameListType : IXmlSerializable, IEnumerable<XmlQualifiedName> {
		public QNameListType(IEnumerable<XmlQualifiedName> qnames) {
			m_qnames = qnames.ToArray();
		}
		private static char[] whiteSpaceChars = { ' ', '\t', '\n', '\r' };
		public XmlSchema GetSchema() {
			return null;
		}
		private XmlQualifiedName[] m_qnames = { };
		private IEnumerable<XmlQualifiedName> ParseQNameList(String text, Func<string, string> LookupNs) {
			var qnames = text.Split(whiteSpaceChars, StringSplitOptions.RemoveEmptyEntries);
			foreach (var qn in qnames) {
				//Parse QName
				int si = qn.IndexOf(':');
				string ns;
				string ln;
				if (si != -1) {
					string prefix = qn.Substring(0, si);
					ns = LookupNs(prefix);
					if (ns == null) {
						throw new XmlException(String.Format("prefix {0} in {1} is undefined", prefix, qn));
					}
					ln = qn.Substring(si + 1);
					if (ln == string.Empty) {
						throw new XmlException(String.Format("no local name is defined in qname {0}", qn));
					}
				} else {
					ns = string.Empty;
					ln = qn;
				}
				ln = XmlConvert.DecodeName(ln);
				yield return new XmlQualifiedName(ln, ns);
			}
		}
		public void ReadXml(XmlReader reader) {
			string text = reader.ReadString();
			if (!string.IsNullOrEmpty(text)) {
				m_qnames = ParseQNameList(text, (ns) => reader.LookupNamespace(ns)).ToArray();
			}
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer) {
			int prefixCount = 0;
			var emptyNsDeclared = false;
			StringBuilder sb = new StringBuilder();
			foreach (var qn in m_qnames) {
				if (sb.Length != 0) {
					sb.Append(' ');
				}
				//SerializationUtility.PrepareQNameString(stringBuilder, ref flag, ref num, writer, current);
				string ln = XmlConvert.EncodeLocalName(qn.Name.Trim());
				string prefix;
				if (qn.Namespace.Length == 0) {
					if (!emptyNsDeclared) {
						writer.WriteAttributeString("xmlns", string.Empty, null, string.Empty);
						emptyNsDeclared = true;
					}
					prefix = null;
				} else {
					prefix = writer.LookupPrefix(qn.Namespace);
					if (prefix == null) {
						prefix = "l" + prefixCount++;
						writer.WriteAttributeString("xmlns", prefix, null, qn.Namespace);
					}
				}
				if (!string.IsNullOrEmpty(prefix)) {
					sb.AppendFormat("{0}:{1}", prefix, ln);
				} else {
					sb.Append(ln);
				}
			}
			writer.WriteString(sb.ToString());
		}

		public IEnumerator<XmlQualifiedName> GetEnumerator() {
			return m_qnames.AsEnumerable().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return m_qnames.GetEnumerator();
		}
	}

	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = false)]
	public partial class SupportedRules {
	}

	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = true)]
	public partial class SupportedAnalyticsModules {
	}

	[Serializable]
	[XmlType(AnonymousType = true, Namespace = "http://www.onvif.org/ver10/schema")]
	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = false)]
	public class Message {
		public ItemList Source { get; set; }
		public ItemList Key { get; set; }
		public ItemList Data { get; set; }
		public MessageExtension Extension { get; set; }
		[XmlAttribute]
		public System.DateTime UtcTime { get; set; }
		[XmlAttribute]
		public PropertyOperation PropertyOperation { get; set; }
		[XmlIgnore]
		public bool PropertyOperationSpecified { get; set; }
		[XmlAnyAttribute]
		public XmlAttribute[] AnyAttr { get; set; }
	}

	[Serializable]
	[XmlType(Namespace = "http://www.onvif.org/ver10/schema")]
	public enum PropertyOperation {
		Initialized,
		Deleted,
		Changed,
	}

	[Serializable]
	[XmlType(Namespace = "http://www.onvif.org/ver10/schema")]
	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = true)]
	public class MessageExtension {
		[XmlAnyElement]
		public XmlElement[] Any { get; set; }
	}
	public partial class Profile {
		public override string ToString() {
			if (String.IsNullOrWhiteSpace(this.name)) {
				return this.token;
			}
			return this.name;
		}
	}
	public partial class PTZSpeed {
		public override string ToString() {
			return new StringBuilder()
				.Append("(pan=")
				.Append(this.panTilt.x.ToString(CultureInfo.InvariantCulture))
				.Append(", tilt=")
				.Append(this.panTilt.y.ToString(CultureInfo.InvariantCulture))
				.Append(", zoom=")
				.Append(this.zoom.x.ToString(CultureInfo.InvariantCulture))
				.Append(")").ToString();
		}
	}
	public partial class FloatRange {
		public override string ToString() {
			return new StringBuilder()
				.Append("(")
				.Append(this.min.ToString(CultureInfo.InvariantCulture))
				.Append(", ")
				.Append(this.max.ToString(CultureInfo.InvariantCulture))
				.Append(")").ToString();
		}
	}

	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = true)]
	public partial class IntRectangle {
		public override string ToString() {
			return new StringBuilder()
				.Append("(")
				.Append(x).Append(", ")
				.Append(y).Append(", ")
				.Append(width).Append(", ")
				.Append(height)
				.Append(")").ToString();
		}
	}

	public partial class ConfigurationEntity {
		public override string ToString() {
			if (this.name != null) {
				return this.name;
			}
			return this.token;
		}
	}

	[XmlRoot(Namespace = "http://www.onvif.org/ver10/schema", IsNullable = true)]
	public partial class VideoResolution : IComparable<VideoResolution>, IComparable {
		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}
			var other = obj as VideoResolution;
			return other != null && (width == other.width) && (height == other.height);
		}
		public override int GetHashCode() {
			return HashCode.Combine(width, height);
		}
		public override string ToString() {
			return String.Format("{0}x{1}", width, height);
		}

		public int CompareTo(VideoResolution other) {
			if (width != other.width) {
				return width - other.width;
			}
			return height - other.height;
		}

		public int CompareTo(object obj) {
			return CompareTo((VideoResolution)obj);
		}
	}

	/// <remarks/>
	[Serializable]
	[XmlType(Namespace = "http://www.onvif.org/ver10/schema")]
	public partial class AttachmentData{
		[XmlText(DataType = "base64Binary")]
		public byte[] Include;
		[XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/2005/05/xmlmime")]
		public string contentType;
	}

	public class XsDuration : IXmlSerializable {
		public TimeSpan timeSpan;

		public XmlSchema GetSchema() {
			return null;
		}

		public static XsDuration FromSeconds(double seconds) {
			return new XsDuration() {
				timeSpan = TimeSpan.FromSeconds(seconds)
			};
		}
		public static implicit operator XsDuration(TimeSpan timeSpan) {
			return new XsDuration() {
				timeSpan = timeSpan
			};
		}

		public static implicit operator TimeSpan(XsDuration duration) {
			return duration.timeSpan;
		}

		public void ReadXml(XmlReader reader) {
			var txt = reader.ReadElementContentAsString();
			if (String.IsNullOrWhiteSpace(txt)) {
				return;
			}
			timeSpan = XmlConvert.ToTimeSpan(txt.Replace(',', '.'));
		}

		public void WriteXml(XmlWriter writer) {
			writer.WriteString(XmlConvert.ToString(timeSpan));
		}
		public string Format() {
			return XmlConvert.ToString(timeSpan);
		}
		public static XsDuration Parse(string value) {
			var duration = new XsDuration();
			duration.timeSpan = XmlConvert.ToTimeSpan(value.Replace(',', '.'));
			return duration;
		}
		public override string ToString() {
			return XmlConvert.ToString(timeSpan);
		}
		public override bool Equals(object obj) {
			var dur = obj as XsDuration;
			return dur != null && timeSpan.Equals(dur.timeSpan);
		}
		public override int GetHashCode() {
			return timeSpan.GetHashCode();
		}

		public static XsDuration FromSeconds(int totalSeconds){
			return new XsDuration(){
				timeSpan = TimeSpan.FromSeconds(totalSeconds)
			};
		}
	}

	/// <remarks/>
	[Serializable]
	[XmlType(AnonymousType=true, TypeName = "QueryExpressionType", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
    public class QueryExpressionType {
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces namespaces;

		[XmlText]
		[XmlAnyElement(Order = 0)]
		public XmlNode[] Any;

		[XmlAttribute(DataType = "anyURI")]
		public string Dialect;
	}

	[XmlRoot(ElementName = "MessageContent", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public class MessageContentFilter {
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces namespaces;
		[XmlText]
		public string expression;
		[XmlAttribute(AttributeName = "Dialect", DataType = "anyURI")]
		public string dialect;
	}

	[XmlRoot(ElementName = "TopicExpression", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public class TopicExpressionFilter {
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces namespaces;
		[XmlText]
		public string expression;
		[XmlAttribute(AttributeName = "Dialect", DataType = "anyURI")]
		public string dialect;
	}

	[Serializable]
	//[XmlType(Namespace = "http://docs.oasis-open.org/wsn/b-2")]
	public class FilterType : IXmlSerializable {
		public XElement[] Any;
		public void ReadXml(XmlReader reader) {
			var xelist = new LinkedList<XElement>();
			reader.Read();
			var dr = reader as XmlDictionaryReader;
			var gns = reader.GetNamespacesInScope();

			while (reader.NodeType != XmlNodeType.EndElement) {
				if (reader.NodeType == XmlNodeType.Element) {
					var x = XElement.ReadFrom(reader) as XElement;
					foreach (var ns in gns) {
						var pref = ns.Key;
						var uri = ns.Value;
						if (x.GetNamespaceOfPrefix(pref) == null) {
							x.Add(new XAttribute(XName.Get(pref, "http://www.w3.org/2000/xmlns/"), uri));
						}
					}
					xelist.AddLast((XElement)x);
				} else {
					reader.Skip();
				}
			}
			Any = xelist.ToArray();
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer) {
			if (Any != null) {
				foreach (var x in Any) {
					x.WriteTo(writer);
				}
			}
		}

		public XmlSchema GetSchema() {
			return null;
		}
	}

	[Serializable]
	[XmlType(Namespace = "http://www.onvif.org/ver10/schema")]
	public class OnvifVersion : IComparable, IComparable<OnvifVersion>, IEquatable<OnvifVersion> {
		[XmlElement(Order = 0)]
		public int Major {get;set;}

		/// <remarks/>
		[XmlElement(Order = 1)]
		public int Minor { get; set; }

		public OnvifVersion() {
		}

		public OnvifVersion(int Major, int Minor) {
			this.Major = Major;
			this.Minor = Minor;
		}

		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}
			return Equals(obj as OnvifVersion);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Major, Minor);
		}

		public override string ToString() {
			if (Minor == 0) {
				return String.Format("{0}.0", Major);
			}
			return String.Format("{0}.{1:D2}", Major, Minor);
		}

		public int CompareTo(OnvifVersion other) {
			if (Major == other.Major) {
				return Minor - other.Minor;
			}
			return Major - other.Major;
		}

		public int CompareTo(object obj) {
			return CompareTo((OnvifVersion)obj);
		}

		public bool Equals(OnvifVersion other) {
			return other != null && (Minor == other.Minor) && (Major == other.Major);
		}
	}
	public static class DateTimeExtensions{
		public static System.DateTime? ToSystemDateTime(this DateTime dateTime, DateTimeKind kind = DateTimeKind.Unspecified) {
			if ((object)dateTime == null || (object)dateTime.date == null || (object)dateTime.time == null) {
				return null;
			}
			// seconds can be in range 0..61 (leap seconds), see http://www.onvif.org/onvif/ver10/device/wsdl/devicemgmt.wsdl, section 38
			// leap seconds are not supported in current implementation
			var seconds = dateTime.time.second;
			if(seconds > 59){
				seconds = 59;
			}
			return new System.DateTime(
					dateTime.date.year, 1, 1, 0, 0, seconds, kind
				)
				.AddMonths(dateTime.date.month - 1)
				.AddDays(dateTime.date.day - 1)
				.AddHours(dateTime.time.hour)
				.AddMinutes(dateTime.time.minute);
		}
	}
}