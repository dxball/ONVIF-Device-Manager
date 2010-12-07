using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace nvc.controls.regionEditor {
	public abstract class BaseMarker: BaseRegion {
		protected static int _physicalHeight = 180;
		protected static int _physicalWidth = 180;
		protected Rectangle _marker;
		protected Rectangle _legendRect;
		protected Point _legendPoint;
		protected Point _top;
		protected Rectangle _topRect;
		protected Point _bottom;
		protected Rectangle _bottomRect;
		protected Point _left;
		protected Rectangle _leftRect;
		protected Point _right;
		protected Rectangle _rightRect;
		protected Font _font;
		protected Pen _legendPen;
		protected Brush _legendBrush;
		protected bool _markerSelected = false;
		protected bool _markerUpBorderSelected = false;
		protected bool _markerBottomBorderSelected = false;
		protected bool _markerLeftBorderSelected = false;
		protected bool _markerRightBorderSelected = false;
		protected int _minHeigth = 5;
		protected int _minWidth = 5;
		protected int _pointRound = 6;
		protected Size _mouseOffset;
		protected bool _isMouseOwner = false;

		protected bool CheckOnUpSide(Point pt, Rectangle rect) {
			Point beg = new Point(rect.X, rect.Y);
			Point end = new Point(rect.X + rect.Width, rect.Y);
			double dist = RegionUtils.distance_Point_to_Segment(pt, new Line(beg, end));
			if (dist < _pointRound)
				return true;
			return false;
		}
		protected bool CheckOnBottomSide(Point pt, Rectangle rect) {
			Point beg = new Point(rect.X, rect.Y + rect.Height);
			Point end = new Point(rect.X + rect.Width, rect.Y + rect.Height);
			double dist = RegionUtils.distance_Point_to_Segment(pt, new Line(beg, end));
			if (dist < _pointRound)
				return true;
			return false;
		}
		protected bool CheckOnLeftSide(Point pt, Rectangle rect) {
			Point beg = new Point(rect.X, rect.Y);
			Point end = new Point(rect.X, rect.Y + rect.Height);
			double dist = RegionUtils.distance_Point_to_Segment(pt, new Line(beg, end));
			if (dist < _pointRound)
				return true;
			return false;
		}
		protected bool CheckOnRightSide(Point pt, Rectangle rect) {
			Point beg = new Point(rect.X + rect.Width, rect.Y);
			Point end = new Point(rect.X + rect.Width, rect.Y + rect.Height);
			double dist = RegionUtils.distance_Point_to_Segment(pt, new Line(beg, end));
			if (dist < _pointRound)
				return true;
			return false;
		}
		protected void SetMarkerUpSide(Point pe) {
			int oUpY = _marker.Y;
			int oButY = _marker.Y + _marker.Height;
			//check if Y did not shift from screen coord.
			if (pe.Y < 0)
				pe.Y = 0;
			//Check if maker size greater than min size and Up point did not match bottom point
			if (Math.Abs(oButY - pe.Y) < _minHeigth || pe.Y > oButY)
				pe.Y = _marker.Y;
			int dY = _marker.Y - pe.Y;
			_marker.Y = pe.Y;
			_marker.Height += dY;
			SetMousePointers(_marker);
		}
		protected void SetMarkerBottomSide(Point pe) {
			int oHeight = _marker.Height;
			//add displaysment to marker vertex position
			_marker.Height = pe.Y - _marker.Y;
			//check if marker heigth not smaller than min heigth
			if (_marker.Height < _minHeigth)
				_marker.Height = oHeight;
			//check if marker heigth not grater than max size
			if ((_marker.Y + _marker.Height) > Resolution.Height) {
				_marker.Height = oHeight;
			}
			SetMousePointers(_marker);
		}
		protected void SetMarkerLeftSide(Point pe) {
			int oLeftX = _marker.X;
			int oRightX = _marker.X + _marker.Width;
			
			if (pe.X < 0)
				pe.X = 0;
			if (oRightX - pe.X < _minWidth)
				pe.X = oLeftX;
			_marker.X = pe.X;
			_marker.Width -= pe.X - oLeftX;
			SetMousePointers(_marker);
		}
		protected void SetMarkerRightSide(Point pe) {
			int oWidth = _marker.Width;

			if (pe.X > Resolution.Width - 1)
				pe.X = Resolution.Width - 1;
			if ((pe.X - _marker.X) < _minWidth)
				pe.X = _marker.X + _minWidth;
			_marker.Width = pe.X - _marker.X;
			SetMousePointers(_marker);
		}
		protected abstract void OpenPhysicalSizeMenu(Point point);

		protected void SetMousePointers(Rectangle rect) {
			SetPointerLocation(rect);
		}

		protected void SetPointerLocation(Rectangle rect) {
			_top = new Point(rect.X + rect.Width / 2, rect.Y);
			_bottom = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height);
			_topRect = new Rectangle(_top.X - 4, _top.Y - 4, 8, 8);
			_bottomRect = new Rectangle(_bottom.X - 4, _bottom.Y - 4, 8, 8);
			_left = new Point(rect.X, rect.Y + rect.Height / 2);
			_right = new Point(rect.X + rect.Width, rect.Y + rect.Height / 2);
			_leftRect = new Rectangle(_left.X - 4, _left.Y - 4, 8, 8);
			_rightRect = new Rectangle(_right.X - 4, _right.Y - 4, 8, 8);

			_legendPoint = new Point(rect.X + rect.Width / 2 - 20, rect.Y + rect.Height / 2 - 15);
			_legendRect = new Rectangle(_legendPoint, new Size(41, 30));
		}

		protected void SetMarkerLocation(Point pe) {
			var nPoint = new Point(pe.X - _mouseOffset.Width, pe.Y - _mouseOffset.Height);
			if (nPoint.Y > 0 && nPoint.X > 0 && (nPoint.Y + _marker.Height) < Resolution.Height && (nPoint.X + _marker.Width) < Resolution.Width) {
				_marker.Location = new Point(pe.X - _mouseOffset.Width, pe.Y - _mouseOffset.Height);
				SetMousePointers(_marker);
			}
		}
		protected void ResetSelectedMarker() {
			_markerSelected = false;
			_markerBottomBorderSelected = false;
			_markerUpBorderSelected = false;
			_markerLeftBorderSelected = false;
			_markerRightBorderSelected = false;
			Parent.Cursor = Cursors.Default;
		}
		protected void SetSelectedMarker(Point pe) {
			if (!(_markerUpBorderSelected = CheckOnUpSide(pe, _marker)))
				if (!(_markerBottomBorderSelected = CheckOnBottomSide(pe, _marker)))
					if(!(_markerLeftBorderSelected = CheckOnLeftSide(pe, _marker)))
						if(!(_markerRightBorderSelected = CheckOnRightSide(pe,_marker)))
							if (RegionUtils.CheckIfInRectangle(pe, _marker)) {
								_markerSelected = true;
								Point point = _marker.Location;
								_mouseOffset = new Size(pe.X - point.X, pe.Y - point.Y);
							} else
								_markerSelected = false;
		}
		protected bool IsMouseOver(Point point) {
			if (CheckOnUpSide(point, _marker) || CheckOnBottomSide(point, _marker) || RegionUtils.CheckIfInRectangle(point, _marker))
				return true;
			else
				return false;
		}

		public override void mouseup(MouseEventArgs e) {
			ResetSelectedMarker();
		}
		public override void mousedown(MouseEventArgs e) {
			SetSelectedMarker(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
		}
		public override void mouseclick(MouseEventArgs e, Point scrPoint) {

		}
		public override void mousedclick(MouseEventArgs e, Point scrPoint) {
			if (e.Button == MouseButtons.Left) {
				if (RegionUtils.CheckIfInRectangle(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution), _marker))
					OpenPhysicalSizeMenu(scrPoint);
			} else {

			}
		}
	}
}
