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

        protected bool HasAnalytics()
        {
            if (modules == null)
            {
                return false;
            }
            if (modules.DigitalAntishaker.HasValue && modules.DigitalAntishaker.Value)
            {
                return true;
            }
            if (modules.DisplayAnnotation.HasValue && modules.DisplayAnnotation.Value)
            {
                return true;
            }
            if (modules.ObjectTracker.HasValue && modules.ObjectTracker.Value)
            {
                return true;
            }
            if (modules.RuleEngine.HasValue && modules.RuleEngine.Value)
            {
                return true;
            }
            if (modules.SceneCalibrator.HasValue && modules.SceneCalibrator.Value)
            {
                return true;
            }
            if (modules.ServiceDetectors.HasValue && modules.ServiceDetectors.Value)
            {
                return true;
            }
            return false;
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

            if (HasAnalytics())
            {
                yield return session.AddDefaultVideoAnalytics(profile).Idle();
            }
            else if (profile.VideoAnalyticsConfiguration != null)
            {
                yield return media.RemoveVideoAnalyticsConfiguration(profile.token).Idle();
                profile.VideoAnalyticsConfiguration = null;
            }
            var vac = profile.VideoAnalyticsConfiguration;

            if (vac != null)
            {
                
                if (modules.DigitalAntishaker.GetValueOrDefault(false))
                {
                    yield return session.GetVideoAnalyticModule(profile, "DigitalAntishaker").Idle();
                }
                else if (modules.DigitalAntishaker.HasValue)
                {
                    yield return session.RemoveVideoAnalyticModule(profile, "DigitalAntishaker").Idle();
                }

                if (modules.DisplayAnnotation.GetValueOrDefault(false))
                {
                    yield return session.GetVideoAnalyticModule(profile, "Display").Idle();
                }
                else if (modules.DisplayAnnotation.HasValue)
                {
                    yield return session.RemoveVideoAnalyticModule(profile, "Display").Idle();
                }

                if (modules.SceneCalibrator.GetValueOrDefault(false))
                {
                    yield return session.GetVideoAnalyticModule(profile, "SceneCalibrator").Idle();
                }
                else if (modules.SceneCalibrator.HasValue)
                {
                    yield return session.RemoveVideoAnalyticModule(profile, "SceneCalibrator").Idle();
                }

                if (modules.ObjectTracker.GetValueOrDefault(false))
                {
                    yield return session.GetVideoAnalyticModule(profile, "ObjectTracker").Idle();
                }
                else if (modules.ObjectTracker.HasValue)
                {
                    yield return session.RemoveVideoAnalyticModule(profile, "ObjectTracker").Idle();
                }

                if (modules.RuleEngine.GetValueOrDefault(false))
                {
                    yield return session.GetVideoAnalyticModule(profile, "RuleEngine").Idle();
                }
                else if (modules.RuleEngine.HasValue)
                {
                    yield return session.RemoveVideoAnalyticModule(profile, "RuleEngine").Idle();
                }

                if (modules.ServiceDetectors.GetValueOrDefault(false))
                {
                    yield return session.GetVideoAnalyticModule(profile, "ServiceDetectors").Idle();
                }
                else if (modules.ServiceDetectors.HasValue)
                {
                    yield return session.RemoveVideoAnalyticModule(profile, "ServiceDetectors").Idle();
                }
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