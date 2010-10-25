using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using onvifdm.utils;

namespace liblenin {
	public class VlcException : Exception {
		public VlcException(): base() {
		}
		public VlcException(string message): base(message) {
		}
		public VlcException(string message, Exception innerException): base(message, innerException) {
		}
	}

	public class VlcPlayer : IDisposable {

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr LockBuffer(IntPtr hUserData, IntPtr hBuffer);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void UnlockBuffer(IntPtr hUserData, IntPtr hPicture, IntPtr hBuffer);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void DisplayBuffer(IntPtr hUserData, IntPtr hPicture);
		
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void libvlc_video_set_mouse_input(IntPtr hMediaPlayer, UInt32 on);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void libvlc_video_set_callbacks(IntPtr hMdeiaPlayer, LockBuffer lb, UnlockBuffer ub, DisplayBuffer db, IntPtr hUserData);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void libvlc_video_set_format(IntPtr hMediaPlayer, string format, UInt32 width, UInt32 height, UInt32 pitch);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr libvlc_media_player_new_from_media(IntPtr hMedia);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void libvlc_media_player_release(IntPtr hMediaPlayer);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr libvlc_media_player_event_manager(IntPtr hMediaPlayer);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern Int32 libvlc_media_player_play(IntPtr hMediaPlayer);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void libvlc_media_player_stop(IntPtr hMediaPlayer);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void libvlc_media_player_set_hwnd(IntPtr hMediaPlayer, IntPtr hWnd);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int libvlc_video_get_size(IntPtr hMediaPlayer, UInt32 num, out UInt32 width, out UInt32 height);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr libvlc_media_player_get_media(IntPtr hMediaPlayer);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void libvlc_media_player_set_media(IntPtr hMediaPlayer, IntPtr hMedia);
		
		private bool disposed = false;
		public Size size{
			get{
				return GetSize();
			}
		}

		private static int s_palyer_cnt = 0;
		private IntPtr m_handle = IntPtr.Zero;
		private Media _media;
		
		~VlcPlayer() {
			DebugHelper.Assert(!disposed);
			DebugHelper.Error("finalizer");
			Dispose(false);
		}

		public void Dispose() {
			DebugHelper.Assert(!disposed);
			if (!disposed) {
				Dispose(true);
				GC.SuppressFinalize(this);
				disposed = true;
			}
		}
		
		public virtual void Dispose(bool disposing) {
			if (disposing) {
				// cleanup managed resources here
			}
			if (IntPtr.Zero != m_handle) {
				//SetHwnd(IntPtr.Zero);
				libvlc_media_player_release(m_handle);
				m_handle = IntPtr.Zero;
			}
			Interlocked.Decrement(ref s_palyer_cnt);
		}
		
		public VlcPlayer(IntPtr hMedia) {
			DebugHelper.Assert(s_palyer_cnt == 0);
			if (hMedia == IntPtr.Zero) {
				throw new ArgumentNullException("hMedia");
			}
			Interlocked.Increment(ref s_palyer_cnt);
			m_handle = libvlc_media_player_new_from_media(hMedia);
			if (m_handle == IntPtr.Zero) {
				throw new VlcException(VlcLib.GetLastError());
			}
		}
		
		public void Play() {
			if (IntPtr.Zero != m_handle) {
				if (libvlc_media_player_play(m_handle) < 0) {
					throw new VlcException(VlcLib.GetLastError());
				}
			}
		}

		public void Stop() {
			if (IntPtr.Zero != m_handle) {
				libvlc_media_player_set_media(m_handle, IntPtr.Zero);
				libvlc_media_player_set_hwnd(m_handle, IntPtr.Zero);
				libvlc_media_player_stop(m_handle);
			}
		}

		public void SetHwnd(IntPtr hWnd) {
			libvlc_media_player_set_hwnd(m_handle, hWnd);
		}

		protected Size GetSize() {
			UInt32 width = 0;
			UInt32 height = 0;
			if (libvlc_video_get_size(m_handle, 0, out width, out height) != 0) {
				throw new Exception(VlcLib.GetLastError());
			}
			return new Size {
				Width = (int)width,
				Height = (int)height
			};
		}
				
		public Media Media {
			get {
				if (null == _media) {
					_media = new Media(libvlc_media_player_get_media(m_handle));
				}
				return _media;
			}
		}

		private EventManager _EventManager = null;
		public EventManager EventManager {
			get {
				if (null == _EventManager) {
					_EventManager = new EventManager(libvlc_media_player_event_manager(m_handle));
				}
				return _EventManager;
			}
		}

		public void SetMouseInput(bool fMI) {
			libvlc_video_set_mouse_input(m_handle, (UInt32)(fMI ? 1 : 0));
		}
				
		public void SetCallbacks(LockBuffer lb, UnlockBuffer ub, DisplayBuffer db) {
			libvlc_video_set_callbacks(m_handle, lb, ub, db, IntPtr.Zero);
		}

		public void SetFormat(string format, UInt32 width, UInt32 height, UInt32 pitch) {
			libvlc_video_set_format(m_handle, format, width, height, pitch);
		}
	}
}
