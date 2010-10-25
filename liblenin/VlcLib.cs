using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using onvifdm.utils;

namespace liblenin {
	public class VlcLib: IDisposable
	{
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr libvlc_new(Int32 argc,	[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]string[] argv);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
		private static extern void libvlc_release(IntPtr handle);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr libvlc_media_new_path(IntPtr hInstance, string path);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr libvlc_media_new_location(IntPtr hInstance, string psz_mrl);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void libvlc_media_release(IntPtr hMedia);
		[DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern string libvlc_errmsg();
		
		private static VlcLib s_instance = null;
		private IntPtr m_handle = IntPtr.Zero;
		
		private VlcLib() {
			m_handle = libvlc_new(0, null);
		}

		~VlcLib() {
			//DebugHelper.Error("finalizer");
			Dispose(false);
		}

		public static VlcLib Instance {
			get {
				if (s_instance == null) {
					s_instance = new VlcLib();
				}
				return s_instance;
			}
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				//clenup manged resources here
			}

			if (m_handle != IntPtr.Zero) {
				libvlc_release(m_handle);
				m_handle = IntPtr.Zero;
			}

		}
		
		public VlcPlayer CreateMediaPlayer(string path) {
			VlcPlayer vlcPlayer = null;
			IntPtr hMedia = IntPtr.Zero;
			try {
				//hMedia = libvlc_media_new_path(m_handle, path);
				hMedia = libvlc_media_new_location(m_handle, path);
				if (hMedia == IntPtr.Zero) {
					throw new VlcException(GetLastError());
				}
				vlcPlayer = new VlcPlayer(hMedia);
			} finally {
				if (hMedia != IntPtr.Zero) {
					libvlc_media_release(hMedia);
					hMedia = IntPtr.Zero;
				}
			}
			return vlcPlayer;
		}
		
		public static string GetLastError() {
			return libvlc_errmsg();
		}
		
	};
}
