using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls;
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;
using plugin_manager;
using utils;

namespace odm.ui.activities {
	public partial class VideoSettingsView : UserControl, IDisposable, IPlaybackController {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var viewFactory = container.Resolve<IVideoSettingsView>();
				var view = viewFactory.CreateView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public ICommand RevertCommand { get; private set; }

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }
		public PropertyVideoStreamingStrings Strings { get { return PropertyVideoStreamingStrings.instance; } }

		private CompositeDisposable disposables = new CompositeDisposable();

		IUnityContainer container;

		IPlaybackSession playbackSession;

		private void Init(Model model) {
			OnCompleted += disposables.Dispose;
			this.DataContext = model;
			container = activityContext.container;

			encResolutions = new List<EncoderResolutionPair>();
			var applyCmd = new DelegateCommand(
				() => {
					model.encoder = EncoderResolution.Encoder;
					model.resolution = EncoderResolution.Resolution;
					Success(new Result.Apply(model));
				},
				() => true
			);
			ApplyCommand = applyCmd;

			var revertCmd = new DelegateCommand(
				() => {
					if (model != null) {
						model.RevertChanges();
						EncoderResolution = encResolutions.Find(
							x => ((x.Encoder == model.encoder) && (x.Resolution.ToString() == model.resolution.ToString()))
						);
					}
				},
				() => true
			);
			RevertCommand = revertCmd;


			InitializeComponent();

			//numericTemp.ValueMin = 0;
			//numericTemp.IntValue = 100;
			//numericTemp.ValueMax = 110;

			numericTemp.Value = 100;


			BindData(model);
			VideoStartup(model);
		}
		
		void VideoStartup(Model model) {
			var playerAct = container.Resolve<IVideoPlayerActivity>();

			var playerModel = new VideoPlayerActivityModel(
				profileToken: model.profToken,
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
				container.RunChildActivity(player, playerModel, (c, m) => playerAct.Run(c, m))
			);
		}

		public class EncoderResolutionPair {
			public string Name {
				get {
					string name = "";
					name = Encoder.ToString() + ": " + Resolution.ToString();
					return name;
				}
			}
			public VideoEncoding Encoder { get; set; }
			public VideoResolution Resolution { get; set; }
			public Brush Foreground {
				get {
					Brush frgnd = Brushes.Black;
					switch (Encoder) {
						case VideoEncoding.H264:
							frgnd = new SolidColorBrush(Color.FromArgb(255, 0, 80, 0));
							break;
						case VideoEncoding.JPEG:
							frgnd = new SolidColorBrush(Color.FromArgb(255, 0, 0, 80));
							break;
						case VideoEncoding.MPEG4:
							frgnd = new SolidColorBrush(Color.FromArgb(255, 80, 0, 0));
							break;
						default:
							break;
					}
					return frgnd;
				}
			}
		}
		public List<EncoderResolutionPair> encResolutions { get; set; }
		void FillEncodersCollection(Model model) {
			model.encoders.ForEach(enc => {
				switch (enc) {
					case VideoEncoding.H264:
						model.encoderOptions.H264.ResolutionsAvailable.ForEach(res => {
							EncoderResolutionPair encres = new EncoderResolutionPair();
							encres.Encoder = enc;
							encres.Resolution = res;
							encResolutions.Add(encres);
						});
						break;
					case VideoEncoding.JPEG:
						model.encoderOptions.JPEG.ResolutionsAvailable.ForEach(res => {
							EncoderResolutionPair encres = new EncoderResolutionPair();
							encres.Encoder = enc;
							encres.Resolution = res;
							encResolutions.Add(encres);
						});

						break;
					case VideoEncoding.MPEG4:
						model.encoderOptions.MPEG4.ResolutionsAvailable.ForEach(res => {
							EncoderResolutionPair encres = new EncoderResolutionPair();
							encres.Encoder = enc;
							encres.Resolution = res;
							encResolutions.Add(encres);
						});

						break;
				}
			});
		}

		public EncoderResolutionPair EncoderResolution {
			get { return (EncoderResolutionPair)GetValue(EncoderResolutionProperty); }
			set { SetValue(EncoderResolutionProperty, value); }
		}
		public static readonly DependencyProperty EncoderResolutionProperty =
			DependencyProperty.Register("EncoderResolution", typeof(EncoderResolutionPair), typeof(VideoSettingsView));

		void BindData(Model model) {
			valueBitrate.CreateBinding(DoubleUpDown.ValueProperty, model, x => x.bitrate, (m, v) => {
				m.bitrate = v;
			});

			FillEncodersCollection(model);

			encoderResValue.ItemsSource = encResolutions;

			EncoderResolution = encResolutions.Find(x => { return x.Encoder == model.encoder && x.Resolution.ToString() == model.resolution.ToString(); });

			frameRateValue.CreateBinding(DoubleUpDown.IsEnabledProperty, model, x => { return !(x.minFrameRate == x.maxFrameRate); });
			frameRateCaption.CreateBinding(Label.IsEnabledProperty, model, x => { return !(x.minFrameRate == x.maxFrameRate); });

			valueGovLength.CreateBinding(IntegerUpDown.ValueProperty, model,
				x => {
					var ret = x.govLength == -1 ? 0 : x.govLength;
					return ret;
				},
				(m, v) => {
					m.govLength = v;
				});
			valueGovLength.CreateBinding(IntegerUpDown.MaximumProperty, model, x => x.maxGovLength);
			valueGovLength.CreateBinding(IntegerUpDown.MinimumProperty, model, x => x.minGovLength);
			valueGovLength.CreateBinding(IntegerUpDown.IsEnabledProperty, model, x => { return x.govLength != -1; });
			govLengthCaption.CreateBinding(Label.IsEnabledProperty, model, x => { return x.govLength != -1; });
		}

		public void Dispose() {
			Cancel();
		}
		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Shutdown() {

		}
	}
}
