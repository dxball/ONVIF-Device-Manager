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
using System.Threading;
using odm.utils;
using System.Globalization;
using System.Diagnostics;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for VideoPlayer.xaml
	/// </summary>
	public partial class VideoPlayer : BaseVideoPlayer {
		public VideoPlayer() {
			InitializeComponent();
		}
		public void ReleaseAll() {
			if (_ftimer != null) {
				_ftimer.Stop();
				_ftimer.Dispose();
			}
			_ftimer = null;
		}

		public void InitPlayback(Rect res) {
			//Start Playback
			try {
				_resolution = res;
				pBox = SetImage;

				_ftimer = new System.Windows.Forms.Timer();
				_ftimer.Interval = 30;
				_ftimer.Tick += new EventHandler(_ftimer_Tick);
				_ftimer.Start();
			} catch (Exception err) {
				MessageBox.Show(err.Message);
			}
			//Stop Playback
		}

		bool isInit = true;
		WriteableBitmap wrBmp;

		void InitBitmap(Rect size) {
			try {
				wrBmp = new WriteableBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32, null);
				_imgVIew.Source = wrBmp;

			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		protected double fps = 0;
		protected long fpsLastUpdateTime;
		protected Stopwatch stopWatch = new Stopwatch();
		protected Buffer<long> frameTimespans = new Buffer<long>(100);
		public Action<Rect> Refresh;

		public void SetImage(IntPtr ptr, Rect size) {
			if (Refresh != null)
				Refresh(size);
			if (isInit) {
				InitBitmap(size);
				isInit = false;
			} else {
				try {
					if (!stopWatch.IsRunning) {
						frameTimespans.Clear();
						stopWatch.Start();
						fpsLastUpdateTime = stopWatch.ElapsedMilliseconds;
						fps = 1.0;
					}

					frameTimespans.Push(stopWatch.ElapsedMilliseconds);
					fpsLastUpdateTime = stopWatch.ElapsedMilliseconds;
					var time = frameTimespans.last - frameTimespans.first;
					if (time != 0) {
						fps = ((double)frameTimespans.length * 1000) / (double)time;
					} else {
						fps = 0;
					}

					int buffSize = (int)size.Width * (int)size.Height * 4;
					int stride = (int)size.Width * 4;

					try {
						wrBmp.Lock();
						wrBmp.WritePixels(new Int32Rect(0, 0, (int)size.Width, (int)size.Height), ptr, buffSize, stride, 0, 0);
						wrBmp.Unlock();
					} catch (Exception err) {
						dbg.Error(err);
					}


					textBlock1.Text = fps.ToString();
				} catch (Exception ex) {
					//MessageBox.Show("Failed to manipulate image:\n" + ex.Message);
				}
			}
		}
		private void BaseVideoPlayer_Loaded(object sender, RoutedEventArgs e) {

		}
	}
}
