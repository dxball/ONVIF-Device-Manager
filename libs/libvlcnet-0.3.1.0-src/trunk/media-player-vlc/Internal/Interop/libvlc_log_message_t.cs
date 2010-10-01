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

#endregion

namespace DZ.MediaPlayer.Vlc.Internal.Interop
{
    internal enum libvlc_log_messate_t_severity
    {
        INFO = 0,
        ERR = 1,
        WARN = 2,
        DBG = 3
    }

    /// <summary>
    /// Represents libvlc-compatible structure of original log message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct libvlc_log_message_t
    {
        /// <summary>
        /// Sizeof() of message structure, must be filled in by user.
        /// </summary>
        public UInt32 sizeof_msg;
        /// <summary>
        /// Severity code :
        /// 0=INFO, 1=ERR, 2=WARN, 3=DBG.
        /// </summary>
        private Int32 i_severity;
        /// <summary>
        /// Module type.
        /// </summary>
        private IntPtr psz_type;
        /// <summary>
        /// Module name.
        /// </summary>
        private IntPtr psz_name;
        /// <summary>
        /// Optional header.
        /// </summary>
        private IntPtr psz_header;
        /// <summary>
        /// Message.
        /// </summary>
        private IntPtr psz_message;

        public libvlc_log_messate_t_severity Severity {
            get {
                return (libvlc_log_messate_t_severity) (i_severity);
            }
        }

        public string Type {
            get {
                return (Marshal.PtrToStringAnsi(psz_type));
            }
        }

        public string Name {
            get {
                return (Marshal.PtrToStringAnsi(psz_name));
            }
        }

        public string Header {
            get {
                return (Marshal.PtrToStringAnsi(psz_header));
            }
        }

        public string Message {
            get {
                return (Marshal.PtrToStringAnsi(psz_message));
            }
        }
    }
}