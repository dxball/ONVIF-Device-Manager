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
using Common.Logging;
using DZ.MediaPlayer.Io;
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal.Interfaces;
using DZ.MediaPlayer.Vlc.Internal.InternalObjects;
using DZ.MediaPlayer.Vlc.Internal.Interop;

namespace DZ.MediaPlayer.Vlc.Internal
{
    /// <summary>
    /// Helper to create vlclib internal objects using the libvlc instance descriptor
    /// retrieved from <see cref="VlcMediaLibraryFactory"/>.
    /// </summary>
    internal class InternalObjectsFactory : IInternalObjectsFactory
    {
        private readonly IntPtr vlclibDescriptor;

        public InternalObjectsFactory(IntPtr vlclibDescriptor) {
            if (vlclibDescriptor == IntPtr.Zero) {
                throw new ArgumentException("Zero pointer.", "vlclibDescriptor");
            }
            //
            this.vlclibDescriptor = vlclibDescriptor;
        }

        #region IInternalObjectsFactory Members

        public VlcMediaInternal CreateVlcMediaInternal(MediaInput mediaInput) {
            if (mediaInput == null) {
                throw new ArgumentNullException("mediaInput");
            }
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            IntPtr mediaDescriptor = LibVlcInterop.libvlc_media_new(vlclibDescriptor, mediaInput.Source, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
            //
            return new VlcMediaInternal(mediaDescriptor);
        }

        public VlcMediaPlayerInternal CreateVlcMediaPlayerInternal() {
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            IntPtr mediaplayerDescriptor = LibVlcInterop.libvlc_media_player_new(vlclibDescriptor, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
            //
            return new VlcMediaPlayerInternal(mediaplayerDescriptor);
        }

        public VlcLog CreateVlcLog(ILog log, ILogVerbosityManager logVerbosityManager) {
            if (log == null) {
                throw new ArgumentNullException("log");
            }
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            IntPtr libvlc_log_t = LibVlcInterop.libvlc_log_open(vlclibDescriptor, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
            //
            return new VlcLog(libvlc_log_t, logVerbosityManager, log);
        }

        public libvlc_instance_t GetInteropStructure() {
            return (libvlc_instance_t) Marshal.PtrToStructure(vlclibDescriptor, typeof (libvlc_instance_t));
        }

        #endregion
    }
}
