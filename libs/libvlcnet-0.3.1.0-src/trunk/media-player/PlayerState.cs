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

namespace DZ.MediaPlayer {
	/// <summary>
	/// State of player.
	/// </summary>
    public enum PlayerState {
		/// <summary>
		/// Nothing to do.
		/// </summary>
        Idle = 0,
		/// <summary>
		/// Media is currently opening.
		/// </summary>
        Opening = 1,
		/// <summary>
		/// Bufferization
		/// </summary>
        Buffering = 2,
		/// <summary>
		/// Playing an media.
		/// </summary>
        Playing = 3,
		/// <summary>
		/// Player is paused.
		/// </summary>
        Paused = 4,
		/// <summary>
		/// Player was stopped.
		/// </summary>
        Stopping = 5,
        ///// <summary>
        ///// TODO: remove?
        ///// </summary>
        //Forward = 6,
        ///// <summary>
        ///// TODO: remove?
        ///// </summary>
        //Backward = 7,
		/// <summary>
		/// End of media reached.
		/// </summary>
        Ended = 8,
		/// <summary>
		/// Error occured.
		/// </summary>
        Error = 9
    }
}