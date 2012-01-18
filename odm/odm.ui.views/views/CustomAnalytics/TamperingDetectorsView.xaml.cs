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
using odm.ui.core;
using odm.ui.controls;
using utils;
using odm.player;
using System.Reactive.Disposables;
using odm.ui.activities;
using onvif.services;

namespace odm.ui.views.CustomAnalytics {
    /// <summary>
    /// Interaction logic for TamperingDetectorsView.xaml
    /// </summary>
    public partial class TamperingDetectorsView : UserControl, IDisposable, IPlaybackController {
        public TamperingDetectorsView() {
            InitializeComponent();
			disposables = new CompositeDisposable();
        }
        odm.ui.views.CustomAnalytics.SynesisAnalyticsConfigView.SynesisAnalyticsModel model;
        IUnityContainer container;
        IVideoInfo videoInfo;
        public PropertyTamperingDetectorsStrings Strings { get { return PropertyTamperingDetectorsStrings.instance; } }

        public void Init(IUnityContainer container, odm.ui.views.CustomAnalytics.SynesisAnalyticsConfigView.SynesisAnalyticsModel model, IVideoInfo videoInfo, string profToken) {
            this.model = model;
            this.container = container;
            this.videoInfo = videoInfo;

            BindData();
			VideoStartup(videoInfo, profToken);
        }
        void BindData() {
            cameraRedirected.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.CameraRedirected, (m, v) => {
                m.CameraRedirected = v;
            });
            imageTooDark.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.ImageTooDark, (m, v) => {
                m.ImageTooDark = v;
            });
            cameraObstructed.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.CameraObstructed, (m, v) => {
                m.CameraObstructed = v;
            });
            imageTooBlurry.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.ImageTooBlurry, (m, v) => {
                m.ImageTooBlurry = v;
            });
            imageTooBright.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.ImageTooBright, (m, v) => {
                m.ImageTooBright = v;
            });
            imageTooNoisy.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.ImageTooNoisy, (m, v) => {
                m.ImageTooNoisy = v;
            });
        }
        public void Apply() {
            GetData();
        }

        void GetData() {
            
        }

		IPlaybackSession playbackSession;
		VideoBuffer vidBuff;
		CompositeDisposable disposables;
		void VideoStartup(IVideoInfo iVideo, string profToken) {
			vidBuff = new VideoBuffer((int)iVideo.Resolution.Width, (int)iVideo.Resolution.Height);

			var playerAct = container.Resolve<IVideoPlayerActivity>();

			var model = new VideoPlayerActivityModel(
				profileToken: profToken,
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

        public bool isCameraObstructedEnabled {
            get { return (bool)GetValue(isCameraObstructedEnabledProperty); }
            set { SetValue(isCameraObstructedEnabledProperty, value); }
        }
        public static readonly DependencyProperty isCameraObstructedEnabledProperty =
            DependencyProperty.Register("isCameraObstructedEnabled", typeof(bool), typeof(TamperingDetectorsView));

        public bool isCameraRedirectedEnabled {
            get { return (bool)GetValue(isCameraRedirectedEnabledProperty); }
            set { SetValue(isCameraRedirectedEnabledProperty, value); }
        }
        public static readonly DependencyProperty isCameraRedirectedEnabledProperty =
            DependencyProperty.Register("isCameraRedirectedEnabled", typeof(bool), typeof(TamperingDetectorsView));



		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Shutdown() {
		}
	}
}
