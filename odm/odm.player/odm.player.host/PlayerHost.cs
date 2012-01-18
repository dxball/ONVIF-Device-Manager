using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using odm.hosting;
using utils;

namespace odm.player {

	public class HostedPlayer : IPlayer{
		PlayerTask playerTask = new PlayerTask();
		PlayerHost playerHost = null;

		public HostedPlayer() {
		}

		private class PlayerHost : MarshalByRefObject, IHostController, IPlaybackController, IDisposable {
			private object syn = new object();
			private IPlaybackController playbackController = null;
			private IPlaybackSession playbackSession;
			public PlayerTask playerTask = null;
			public PlayerHost(PlayerTask playerTask){
				playbackController = playerTask.playbackController;
				playerTask.playbackController = this;
				this.playerTask = playerTask;
			}
			public override Object InitializeLifetimeService() {
				//
				// Returning null designates an infinite non-expiring lease.
				// We must ensure that RemotingServices.Disconnect() is called 
				// when it's no longer needed otherwise there will be a memory leak.
				//
				return null;
			}

			bool IPlaybackController.Initialized(IPlaybackSession playbackSession) {
				IPlaybackController playbackController = null;
				lock (syn) {
					if (this.playbackController == null) {
						return false;
					}
					playbackController = this.playbackController;
					dbg.Assert(this.playbackSession == null);
				}
				var result = playbackController.Initialized(playbackSession);
				lock (syn) {
					if (this.playbackController == null) {
						return false;
					}
					dbg.Assert(this.playbackSession == null);
					if (result) {
						this.playbackSession = playbackSession;
					}
					return result;
				}
			}

			void IPlaybackController.Shutdown() {
				Dispose();
			}

			Action<IHostController> IHostController.Hello() {
				if (playbackController != null) {
					return playerTask.Start;
				} else {
					return (hostController) => { };
				}
			}

			void IHostController.Bye() {
				//Dispose();
			}

			bool IHostController.isAlive() {
				return playbackController!=null;
			}

			protected virtual void Dispose(bool disposing) {
				if (disposing) {
					IPlaybackController playbackController = null;
					IPlaybackSession playbackSession = null; 
					lock (syn) {
						playbackSession = this.playbackSession;
						playbackController = this.playbackController;
						this.playbackController = null;
						this.playbackSession = null;
					}
					if (playbackController != null) {
						playbackController.Shutdown();
					}
					if(playbackSession != null){
						try {
							playbackSession.Close();
						} catch (Exception err) {
							dbg.Error(err);
						}
					}
				}
				
			}
			public void Dispose(){
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			~PlayerHost(){
				Dispose(false);
			}

		}

		[Serializable]
		public class PlayerTask {
			public VideoBuffer videoBuffer;
			public IMetadataReceiver metadataReceiver;
			public MediaStreamInfo mediaStreamInfo;
			public IPlaybackController playbackController;
			public bool keepAlive = true;

			public void Start(IHostController hostController) {
				AppHosting.SetupChannel();
				var d = new SingleAssignmentDisposable();
				if (keepAlive) {
					d.Disposable = Observable.Interval(TimeSpan.FromMilliseconds(500))
						.Subscribe(i => {
							try {
								if (!hostController.isAlive()) {
									d.Dispose();
									Process.GetCurrentProcess().Kill();
								}
							} catch(Exception err) {
								dbg.Error(err);
								Process.GetCurrentProcess().Kill();
							}
						});
				}

				var live555 = new Live555(videoBuffer, metadataReceiver);
				live555.Play(mediaStreamInfo, playbackController);
				d.Dispose();
			}
		}

		public IDisposable Play(MediaStreamInfo mediaStreamInfo, IPlaybackController playbackController) {
			var disposable = new SingleAssignmentDisposable();
			playerTask.mediaStreamInfo = mediaStreamInfo;
			playerTask.playbackController = playbackController;
			if(playerHost!=null){
				playerHost.Dispose();
				RemotingServices.Disconnect(playerHost);
				playerHost = null;
			}
			
			playerHost = new PlayerHost(playerTask);
			RemotingServices.Marshal(playerHost);
			var ipcChannel = AppHosting.SetupChannel();
			var hostControllerUri = RemotingServices.GetObjectUri(playerHost);
			var hostControllerUrl = ipcChannel.GetUrlsForUri(hostControllerUri).First();
			
			//start player host process
			var hostProcessArgs = new CommandLineArgs();
			var t = Uri.EscapeDataString(hostControllerUrl);
			hostProcessArgs.Add("controller-url", new string[] { hostControllerUrl }.ToList());
			var hostProcess = Process.Start(new ProcessStartInfo() {
				FileName = Assembly.GetExecutingAssembly().Location,
				UseShellExecute = false,
				Arguments = String.Join(" ", hostProcessArgs.Format())
			});
			hostProcess.Exited += (s, o) => {
				//Console.WriteLine("host process exited!!!");
			};
			hostProcess.EnableRaisingEvents = true;
			return Disposable.Create(() => {
				if (playerHost != null) {
					playerHost.Dispose();
					RemotingServices.Disconnect(playerHost);
					playerHost = null;
				}
				//try {
				//    hostProcess.Kill();
				//} catch (Exception err) {
				//    dbg.Error(err);
				//}
			});
		}
	
		public void SetVideoBuffer(VideoBuffer videoBuffer){
			playerTask.videoBuffer = videoBuffer;
		}

		//public void SetUserNamePassword(string userName, string password){
		//    playerTask.userNameToken = new UserNameToken(userName, password);
		//}

		public void SetMetadataReciever(IMetadataReceiver metadataReceiver) {
			playerTask.metadataReceiver = metadataReceiver;
		}

		public void Dispose(){
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing) {
			if (playerHost != null) {
				RemotingServices.Disconnect(playerHost);
				playerHost.Dispose();
				playerHost = null;
			}
			if (disposing) {
				playerTask = null;
			}
		}
		~HostedPlayer() {
			Dispose(false);
		}
	}

}


