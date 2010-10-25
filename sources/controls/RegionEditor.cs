using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using liblenin;

namespace nvc.controls {
	public class GraphEditor {
		public GraphEditor(Rectangle resolution) {
			_mediaResolution = resolution;
			List<BaseRegion> _regions = new List<BaseRegion>();
		}
		public void SetParent(VlcControlInner parent) {
			_parent = parent;
		}

		void SybsvribeToEvents() {
			_parent.MouseDoubleClick += new MouseEventHandler(_parent_MouseDoubleClick);
			_parent.MouseDown += new MouseEventHandler(_parent_MouseDown);
			_parent.MouseUp += new MouseEventHandler(_parent_MouseUp);
			_parent.MouseMove += new MouseEventHandler(_parent_MouseMove);
			_parent.MouseClick += new MouseEventHandler(_parent_MouseClick);
		}

		void AddRegion(BaseRegion reg) {
			Regions.Add(reg);
		}
		List<BaseRegion> Regions{
			get {
				if (_regions == null) {
					_regions = new List<BaseRegion>();
					SybsvribeToEvents();
				}
				return _regions;
			}
		}
		public void AddHeightMarker(Point top, Point bottom, int physicalHeight) {
			HeightMarkerRegion hReg = new HeightMarkerRegion(top, bottom, physicalHeight) { Parent = _parent };
			hReg.ClientRect = _parent.ClientRectangle;
			hReg.Resolution = _mediaResolution.Size;
			AddRegion(hReg);
		}
		public void AddRegionEditor(List<Point> plst) {
			if (plst == null) {
				plst = new List<Point>();
				plst.Add(new Point { X = 10, Y = 30 });
				plst.Add(new Point { X = 50, Y = 30 });
				plst.Add(new Point { X = 50, Y = 70 });
				plst.Add(new Point { X = 10, Y = 70 });
			}
			TrackerRegion CurrentRegion = new TrackerRegion(plst) { Parent = _parent };
			CurrentRegion.ClientRect = _parent.ClientRectangle;
			CurrentRegion.Resolution = _mediaResolution.Size;

			AddRegion(CurrentRegion);
		}

		public List<HMarker> GetMarkers() {
			List<HMarker> lst = new List<HMarker>();
			Regions.Where(c => c.GetMarker() != null).ForEach(x=> lst.Add(x.GetMarker()));
			return lst;
		}

		public List<Point> GetRegion() {			
			return Regions.Where(x => {
				if (x.GetRegion() != null)
					return true;
				return false;
			}).FirstOrDefault().GetRegion();
		}
		public void ReleaseAll() {

		}

		Rectangle _mediaResolution;
		VlcControlInner _parent;
		List<BaseRegion> _regions;

		void _parent_MouseClick(object sender, MouseEventArgs e) {
			Regions.ForEach(x => x.mouseclick(e, _parent.PointToScreen(e.Location)));
		}
		void _parent_MouseDoubleClick(object sender, MouseEventArgs e) {
			Regions.ForEach(x => x.mousedclick(e, _parent.PointToScreen(e.Location)));
		} 
		void _parent_MouseDown(object sender, MouseEventArgs e) {
			Regions.ForEach(x => x.mousedown(e));
		}
		void _parent_MouseMove(object sender, MouseEventArgs e) {
			Regions.ForEach(x => x.mousemove(e));
		}
		void _parent_MouseUp(object sender, MouseEventArgs e) {
			Regions.ForEach(x => x.mouseup(e));
		}

		public void FillBitmap(Graphics graph, Rectangle videoRect){
			Regions.ForEach(x => { 
				x.ClientRect = videoRect; 
				x.draw(graph); 
			});
		}
	}
	public class Line {
		public Line() { }
		public Line(Point beg, Point end) { Begin = beg; End = end; }
		public Point Begin;
		public Point End;
	}
	public class HMarker {
		public Point top;
		public Point bottom;
		public int pheight;
	}

	public abstract class BaseRegion {
		public BaseRegion() {
			_clientRect = Rectangle.Empty;

			//Brushes and pens
			//Red
			redBrush = new SolidBrush(Color.Red);
			redTransparentBrush = new SolidBrush(Color.FromArgb(50, Color.Red));
			redPen = new Pen(redBrush);
			//green
			greenBrash = new SolidBrush(Color.LightGreen);
			greenTransparentBrash = new SolidBrush(Color.FromArgb(150, Color.LightGreen));
			greenPen = new Pen(greenBrash);
			//yellow
			yellowBrush = new SolidBrush(Color.Yellow);
			yellowTransparentBrush = new SolidBrush(Color.FromArgb(50, Color.Yellow));
			yellowPen = new Pen(yellowTransparentBrush);
		}
		protected int margin = 4;
		protected int doublem = 8;

