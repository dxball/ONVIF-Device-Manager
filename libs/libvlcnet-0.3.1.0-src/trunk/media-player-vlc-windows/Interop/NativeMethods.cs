// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.

#region Usings

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

#endregion
namespace DZ.MediaPlayer.Vlc.Windows.Interop {
	internal static class NativeMethods {
		#region Added

		#region CmdShow enum

		public enum CmdShow {
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_FORCEMINIMIZE = 11,
			SW_MAX = 11
		}

		#endregion

		#region WindowClassLongIndex enum

		public enum WindowClassLongIndex {
			GCL_MENUNAME = (-8),
			GCL_HBRBACKGROUND = (-10),
			GCL_HCURSOR = (-12),
			GCL_HICON = (-14),
			GCL_HMODULE = (-16),
			GCL_CBWNDEXTRA = (-18),
			GCL_CBCLSEXTRA = (-20),
			GCL_WNDPROC = (-24),
			GCL_STYLE = (-26),
			GCW_ATOM = (-32)
		}

		#endregion

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		[SuppressUnmanagedCodeSecurity]
		public static extern Int32 SetClassLong(IntPtr hwnd, WindowClassLongIndex nIndex, Int32 dwNewLong);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateSolidBrush(UInt32 colorref);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern bool InvalidateRect(IntPtr hwnd, IntPtr lprcUpdate, bool bErase);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern bool ShowWindow(IntPtr hwnd, CmdShow nCmdShow);

