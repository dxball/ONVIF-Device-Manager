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
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Collections;
using onvifdm.utils;

namespace nvc.controls {
	

	public partial class BasePropertyControl : UserControl{
		public BasePropertyControl() {
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			InitializeComponent();
		}
		void Localization(){ }
		public virtual void ReleaseUnmanaged() { }
		public virtual void ReleaseAll() {
			if (_tmr != null)
				_tmr.Dispose();
		}
		public Action<Exception, string> onBindingError { get; set; }
		public Action<string> onVideoInitialisationError { get; set; }

		protected virtual void VideoOperationError(string message) {
			if (onVideoInitialisationError != null) {
				onVideoInitialisationError(message);
			}
		}
		protected virtual void BindingError(Exception err, string message) {
			if (onBindingError != null) {
				onBindingError(err, message);
			}
		}

		#region WORKAROUND
		//protected Process _vlcProcess;
		//int streamSize;
		//protected string dataFileName;

		public MemoryMappedFile memFile;
		Size _resolution;
		protected void CreateStandAloneVLC(string uri, Size resolution) {
			_resolution = resolution;
		}

		protected Timer _tmr;
		protected int _width = 0;
		protected int _heigth = 0;
		protected UserPictureBox pBox;
		
		

		protected double fps = 0;
		protected long fpsLastUpdateTime;
		protected Stopwatch stopWatch = new Stopwatch();
		protected Bitmap middleBmp = null;
		protected Buffer<long> frameTimespans = new Buffer<long>(100);
		
		protected Action<Graphics, Rectangle> drwaAction;
		protected void _tmr_Tick(object sender, EventArgs e) {
			//Refresh image
			try {
				var resolution = _resolution;
				if (!stopWatch.IsRunning) {					
					frameTimespans.Clear();
					stopWatch.Start();
					fpsLastUpdateTime = stopWatch.ElapsedMilliseconds;
					fps = 1.0;
				}

				frameTimespans.Push(stopWatch.ElapsedMilliseconds);
				//if (stopWatch.ElapsedMilliseconds - fpsLastUpdateTime > 300) {
					fpsLastUpdateTime = stopWatch.ElapsedMilliseconds;
					var time = frameTimespans.last - frameTimespans.first;
					if (time != 0) {
						fps = ((double)frameTimespans.length * 1000) / (double)time;
					} else {
						fps = 0;
					}
				//}
				
				
				if (middleBmp == null) {
					middleBmp = new Bitmap(pBox.Width, pBox.Height);
				}

				if (middleBmp.Width != pBox.Width || middleBmp.Height != pBox.Height) {
					if (middleBmp != null) {
						middleBmp.Dispose();						
					}
					middleBmp = new Bitmap(pBox.Width, pBox.Height);
				}

				using (var f = memFile.CreateViewStream()) {
					var scan0 = f.SafeMemoryMappedViewHandle.DangerousGetHandle();
					_width = resolution.Width;
					_heigth = resolution.Height;

					using (Bitmap bmp = new Bitmap(_width, _heigth, _width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, scan0)) {

						Rectangle boundRect = GetVideoBounds(pBox.ClientRectangle, new Rectangle(0, 0, _width, _heigth));
						
						using (var middleGraphics = Graphics.FromImage(middleBmp)) {
							middleGraphics.Clear(Color.Black);
							//middleGraphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, pBox.Width, pBox.Height));
							middleGraphics.DrawImage(bmp, boundRect);

#if SHOW_FPS
							middleGraphics.DrawString(String.Format("rendering fps: {0:F2}", fps), new Font(FontFamily.GenericSerif, 10, FontStyle.Regular), new SolidBrush(Color.LightGreen), 0, 0);
#endif
							if (drwaAction != null) {
								drwaAction(middleGraphics, boundRect);
							}
						}
					}

					using (var contolGraphix = pBox.CreateGraphics()) {
						contolGraphix.DrawImageUnscaled(middleBmp, 0, 0);
					}
					
					
				}
			} catch(Exception err) {
				DebugHelper.Error(err);
			    string msg = err.Message;
				File.AppendAllText("onvifDMplay.log", msg);
			}
		}
		public Rectangle GetVideoBounds(Rectangle clientRect, Rectangle videoRect) {
			Rectangle r = new Rectangle();

			double kx = ((Double)clientRect.Width) / ((Double)videoRect.Width);
			double ky = ((Double)clientRect.Height) / ((Double)videoRect.Height);

			if (ky > kx) {
				var h = videoRect.Height * kx;
				r.Width = clientRect.Width;
				r.Height = (int)h;
				r.X = 0;
				r.Y = (int)((clientRect.Height - h) * 0.5);
				return r;
			}

			if (kx > ky) {
				var w = videoRect.Width * ky;
				r.Width = (int)w;
				r.Height = clientRect.Height;
				r.X = (int)((clientRect.Width - w) * 0.5);
				r.Y = 0;
				return r;
			}

			r.Width = clientRect.Width;
			r.Height = clientRect.Height;
			r.X = 0;
			r.Y = 0;

			return r;
		}
		#endregion WORKAROUND
	}
}
