namespace onvif {
	using System;
	using System.Diagnostics;
	using System.Xml.Serialization;
	using System.Collections;
	using System.Xml.Schema;
	using System.ComponentModel;
	using System.Runtime.Serialization;

	[Serializable]
	public class Token {
		public Token() {
			this.value = null;
		}

		public Token(string value) {
			this.value = value;
		}

		[XmlText]
		public string value;

		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}
			if (this.GetType() != obj.GetType()) {
				return false;
			}

			return (this == (Token)obj);
		}

		public bool Equals(Token token) {
			if (token == null) {
				return false;
			}
			return (this == token);
		}

		public static bool operator ==(Token left, Token right) {
			if (object.ReferenceEquals(left, right)) {
				return true;
			}

			if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null)) {
				return false;
			}

			return left.value == right.value;
		}

		public static bool operator !=(Token left, Token right) {
			return !(left == right);
		}

		public override int GetHashCode() {
			if (value == null) {
				return 0;
			}
			return value.GetHashCode();
		}

		public override string ToString() {
			return value;
		}



		public string ToString(string format, IFormatProvider formatProvider) {
			throw new NotImplementedException();
		}
	}

	public class VideoSourceToken : Token {
		public VideoSourceToken() {
		}
		public VideoSourceToken(string value)
			: base(value) {
		}
	}

	public class ProfileToken : Token {
		public ProfileToken() {
		}
		public ProfileToken(string value)
			: base(value) {
		}
	}

	public class MetadataConfigurationToken : Token {
		public MetadataConfigurationToken() {
		}
		public MetadataConfigurationToken(string value)
			: base(value) {
		}
	}

	public class VideoEncoderConfigurationToken : Token {
		public VideoEncoderConfigurationToken() {
		}
		public VideoEncoderConfigurationToken(string value)
			: base(value) {
		}
	}

	public class VideoAnalyticsConfigurationToken : Token {
		public VideoAnalyticsConfigurationToken() {
		}
		public VideoAnalyticsConfigurationToken(string value)
			: base(value) {
		}
	}

	public class VideoSourceConfigurationToken : Token {
		public VideoSourceConfigurationToken() {
		}
		public VideoSourceConfigurationToken(string value)
			: base(value) {
		}
	}
}