		#endregion

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[SecurityCritical, SuppressUnmanagedCodeSecurity,
		 DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		[SecurityCritical, SuppressUnmanagedCodeSecurity,
		 DllImport("user32.dll", EntryPoint = "PostThreadMessage", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int PostThreadMessage(int id, int msg, IntPtr wparam, IntPtr lparam);

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GlobalAlloc(int flags, int size);

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GlobalLock(IntPtr handle);

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GlobalUnlock(IntPtr handle);

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GlobalFree(IntPtr handle);

		[SuppressUnmanagedCodeSecurity,
		 DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		internal static extern IntPtr GetStockObject(StockObjects fnObject);

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)
		]
		internal static extern int GetMessagePos();

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)
		]
		internal static extern int GetMessageTime();

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CreateDC(string szdriver, string szdevice, string szoutput, IntPtr devmode);

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DeleteDC(IntPtr hdc);

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)
		]
		internal static extern void SetWindowLong(IntPtr hWnd, GWL GetWindowLongParam, int nValue);

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)
		]
		internal static extern int SetWindowPos(IntPtr hWnd, HWND pos, int X, int Y, int cx, int cy, SWP uFlags);

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetWindowLong(IntPtr hWnd, GWL nIndex);

		[SecurityCritical, SuppressUnmanagedCodeSecurity,
		 DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TranslateMessage([In, Out] ref WindowMessage msg);

		[SecurityCritical]
		internal static IntPtr CreateWindowEx(WindowsStyleEx dwExStyle, string lpszClassName, string lpszWindowName,
		                                      WindowsStyle style, int x, int y, int width, int height,
		                                      IntPtr hWndParent, IntPtr hMenu, IntPtr hInst,
		                                      [MarshalAs(UnmanagedType.AsAny)] object pvParam) {
			IntPtr ptr = IntCreateWindowEx(dwExStyle, lpszClassName, lpszWindowName, style, x, y, width, height, hWndParent,
			                               hMenu, hInst, pvParam);
			if (ptr == IntPtr.Zero) {
				throw new Win32Exception();
			}
			return ptr;
		}

		[SecurityCritical]
		internal static int MsgWaitForMultipleObjectsEx(int nCount, IntPtr[] pHandles, int dwMilliseconds,
		                                                MsgWaitEventFlags dwWakeMask, MsgWaitFlags dwFlags) {
			int num = IntMsgWaitForMultipleObjectsEx(nCount, pHandles, dwMilliseconds, dwWakeMask, dwFlags);
			if (num == -1) {
				throw new Win32Exception();
			}
			return num;
		}

		[SecurityCritical, SuppressUnmanagedCodeSecurity,
		 DllImport("user32.dll", EntryPoint = "MsgWaitForMultipleObjectsEx", CharSet = CharSet.Auto, SetLastError = true,
		 	ExactSpelling = true)]
		private static extern int IntMsgWaitForMultipleObjectsEx(int nCount, IntPtr[] pHandles, int dwMilliseconds,
		                                                         MsgWaitEventFlags dwWakeMask, MsgWaitFlags dwFlags);

		[SuppressUnmanagedCodeSecurity,
		 DllImport("user32.dll", EntryPoint = "GetMessageW", CharSet = CharSet.Unicode, SetLastError = true,
		 	ExactSpelling = true)]
		public static extern int GetMessageW([In, Out] ref WindowMessage msg, IntPtr hWnd, int uMsgFilterMin,
		                                     int uMsgFilterMax);

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PeekMessage([In, Out] ref WindowMessage msg, IntPtr hwnd, int msgMin, int msgMax, int remove);

		[SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DispatchMessage([In] ref WindowMessage msg);

		[SecurityCritical, SuppressUnmanagedCodeSecurity,
		 DllImport("user32.dll", EntryPoint = "RegisterClassEx", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern ushort IntRegisterClassEx(WNDCLASSEX_D wc_d);

		[SecurityCritical]
		internal static ushort RegisterClassEx(WNDCLASSEX_D wc_d) {
			ushort num = IntRegisterClassEx(wc_d);
			if (num == 0) {
				throw new Win32Exception();
			}
			return num;
		}

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr IntCreateWindowEx(WindowsStyleEx dwExStyle, string lpszClassName, string lpszWindowName,
		                                                WindowsStyle style, int x, int y, int width, int height,
		                                                IntPtr hWndParent, IntPtr hMenu, IntPtr hInst,
		                                                [MarshalAs(UnmanagedType.AsAny)] object pvParam);

		internal static void SetWindowStyle(IntPtr hwnd, int style) {
			SetWindowLong(hwnd, GWL.STYLE, style);
			SetWindowLong(hwnd, GWL.STYLE, style);
			SetWindowPos(hwnd, HWND.TOPMOST, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOSIZE);
		}


		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[SuppressUnmanagedCodeSecurity, SecurityCritical, DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DestroyWindow(IntPtr hWnd);

		[DllImport("gdiplus.dll", ExactSpelling = true)]
		internal static extern int GdipCreateBitmapFromGdiDib(IntPtr bminfo, IntPtr pixdat, ref IntPtr image);

		[DllImport("gdiplus.dll", ExactSpelling = true)]
		internal static extern int GdiplusStartup(ref ulong handle, ref GdiplusStartupInput startup, IntPtr zero);

		[DllImport("gdiplus.dll", ExactSpelling = true)]
		internal static extern int GdiplusShutdown(ref ulong handle);

		[DllImport("gdi32.dll")]
		internal static extern uint RealizePalette(IntPtr hdc);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("user32.dll", EntryPoint = "ReleaseDC", CharSet = CharSet.Auto, ExactSpelling = true)]
		internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[SuppressUnmanagedCodeSecurity, SecurityCritical,
		 DllImport("user32.dll", EntryPoint = "GetDC", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		internal static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("gdi32.dll")] // 0x04L
		internal static extern IntPtr CreateDIBitmap(IntPtr hdc, IntPtr header, uint fdwInit, IntPtr bits, IntPtr headerInfo,
		                                             uint fuUsage);

		internal static IntPtr CreateDIBPalette(IntPtr bitmapInfo) {
			BITMAPINFOHEADER bmi =
				(BITMAPINFOHEADER) Marshal.PtrToStructure(bitmapInfo, typeof (BITMAPINFOHEADER));
			//
			if (bmi.biSize != Marshal.SizeOf(typeof (BITMAPINFOHEADER))) {
				return (IntPtr.Zero);
			} else {
				IntPtr ptrRgb = new IntPtr((long) bitmapInfo + bmi.biSize);
				int colorsCount = DIBNumColors(bitmapInfo);
				if (colorsCount != 0) {
					int entrySize = Marshal.SizeOf(typeof (PALETTEENTRY));
					IntPtr palette = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (LOGPALETTE)) +
					                                      colorsCount*entrySize);
					//
					try {
						Marshal.WriteInt16(palette, 0, 0x300);
						Marshal.WriteInt16(palette, 2, (short) colorsCount);
						//
						for (int i = 0; i < colorsCount; i++) {
							Marshal.WriteByte(palette, 4 + i*entrySize, Marshal.ReadByte(ptrRgb, i*4 + 2));
							Marshal.WriteByte(palette, 5 + i*entrySize, Marshal.ReadByte(ptrRgb, i*4 + 1));
							Marshal.WriteByte(palette, 6 + i*entrySize, Marshal.ReadByte(ptrRgb, i*4));
							Marshal.WriteByte(palette, 7 + i*entrySize, 0);
						}
						//
						IntPtr resulting = CreatePalette(palette);
						return (resulting);
					} finally {
						Marshal.FreeHGlobal(palette);
					}
					//
				} else {
					if (bmi.biBitCount == 24) {
					}
				}
			}
			return (IntPtr.Zero);
		}

		internal static int DIBNumColors(IntPtr ptr) {
			int bitCount = 0;
			BITMAPINFOHEADER header = (BITMAPINFOHEADER) Marshal.PtrToStructure(ptr, typeof (BITMAPINFOHEADER));
			if (header.biSize != Marshal.SizeOf(typeof (BITMAPCOREHEADER))) {
				if (header.biClrUsed != 0) {
					return (header.biClrUsed);
				}
				bitCount = header.biBitCount;
			} else {
				bitCount = header.biBitCount;
			}
			if (bitCount <= 8) {
				return (1 << bitCount);
			} else {
				return (0);
			}
		}

		[DllImport("gdi32.dll")]
		internal static extern IntPtr CreatePalette([In] ref LOGPALETTE lplgpl);

		[DllImport("gdi32.dll")]
		internal static extern IntPtr CreatePalette(IntPtr lplgpl);

		[DllImport("gdi32.dll")]
		internal static extern int DeleteObject(IntPtr handle);

		#region Nested type: BITMAPCOREHEADER

		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct BITMAPCOREHEADER {
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
		}

		#endregion

		#region Nested type: BITMAPINFOHEADER

		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct BITMAPINFOHEADER {
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int biClrImportant;
		}

		#endregion

		#region Nested type: GdiplusStartupInput

		[StructLayout(LayoutKind.Sequential)]
		internal struct GdiplusStartupInput {
			public UInt32 GdiplusVersion;
			public IntPtr DebugEventCallback;

			[MarshalAs(UnmanagedType.Bool)]
			public bool SuppressBackgroundThread;

			[MarshalAs(UnmanagedType.Bool)]
			public bool SuppressExternalCodecs;
		} ;

		#endregion

		#region Nested type: GW

		internal enum GW {
			HWNDFIRST,
			HWNDLAST,
			HWNDNEXT,
			HWNDPREV,
			OWNER,
			CHILD
		}

		#endregion

		#region Nested type: GWL

		internal enum GWL {
			EXSTYLE = -20,
			HINSTANCE = -6,
			HWNDPARENT = -8,
			ID = -12,
			STYLE = -16,
			USERDATA = -21,
			WNDPROC = -4
		}

		#endregion

		#region Nested type: HWND

		internal enum HWND {
			BOTTOM = 1,
			MESSAGE = -3,
			NOTOPMOST = -2,
			TOP = 0,
			TOPMOST = -1
		}

		#endregion

		#region Nested type: KeyStateFlags

		[Flags]
		internal enum KeyStateFlags {
			AnyAlt = 0x10000000,
			AnyCtrl = 0x40000000,
			AnyShift = 0x20000000,
			AsyncDown = 2,
			Capital = 0x8000000,
			Dead = 0x20000,
			Down = 0x80,
			Language1 = 0x8000,
			LeftAlt = 0x1000000,
			LeftCtrl = 0x4000000,
			LeftShift = 0x2000000,
			LeftWin = 0x800000,
			NoCharacter = 0x10000,
			NumLock = 0x1000,
			PrevDown = 0x40,
			RightAlt = 0x100000,
			RightCtrl = 0x400000,
			RightShift = 0x200000,
			RightWin = 0x80000,
			Toggled = 1
		}

		#endregion

		#region Nested type: LOGPALETTE

		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct LOGPALETTE {
			public ushort palVersion;
			public ushort numEntries;
		}

		#endregion

		#region Nested type: MsgWaitEventFlags

		[Flags]
		internal enum MsgWaitEventFlags {
			QS_ALLEVENTS = 0xbf,
			QS_ALLINPUT = 0xff,
			QS_ALLPOSTMESSAGE = 0x100,
			QS_EVENT = 0x2000,
			QS_HOTKEY = 0x80,
			QS_INPUT = 7,
			QS_KEY = 1,
			QS_MOUSE = 6,
			QS_MOUSEBUTTON = 4,
			QS_MOUSEMOVE = 2,
			QS_PAINT = 0x20,
			QS_POSTMESSAGE = 8,
			QS_SENDMESSAGE = 0x40,
			QS_TIMER = 0x10,
		}

		#endregion

		#region Nested type: MsgWaitFlags

		[Flags]
		internal enum MsgWaitFlags {
			WaitAny = 0,
			WaitAll = 1,
			Alertable = 2,
			InputAvailable = 4,
		}

		#endregion

		#region Nested type: PALETTEENTRY

		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct PALETTEENTRY {
			public Byte peRed;
			public Byte peGreen;
			public Byte peBlue;
			public Byte peFlags;
		}

		#endregion

		#region Nested type: POINT

		[StructLayout(LayoutKind.Sequential)]
		internal class POINT {
			public int x;
			public int y;

			public POINT() {
			}

			public POINT(int x, int y) {
				this.x = x;
				this.y = y;
			}
		}

		#endregion

		#region Nested type: StockObjects

		internal enum StockObjects {
			WHITE_BRUSH = 0,
			LTGRAY_BRUSH = 1,
			GRAY_BRUSH = 2,
			DKGRAY_BRUSH = 3,
			BLACK_BRUSH = 4,
			NULL_BRUSH = 5,
			HOLLOW_BRUSH = NULL_BRUSH,
			WHITE_PEN = 6,
			BLACK_PEN = 7,
			NULL_PEN = 8,
			OEM_FIXED_FONT = 10,
			ANSI_FIXED_FONT = 11,
			ANSI_VAR_FONT = 12,
			SYSTEM_FONT = 13,
			DEVICE_DEFAULT_FONT = 14,
			DEFAULT_PALETTE = 15,
			SYSTEM_FIXED_FONT = 16,
			DEFAULT_GUI_FONT = 17,
			DC_BRUSH = 18,
			DC_PEN = 19,
		}

		#endregion

		#region Nested type: SW

		internal enum SW {
			HIDE = 0,
			MAXIMIZE = 12,
			MINIMIZE = 6,
			RESTORE = 13,
			SHOW = 5,
			SHOWMAXIMIZED = 11,
			SHOWNA = 8,
			SHOWNOACTIVATE = 4,
			SHOWNORMAL = 1
		}

		#endregion

		#region Nested type: SWP

		[Flags]
		internal enum SWP {
			ASYNCWINDOWPOS = 0x4000,
			DEFERERASE = 0x2000,
			DRAWFRAME = 0x20,
			FRAMECHANGED = 0x20,
			HIDEWINDOW = 0x80,
			NOACTIVATE = 0x10,
			NOCOPYBITS = 0x100,
			NOMOVE = 2,
			NOOWNERZORDER = 0x200,
			NOREDRAW = 8,
			NOREPOSITION = 0x200,
			NOSENDCHANGING = 0x400,
			NOSIZE = 1,
			NOZORDER = 4,
			SHOWWINDOW = 0x40
		}

		#endregion

		#region Nested type: Window

		internal class Window {
			private readonly bool destroyWindow;
			private IntPtr oldWindowProc = IntPtr.Zero;
			//
			private IntPtr thisWindowProcPtr = IntPtr.Zero;
			private WndProcDelegate windowProc;
			private IntPtr wndHandle = IntPtr.Zero;

			public Window(IntPtr subclassHandle) {
				PreInitialize();
				//
				wndHandle = subclassHandle;
				Subclass();
			}

			public Window(IntPtr pParent, WindowsStyle pStyle, WindowsStyleEx pExStyle,
			              string pClassName, string pWindowName,
			              int pLeft, int pTop, int pHeight, int pWidth) {
				PreInitialize();

				IntPtr xHandle =
					CreateWindowEx(pExStyle, pClassName, pWindowName, pStyle, pLeft, pTop, pWidth, pHeight,
					               pParent, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
				if (xHandle == IntPtr.Zero) {
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				wndHandle = xHandle;
				destroyWindow = true;
				Subclass();
			}

			public Window(WindowsStyle pStyle, WindowsStyleEx pExStyle, string pClassName,
			              string pWindowName, int pLeft, int pTop,
			              int pHeight, int pWidth) {
				PreInitialize();

				IntPtr xHandle =
					CreateWindowEx(pExStyle, pClassName, pWindowName, pStyle, pLeft, pTop, pWidth, pHeight,
					               IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
				if (xHandle == IntPtr.Zero) {
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				wndHandle = xHandle;
				destroyWindow = true;
				Subclass();
			}

			public IntPtr Handle {
				get {
					return (wndHandle);
				}
			}

			public void Subclass(IntPtr subclassHandle) {
				ReleaseHandle();
				PreInitialize();
				//
				wndHandle = subclassHandle;
				Subclass();
			}

			private void PreInitialize() {
				windowProc = WndProc;
				thisWindowProcPtr = Marshal.GetFunctionPointerForDelegate(windowProc);
			}


			private void Subclass() {
				if (wndHandle != IntPtr.Zero) {
					oldWindowProc = GetWindowLong(wndHandle, GWL.WNDPROC);
					SetWindowLong(wndHandle, GWL.WNDPROC, (int) thisWindowProcPtr);
				}
			}

			private void UnSubclass() {
				if (thisWindowProcPtr == GetWindowLong(wndHandle, GWL.WNDPROC)) {
					SetWindowLong(wndHandle, GWL.WNDPROC, (int) oldWindowProc);
				}
			}

			public void ReleaseHandle() {
				if (wndHandle != IntPtr.Zero) {
					UnSubclass();
					if (destroyWindow) {
						DestroyWindow(wndHandle);
					}
					wndHandle = IntPtr.Zero;
					oldWindowProc = IntPtr.Zero;
				}
			}

			protected virtual void OnWindowDestroyed() {
			}

			private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) {
				WindowMessage xMessage = new WindowMessage(hWnd, (int) msg, wParam, lParam);
				//
				if (msg == (uint) WindowsMessages.WM_CLOSE) {
					PostMessage(hWnd, (int) WindowsMessages.WM_DESTROY, IntPtr.Zero, IntPtr.Zero);
				}
				if (msg == (uint) WindowsMessages.WM_DESTROY ||
				    msg == (uint) WindowsMessages.WM_NCDESTROY) {
					// HANDLE DESTROY
					//
					try {
						CallOriginalWindowProc(ref xMessage);
					} finally {
						//
						ReleaseHandle();
						OnWindowDestroyed();
					}
					return (xMessage.result);
				} else {
					WindowProc(ref xMessage);
				}
				return (xMessage.result);
			}

			protected virtual void WindowProc(ref WindowMessage pMessage) {
				CallOriginalWindowProc(ref pMessage);
			}

			private void CallOriginalWindowProc(ref WindowMessage pMessage) {
				if (oldWindowProc == IntPtr.Zero) {
					pMessage.result = DefWindowProc(pMessage.windowHandle, pMessage.messageCode, pMessage.wParam,
					                                pMessage.lParam);
				} else {
					pMessage.result =
						CallWindowProc(oldWindowProc, pMessage.windowHandle, pMessage.messageCode, pMessage.wParam,
						               pMessage.lParam);
				}
			}
		}

		#endregion

		#region Nested type: WindowsMessages

		internal enum WindowsMessages {
			WM_ACTIVATE = 6,
			WM_ACTIVATEAPP = 0x1c,
			WM_AFXFIRST = 0x360,
			WM_AFXLAST = 0x37f,
			WM_APP = 0x8000,
			WM_APPCOMMAND = 0x319,
			WM_ASKCBFORMATNAME = 780,
			WM_CANCELJOURNAL = 0x4b,
			WM_CANCELMODE = 0x1f,
			WM_CAPTURECHANGED = 0x215,
			WM_CHANGECBCHAIN = 0x30d,
			WM_CHANGEUISTATE = 0x127,
			WM_CHAR = 0x102,
			WM_CHARTOITEM = 0x2f,
			WM_CHILDACTIVATE = 0x22,
			WM_CHOOSEFONT_GETLOGFONT = 0x401,
			WM_CLEAR = 0x303,
			WM_CLOSE = 0x10,
			WM_COMMAND = 0x111,
			WM_COMMNOTIFY = 0x44,
			WM_COMPACTING = 0x41,
			WM_COMPAREITEM = 0x39,
			WM_CONTEXTMENU = 0x7b,
			WM_COPY = 0x301,
			WM_COPYDATA = 0x4a,
			WM_CREATE = 1,
			WM_CTLCOLOR = 0x19,
			WM_CTLCOLORBTN = 0x135,
			WM_CTLCOLORDLG = 310,
			WM_CTLCOLOREDIT = 0x133,
			WM_CTLCOLORLISTBOX = 0x134,
			WM_CTLCOLORMSGBOX = 0x132,
			WM_CTLCOLORSCROLLBAR = 0x137,
			WM_CTLCOLORSTATIC = 0x138,
			WM_CUT = 0x300,
			WM_DEADCHAR = 0x103,
			WM_DELETEITEM = 0x2d,
			WM_DESTROY = 2,
			WM_DESTROYCLIPBOARD = 0x307,
			WM_DEVICECHANGE = 0x219,
			WM_DEVMODECHANGE = 0x1b,
			WM_DISPLAYCHANGE = 0x7e,
			WM_DRAWCLIPBOARD = 0x308,
			WM_DRAWITEM = 0x2b,
			WM_DROPFILES = 0x233,
			WM_DWMCOMPOSITIONCHANGED = 0x31e,
			WM_ENABLE = 10,
			WM_ENDSESSION = 0x16,
			WM_ENTERIDLE = 0x121,
			WM_ENTERMENULOOP = 0x211,
			WM_ENTERSIZEMOVE = 0x231,
			WM_ERASEBKGND = 20,
			WM_EXITMENULOOP = 530,
			WM_EXITSIZEMOVE = 0x232,
			WM_FLICK = 0x2cb,
			WM_FONTCHANGE = 0x1d,
			WM_GETDLGCODE = 0x87,
			WM_GETFONT = 0x31,
			WM_GETHOTKEY = 0x33,
			WM_GETICON = 0x7f,
			WM_GETMINMAXINFO = 0x24,
			WM_GETOBJECT = 0x3d,
			WM_GETTEXT = 13,
			WM_GETTEXTLENGTH = 14,
			WM_HANDHELDFIRST = 0x358,
			WM_HANDHELDLAST = 0x35f,
			WM_HELP = 0x53,
			WM_HOTKEY = 0x312,
			WM_HSCROLL = 0x114,
			WM_HSCROLLCLIPBOARD = 0x30e,
			WM_ICONERASEBKGND = 0x27,
			WM_IME_CHAR = 0x286,
			WM_IME_COMPOSITION = 0x10f,
			WM_IME_COMPOSITIONFULL = 0x284,
			WM_IME_CONTROL = 0x283,
			WM_IME_ENDCOMPOSITION = 270,
			WM_IME_KEYDOWN = 0x290,
			WM_IME_KEYLAST = 0x10f,
			WM_IME_KEYUP = 0x291,
			WM_IME_NOTIFY = 0x282,
			WM_IME_REQUEST = 0x288,
			WM_IME_SELECT = 0x285,
			WM_IME_SETCONTEXT = 0x281,
			WM_IME_STARTCOMPOSITION = 0x10d,
			WM_INITDIALOG = 0x110,
			WM_INITMENU = 0x116,
			WM_INITMENUPOPUP = 0x117,
			WM_INPUT = 0xff,
			WM_INPUTLANGCHANGE = 0x51,
			WM_INPUTLANGCHANGEREQUEST = 80,
			WM_KEYDOWN = 0x100,
			WM_KEYFIRST = 0x100,
			WM_KEYLAST = 0x108,
			WM_KEYUP = 0x101,
			WM_KILLFOCUS = 8,
			WM_LBUTTONDBLCLK = 0x203,
			WM_LBUTTONDOWN = 0x201,
			WM_LBUTTONUP = 0x202,
			WM_MBUTTONDBLCLK = 0x209,
			WM_MBUTTONDOWN = 0x207,
			WM_MBUTTONUP = 520,
			WM_MDIACTIVATE = 0x222,
			WM_MDICASCADE = 0x227,
			WM_MDICREATE = 0x220,
			WM_MDIDESTROY = 0x221,
			WM_MDIGETACTIVE = 0x229,
			WM_MDIICONARRANGE = 0x228,
			WM_MDIMAXIMIZE = 0x225,
			WM_MDINEXT = 0x224,
			WM_MDIREFRESHMENU = 0x234,
			WM_MDIRESTORE = 0x223,
			WM_MDISETMENU = 560,
			WM_MDITILE = 550,
			WM_MEASUREITEM = 0x2c,
			WM_MENUCHAR = 0x120,
			WM_MENUSELECT = 0x11f,
			WM_MOUSEACTIVATE = 0x21,
			WM_MOUSEFIRST = 0x200,
			WM_MOUSEHOVER = 0x2a1,
			WM_MOUSELAST = 0x20a,
			WM_MOUSELEAVE = 0x2a3,
			WM_MOUSEMOVE = 0x200,
			WM_MOUSEQUERY = 0x9b,
			WM_MOUSEWHEEL = 0x20a,
			WM_MOVE = 3,
			WM_MOVING = 0x216,
			WM_NCACTIVATE = 0x86,
			WM_NCCALCSIZE = 0x83,
			WM_NCCREATE = 0x81,
			WM_NCDESTROY = 130,
			WM_NCHITTEST = 0x84,
			WM_NCLBUTTONDBLCLK = 0xa3,
			WM_NCLBUTTONDOWN = 0xa1,
			WM_NCLBUTTONUP = 0xa2,
			WM_NCMBUTTONDBLCLK = 0xa9,
			WM_NCMBUTTONDOWN = 0xa7,
			WM_NCMBUTTONUP = 0xa8,
			WM_NCMOUSELEAVE = 0x2a2,
			WM_NCMOUSEMOVE = 160,
			WM_NCPAINT = 0x85,
			WM_NCRBUTTONDBLCLK = 0xa6,
			WM_NCRBUTTONDOWN = 0xa4,
			WM_NCRBUTTONUP = 0xa5,
			WM_NCXBUTTONDBLCLK = 0xad,
			WM_NCXBUTTONDOWN = 0xab,
			WM_NCXBUTTONUP = 0xac,
			WM_NEXTDLGCTL = 40,
			WM_NEXTMENU = 0x213,
			WM_NOTIFY = 0x4e,
			WM_NOTIFYFORMAT = 0x55,
			WM_NULL = 0,
			WM_PAINT = 15,
			WM_PAINTCLIPBOARD = 0x309,
			WM_PAINTICON = 0x26,
			WM_PALETTECHANGED = 0x311,
			WM_PALETTEISCHANGING = 0x310,
			WM_PARENTNOTIFY = 0x210,
			WM_PASTE = 770,
			WM_PENWINFIRST = 0x380,
			WM_PENWINLAST = 0x38f,
			WM_POWER = 0x48,
			WM_POWERBROADCAST = 0x218,
			WM_PRINT = 0x317,
			WM_PRINTCLIENT = 0x318,
			WM_QUERYDRAGICON = 0x37,
			WM_QUERYENDSESSION = 0x11,
			WM_QUERYNEWPALETTE = 0x30f,
			WM_QUERYOPEN = 0x13,
			WM_QUERYSYSTEMGESTURESTATUS = 0x2cc,
			WM_QUERYUISTATE = 0x129,
			WM_QUEUESYNC = 0x23,
			WM_QUIT = 0x12,
			WM_RBUTTONDBLCLK = 0x206,
			WM_RBUTTONDOWN = 0x204,
			WM_RBUTTONUP = 0x205,
			WM_REFLECT = 0x2000,
			WM_RENDERALLFORMATS = 0x306,
			WM_RENDERFORMAT = 0x305,
			WM_SETCURSOR = 0x20,
			WM_SETFOCUS = 7,
			WM_SETFONT = 0x30,
			WM_SETHOTKEY = 50,
			WM_SETICON = 0x80,
			WM_SETREDRAW = 11,
			WM_SETTEXT = 12,
			WM_SETTINGCHANGE = 0x1a,
			WM_SHOWWINDOW = 0x18,
			WM_SIZE = 5,
			WM_SIZECLIPBOARD = 0x30b,
			WM_SIZING = 0x214,
			WM_SPOOLERSTATUS = 0x2a,
			WM_STYLECHANGED = 0x7d,
			WM_STYLECHANGING = 0x7c,
			WM_SYSCHAR = 0x106,
			WM_SYSCOLORCHANGE = 0x15,
			WM_SYSCOMMAND = 0x112,
			WM_SYSDEADCHAR = 0x107,
			WM_SYSKEYDOWN = 260,
			WM_SYSKEYUP = 0x105,
			WM_TABLET_ADDED = 0x2c8,
			WM_TABLET_REMOVED = 0x2c9,
			WM_TCARD = 0x52,
			WM_THEMECHANGED = 0x31a,
			WM_TIMECHANGE = 30,
			WM_TIMER = 0x113,
			WM_UNDO = 0x304,
			WM_UNINITMENUPOPUP = 0x125,
			WM_UPDATEUISTATE = 0x128,
			WM_USER = 0x400,
			WM_USERCHANGED = 0x54,
			WM_VKEYTOITEM = 0x2e,
			WM_VSCROLL = 0x115,
			WM_VSCROLLCLIPBOARD = 0x30a,
			WM_WINDOWPOSCHANGED = 0x47,
			WM_WINDOWPOSCHANGING = 70,
			WM_WININICHANGE = 0x1a,
			WM_WTSSESSION_CHANGE = 0x2b1,
			WM_XBUTTONDBLCLK = 0x20d,
			WM_XBUTTONDOWN = 0x20b,
			WM_XBUTTONUP = 0x20c,
		}

		#endregion

		#region Nested type: WindowsStyle

		[Flags]
		internal enum WindowsStyle {
			WS_BORDER = 0x800000,
			WS_CAPTION = 0xc00000,
			WS_CHILD = 0x40000000,
			WS_CLIPCHILDREN = 0x2000000,
			WS_CLIPSIBLINGS = 0x4000000,
			WS_DISABLED = 0x8000000,
			WS_DLGFRAME = 0x400000,
			WS_GROUP = 0x20000,
			WS_HSCROLL = 0x100000,
			WS_MAXIMIZEBOX = 0x20000,
			WS_MINIMIZEBOX = 0x10000,
			WS_OVERLAPPED = 0xc00000,
			WS_POPUP = -2147483648,
			WS_SYSMENU = 0x80000,
			WS_TABSTOP = 0x10000,
			WS_THICKFRAME = 0x40000,
			WS_VISIBLE = 0x10000000,
			WS_VSCROLL = 0x200000
		}

		#endregion

		#region Nested type: WindowsStyleEx

		[Flags]
		internal enum WindowsStyleEx {
			WS_EX_ABOVESTARTUP = 0x20000000,
			WS_EX_CAPTIONOKBUTTON = -2147483648,
			WS_EX_CLIENTEDGE = 0x200,
			WS_EX_CONTEXTHELP = 0x400,
			WS_EX_DLGMODALFRAME = 1,
			WS_EX_INK = 0x10000000,
			WS_EX_NOACTIVATE = 0x8000000,
			WS_EX_NOANIMATION = 0x4000000,
			WS_EX_NODRAG = 0x40000000,
			WS_EX_NONE = 0,
			WS_EX_OVERLAPPEDWINDOW = 0x300,
			WS_EX_STATICEDGE = 0x20000,
			WS_EX_TOOLWINDOW = 0x80,
			WS_EX_TOPMOST = 8,
			WS_EX_WINDOWEDGE = 0x100
		}

		#endregion

		#region Nested type: WNDCLASSEX_D

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class WNDCLASSEX_D {
			public int cbSize;
			public int style;
			public IntPtr lpfnWndProc;
			public int cbClsExtra;
			public int cbWndExtra;
			public IntPtr hInstance;
			public IntPtr hIcon;
			public IntPtr hCursor;
			public IntPtr hbrBackground;
			public string lpszMenuName;
			public string lpszClassName;
			public IntPtr hIconSm;
		}

		#endregion

		#region Nested type: WndProcDelegate

		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		internal delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		#endregion
	}

    /// <summary>
    /// Win32 message struct.
    /// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WindowMessage {
        /// <summary>
        /// windowHandle.
        /// </summary>
		public IntPtr windowHandle;
        /// <summary>
        /// messageCode.
        /// </summary>
		public int messageCode;
        /// <summary>
        /// wParam.
        /// </summary>
		public IntPtr wParam;
        /// <summary>
        /// lParam.
        /// </summary>
		public IntPtr lParam;
        /// <summary>
        /// messageTime.
        /// </summary>
		public int messageTime;
        /// <summary>
        /// pointX.
        /// </summary>
		public int pointX;
        /// <summary>
        /// pointY.
        /// </summary>
		public int pointY;

        /// <summary>
        /// result.
        /// </summary>
		public IntPtr result;

		internal WindowMessage(IntPtr handle, int code, IntPtr wParam, IntPtr lParam, int messageTime, int posX, int posY) {
			windowHandle = handle;
			messageCode = code;
			this.wParam = wParam;
			this.lParam = lParam;
			this.messageTime = messageTime;
			pointX = posX;
			pointY = posY;
			result = IntPtr.Zero;
		}

		internal WindowMessage(IntPtr handle, int code, IntPtr wParam, IntPtr lParam, int messageTime) {
			windowHandle = handle;
			messageCode = code;
			this.wParam = wParam;
			this.lParam = lParam;
			this.messageTime = messageTime;
			int position = GetMessagePos();
			pointX = (short) position;
			pointY = (short) (position >> 16);
			result = IntPtr.Zero;
		}

		internal WindowMessage(IntPtr handle, int code, IntPtr wParam, IntPtr lParam) {
			windowHandle = handle;
			messageCode = code;
			this.wParam = wParam;
			this.lParam = lParam;
			messageTime = GetMessageTime();
			int position = GetMessagePos();
			pointX = (short) position;
			pointY = (short) (position >> 16);
			result = IntPtr.Zero;
		}

		[DllImport("user32.dll", ExactSpelling = true)]
		internal static extern int GetMessagePos();

		[DllImport("user32.dll", ExactSpelling = true)]
		internal static extern int GetMessageTime();
	}
}