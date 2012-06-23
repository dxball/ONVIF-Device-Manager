using System;
using System.Reactive.Disposables;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;

namespace odm.ui.activities {
	public partial class LiveVideoView : BasePropertyControl, IDisposable, IPlaybackController {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new LiveVideoView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private CompositeDisposable disposables = new CompositeDisposable();
		private Model model;

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.model = model;
			InitializeComponent();
			VideoStartup(model.profToken);
		}

		IPlaybackSession playbackSession;
		void VideoStartup(string token) {
			var playerAct = activityContext.container.Resolve<IVideoPlayerActivity>();
	
			var playerModel = new VideoPlayerActivityModel(
				profileToken: model.profToken,
				showStreamUrl: true,
				streamSetup: new StreamSetup() {
					Stream = StreamType.RTPUnicast,
					Transport = new Transport() {
						Protocol = AppDefaults.visualSettings.Transport_Type,
						Tunnel = null
					}
				}
			);
			
			disposables.Add(
				activityContext.container.RunChildActivity(player, playerModel, (c, m) => playerAct.Run(c, m))
			);
		}

		public void Dispose() {
			Cancel();
		}

		public void Shutdown() {
			
		}

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}
	}
}
