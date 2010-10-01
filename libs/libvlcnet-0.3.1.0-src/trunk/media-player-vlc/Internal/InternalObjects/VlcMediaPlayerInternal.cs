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
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal.Interop;

#endregion

namespace DZ.MediaPlayer.Vlc.Internal.InternalObjects
{
    internal sealed class VlcMediaPlayerInternal : InternalObjectBase
    {
        private readonly IntPtr descriptor;

        public VlcMediaPlayerInternal(IntPtr descriptor) {
            if (descriptor == IntPtr.Zero) {
                throw new ArgumentException("Zero pointer.", "descriptor");
            }
            //
            this.descriptor = descriptor;
        }

        public override IntPtr Descriptor {
            get {
                VerifyObjectIsNotDisposed();
                //
                return (descriptor);
            }
        }

        protected override void Dispose(bool isDisposing) {
            try {
                LibVlcInterop.libvlc_media_player_release(descriptor);
            } finally {
                base.Dispose(isDisposing);
            }
        }

        #region Public Interfaces

        public TimeSpan Length {
            get {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                Int64 res = LibVlcInterop.libvlc_media_player_get_length(descriptor, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
                //
                return new TimeSpan(res * 10000);
            }
        }

        public TimeSpan Time {
            get {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                Int64 res = LibVlcInterop.libvlc_media_player_get_time(descriptor, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
                //
                return new TimeSpan(res * 10000);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                LibVlcInterop.libvlc_media_player_set_time(descriptor, Convert.ToInt64(value.TotalMilliseconds), ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
            }
        }

        public float Position {
            get {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                float res = LibVlcInterop.libvlc_media_player_get_position(descriptor, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
                //
                return (res);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                LibVlcInterop.libvlc_media_player_set_position(descriptor, value, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
            }
        }

        public int SPU {
            get {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                int res = LibVlcInterop.libvlc_video_get_spu(descriptor, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
                //
                return (res);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                LibVlcInterop.libvlc_video_set_spu(descriptor, value, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
            }
        }

        /// <summary>
        /// Frames per second.
        /// </summary>
        public float FPS {
            get {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                float res = LibVlcInterop.libvlc_media_player_get_fps(descriptor, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
                //
                return (res);
            }
        }

        public void SetMedia(VlcMediaInternal media) {
            if (media == null) {
                throw new ArgumentNullException("media");
            }
            //
            VerifyObjectIsNotDisposed();
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            LibVlcInterop.libvlc_media_player_set_media(descriptor, media.Descriptor, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
        }

        public void SetDisplayOutput(Int32 handle) {
            VerifyObjectIsNotDisposed();
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            LibVlcInterop.libvlc_media_player_set_drawable(descriptor, handle, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
        }

        public void Play() {
            VerifyObjectIsNotDisposed();
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            LibVlcInterop.libvlc_media_player_play(descriptor, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
        }

        public void Stop() {
            VerifyObjectIsNotDisposed();
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            LibVlcInterop.libvlc_media_player_stop(descriptor, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
        }

        public void Pause() {
            VerifyObjectIsNotDisposed();
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            LibVlcInterop.libvlc_media_player_pause(descriptor, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
        }

        public void TakeSnapshot(string filePath, int width, int height) {
            VerifyObjectIsNotDisposed();
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            IntPtr filePathPtr = Marshal.StringToHGlobalAnsi(filePath);
            //
            uint uwidth = Convert.ToUInt32(width);
            uint uheight = Convert.ToUInt32(height);
            //
            try {
                LibVlcInterop.libvlc_video_take_snapshot(descriptor, filePathPtr, uwidth, uheight, ref exc);
            } finally {
                Marshal.FreeHGlobal(filePathPtr);
            }
            //
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
        }

        #endregion
    }
}