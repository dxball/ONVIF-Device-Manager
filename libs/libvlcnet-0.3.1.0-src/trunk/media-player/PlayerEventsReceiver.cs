/* This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.
*/

namespace DZ.MediaPlayer {
    /// <summary>
    /// Receives events from vlc media player.
    /// </summary>
    public class PlayerEventsReceiver {
        /// <summary>
        /// Time of player changed.
        /// </summary>
        public virtual void OnTimeChanged() {
            //
        }

        /// <summary>
        /// Player is stopped.
        /// </summary>
        public virtual void OnStopped() {
            //
        }

        /// <summary>
        /// Position of player is changed.
        /// </summary>
        public virtual void OnPositionChanged() {
            //
        }

        /// <summary>
        /// Some error occured.
        /// </summary>
        public virtual void OnEncounteredError() {
            //
        }

        /// <summary>
        /// End of media reached.
        /// </summary>
        public virtual void OnEndReached() {
            //
        }

        /// <summary>
        /// State of player changed.
        /// </summary>
        public virtual void OnStateChanged() {
            //
        }
    }
}