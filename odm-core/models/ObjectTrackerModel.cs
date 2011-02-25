using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Globalization;

using odm.onvif;
using odm.utils;
using onvif.services.media;
using onvif.services.analytics;
using media = global::onvif.services.media;
using analytics = global::onvif.services.analytics;
using tt = global::onvif.types;
using onvif;

namespace odm.models {

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

		public static bool? GetSimpleItemAsBoolNullable(this media::Config config, string name) {
			var str = config.GetSimpleItem(name);
			if (str == null) {
				return null;
			}
			return BoolHelper.parse(str);
		}

		public static void SetSimpleItem(this media::Config config, string name, string value) {
			if (config.Parameters == null) {
				config.Parameters = new media::ItemList();
			}
			if (config.Parameters.SimpleItem == null) {
				config.Parameters.SimpleItem = new media::ItemListSimpleItem[] { };
			}
			var item = config.Parameters.SimpleItem.FirstOrDefault(x => x.Name == name);
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
		//ChannelDescription m_channel;
		//public ObjectTrackerModel(ChannelDescription channel) {
		//    m_channel = channel;
		//}
		ProfileToken m_profileToken;
		public ObjectTrackerModel(ProfileToken profileToken) {
			this.m_profileToken = profileToken;
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<ObjectTrackerModel> observer) {
			AnalyticsObservable analytics = null;
			yield return session.GetAnalyticsClient().Handle(x => analytics = x);
			dbg.Assert(analytics != null);		

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			//if (profile == null) {
			//    yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			//}
			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();
			dbg.Assert(profile != null);

			yield return session.AddDefaultVideoAnalytics(profile).Idle();
			
			media::Config module = null;
			yield return session.GetVideoAnalyticModule(profile, "ObjectTracker").Handle(x => module = x);
			dbg.Assert(module != null);

			media::Config sceneModule = null;
			yield return session.GetVideoAnalyticModule(profile, "SceneCalibrator").Handle(x => sceneModule = x);
			dbg.Assert(sceneModule != null);

			m_minObjectArea.SetBoth(module.GetSimpleItemAsFloat("min_object_area"));
			m_maxObjectArea.SetBoth(module.GetSimpleItemAsFloat("max_object_area"));
			m_maxObjectSpeed.SetBoth(module.GetSimpleItemAsInt("max_object_speed"));
			m_contrastSensitivity.SetBoth(module.GetSimpleItemAsInt("contrast_sensivity"));
			m_displacementSensitivity.SetBoth(module.GetSimpleItemAsInt("displacement_sensivity"));
			m_stabilizationTime.SetBoth(module.GetSimpleItemAsInt("stabilization_time"));

			m_rose_up.SetBoth(sceneModule.GetSimpleItemAsFloat("rose_up"));
			m_rose_up_right.SetBoth(sceneModule.GetSimpleItemAsFloat("rose_up_right"));
			m_rose_right.SetBoth(sceneModule.GetSimpleItemAsFloat("rose_right"));
			m_rose_down_right.SetBoth(sceneModule.GetSimpleItemAsFloat("rose_down_right"));
			m_rose_down.SetBoth(sceneModule.GetSimpleItemAsFloat("rose_down"));
			m_rose_down_left.SetBoth(sceneModule.GetSimpleItemAsFloat("rose_down_left"));
			m_rose_left.SetBoth(sceneModule.GetSimpleItemAsFloat("rose_left"));
			m_rose_up_left.SetBoth(sceneModule.GetSimpleItemAsFloat("rose_up_left"));

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
			dbg.Assert(mediaUri != null);

			NotifyPropertyChanged(x => x.minObjectArea);
			NotifyPropertyChanged(x => x.maxObjectArea);
			NotifyPropertyChanged(x => x.maxObjectSpeed);
			NotifyPropertyChanged(x => x.contrastSensitivity);
			NotifyPropertyChanged(x => x.displacementSensitivity);
			NotifyPropertyChanged(x => x.stabilizationTime);

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

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<ObjectTrackerModel> observer) {

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();
			dbg.Assert(profile != null);

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
			dbg.Assert(module != null);

			media::Config sceneModule = null;
			yield return session.GetVideoAnalyticModule(profile, "SceneCalibrator").Handle(x => sceneModule = x);
			dbg.Assert(sceneModule != null);

			module.SetSimpleItemAsFloat("min_object_area", minObjectArea);
			module.SetSimpleItemAsFloat("max_object_area", maxObjectArea);

			module.SetSimpleItemAsInt("max_object_speed", maxObjectSpeed);
			module.SetSimpleItemAsInt("contrast_sensivity", contrastSensitivity);
			module.SetSimpleItemAsInt("displacement_sensivity", displacementSensitivity);
			module.SetSimpleItemAsFloat("stabilization_time", stabilizationTime);

			sceneModule.SetSimpleItemAsFloat("rose_up", rose_up);
			sceneModule.SetSimpleItemAsFloat("rose_up_right", rose_up_right);
			sceneModule.SetSimpleItemAsFloat("rose_right", rose_right);
			sceneModule.SetSimpleItemAsFloat("rose_down_right", rose_down_right);
			sceneModule.SetSimpleItemAsFloat("rose_down", rose_down);
			sceneModule.SetSimpleItemAsFloat("rose_down_left", rose_down_left);
			sceneModule.SetSimpleItemAsFloat("rose_left", rose_left);
			sceneModule.SetSimpleItemAsFloat("rose_up_left", rose_up_left);

			yield return media.SetVideoAnalyticsConfiguration(vac, true).Idle();

			yield return Observable.Concat(LoadImpl(session, observer)).Idle();
		}

		public override void RevertChanges() {

			m_minObjectArea.Revert();
			m_maxObjectArea.Revert();
			m_maxObjectSpeed.Revert();
			m_contrastSensitivity.Revert();
			m_displacementSensitivity.Revert();
			m_stabilizationTime.Revert();

			m_rose_up.Revert();
			m_rose_up_right.Revert();
			m_rose_right.Revert();
			m_rose_down_right.Revert();
			m_rose_down.Revert();
			m_rose_down_left.Revert();
			m_rose_left.Revert();
			m_rose_up_left.Revert();			

			NotifyPropertyChanged(x => x.minObjectArea);
			NotifyPropertyChanged(x => x.maxObjectArea);
			NotifyPropertyChanged(x => x.maxObjectSpeed);
			NotifyPropertyChanged(x => x.contrastSensitivity);
			NotifyPropertyChanged(x => x.displacementSensitivity);
			NotifyPropertyChanged(x => x.stabilizationTime);

			NotifyPropertyChanged(x => x.rose_up);
			NotifyPropertyChanged(x => x.rose_up_right);
			NotifyPropertyChanged(x => x.rose_right);
			NotifyPropertyChanged(x => x.rose_down_right);
			NotifyPropertyChanged(x => x.rose_down);
			NotifyPropertyChanged(x => x.rose_down_left);
			NotifyPropertyChanged(x => x.rose_left);
			NotifyPropertyChanged(x => x.rose_up_left);
		}
		//private int m_displacementSensitivity;
		//private int m_contrastSensitivity;
		//private float m_minObjectArea;
		//private float m_maxObjectArea;
		//private int m_maxObjectSpeed;
		//private float m_stabilizationTime;

		private ChangeTrackingProperty<int> m_displacementSensitivity = new ChangeTrackingProperty<int>();
		public int displacementSensitivity {
			get {
				return m_displacementSensitivity.current;
			}
			set {
				if (m_displacementSensitivity.current != value) {
					m_displacementSensitivity.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.displacementSensitivity);
				}
			}
		}

