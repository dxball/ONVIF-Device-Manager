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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace odm.ui.controls.GraphEditor {
	/// <summary>
	/// Interaction logic for RectangleEditor.xaml
	/// </summary>
	public partial class RectangleEditor : UserControl {
		public RectangleEditor() {
			InitializeComponent();

			group = new TransformGroup();
			trans = new TranslateTransform();
			scale = new ScaleTransform();
			group.Children.Add(trans);
			group.Children.Add(scale);
		}
		Ellipse topLeft;
		Ellipse topRight;
		Ellipse bottomLeft;
		Ellipse bottomRight;
		Rect crop;
		Rect bountRct;
		private TranslateTransform trans;
		private ScaleTransform scale;
		private TransformGroup group;
		double markerPointerR = 5;
		private Point oldPoint;
		double origOffsetLeft;
		double origOffsetTop;

		public void Init(Rect croppingRect, Rect bound) {
			bountRct = bound;
			crop = croppingRect;

			topLeft = CresteElipse(croppingRect.TopLeft);
			topRight = CresteElipse(croppingRect.TopRight);
			bottomRight = CresteElipse(croppingRect.BottomRight);
			bottomLeft = CresteElipse(croppingRect.BottomLeft);

			InitEllipses(crop, bound);
			
			RefreshRect();
			AddEllipses();
		}
		void InitEllipses(Rect croppingRect, Rect bound) {
			topLeft.Tag = new EllTag() { point = croppingRect.TopLeft, pos = EllTag.position.TOPLEFT };
			Canvas.SetTop(topLeft, croppingRect.TopLeft.Y - markerPointerR);
			Canvas.SetLeft(topLeft, croppingRect.TopLeft.X - markerPointerR);

			topRight.Tag = new EllTag() { point = croppingRect.TopRight, pos = EllTag.position.TOPRIGHT };
			Canvas.SetTop(topRight, croppingRect.TopRight.Y - markerPointerR);
			Canvas.SetLeft(topRight, croppingRect.TopRight.X - markerPointerR);

			bottomRight.Tag = new EllTag() { point = croppingRect.BottomRight, pos = EllTag.position.BOTTOMRIGHT };
			Canvas.SetTop(bottomRight, croppingRect.BottomRight.Y - markerPointerR);
			Canvas.SetLeft(bottomRight, croppingRect.BottomRight.X - markerPointerR);

			bottomLeft.Tag = new EllTag() { point = croppingRect.BottomLeft, pos = EllTag.position.BOTTOMLEFT };
			Canvas.SetTop(bottomLeft, croppingRect.BottomLeft.Y - markerPointerR);
			Canvas.SetLeft(bottomLeft, croppingRect.BottomLeft.X - markerPointerR);
		}
		public void ClearEllipses() {
			if (topLeft != null && topRight != null && bottomRight != null && bottomLeft != null) {
				mcanvas.Children.Remove(topLeft);
				mcanvas.Children.Remove(topRight);
				mcanvas.Children.Remove(bottomRight);
				mcanvas.Children.Remove(bottomLeft);
			}
		}
		void AddEllipses() {
			mcanvas.Children.Add(topLeft);
			mcanvas.Children.Add(topRight);
			mcanvas.Children.Add(bottomRight);
			mcanvas.Children.Add(bottomLeft);
		}

		Ellipse CresteElipse(Point pt) {
			Ellipse ell = new Ellipse();
			ell.Style = (Style)FindResource("RectanglePointer");
			Canvas.SetTop(ell, pt.Y - markerPointerR);
			Canvas.SetLeft(ell, pt.X - markerPointerR);
			ell.Width = markerPointerR * 2;
			ell.Height = markerPointerR * 2;
			ell.RenderTransform = group;
			ell.Focusable = true;

			ell.MouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => {
				e.MouseDevice.Capture((Ellipse)sender);
				if (e.LeftButton == MouseButtonState.Pressed) {
					oldPoint = e.GetPosition(this);
					origOffsetLeft = Canvas.GetLeft((Ellipse)sender);
					origOffsetTop = Canvas.GetTop((Ellipse)sender);

					origOffsetLeft = Canvas.GetLeft((Ellipse)sender);
					origOffsetTop = Canvas.GetTop((Ellipse)sender);
				}
			});
			ell.MouseUp += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => {
				e.MouseDevice.Capture(null);
			});
			ell.MouseMove += new MouseEventHandler((object sender, MouseEventArgs e) => {
				if (e.LeftButton == MouseButtonState.Pressed) {
					Point newPoint = e.GetPosition(this);

					double pUl = origOffsetLeft + (newPoint.X - oldPoint.X);
					double pUt = origOffsetTop + (newPoint.Y - oldPoint.Y);
					double pDr = origOffsetLeft + (newPoint.X - oldPoint.X) + markerPointerR * 2;
					double pDb = origOffsetTop + (newPoint.Y - oldPoint.Y) + markerPointerR * 2;

					if (pUl < bountRct.X || pUt < bountRct.Y || pDb > bountRct.Height + bountRct.Y || pDr > bountRct.Width + bountRct.X) {
						return;
					}

					var ellipse = (Ellipse)sender;
					switch (((EllTag)ellipse.Tag).pos) {
						case EllTag.position.TOPLEFT:
							Canvas.SetLeft(ellipse, origOffsetLeft + (newPoint.X - oldPoint.X));
							Canvas.SetTop(ellipse, origOffsetTop + (newPoint.Y - oldPoint.Y));

							Canvas.SetLeft(bottomLeft, origOffsetLeft + (newPoint.X - oldPoint.X));
							Canvas.SetTop(topRight, origOffsetTop + (newPoint.Y - oldPoint.Y));
							break;
						case EllTag.position.TOPRIGHT:
							Canvas.SetLeft(ellipse, origOffsetLeft + (newPoint.X - oldPoint.X));
							Canvas.SetTop(ellipse, origOffsetTop + (newPoint.Y - oldPoint.Y));

							Canvas.SetLeft(bottomRight, origOffsetLeft + (newPoint.X - oldPoint.X));
							Canvas.SetTop(topLeft, origOffsetTop + (newPoint.Y - oldPoint.Y));
							break;
						case EllTag.position.BOTTOMLEFT:
							Canvas.SetLeft(ellipse, origOffsetLeft + (newPoint.X - oldPoint.X));
							Canvas.SetTop(ellipse, origOffsetTop + (newPoint.Y - oldPoint.Y));

							Canvas.SetLeft(topLeft, origOffsetLeft + (newPoint.X - oldPoint.X));
							Canvas.SetTop(bottomRight, origOffsetTop + (newPoint.Y - oldPoint.Y));
							break;
						case EllTag.position.BOTTOMRIGHT:
							Canvas.SetLeft(ellipse, origOffsetLeft + (newPoint.X - oldPoint.X));
							Canvas.SetTop(ellipse, origOffsetTop + (newPoint.Y - oldPoint.Y));

							Canvas.SetLeft(topRight, origOffsetLeft + (newPoint.X - oldPoint.X));
							Canvas.SetTop(bottomLeft, origOffsetTop + (newPoint.Y - oldPoint.Y));
							break;
					}
					RefreshRect();
				}
			});
			return ell;
		}
		public void Resize(Rect brect, Size Res) {
			//save current crop;
			double left = Canvas.GetLeft(rectangle);
			double top = Canvas.GetTop(rectangle);
			crop = new Rect(left, top, rectangle.Width, rectangle.Height);
			crop = EditorConverter.ScreenToStreamR(crop, bountRct, Res);

			crop = EditorConverter.StreamToScreenR(crop, brect, Res);

			InitEllipses(crop, brect);
			RefreshRect();

			bountRct = brect;
		}
		void RefreshRect() {
			double Up;
			double Down;
			double Left;
			double Right;

			if (Canvas.GetTop(topLeft) < Canvas.GetTop(bottomRight)) {
				Up = Canvas.GetTop(topLeft);
				Down = Canvas.GetTop(bottomRight);
			} else {
				Up = Canvas.GetTop(bottomRight);
				Down = Canvas.GetTop(topLeft);
			}
			if (Canvas.GetLeft(topLeft) < Canvas.GetLeft(bottomRight)) {
				Left = Canvas.GetLeft(topLeft);
				Right = Canvas.GetLeft(bottomRight);
			} else {
				Left = Canvas.GetLeft(bottomRight);
				Right = Canvas.GetLeft(topLeft);
			}

			Canvas.SetTop(rectangle, Up + markerPointerR);
			Canvas.SetLeft(rectangle, Left + markerPointerR);
			rectangle.Width = Math.Abs(Right - Left);
			rectangle.Height = Math.Abs(Down - Up);
		}
		public Rect GetCroppingRect() {
			return new Rect(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle), rectangle.Width, rectangle.Height);
		}
	}

	public class EllTag {
		public Point point { get; set; }
		public enum position { 
			TOPLEFT,
			TOPRIGHT,
			BOTTOMLEFT,
			BOTTOMRIGHT
		}
		public position pos { get; set; }
	}
}
