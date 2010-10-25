using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Disposables;

using onvifdm.utils;
using System.Drawing.Imaging;

namespace liblenin {
	public class VlcControlInner : Panel {
		//private Vlc mVlc = null;
		private VlcPlayer m_vlcPlayer = null;
		private bool mIsPlaying = false;

		//private System.Drawing.Imaging.BitmapData mBmpData = null;
		//private Bitmap m_bitmap;
		//private Rectangle mBitmapRect;

		private VlcPlayer.LockBuffer mLockBuffer;
		private VlcPlayer.UnlockBuffer mUnlockBuffer;
		private VlcPlayer.DisplayBuffer mDisplayBuffer;

		private Graphics mGraphics = null;
		private object m_gate = new object();

		//protected override void OnParentChanged(EventArgs e) {
		//    base.OnParentChanged(e);
		//}
			
		protected override void OnHandleDestroyed(EventArgs e) {
			//DebugHelper.Assert(m_state == State.diposed);
			if (m_state != State.diposed && m_state != State.disposing) {
				//DebugHelper.Assert(m_state != State.disposing);
				Cleanup();
			}
			base.OnHandleDestroyed(e);
		}
		public VlcControlInner() {
			//mVlc = Vlc.Instance;
			//this.DoubleBuffered = true;
			this.Resize += (sender, args)=>{
				lock (m_gate) {
					mGraphics = this.CreateGraphics();
				}
			};
		}

		~VlcControlInner() {
			DebugHelper.Assert(m_state == State.diposed);
			DebugHelper.Error("finalizer");
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				Cleanup();
			}
			// free unmanaged resources here
			base.Dispose(disposing);
		}

		private void Cleanup() {
			DebugHelper.Assert(m_state != State.disposing);
			if (m_state != State.diposed || m_state != State.disposing) {
				m_state = State.disposing;
				this.Stop();
				m_state = State.diposed;				
			}
		}

		public void Stop() {

			if (mIsPlaying && null != m_vlcPlayer) {
				lock (m_gate) {
					if (m_state == State.playing) {
						m_state = State.idle;
					}
				}

				mIsPlaying = false;
				//m_vlcPlayer.SetCallbacks(null, null, null);
				m_vlcPlayer.Stop();
				m_vlcPlayer.Dispose();
				m_vlcPlayer = null;

			}
		}

		//public Action<Graphics, Rectangle> editor;
		//public void CallEditor(Graphics bmp, Rectangle rect) {
		//    if (editor != null)
		//        editor(bmp, rect);
		//}

		public void Play(string url) {
			Stop();
			m_vlcPlayer = VlcLib.Instance.CreateMediaPlayer(url);
			m_vlcPlayer.SetHwnd(this.Handle);
			lock (m_gate) {
				DebugHelper.Assert(m_state == State.idle);
				m_vlcPlayer.Play();
				mIsPlaying = true;
				m_state = State.playing;
			}
		}

