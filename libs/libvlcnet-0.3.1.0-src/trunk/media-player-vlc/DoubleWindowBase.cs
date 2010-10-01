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
using DZ.MediaPlayer.Io;

#endregion

namespace DZ.MediaPlayer.Vlc
{
    /// <summary>
    /// Provides logic for VLC library to allow
    /// changing video streams on the fly.
    /// </summary>
    public abstract class DoubleWindowBase : MediaWindow
    {
        internal bool PlayerVisibleInternal {
            get {
                return (PlayerVisible);
            }
            set {
                if (PlayerVisible != value) {
                    PlayerVisible = value;
                }
            }
        }

        /// <summary>
        /// Player visibility.
        /// </summary>
        protected abstract bool PlayerVisible {
            get;
            set;
        }

        /// <summary>
        /// Returns handle to first window handle.
        /// </summary>
        /// <returns>OS specific handle of window.</returns>
        protected abstract IntPtr GetActiveWindowHandle();

        /// <summary>
        /// Returns handle to second window handle.
        /// </summary>
        /// <returns>OS specific handle of window.</returns>
        protected abstract IntPtr GetInactiveWindowHandle();

        /// <summary>
        /// Provides logic to switch between two windows.
        /// </summary>
        protected abstract void SwitchWindows();

        /// <summary>
        /// For internal library calls.
        /// </summary>
        internal IntPtr GetActiveWindowHandleInternal() {
            return (GetActiveWindowHandle());
        }

        /// <summary>
        /// For internal library calls.
        /// </summary>
        internal IntPtr GetInactiveWindowHandleInternal() {
            return (GetInactiveWindowHandle());
        }

        /// <summary>
        /// For internal library calls.
        /// </summary>
        internal void SwitchWindowsInternal() {
            SwitchWindows();
        }
    }
}