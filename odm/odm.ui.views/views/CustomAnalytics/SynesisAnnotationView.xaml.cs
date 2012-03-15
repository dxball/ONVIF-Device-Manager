using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;
using utils;
using System.ComponentModel;
using onvif.services;
using odm.ui.core;
using odm.ui.controls;
using odm.player;
using System.Reactive.Disposables;
using odm.ui.activities;

namespace odm.ui.views.CustomAnalytics {
    /// <summary>
    /// Interaction logic for SynesisAnnotationView.xaml
    /// </summary>
    public partial class SynesisAnnotationView : UserControl, IDisposable, IPlaybackController {
        public SynesisAnnotationView() {
            InitializeComponent();
			disposables = new CompositeDisposable();
        }
        public void Apply() {
            try {
                GetData(modulDescr.config.Parameters.SimpleItem, model);
            } catch (Exception err) {
                dbg.Error(err);
            }
        }

        public LocalDisplayAnnotation Strings { get { return LocalDisplayAnnotation.instance; } }

        public class SynesisAnalyticsModel : INotifyPropertyChanged {
            bool enableMovingRects;
            public bool EnableMovingRects {
                get {
                    return enableMovingRects;
                }
                set {
                    enableMovingRects = value;
                    NotifyPropertyChanged("EnableMovingRects");
                }
            }
            bool enableSpeed;
            public bool EnableSpeed {
                get {
                    return enableSpeed;
                }
                set {
                    enableSpeed = value;
                    NotifyPropertyChanged("EnableSpeed");
                }
            }
            bool enableTimestamp;
            public bool EnableTimestamp {
                get {
                    return enableTimestamp;
                }
                set {
                    enableTimestamp = value;
                    NotifyPropertyChanged("EnableTimestamp");
                }
            }
            bool enableSystemInformation;
            public bool EnableSystemInformation {
                get {
                    return enableSystemInformation;
                }
                set {
                    enableSystemInformation = value;
                    NotifyPropertyChanged("EnableSystemInformation");
                }
            }
            bool enableChannelName;
            public bool EnableChannelName {
                get {
                    return enableChannelName;
                }
                set {
                    enableChannelName = value;
                    NotifyPropertyChanged("EnableChannelName");
                }
            }
            bool enableTracking;
            public bool EnableTracking {
                get {
                    return enableTracking;
                }
                set {
                    enableTracking = value;
                    NotifyPropertyChanged("EnableTracking");
                }
            }
            bool enableUserRegion;
            public bool EnableUserRegion {
                get {
                    return enableUserRegion;
                }
                set {
                    enableUserRegion = value;
                    NotifyPropertyChanged("EnableUserRegion");
                }
            }
            bool enableRules;
            public bool EnableRules {
                get {
                    return enableRules;
                }
                set {
                    enableRules = value;
                    NotifyPropertyChanged("EnableRules");
                }
            }
            bool enableCalibrationResults;
            public bool EnableCalibrationResults {
                get {
                    return enableCalibrationResults;
                }
                set {
                    enableCalibrationResults = value;
                    NotifyPropertyChanged("EnableCalibrationResults");
                }
            }
			private void NotifyPropertyChanged(String info) {
				var prop_changed = this.PropertyChanged;
				if (prop_changed != null) {
					prop_changed(this, new PropertyChangedEventArgs(info));
				}
			}
            public event PropertyChangedEventHandler PropertyChanged;
        }
        void GetData(ItemListSimpleItem[] simpleItems, SynesisAnalyticsModel model) {
            simpleItems.ForEach(simple => {
                switch (simple.Name) {
                    case "EnableMovingRects":
                        simple.Value = DataConverter.BoolToString(model.EnableMovingRects);
                        break;
                    case "EnableSpeed":
                        simple.Value = DataConverter.BoolToString(model.EnableSpeed);
                        break;
                    case "EnableTimestamp":
                        simple.Value = DataConverter.BoolToString(model.EnableTimestamp);
                        break;
                    case "EnableSystemInformation":
                        simple.Value = DataConverter.BoolToString(model.EnableSystemInformation);
                        break;
                    case "EnableChannelName":
                        simple.Value = DataConverter.BoolToString(model.EnableChannelName);
                        break;
                    case "EnableTracking":
                        simple.Value = DataConverter.BoolToString(model.EnableTracking);
                        break;
                    case "EnableUserRegion":
                        simple.Value = DataConverter.BoolToString(model.EnableUserRegion);
                        break;
                    case "EnableRules":
                        simple.Value = DataConverter.BoolToString(model.EnableRules);
                        break;
                    case "EnableCalibrationResults":
                        simple.Value = DataConverter.BoolToString(model.EnableCalibrationResults);
                        break;
                }
            });
        }
        void FillData(ItemListSimpleItem[] simpleItems, SynesisAnalyticsModel model) {
            simpleItems.ForEach(simple => {
                switch (simple.Name) {
                    case "EnableMovingRects":
                        model.EnableMovingRects = DataConverter.StringToBool(simple.Value);
                        break;
                    case "EnableSpeed":
                        model.EnableSpeed = DataConverter.StringToBool(simple.Value);
                        break;
                    case "EnableTimestamp":
                        model.EnableTimestamp = DataConverter.StringToBool(simple.Value);
                        break;
                    case "EnableSystemInformation":
                        model.EnableSystemInformation = DataConverter.StringToBool(simple.Value);
                        break;
                    case "EnableChannelName":
                        model.EnableChannelName = DataConverter.StringToBool(simple.Value);
                        break;
                    case "EnableTracking":
                        model.EnableTracking = DataConverter.StringToBool(simple.Value);
                        break;
                    case "EnableUserRegion":
                        model.EnableUserRegion = DataConverter.StringToBool(simple.Value);
                        break;
                    case "EnableRules":
                        model.EnableRules = DataConverter.StringToBool(simple.Value);
                        break;
                    case "EnableCalibrationResults":
                        model.EnableCalibrationResults = DataConverter.StringToBool(simple.Value);
                        break;
                }
            });
        }
        void BindData() {
            EnableCalibrationResults.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableCalibrationResults, (m, v) => { m.EnableCalibrationResults = v; });
            EnableChannelName.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableChannelName, (m, v) => { m.EnableChannelName = v; });
            EnableMovingRects.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableMovingRects, (m, v) => { m.EnableMovingRects = v; });
            EnableRules.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableRules, (m, v) => { m.EnableRules = v; });
            EnableSpeed.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableSpeed, (m, v) => { m.EnableSpeed = v; });
            EnableSystemInformation.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableSystemInformation, (m, v) => { m.EnableSystemInformation = v; });
            EnableTimestamp.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableTimestamp, (m, v) => { m.EnableTimestamp = v; });
            EnableTracking.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableTracking, (m, v) => { m.EnableTracking = v; });
            EnableUserRegion.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.EnableUserRegion, (m, v) => { m.EnableUserRegion = v; });
        }

        public string title = "Annotation module configuration";
        SynesisAnalyticsModel model;
        odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr;
        odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr;
        IUnityContainer container;
        IVideoInfo videoInfo;
        
        public bool Init(IUnityContainer container, odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr, odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr) {
            this.modulDescr = modulDescr;
            this.videoDescr = videoDescr;
            this.container = container;
            this.videoInfo = videoDescr.videoInfo;

            model = new SynesisAnalyticsModel();
            
            FillData(modulDescr.config.Parameters.SimpleItem, model);
            BindData();
			VideoStartup(videoInfo);
            return true;
        }

		IPlaybackSession playbackSession;
		VideoBuffer vidBuff;
		CompositeDisposable disposables;
		void VideoStartup(IVideoInfo iVideo) {
			vidBuff = new VideoBuffer((int)iVideo.Resolution.Width, (int)iVideo.Resolution.Height);

			var playerAct = container.Resolve<IVideoPlayerActivity>();

			var model = new VideoPlayerActivityModel(
				profileToken: videoDescr.profileToken,
				showStreamUrl: false,
				streamSetup: new StreamSetup() {
					Stream = StreamType.RTPUnicast,
					Transport = new Transport() {
						Protocol = AppDefaults.visualSettings.Transport_Type,
						Tunnel = null
					}
				}
			);

			disposables.Add(
				container.RunChildActivity(player, model, (c, m) => playerAct.Run(c, m))
			);
		}

		public void Dispose() {
			if (vidBuff != null) {
				vidBuff.Dispose();
			}
			disposables.Dispose();
			//TODO: release player host
		}

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Shutdown() {
		}
	}
}
