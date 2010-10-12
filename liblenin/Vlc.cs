using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace liblenin
{
    public class Vlc : IDisposable
    {
        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr libvlc_new(Int32 argc,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]string[] argv);

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern void libvlc_release(IntPtr handle);

        public Vlc()
        {
            m_handle = libvlc_new(0, null);
        }

        private IntPtr m_handle = IntPtr.Zero;

        #region IDisposable Members

        public void Dispose()
        {
            if (IntPtr.Zero != m_handle)
            {
                libvlc_release(m_handle);
                m_handle = IntPtr.Zero;
            }
        }

        #endregion

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr libvlc_media_new_path(IntPtr hInstance, string path);
        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libvlc_media_release(IntPtr hMedia);

        public MediaPlayer CreateMediaPlayer(string path)
        {
            IntPtr hMedia = IntPtr.Zero;
            MediaPlayer mp = null;
            try
            {
                hMedia = libvlc_media_new_path(m_handle, path);
                if (IntPtr.Zero != hMedia)
                {
                    mp = MediaPlayer.CreateInstance(hMedia);
                }
            }
            finally
            {
                if (IntPtr.Zero != hMedia)
                {
                    libvlc_media_release(hMedia);
                }
            }
            return mp;
        }
    };
}
