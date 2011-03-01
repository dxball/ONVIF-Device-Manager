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
using med = global::onvif.services.media;
using analytics = global::onvif.services.analytics;
using syn = global::synesis.onvif.extensions;
using tt = global::onvif.types;
using onvif;
using onvif.types;

namespace odm.models {

	public partial class ChannelModel : ModelBase<ChannelModel> {
		ProfileToken m_profileToken;
		public ChannelModel(ProfileToken profileToken) {
			this.m_profileToken = profileToken;
		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<ChannelModel> observer) {

			Capabilities caps = null;
			//med::VideoSource[] vsources = null;
			med::Profile[] profiles = null;
			var channels = new List<Channel>();

			yield return Observable.Merge(
				//session.GetVideoSources().Handle(x => vsources = x ?? new med::VideoSource[] { }),
				session.GetProfiles().Handle(x => profiles = x),
				session.GetCapabilities().Handle(x => caps = x)
			);

			//dbg.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();

			yield return session.AddDefaultVideoEncoder(profile).Idle();

			//Image snapshot = null;
			//yield return session.GetSnapshot(profile.token).Handle(x=>snapshot = x);
			var modules = new AnalyticsModules();

			if (caps.Analytics != null && caps.Analytics.AnalyticsModuleSupport) {
				yield return session.AddDefaultVideoAnalytics(profile).Idle();
				var vac = profile.VideoAnalyticsConfiguration;

				if (vac != null) {
					foreach (var m in vac.AnalyticsEngineConfiguration.Extension.Any.Select(x => x.Deserialize<syn::DefaultModule>()).Select(x => x.type.Name)) {
						modules[m] = false;
					}

					foreach (var m in vac.AnalyticsEngineConfiguration.AnalyticsModule.Select(x => x.Type.Name)) {
						modules[m] = true;
					}
				}
			}


			var res = new Size() {
				Width = profile.VideoEncoderConfiguration.Resolution.Width,
				Height = profile.VideoEncoderConfiguration.Resolution.Height
			};

			var streamSetup = new med::StreamSetup();
			streamSetup.Stream = med::StreamType.RTPUnicast;
			streamSetup.Transport = new med::Transport();
			streamSetup.Transport.Protocol = med::TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			string uri = null;
			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => uri = x);
			dbg.Assert(uri != null);

			this.profileToken = profile.token;
			this.sourceToken = profile.VideoSourceConfiguration.SourceToken;
			this.name = profile.Name;
			this.modules = modules;
			this.encoderResolution = res;
			this.mediaUri = uri;

			NotifyPropertyChanged(x => x.profileToken);
			NotifyPropertyChanged(x => x.sourceToken);
			NotifyPropertyChanged(x => x.name);
			NotifyPropertyChanged(x => x.modules);
			NotifyPropertyChanged(x => x.encoderResolution);
			NotifyPropertyChanged(x => x.mediaUri);

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected bool HasAnalytics() {
			dbg.Assert(modules != null);
			return modules.Any(x => x.Value);
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<ChannelModel> observer) {
            MediaObservable media = null;
            Capabilities caps = null;
            med::VideoSource[] vsources = null;
            med::Profile[] profiles = null;
            

            yield return Observable.Merge(
                session.GetMediaClient().Handle(x => media = x),
                session.GetVideoSources().Handle(x => vsources = x ?? new med::VideoSource[] { }),
                session.GetProfiles().Handle(x => profiles = x),
                session.GetCapabilities().Handle(x => caps = x)
            );

            //dbg.Assert(profiles != null);
            var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();

			if (HasAnalytics()) {
				yield return session.AddDefaultVideoAnalytics(profile).Idle();
			} else if (profile.VideoAnalyticsConfiguration != null) {
				yield return media.RemoveVideoAnalyticsConfiguration(profile.token).Idle();
				profile.VideoAnalyticsConfiguration = null;
			}
            
            var vac = profile.VideoAnalyticsConfiguration;

			if (vac != null) {
				foreach (var m in modules) {
					if (m.Value) {
						yield return session.GetVideoAnalyticModule(profile, m.Key).Idle();
					} else {
						yield return session.RemoveVideoAnalyticModule(profile, m.Key).Idle();
					}
				};
			}

            if (observer != null){
                observer.OnNext(this);
            }
		}

		public override void RevertChanges() {

		}

		public ProfileToken profileToken {get;private set;}
		public VideoSourceToken sourceToken {get;private set;}
		public string name {get;private set;}
		public Size encoderResolution {get;private set;}
		public string mediaUri {get;private set;}
		//public Capabilities Capabilities {get;}
		//public Image snapshot{get;}
		public AnalyticsModules modules {get;private set;}


	}
}