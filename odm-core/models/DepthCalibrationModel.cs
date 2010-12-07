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
	//[Serializable]
	public class Marker {
		//[Serializable]
		//public class Line{
		//    [XmlAttribute]
		//    public int x;
		//    [XmlAttribute]
		//    public int bottom;
		//    [XmlAttribute]
		//    public int top;
		//}
		//[XmlElement]
		public tt::Vector size;

		//[XmlElement]
		public tt::Polyline line1;

		//[XmlElement]
		public tt::Polyline line2;

		public static tt::IntRectangle GetRectFromPolyline(tt::Polyline line) {
			var rect = new tt.IntRectangle();
			rect.x = Math.Min((int)line.Point[0].x, (int)line.Point[1].x);
			rect.y = Math.Min((int)line.Point[0].y, (int)line.Point[1].y);
			rect.width = Math.Abs((int)line.Point[1].x - (int)line.Point[0].x);
			rect.height = Math.Abs((int)line.Point[1].y - (int)line.Point[0].y);
			return rect;
		}

		public static tt::Polyline GetPolylineFromRect(tt::IntRectangle rect) {
			return new tt::Polyline() {
				Point = new tt.Vector[]{
					new tt::Vector(){
						x = rect.x,
			            xSpecified = true,
			            y = rect.y,
			            ySpecified = true
					},
					new tt::Vector(){
						x = rect.x + rect.width,
						xSpecified = true,
						y = rect.y + rect.height,
						ySpecified = true,
					}
				}
			};
		}

		//[XmlIgnore]
		//public tt::IntRectangle rect1 {
		//    get {
		//        return GetRectFromPolyline(line1);
		//    }
		//}

		//[XmlIgnore]
		//public tt::IntRectangle rect2 {
		//    get {
		//        return GetRectFromPolyline(line2);
		//    }
		//}
	}

	public enum MarkerType {
		marker1D,
		marker2D
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

			yield return session.AddDefaultVideoAnalytics(profile).Idle();
			//yield return session.AddDefaultMetadata(profile).Idle();
			
			//var meta = profile.MetadataConfiguration;
			//if (!meta.AnalyticsSpecified || !meta.Analytics) {
			//    meta.AnalyticsSpecified = true;
			//    meta.Analytics = true;
			//    yield return media.SetMetadataConfiguration(meta, true).Idle();
			//}
			
			VideoAnalyticsConfiguration vac = profile.VideoAnalyticsConfiguration;
			DebugHelper.Assert(vac != null);

			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "SceneCalibrator").Handle(x=>module = x);
			DebugHelper.Assert(module != null);

			var roi = module.Parameters.ElementItem
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

			focalLength = module.GetSimpleItemAsInt("focal_length");
			matrixFormat = module.GetSimpleItem("matrix_format");
			photosensorPixelSize = module.GetSimpleItemAsFloat("photosensor_pixel_size");
			use2DMarkers = module.GetSimpleItemAsBool("use_2d_markers");

			Marker m1 = new Marker();
			m1.size = module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_size")
					.Select(x => x.Any.Deserialize<tt::Vector>())
					.FirstOrDefault();
			
			if (!use2DMarkers) {				

				m1.line1 = module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_line0")
					.Select(x => x.Any.Deserialize<tt::Polyline>())
					.FirstOrDefault();

				m1.line2 = module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_line1")
					.Select(x => x.Any.Deserialize<tt::Polyline>())
					.FirstOrDefault();

			} else {

				m1.line1 = module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_rect0")
					.Select(x => x.Any.Deserialize<tt::IntRectangle>())
					.Select(x => Marker.GetPolylineFromRect(x))
					.FirstOrDefault();

				m1.line2 = module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_rect1")
					.Select(x => x.Any.Deserialize<tt::IntRectangle>())
					.Select(x => Marker.GetPolylineFromRect(x))
					.FirstOrDefault();
			}

			markers = new Marker[] { m1 };
			
			bounds.X = profile.VideoSourceConfiguration.Bounds.x;
			bounds.Y = profile.VideoSourceConfiguration.Bounds.y;
			bounds.Width = profile.VideoSourceConfiguration.Bounds.width;
			bounds.Height = profile.VideoSourceConfiguration.Bounds.height;

			encoderResolution = new Size() {
				Width = profile.VideoEncoderConfiguration.Resolution.Width,
				Height = profile.VideoEncoderConfiguration.Resolution.Height
			};

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
				yield return session.AddDefaultVideoAnalytics(profile).Handle(x=> vac=x);
				profile.VideoAnalyticsConfiguration = vac;
			}

			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "SceneCalibrator").Handle(x => module = x);
			DebugHelper.Assert(module != null);

			var roi = module.Parameters.ElementItem
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

			module.SetSimpleItemAsBool("use_2d_markers", use2DMarkers);
			module.SetSimpleItemAsInt("focal_length", focalLength);
			module.SetSimpleItem("matrix_format", matrixFormat);
			module.SetSimpleItemAsFloat("photosensor_pixel_size", photosensorPixelSize);
			
			module.Parameters
				.ElementItem
				.Where(x => x.Name == "marker0_size")
				.FirstOrDefault()
				.Any = m1.size.Serialize();

			foreach (var p in m1.line1.Point) {
				p.xSpecified = true;
				p.ySpecified = true;
			}

			foreach (var p in m1.line2.Point) {
				p.xSpecified = true;
				p.ySpecified = true;
			}

			if (!use2DMarkers) {
				module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_line0")
					.FirstOrDefault()
					.Any = m1.line1.Serialize();

				module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_line1")
					.FirstOrDefault()
					.Any = m1.line2.Serialize();
			} else {

				module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_rect0")
					.FirstOrDefault()
					.Any = Marker.GetRectFromPolyline(m1.line1).Serialize();
				
				module.Parameters
					.ElementItem
					.Where(x => x.Name == "marker0_rect1")
					.FirstOrDefault()
					.Any = Marker.GetRectFromPolyline(m1.line2).Serialize();
				
			}

			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();

			if (observer != null) {
				observer.OnNext(this);
			}

		}

		public int focalLength { get; set; }
		public string matrixFormat { get; set; }
		public float photosensorPixelSize { get; set; }
		public Marker[] markers { get; set; }
		public bool use2DMarkers{ get; set; }
		public System.Drawing.Rectangle bounds;
		public List<Point> region { get; set; }
		public Size encoderResolution{ get; private set; }

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