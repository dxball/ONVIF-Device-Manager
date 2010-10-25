using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Globalization;

using nvc;
using nvc.onvif;
using onvifdm.utils;
using onvif.services.media;
using onvif.services.analytics;
using media = onvif.services.media;
using analytics = onvif.services.analytics;
using tt = onvif.types;
using System.Xml.Serialization;


namespace nvc.models {
	[Serializable]
	public class Marker1D {
		//[Serializable]
		//public class Line{
		//    [XmlAttribute]
		//    public int x;
		//    [XmlAttribute]
		//    public int bottom;
		//    [XmlAttribute]
		//    public int top;
		//}
		[XmlElement]
		public int height;
		[XmlElement]
		public tt::Polyline line1;
		[XmlElement]
		public tt::Polyline line2;
	}

	public partial class DepthCalibrationModel : ModelBase<DepthCalibrationModel> {
		ChannelDescription m_channel;
		public DepthCalibrationModel(ChannelDescription channel) {
			m_channel = channel;
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<DepthCalibrationModel> observer) {
			AnalyticsObservable analytics = null;
			yield return session.GetAnalyticsClient().Handle(x => analytics = x);
			DebugHelper.Assert(analytics != null);		

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			if (profile == null) {
				yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			}
			DebugHelper.Assert(profile != null);

			VideoAnalyticsConfiguration vac = profile.VideoAnalyticsConfiguration;
			if (vac == null) {
				VideoAnalyticsConfiguration[] comp_vacs = null;
				yield return session.GetCompatibleVideoAnalyticsConfigurations(profile.token).Handle(x=> comp_vacs = x);
				DebugHelper.Assert(comp_vacs != null);
				vac = comp_vacs.FirstOrDefault();
				yield return session.AddVideoAnalyticsConfiguration(profile.token, vac.token).Idle();
				profile.VideoAnalyticsConfiguration = vac;
			}

			var sceneMod = vac.AnalyticsEngineConfiguration.AnalyticsModule.Where(x => x.Type == new XmlQualifiedName("SceneCalibrator")).FirstOrDefault();
						
			if (sceneMod == null) {
				var sceneMod_defaults = vac.AnalyticsEngineConfiguration.Extension.Any
							.Where(x => x.LocalName == "DefaultModule" && x.NamespaceURI == "http://www.onvif.org/ver10/schema")
							.Select(x => x.Deserialize<media::Config>())
							.Where(x => x.Type.Name == "SceneCalibrator")
							.FirstOrDefault();
				vac.AnalyticsEngineConfiguration.AnalyticsModule = vac.AnalyticsEngineConfiguration.AnalyticsModule.Append(sceneMod_defaults).ToArray();
				yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();
				sceneMod = sceneMod_defaults;
				//yield return analytics.CreateAnalyticsModules(vac.token, new media::Config[] { defMod }).Idle();
			}
			//var 
			
			var roi = sceneMod.Parameters.ElementItem
				.Where(x => x.Name == "roi")
				.Select(x=>x.Any.Deserialize<tt::Polygon>())
				.FirstOrDefault();
			if (roi != null) {
				region = (roi.Point??new tt::Vector[0])
					.Select(p => new Point((int)p.x, (int)p.y))
					.ToList();
			} else {
				region = null;
			}
			
			focalLength = sceneMod.Parameters
				.SimpleItem
				.Where(x => x.Name == "focal_length")
				.Select(x => int.Parse(x.Value))
				.FirstOrDefault();

			matrixFormat = sceneMod.Parameters
				.SimpleItem
				.Where(x => x.Name == "matrix_format")
				.Select(x => x.Value)
				.FirstOrDefault();

			photosensorPixelSize = sceneMod.Parameters
				.SimpleItem
				.Where(x => x.Name == "photosensor_pixel_size")
				.Select(x => float.Parse(x.Value, NumberFormatInfo.InvariantInfo))
				.FirstOrDefault();

			Marker1D m1 = new Marker1D();
			m1.height = sceneMod.Parameters
				.SimpleItem
				.Where(x => x.Name == "marker0_physical_height")
				.Select(x => int.Parse(x.Value))
				.FirstOrDefault();

			m1.line1 = sceneMod.Parameters
				.ElementItem
				.Where(x => x.Name == "marker0_line0")
				.Select(x => x.Any.Deserialize<tt::Polyline>())
				.FirstOrDefault();

			m1.line2 = sceneMod.Parameters
				.ElementItem
				.Where(x => x.Name == "marker0_line1")
				.Select(x => x.Any.Deserialize<tt::Polyline>())
				.FirstOrDefault();

			markers = new Marker1D[] { m1 };
			
			bounds.X = profile.VideoSourceConfiguration.Bounds.x;
			bounds.Y = profile.VideoSourceConfiguration.Bounds.y;
			bounds.Width = profile.VideoSourceConfiguration.Bounds.width;
			bounds.Height = profile.VideoSourceConfiguration.Bounds.height;

			encoderResolution.Width = profile.VideoEncoderConfiguration.Resolution.Width;
			encoderResolution.Height = profile.VideoEncoderConfiguration.Resolution.Height;

			NotifyPropertyChanged(x => x.region);
			NotifyPropertyChanged(x => x.focalLength);
			NotifyPropertyChanged(x => x.matrixFormat);
			NotifyPropertyChanged(x => x.photosensorPixelSize);
			NotifyPropertyChanged(x => x.encoderResolution);
			NotifyPropertyChanged(x => x.bounds);

			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => mediaUri = x);
			DebugHelper.Assert(mediaUri != null);
			NotifyPropertyChanged(x => x.mediaUri);

