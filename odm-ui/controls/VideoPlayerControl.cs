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
using onvifdm.utils;
using System.Threading;
//using vlc_net;
using System.Runtime.InteropServices;

namespace nvc.controls
{

    public partial class VideoPlayerControl : BasePropertyControl
    {
		//public VlcControlInner m_VlcControl = null;
		//string _medInput;

		//public Size _mediaStreamSize;
		//public Action<Graphics, Rectangle> _action;

		//public VideoPlayerControl(string medInput)
		//{
		//    InitializeComponent();
		//    _medInput = medInput;
		//    Load += new EventHandler(VideoPlayerControl_Load);
		//}

		//~VideoPlayerControl() {
		//    DebugHelper.Break();			
		//}

		//public override void ReleaseUnmanaged() {
		//    if (m_VlcControl != null) {
		//        m_VlcControl.Stop();
		//        m_VlcControl.Dispose();
		//    }
		//}

		//void VideoPlayerControl_Load(object sender, EventArgs e) {
		//    DebugHelper.Assert(Program.uiThread == Thread.CurrentThread);
			
		//    if (null == _medInput || _medInput.IsEmpty()) {
		//        InitError(ExceptionStrings.Instance.sErrorVlcMediaUriIsEmpty);
		//    } else if (!Uri.IsWellFormedUriString(_medInput, UriKind.RelativeOrAbsolute)) {
		//        InitError(ExceptionStrings.Instance.sErrorVlcMediaUriIsBad);
		//    } else {
		//        try {
		//            m_VlcControl = new VlcControlInner();
		//            Controls.Add(m_VlcControl);
		//            m_VlcControl.MouseClick += new MouseEventHandler(m_VlcControl_MouseClick);
		//            //m_VlcControl.Dock = DockStyle.Fill;
		//            m_VlcControl.Location = Location;
		//            m_VlcControl.Size = Size;
		//            m_VlcControl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
		//            m_VlcControl.Dock = DockStyle.Fill;
		//            if (_action != null)
		//                Play(_medInput, _mediaStreamSize, _action);
		//            else
		//                m_VlcControl.Play(_medInput);
		//        } catch {
		//            ReleaseAll();
		//            InitError(ExceptionStrings.Instance.sErrorVlcPlayer);
		//        }
		//    }
		//}

		//void m_VlcControl_MouseClick(object sender, MouseEventArgs e) {
		//    //throw new NotImplementedException();
		//}

		//private void InitError(string p)
		//{
		//    Controls.Clear();

		//    var error = new InfoPageError();			
		//    //error.Title = ExceptionStrings.Instance.sErrorVlc;
		//    //error.Message = p;
		//    error.Dock = DockStyle.Fill;

		//    Controls.Add(error);
		//    Dock = DockStyle.Fill;
		//}

		///// <summary> 
		///// Clean up any resources being used.
		///// </summary>
		///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		//protected override void Dispose(bool disposing)
		//{
		//    if (disposing && (components != null))
		//    {
		//        components.Dispose();
		//    }
		//    base.Dispose(disposing);
		//}

		//internal void Stop()
		//{
		//    try
		//    {
		//        if (null != m_VlcControl)
		//        {
		//            m_VlcControl.Stop();
		//        }
		//    }
		//    catch
		//    {
		//        ReleaseAll();
		//        InitError(ExceptionStrings.Instance.sErrorVlcPlayer);
		//    }
		//}

		//public void Play(string url, Size medResolution, Action<Graphics, Rectangle> action) {
		//    try {
		//        if (null != m_VlcControl) {
		//            m_VlcControl.Play(url, medResolution, action);
		//        }
		//    } catch {
		//        ReleaseAll();
		//        InitError(ExceptionStrings.Instance.sErrorVlcPlayer);
		//    }
		//}

		//internal void Play(string p)
		//{
		//    try
		//    {
		//        if (null != m_VlcControl)
		//        {
		//            m_VlcControl.Play(p);
		//        }
		//    }
		//    catch
		//    {
		//        ReleaseAll();
		//        InitError(ExceptionStrings.Instance.sErrorVlcPlayer);
		//    }
		//}

		//internal void ReleaseAll()
		//{
		//    //if (null != m_VlcControl)
		//    //{
		//    //    try
		//    //    {
		//    //        m_VlcControl.Dispose();
		//    //        m_VlcControl = null;
		//    //    }
		//    //    catch
		//    //    {
		//    //        //do nothing
		//    //    }
		//    //    Controls.Clear();
		//    //}
		//}
    }
}
