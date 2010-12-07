using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace nvc.controls.regionEditor {
	public class HeightMarker2DRegion : BaseMarker {
		public HeightMarker2DRegion(Point P1, Point P2, Size physSize) {
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

			_marker = new Rectangle() { X = lX, Y = tY, Width = width, Height = heigth };

			SetMousePointers(_marker);

			_legendPen = new Pen(Color.Black);
			_legendBrush = new SolidBrush(Color.Black);
			FontFamily ff = new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace);
			_font = new Font(ff, 8, FontStyle.Regular);

			_physicalHeight = physSize.Height;
			_physicalWidth = physSize.Width;
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
			graph.DrawString(wlegend, _font, _legendBrush, RegionUtils.StreamToScreen(_legendPoint, ClientRect, Resolution));
			graph.DrawString(hlegend, _font, _legendBrush, RegionUtils.StreamToScreen(new Point(_legendRect.X, _legendRect.Y + _legendRect.Height/2), ClientRect, Resolution));
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
