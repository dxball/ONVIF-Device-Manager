using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.player;
using odm.ui.core;
using onvif.services;
using utils;
using Unit = Microsoft.FSharp.Core.Unit;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for VideoPlayer.xaml
	/// </summary>
	public partial class VideoPlayerView : UserControl, IDisposable, IPlaybackController, INotifyPropertyChanged {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new VideoPlayerView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private CompositeDisposable disposables = new CompositeDisposable();
		private SerialDisposable renderSubscription = new SerialDisposable();
		bool _isPaused = false;
		public bool isPaused {
			get {
				return _isPaused;
			}
			set {
				_isPaused = value;
				NotifyPropertyChanged("isPaused");
			}
		}
		IPlaybackSession playbackSession;

		private void Init(Model model) {
			OnCompleted += () => {
				try {
					disposables.Dispose();
				} catch (Exception err) {
					dbg.Error(err);
				}
			};

			VerifyAccess();
			disposables.Add(renderSubscription);
			isPaused = false;
			InitializeComponent();

			btnPause.Click += new RoutedEventHandler(btnPause_Click);
			btnResume.Click += new RoutedEventHandler(btnPause_Click);

			btnPause.CreateBinding(Button.VisibilityProperty, this, x => { return x.isPaused ? Visibility.Collapsed : Visibility.Visible; });
			btnResume.CreateBinding(Button.VisibilityProperty, this, x => { return !x.isPaused ? Visibility.Collapsed : Visibility.Visible; });

			if (AppDefaults.visualSettings.Enable_UI_Fps_Caption) {
				fpsCaption.Visibility = System.Windows.Visibility.Visible;
			} else {
				fpsCaption.Visibility = System.Windows.Visibility.Hidden;
			}

			if (model.isUriEnabled) {
				uriString.Visibility = System.Windows.Visibility.Visible;
				uriString.Text = model.mediaUri.Uri;
			} else {
				uriString.Visibility = System.Windows.Visibility.Collapsed;
			}
			VideoStartup(model);
		}

		void btnPause_Click(object sender, RoutedEventArgs e) {
			if (isPaused)
				Resume();
			else
				Pause();
		}
		IPlayer player;
		VideoBuffer videoBuff;
		void VideoStartup(Model model) {
			player = new HostedPlayer();
			videoBuff = new VideoBuffer(model.encoderResolution.Width, model.encoderResolution.Height);
			player.SetVideoBuffer(videoBuff);
			
			var account = AccountManager.CurrentAccount;
			UserNameToken utoken = null;
			if(account != null && account.Account != null){
				utoken = new UserNameToken(account.Name, account.Password);
			}

			MediaStreamInfo.Transport transp;
			switch(model.streamSetup.Transport.Protocol){
				case TransportProtocol.HTTP:
					transp = MediaStreamInfo.Transport.Http;
					break;
				case TransportProtocol.RTSP:
					transp = MediaStreamInfo.Transport.Tcp;
					break;
				default:
					transp = MediaStreamInfo.Transport.Udp;
					break;
			}
			MediaStreamInfo mstrInfo = new MediaStreamInfo(model.mediaUri.Uri, transp, utoken);
			disposables.Add(player.Play(mstrInfo, this));
			InitPlayback(videoBuff);
		}
		public void Dispose() {
			try {
				Cancel();
			} catch (Exception err) {
				dbg.Error(err);
			}
		}

		public void Shutdown() {

		}

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Pause() {
			VerifyAccess();
			isPaused = true;
		}
		public void Resume() {
			VerifyAccess();
			isPaused = false;
		}

		/// <summary>
		/// Start Playback
		/// </summary>
		/// <param name="res"></param>
		public void InitPlayback(VideoBuffer videoBuffer) {
			if (videoBuffer == null) {
				throw new ArgumentNullException("videoBufferDescription");
			}
			VerifyAccess();
			
			TimeSpan renderinterval;
			try {
				int fps = AppDefaults.visualSettings.ui_video_rendering_fps;
				fps = (fps <= 0 || fps > 100) ? 100 : fps;
				renderinterval = TimeSpan.FromMilliseconds(1000 / fps);
			} catch {
				renderinterval = TimeSpan.FromMilliseconds(1000 / 30);
			}

			var cancellationTokenSource = new CancellationTokenSource();
			renderSubscription.Disposable = Disposable.Create(() => {
				cancellationTokenSource.Cancel();
			});
			var bitmap = PrepareForRendering(videoBuffer);
			var cancellationToken = cancellationTokenSource.Token;
			var dispatcher = Application.Current.Dispatcher;
			var renderingTask = Task.Factory.StartNew(() => {
				var statistics = new CircularBuffer<long>(100);
				using (videoBuffer.Lock()) {
					try {
						//start rendering loop
						while (!cancellationToken.IsCancellationRequested) {
							using (var processingEvent = new ManualResetEventSlim(false)) {
								dispatcher.BeginInvoke(() => {
									using (Disposable.Create(() => processingEvent.Set())) {
										if (!cancellationToken.IsCancellationRequested) {
											//update statisitc info
											statistics.Enqueue(Stopwatch.GetTimestamp());
											//evaluate averange rendering fps
											var ticksEllapsed = statistics.last - statistics.first;
											double avgFps = 0;
											if (ticksEllapsed > 0) {
												avgFps = ((double)statistics.length * (double)Stopwatch.Frequency) / (double)ticksEllapsed;
											}
											//render farme to screen
											DrawFrame(bitmap, videoBuffer, avgFps);
										}
									}
								});
								processingEvent.Wait(cancellationToken);
							}
							cancellationToken.WaitHandle.WaitOne(renderinterval);
						}
					} catch (OperationCanceledException) {
						//swallow exception
					} catch (Exception error) {
						dbg.Error(error);
					}
				}
			}, cancellationToken);
			
		}

		private WriteableBitmap PrepareForRendering(VideoBuffer videoBuffer) {
			PixelFormat pixelFormat;
			if (videoBuffer.pixelFormat == PixFrmt.rgb24) {
				pixelFormat = PixelFormats.Rgb24;
			} else if (videoBuffer.pixelFormat == PixFrmt.bgra32) {
				pixelFormat = PixelFormats.Bgra32;
			} else if (videoBuffer.pixelFormat == PixFrmt.bgr24) {
				pixelFormat = PixelFormats.Bgr24;
			} else {
				throw new Exception("unsupported pixel format");
			}
			var bitmap = new WriteableBitmap(
				videoBuffer.width, videoBuffer.height,
				96, 96,
				pixelFormat, null
			);
			_imgVIew.Source = bitmap;
			return bitmap;
		}

		private void DrawFrame(WriteableBitmap bitmap, VideoBuffer videoBuffer, double averangeFps) {
			VerifyAccess();
			if (isPaused) {
				return;
			}

			bitmap.Lock();
			try {
				using (var ptr = videoBuffer.Lock()) {
					bitmap.WritePixels(
						new Int32Rect(0, 0, videoBuffer.width, videoBuffer.height),
						ptr.value, videoBuffer.size, videoBuffer.stride,
						0, 0
					);
				}
			} finally {
				bitmap.Unlock();
			}
			fpsCaption.Text = averangeFps.ToString("F1");
		}
		private void BaseVideoPlayer_Loaded(object sender, RoutedEventArgs e) {

		}
		private void NotifyPropertyChanged(String info) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class VideoPlayerActivityModel {
		public VideoPlayerActivityModel(string profileToken, StreamSetup streamSetup, bool showStreamUrl) {
			this.profileToken = profileToken;
			this.streamSetup = streamSetup;
			this.showStreamUrl = showStreamUrl;
		}
		public string profileToken { get; private set; }
		public StreamSetup streamSetup { get; private set; }
		public bool showStreamUrl { get; private set; }
	}
	public interface IVideoPlayerActivity {
		FSharpAsync<Unit> Run(IUnityContainer ctx, VideoPlayerActivityModel model);
	}
}
