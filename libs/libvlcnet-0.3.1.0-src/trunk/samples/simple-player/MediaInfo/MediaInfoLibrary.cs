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
using System.IO;
using System.Runtime.InteropServices;
using DZ.MediaPlayer.Common;
using SimplePlayer.MediaInfo.Enums;

namespace SimplePlayer.MediaInfo
{
    /// <summary>
    /// Provides access to the native MediaInfo API.
    /// </summary>
    public sealed class MediaInfoLibrary : DisposingRequiredObjectBase
    {
        private readonly IntPtr handle;

        #region Constructors & Destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MediaInfoLibrary() {
            handle = MediaInfoInterop.MediaInfo_New();
        }

        /// <summary>
        /// Clean up resources according <see cref="DisposingRequiredObjectBase"/> model.
        /// </summary>
        /// <param name="isDisposing"></param>
        protected override void Dispose(bool isDisposing) {
            try {
                MediaInfoInterop.MediaInfo_Delete(handle);
            } finally {
                base.Dispose(isDisposing);
            }
        }

        #endregion

        #region Public Interfaces

        private bool isAnyFileOpened = false;

        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="FileName">Name of file.</param>
        public int OpenFile(String FileName) {
            VerifyObjectIsNotDisposed();
            //
            if (FileName == null) {
                throw new ArgumentNullException("FileName");
            }
            if (!File.Exists(FileName)) {
                throw new FileNotFoundException("File not found", FileName);
            }
            //
            if (isAnyFileOpened) {
                CloseFile();
                isAnyFileOpened = false;
            }
            int res = (int) MediaInfoInterop.MediaInfo_Open(handle, FileName);
            isAnyFileOpened = true;
            return (res);
        }

        /// <summary>
        /// Closes a file.
        /// </summary>
        public void CloseFile() {
            VerifyObjectIsNotDisposed();
            //
            if (!isAnyFileOpened) {
                throw new ApplicationException("No media is opened now.");
            }
            //
            MediaInfoInterop.MediaInfo_Close(handle);
            isAnyFileOpened = false;
        }

        /// <summary>
        /// Get inform string.
        /// </summary>
        public String Inform() {
            VerifyObjectIsNotDisposed();
            //
            return Marshal.PtrToStringUni(MediaInfoInterop.MediaInfo_Inform(handle, (IntPtr) 0));
        }

        /// <summary>
        /// Gets description string for specified parameters.
        /// </summary>
        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo, InfoKind KindOfSearch) {
            VerifyObjectIsNotDisposed();
            //
            return Marshal.PtrToStringUni(MediaInfoInterop.MediaInfo_Get(handle, (IntPtr) StreamKind,
                (IntPtr) StreamNumber, Parameter, (IntPtr) KindOfInfo, (IntPtr) KindOfSearch));
        }

        /// <summary>
        /// Gets description string for specified parameters.
        /// </summary>
        public String Get(StreamKind StreamKind, int StreamNumber, int Parameter, InfoKind KindOfInfo) {
            VerifyObjectIsNotDisposed();
            //
            return Marshal.PtrToStringUni(MediaInfoInterop.MediaInfo_GetI(handle, (IntPtr) StreamKind, (IntPtr) StreamNumber, (IntPtr) Parameter, (IntPtr) KindOfInfo));
        }

        /// <summary>
        /// Gets option string for specified parameters.
        /// </summary>
        public String Option(String Option, String Value) {
            VerifyObjectIsNotDisposed();
            //
            if (Option == null) {
                throw new ArgumentNullException("Option");
            }
            if (Value == null) {
                throw new ArgumentNullException("Value");
            }
            //
            return Marshal.PtrToStringUni(MediaInfoInterop.MediaInfo_Option(handle, Option, Value));
        }

        /// <summary>
        /// Gets state string.
        /// </summary>
        public int State_Get() {
            VerifyObjectIsNotDisposed();
            //
            return (int) MediaInfoInterop.MediaInfo_State_Get(handle);
        }

        /// <summary>
        /// Gets count of streams.
        /// </summary>
        public int Count_Get(StreamKind StreamKind, int StreamNumber) {
            VerifyObjectIsNotDisposed();
            //
            return (int) MediaInfoInterop.MediaInfo_Count_Get(handle, (IntPtr) StreamKind, (IntPtr) StreamNumber);
        }

        /// <summary>
        /// Gets description string for specified parameters.
        /// </summary>
        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo) {
            VerifyObjectIsNotDisposed();
            //
            return Get(StreamKind, StreamNumber, Parameter, KindOfInfo, InfoKind.Name);
        }

        /// <summary>
        /// Gets description string for specified parameters.
        /// </summary>
        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter) {
            VerifyObjectIsNotDisposed();
            //
            return Get(StreamKind, StreamNumber, Parameter, InfoKind.Text, InfoKind.Name);
        }

        /// <summary>
        /// Gets description string for specified parameters.
        /// </summary>
        public String Get(StreamKind StreamKind, int StreamNumber, int Parameter) {
            VerifyObjectIsNotDisposed();
            //
            return Get(StreamKind, StreamNumber, Parameter, InfoKind.Text);
        }

        /// <summary>
        /// Gets option string for specified parameters.
        /// </summary>
        public String Option(String option) {
            VerifyObjectIsNotDisposed();
            //
            if (option == null) {
                throw new ArgumentNullException("option");
            }
            //
            return Option(option, String.Empty);
        }

        /// <summary>
        /// Gets count of streams with specified type.
        /// </summary>
        public int Count_Get(StreamKind StreamKind) {
            VerifyObjectIsNotDisposed();
            //
            return Count_Get(StreamKind, -1);
        }

        #endregion
    }
}