using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Common.Logging;
using DZ.MediaPlayer.Io;
using DZ.MediaPlayer.Vlc.WindowsForms;
using SimplePlayer.MediaInfo;
using SimplePlayer.Playlist;

namespace SimplePlayer
{
    /// <summary>
    /// Main form of application.
    /// </summary>
    public partial class MainWindow : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainWindow));
        private VideoWindow videoWindow;

        #region Initialization and clean up

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            //
            initializeVideoWindow();
            initializeStatusBar();
            initializeTrackbarPosition();
            initializeTrackbarVolume();
        }

        private void initializeVlcPlayerControl(bool showVideoWindow) {
            initializeVideoWindow();
            //
            if (showVideoWindow) {
                videoWindow.Show();
            }
            //
            if (!videoWindow.VlcPlayerControl.IsInitialized) {
                videoWindow.VlcPlayerControl.Initialize();
            }
        }

        private void initializeVideoWindow() {
            if ((videoWindow == null) || (videoWindow.IsDisposed)) {
                videoWindow = new VideoWindow();
                videoWindow.Closing += VideoWindowOnClosing;
                playlistEditorControl.Playlist.PlaylistItemEntered += Playlist_PlaylistItemEntered;
                //
                videoWindow.VlcPlayerControl.StateChanged += VlcPlayerControl1OnStateChanged;
                videoWindow.VlcPlayerControl.PositionChanged += VlcPlayerControlOnPositionChanged;
                videoWindow.VlcPlayerControl.EndReached += VlcPlayerControlOnEndReached;
            }
        }

        private void initializeStatusBar() {
            statusStrip.Items["playerStatus"].Text = Convert.ToString(videoWindow.VlcPlayerControl.State);
            statusStrip.Items["currentTime"].Text = String.Format("{0}", videoWindow.VlcPlayerControl.Time);
        }

        private void initializeTrackbarPosition() {
            trackbarPosition.Tag = true;
        }

        private void initializeTrackbarVolume() {
            trackbarVolume.Value = (int) ((1.0 * videoWindow.VlcPlayerControl.Volume / 100) * trackbarVolume.Maximum);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            // It is necessary to call videoWindow.Dispose() explicitly because
            // if it is in hidden state, Dispose() will not be called from anywhere else
            if (videoWindow != null) {
                videoWindow.Dispose();
            }
            //
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Internal objects events handling routine

        private void VlcPlayerControl1OnStateChanged(object sender, EventArgs args) {
            VlcPlayerControlState currentState = videoWindow.VlcPlayerControl.State;
            //
            switch (currentState) {
                case VlcPlayerControlState.IDLE: {
                    buttonPlay.Text = "Play";
                    break;
                }
                case VlcPlayerControlState.PAUSED: {
                    buttonPlay.Text = "Resume";
                    break;
                }
                case VlcPlayerControlState.PLAYING: {
                    buttonPlay.Text = "Pause";
                    break;
                }
            }
            //
            statusStrip.Items["playerStatus"].Text = Convert.ToString(currentState);
        }

        private void VlcPlayerControlOnEndReached(object sender, EventArgs e) {
            buttonPlaynext_Click(this, EventArgs.Empty);
        }

        private void VlcPlayerControlOnPositionChanged(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(new ThreadStart(updateTrackBar));
            } else {
                updateTrackBar();
                updateStatusBar();
            }
        }

        private void updateTrackBar() {
            trackbarPosition.Tag = false;
            int newTrackbarPositionValue = (int) (videoWindow.VlcPlayerControl.Position * 1000);
            trackbarPosition.Value = newTrackbarPositionValue > trackbarPosition.Maximum ? trackbarPosition.Maximum : newTrackbarPositionValue;
            trackbarPosition.Tag = true;
        }

        private void updateStatusBar() {
            TimeSpan time = videoWindow.VlcPlayerControl.Time;
            statusStrip.Items["currentTime"].Text = String.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
        }

        void Playlist_PlaylistItemEntered(object sender, PlaylistItemEnteredEventArgs e) {
            PlaylistItem currentItem = playlistEditorControl.Playlist.CurrentItem;
            if (currentItem == null) {
                return;
            }
            //
            try {
                BasicVideoInformation information = MediaInfoHelper.GetBasicVideoInfo(currentItem.MediaInput.Source);
                if (String.IsNullOrEmpty(information.AudioCodec) && String.IsNullOrEmpty(information.VideoCodec)) {
                    currentItem.IsError = true;
                    stopPlayer();
                    return;
                }
                //
                initializeVlcPlayerControl(true);
                //
                currentItem.IsError = false;
                videoWindow.VlcPlayerControl.Play(currentItem.MediaInput);
            } catch (FileNotFoundException exc) {
                if (logger.IsWarnEnabled) {
                    logger.Warn(String.Format("File referenced from playlist item was not found. Exception : {0}", exc));
                }
                //
                currentItem.IsError = true;
            } catch (Exception exc) {
                if (logger.IsErrorEnabled) {
                    logger.Error("Cannot start playing.", exc);
                }
                //
                MessageBox.Show(String.Format("Cannot start playing : {0}", exc));
                currentItem.IsError = true;
            }
        }

        private void stopPlayer() {
            try {
                if (videoWindow.VlcPlayerControl.State != VlcPlayerControlState.IDLE) {
                    videoWindow.VlcPlayerControl.Stop();
                    updateTrackBar();
                    updateStatusBar();
                }
            } catch (Exception exc) {
                if (logger.IsErrorEnabled) {
                    logger.Error("Cannot stop player.", exc);
                }
                //
                MessageBox.Show(String.Format("Cannot stop player : {0}", exc));
            }
        }

        #endregion

        #region UI events

        private void buttonPlay_Click(object sender, EventArgs e) {
            PlaylistItem currentItem = playlistEditorControl.Playlist.CurrentItem;
            if (currentItem == null) {
                return;
            }
            //
            try {
                BasicVideoInformation information = MediaInfoHelper.GetBasicVideoInfo(currentItem.MediaInput.Source);
                if (String.IsNullOrEmpty(information.AudioCodec) && String.IsNullOrEmpty(information.VideoCodec)) {
                    currentItem.IsError = true;
                    stopPlayer();
                    return;
                }
                //
                initializeVlcPlayerControl(true);
                //
                currentItem.IsError = false;
                switch (videoWindow.VlcPlayerControl.State) {
                    case VlcPlayerControlState.IDLE: {
                        videoWindow.VlcPlayerControl.Play(currentItem.MediaInput);
                        break;
                    }
                    case VlcPlayerControlState.PAUSED: {
                        videoWindow.VlcPlayerControl.PauseOrResume();
                        break;
                    }
                    case VlcPlayerControlState.PLAYING: {
                        videoWindow.VlcPlayerControl.PauseOrResume();
                        break;
                    }
                }
            } catch (FileNotFoundException exc) {
                if (logger.IsWarnEnabled) {
                    logger.Warn(String.Format("File referenced from playlist item was not found. Exception : {0}", exc));
                }
                //
                currentItem.IsError = true;
            } catch (Exception exc) {
                if (logger.IsErrorEnabled) {
                    logger.Error("Cannot start playing.", exc);
                }
                //
                MessageBox.Show(String.Format("Cannot start playing : {0}", exc));
                currentItem.IsError = true;
            }
        }

        private void buttonPlayback_Click(object sender, EventArgs e) {
            playlistEditorControl.Playlist.MovePrev();
            PlaylistItem currentItem = playlistEditorControl.Playlist.CurrentItem;
            if (currentItem == null) {
                return;
            }
            //
            try {
                BasicVideoInformation information = MediaInfoHelper.GetBasicVideoInfo(currentItem.MediaInput.Source);
                if (String.IsNullOrEmpty(information.AudioCodec) && String.IsNullOrEmpty(information.VideoCodec)) {
                    currentItem.IsError = true;
                    stopPlayer();
                    //
                    return;
                }
                //
                initializeVlcPlayerControl(true);
                //
                currentItem.IsError = false;
                videoWindow.VlcPlayerControl.Play(currentItem.MediaInput);
            } catch (FileNotFoundException exc) {
                if (logger.IsWarnEnabled) {
                    logger.Warn(String.Format("File referenced from playlist item was not found. Exception : {0}", exc));
                }
                //
                currentItem.IsError = true;
            } catch (Exception exc) {
                if (logger.IsErrorEnabled) {
                    logger.Error("Cannot start playing.", exc);
                }
                //
                MessageBox.Show(String.Format("Cannot start playing : {0}", exc));
                currentItem.IsError = true;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e) {
            stopPlayer();
        }

        private void buttonPlaynext_Click(object sender, EventArgs e) {
            playlistEditorControl.Playlist.MoveNext();
            PlaylistItem currentItem = playlistEditorControl.Playlist.CurrentItem;
            if (currentItem == null) {
                return;
            }
            //
            try {
                BasicVideoInformation information = MediaInfoHelper.GetBasicVideoInfo(currentItem.MediaInput.Source);
                if (String.IsNullOrEmpty(information.AudioCodec) && String.IsNullOrEmpty(information.VideoCodec)) {
                    currentItem.IsError = true;
                    stopPlayer();
                    return;
                }
                //
                initializeVlcPlayerControl(true);
                //
                currentItem.IsError = false;
                videoWindow.VlcPlayerControl.Play(currentItem.MediaInput);
            } catch (FileNotFoundException exc) {
                if (logger.IsWarnEnabled) {
                    logger.Warn(String.Format("File referenced from playlist item was not found. Exception : {0}", exc));
                }
                //
                currentItem.IsError = true;
            } catch (Exception exc) {
                if (logger.IsErrorEnabled) {
                    logger.Error("Cannot start playing.", exc);
                }
                //
                MessageBox.Show(String.Format("Cannot start playing : {0}", exc));
                currentItem.IsError = true;
            }
        }

        /// <summary>
        /// Deny video window to close.
        /// </summary>
        private void VideoWindowOnClosing(object sender, CancelEventArgs args) {
            stopPlayer();
            args.Cancel = true;
            videoWindow.Hide();
        }

        private void openFilesToolStripMenuItem_Click(object sender, EventArgs e) 
        {
                using (OpenFileDialog openFileDialog = new OpenFileDialog()) 
                {
                    openFileDialog.Multiselect = true;
                    DialogResult result = openFileDialog.ShowDialog();
                    if ((result == DialogResult.OK) || (result == DialogResult.Yes)) {
                        foreach (string fileName in openFileDialog.FileNames) {
                            try {
                                if (!File.Exists(fileName)) {
                                    continue;
                                }
                                BasicVideoInformation information = MediaInfoHelper.GetBasicVideoInfo(mediaInfoLibrary, fileName);
                                PlaylistItem playlistItem = new PlaylistItem(
                                    new MediaInput(MediaInputType.File, fileName),
                                    fileName,
                                    TimeSpan.FromMilliseconds(10000));
                                //
                                playlistEditorControl.Playlist.Items.Add(playlistItem);
                            } catch (Exception exc) {
                                if (logger.IsErrorEnabled ) {
                                    logger.Error(String.Format("Error during processing selected file list : {0}", exc));
                            }
                        }
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Simple player ver. 0.1", "About..", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void trackbarPosition_ValueChanged(object sender, EventArgs e) {
            try {
                TrackBar trackBar = ((TrackBar) sender);
                if ((trackBar.Tag is Boolean) && ((Boolean) trackBar.Tag)) {
                    //
                    if (videoWindow.VlcPlayerControl.State == VlcPlayerControlState.IDLE) {
                        trackBar.Value = 0;
                        return;
                    }
                    //
                    videoWindow.VlcPlayerControl.Position = 1f * trackBar.Value / trackBar.Maximum;
                }
            } catch (Exception exc) {
                if (logger.IsErrorEnabled) {
                    logger.Error("Cannot change position.", exc);
                }
                //
                MessageBox.Show(String.Format("Cannot change position : {0}", exc));
            }
        }

        #endregion

        private void trackbarVolume_ValueChanged(object sender, EventArgs e) {
            try {
                TrackBar trackBar = (TrackBar) sender;
                videoWindow.VlcPlayerControl.Volume = (int) ((1.0 * trackBar.Value / trackBar.Maximum) * 100);
            } catch (Exception exc) {
                if (logger.IsErrorEnabled) {
                    logger.Error("Cannot change volume level.", exc);
                }
                //
                MessageBox.Show(String.Format("Cannot change volume level: {0}", exc));
            }
        }
    }
}