			isModified = true;

			if (observer != null) {
				observer.OnNext(this);
			}
		}
		
		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<DepthCalibrationModel> observer) {

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			var vac = profile.VideoAnalyticsConfiguration;
			if (vac == null) {
				VideoAnalyticsConfiguration[] vacs = null;
				yield return session.GetCompatibleVideoAnalyticsConfigurations(profile.token).Handle(x => vacs = x);
				vac = vacs.OrderBy(x => x.UseCount).FirstOrDefault();
				yield return session.AddVideoAnalyticsConfiguration(profile.token, vac.token).Idle();
				profile.VideoAnalyticsConfiguration = vac;
			}

			var sceneMod = vac.AnalyticsEngineConfiguration.AnalyticsModule.Where(x => x.Type == new XmlQualifiedName("SceneCalibrator")).FirstOrDefault();
			if (sceneMod == null) {
				media::Config defMod = vac.AnalyticsEngineConfiguration.Extension.Any
							.Where(x => x.LocalName == "DefaultModule" && x.NamespaceURI == "http://www.onvif.org/ver10/schema")
							.Select(x => x.Deserialize<media::Config>())
							.Where(x => x.Type.Name == "SceneCalibrator")
							.FirstOrDefault();
				vac.AnalyticsEngineConfiguration.AnalyticsModule = vac.AnalyticsEngineConfiguration.AnalyticsModule.Append(defMod).ToArray();
				yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();
				sceneMod = defMod;
				//yield return analytics.CreateAnalyticsModules(vac.token, new media::Config[] { defMod }).Idle();
			}

			var roi = sceneMod.Parameters.ElementItem
				.Where(x => x.Name == "roi")
				.FirstOrDefault();
			roi.Any = new tt::Polygon() {
				Point = region.Select(p => new tt::Vector() {
					x = p.X,
					xSpecified = true,
					y = p.Y,
					ySpecified = true
				}).ToArray()
			}.Serialize();

			var m1 = markers[0];

			sceneMod.Parameters
				.SimpleItem
				.Where(x => x.Name == "marker0_physical_height")
				.FirstOrDefault()
				.Value = m1.height.ToString();

			foreach (var p in m1.line1.Point) {
				p.xSpecified = true;
				p.ySpecified = true;
			}

			foreach (var p in m1.line2.Point) {
				p.xSpecified = true;
				p.ySpecified = true;
			}

			sceneMod.Parameters
				.ElementItem
				.Where(x => x.Name == "marker0_line0")
				.FirstOrDefault()
				.Any = m1.line1.Serialize();

			sceneMod.Parameters
				.ElementItem
				.Where(x => x.Name == "marker0_line1")
				.FirstOrDefault()
				.Any = m1.line2.Serialize();

			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();

			if (observer != null) {
				observer.OnNext(this);
			}

		}

		public int focalLength { get; set; }
		public string matrixFormat { get; set; }
		public float photosensorPixelSize { get; set; }
		public Marker1D[] markers { get; set; }
		public System.Drawing.Rectangle bounds;
		public List<Point> region { get; set; }
		public Size encoderResolution;

		public string mediaUri {
			get;
			private set;
		}
	}

	public class HeigthMarker {
		int physikalHeigth { get; set; }
		System.Drawing.Rectangle marker { get; set; }
	}
}
