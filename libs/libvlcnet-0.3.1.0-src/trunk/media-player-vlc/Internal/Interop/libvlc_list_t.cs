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
    [StructLayout(LayoutKind.Sequential)]
    internal struct libvlc_list_t {
        private int i_count;
        private IntPtr p_values;
        private IntPtr pi_types;

        public libvlc_value_t[] values {
            get {
                libvlc_value_t[] arr = new libvlc_value_t[i_count];
                for (int i = 0; i < i_count; i++) {
                    IntPtr ptr = new IntPtr((Int32) p_values + i*Marshal.SizeOf(typeof (libvlc_value_t)));
                    libvlc_value_t structure = (libvlc_value_t) Marshal.PtrToStructure(ptr, typeof (libvlc_value_t));
                    arr[i] = structure;
                }
                return (arr);
            }
        }
    }
}