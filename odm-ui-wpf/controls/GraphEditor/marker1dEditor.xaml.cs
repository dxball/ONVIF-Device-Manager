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
	/// Interaction logic for marker1dEditor.xaml
	/// </summary>
	public partial class marker1dEditor : UserControl {
		public marker1dEditor() {
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
		double markerWidth = 40;
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

		public void Init(Point p1, Point p2, Rect boundRect) {
			if (p1.Y < p2.Y) {
				top = p1;
				bottom = p2;
			} else {
				top = p2;
				bottom = p1;
			}
			
			bountRct = boundRect;

			Canvas.SetLeft(pointerUp, top.X - markerPointerR);
			Canvas.SetTop(pointerUp, top.Y - markerPointerR);
			Canvas.SetLeft(pointerDown, bottom.X - markerPointerR);
			Canvas.SetTop(pointerDown, bottom.Y - markerPointerR);

			Canvas.SetLeft(marker, top.X - markerWidth/2);
			Canvas.SetTop(marker, top.Y);
			
			marker.Width = markerWidth;
			marker.Height = Math.Abs(bottom.Y - top.Y);
		}

		void Refresh() { 

		}

		void marker_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

				double pUl = pointerUpOffsetLeft + (newPoint.X - oldPoint.X);
				double pUt = pointerUpOffsetTop + (newPoint.Y - oldPoint.Y);
				double pDr = pointerDownOffsetLeft + (newPoint.X - oldPoint.X) + markerPointerR*2;
				double pDb = pointerDownOffsetTop + (newPoint.Y - oldPoint.Y) + markerPointerR*2;

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

				if (chHeigth < 15)
					return;
				if (newPoint.Y >= bountRct.Height + bountRct.Y - markerPointerR)
					return;


				Canvas.SetTop(pointerDown, pointerDownOffsetTop + (newPoint.Y - oldPoint.Y));
				marker.Height = Canvas.GetTop(pointerDown) - Canvas.GetTop(pointerUp);
			}
		}
		void pointerDown_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(pointerDown);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);

				pointerDownOffsetTop = Canvas.GetTop(pointerDown);
			}
		}
		void pointerUp_MouseMove(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				Point newPoint = e.GetPosition(this);

				double chHeigth = Canvas.GetTop(pointerDown) - newPoint.Y;

				if (chHeigth < 5)
					return;
				if (newPoint.Y < bountRct.Y)
					return;

				Canvas.SetTop(pointerUp, pointerUpOffsetTop + (newPoint.Y - oldPoint.Y));
				Canvas.SetTop(marker, pointerUpOffsetTop + (newPoint.Y - oldPoint.Y) + markerPointerR);

				marker.Height = Canvas.GetTop(pointerDown) - Canvas.GetTop(pointerUp);
			}
		}
		void pointerUp_MouseDown(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(pointerUp);

			if (e.LeftButton == MouseButtonState.Pressed) {
				oldPoint = e.GetPosition(this);

				pointerUpOffsetTop = Canvas.GetTop(pointerUp);
			}
		}
		void MouseUpHandler(object sender, MouseButtonEventArgs e) {
			e.MouseDevice.Capture(null);
		}
	}
}
