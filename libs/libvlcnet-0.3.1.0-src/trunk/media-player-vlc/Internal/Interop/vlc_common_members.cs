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
    internal struct vlc_common_members
    {
        public Int32 i_object_id;
        public Int32 i_object_type;
        public IntPtr psz_object_type;
        public IntPtr psz_object_name;
        public IntPtr psz_header;
        public Int32 i_flags;
        public bool b_error;
        public bool b_die;
        public bool b_dead;
        public bool b_force;
        public IntPtr p_libvlc;
        public IntPtr p_parent;
        public IntPtr p_private;
        public Int32 be_sure_to_add_VLC_COMMON_MEMBERS_to_struct;

        public string ObjectName {
            get {
                return Marshal.PtrToStringAnsi(psz_object_name);
            }
        }

        public string ObjectType {
            get {
                return Marshal.PtrToStringAnsi(psz_object_type);
            }
        }

        public string Header {
            get {
                return Marshal.PtrToStringAnsi(psz_header);
            }
        }

        public override string ToString() {
            return String.Format("Name='{0}' Type='{1}' TypeID='{2}'", ObjectName, ObjectType, i_object_type);
        }
    }
}
