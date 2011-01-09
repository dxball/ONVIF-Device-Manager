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

namespace odm.controls.GraphEditor {
	/// <summary>
	/// Interaction logic for marker2dEditor.xaml
	/// </summary>
	public partial class marker2dEditor : UserControl {
		public marker2dEditor() {
			InitializeComponent();

			group = new TransformGroup();
			trans = new TranslateTransform();

			group.Children.Add(trans);

			marker.RenderTransform = group;
			pointerUp.RenderTransform = group;
			pointerDown.RenderTransform = group;

			marker.MouseDown += new MouseButtonEventHandler(marker_MouseDown);
			marker.MouseMove += new MouseEventHandler(marker_MouseMove);
			marker.MouseUp += new MouseButtonEventHandler(MouseUpHandler);

			pointerUp.MouseDown += new MouseButtonEventHandler(pointerUp_MouseDown);
			pointerUp.MouseMove += new MouseEventHandler(pointerUp_MouseMove);
			pointerUp.MouseUp += new MouseButtonEventHandler(MouseUpHandler);
			pointerDown.MouseDown += new MouseButtonEventHandler(pointerDown_MouseDown);
			pointerDown.MouseMove += new MouseEventHandler(pointerDown_MouseMove);
			pointerDown.MouseUp += new MouseButtonEventHandler(MouseUpHandler);
		}
		Point top;
		Point bottom;
		public Point Top {
			get {
				return new Point(Canvas.GetLeft(pointerUp) + markerPointerR, Canvas.GetTop(pointerUp) + markerPointerR);
			}
		}
		public Point Bottom {
			get {
				return new Point(Canvas.GetLeft(pointerDown) + markerPointerR, Canvas.GetTop(pointerDown) + markerPointerR);
			}
		}
		double markerPointerR = 5;
		private TranslateTransform trans;
		private TransformGroup group;
		private Point oldPoint;
		double origOffsetLeft;
		double origOffsetTop;
		double pointerUpOffsetLeft;
		double pointerDownOffsetLeft;
		double pointerUpOffsetTop;
		double pointerDownOffsetTop;
		Rect bountRct;

		public Size physicalSize;
		public Size PhysicalSize {
			get { return physicalSize; }
			set {
				physicalSize = value;
				Refresh();
			}
		}
		void Refresh() { 

		}

		public void Init(Point p1, Point p2, Rect boundRect) {
			if (p1.Y < p2.Y) {
				top.Y = p1.Y;
				bottom.Y = p2.Y;
			} else {
				top.Y = p2.Y;
				bottom.Y = p1.Y;
			}
			if (p1.X < p2.X) {
				top.X = p1.X;
				bottom.X = p2.X;
			} else {
				top.X = p2.X;
				bottom.X = p1.X;
			}

			bountRct = boundRect;

			Canvas.SetLeft(pointerUp, top.X - markerPointerR);
			Canvas.SetTop(pointerUp, top.Y - markerPointerR);
			Canvas.SetLeft(pointerDown, bottom.X - markerPointerR);
			Canvas.SetTop(pointerDown, bottom.Y - markerPointerR);

			Canvas.SetLeft(marker, top.X);
			Canvas.SetTop(marker, top.Y);

			marker.Width = Math.Abs(bottom.X - top.X);
			marker.Height = Math.Abs(bottom.Y - top.Y);
		}

		void marker_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

				double pUl = pointerUpOffsetLeft + (newPoint.X - oldPoint.X);
				double pUt = pointerUpOffsetTop + (newPoint.Y - oldPoint.Y);
				double pDr = pointerDownOffsetLeft + (newPoint.X - oldPoint.X) + markerPointerR * 2;
				double pDb = pointerDownOffsetTop + (newPoint.Y - oldPoint.Y) + markerPointerR * 2;

				if (pUl < bountRct.X || pUt < bountRct.Y || pDb > bountRct.Height + bountRct.Y || pDr > bountRct.Width + bountRct.X) {
					return;
				}

