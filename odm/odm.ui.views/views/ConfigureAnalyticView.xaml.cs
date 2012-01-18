using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.core;
using odm.infra;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views.CommonAnalytics;
using odm.ui.views.CustomAnalytics;
using odm.ui.views.CustomAppro;
using utils;
using onvif.services;


namespace odm.ui.activities {
    /// <summary>
    /// Interaction logic for ConfigureAnalyticModuleView.xaml
    /// </summary>
    public partial class ConfigureAnalyticView : UserControl, IDisposable {
		
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ConfigureAnalyticView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private CompositeDisposable disposables = new CompositeDisposable();
        //XmlParserFactory xparser;

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
        public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }

        public class ModuleDescriptor {
            public ModuleDescriptor(global::onvif.services.Config config,
                global::onvif.services.ConfigDescription configDescription,
                global::System.Xml.Schema.XmlSchemaSet schema) {
                    this.schema = schema;
                    this.config = config;
                    this.configDescription = configDescription;

            }
            public global::onvif.services.Config config { get; set; }
            public global::onvif.services.ConfigDescription configDescription { get; set; }
            public global::System.Xml.Schema.XmlSchemaSet schema { get; set; }
        }

        public class AnalyticsVideoDescriptor {
            public IVideoInfo videoInfo { get; set; }
            public Size videoSourceResolution { get; set; }
			public string profileToken { get; set; }
        }
        void StartConfiguring(UIElement conf) {
            content.Children.Add(conf);
        }
        void BindModel(Model model){
            //"http://www.synesis.ru/onvif/magicbox_video_analytics"
            XmlQualifiedName xMagicboxAnalytics = new XmlQualifiedName("AnalyticsModule", "http://www.synesis.ru/onvif/VideoAnalytics");
            XmlQualifiedName xMagicboxAnnotation = new XmlQualifiedName("AnnotationModule", "http://www.synesis.ru/onvif/VideoAnalytics");
            XmlQualifiedName xMagicboxRegionRule = new XmlQualifiedName("RegionRule", "http://www.synesis.ru/onvif/VideoAnalytics");
            XmlQualifiedName xMagicboxWireRule = new XmlQualifiedName("TripWireRule", "http://www.synesis.ru/onvif/VideoAnalytics");

            XmlQualifiedName xApproAnalytics = new XmlQualifiedName("ApproMotionDetector", "http://www.incotex.ru/onvif/ApproMotionDetector");

            if (!AppDefaults.visualSettings.CustomAnalytics_IsEnabled) {
                CommonAnalytics customview = new CommonAnalytics();
                customview.Init(new ModuleDescriptor(model.config, model.configDescription, model.schemes));
                StartConfiguring(customview);

                return;
            }

            if (model.configDescription.Name == xMagicboxAnalytics) {
                //Custom 
                var isession = activityContext.container.Resolve<INvtSession>();
                AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
                ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

                videoDescr.videoInfo = activityContext.container.Resolve<IVideoInfo>();
				videoDescr.profileToken = model.profile.token;

                Size encoderResolution = new Size(model.profile.VideoEncoderConfiguration.Resolution.Width, model.profile.VideoEncoderConfiguration.Resolution.Height);
                Size videoResolution = Size.Empty;
                
                //prof.VideoSourceConfiguration.token;
                disposables.Add(isession.GetVideoSources().ObserveOnCurrentDispatcher().Subscribe(vidSources => {
                    vidSources.ForEach(x => {
                        if (x.token == model.profile.VideoSourceConfiguration.SourceToken) {
                            videoResolution = new Size(x.Resolution.Width, x.Resolution.Height);
                            //init stream setup for getstreamuri
                            var streamSetup = new StreamSetup();
                            streamSetup.Stream = StreamType.RTPUnicast;
                            streamSetup.Transport = new Transport();
                            streamSetup.Transport.Protocol = TransportProtocol.UDP;
                            streamSetup.Transport.Tunnel = null;

                            disposables.Add(isession.GetStreamUri(streamSetup, model.profile.token).ObserveOnCurrentDispatcher().Subscribe(uri => {
                                videoDescr.videoInfo.MediaUri = uri.Uri;
                                videoDescr.videoInfo.Resolution = encoderResolution;
                                videoDescr.videoSourceResolution = videoResolution;

                                SynesisAnalyticsConfigView customview = new SynesisAnalyticsConfigView();
                                //captionTitle.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, str => str.analyticsModuleCofig);
                                bool retres = customview.Init(activityContext.container, moduleDescriptor, videoDescr);
                                if (!retres) {
                                    Error(new Exception("Module is in unproper state. Please delete and create new one."));
                                }
                                Apply = customview.Apply;

                                StartConfiguring(customview);

                                controlDisposable = customview;
                            }, err => {
                                Error(err);
                            }));
                        }

                    });
                }, err => {
                    Error(err);
                }));
                
            } else if (model.configDescription.Name == xMagicboxAnnotation) {

                var isession = activityContext.container.Resolve<INvtSession>();
                AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
                ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

                videoDescr.videoInfo = activityContext.container.Resolve<IVideoInfo>();
				videoDescr.profileToken = model.profile.token;

                Size encoderResolution = new Size(model.profile.VideoEncoderConfiguration.Resolution.Width, model.profile.VideoEncoderConfiguration.Resolution.Height);
                Size videoResolution = Size.Empty;

                //prof.VideoSourceConfiguration.token;
                disposables.Add(isession.GetVideoSources().ObserveOnCurrentDispatcher().Subscribe(vidSources => {
                    vidSources.ForEach(x => {
                        if (x.token == model.profile.VideoSourceConfiguration.SourceToken) {
                            videoResolution = new Size(x.Resolution.Width, x.Resolution.Height);
                            //init stream setup for getstreamuri
                            var streamSetup = new StreamSetup();
                            streamSetup.Stream = StreamType.RTPUnicast;
                            streamSetup.Transport = new Transport();
                            streamSetup.Transport.Protocol = TransportProtocol.UDP;
                            streamSetup.Transport.Tunnel = null;

                            disposables.Add(isession.GetStreamUri(streamSetup, model.profile.token).ObserveOnCurrentDispatcher().Subscribe(uri => {
                                videoDescr.videoInfo.MediaUri = uri.Uri;
                                videoDescr.videoInfo.Resolution = encoderResolution;
                                videoDescr.videoSourceResolution = videoResolution;

                                SynesisAnnotationView customview = new SynesisAnnotationView();
                                //captionTitle.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, str => str.annotationModuleCofig);
                                bool retres = customview.Init(activityContext.container, moduleDescriptor, videoDescr);
                                if (!retres) {
                                    Error(new Exception("Module is in unproper state. Please delete and create new one."));
                                }
                                Apply = customview.Apply;

                                StartConfiguring(customview);

                                controlDisposable = customview;
                            }, err => {
                                Error(err);
                            }));
                        }

                    });
                }, err => {
                    Error(err);
                }));
            } else if (model.configDescription.Name == xMagicboxRegionRule) {

                var isession = activityContext.container.Resolve<INvtSession>();
                AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
                ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

                videoDescr.videoInfo = activityContext.container.Resolve<IVideoInfo>();
				videoDescr.profileToken = model.profile.token;

                Size encoderResolution = new Size(model.profile.VideoEncoderConfiguration.Resolution.Width, model.profile.VideoEncoderConfiguration.Resolution.Height);
                Size videoResolution = Size.Empty;

                //prof.VideoSourceConfiguration.token;
                disposables.Add(isession.GetVideoSources().ObserveOnCurrentDispatcher().Subscribe(vidSources => {
                    vidSources.ForEach(x => {
                        if (x.token == model.profile.VideoSourceConfiguration.SourceToken) {
                            videoResolution = new Size(x.Resolution.Width, x.Resolution.Height);
                            //init stream setup for getstreamuri
                            var streamSetup = new StreamSetup();
                            streamSetup.Stream = StreamType.RTPUnicast;
                            streamSetup.Transport = new Transport();
                            streamSetup.Transport.Protocol = TransportProtocol.UDP;
                            streamSetup.Transport.Tunnel = null;

                            disposables.Add(isession.GetStreamUri(streamSetup, model.profile.token).ObserveOnCurrentDispatcher().Subscribe(uri => {
                                videoDescr.videoInfo.MediaUri = uri.Uri;
                                videoDescr.videoInfo.Resolution = encoderResolution;
                                videoDescr.videoSourceResolution = videoResolution;

                                SynesisRegionRuleView customview = new SynesisRegionRuleView();
                                //captionTitle.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, str => str.regionRuleCofig);
                                bool retres = customview.Init(activityContext.container, moduleDescriptor, videoDescr);
                                if (!retres) {
                                    Error(new Exception("Module is in unproper state. Please delete and create new one."));
                                }
                                Apply = customview.Apply;

                                StartConfiguring(customview);

                                controlDisposable = customview;
                            }, err => {
                                Error(err);
                            }));
                        }

                    });
                }, err => {
                    Error(err);
                }));
            } else if (model.configDescription.Name == xMagicboxWireRule) {

                var isession = activityContext.container.Resolve<INvtSession>();
                AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
                ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

                videoDescr.videoInfo = activityContext.container.Resolve<IVideoInfo>();
				videoDescr.profileToken = model.profile.token;

                Size encoderResolution = new Size(model.profile.VideoEncoderConfiguration.Resolution.Width, model.profile.VideoEncoderConfiguration.Resolution.Height);
                Size videoResolution = Size.Empty;

                //prof.VideoSourceConfiguration.token;
                disposables.Add(isession.GetVideoSources().ObserveOnCurrentDispatcher().Subscribe(vidSources => {
                    vidSources.ForEach(x => {
                        if (x.token == model.profile.VideoSourceConfiguration.SourceToken) {
                            videoResolution = new Size(x.Resolution.Width, x.Resolution.Height);
                            //init stream setup for getstreamuri
                            var streamSetup = new StreamSetup();
                            streamSetup.Stream = StreamType.RTPUnicast;
                            streamSetup.Transport = new Transport();
                            streamSetup.Transport.Protocol = TransportProtocol.UDP;
                            streamSetup.Transport.Tunnel = null;

                            disposables.Add(isession.GetStreamUri(streamSetup, model.profile.token).ObserveOnCurrentDispatcher().Subscribe(uri => {
                                videoDescr.videoInfo.MediaUri = uri.Uri;
                                videoDescr.videoInfo.Resolution = encoderResolution;
                                videoDescr.videoSourceResolution = videoResolution;

                                SynesisTripWireRuleView customview = new SynesisTripWireRuleView();
                                //captionTitle.CreateBinding(ContentColumn.TitleProperty, LinkButtonsStrings.instance, str => str.wireRuleCofig);
                                bool retres = customview.Init(activityContext.container, moduleDescriptor, videoDescr);
                                if (!retres) {
                                    Error(new Exception("Module is in unproper state. Please delete and create new one."));
                                }
                                Apply = customview.Apply;

                                StartConfiguring(customview);

                                controlDisposable = customview;
                            }, err => {
                                Error(err);
                            }));
                        }

                    });
                }, err => {
                    Error(err);
                }));
            } else if (model.configDescription.Name == xApproAnalytics) {

                var isession = activityContext.container.Resolve<INvtSession>();
                AnalyticsVideoDescriptor videoDescr = new AnalyticsVideoDescriptor();
                ModuleDescriptor moduleDescriptor = new ModuleDescriptor(model.config, model.configDescription, model.schemes);

                videoDescr.videoInfo = activityContext.container.Resolve<IVideoInfo>();
				videoDescr.profileToken = model.profile.token;

                Size encoderResolution = new Size(model.profile.VideoEncoderConfiguration.Resolution.Width, model.profile.VideoEncoderConfiguration.Resolution.Height);
                Size videoResolution = Size.Empty;

                //prof.VideoSourceConfiguration.token;
                disposables.Add(isession.GetVideoSources().ObserveOnCurrentDispatcher().Subscribe(vidSources => {
                    vidSources.ForEach(x => {
                        if (x.token == model.profile.VideoSourceConfiguration.SourceToken) {
                            videoResolution = new Size(x.Resolution.Width, x.Resolution.Height);
                            //init stream setup for getstreamuri
                            var streamSetup = new StreamSetup();
                            streamSetup.Stream = StreamType.RTPUnicast;
                            streamSetup.Transport = new Transport();
                            streamSetup.Transport.Protocol = TransportProtocol.UDP;
                            streamSetup.Transport.Tunnel = null;

                            disposables.Add(isession.GetStreamUri(streamSetup, model.profile.token).ObserveOnCurrentDispatcher().Subscribe(uri => {
                                videoDescr.videoInfo.MediaUri = uri.Uri;
                                videoDescr.videoInfo.Resolution = encoderResolution;
                                videoDescr.videoSourceResolution = videoResolution;

                                ApproAnalyticsConfigView customview = new ApproAnalyticsConfigView();
                                //captionTitle.CreateBinding(ContentColumn.TitleProperty, ApproAnalyticsStrings.instance, str => str.title);
                                bool retres = customview.Init(activityContext.container, moduleDescriptor, videoDescr);
                                if (!retres) {
                                    Error(new Exception("Module is in unproper state. Please delete and create new one."));
                                }
                                Apply = customview.Apply;

                                StartConfiguring(customview);

                                controlDisposable = customview;
                            }, err => {
                                Error(err);
                            }));
                        }

                    });
                }, err => {
                    Error(err);
                }));
            } else {
                //Common
                CommonAnalytics customview = new CommonAnalytics();
                customview.Init(new ModuleDescriptor(model.config, model.configDescription, model.schemes));
                StartConfiguring(customview);
            }
        }

        public Action Apply;
        IDisposable controlDisposable;
		//TODO: Stub fix for #225 Remove this with plugin functionality
		LastEditedModule last;
		//

        void ReleaseAll() {
            if (controlDisposable != null)
                controlDisposable.Dispose();
        }

		private void Init(Model model) {
			
			OnCompleted += () => {
				disposables.Dispose();
				ReleaseAll();
			};

            this.DataContext = model;

            InitializeComponent();
            BindModel(model);

            var abortCommand = new DelegateCommand(
                () => {
					//TODO: Stub fix for #225 Remove this with plugin functionality
                    last = activityContext.container.Resolve<LastEditedModule>();
                    last.module = null;
					//
                    Success(new Result.Abort()); 
				},
                () => true
            );
            AbortCommand = abortCommand;

            var applyCommand = new DelegateCommand(
                () => {
                    if (Apply != null)
                        Apply();
                    Success(new Result.Apply(model));
                },
                () => true
            );
            ApplyCommand = applyCommand;

            btnApply.Command = ApplyCommand;
            btnAbort.Command = AbortCommand;
		}

		public void Dispose() {
			Cancel();
		}
    }
}