		public Control Parent{get; set;}

		protected bool _IsMouseCatched = false;

		protected Brush redBrush;
		protected Brush redTransparentBrush;
		protected Brush greenBrash;
		protected Brush greenTransparentBrash;
		protected Brush yellowBrush;
		protected Brush yellowTransparentBrush;
		protected Pen redPen;
		protected Pen greenPen;
		protected Pen yellowPen;

		public abstract void draw(Graphics graph);
		public abstract void mouseup(MouseEventArgs e);
		public abstract void mousedown(MouseEventArgs e);
		public abstract void mouseclick(MouseEventArgs e, Point pt);
		public abstract void mousedclick(MouseEventArgs e, Point pt);
		public abstract void mousemove(MouseEventArgs e);
		public abstract List<Point> GetRegion();
		public abstract HMarker GetMarker();

		protected Rectangle _clientRect;
		public Rectangle ClientRect {
			get {
				return _clientRect;
			}
			set {
				//Recalculate coordinates for new values
				_clientRect = value;
			}
		}
		public Size Resolution;
		
		public int Count {
			get {
				return NodesList.Count;
			}
		}
		protected List<Point> _nodesList;
		public List<Point> NodesList {
			get {
				if (_nodesList == null)
					_nodesList = new List<Point>();
				return _nodesList;
			}
			set { _nodesList = value; }
		}
		protected int _maxNodes = 10;
		protected int _minNodes = 3;
		protected int _checkedIndex = -1;
		bool _regionChecked = false;
		
		public List<Point> ToList_Scr() {
			return NodesList.Select(p => {
				    return StreamToScreen(p);
				}).ToList();
		}
		
		protected bool IsPointInRound(Point p1, Point p2) {
			int dx = Math.Abs(p1.X - p2.X);
			int dy = Math.Abs(p1.Y - p2.Y);
			if (dx < 10 && dy < 10)
				return true;
			else
				return false;
		}
		//protected void SetRegionSelection() {
		//    _regionChecked = IsPointInRegion(NodesList);
		//}
		protected void ResetRegionSelection(){
			_regionChecked = false;
		}
		protected void SetSelection(Point pe) {
			var res = NodesList.Where(x => {
				return IsPointInRound(x, pe);
			});
			if (res.Any()) {
				_checkedIndex = NodesList.FindIndex(x => x == res.First());
			}
		}
		public void ResetSelection() {
			_checkedIndex = -1;
		}
		protected Point FindPoint(Point point) {
			Point np = new Point(-1, -1);
			if (!IfValid(point))
				np = NodesList.Where(x => { return IsPointInRound(x, point); }).First();
			return np;
		}
		protected bool IfValid(Point point) {
			return !NodesList.Where(x => { return IsPointInRound(x, point); }).Any();
		}
		protected bool CheckMax() {
			return NodesList.Count < _maxNodes;
		}
		protected bool CheckMin() {
			return NodesList.Count == _minNodes;
		}

