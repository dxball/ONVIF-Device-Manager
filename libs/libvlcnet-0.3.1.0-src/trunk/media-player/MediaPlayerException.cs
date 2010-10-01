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
using System.Runtime.Serialization;

#endregion

namespace DZ.MediaPlayer {
	/// <summary>
	/// Exception represents an error when using media library.
	/// </summary>
    [Serializable]
    public class MediaPlayerException : ApplicationException {
		/// <summary>
		/// Default constructor.
		/// </summary>
        public MediaPlayerException() {
        }

		/// <summary>
		/// Creates MediaPlayerException instance with specified message.
		/// </summary>
		/// <param name="message">Message of exception</param>
        public MediaPlayerException(string message)
            : base(message) {
        }

		/// <summary>
		/// Creates MediaPlayerException instance with specified message.
		/// </summary>
		/// <param name="message">Message of exception</param>
        /// <param name="inner">Inner exception.</param>
        public MediaPlayerException(string message, Exception inner)
            : base(message, inner) {
        }

		/// <summary>
		/// Constructor used during serialization.
		/// </summary>
		/// <param name="info"><see cref="SerializationInfo"/> instance.</param>
		/// <param name="context"><see cref="StreamingContext"/> instance.</param>
        public MediaPlayerException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}