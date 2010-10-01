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
using System.Diagnostics;

#endregion

namespace DZ.MediaPlayer.Common
{
    /// <summary>
    /// Base class for types requires necessary Dispose() call after using instance of type.
    /// </summary>
    public class DisposingRequiredObjectBase : IDisposable
    {
        private bool isDisposed;
        /// <summary>
        /// Is object already disposed.
        /// </summary>
        public bool IsDisposed {
            get {
                return (isDisposed);
            }
        }

        /// <summary>
        /// Verifies that object was not disposed.
        /// If disposed, <see cref="ObjectDisposedException"/> will be thrown.
        /// </summary>
        protected void VerifyObjectIsNotDisposed() {
            if (isDisposed) {
                throw new ObjectDisposedException(ToString(), String.Format("This instance of {0} has been already disposed.", GetType()));
            }
        }

        /// <summary>
        /// Overridable method for release allocated resources from the derived types.
        /// </summary>
        /// <param name="isDisposing">isDisposing is True if using a determined destruction, False if called from finalizer.</param>
        protected virtual void Dispose(bool isDisposing) {
        }

        /// <summary>
        /// Cleanup all.
        /// </summary>
        public void Dispose() {
            if (!isDisposed) {
                isDisposed = true;
                //
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        
        /// <summary>
        /// Debug-style finalizer.
        /// </summary>
        ~DisposingRequiredObjectBase() {
#if DEBUG
            Debugger.Log(0, Debugger.DefaultCategory, String.Format("Finalizer of {0} has been called. The object has not been disposed correctly.\n", GetType()));
#endif
            //
            if (!isDisposed) {
                // NOTE : you can ban the finalizer call if comment next statement
                Dispose(false);
            }
        }
    }
}