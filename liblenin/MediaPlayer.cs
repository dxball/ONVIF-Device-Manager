using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace liblenin
{
    public class MediaPlayer : IDisposable
    {
        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr libvlc_media_player_new_from_media(IntPtr hMedia);
        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libvlc_media_player_release(IntPtr hMediaPlayer);

        #region IDisposable Members

        public void Dispose()
        {
            if (IntPtr.Zero != m_handle)
            {
                libvlc_media_player_release(m_handle);
            }
        }

        #endregion

        protected MediaPlayer()
        {
        }

        public static MediaPlayer CreateInstance(IntPtr hMedia)
        {
            MediaPlayer mp = new MediaPlayer();
            mp.m_handle = libvlc_media_player_new_from_media(hMedia);
            return mp;
        }

        private IntPtr m_handle = IntPtr.Zero;

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 libvlc_media_player_play(IntPtr hMediaPlayer);
        public void Play()
        {
            if (IntPtr.Zero != m_handle)
            {
                if (0 > libvlc_media_player_play(m_handle))
                {
                    throw new ArgumentNullException("asdsad");
                }
            }
        }

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libvlc_media_player_stop(IntPtr hMediaPlayer);
        public void Stop()
        {
            if (IntPtr.Zero != m_handle)
            {
                libvlc_media_player_stop(m_handle);
            }
        }

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libvlc_media_player_set_hwnd(IntPtr hMediaPlayer, IntPtr hWnd);
        public void SetHwnd(IntPtr hWnd)
        {
            if (IntPtr.Zero != m_handle)
            {
                try
                {
                    libvlc_media_player_set_hwnd(m_handle, hWnd);
                }
                finally
                {
                }
            }
        }
    }
}
