using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace odm.controls.regionEditor {
	public class HeightMarker2DRegion : BaseMarker {
		public HeightMarker2DRegion(Point P1, Point P2, Size physSize) {
			_physicalHeight = physSize.Height;
			_physicalWidth = physSize.Width == 0 ? physSize.Height : physSize.Width;

			int lX;
			int width = Math.Abs(P1.X - P2.X);
			int tY;
			int heigth = Math.Abs(P1.Y - P2.Y);

			if (P1.Y <= P2.Y) {
				tY = P1.Y;
			} else {
				tY = P2.Y;
			}
			if (P1.X <= P2.X) {
				lX = P1.X;
			} else {
				lX = P2.X;
			}

			_marker = MakeInitialCorrections(new Rectangle() { X = lX, Y = tY, Width = width, Height = heigth });

			SetMousePointers(_marker);

			_legendPen = new Pen(Color.Black);
			_legendBrush = new SolidBrush(Color.Black);
			FontFamily ff = new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace);
			_font = new Font(ff, 8, FontStyle.Regular);
		}
		public override void Refresh() {
			_marker = MakeInitialCorrections(_marker);
			SetMousePointers(_marker);
		}
		Rectangle MakeInitialCorrections(Rectangle marker) {
			int dX = marker.Width;
			int dY = marker.Height;

			float k = ((float)_physicalWidth) / ((float)_physicalHeight);
			dX = (int)((float)dY * k);

			Rectangle newRect = new Rectangle(marker.Location, new Size(dX, dY));

			return newRect;
		}
		public override void draw(Graphics graph) {
			Rectangle markerRect = RegionUtils.RectToScreen(_marker, ClientRect, Resolution);

			GraphicsPath gp = new GraphicsPath();

			Pen currPen;
			Brush currBrush;
			Brush currTransparentBrush;

			currPen = RegionUtils.greenPen;
			currBrush = RegionUtils.greenBrash;
			currTransparentBrush = RegionUtils.greenTransparentBrash;

			graph.DrawRectangle(currPen, markerRect);
			graph.FillRectangle(currTransparentBrush, markerRect);

			//draw lines (up-bottom, left-right)
			graph.DrawLine(currPen, RegionUtils.StreamToScreen(_bottom, ClientRect, Resolution), RegionUtils.StreamToScreen(_top, ClientRect, Resolution));
			graph.DrawLine(currPen, RegionUtils.StreamToScreen(_left, ClientRect, Resolution), RegionUtils.StreamToScreen(_right, ClientRect, Resolution));
			
			//draw marker poiners (up, left, right, bottom)
			graph.FillEllipse(currBrush, RegionUtils.RectToScreen(_topRect, ClientRect, Resolution));
			graph.FillEllipse(currBrush, RegionUtils.RectToScreen(_bottomRect, ClientRect, Resolution));
			graph.FillEllipse(currBrush, RegionUtils.RectToScreen(_leftRect, ClientRect, Resolution));
			graph.FillEllipse(currBrush, RegionUtils.RectToScreen(_rightRect, ClientRect, Resolution));

			graph.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.White)), RegionUtils.RectToScreen(_legendRect, ClientRect, Resolution));
			string wlegend = PropertyDepthCalibrationStrings.Instance.widthLiteral +_physicalWidth.ToString();
			string hlegend = PropertyDepthCalibrationStrings.Instance.heigthLiteral + _physicalHeight.ToString();
			graph.DrawString(hlegend, _font, _legendBrush, RegionUtils.StreamToScreen(_legendPoint, ClientRect, Resolution));
			graph.DrawString(wlegend, _font, _legendBrush, RegionUtils.StreamToScreen(new Point(_legendRect.X, _legendRect.Y + _legendRect.Height/2), ClientRect, Resolution));
		}
		public override void mousemove(MouseEventArgs e) {
			if (_markerSelected) {
				SetMarkerLocation(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
			}
			if (_markerUpBorderSelected) {
				SetMarkerUpSide(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
			}
			if (_markerBottomBorderSelected) {
				SetMarkerBottomSide(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
			}
			if (_markerLeftBorderSelected) {
				SetMarkerLeftSide(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
			}
			if (_markerRightBorderSelected) {
				SetMarkerRightSide(RegionUtils.ScreenToStream(e.Location, ClientRect, Resolution));
			}
		}
		protected override void SetMarkerUpSide(Point pe) {
			int oButY = _marker.Y + _marker.Height;
			
			base.SetMarkerUpSide(pe);

			//Check marker relation rule
			float rW = _physicalWidth ==0 ? 1:(float)_physicalWidth;
			float k = ((float)_physicalHeight)/(rW);
			float vW = (float)_marker.Width;
			float relation = vW*k;
			if (relation < _marker.Height) {
				_marker.Y = oButY - (int)relation;
				_marker.Height = (int)relation;
			}

			SetMousePointers(_marker);
		}
		protected override void SetMarkerBottomSide(Point pe) {
			base.SetMarkerBottomSide(pe);
			
			//Check marker relation rule
			float rW = _physicalWidth == 0 ? 1 : (float)_physicalWidth;
			float k = ((float)_physicalHeight) / (rW);
			float vW = (float)_marker.Width;
			float relation = vW * k;
			if (relation < _marker.Height) {
				_marker.Height = (int)relation;
			}

			SetMousePointers(_marker);
		}
		protected override void SetMarkerLeftSide(Point pe) {
			int oldLeftX = _marker.X + _marker.Width;
			base.SetMarkerLeftSide(pe);

			//Check marker relation rule
			float rH = _physicalHeight == 0 ? 1 : (float)_physicalHeight;
			float k = ((float)_physicalWidth) / (rH);
			float vH = (float)_marker.Height;
			float relation = vH * k;
			if (_marker.Width < relation) {
				_marker.X = oldLeftX - (int)relation;
				_marker.Width = (int)relation;
			}

			SetMousePointers(_marker);
		}
		protected override void SetMarkerRightSide(Point pe) {
			base.SetMarkerRightSide(pe);

			//Check marker relation rule
			float rH = _physicalHeight == 0 ? 1 : (float)_physicalHeight;
			float k = ((float)_physicalWidth) / (rH);
			float vH = (float)_marker.Height;
			float relation = vH * k;
			if (_marker.Width < relation) {
				_marker.Width = (int)relation;
			}

			SetMousePointers(_marker);
		}

		public override List<Point> GetRegion() { return null; }
		public override HMarker GetMarker() {
			return new HMarker() { P1 = new Point(_marker.X, _marker.Y), 
				P2 = new Point(_marker.X + _marker.Width, _marker.Y + _marker.Height), 
				pheight = _physicalHeight, pwidth = _physicalWidth };
		}
		public override Rectangle GetRectangle() { return Rectangle.Empty; }

		protected override void OpenPhysicalSizeMenu(Point point) {
			Point pt = new Point(point.X - 82, point.Y - 35);
			var setSizeForm = new SetSize(new Size(_physicalWidth, _physicalHeight), pt, true);
			setSizeForm.ShowDialog();
			_physicalHeight = (int)setSizeForm._heigth.Value;
			_physicalWidth = (int)setSizeForm._width.Value;
		}
	}
}
