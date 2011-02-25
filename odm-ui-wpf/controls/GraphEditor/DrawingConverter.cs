#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace odm.ui.controls.GraphEditor {
	public class DrawingConverter {
		public static System.Drawing.Point PointWpfToForm(System.Windows.Point inPoint) {
			return new System.Drawing.Point((int)inPoint.X, (int)inPoint.Y);
		}
		public static System.Windows.Point PointFormToWpf(System.Drawing.Point inPoint) {
			return new System.Windows.Point(inPoint.X, inPoint.Y);
		}
		public static System.Drawing.Rectangle RectToRectangle(System.Windows.Rect inRect) {
			return new System.Drawing.Rectangle(PointWpfToForm(inRect.TopLeft), new System.Drawing.Size((int)inRect.Width,(int)inRect.Height));
		}
		public static System.Windows.Rect RectangleToRect(System.Drawing.Rectangle inRect) {
			return new System.Windows.Rect(PointFormToWpf(inRect.Location), new System.Windows.Point(inRect.Right, inRect.Bottom));
		}
	}
}
