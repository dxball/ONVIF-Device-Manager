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

namespace SimplePlayer.MediaInfo
{
    /// <summary>
    /// The most important information about video
    /// </summary>
    public struct BasicVideoInformation {
        /// <summary>
        /// File name.
        /// </summary>
        public string FileName;
        /// <summary>
        /// Size of file.
        /// </summary>
        public int FileSize;
        /// <summary>
        /// Duration in milliseconds.
        /// </summary>
        public int DurationMilliseconds;
        /// <summary>
        /// Video format description string.
        /// </summary>
        public string VideoFormat;
        /// <summary>
        /// Video codec description string.
        /// </summary>
        public string VideoCodec;
        /// <summary>
        /// Audio codec description string.
        /// </summary>
        public string AudioCodec;
        /// <summary>
        /// Video original width in pixels.
        /// </summary>
        public int Width;
        /// <summary>
        /// Video original height in pixels.
        /// </summary>
        public int Height;
        /// <summary>
        /// Video bitrate.
        /// </summary>
        public int Bitrate;
        /// <summary>
        /// Video frames per seconds.
        /// </summary>
        public double Fps;
    }
}