		private ChangeTrackingProperty<int> m_contrastSensitivity = new ChangeTrackingProperty<int>();
		public int contrastSensitivity {
			get {
				return m_contrastSensitivity.current;
			}
			set{
				if(m_contrastSensitivity.current!=value){
					m_contrastSensitivity.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.contrastSensitivity);
				}
			}
		}

		private ChangeTrackingProperty<float> m_minObjectArea = new ChangeTrackingProperty<float>();
		public float minObjectArea {
			get{return m_minObjectArea.current;}
			set {
				if (m_minObjectArea.current != value) {
					m_minObjectArea.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.minObjectArea);
				}
		}}

		private ChangeTrackingProperty<float> m_maxObjectArea = new ChangeTrackingProperty<float>();
		public float maxObjectArea {
			get {
				return m_maxObjectArea.current;
			}
			set {
				if (m_maxObjectArea.current != value) {
					m_maxObjectArea.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.maxObjectArea);
				}
		}}

		private ChangeTrackingProperty<int> m_maxObjectSpeed = new ChangeTrackingProperty<int>();
		public int maxObjectSpeed {
			get {
				return m_maxObjectSpeed.current;
			}
			set {
				if (m_maxObjectSpeed.current != value) {
					m_maxObjectSpeed.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.maxObjectSpeed);
				}
		}}


		private ChangeTrackingProperty<float> m_stabilizationTime = new ChangeTrackingProperty<float>();
		public float stabilizationTime {
			get{return m_stabilizationTime.current;}
			set {
				if (m_stabilizationTime.current != value) {
					m_stabilizationTime.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.stabilizationTime);
				}
		}}

		private ChangeTrackingProperty<float> m_rose_up = new ChangeTrackingProperty<float>();
		public float rose_up {
			get {
				return m_rose_up.current;
			}
			set {
				if (m_rose_up.current != value) {
					m_rose_up.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.rose_up);
				}
			}
		}

		private ChangeTrackingProperty<float> m_rose_up_right = new ChangeTrackingProperty<float>();
		public float rose_up_right {
			get {
				return m_rose_up_right.current;
			}
			set {
				if (m_rose_up_right.current != value) {
					m_rose_up_right.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.rose_up_right);
				}
			}
		}

		private ChangeTrackingProperty<float> m_rose_right = new ChangeTrackingProperty<float>();
		public float rose_right {
			get {
				return m_rose_right.current;
			}
			set {
				if (m_rose_right.current != value) {
					m_rose_right.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.rose_right);
				}
			}
		}

		private ChangeTrackingProperty<float> m_rose_down_right = new ChangeTrackingProperty<float>();
		public float rose_down_right {
			get {
				return m_rose_down_right.current;
			}
			set {
				if (m_rose_down_right.current != value) {
					m_rose_down_right.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.rose_down_right);
				}
			}
		}

		private ChangeTrackingProperty<float> m_rose_down = new ChangeTrackingProperty<float>();
		public float rose_down {
			get {
				return m_rose_down.current;
			}
			set {
				if (m_rose_down.current != value) {
					m_rose_down.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.rose_down);
				}
			}
		}

		private ChangeTrackingProperty<float> m_rose_down_left = new ChangeTrackingProperty<float>();
		public float rose_down_left {
			get {
				return m_rose_down_left.current;
			}
			set {
				if (m_rose_down_left.current != value) {
					m_rose_down_left.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.rose_down_left);
				}
			}
		}

		private ChangeTrackingProperty<float> m_rose_left = new ChangeTrackingProperty<float>();
		public float rose_left {
			get {
				return m_rose_left.current;
			}
			set {
				if (m_rose_left.current != value) {
					m_rose_left.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.rose_left);
				}
			}
		}

		private ChangeTrackingProperty<float> m_rose_up_left = new ChangeTrackingProperty<float>();
		public float rose_up_left {
			get {
				return m_rose_up_left.current;
			}
			set {
				if (m_rose_up_left.current != value) {
					m_rose_up_left.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.rose_up_left);
				}
			}
		}

		public Size encoderResolution{get; private set;}

		public string mediaUri {
			get;
			private set;
		}
	}
}