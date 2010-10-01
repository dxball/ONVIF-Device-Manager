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
using System.Reflection;
using System.Runtime.InteropServices;
using DZ.MediaPlayer.Vlc.Windows.Interop;

#endregion

namespace DZ.MediaPlayer.Vlc.Windows {
    /// <summary>
    /// Win32 implementation of <see cref="DoubleWindowBase"/>.
    /// </summary>
	public class WindowsOSDoubleWindow : DoubleWindowBase {
		private const string childWindowClassName = "libvlc.net child vlc window";
		private const string mainWindowClassName = "libvlc.net main vlc window";
		private static IntPtr moduleHandle;

		private readonly NativeMethods.Window childWindowFirst;
		private readonly NativeMethods.Window childWindowSecond;
		private readonly NativeMethods.Window mainWindow;

		private uint currentBackgroundColor;
		private int currentHeight;
		private int currentLeft;
		private int currentTop;
		private int currentWidth;
		private NativeMethods.WndProcDelegate dispWndProc;
		private NativeMethods.WndProcDelegate dispWndProc2;

		private bool firstChildIsActive = true;
		private bool isDisposed;

		private IntPtr mainWindowSolidBrush;
		private bool playerVisible;
		private bool visible;
		private bool windowClassRegistered;

        /// <summary>
        /// Default constructor.
        /// </summary>
		public WindowsOSDoubleWindow() {
			currentBackgroundColor = 0xaaaaaa;
			currentLeft = 0;
			currentTop = 0;
			currentWidth = 500;
			currentHeight = 400;
			visible = false;
			//
			registerWindowsClasses();
			//
			mainWindow = new NativeMethods.Window(
				NativeMethods.WindowsStyle.WS_POPUP | NativeMethods.WindowsStyle.WS_VISIBLE,
				NativeMethods.WindowsStyleEx.WS_EX_TOPMOST | NativeMethods.WindowsStyleEx.WS_EX_NONE,
				mainWindowClassName, "vlc main window",
				currentLeft, currentTop, currentWidth, currentHeight);

			childWindowFirst = new NativeMethods.Window(mainWindow.Handle,
			                                            NativeMethods.WindowsStyle.WS_CHILD |
			                                            NativeMethods.WindowsStyle.WS_CLIPSIBLINGS
			                                            | NativeMethods.WindowsStyle.WS_BORDER,
			                                            NativeMethods.WindowsStyleEx.WS_EX_TOPMOST,
			                                            childWindowClassName, "vlc child window 1",
			                                            0, 0, 200, 200);

			childWindowSecond = new NativeMethods.Window(mainWindow.Handle,
			                                             NativeMethods.WindowsStyle.WS_CHILD |
			                                             NativeMethods.WindowsStyle.WS_CLIPSIBLINGS
			                                             | NativeMethods.WindowsStyle.WS_BORDER,
			                                             NativeMethods.WindowsStyleEx.WS_EX_TOPMOST,
			                                             childWindowClassName, "vlc child window 2",
			                                             250, 0, 200, 200);
			//
			setWindowVisibility(mainWindow.Handle, visible = true);
		}

        /// <summary>
        /// Window width.
        /// </summary>
		public override int Width {
			get {
				return currentWidth;
			}
			set {
				currentWidth = value;
				applyWindowPosition();
			}
		}

        /// <summary>
        /// Window height.
        /// </summary>
		public override int Height {
			get {
				return currentHeight;
			}
			set {
				currentHeight = value;
				applyWindowPosition();
			}
		}

        /// <summary>
        /// Window left position.
        /// </summary>
		public override int Left {
			get {
				return currentLeft;
			}
			set {
				currentLeft = value;
				applyWindowPosition();
			}
		}

        /// <summary>
        /// Window top position.
        /// </summary>
		public override int Top {
			get {
				return currentTop;
			}
			set {
				currentTop = value;
				applyWindowPosition();
			}
		}

        /// <summary>
        /// Background color.
        /// </summary>
		public override uint BackgroundColor {
			get {
				return currentBackgroundColor;
			}
			set {
				currentBackgroundColor = value;
				//
				NativeMethods.DeleteObject(mainWindowSolidBrush);
				mainWindowSolidBrush = NativeMethods.CreateSolidBrush(currentBackgroundColor);
				//
				NativeMethods.SetClassLong(mainWindow.Handle,
				                           NativeMethods.WindowClassLongIndex.GCL_HBRBACKGROUND,
				                           mainWindowSolidBrush.ToInt32());
				NativeMethods.InvalidateRect(mainWindow.Handle, IntPtr.Zero, true);
			}
		}

        /// <summary>
        /// Background image file path.
        /// </summary>
		public override string BackgroundImageFilePath {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}

        /// <summary>
        /// Is window visible.
        /// </summary>
		public override bool Visible {
			get {
				return visible;
			}
			set {
				if (visible != value) {
					visible = value;
					//
					setWindowVisibility(mainWindow.Handle, visible);
				}
			}
		}

        /// <summary>
        /// Is active subwindow visible.
        /// </summary>
		protected override bool PlayerVisible {
			get {
				return (playerVisible);
			}
			set {
				if (playerVisible != value) {
					playerVisible = value;
					if (playerVisible) {
						setWindowVisibility(GetInactiveWindowHandle(), false);
						setWindowVisibility(GetActiveWindowHandle(), true);
					} else {
						setWindowVisibility(GetInactiveWindowHandle(), false);
						setWindowVisibility(GetActiveWindowHandle(), false);
					}
				}
			}
		}

