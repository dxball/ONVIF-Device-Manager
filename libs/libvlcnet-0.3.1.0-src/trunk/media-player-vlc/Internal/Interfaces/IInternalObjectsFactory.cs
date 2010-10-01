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

using Common.Logging;
using DZ.MediaPlayer.Io;
using DZ.MediaPlayer.Vlc.Internal.InternalObjects;
using DZ.MediaPlayer.Vlc.Internal.Interop;

namespace DZ.MediaPlayer.Vlc.Internal.Interfaces
{
    internal interface IInternalObjectsFactory
    {
        VlcMediaInternal CreateVlcMediaInternal(MediaInput mediaInput);

        VlcMediaPlayerInternal CreateVlcMediaPlayerInternal();

        VlcLog CreateVlcLog(ILog log, ILogVerbosityManager logVerbosityManager);

        // mb extract methods like this into another common interface, for example, IGlobalVlcManager
        libvlc_instance_t GetInteropStructure();
    }
}