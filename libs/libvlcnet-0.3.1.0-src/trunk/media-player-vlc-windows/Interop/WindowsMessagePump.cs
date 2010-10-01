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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Common.Logging;

#endregion

namespace DZ.MediaPlayer.Vlc.Windows.Interop
{
    /// <summary>
    /// Represents a custom windows messages pump.
    /// </summary>
    public class WindowsMessagePump : IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof (WindowsMessagePump).FullName);
        private readonly ManualResetEvent eventStop = new ManualResetEvent(false);
        private StringBuilder builder = new StringBuilder(256);
        private bool run;
        private IntPtr windowHandle;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WindowsMessagePump() {
            windowHandle = IntPtr.Zero;
            initialize();
        }

        /// <summary>
        /// Constructor with window specified.
        /// </summary>
        public WindowsMessagePump(IntPtr window) {
            windowHandle = window;
            initialize();
        }

        /// <summary>
        /// Handle of window.
        /// </summary>
        public IntPtr WindowHandle {
            get {
                return (windowHandle);
            }
            set {
                if (run) {
                    throw new InvalidOperationException("Cannot set window handle of pump when message pump in RUN state.");
                }
                windowHandle = value;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Clean up resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }

        #endregion

        private void initialize() {
        }

        /// <summary>
        /// Runs the pump.
        /// </summary>
        public void Run() {
            run = true;
            eventStop.Reset();
            DispatchRoutine();
        }

        /// <summary>
        /// Stops the pump.
        /// </summary>
        public void Stop() {
            run = false;
            eventStop.Set();
        }

        private void DispatchRoutine() {
            //
            WindowMessage msg = new WindowMessage();
            while (run) {
                IntPtr[] array = new IntPtr[1];
                array[0] = eventStop.SafeWaitHandle.DangerousGetHandle();
                //
                int result = NativeMethods.MsgWaitForMultipleObjectsEx(1, array, -1,
                    NativeMethods.MsgWaitEventFlags.QS_ALLEVENTS |
                    NativeMethods.MsgWaitEventFlags.QS_ALLPOSTMESSAGE |
                    NativeMethods.MsgWaitEventFlags.QS_SENDMESSAGE |
                    NativeMethods.MsgWaitEventFlags.QS_EVENT |
                    NativeMethods.MsgWaitEventFlags.QS_KEY |
                    NativeMethods.MsgWaitEventFlags.QS_PAINT |
                    NativeMethods.MsgWaitEventFlags.QS_POSTMESSAGE,
                    NativeMethods.MsgWaitFlags.InputAvailable);
                if (result == 0) {
                    break;
                } else if (result == 1) {
                    result = NativeMethods.GetMessageW(ref msg, windowHandle, 0, 0);
                    //
                    /*logger.Info("Message received. Window: 0x{0:x}({1}) ; Message Id: 0x{2:x}({3}) ", (int)msg.windowHandle, GetWindowText(msg.windowHandle),
								msg.messageCode,
								Enum.GetName(typeof(NativeMethods.WindowsMessages), (NativeMethods.WindowsMessages)msg.messageCode));*/
                    //
                    if (result == -1) {
                        return;
                    } else if (result == 0) {
                        return;
                    }
                    if (!FilterWindowMessage(ref msg)) {
                        NativeMethods.TranslateMessage(ref msg);
                        NativeMethods.DispatchMessage(ref msg);
                    } else {
                        //logger.Info("Message filtered and not dispatched.");
                    }
                }
            }
        }

        /// <summary>
        /// Returns window's text by the API function.
        /// </summary>
        public string GetWindowText(IntPtr handle) {
            GetWindowTextW(handle, builder, builder.Capacity);
            return (builder.ToString());
        }

        /// <summary>
        /// Win32 thunk to original unicode function.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int GetWindowTextW(IntPtr hWnd,
                                                [MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 2)] StringBuilder lpString,
                                                int nMaxCount
            );

        /// <summary>
        /// FilterWindowMessage overridable method.
        /// </summary>
        protected internal virtual bool FilterWindowMessage(ref WindowMessage message) {
            return (false);
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~WindowsMessagePump() {
            Dispose(false);
        }

        /// <summary>
        /// Clean up.
        /// </summary>
        protected virtual void Dispose(bool disposing) {
            Stop();
            eventStop.Close();
            if (disposing) {
                GC.SuppressFinalize(this);
            }
        }
    }
}