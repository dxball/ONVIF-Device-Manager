using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using odm.onvif;
using odm.utils;

using onvif.types;
using onvif.services.device;
using onvif.services.media;

using dev = global::onvif.services.device;
using med = global::onvif.services.media;
using tt = global::onvif.types;
using syn = global::synesis.onvif.extensions;

using System.Xml;
using onvif;
using System.Collections;

namespace odm.models {

	public class AnalyticsModules:IEnumerable<KeyValuePair<string, bool>> {
		private Dictionary<string, bool> m_dic = new Dictionary<string,bool>();
		public bool? sceneCalibrator{get{return this["SceneCalibrator"];}set{this["SceneCalibrator"]=value;}}
		public bool? serviceDetectors{get{return this["ServiceDetectors"];}set{this["ServiceDetectors"]=value;}}
		public bool? displayAnnotation{get{return this["Display"];}set{this["Display"]=value;}}
		public bool? objectTracker{get{return this["ObjectTracker"];}set{this["ObjectTracker"]=value;}}
		public bool? ruleEngine{get{return this["RuleEngine"];}set{this["RuleEngine"]=value;}}
		public bool? digitalAntishaker{get{return this["DigitalAntishaker"];}set{this["DigitalAntishaker"]=value;}}
		public bool? approMotionDetector{get{return this["ApproMotionDetector"];}set{this["ApproMotionDetector"]=value;}}
		public bool? this[string moduleType]{
			get {
				bool enabled;
				if (!m_dic.TryGetValue(moduleType, out enabled)) {
					return null;
				}
				return enabled;
			}
			set{
				if(value != null){
					m_dic[moduleType] = value.Value;
				}else{
					m_dic.Remove(moduleType);
				}
			}
		}

		public bool isEmpty {
			get {
				return count == 0;
			}
		}

		public int count {
			get {
				return m_dic.Count;
			}
		}

		public IEnumerator<KeyValuePair<string, bool>> GetEnumerator() {
			return m_dic.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return (m_dic as IEnumerable).GetEnumerator();
		}
	}

	public partial class DeviceCapabilityModel : ModelBase<DeviceCapabilityModel> {
		
		public DeviceCapabilityModel() {

		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<DeviceCapabilityModel> observer) {
			
			Capabilities caps = null;
			med::VideoSource[] vsources = null;
			//med::Profile[] profiles = null;
			var channels = new List<Channel>();
			
			yield return Observable.Merge(
				session.GetVideoSources().Handle(x => vsources = x ?? new med::VideoSource[] { }),
				//session.GetProfiles().Handle(x => profiles = x),
				session.GetCapabilities().Handle(x => caps = x),
				session.GetDeviceInfo().Handle(x => devInfo = x)
			);
			
			//dbg.Assert(profiles != null);
			
			//}
			capabilities = caps;
			videoSources = vsources.Select(x => x.token).ToArray();
			//AnalyticsCapabilities analyticsField;
			//DeviceCapabilities deviceField;
			//EventCapabilities eventsField;
			//ImagingCapabilities imagingField;
			//MediaCapabilities mediaField;
			//PTZCapabilities pTZField;
			
			//m_Channels = channels.ToArray();
			
			if (observer != null) {
				observer.OnNext(this);
			}
		}

		

		
		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<DeviceCapabilityModel> observer) {

			throw new NotImplementedException();

			//MediaObservable media = null;
			//Capabilities caps = null;
			//med::VideoSource[] vsources = null;
			//med::Profile[] profiles = null;
			//var channels = new List<Channel>();
			
			
			////yield return Observable.Merge(
			//yield return session.GetMediaClient().Handle(x => media = x);
			//yield return session.GetVideoSources().Handle(x => vsources = x ?? new med::VideoSource[] { });
			//yield return session.GetProfiles().Handle(x => profiles = x);
			//yield return session.GetCapabilities().Handle(x => caps = x);
			////);
			
			//dbg.Assert(profiles != null);
			//foreach (var channel in m_Channels) {
			//    var profile_token = NvcHelper.GetChannelProfileToken(channel.Id);
			//    var profile = profiles.Where(x => x.token == profile_token).FirstOrDefault();
			//    if (profile == null) {
			//        yield return session.CreateDefaultProfile(channel.Id).Handle(x => profile = x);
			//    }

			//    //if (HasAnalytics(channel)) {
			//    //    yield return session.AddDefaultVideoAnalytics(profile).Idle();
			//    //} else if (profile.VideoAnalyticsConfiguration != null) {
			//    //    yield return media.RemoveVideoAnalyticsConfiguration(profile.token).Idle();
			//    //    profile.VideoAnalyticsConfiguration = null;
			//    //}
			//    var vac = profile.VideoAnalyticsConfiguration;

			//    if (vac != null) {
			//        if (channel.modules.DigitalAntishaker.GetValueOrDefault(false)) {
			//            yield return session.GetVideoAnalyticModule(profile, "DigitalAntishaker").Idle();
			//        } else if(channel.modules.DigitalAntishaker.HasValue) {
			//            yield return session.RemoveVideoAnalyticModule(profile, "DigitalAntishaker").Idle();
			//        }

			//        if (channel.modules.DisplayAnnotation.GetValueOrDefault(false)) {
			//            yield return session.GetVideoAnalyticModule(profile, "Display").Idle();
			//        } else if(channel.modules.DisplayAnnotation.HasValue){
			//            yield return session.RemoveVideoAnalyticModule(profile, "Display").Idle();
			//        }

			//        if (channel.modules.SceneCalibrator.GetValueOrDefault(false)) {
			//            yield return session.GetVideoAnalyticModule(profile, "SceneCalibrator").Idle();
			//        } else if(channel.modules.SceneCalibrator.HasValue) {
			//            yield return session.RemoveVideoAnalyticModule(profile, "SceneCalibrator").Idle();
			//        }

			//        if (channel.modules.ObjectTracker.GetValueOrDefault(false)) {
			//            yield return session.GetVideoAnalyticModule(profile, "ObjectTracker").Idle();
			//        } else if(channel.modules.ObjectTracker.HasValue) {
			//            yield return session.RemoveVideoAnalyticModule(profile, "ObjectTracker").Idle();
			//        }

			//        if (channel.modules.RuleEngine.GetValueOrDefault(false)) {
			//            yield return session.GetVideoAnalyticModule(profile, "RuleEngine").Idle();
			//        } else if(channel.modules.RuleEngine.HasValue) {
			//            yield return session.RemoveVideoAnalyticModule(profile, "RuleEngine").Idle();
			//        }

			//        if (channel.modules.ServiceDetectors.GetValueOrDefault(false)) {
			//            yield return session.GetVideoAnalyticModule(profile, "ServiceDetectors").Idle();
			//        } else if (channel.modules.ServiceDetectors.HasValue) {
			//            yield return session.RemoveVideoAnalyticModule(profile, "ServiceDetectors").Idle();
			//        }
			//    }
			//}
			
			//if (observer != null) {
			//    observer.OnNext(this);
			//}
		}

		public VideoSourceToken[] videoSources;
		//private ChannelDescription[] m_Channels;
		//public IEnumerable<ChannelDescription> Channels { 
		//    get{
		//        return m_Channels;
		//    } 
		//}
		public Capabilities capabilities;
		//Image _deviceImage;
		//public Image DeviceImage {
		//    get {
		//        return _deviceImage;
		//    }
		//    set {
		//        _deviceImage = value;
		//    }
		//}
		public DeviceInfo devInfo{get; private set;}
		
	}
}
