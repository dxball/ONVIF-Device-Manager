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

namespace DZ.MediaPlayer.Vlc
{
    /// <summary>
    /// Instantiates windows based on generic type parameter. Window
    /// type should have default contstructor.
    /// </summary>
    /// <typeparam name="TWindowType">Type of windows to instantiate.</typeparam>
    public class SimpleVlcDoubleWindowFactory<TWindowType> : VlcDoubleWindowFactory
        where TWindowType : DoubleWindowBase, new()
    {

        /// <summary>
        /// Creates window derived from <see cref="DoubleWindowBase"/> which
        /// can provide additional information for vlc.
        /// </summary>
        /// <returns>Instance of window</returns>
        public override DoubleWindowBase CreateWindow() {
            return new TWindowType();
        }
    }
}