		public void Play(string url, Size resolution, Action<Graphics, Rectangle> callback) {
			//if (callback == null) {
			//    throw new ArgumentNullException("callback");
			//}
			Stop();
			m_vlcPlayer = VlcLib.Instance.CreateMediaPlayer(url);
			var stream = resolution;//new Size(768, 576);//m_vlcPlayer.size;
			//var waWidth = Screen.PrimaryScreen.WorkingArea.Width;
			//var waHeight = Screen.PrimaryScreen.WorkingArea.Height;
			var frame_bitmap = new Bitmap(stream.Width, stream.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			Rectangle frame_rect = new Rectangle();
			frame_rect.X = 0;
			frame_rect.Y = 0;
			frame_rect.Width = stream.Width;
			frame_rect.Height = stream.Height;
			BitmapData data = null;

			//BitmapData
			//mBitmapRect = new Rectangle {
			//    X = 0,
			//    Y = 0,
			//    Width = mBitmap.Width,
			//    Height = mBitmap.Height
			//};
			mGraphics = this.CreateGraphics();
			mLockBuffer = new VlcPlayer.LockBuffer((a, b) => {
					//DebugHelper.Assert(m_state == State.playing);
				DebugHelper.Assert(m_state != State.processing_frame);
				Debug.Assert(m_state != State.processing_frame);
					Monitor.Enter(m_gate);
					if (m_state == State.playing) {
						m_state = State.processing_frame;
					}
				//lock (m_gate) {
					
					//DebugHelper.Assert(m_state == State.playing);
					
					try {
						data = frame_bitmap.LockBits(frame_rect,
								System.Drawing.Imaging.ImageLockMode.ReadWrite,
								System.Drawing.Imaging.PixelFormat.Format32bppArgb);
						Marshal.WriteIntPtr(b, data.Scan0);
					} catch (Exception e) {
						DebugHelper.Error(e.Message);
					}
					return IntPtr.Zero;
				//}
			});


			mUnlockBuffer = new VlcPlayer.UnlockBuffer((a, b, c) => {
				//lock (m_gate) {
				frame_bitmap.UnlockBits(data);
				DebugHelper.Assert(m_state != State.playing);
				//if (m_state != State.playing) {
				//    return;
				//}
				//m_state = State.processing_frame;
				if (m_state == State.processing_frame) {
					try {

						if (0 != this.Width && 0 != this.Height) {
							using (Bitmap bm = new Bitmap(this.Width, this.Height)) {
								using (Graphics grSrc = Graphics.FromImage(bm)) {
									grSrc.FillRectangle(Brushes.Black, this.WindowRectangle);
									grSrc.DrawImage(frame_bitmap, videoBounds);
									if (callback != null) {
										callback(grSrc, videoBounds);
									}
									this.mGraphics.DrawImageUnscaled(bm, 0, 0);
								}
							}


						}
					} catch (Exception err) {
						DebugHelper.Error(err.StackTrace);
					}
					m_state = State.playing;
				}
				Monitor.Exit(m_gate);
				//}
			});
			mDisplayBuffer = new VlcPlayer.DisplayBuffer((a, b) => {
				
			});

			m_vlcPlayer.SetCallbacks(mLockBuffer, mUnlockBuffer, mDisplayBuffer);
			//m_vlcPlayer.SetHwnd(this.Handle);
			m_vlcPlayer.SetFormat("RV32", (UInt32)stream.Width, (UInt32)stream.Height, (UInt32)stream.Width * 4);
			lock (m_gate) {
				DebugHelper.Assert(m_state == State.idle);
				m_vlcPlayer.Play();
				mIsPlaying = true;
				m_state = State.playing;
			}
		}

		protected enum State {
			playing,
			processing_frame,
			idle,
			disposing,
			diposed
		};

		protected State m_state = State.idle;

		private Rectangle _WindowRectangle = Rectangle.Empty;
		public Rectangle WindowRectangle {
			get {
				if (Rectangle.Empty == _WindowRectangle) {
					_WindowRectangle = new Rectangle {
						X = 0,
						Y = 0
					};
				}
				_WindowRectangle.Width = this.Width;
				_WindowRectangle.Height = this.Height;
				return _WindowRectangle;
			}
		}

		public Rectangle videoBounds {
			get {
				Rectangle r = new Rectangle();

				double kx = ((Double)this.Width) / ((Double)m_vlcPlayer.size.Width);
				double ky = ((Double)this.Height) / ((Double)m_vlcPlayer.size.Height);

				if (ky > kx) {
					var h = m_vlcPlayer.size.Height * kx;
					r.Width = Width;
					r.Height = (int)h;
					r.X = 0;
					r.Y = (int)((Height - h) * 0.5);
					return r;
				}

				if (kx > ky) {
					var w = m_vlcPlayer.size.Width * ky;
					r.Width = (int)w;
					r.Height = Height;
					r.X = (int)((Width - w) * 0.5);
					r.Y = 0;
					return r;
				}

				r.Width = Width;
				r.Height = Height;
				r.X = 0;
				r.Y = 0;

				return r;
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			//base.OnPaint(e);
			//CallEditor(e.Graphics, this.VideoRectangle);
		}

		//using (Bitmap bm = new Bitmap(this.Width, this.Height))
		//{
		//    using (Graphics clientDC = this.CreateGraphics())
		//    {
		//        IntPtr hdc = clientDC.GetHdc();
		//        IntPtr memdc = CreateCompatibleDC(hdc);
		//        SelectObject(memdc, bm.GetHbitmap());
		//        using (Graphics gSrc = Graphics.FromHdc(memdc))
		//        {
		//            gSrc.FillRectangle(Brushes.Black, this.WindowRectangle);
		//            gSrc.DrawImage(mBitmap, this.VideoRectangle);
		//            //CallEditor(bm);
		//            CallEditor(gSrc, this.VideoRectangle);
		//            //gr.DrawImageUnscaled(bm, 0, 0);

		//            using (Graphics gr = this.CreateGraphics())
		//            {
		//                IntPtr grHdc = gr.GetHdc();
		//                IntPtr gSrcHdc = gSrc.GetHdc();
		//                BitBlt(grHdc, 0, 0, this.Width, this.Height, gSrcHdc, 0, 0, SRCCOPY);//
		//                //gr.DrawImage(bm, new Rectangle { X = 0, Y = 0, Width = this.Width, Height = this.Height });

		//                gr.ReleaseHdc(grHdc);
		//                gSrc.ReleaseHdc(gSrcHdc);
		//            }
		//        }
		//        clientDC.ReleaseHdc(hdc);
		//        DeleteDC(memdc);
		//    }
		//}		
	}
}
