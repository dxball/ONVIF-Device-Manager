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

namespace nvc.models {

	public static class ConfigExtensions{
		public static string GetSimpleItem(this media::Config config, string name) {
			var item = config.Parameters.SimpleItem.FirstOrDefault(x => x.Name == name);
			if (item == null) {
				return null;
			}
			return item.Value;
		}

		public static float GetSimpleItemAsFloat(this media::Config config, string name) {
			var str = config.GetSimpleItem(name);
			if (str == null) {
				return default(float);
			}
			return float.Parse(str, NumberFormatInfo.InvariantInfo);
		}

		public static int GetSimpleItemAsInt(this media::Config config, string name) {
			var str = config.GetSimpleItem(name);
			if (str == null) {
				return default(int);
			}
			return int.Parse(str, NumberFormatInfo.InvariantInfo);
		}

		public static bool GetSimpleItemAsBool(this media::Config config, string name) {
			var str = config.GetSimpleItem(name);
			if (str == null) {
				return false;
			}
			return BoolHelper.parse(str);
		}

		public static void SetSimpleItem(this media::Config config, string name, string value) {
			var item = config.Parameters.SimpleItem.FirstOrDefault(x=>x.Name == name);
			if (item == null) {
				item = new media::ItemListSimpleItem() {
					Name = name
				};
				config.Parameters.SimpleItem = config.Parameters.SimpleItem.Append(item).ToArray();
			}
			item.Value = value;
		}

		public static void SetSimpleItemAsFloat(this media::Config config, string name, float value) {
			config.SetSimpleItem(name, value.ToString(NumberFormatInfo.InvariantInfo));
		}

		public static void SetSimpleItemAsInt(this media::Config config, string name, int value) {
			config.SetSimpleItem(name, value.ToString(NumberFormatInfo.InvariantInfo));
		}
		
		public static void SetSimpleItemAsBool(this media::Config config, string name, bool value) {
			config.SetSimpleItem(name, value ? "true" : "false");
		}

	}

	public partial class ObjectTrackerModel : ModelBase<ObjectTrackerModel> {
		ChannelDescription m_channel;
		public ObjectTrackerModel(ChannelDescription channel) {
			m_channel = channel;
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<ObjectTrackerModel> observer) {
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
			
			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "ObjectTracker").Handle(x => module = x);
			DebugHelper.Assert(module != null);

			minObjectArea = module.GetSimpleItemAsFloat("min_object_area");
			maxObjectArea = module.GetSimpleItemAsFloat("max_object_area");
			maxObjectSpeed = module.GetSimpleItemAsInt("max_object_speed");
			contrastSensitivity = module.GetSimpleItemAsInt("contrast_sensivity");
			displacementSensitivity = module.GetSimpleItemAsInt("displacement_sensivity");
			stabilizationTime = module.GetSimpleItemAsInt("stabilization_time");

			rose_up = module.GetSimpleItemAsInt("rose_up");
			rose_up_right = module.GetSimpleItemAsInt("rose_up_right");
			rose_right = module.GetSimpleItemAsInt("rose_right");
			rose_down_right = module.GetSimpleItemAsInt("rose_down_right");
			rose_down = module.GetSimpleItemAsInt("rose_down");
			rose_down_left = module.GetSimpleItemAsInt("rose_down_left");
			rose_left = module.GetSimpleItemAsInt("rose_left");
			rose_up_left = module.GetSimpleItemAsInt("rose_up_left");

			encoderResolution = new Size() {
				Width = profile.VideoEncoderConfiguration.Resolution.Width,
				Height = profile.VideoEncoderConfiguration.Resolution.Height
			};
			
			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => mediaUri = x);
			DebugHelper.Assert(mediaUri != null);

			NotifyPropertyChanged(x => x.rose_up);
			NotifyPropertyChanged(x => x.rose_up_right);
			NotifyPropertyChanged(x => x.rose_right);
			NotifyPropertyChanged(x => x.rose_down_right);
			NotifyPropertyChanged(x => x.rose_down);
			NotifyPropertyChanged(x => x.rose_down_left);
			NotifyPropertyChanged(x => x.rose_left);
			NotifyPropertyChanged(x => x.rose_up_left);
			NotifyPropertyChanged(x => x.encoderResolution);
			NotifyPropertyChanged(x => x.mediaUri);
			NotifyPropertyChanged(x => x.isModified);

			isModified = true;

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<ObjectTrackerModel> observer) {

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			var vac = profile.VideoAnalyticsConfiguration;
			if (vac == null) {
				VideoAnalyticsConfiguration[] vacs =null;
				yield return session.GetCompatibleVideoAnalyticsConfigurations(profile.token).Handle(x=>vacs = x);
				vac = vacs.OrderBy(x=>x.UseCount).FirstOrDefault();
				yield return session.AddVideoAnalyticsConfiguration(profile.token, vac.token).Idle();
				profile.VideoAnalyticsConfiguration = vac;
			}
			
			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "ObjectTracker").Handle(x => module = x);

			module.SetSimpleItemAsFloat("min_object_area", minObjectArea);
			module.SetSimpleItemAsFloat("max_object_area", maxObjectArea);

			module.SetSimpleItemAsInt("max_object_speed", maxObjectSpeed);
			module.SetSimpleItemAsInt("contrast_sensivity", contrastSensitivity);
			module.SetSimpleItemAsInt("displacement_sensivity", displacementSensitivity);
			module.SetSimpleItemAsFloat("stabilization_time", stabilizationTime);

			module.SetSimpleItemAsFloat("rose_up", rose_up);
			module.SetSimpleItemAsFloat("rose_up_right", rose_up_right);
			module.SetSimpleItemAsFloat("rose_right", rose_right);
			module.SetSimpleItemAsFloat("rose_down_right", rose_down_right);
			module.SetSimpleItemAsFloat("rose_down", rose_down);
			module.SetSimpleItemAsFloat("rose_down_left", rose_down_left);
			module.SetSimpleItemAsFloat("rose_left", rose_left);
			module.SetSimpleItemAsFloat("rose_up_left", rose_up_left);

			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();
			if (observer != null) {
				observer.OnNext(this);
			}				
		}

		//private int m_displacementSensitivity;
		//private int m_contrastSensitivity;
		//private float m_minObjectArea;
		//private float m_maxObjectArea;
		//private int m_maxObjectSpeed;
		//private float m_stabilizationTime;

		public int displacementSensitivity { get; set; }
		public int contrastSensitivity { get; set; }
		public float minObjectArea { get; set; }
		public float maxObjectArea { get; set; }
		public int maxObjectSpeed { get; set; }
		public float stabilizationTime { get; set; }

		public float rose_up;
		public float rose_up_right;
		public float rose_right;
		public float rose_down_right;
		public float rose_down;
		public float rose_down_left;
		public float rose_left;
		public float rose_up_left;

		public Size encoderResolution{get; private set;}

		public string mediaUri {
			get;
			private set;
		}
	}
}