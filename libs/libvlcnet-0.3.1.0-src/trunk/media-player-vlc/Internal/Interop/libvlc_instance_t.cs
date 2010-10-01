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
    internal struct libvlc_instance_t
    {
        public IntPtr p_libvlc_int;
        public IntPtr p_vlm;
        public Int32 b_playlist_locked;
        public UInt32 ref_count;
        public IntPtr instance_lock; // vlc_mutex_t == win32 HANDLE
        public IntPtr event_callback_lock;
        public IntPtr p_callback_list;
    }
}