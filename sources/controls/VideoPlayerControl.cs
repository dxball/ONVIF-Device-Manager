#region License and Terms
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
//----------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using nvc.utils;
using System.Threading;
using liblenin;

namespace nvc.controls
{

    public partial class VideoPlayerControl : UserControl
    {
        private VlcControlInner m_VlcControl = null;

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
                    m_VlcControl = new VlcControlInner();
                    Controls.Add(m_VlcControl);
                    m_VlcControl.Dock = DockStyle.Fill;
                    m_VlcControl.Play(medInput);
                }
                catch (Exception ex)
                {
                    ReleaseAll();
                    InitError(ex.Message);
                }
            }
        }

        private void InitError(string p)
        {
            Controls.Clear();

            var error = new ErrorMessageControl();
			error.Title = Constants.Instance.sErrorVlc;
            error.Message = p;
            error.Dock = DockStyle.Fill;

            Controls.Add(error);
            Dock = DockStyle.Fill;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal void Stop()
        {
            try
            {
                if (null != m_VlcControl)
                {
                    m_VlcControl.Stop();
                }
            }
            finally
            {
                ReleaseAll();
            }
        }

        internal void Play(string p)
        {
            try
            {
                if (null != m_VlcControl)
                {
                    m_VlcControl.Play(p);
                }
            }
            finally
            {
                ReleaseAll();
            }
        }

        internal void ReleaseAll()
        {
            if (null != m_VlcControl)
            {
                m_VlcControl.Dispose();
            }
        }
    }

    class VlcControlInner : Panel
    {
        private Vlc mVlc = null;
        private MediaPlayer mPlayer = null;
        private bool mIsPlaying = false;

        public VlcControlInner()
        {
            mVlc = new Vlc();
        }

        public void Stop()
        {
            if (mIsPlaying)
            {
                mPlayer.Stop();
                mPlayer.Dispose();
                mPlayer = null;
                mIsPlaying = false;
            }
        }

        public void Play(string url)
        {
            Stop();
            mPlayer = mVlc.CreateMediaPlayer(url);
            mPlayer.SetHwnd(this.Handle);
            mPlayer.Play();
            mIsPlaying = true;
        }

        protected override void Dispose(bool disposing)
        {
            Stop();
            if (null != mVlc)
            {
                mVlc.Dispose();
                mVlc = null;
            }
            base.Dispose(disposing);
        }
    }
}
