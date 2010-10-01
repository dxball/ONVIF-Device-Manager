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
using System.Collections.Generic;

#endregion

namespace DZ.MediaPlayer.Io
{
    /// <summary>
    /// Container for player's output parameters.
    /// </summary>
    public sealed class PlayerOutput {
        /// <summary>
        /// Creates player output without window, which can be used to transcode or streaming.
        /// </summary>
        public PlayerOutput() {
        }

        /// <summary>
        /// Creates player with duplicated output to window.
        /// </summary>
        /// <param name="window">Window where rendering goes.</param>
        /// <exception cref="ArgumentNullException"><see cref="window"/> parameter is null.</exception>
        public PlayerOutput(MediaWindow window) {
            if (window == null) {
                throw new ArgumentNullException("window");
            }
            //
            this.window = window;
        }

        private readonly MediaWindow window;
        /// <summary>
        /// Window where video is rendered. Can be null.
        /// </summary>
        public MediaWindow Window {
            get {
                return (window);
            }
        }

        private readonly List<OutFile> files = new List<OutFile>();
        /// <summary>
        /// List of file output objects.
        /// </summary>
        public IList<OutFile> Files {
            get {
                return (files);
            }
        }

        private readonly List<OutputNetworkStream> networkStreams = new List<OutputNetworkStream>();
        /// <summary>
        /// List of network output objects.
        /// </summary>
        public IList<OutputNetworkStream> NetworkStreams {
            get {
                return (networkStreams);
            }
        }

        /// <summary>
        /// Is output window specified.
        /// </summary>
        public bool IsWindowDefined {
            get {
                return (window != null);
            }
        }
    }
}