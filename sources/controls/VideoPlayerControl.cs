//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//
//----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DZ.MediaPlayer;
using DZ.MediaPlayer.Vlc;
using DZ.MediaPlayer.Io;
using DZ.MediaPlayer.Vlc.Deployment;
using System.IO;
using nvc.utils;
using System.Threading;

namespace nvc.controls
{
    public partial class VideoPlayerControl : UserControl
    {
        public VideoPlayerControl(string medInput)
        {
			InitializeComponent();
            if (null == medInput || medInput.IsEmpty())
            {
                InitError(Constants.Instance.sErrorVlcMediaUriIsEmpty);
            }
            else if (!Uri.IsWellFormedUriString(medInput, UriKind.RelativeOrAbsolute))
            {
				InitError(Constants.Instance.sErrorVlcMediaUriIsBad);
            }
            else
            {
                try
                {
                    _medInput = new MediaInput(MediaInputType.NetworkStream, medInput);
                    InitVideoPlayer();
                }
                catch (Exception ex)
                {
                    InitError(ex.Message);
                }
            }
        }

		public void ReleaseAll() {
            if (null != VlcSingleton.Instance)
            {
                VlcSingleton.Instance.Load -= _loadVlc;
                Controls.Remove(VlcSingleton.Instance);
                if (VlcSingleton.Instance.State == DZ.MediaPlayer.Vlc.WindowsForms.VlcPlayerControlState.PLAYING)
                {
                    VlcSingleton.Instance.Stop();
                }
            }
		}

        private void InitError(string p)
        {
            var error = new ErrorMessageControl();
			error.Title = Constants.Instance.sErrorVlc;
            error.Message = p;
            error.Dock = DockStyle.Fill;

            Controls.Add(error);
            Dock = DockStyle.Fill;
        }

        protected MediaInput _medInput;
        protected EventHandler _loadVlc;

        protected void InitVideoPlayer()
        {
            try
            {
                _loadVlc = new EventHandler(_vlcPlayer_Load);
                VlcSingleton.Instance.Load += _loadVlc;
                VlcSingleton.Instance.Dock = DockStyle.Fill;
                Controls.Add(VlcSingleton.Instance);
                Dock = DockStyle.Fill;
            }
            catch (Exception e)
            {
                InitError(e.Message);
            }
        }

        void _vlcPlayer_Load(object sender, EventArgs e)
        {
            try
            {
                VlcSingleton.Instance.Play(_medInput);
                VlcSingleton.Instance.Refresh();
            }
            catch (Exception ex)
            {
                if (null != VlcSingleton.Instance)
                {
                    Controls.Remove(VlcSingleton.Instance);
                }
                InitError(ex.Message);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (VlcSingleton.Instance.IsDisposed)
            {
                throw new InvalidOperationException("Control is disposed");
            }
            if (null != VlcSingleton.Instance && !VlcSingleton.Instance.IsDisposed)
            {
                this.ReleaseAll();
            }
            //
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public sealed class VlcSingleton
    {
        private static readonly DZ.MediaPlayer.Vlc.WindowsForms.VlcPlayerControl _instance
            = new DZ.MediaPlayer.Vlc.WindowsForms.VlcPlayerControl();

        private VlcSingleton() { }

        public static DZ.MediaPlayer.Vlc.WindowsForms.VlcPlayerControl Instance
        {
            get
            {
                if (!_instance.IsInitialized)
                {
                    _instance.Initialize();
                }
                return _instance;
            }
        }
    }

}
