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
using DZ.MediaPlayer.Io;
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal.Interop;

#endregion

namespace DZ.MediaPlayer.Vlc.Internal.InternalObjects
{
    internal sealed class VlcMediaInternal : InternalObjectBase
    {
        private readonly IntPtr descriptor;

        public VlcMediaInternal(IntPtr descriptor) {
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
                LibVlcInterop.libvlc_media_release(descriptor);
            } finally {
                base.Dispose(isDisposing);
            }
        }

        private void addOption(string option) {
            if (option == null) {
                throw new ArgumentNullException("option");
            }
            if (option.Length == 0) {
                throw new ArgumentException("String is empty.", "option");
            }
            //
            VerifyObjectIsNotDisposed();
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            LibVlcInterop.libvlc_media_add_option(descriptor, option, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
        }

        #region Public interfaces

        public VlcMediaState State {
            get {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                VlcMediaState res = (VlcMediaState) LibVlcInterop.libvlc_media_get_state(descriptor, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
                //
                return (res);
            }
        }

        public void SetOutput(PlayerOutput playerOutput) {
            if (playerOutput == null) {
                throw new ArgumentNullException("playerOutput");
            }
            //
            VerifyObjectIsNotDisposed();
            //
            if ((playerOutput.Files.Count == 0) && (playerOutput.NetworkStreams.Count == 0)) {
                //
                //addOption("--video-filter=adjust@my_label");
                //
                return;
            }

            //string transcodeString = "vcodec=WMV2,vb=800,scale=1,acodec=wma,ab=128,channels=2";
            const string transcodeString = "vcodec=WMV2,vb=1024,scale=1";

            // Здесь media должна знать, будет ли она дублироваться на экран
            string duplicateString = (playerOutput.Window != null) ? "dst=display" : String.Empty;
            foreach (OutFile file in playerOutput.Files) {
                //dst=std{access=file,mux=ps,dst=\"{0}\"}
                string s = String.Format("dst=std[access=file,mux=ps,dst=\"{0}\"]", file.FileName);

                if (String.IsNullOrEmpty(duplicateString)) {
                    duplicateString = s;
                } else {
                    duplicateString += "," + s;
                }
            }

            foreach (OutputNetworkStream stream in playerOutput.NetworkStreams) {
                //dst=std{access=http,mux=asf,dst=172.28.1.4:8888}
                //dst=rtp{dst=1231232,mux=asf,port=1234,port-audio=12367,port-video=31236}
                string s;
                if (stream.Protocol != NetworkProtocol.RTP) {
                    s = String.Format("dst=std[access={0},mux=asf,dst={1}:{2}]",
                        stream.Protocol.ToString().ToLower(), stream.Ip, stream.Port);
                } else {
                    s = String.Format("dst=rtp[dst={0},mux=asf,port={1},port-audio={2},port-video={3}]",
                        stream.Ip, stream.Port, stream.RtpPortAudio, stream.RtpPortVideo);
                }

                if (String.IsNullOrEmpty(duplicateString)) {
                    duplicateString = s;
                } else {
                    duplicateString += "," + s;
                }
            }

            string optionsString = String.Format(":sout=#transcode[{0}]:duplicate[{1}]", transcodeString, duplicateString);

            addOption(optionsString.Replace('[', '{').Replace(']', '}'));
        }

        #endregion
    }
}