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
using System.Windows;

namespace odm.ui.controls.GraphEditor {
	public class EditorConverter {
		public static Rect StreamToScreenR(Rect val, Rect ClientRect, Size Resolution){
			Point topleft = StreamToScreen(val.TopLeft, ClientRect, Resolution);
			Point bottright = StreamToScreen(val.BottomRight, ClientRect, Resolution);
			return new Rect(topleft, bottright);
		}
		public static Rect ScreenToStreamR(Rect val, Rect ClientRect, Size Resolution) {
			Point topleft = ScreenToStream(val.TopLeft, ClientRect, Resolution);
			Point bottright = ScreenToStream(val.BottomRight, ClientRect, Resolution);
			return new Rect(topleft, bottright);
		}
		public static Point StreamToScreen(Point val, Rect ClientRect, Size Resolution) {
			double kx = ClientRect.Width / Resolution.Width;
			double ky = ClientRect.Height / Resolution.Height;
			return new Point(val.X * kx + ClientRect.X, val.Y * ky + ClientRect.Y);
		}
		public static Point ScreenToStream(Point val, Rect ClientRect, Size Resolution) {
			double kx = ClientRect.Width / Resolution.Width;
			double ky = ClientRect.Height / Resolution.Height;
			return new Point(val.X / kx - ClientRect.X / kx, val.Y / ky - ClientRect.Y / ky);
		}
		public static Rect FromWinForms(System.Drawing.Rectangle rect) {
			return new Rect(rect.Left, rect.Top, rect.Width, rect.Height);
		}
		public static System.Drawing.Rectangle ToWinForms(Rect rect) {
			return new System.Drawing.Rectangle((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
		}
		public static Rect GetVideoBounds(Rect clientRect, Rect videoRect) {
			Rect r = new Rect();

			double kx = clientRect.Width / videoRect.Width;
			double ky = clientRect.Height / videoRect.Height;

			if (ky > kx) {
				var h = videoRect.Height * kx;
				r.Width = clientRect.Width;
				r.Height = h;
				r.X = 0;
				r.Y = ((clientRect.Height - h) * 0.5);
				return r;
			}

			if (kx > ky) {
				var w = videoRect.Width * ky;
				r.Width = w;
				r.Height = clientRect.Height;
				r.X = ((clientRect.Width - w) * 0.5);
				r.Y = 0;
				return r;
			}

			r.Width = clientRect.Width;
			r.Height = clientRect.Height;
			r.X = 0;
			r.Y = 0;

			return r;
		}
	}
}