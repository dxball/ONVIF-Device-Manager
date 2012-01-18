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
using utils;
using Microsoft.Practices.Unity;
using odm.ui.core;
using odm.ui.views.CustomAnalytics;
using odm.ui.controls;
using odm.player;
using odm.ui.activities;
using onvif.services;
using System.Reactive.Disposables;

namespace odm.ui.views.CustomAppro {
    /// <summary>
    /// Interaction logic for ApproAnalyticsConfigView.xaml
    /// </summary>
	public partial class ApproAnalyticsConfigView : UserControl, IDisposable, IPlaybackController {
        public ApproAnalyticsConfigView() {
            InitializeComponent();
			disposables = new CompositeDisposable();
            Localization();
        }
		CompositeDisposable disposables;
        public LocalApproAnalytics Strings { get { return LocalApproAnalytics.instance; } }
        public string title = "Appro motion detector module config";
        odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr;
        odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr;
        
        IUnityContainer container;
        IVideoInfo videoInfo;

        public void Apply() {
            try {
                GetData();
            } catch (Exception err) {
                dbg.Error(err);
            }
        }
        void Localization() {
            sensitivityCaption.CreateBinding(Label.ContentProperty, Strings, x => x.sensitivity);
            isEnabled.CreateBinding(CheckBox.ContentProperty, Strings, x => x.isEnabled);
        }
        public bool Init(IUnityContainer container, odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr, odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr) {
            this.modulDescr = modulDescr;
            this.videoDescr = videoDescr;
            this.container = container;
            this.videoInfo = videoDescr.videoInfo;


            try {
                modulDescr.schema.GlobalTypes.Values.ForEach(x => {
                    var val = x as global::System.Xml.Schema.XmlSchemaSimpleType;
                    if (val == null)
                        return;
                    if (val.QualifiedName.Name == "sensitivity") {
                        var tm1 = val.Content as System.Xml.Schema.XmlSchemaSimpleTypeRestriction;
                        if (tm1 == null)
                            return;
                        var tm2 = tm1.Facets as System.Collections.CollectionBase;
                        if (tm2 == null)
                            return;

                        tm2.ForEach(xx => {
                            var min = xx as System.Xml.Schema.XmlSchemaMinInclusiveFacet;
                            if (min != null) {
                                int mval = 0;
                                Int32.TryParse(min.Value, out mval);
                                valueSensitivity.Minimum = mval;
                            }
                            var max = xx as System.Xml.Schema.XmlSchemaMaxInclusiveFacet;
                            if (max != null) {
                                int mval = 0;
                                Int32.TryParse(max.Value, out mval);
                                valueSensitivity.Maximum = mval;
                            }
                        });
                    }
                });
            } catch (Exception err) {
                dbg.Error(err);
            }
            try {
				VideoStartup(videoInfo);
                InitApproEditor();
            } catch (Exception err) {
                dbg.Error(err);
                return false;
            }

            return true;
        }
        void GetData() {
            modulDescr.config.Parameters.SimpleItem.ForEach(x => {
                switch (x.Name) {
                    case "sensitivity":
                        x.Value = DataConverter.IntToString((int)valueSensitivity.Value);
                        break;
                    case "region_mask":
                        x.Value = DataConverter.IntToString(approEditor.MaskedValue);
                        break;
                    case "enable":
                        x.Value = DataConverter.BoolToString(isEnabled.IsChecked.Value);
                        break;
                }
            });

        }
        void InitApproEditor() { 
            approEditor.Width = videoDescr.videoInfo.Resolution.Width;
            approEditor.Height = videoDescr.videoInfo.Resolution.Height;
            
            modulDescr.config.Parameters.SimpleItem.ForEach(x => {
                switch (x.Name) {
                    case "sensitivity":
                        valueSensitivity.Value = DataConverter.StringToInt(x.Value);
                        break;
                    case "region_mask":
                        approEditor.MaskedValue = DataConverter.StringToInt(x.Value);
                        break;
                    case "enable":
                        isEnabled.IsChecked = DataConverter.StringToBool(x.Value);
                        break;
                }
            });
        }
		IPlaybackSession playbackSession;
		VideoBuffer vidBuff;
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
			if (vidBuff != null)
				vidBuff.Dispose();
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
