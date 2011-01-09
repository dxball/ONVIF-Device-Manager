using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace odm.controls.regionEditor {
	public class TrackerRegion : BaseRegion {
		public TrackerRegion() {
			_maxNodes = 10;
			_minNodes = 3;
		}

		//Defines
		protected int margin = 4;
		protected int doublem = 8;

		protected int _maxNodes = 10;
		protected int _minNodes = 3;
		protected bool _IsMouseCatched = false;
		/// <summary>
		/// Returns value indicated if max amount of Nodes reached
		/// </summary>
		/// <returns>true/false</returns>
		protected bool CheckMax() {
			return NodesList.Count < _maxNodes;
		}

		/// <summary>
		/// Returns value indicated if min amount of Nodes reached
		/// </summary>
		/// <returns>true/false</returns>
		protected bool CheckMin() {
			return NodesList.Count == _minNodes;
		}

		protected List<Point> _nodesList;
		/// <summary>
		/// list of nodes on current region
		/// </summary>
		public List<Point> NodesList {
			get {
				if (_nodesList == null)
					_nodesList = new List<Point>();
				return _nodesList;
			}
			set { _nodesList = value; }
		}
		/// <summary>
		/// Count of nodes on current shape
		/// </summary>
		public int Count {
			get {
				return NodesList.Count;
			}
		}
		/// <summary>
		/// Set region (shape) selected if point s in "round" on any Node in region nodes list
		/// </summary>
		/// <param name="pe">Mouse click Point</param>
		protected void SetSelection(Point pe) {
			var res = NodesList.Where(x => {
				return RegionUtils.IsPointInRound(x, pe);
			});
			if (res.Any()) {
				_checkedIndex = NodesList.FindIndex(x => x == res.First());
			}
		}
		/// <summary>
		/// Set current region (shape) not selected
		/// </summary>
		public void ResetSelection() {
			_checkedIndex = -1;
		}
		/// <summary>
		/// Set current region (shape) not selected
		/// </summary>
		protected void ResetRegionSelection() {
			_regionChecked = false;
		}
		//Initial values
		protected int _checkedIndex = -1;
		bool _regionChecked = false;

		public override void Refresh() { }
		public override List<Point> GetRegion() { return NodesList; }
		public override Rectangle GetRectangle() { return Rectangle.Empty; }
		public override HMarker GetMarker() { return null; }

		public override void draw(Graphics graph) {
			bool isIntersect = RegionUtils.CheckIntersection(NodesList);
			if (Count > 1) {
				var points = RegionUtils.ToList_Scr(NodesList, ClientRect, Resolution);

				GraphicsPath gp = new GraphicsPath();
				gp.AddLines(points.ToArray());

				Pen currPen;
				Brush currBrush;
				Brush currTransparentBrush;
				if (isIntersect) {
					currPen = RegionUtils.yellowPen;
					currBrush = RegionUtils.yellowBrush;
					currTransparentBrush = RegionUtils.yellowTransparentBrush;
				} else {
					currPen = RegionUtils.redPen;
					currBrush = RegionUtils.redBrush;
					currTransparentBrush = RegionUtils.redTransparentBrush;
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
			SetPointLocation(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
		}
		public override void mousedown(MouseEventArgs e) {
			SetSelection(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
		}
		public override void mouseclick(MouseEventArgs e, Point scrPoint) {

		}
		public override void mousedclick(MouseEventArgs e, Point scrPoint) {
			if (e.Button == MouseButtons.Left) {
				InsertPoint(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
			} else {
				Pop(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
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
			if (RegionUtils.IfValid(nP, NodesList))
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
				Point p = RegionUtils.FindPoint(pe, NodesList);
				if (p.X != -1 && p.Y != -1)
					NodesList.Remove(p);
			}
		}
		public void InsertPoint(Point pe) {
			var line = RegionUtils.IfOnLine(pe, RegionUtils.SplitToLines(NodesList));
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
		public Point NewNode {
			set {
				if (CheckMax() && RegionUtils.IfValid(value, NodesList))
					NodesList.Add(value);
			}
		}

	}
}
