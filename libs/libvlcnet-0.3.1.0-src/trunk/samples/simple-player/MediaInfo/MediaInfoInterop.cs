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

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SimplePlayer.MediaInfo
{
    /// <summary>
    /// Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
    /// </summary>
    internal static class MediaInfoInterop
    {
        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MediaInfo_New();

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void MediaInfo_Delete(IntPtr Handle);

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MediaInfo_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName);

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void MediaInfo_Close(IntPtr Handle);

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MediaInfo_Inform(IntPtr Handle, IntPtr Reserved);

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MediaInfo_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MediaInfo_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, [MarshalAs(UnmanagedType.LPWStr)] string Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MediaInfo_Option(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string Option, [MarshalAs(UnmanagedType.LPWStr)] string Value);

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MediaInfo_State_Get(IntPtr Handle);

        [DllImport("MediaInfo.dll", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MediaInfo_Count_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber);
    }
}