using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.Drawing.Imaging;
using System.Concurrency;
using System.Disposables;

namespace odm.player {

	public static class NativePlayer {
		public const string odm_player_dll = "odm-player.dll";

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void onvifmp_error_handler(string aErrorMsg);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void onvifmp_log_handler(string aMsg, string aSource, LogType aType);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void onvifmp_meta_callback(IntPtr aBuffer, UInt32 aSize);
				
		public enum LogType : int {
			LOG_ERROR = 0,
			LOG_WARNING,
			LOG_INFORMATION,
		};

		public enum OnvifmpPixelFormat : int {
			ONVIFMP_PF_RGB32 = 0
		};

		[DllImport(odm_player_dll, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr onvifmp_initialize(onvifmp_error_handler aErrorHandler, onvifmp_log_handler aLogHandler);

		[DllImport(odm_player_dll, CallingConvention = CallingConvention.Cdecl)]
		public static extern void onvifmp_close(IntPtr aInstance);

		[DllImport(odm_player_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int onvifmp_start_parsing(IntPtr aInstance, string aUrl, int aWidth, int aHeight, int aStride, int pixFormat, string aMapName, onvifmp_meta_callback aCallback, int aSilentMode);

        [DllImport(odm_player_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int onvifmp_player_set_silent_mode(IntPtr aInstance, string aUrl, int aSilentMode);

		[DllImport(odm_player_dll, CallingConvention = CallingConvention.Cdecl)]
		public static extern int onvifmp_stop_parsing(IntPtr aInstance, string aUrl);

		[DllImport(odm_player_dll, CallingConvention = CallingConvention.Cdecl)]
		public static extern int onvifmp_start_record(IntPtr aInstance, string aUrl, string aFilePath);

		[DllImport(odm_player_dll, CallingConvention = CallingConvention.Cdecl)]
		public static extern int onvifmp_stop_record(IntPtr aInstance, string aUrl);

	}
}


