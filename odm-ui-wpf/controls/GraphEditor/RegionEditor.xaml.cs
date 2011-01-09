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
	/// Interaction logic for RegionEditor.xaml
	/// </summary>
	public partial class RegionEditor : UserControl {
		public RegionEditor() {
			InitializeComponent();

			group = new TransformGroup();
			trans = new TranslateTransform();
			group.Children.Add(trans);
		}

		List<Ellipse> elipsesList = new List<Ellipse>();
		List<Point> pointsList = new List<Point>();
		List<Line> linesList = new List<Line>();

		double markerPointerR = 5;
		private TranslateTransform trans;
		private TransformGroup group;
		private Point oldPoint;
		double origOffsetLeft;
		double origOffsetTop;
		Rect bountRct;
		int maxPoints = 10;
		int minPoints = 3;

		public void Init(List<Point> plst, Rect bound) {
			elipsesList.Clear();
			pointsList.Clear();
			pointsList.AddRange(plst);
			region.Points = new PointCollection(pointsList);

			region.MouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => {
				var hit = e.GetPosition((Polygon)sender);
				var s = VisualTreeHelper.HitTest((Polygon)sender, hit);
			});

			linesList = CreateLinesList(plst);
			linesList.ForEach(x => { mcanvas.Children.Add(x); });
			
			plst.ForEach(x => {
				var ell = CresteElipse(x);
				elipsesList.Add(ell);
				mcanvas.Children.Add(ell);
			});

			bountRct = bound;
		}
		void PointAdded() {
			region.Points.Clear();
			linesList.ForEach(x => {
				mcanvas.Children.Remove(x);
			});
			elipsesList.ForEach(x => {
				mcanvas.Children.Remove(x);
			});
			linesList.Clear();
			elipsesList.Clear();

			linesList = CreateLinesList(pointsList);
			linesList.ForEach(x => { mcanvas.Children.Add(x); });
			pointsList.ForEach(x => {
				region.Points.Add(x);
			});
			pointsList.ForEach(x => {
				var ell = CresteElipse(x);
				elipsesList.Add(ell);
				mcanvas.Children.Add(ell);
			});
		}
		void RefreshLines() {
			region.Points.Clear();
			linesList.ForEach(x => {
				mcanvas.Children.Remove(x);
			});
			elipsesList.ForEach(x => {
				mcanvas.Children.Remove(x);
			});
			linesList.Clear();
			pointsList.Clear();

			elipsesList.ForEach(x => {
				Point pt = new Point(Canvas.GetLeft(x) + markerPointerR, Canvas.GetTop(x) + markerPointerR);
				region.Points.Add(pt);
				pointsList.Add(pt);
			});

			linesList = CreateLinesList(pointsList);
			linesList.ForEach(x => { mcanvas.Children.Add(x); });
			elipsesList.ForEach(x => { mcanvas.Children.Add(x); });
		}
		List<Line> CreateLinesList(List<Point> plst) {
			List<Line> linesLst = new List<Line>();
			for (int x = 1; x < plst.Count; x++) {
				if (x == plst.Count - 1) {
					linesLst.Add(CreateLine(plst[x - 1], plst[x]));
					linesLst.Add(CreateLine(plst[x],plst[0]));
				} else
					linesLst.Add(CreateLine(plst[x - 1], plst[x]));
			}
			return linesLst;
		}
		Line CreateLine(Point p1, Point p2) {
			Line ln = new Line();
			ln.Style = (Style)FindResource("RegionLineStyle");
			ln.X1 = p1.X;
			ln.X2 = p2.X;
			ln.Y1 = p1.Y;
			ln.Y2 = p2.Y;
			ln.RenderTransform = group;
			ln.MouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => {
				if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2) {
					if (pointsList.Count >= maxPoints)
						return;
					Point nPoint = e.GetPosition(this);
					Point lpt = new Point(((Line)sender).X2, ((Line)sender).Y2);
					Point tmp = pointsList.Where(x => x.X == lpt.X && x.Y == lpt.Y ).FirstOrDefault();
					int ind = pointsList.IndexOf(tmp);
					pointsList.Insert(ind, nPoint);
					PointAdded();
				}
			});
			return ln;
		}

		Ellipse CresteElipse(Point pt) {
			Ellipse ell = new Ellipse();
			ell.Style = (Style)FindResource("TrackerPointer");
			Canvas.SetTop(ell, pt.Y - markerPointerR);
			Canvas.SetLeft(ell, pt.X - markerPointerR);
			ell.Width = markerPointerR * 2;
			ell.Height = markerPointerR * 2;
			ell.RenderTransform = group;
			ell.Focusable = true;
			ell.Tag = pt;

			ell.MouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => { 
				e.MouseDevice.Capture((Ellipse)sender);
				if (e.LeftButton == MouseButtonState.Pressed) {
					oldPoint = e.GetPosition(this);
					origOffsetLeft = Canvas.GetLeft((Ellipse)sender);
					origOffsetTop = Canvas.GetTop((Ellipse)sender);

					origOffsetLeft = Canvas.GetLeft((Ellipse)sender);
					origOffsetTop = Canvas.GetTop((Ellipse)sender);
				}
				if (e.RightButton == MouseButtonState.Pressed && e.ClickCount ==2) {
					if (pointsList.Count <= minPoints)
						return;
					Ellipse elps = (Ellipse)sender;
					pointsList.Remove((Point)elps.Tag);
					PointAdded();
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
					Canvas.SetLeft(ellipse, origOffsetLeft + (newPoint.X - oldPoint.X));
					Canvas.SetTop(ellipse, origOffsetTop + (newPoint.Y - oldPoint.Y));
					RefreshLines();
				}
			});
			

			return ell;
		}

		public List<Point> GetRegion() {
			return pointsList;
		}
	}
}
