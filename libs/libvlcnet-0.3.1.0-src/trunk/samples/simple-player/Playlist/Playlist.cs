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
using System.Collections.Generic;
using System.ComponentModel;

namespace SimplePlayer.Playlist
{
    /// <summary>
    /// Represents a container for playlist items.
    /// </summary>
    public sealed class Playlist : INotifyPropertyChanged
    {
        private readonly Random random = new Random();

        private bool repeatMode;

        /// <summary>
        /// Is playlist in a repeat mode.
        /// </summary>
        public bool RepeatMode {
            get {
                return (repeatMode);
            }
            set {
                repeatMode = value;
            }
        }

        private bool randomMode;

        /// <summary>
        /// Is playlist in a random mode.
        /// </summary>
        public bool RandomMode {
            get {
                return (randomMode);
            }
            set {
                randomMode = value;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Playlist() {
            items = new BindingList<PlaylistItem>();
        }

        private readonly BindingList<PlaylistItem> items;

        /// <summary>
        /// Collection of items in the playlist.
        /// Supports data binding.
        /// </summary>
        public IList<PlaylistItem> Items {
            get {
                return (items);
            }
        }

        private int currentIndex;

        /// <summary>
        /// Index of active playlist item.
        /// </summary>
        public int CurrentIndex {
            get {
                return (currentIndex);
            }
            set {
                if (currentIndex != value) {
                    currentIndex = value;
                    //
                    onPropertyChanged("CurrentIndex");
                }
            }
        }

        private void onPropertyChanged(string currentindex) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler.Invoke(this, new PropertyChangedEventArgs(currentindex));
            }
        }

        /// <summary>
        /// Item pointed from <see cref="CurrentIndex"/>.
        /// Can be null.
        /// </summary>
        public PlaylistItem CurrentItem {
            get {
                if ((currentIndex < 0) || (currentIndex >= items.Count)) {
                    return (NextItem);
                }
                //
                return (items[currentIndex]);
            }
        }

        /// <summary>
        /// Item pointed from next after <see cref="CurrentIndex"/> item.
        /// Can be null.
        /// </summary>
        public PlaylistItem NextItem {
            get {
                if (items.Count == 0) {
                    return (null);
                }
                //
                return (items[getNextIndex()]);
            }
        }

        /// <summary>
        /// Item pointed from previous before <see cref="CurrentIndex"/> item.
        /// Can be null.
        /// </summary>
        public PlaylistItem PrevItem {
            get {
                if (items.Count == 0) {
                    return (null);
                }
                //
                return (items[getPrevIndex()]);
            }
        }

        private int getNextIndex() {
            int res = currentIndex + 1;
            if ((res >= items.Count) || (res < 0)) {
                return (0);
            }
            //
            return (res);
        }

        private int getPrevIndex() {
            int res = currentIndex - 1;
            if ((res < 0) || (res >= items.Count)) {
                return (items.Count - 1);
            }
            //
            return (res);
        }

        /// <summary>
        /// Moves <see cref="CurrentIndex"/> pointer to begin.
        /// </summary>
        public void MoveBegin() {
            CurrentIndex = 0;
        }

        /// <summary>
        /// Moves <see cref="CurrentIndex"/> pointer to the next item circularly.
        /// </summary>
        public void MoveNext() {
            CurrentIndex = getNextIndex();
        }

        /// <summary>
        /// Moves <see cref="CurrentIndex"/> pointer to the previous item circularly.
        /// </summary>
        public void MovePrev() {
            CurrentIndex = getPrevIndex();
        }

        /// <summary>
        /// Moves <see cref="CurrentIndex"/> pointer to the random item.
        /// </summary>
        public void MoveRandom() {
            CurrentIndex = (items.Count == 0) ? (0) : random.Next(0, items.Count - 1);
        }

        /// <summary>
        /// Fires <see cref="PlaylistItemEntered"/> event on current item.
        /// </summary>
        public void EnterOnItem() {
            PlaylistItemEnteredHandler handler = PlaylistItemEntered;
            //
            if (handler != null) {
                handler.Invoke(this, new PlaylistItemEnteredEventArgs(currentIndex));
            }
        }

        /// <summary>
        /// Event raised by user code and intercepted by player.
        /// </summary>
        public event PlaylistItemEnteredHandler PlaylistItemEntered;

        /// <summary>
        /// Event signals that property has been changed.
        /// For data binding.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// Type of callback method.
    /// </summary>
    public delegate void PlaylistItemEnteredHandler(object sender, PlaylistItemEnteredEventArgs e);

    /// <summary>
    /// Defines event arguments for <see cref="Playlist.PlaylistItemEntered"/> event.
    /// </summary>
    public sealed class PlaylistItemEnteredEventArgs : EventArgs
    {
        private readonly int selectedPlaylistItemIndex;

        /// <summary>
        /// Entered item index.
        /// </summary>
        public int SelectedPlaylistItemIndex {
            get {
                return (selectedPlaylistItemIndex);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlaylistItemEnteredEventArgs(int selectedPlaylistItemIndex) {
            this.selectedPlaylistItemIndex = selectedPlaylistItemIndex;
        }
    }
}