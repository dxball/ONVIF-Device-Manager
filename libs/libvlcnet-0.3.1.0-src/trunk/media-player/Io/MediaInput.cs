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
using System.IO;

#endregion

namespace DZ.MediaPlayer.Io
{
    /// <summary>
    /// Represents input media stream.
    /// </summary>
    public class MediaInput {
        private readonly string source;
        private readonly MediaInputType type;

        /// <summary>
        /// Instantiates media input using wpecified type and source.
        /// </summary>
        /// <param name="type">Type of media stream.</param>
        /// <param name="source">String which identifies source of media resource.</param>
        /// <exception cref="ArgumentOutOfRangeException">Invalid type of media input.</exception>
        /// <exception cref="ArgumentNullException">Source string is null.</exception>
        /// <exception cref="ArgumentException">Source string is empty.</exception>
        /// <exception cref="FileNotFoundException">If media type is <code>MediaInputType.File</code> and source file does not exists.</exception>
        public MediaInput(MediaInputType type, string source) {
            if (type < MediaInputType.File || type > MediaInputType.Device) {
                throw new ArgumentOutOfRangeException("type");
            }
            if (source == null) {
                throw new ArgumentNullException("source");
            }
            if (source.Length == 0) {
                throw new ArgumentException("Source string cannot be empty.", "source");
            }
            //
            if (type == MediaInputType.File) {
                if (!File.Exists(source)) {
                    throw new FileNotFoundException("Source file not found.", source);
                }
            }
            //
            this.type = type;
            this.source = source;
        }

        /// <summary>
        /// Type of media stream.
        /// </summary>
        public MediaInputType Type {
            get {
                return (type);
            }
        }

        /// <summary>
        /// String which identifies source of media resource.
        /// </summary>
        public string Source {
            get {
                return (source);
            }
        }
    }
}