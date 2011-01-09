using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace odm.controls.regionEditor {
	public class RegionUtils {
		// Predefined brushes
		public static Brush redBrush = new SolidBrush(Color.Red);
		public static Brush redTransparentBrush = new SolidBrush(Color.FromArgb(50, Color.Red));
		public static Brush greenBrash = new SolidBrush(Color.LightGreen);
		public static Brush greenTransparentBrash = new SolidBrush(Color.FromArgb(150, Color.LightGreen));
		public static Brush yellowBrush = new SolidBrush(Color.Yellow);
		public static Brush yellowTransparentBrush = new SolidBrush(Color.FromArgb(50, Color.Yellow));
		public static Brush whiteBrush = new SolidBrush(Color.White);
		public static Brush whiteTransparentBrush = new SolidBrush(Color.FromArgb(50, Color.White));

		//Defined Pens
		public static Pen redPen = new Pen(Color.Red);
		public static Pen greenPen = new Pen(Color.Green);
		public static Pen yellowPen = new Pen(Color.Yellow);
		public static Pen whitePet = new Pen(Color.White);
		public static Pen blackPen = new Pen(Color.Black);

		/// <summary>
		/// size of "Round" area for two points
		/// </summary>
		public static int _pointRoundArea = 10;

		public static List<Point> ToList_Scr(List<Point> streamNodesList, Rectangle ClientRect, Size Resolution) {
			return streamNodesList.Select(p => {
				return StreamToScreen(p, ClientRect, Resolution);
			}).ToList();
		}
		public static Point StreamToScreen(Point val, Rectangle ClientRect, Size Resolution) {
			double kx = ((double)ClientRect.Width) / ((double)Resolution.Width);
			double ky = ((double)ClientRect.Height) / ((double)Resolution.Height);
			return new Point((int)(((double)val.X) * kx + (double)ClientRect.X), (int)(((double)val.Y) * ky + (double)ClientRect.Y));
		}
		public static Point ScreenToStream(Point val, Rectangle ClientRect, Size Resolution) {
			double kx = ((double)ClientRect.Width) / ((double)Resolution.Width);
			double ky = ((double)ClientRect.Height) / ((double)Resolution.Height);
			return new Point((int)(((double)val.X) / kx - (double)ClientRect.X / kx), (int)(((double)val.Y) / ky - (double)ClientRect.Y / ky));
		}

		public static Rectangle RectToScreen(Rectangle rect, Rectangle ClientRect, Size Resolution) {
			Point lu = RegionUtils.StreamToScreen(new Point(rect.X, rect.Y), ClientRect, Resolution);
			Point rd = RegionUtils.StreamToScreen(new Point(rect.X + rect.Width, rect.Y + rect.Height), ClientRect, Resolution);

			Rectangle nrect = new Rectangle(lu.X, lu.Y, rd.X - lu.X, rd.Y - lu.Y);
			return nrect;
		}

		/// <summary>
		/// Method that indicated if point is in "round" on other point
		/// </summary>
		/// <param name="p1">first point</param>
		/// <param name="p2">second point</param>
		/// <returns></returns>
		public static bool IsPointInRound(Point p1, Point p2) {
			int dx = Math.Abs(p1.X - p2.X);
			int dy = Math.Abs(p1.Y - p2.Y);
			if (dx < _pointRoundArea && dy < _pointRoundArea)
				return true;
			else
				return false;
		}
		/// <summary>
		/// Find point in Nodes list which are in "round" of input point
		/// </summary>
		/// <param name="point">Mouse click point</param>
		/// <returns>Node in nodes list</returns>
		public static Point FindPoint(Point point, List<Point> NodesList) {
			Point np = new Point(-1, -1);
			if (!IfValid(point, NodesList))
				np = NodesList.Where(x => { return IsPointInRound(x, point); }).First();
			return np;
		}
		/// <summary>
		/// Return value indicated if point is one of the points in Nodes list
		/// </summary>
		/// <param name="point">Input point</param>
		/// <returns>true/false</returns>
		public static bool IfValid(Point point, List<Point> NodesList) {
			return !NodesList.Where(x => { return IsPointInRound(x, point); }).Any();
		}
		public static bool CheckIfInRectangle(Point point, Rectangle Rect) {
			if (point.X > Rect.X && point.X < Rect.X + Rect.Width && point.Y > Rect.Y && point.Y < Rect.Y + Rect.Height) {
				return true;
			}
			return false;
		}
		public static bool CheckIfInRoundRectangle(Point point, Rectangle Rect) {
			if (point.X > Rect.X - 3 && point.X < Rect.X + Rect.Width + 3 && point.Y > Rect.Y - 3 && point.Y < Rect.Y + Rect.Height + 3) {
				return true;
			}
			return false;
		}
		public static double dot(Point u, Point v) {
			return ((u).X * (v).X + (u).Y * (v).Y);
		}
		public static double norm(Point v) {
			return Math.Sqrt(dot(v, v));
		}
		public static double d(Point u, Point v) {
			return norm(new Point(u.X - v.X, u.Y - v.Y));
		}
		public static double distance_Point_to_Segment(Point P, Line S) {
			Point v = new Point(S.End.X - S.Begin.X, S.End.Y - S.Begin.Y);
			Point w = new Point(P.X - S.Begin.X, P.Y - S.Begin.Y);

			double c1 = dot(w, v);
			if (c1 <= 0)
				return d(P, S.Begin);

			double c2 = dot(v, v);
			if (c2 <= c1)
				return d(P, S.End);

			double b = c1 / c2;
			//Point Pb = new Point( P0 + b * v;
			Point Pb = new Point((S.Begin.X + (int)(b * v.X)), (S.Begin.Y + (int)(b * v.Y)));
			return d(P, Pb);
		}
		public static Line IfOnLine(Point point, List<Line> lines) {
			Line retVal = null;
			double dist;
			foreach (var line in lines) {
				dist = Math.Abs(distance_Point_to_Segment(point, line));
				if (dist < 4) {
					retVal = line;
					return retVal;
				}
			}
			return retVal;
		}
		public static List<Line> SplitToLines(List<Point> NodesList) {
			var lines = new List<Line>();
			var points = NodesList.ToList();
			points.Add(points.First());
			bool isFirst = true;
			Point previos = Point.Empty;
			foreach (var point in points) {
				if (isFirst) {
					isFirst = false;
				} else {
					lines.Add(new Line(previos, point));
				}
				previos = point;
			}
			return lines;
		}
		public static bool transection(long ax1, long ay1, long ax2, long ay2, long bx1, long by1, long bx2, long by2) {
			long v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
			long v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
			long v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
			long v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
			return ((v1 * v2 <= 0) && (v3 * v4 <= 0));
		}
		public static bool CheckTwoLinesIntersected(Line line1, Line line2) {
			return transection(line1.Begin.X, line1.Begin.Y, line1.End.X, line1.End.Y, line2.Begin.X, line2.Begin.Y, line2.End.X, line2.End.Y);
		}
		public static bool CheckLinesIntersected(Line line1, List<Line> lines) {
			List<Line> tempList = new List<Line>();

			int ind = lines.FindIndex(x => x == line1);
			int prevInd;
			if (ind == 0)
				prevInd = lines.Count - 1;
			else
				prevInd = ind - 1;
			int nextInd;
			if (ind == lines.Count - 1)
				nextInd = 0;
			else
				nextInd = ind + 1;
			for (int i = 0; i < lines.Count; i++) {
				if (ind != i && prevInd != i && nextInd != i)
					tempList.Add(lines[i]);

			}
			foreach (var line2 in tempList) {
				if (CheckTwoLinesIntersected(line1, line2))
					return true;
			}
			return false;
		}
		public static bool IsIntersect(List<Point> NodesList) {
			List<Line> lines = SplitToLines(NodesList);
			foreach (var val in lines) {
				if (CheckLinesIntersected(val, lines))
					return true;
			}
			return false;
		}
		public static bool CheckIntersection(List<Point> NodesList) {
			bool isIntersect = false;
			if (NodesList.Count > 3 && IsIntersect(NodesList)) {
				isIntersect = true;
			}
			return isIntersect;
		}
	}

	public class Line {
		public Line() { }
		public Line(Point beg, Point end) { Begin = beg; End = end; }
		public Point Begin;
		public Point End;
	}
	public class HMarker {
		public Point P1;
		public Point P2;
		public int pheight;
		public int pwidth;
	}
}
