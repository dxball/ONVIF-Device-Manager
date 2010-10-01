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

#endregion

namespace DZ.MediaPlayer.Io
{
    /// <summary>
    /// Window where player renders ouput.
    /// </summary>
    public abstract class MediaWindow : IDisposable {
        /// <summary>
        /// Width of window.
        /// </summary>
        public abstract int Width {
            get;
            set;
        }

        /// <summary>
        /// Height of window.
        /// </summary>
        public abstract int Height {
            get;
            set;
        }

        /// <summary>
        /// X coordinate of window.
        /// </summary>
        public abstract int Left {
            get;
            set;
        }

        /// <summary>
        /// Y coordinate of window.
        /// </summary>
        public abstract int Top {
            get;
            set;
        }

        /// <summary>
        /// Background color used to fill
        /// background of window.
        /// </summary>
        public abstract uint BackgroundColor {
            get;
            set;
        }

        /// <summary>
        /// Path to background image path.
        /// </summary>
        public abstract string BackgroundImageFilePath {
            get;
            set;
        }

        /// <summary>
        /// Window visibility.
        /// </summary>
        public abstract bool Visible {
            get;
            set;
        }

        #region IDisposable Members

        ///<summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose() {
            Dispose(true);
        }

        #endregion

        /// <summary>
        /// Disposes resources.
        /// </summary>
        /// <param name="isDisposing">Defines where this method is invoked. <code>True</code> if from Dispose call, else from finalizer.</param>
        protected virtual void Dispose(bool isDisposing) {
        }
    }
}