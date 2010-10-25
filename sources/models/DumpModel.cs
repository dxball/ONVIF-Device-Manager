using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

using nvc.onvif;
using onvifdm.utils;

using onvif.services.device;
using onvif.services.media;
using onvif.types;

using device = global::onvif.services.device;
using media = global::onvif.services.media;
using tt = global::onvif.types;


namespace nvc.models {

	[Serializable]
	[XmlRoot("onvif-dump")]
	public class Dump {
		[XmlArray("scopes")]
		public Scope[] scopes;
		[XmlElement("capabilities")]
		public Capabilities capabilities;
		[XmlArray("profiles")]
		public media::Profile[] profiles;
		//[XmlArray("nics")]
		//public device::NetworkInterface[] nics;
		[XmlArray("video-sources", IsNullable = true)]
		public media::VideoSource[] videoSources;
		[XmlArray("video-source-cofigurations")]
		public media::VideoSourceConfiguration[] videoSourcesConfigurations;
		[XmlArray("video-encoder-cofigurations")]
		public media::VideoEncoderConfiguration[] videoEncoderConfigurations;
		[XmlArray("video-analytics-cofigurations")]
		public media::VideoAnalyticsConfiguration[] videoAnalyticsConfigurations;

	}

	public class DumpModel : ModelBase<DumpModel> {

		protected IEnumerable<IObservable<object>> LoadDumpImpl(onvif.Session session, IObserver<Dump> observer) {
			var dump = new Dump();

			DeviceObservable device = null;
			yield return session.GetDeviceClient().Handle(x => device = x);
			
			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);

			yield return session.GetScopes().Handle(x => dump.scopes = x).IgnoreError();
			//yield return session.GetNetworkInterfaces().Handle(x => dump.nics = x).IgnoreError();
			yield return session.GetCapabilities().Handle(x => dump.capabilities = x).IgnoreError();
			yield return session.GetProfiles().Handle(x => dump.profiles = x).IgnoreError();
			yield return session.GetVideoSources().Handle(x => dump.videoSources = x).IgnoreError();
			yield return session.GetVideoSourceConfigurations().Handle(x => dump.videoSourcesConfigurations = x).IgnoreError();
			yield return session.GetVideoEncoderConfigurations().Handle(x => dump.videoEncoderConfigurations = x).IgnoreError();
			yield return session.GetVideoAnalyticsConfigurations().Handle(x => dump.videoAnalyticsConfigurations = x).IgnoreError();

			//yield return session

			name = String.Format("{0} - {1}", NvcHelper.GetName(dump.scopes.Select(x=>x.ScopeItem)), System.DateTime.Now);
			NotifyPropertyChanged(x=>x.name);

			if (observer != null) {
				observer.OnNext(dump);
			}
		}

		public IObservable<Dump> LoadDump(onvif.Session session) {
			return Observable.Iterate<Dump>(observer=>LoadDumpImpl(session, observer));  
		}
		

		protected override IEnumerable<IObservable<object>> LoadImpl(onvif.Session session, IObserver<DumpModel> observer) {
			Dump dump =null;
			yield return LoadDump(session).Handle(x => dump = x);
			DebugHelper.Assert(dump != null);

			var xml = new XDocument();
			
			var ser = new XmlSerializer(typeof(Dump));
			using (var w = xml.CreateWriter()) {
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add("onvif", @"http://www.onvif.org/ver10/schema");
				ser.Serialize(w, dump, ns);
			}

			xmlDump = XPathNavigable.Create(()=>xml.CreateNavigator());
			NotifyPropertyChanged(x => x.xmlDump);
			
			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public string name {get; private set;}		
		public IXPathNavigable xmlDump {get;private set;}
	}
}
