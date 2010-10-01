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
    [StructLayout(LayoutKind.Explicit)]
    internal struct libvlc_value_t {
        [FieldOffset(0)]
        private int i_int;

        [FieldOffset(0)]
        private bool b_bool;

        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.R4)]
        private float f_float;

        [FieldOffset(0)]
        private IntPtr psz_string;

        [FieldOffset(0)]
        private IntPtr p_address;

        [FieldOffset(0)]
        public IntPtr p_object;

        [FieldOffset(0)]
        private IntPtr p_list;

        [FieldOffset(0)]
        private Int64 i_time;

        [FieldOffset(0)]
        private IntPtr psz_name;

        [FieldOffset(4)]
        private int i_object_id;

        [FieldOffset(0)]
        private byte a;

        [FieldOffset(1)]
        private byte b;

        [FieldOffset(2)]
        private byte c;

        [FieldOffset(3)]
        private byte d;

        [FieldOffset(4)]
        private byte e;

        [FieldOffset(5)]
        private byte f;

        [FieldOffset(6)]
        private byte g;

        [FieldOffset(7)]
        private byte h;

        public string name {
            get {
                return (psz_name != IntPtr.Zero ? Marshal.PtrToStringAnsi(psz_name) : null);
            }
        }

        public vlc_common_members? members {
            get {
                if (p_object == IntPtr.Zero) {
                    return (null);
                }
                return (vlc_common_members)
                       Marshal.PtrToStructure(p_object, typeof (vlc_common_members));
            }
        }
    }
}