		private static IntPtr GetInstanceHandle() {
			if (moduleHandle == IntPtr.Zero) {
				Module[] modules = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetModules(false);
				moduleHandle = Marshal.GetHINSTANCE(modules[0]);
			}
			return moduleHandle;
		}

        /// <summary>
        /// Handle of main window.
        /// </summary>
        /// <returns></returns>
		public IntPtr GetMainWindowHandle() {
			return (mainWindow.Handle);
		}

        /// <summary>
        /// Clean up resources.
        /// </summary>
        /// <param name="isDisposing"></param>
		protected override void Dispose(bool isDisposing) {
			if (!isDisposed) {
				isDisposed = true;
				//
				if (mainWindow != null) {
					mainWindow.ReleaseHandle();
				}
				if (childWindowFirst != null) {
					childWindowFirst.ReleaseHandle();
				}
				if (childWindowSecond != null) {
					childWindowSecond.ReleaseHandle();
				}
			}
		}

        /// <summary>
        /// Finalizer.
        /// </summary>
		~WindowsOSDoubleWindow() {
			Dispose(false);
		}

		private void registerWindowsClasses() {
			// do not register classes more than once
			if (!windowClassRegistered) {
				windowClassRegistered = true;
				//
				mainWindowSolidBrush = NativeMethods.CreateSolidBrush(currentBackgroundColor);
				//
				NativeMethods.WNDCLASSEX_D wndClass = new NativeMethods.WNDCLASSEX_D();
				wndClass.cbSize = Marshal.SizeOf(typeof (NativeMethods.WNDCLASSEX_D));
				wndClass.hbrBackground = mainWindowSolidBrush;
				wndClass.hCursor = IntPtr.Zero;
				wndClass.hIcon = IntPtr.Zero;
				wndClass.hIconSm = IntPtr.Zero;
				wndClass.hInstance = GetInstanceHandle();
				wndClass.lpszMenuName = null;
				wndClass.style = 0x08;
				wndClass.cbClsExtra = 0;
				wndClass.cbWndExtra = 0;
				// store delegate to not lose it for GC
				dispWndProc = dispatchingEmptyWindowProcedure;
				wndClass.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(dispWndProc);
				//
				wndClass.lpszClassName = mainWindowClassName;

				NativeMethods.RegisterClassEx(wndClass);
				//
				wndClass = new NativeMethods.WNDCLASSEX_D();
				wndClass.cbSize = Marshal.SizeOf(typeof (NativeMethods.WNDCLASSEX_D));
				wndClass.hbrBackground = NativeMethods.GetStockObject(NativeMethods.StockObjects.BLACK_BRUSH);
				wndClass.hCursor = IntPtr.Zero;
				wndClass.hIcon = IntPtr.Zero;
				wndClass.hIconSm = IntPtr.Zero;
				wndClass.hInstance = GetInstanceHandle();
				wndClass.lpszMenuName = null;
				wndClass.style = 0x08; // CS_DOUBLECLICKS
				wndClass.cbClsExtra = 0;
				wndClass.cbWndExtra = 0;
				// store delegate to not lose it for GC
				dispWndProc2 = dispatchingEmptyWindowProcedure;
				wndClass.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(dispWndProc2);
				//
				wndClass.lpszClassName = childWindowClassName;
				NativeMethods.RegisterClassEx(wndClass);
			}
		}

		//[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static IntPtr dispatchingEmptyWindowProcedure(IntPtr wnd, uint msg, IntPtr param, IntPtr lParam) {
			if (msg == (uint) NativeMethods.WindowsMessages.WM_NCCREATE) {
				// return 1 to create window, another value means error when creating
				return (new IntPtr(1));
			}
			if (msg == (uint) NativeMethods.WindowsMessages.WM_CLOSE) {
				// destroy window after closing
				NativeMethods.DestroyWindow(wnd);
			}
			// dispatch to default procedure
			return (NativeMethods.DefWindowProc(wnd, (int) msg, param, lParam));
		}

		private void applyWindowPosition() {
			NativeMethods.SetWindowPos(mainWindow.Handle, 0, currentLeft, currentTop, currentWidth,
			                           currentHeight, 0);
			NativeMethods.SetWindowPos(childWindowFirst.Handle, 0, 0, 0, currentWidth,
			                           currentHeight, 0);
			NativeMethods.SetWindowPos(childWindowSecond.Handle, 0, 0, 0, currentWidth,
			                           currentHeight, 0);
		}

		private static void setWindowVisibility(IntPtr hwnd, bool visibility) {
			NativeMethods.ShowWindow(hwnd, visibility ? NativeMethods.CmdShow.SW_SHOW : NativeMethods.CmdShow.SW_HIDE);
		}

        /// <summary>
        /// Gets handle of active subwindow.
        /// </summary>
        /// <returns></returns>
		protected override IntPtr GetActiveWindowHandle() {
			//return firstChildIsActive ? childWindowFirst.Handle : childWindowSecond.Handle;
			return mainWindow.Handle;
		}

        /// <summary>
        /// Gets handle of inactive subwindow.
        /// </summary>
        /// <returns></returns>
		protected override IntPtr GetInactiveWindowHandle() {
			return firstChildIsActive ? childWindowSecond.Handle : childWindowFirst.Handle;
		}

        /// <summary>
        /// Switches active and inactive subwindows.
        /// </summary>
		protected override void SwitchWindows() {
			firstChildIsActive = !firstChildIsActive;
			if (playerVisible) {
				setWindowVisibility(GetInactiveWindowHandle(), false);
				setWindowVisibility(GetActiveWindowHandle(), true);
			} else {
				setWindowVisibility(GetInactiveWindowHandle(), false);
				setWindowVisibility(GetActiveWindowHandle(), false);
			}
		}
	}
}