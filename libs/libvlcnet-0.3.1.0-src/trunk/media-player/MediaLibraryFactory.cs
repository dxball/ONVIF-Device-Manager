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

using DZ.MediaPlayer.Common;
using DZ.MediaPlayer.Filters;
using DZ.MediaPlayer.Io;

#endregion

namespace DZ.MediaPlayer
{
    /// <summary>
    /// Creates and instantiates all media library classes. Central
    /// factory of media library.
    /// </summary>
    public abstract class MediaLibraryFactory : DisposingRequiredObjectBase
    {
        /// <summary>
        /// Cleanup resources. Should be overrided.
        /// </summary>
        protected override void Dispose(bool isDisposing) {
            try {
            } finally {
                base.Dispose(isDisposing);
            }
        }

        /// <summary>
        /// Creates player which can be used to control media playing.
        /// </summary>
        /// <returns>Player instance.</returns>
        public abstract Player CreatePlayer(PlayerOutput playerOutput);

        /// <summary>
        /// Creates player with set of necessary filters.
        /// </summary>
        public abstract Player CreatePlayer(PlayerOutput playerOutput, IFilterBase filtersRequired );

        /// <summary>
        /// Creates window where player renders video.
        /// User can control position of window and it's size.
        /// </summary>
        /// <returns>Window for rendering video.</returns>
        public abstract MediaWindow CreateWindow();
    }
}