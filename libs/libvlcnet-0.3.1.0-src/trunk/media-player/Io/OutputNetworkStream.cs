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

namespace DZ.MediaPlayer.Io
{
    /// <summary>
    /// Represents a network streaming options.
    /// </summary>
    public sealed class OutputNetworkStream
    {
        private readonly string ip;
        private readonly int port;
        private readonly NetworkProtocol protocol;

        /// <summary>
        /// Instantiates network streaming definition. This constructor can
        /// be used with any <see cref="NetworkProtocol"/> except <see cref="NetworkProtocol.RTP"/>.
        /// </summary>
        /// <param name="protocol">Protocol to use.</param>
        /// <param name="ip">Ip address to use.</param>
        /// <param name="port">Port to use.</param>
        /// <exception cref="ArgumentException"><see cref="protocol"/> is RTP. This constuctor does not allows defining audio and video ports separately.</exception>
        public OutputNetworkStream(NetworkProtocol protocol, string ip, ushort port) {
            if (ip == null) {
                throw new ArgumentNullException("ip");
            }
            if (ip.Length == 0) {
                throw new ArgumentException("String is empty.", "ip");
            }
            //
            this.protocol = protocol;
            this.ip = ip;
            this.port = port;
        }

        /// <summary>
        /// Instantiates network streaming definition. This constructor can
        /// be used with only with <see cref="NetworkProtocol.RTP"/> value of protocol parameter.
        /// </summary>
        /// <param name="protocol">Protocol to use.</param>
        /// <param name="ip">Ip address to use.</param>
        /// <param name="port">Port to use.</param>
        /// <param name="rtpPortAudio">Port to use for audio.</param>
        /// <param name="rtpPortVideo">Port to use for video.</param>
        /// <exception cref="ArgumentException"><see cref="protocol"/> is not RTP.</exception>
        public OutputNetworkStream(NetworkProtocol protocol, string ip, ushort port, ushort rtpPortAudio, ushort rtpPortVideo)
            : this(protocol, ip, port) {
            //
            this.rtpPortAudio = rtpPortAudio;
            this.rtpPortVideo = rtpPortVideo;
        }

        /// <summary>
        /// Protocol of network streaming.
        /// </summary>
        public NetworkProtocol Protocol {
            get {
                return (protocol);
            }
        }

        /// <summary>
        /// Ip address.
        /// </summary>
        public string Ip {
            get {
                return (ip);
            }
        }

        /// <summary>
        /// Port number.
        /// </summary>
        public int Port {
            get {
                return (port);
            }
        }

        #region Only for RTP

        private readonly int rtpPortAudio;
        /// <summary>
        /// Port for audio.
        /// </summary>
        public int RtpPortAudio {
            get {
                return (rtpPortAudio);
            }
        }

        private readonly int rtpPortVideo;
        /// <summary>
        /// Port for video.
        /// </summary>
        public int RtpPortVideo {
            get {
                return (rtpPortVideo);
            }
        }

        #endregion
    }
}