				Canvas.SetLeft(marker, origOffsetLeft + (newPoint.X - oldPoint.X));
				Canvas.SetTop(marker, origOffsetTop + (newPoint.Y - oldPoint.Y));

				Canvas.SetLeft(pointerUp, pointerUpOffsetLeft + (newPoint.X - oldPoint.X));
				Canvas.SetTop(pointerUp, pointerUpOffsetTop + (newPoint.Y - oldPoint.Y));
				Canvas.SetLeft(pointerDown, pointerDownOffsetLeft + (newPoint.X - oldPoint.X));
				Canvas.SetTop(pointerDown, pointerDownOffsetTop + (newPoint.Y - oldPoint.Y));
			}
		}

		void marker_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(marker);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);
				origOffsetLeft = Canvas.GetLeft(marker);
				origOffsetTop = Canvas.GetTop(marker);

				pointerUpOffsetLeft = Canvas.GetLeft(pointerUp);
				pointerUpOffsetTop = Canvas.GetTop(pointerUp);
				pointerDownOffsetLeft = Canvas.GetLeft(pointerDown);
				pointerDownOffsetTop = Canvas.GetTop(pointerDown);
			}
		}
		void pointerDown_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

				double chHeigth = newPoint.Y - Canvas.GetTop(pointerUp);
				double chWidth = newPoint.X - Canvas.GetLeft(pointerUp);

				if (chWidth < 15 || chHeigth < 15)
					return;
				if (newPoint.Y >= bountRct.Height + bountRct.Y - markerPointerR || newPoint.X >= bountRct.Width + bountRct.X - markerPointerR)
					return;

				double heigth = Canvas.GetTop(pointerDown) - Canvas.GetTop(pointerUp);
				double width = Canvas.GetLeft(pointerDown) - Canvas.GetLeft(pointerUp);

				Canvas.SetTop(pointerDown, pointerDownOffsetTop + (newPoint.Y - oldPoint.Y));
				marker.Height = heigth;
				Canvas.SetLeft(pointerDown, pointerDownOffsetLeft + (newPoint.X - oldPoint.X));
				marker.Width = width;
			}
		}
		void pointerDown_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(pointerDown);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);

				pointerDownOffsetTop = Canvas.GetTop(pointerDown);
				pointerDownOffsetLeft = Canvas.GetLeft(pointerDown);
			}
		}
		void pointerUp_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

				double chHeigth = Canvas.GetTop(pointerDown) - newPoint.Y;
				double chWidth = Canvas.GetLeft(pointerDown) - newPoint.X;

				if (chWidth < 5 || chHeigth < 5)
					return;
				if (newPoint.Y < bountRct.Y || newPoint.X < bountRct.X)
					return;

				double heigth = Canvas.GetTop(pointerDown) - Canvas.GetTop(pointerUp);
				double width = Canvas.GetLeft(pointerDown) - Canvas.GetLeft(pointerUp);

				Canvas.SetTop(pointerUp, pointerUpOffsetTop + (newPoint.Y - oldPoint.Y));
				Canvas.SetTop(marker, pointerUpOffsetTop + (newPoint.Y - oldPoint.Y) + markerPointerR);
				marker.Height = heigth;
				Canvas.SetLeft(pointerUp, pointerUpOffsetLeft + (newPoint.X - oldPoint.X));
				Canvas.SetLeft(marker, pointerUpOffsetLeft + (newPoint.X - oldPoint.X) + markerPointerR);
				marker.Width = width;
			}
		}
		void pointerUp_MouseDown(object sender, MouseEventArgs e) {
			e.MouseDevice.Capture(pointerUp);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);

				pointerUpOffsetTop = Canvas.GetTop(pointerUp);
				pointerUpOffsetLeft = Canvas.GetLeft(pointerUp);
			}
		}
		void MouseUpHandler(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(null);
		}
	}
}
