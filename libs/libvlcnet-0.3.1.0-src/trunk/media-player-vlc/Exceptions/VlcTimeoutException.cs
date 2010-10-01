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
using System.Runtime.Serialization;

namespace DZ.MediaPlayer.Vlc.Exceptions
{
    /// <summary>
    /// Represents timeouted vlc operation.
    /// </summary>
    public sealed class VlcTimeoutException : MediaPlayerException
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public VlcTimeoutException() {
        }

        /// <summary>
        /// Constructor with string message.
        /// </summary>
        public VlcTimeoutException(string message) : base(message) {
        }

        /// <summary>
        /// Constructor for inner exceptions handling.
        /// </summary>
        public VlcTimeoutException(string message, Exception inner) : base(message, inner) {
        }

        /// <summary>
        /// Constructor with serialization support.
        /// </summary>
        public VlcTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
