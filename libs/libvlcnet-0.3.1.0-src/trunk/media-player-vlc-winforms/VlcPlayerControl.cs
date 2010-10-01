// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Common.Logging;
using DZ.MediaPlayer.Io;
using DZ.MediaPlayer.Vlc.Deployment;

namespace DZ.MediaPlayer.Vlc.WindowsForms
{
    /// <summary>
    /// User control provides straightforward access to libvlcnet features.
    /// </summary>
    public partial class VlcPlayerControl : UserControl
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(VlcPlayerControl));

        private void lazyInitialize() {
            if (!isInitialized) {
                Initialize();
            }
        }

        private VlcPlayerControlState state = VlcPlayerControlState.IDLE;

        /// <summary>
        /// State of control.
        /// </summary>
        public VlcPlayerControlState State {
            get {
                return (state);
            }
        }

        /// <summary>
        /// Signals about player changed its state.
        /// </summary>
        public event EventHandler StateChanged;

        #region Nested type : VlcPlayerEventsReceiver

        /// <summary>
        /// Subscriber to the several VLC events.
        /// </summary>
        private sealed class VlcPlayerEventsReceiver : PlayerEventsReceiver
        {
            private static readonly ILog logger = LogManager.GetLogger(typeof(VlcPlayerEventsReceiver));

            #region EndReached

            public override void OnEndReached() {
                base.OnEndReached();
                //
                EventHandler handler = EndReached;
                if (handler != null) {
                    ThreadPool.QueueUserWorkItem(endReachedWorkItem, handler);
                }
            }

            private static void endReachedWorkItem(object state) {
                try {
                    EventHandler handler = (EventHandler) state;
                    handler.Invoke(null, EventArgs.Empty);
                } catch (Exception exc) {
                    if (logger.IsErrorEnabled) {
                        logger.Error(String.Format("Unhandled exception in end reached dispatcher code : {0}", exc));
                    }
                }
            }

            #endregion

            public override void OnPositionChanged() {
                base.OnPositionChanged();
                //
                EventHandler handler = PositionChanged;
                if (handler != null) {
                    ThreadPool.QueueUserWorkItem(positionChangedWorkItem, handler);
                }
            }

            private static void positionChangedWorkItem(object state) {
                try {
                    EventHandler handler = (EventHandler) state;
                    handler.Invoke(null, EventArgs.Empty);
                } catch (Exception exc) {
                    if (logger.IsErrorEnabled) {
                        logger.Error(String.Format("Unhandled exception in position changed dispatcher code : {0}", exc));
                    }
                }
            }

            public event EventHandler EndReached;

            public event EventHandler PositionChanged;
        }

        #endregion

        private MediaLibraryFactory mediaLibraryFactory;
        public Player player;

        private bool isInitialized;

        /// <summary>
        /// Is VCL subsystem initialized.
        /// </summary>
        public bool IsInitialized {
            get {
                return (isInitialized);
            }
        }

        private static string getStartupPath() {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VlcPlayerControl() {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization of vlclib resources being used by control.
        /// <see cref="InvalidOperationException"/> will be thrown on re-initialization.
        /// <see cref="MediaPlayerException"/> may be thrown during initialization VLC subsystem.
        /// </summary>
        public void Initialize() {
            if (isInitialized) {
                throw new InvalidOperationException("This object does not support multi time initialization.");
            }
            //
            isInitialized = true;
            // Install VLC packages if necessary
            VlcDeployment deployment = VlcDeployment.Default;
            if (!deployment.CheckVlcLibraryExistence(true, false)) {
                if (logger.IsInfoEnabled) {
                    logger.Info("Unable to find installed vlc library. Starting installing from zip archive.");
                }
                deployment.Install(false, true, false, false);
                if (logger.IsInfoEnabled) {
                    logger.Info("Installed.");
                }
            }
            // VLC objects initialization
            mediaLibraryFactory = new VlcMediaLibraryFactory(new string[] {
                "--no-snapshot-preview",
                "--ignore-config",
                "--no-osd",
                "--plugin-path", Path.Combine(getStartupPath(), "plugins")
            });
            player = mediaLibraryFactory.CreatePlayer(new PlayerOutput(vlcWindowControl.Window));
            // Subscribe to events
            VlcPlayerEventsReceiver receiver = new VlcPlayerEventsReceiver();
            receiver.EndReached += endReachedEventHandler;
            receiver.PositionChanged += positionChangedEventHandler;
            player.EventsReceivers.Add(receiver);
        }

        private bool isEventDispatchingEnabled;

        private void endReachedEventHandler(object sender, EventArgs e) {
            if (logger.IsTraceEnabled) {
                logger.Trace("End reached event received.");
            }
            if (InvokeRequired) {
                Invoke(new ThreadStart(endReachedEventHandlerCore));
            } else {
                endReachedEventHandlerCore();
            }
        }

        private void endReachedEventHandlerCore() {
            if (isEventDispatchingEnabled) {
                if (logger.IsTraceEnabled) {
                    logger.Trace("END_REACHED event dispatching is allowed.");
                }
                //
                EventHandler handler = EndReached;
                if (handler != null) {
                    handler.Invoke(this, EventArgs.Empty);
                }
                //
                if (logger.IsTraceEnabled) {
                    logger.Trace("END_REACHED dispatched OK.");
                }
            }
        }

        private void positionChangedEventHandler(object sender, EventArgs e) {
            if (logger.IsTraceEnabled) {
                logger.Trace("Position changed event received.");
            }
            if (InvokeRequired) {
                Invoke(new ThreadStart(positionChangedEventHandlerCore));
            } else {
                positionChangedEventHandlerCore();
            }
        }

        private void positionChangedEventHandlerCore() {
            if (isEventDispatchingEnabled) {
                EventHandler handler = PositionChanged;
                if (handler != null) {
                    handler.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Signals that position has been changed.
        /// Event will be invoked from appropriate thread.
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        /// Signals that movie has been played and stopped at the end.
        /// Event will be invoked from appropriate thread.
        /// </summary>
        public event EventHandler EndReached;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                isEventDispatchingEnabled = false;
                // Clean up vlclib resources
                if (player != null) {
                    player.Dispose();
                }
                if (mediaLibraryFactory != null) {
                    mediaLibraryFactory.Dispose();
                }
            }
            //
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Debug-style finalizer.
        /// </summary>
        ~VlcPlayerControl() {
#if DEBUG
            Debugger.Log(1, Debugger.DefaultCategory, String.Format("Finalizer of {0} has been called. The object has not been disposed correctly.\n", GetType()));
#endif
        }

        /// <summary>
        /// Starts playing.
        /// </summary>
        /// <param name="mediaInput">Media to play.</param>
        public void Play(MediaInput mediaInput) {
            if (mediaInput == null) {
                throw new ArgumentNullException("mediaInput");
            }
            //
            lazyInitialize();
            //
            try {
                currentPlaying = mediaInput;
                player.SetMediaInput(mediaInput);
                //player.SetNextMediaInput(mediaInput);
                player.Volume = volume;
                //player.PlayNext();
                player.Play();

                //
                isEventDispatchingEnabled = true;
                setCurrentState(VlcPlayerControlState.PLAYING);
            } catch {
                Stop();
                throw;
            }
        }

        private void setCurrentState(VlcPlayerControlState _state) {
            state = _state;
            //
            EventHandler handler = StateChanged;
            if (handler != null) {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Pauses or resumes playing current movie.
        /// If no movie is loaded, no actions will be given.
        /// </summary>
        public void PauseOrResume() {
            if ((state != VlcPlayerControlState.PLAYING) && (state != VlcPlayerControlState.PAUSED)) {
                return;
            }
            //
            lazyInitialize();
            //
            if (state == VlcPlayerControlState.PLAYING) {
                try {
                    player.Pause();
                    setCurrentState(VlcPlayerControlState.PAUSED);
                } catch {
                    Stop();
                    throw;
                }
            } else if (state == VlcPlayerControlState.PAUSED) {
                try {
                    player.Resume();
                    setCurrentState(VlcPlayerControlState.PLAYING);
                } catch {
                    Stop();
                    throw;
                }
            }
        }

        /// <summary>
        /// Stops currently playing movie if player is not empty.
        /// </summary>
        public void Stop() {
            if (state != VlcPlayerControlState.IDLE) {
                lazyInitialize();
                //
                try {
                    isEventDispatchingEnabled = false;
                    //
                    player.Stop();
                } finally {
                    currentPlaying = null;
                    setCurrentState(VlcPlayerControlState.IDLE);
                }
            }
        }

        private MediaInput currentPlaying;

        /// <summary>
        /// Current playing item.
        /// </summary>
        public MediaInput CurrentPlaying {
            get {
                return (currentPlaying);
            }
        }

        /// <summary>
        /// Position of playing movie.
        /// (0.0 - 1.0).
        /// </summary>
        public double Position {
            get {
                if ((state != VlcPlayerControlState.PAUSED) && (state != VlcPlayerControlState.PLAYING)) {
                    return (0);
                }
                //
                try {
                    return (player.Position);
                } catch {
                    Stop();
                    throw;
                }
            }
            set {
                if ((state != VlcPlayerControlState.PAUSED) && (state != VlcPlayerControlState.PLAYING)) {
                    return;
                }
                //
                try {
                    player.Position = (float) value;
                } catch {
                    Stop();
                    throw;
                }
            }
        }

        /// <summary>
        /// Current playing time.
        /// </summary>
        public TimeSpan Time {
            get {
                if ((state != VlcPlayerControlState.PAUSED) && (state != VlcPlayerControlState.PLAYING)) {
                    return (TimeSpan.Zero);
                }
                //
                try {
                    return (player.Time);
                } catch (Exception) {
                    Stop();
                    throw;
                }
            }
            set {
                if ((state == VlcPlayerControlState.PLAYING) || (state == VlcPlayerControlState.PAUSED)) {
                    try {
                        player.Time = value;
                    } catch {
                        Stop();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Default volume level.
        /// </summary>
        public const int DEFAULT_VOLUME = 50;

        private int volume = DEFAULT_VOLUME;

        /// <summary>
        /// Volume level.
        /// </summary>
        public int Volume {
            get {
                return (volume);
            }
            set {
                if ((volume < 0) || (volume > 100)) {
                    throw new ArgumentException("Argument is out of range.", "value");
                }
                //
                if (volume != value) {
                    volume = value;
                    //
                    if (state != VlcPlayerControlState.IDLE) {
                        player.Volume = volume;
                    }
                }
            }
        }
    }
}