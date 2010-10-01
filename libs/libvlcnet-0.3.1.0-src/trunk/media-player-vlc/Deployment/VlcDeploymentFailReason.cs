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

namespace DZ.MediaPlayer.Vlc.Deployment
{
    /// <summary>
    /// Deployment fail reason flags.
    /// </summary>
    [Flags]
    public enum VlcDeploymentFailReason {
        /// <summary>
        /// Successfully processed.
        /// </summary>
        None = 0,
        /// <summary>
        /// Required and existing library version differs.
        /// </summary>
        LibraryVersionDiffers = 1,
        /// <summary>
        /// Library exists, but cannot be loaded.
        /// </summary>
        LibraryCannotBeLoaded = 2,
        /// <summary>
        /// Not all required files deployed.
        /// </summary>
        NotAllFilesDeployed = 4,
        /// <summary>
        /// Library does not exists.
        /// </summary>
        EmptyDeployment = 8,
        /// <summary>
        /// Invalid hash of file from deployment
        /// package.
        /// </summary>
        InvalidHashOfFile = 16
    }
}