		#region Algorythms
		protected bool CheckIfInRectangle(Point point, Rectangle Rect) {
			if (point.X > Rect.X && point.X < Rect.X + Rect.Width && point.Y > Rect.Y && point.Y < Rect.Y + Rect.Height) {
				return true;
			}
			return false;
		}
		#region PointOnLine
		protected double dot(Point u, Point v) {
			return ((u).X * (v).X + (u).Y * (v).Y);
		}
		protected double norm(Point v) {
			return Math.Sqrt(dot(v, v));
		}
		protected double d(Point u, Point v) {
			return norm(new Point(u.X - v.X, u.Y - v.Y));
		}
		protected double distance_Point_to_Segment(Point P, Line S) {
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
		protected Line IfOnLine(Point point, List<Line> lines) {
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
		#endregion PointOnLine
		protected List<Line> SplitToLines() {
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
		#region Intersection Check
		protected bool transection(long ax1, long ay1, long ax2, long ay2, long bx1, long by1, long bx2, long by2) {
			long v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
			long v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
			long v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
			long v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
			return ((v1 * v2 <= 0) && (v3 * v4 <= 0));
		}
		protected bool CheckTwoLinesIntersected(Line line1, Line line2) {
			return transection(line1.Begin.X, line1.Begin.Y, line1.End.X, line1.End.Y, line2.Begin.X, line2.Begin.Y, line2.End.X, line2.End.Y);
		}
		protected bool CheckLinesIntersected(Line line1, List<Line> lines) {
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
		protected bool IsIntersect() {
			List<Line> lines = SplitToLines();
			foreach (var val in lines) {
				if (CheckLinesIntersected(val, lines))
					return true;
			}
			return false;
		}
		public bool CheckIntersection() {
			bool isIntersect = false;
			if (NodesList.Count > 3 && IsIntersect()) {
				isIntersect = true;
			}
			return isIntersect;
		}
		#endregion Intersection Check 
		#region Coordinat translation
		protected Point StreamToScreen(Point val) {
			double kx = ((double)ClientRect.Width) / ((double)Resolution.Width);
			double ky = ((double)ClientRect.Height) / ((double)Resolution.Height);
			return new Point((int)(((double)val.X) * kx + (double)ClientRect.X), (int)(((double)val.Y) * ky + (double)ClientRect.Y));
		}
		protected Point ScreenToStream(Point val) {
			double kx = ((double)ClientRect.Width) / ((double)Resolution.Width);
			double ky = ((double)ClientRect.Height) / ((double)Resolution.Height);
			return new Point((int)(((double)val.X) / kx - (double)ClientRect.X / kx), (int)(((double)val.Y) / ky - (double)ClientRect.Y / ky));
		}
		#endregion
		#endregion Algorythms
	}
	public class HeightMarkerRegion : BaseRegion {
		public HeightMarkerRegion(Point top, Point bottom, int physHeight) {
			_marker = new Rectangle() { X = top.X - 30, Y = top.Y, Width = 60, Height = Math.Abs(bottom.Y - top.Y)};
			SetTopBottomLegend(_marker);
			
			_legendPen = new Pen(Color.Black);
			_legendBrush = new SolidBrush(Color.Black);
			FontFamily ff = new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace);
			_font = new Font(ff, 8, FontStyle.Regular);

			_physicalHeight = physHeight;

			_maxNodes = 4;
			_minNodes = 4;
		}

		static int _physicalHeight = 180;
		Rectangle _marker;
		Rectangle _legendRect;
		Point _legendPoint;
		Point _top;
		Rectangle _topRect;
		Point _bottom;
		Rectangle _bottomRect;
		Font _font;
		Pen _legendPen;
		Brush _legendBrush;
		bool _markerSelected=false;
		bool _markerUpBorderSelected = false;
		bool _markerBottomBorderSelected = false;
		int _minHeigth = 40;
		Size _mouseOffset;

		Rectangle RectToScreen(Rectangle rect) {
			Point lu = StreamToScreen(new Point(rect.X, rect.Y));
			Point rd = StreamToScreen(new Point(rect.X + rect.Width, rect.Y + rect.Height));

			Rectangle nrect = new Rectangle(lu.X, lu.Y, rd.X - lu.X, rd.Y - lu.Y);
			return nrect;
		}

		public override List<Point> GetRegion() { return null; }
		public override HMarker GetMarker() {
			return new HMarker() { top = _top, bottom = _bottom, pheight = _physicalHeight };
		}

		public override void draw(Graphics graph) {
			Rectangle markerRect = RectToScreen(_marker);

			GraphicsPath gp = new GraphicsPath();

			Pen currPen;
			Brush currBrush;
			Brush currTransparentBrush;
			
			currPen = greenPen;
			currBrush = greenBrash;
			currTransparentBrush = greenTransparentBrash;

			graph.DrawRectangle(currPen, markerRect);
			graph.FillRectangle(currTransparentBrush, markerRect);

			graph.DrawLine(currPen, StreamToScreen(_bottom), StreamToScreen(_top));
			graph.FillEllipse(currBrush, RectToScreen(_topRect));
			graph.FillEllipse(currBrush, RectToScreen(_bottomRect));
			graph.FillRectangle(new SolidBrush(Color.White), RectToScreen(_legendRect));
			graph.DrawString(_physicalHeight.ToString(), _font, _legendBrush, StreamToScreen(_legendPoint));
		}
		public override void mouseup(MouseEventArgs e) {
			ResetSelectedMarker();
		}
		bool _isMouseOwner = false;
		public override void mousemove(MouseEventArgs e) {
			if (_markerSelected) {
				SetMarkerLocation(ScreenToStream(e.Location));
			}
			if (_markerUpBorderSelected) {
				SetMarkerUpSide(ScreenToStream(e.Location));
			}
			if (_markerBottomBorderSelected) {
				SetMarkerBottomSide(ScreenToStream(e.Location));
			}
		}
		public override void mousedown(MouseEventArgs e) {
			SetSelectedMarker(ScreenToStream(e.Location));
		}
		public override void mouseclick(MouseEventArgs e, Point scrPoint) {
			
		}
		public override void mousedclick(MouseEventArgs e, Point scrPoint) {
			if (e.Button == MouseButtons.Left) {
				if (CheckIfInRectangle(ScreenToStream(e.Location), _marker))
					OpenPhysicalSizeMenu(scrPoint);
			} else {
				
			}
		}

		bool IsMouseOver(Point point) {
			if (CheckOnUpSide(point, _marker) || CheckOnBottomSide(point, _marker) || CheckIfInRectangle(point, _marker))
				return true;
			else
				return false;
		}
		void OpenPhysicalSizeMenu(Point point) {
			Point pt = new Point(point.X - 82, point.Y - 35);
			var setSizeForm = new SetSize(_physicalHeight, pt);
			setSizeForm.ShowDialog();
			_physicalHeight = (int)setSizeForm._size.Value;
		}
		void SetTopBottomLegend(Rectangle rect) {
			_top = new Point(rect.X + rect.Width / 2, rect.Y);
			_bottom = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height);
			_topRect = new Rectangle(_top.X - 4, _top.Y - 4, 8, 8);
			_bottomRect = new Rectangle(_bottom.X - 4, _bottom.Y - 4, 8, 8);
			_legendRect = new Rectangle(new Point(rect.X + 10, rect.Y + rect.Height / 2 - 15), new Size(41, 30));
			_legendPoint = new Point(rect.X + 10, rect.Y + rect.Height / 2 - 15);
		}
		protected void SetMarkerUpSide(Point pe) {
			int oUpY = _marker.Y;
			int oButY = _marker.Y + _marker.Height;
			if (pe.Y < 0)
				pe.Y = 0;
			if (Math.Abs(oButY - pe.Y) < _minHeigth || pe.Y > oButY)
				pe.Y = _marker.Y;
			int dY = _marker.Y - pe.Y;
			_marker.Y = pe.Y;
			_marker.Height += dY;
			SetTopBottomLegend(_marker);
		}
		protected void SetMarkerBottomSide(Point pe) {
			int oHeight = _marker.Height;
			_marker.Height = pe.Y - _marker.Y;
			if (_marker.Height < _minHeigth)
				_marker.Height = oHeight;
			if ((_marker.Y + _marker.Height) > Resolution.Height) {
				_marker.Height = oHeight;
			}
			SetTopBottomLegend(_marker);
		}
		protected void SetMarkerLocation(Point pe) {
			var nPoint = new Point(pe.X - _mouseOffset.Width, pe.Y - _mouseOffset.Height);
			if (nPoint.Y > 0 && nPoint.X > 0 && (nPoint.Y + _marker.Height) < Resolution.Height && (nPoint.X + _marker.Width) < Resolution.Width) {
				_marker.Location = new Point(pe.X - _mouseOffset.Width, pe.Y - _mouseOffset.Height);
				SetTopBottomLegend(_marker);
			}
		}
		protected void ResetSelectedMarker() {
			_markerSelected = false;
			_markerBottomBorderSelected = false;
			_markerUpBorderSelected = false;
			Parent.Cursor = Cursors.Default;
		}

		bool CheckOnUpSide(Point pt, Rectangle rect){
			Point beg = new Point(rect.X, rect.Y);
			Point end = new Point(rect.X+rect.Width, rect.Y);
			double dist = distance_Point_to_Segment(pt, new Line(beg,end));
			if (dist < 6)
				return true;
			return false;
		}
		bool CheckOnBottomSide(Point pt, Rectangle rect) {
			Point beg = new Point(rect.X, rect.Y + rect.Height);
			Point end = new Point(rect.X + rect.Width, rect.Y + rect.Height);
			double dist = distance_Point_to_Segment(pt, new Line(beg, end));
			if (dist < 6)
				return true;
			return false;
		}

		protected void SetSelectedMarker(Point pe) {
			if(!(_markerUpBorderSelected = CheckOnUpSide(pe, _marker)))
			if(!(_markerBottomBorderSelected = CheckOnBottomSide(pe, _marker)))
			if (CheckIfInRectangle(pe, _marker)) {
				_markerSelected = true;
				Point point = _marker.Location;
				_mouseOffset = new Size(pe.X - point.X, pe.Y - point.Y);
			} else
				_markerSelected = false;
		}
	}
	public class TrackerRegion : BaseRegion {
		public TrackerRegion() {
			_maxNodes = 10;
			_minNodes = 3;
		}

		public override List<Point> GetRegion() { return NodesList; }
		public override HMarker GetMarker() { return null; }

		public override void draw(Graphics graph) {
			bool isIntersect = CheckIntersection();
			if (Count > 1) {
				var points = ToList_Scr();

				GraphicsPath gp = new GraphicsPath();
				gp.AddLines(points.ToArray());

				Pen currPen;
				Brush currBrush;
				Brush currTransparentBrush;
				if (isIntersect) {
					currPen = yellowPen;
					currBrush = yellowBrush;
					currTransparentBrush = yellowTransparentBrush;
				} else {
					currPen = redPen;
					currBrush = redBrush;
					currTransparentBrush = redTransparentBrush;
				}

				points.ForEach(x => {
					graph.FillRectangle(currBrush, new Rectangle(x.X - margin, x.Y - margin, doublem, doublem));
				});
				graph.FillPolygon(currTransparentBrush, points.ToArray());
				gp.CloseFigure();
				graph.DrawPath(currPen, gp);
			}
		}
		public override void mouseup(MouseEventArgs e) {
			ResetSelection();
		}
		public override void mousemove(MouseEventArgs e) {
			SetPointLocation(ScreenToStream(e.Location));
		}
		public override void mousedown(MouseEventArgs e) {
			SetSelection(ScreenToStream(e.Location));
		}
		public override void mouseclick(MouseEventArgs e, Point scrPoint) {
			
		}
		public override void mousedclick(MouseEventArgs e, Point scrPoint) {
			if (e.Button == MouseButtons.Left) {
				InsertPoint(ScreenToStream(e.Location));
			} else {
				Pop(ScreenToStream(e.Location));
			}
		}

		public TrackerRegion(List<Point> plst) {
			NodesList.AddRange(plst);
			_maxNodes = 10;
			_minNodes = 3;
			_clientRect = Rectangle.Empty;
		}
		protected Point LastLocationPoint;
		
		protected Point CorrectCoordinates(Point point) {
			Point nP = Point.Empty;
			nP.X = point.X;
			nP.Y = point.Y;
			if (nP.X < 0)
				nP.X = 0;
			if (nP.X > Resolution.Width)
				nP.X = Resolution.Width;
			if (nP.Y < 0)
				nP.Y = 0;
			if (nP.Y > Resolution.Height)
				nP.Y = Resolution.Height;
			if (IfValid(nP))
				return nP;
			return LastLocationPoint;
		}
		public Point FirstPoint() {
			Point first = Point.Empty;
			if (!CheckMin())
				first = NodesList.Last();
			return first;
		}
		public void SetPointLocation(Point pe) {
			if (_checkedIndex != -1) {
				NodesList[_checkedIndex] = CorrectCoordinates(pe);
				LastLocationPoint = NodesList[_checkedIndex];
			}
		}

		public void Pop(Point pe) {
			if (!CheckMin()) {
				Point p = FindPoint(pe);
				if (p.X != -1 && p.Y != -1)
					NodesList.Remove(p);
			}
		}
		public void InsertPoint(Point pe) {
			var line = IfOnLine(pe, SplitToLines());
			if (line != null)
			if (true) {
				if (CheckMax()) {
					LinkedList<Point> lines = new LinkedList<Point>(NodesList);
					var val = lines.Find(line.Begin);
					if (val != null) {
						lines.AddAfter(val, pe);
						NodesList = lines.ToList();
					}
				}
			}
		}
		public Point NewNode { set {
			if (CheckMax() && IfValid(value))
				NodesList.Add(value); 
		} }

	}
}
