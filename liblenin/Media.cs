using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace liblenin
{
    public class Media : IDisposable
    {
        private IntPtr mHandle = IntPtr.Zero;

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libvlc_media_release(IntPtr hMedia);

        public Media(IntPtr hMedia)
        {
            mHandle = hMedia;
        }

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr libvlc_media_event_manager(IntPtr hMedia);
        private EventManager _EventManager = null;
        public EventManager EventManager
        {
            get
            {
                if (null == _EventManager)
                {
                    _EventManager = new EventManager(libvlc_media_event_manager(mHandle));
                }
                return _EventManager;
            }
        }

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libvlc_media_parse_async(IntPtr hMedia);
        public void ParseAsync()
        {
            libvlc_media_parse_async(mHandle);
        }

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int libvlc_media_get_tracks_info(IntPtr hMedia, ref IntPtr hTracks);
        public int GetTracksInfo(ref IntPtr hTracks)
        {
            return libvlc_media_get_tracks_info(mHandle, ref hTracks);
        }

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libvlc_media_add_option_flag(IntPtr hMedia, string option, UInt32 iFlag);
        public void AddOption(string opt, UInt32 iFlag)
        {
            libvlc_media_add_option_flag(mHandle, opt, iFlag);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (IntPtr.Zero != mHandle) 
            {
                libvlc_media_release(mHandle);
                mHandle = IntPtr.Zero;
            }
        }

        #endregion
    }
}
