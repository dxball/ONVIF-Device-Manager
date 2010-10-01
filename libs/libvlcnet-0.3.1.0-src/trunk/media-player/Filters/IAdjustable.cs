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

namespace DZ.MediaPlayer.Filters {
	/// <summary>
	/// Defines interface to control brightness, contrast and another parameters of video.
	/// This interface should be implemented by player.
	/// </summary>
	public interface IAdjustable : IFilterBase {
		/// <summary>
		/// Is parameters can be adjusted.
		/// </summary>
		bool IsFilterAvailable {
			get;
		}

		/// <summary>
        /// Brightness (0.0 - 2.0)
		/// </summary>
		float Brightness {
			get;
			set;
		}

		/// <summary>
        /// Contrast (0.0 - 2.0)
		/// </summary>
		float Contrast {
			get;
			set;
		}

		/// <summary>
        /// Gamma (0.01 - 10.0)
		/// </summary>
		float Gamma {
			get;
			set;
		}

		/// <summary>
        /// Hue (0 - 360)
		/// </summary>
		int Hue {
			get;
			set;
		}

		/// <summary>
        /// Saturation (0.0 - 3.0)
		/// </summary>
		float Saturation {
			get;
			set;
		}

        /// <summary>
        /// Event messages about filter has been successfully loaded into domain.
        /// </summary>
        event AdjustFilterLoadedHandler FilterLoaded;
	}

    /// <summary>
    /// Event dispatcher delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void AdjustFilterLoadedHandler(object sender, AdjustFilterLoadedHandlerArgs args);

    /// <summary>
    /// Represents parameters of according event.
    /// </summary>
    public class AdjustFilterLoadedHandlerArgs : EventArgs
    {
    }
}