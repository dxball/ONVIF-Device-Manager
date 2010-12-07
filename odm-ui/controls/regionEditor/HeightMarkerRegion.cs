using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace nvc.controls.regionEditor {
	public class HeightMarkerRegion : BaseMarker {
		public HeightMarkerRegion(Point inTop, Point inBottom, Size physSize) {
			//CorrectIn values
			Point Top = new Point((inTop.X + inBottom.X) / 2, inTop.Y);
			Point Bottom = new Point((inTop.X + inBottom.X) / 2, inBottom.Y);
			//
			Point top;
			Point bottom;
			if (Top.Y < Bottom.Y) {
				top = Top;
				bottom = Bottom;
			} else {
				top = Bottom;
				bottom = Top;
			}
			_marker = new Rectangle() { X = top.X - 30, Y = top.Y, Width = 60, Height = Math.Abs(bottom.Y - top.Y)};

			SetMousePointers(_marker);

			_legendPen = new Pen(Color.Black);
			_legendBrush = new SolidBrush(Color.Black);
			FontFamily ff = new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace);
			_font = new Font(ff, 8, FontStyle.Regular);

			_physicalHeight = physSize.Height;
		}



		public override List<Point> GetRegion() { return null; }
		public override Rectangle GetRectangle() { return Rectangle.Empty; }
		public override HMarker GetMarker() {
			return new HMarker() { P1 = _top, P2 = _bottom, pheight = _physicalHeight };
		}

		public override void draw(Graphics graph) {
			int pointRadius = 3;

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

			graph.DrawLine(currPen, RegionUtils.StreamToScreen(_bottom, ClientRect, Resolution), RegionUtils.StreamToScreen(_top, ClientRect, Resolution));
			graph.FillEllipse(currBrush, (markerRect.Left + markerRect.Right) / 2 - pointRadius, markerRect.Top - pointRadius, pointRadius * 2, pointRadius*2);
			graph.FillEllipse(currBrush, (markerRect.Left + markerRect.Right) / 2 - pointRadius, markerRect.Bottom - pointRadius, pointRadius * 2, pointRadius*2);
			graph.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.White)), RegionUtils.RectToScreen(_legendRect, ClientRect, Resolution));
			graph.DrawString(_physicalHeight.ToString(), _font, _legendBrush, RegionUtils.StreamToScreen(_legendPoint, ClientRect, Resolution));
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
		}

		protected override void OpenPhysicalSizeMenu(Point point) {
			Point pt = new Point(point.X - 82, point.Y - 35);
			var setSizeForm = new SetSize(new Size(0, _physicalHeight), pt, false);
			setSizeForm.ShowDialog();
			_physicalHeight = (int)setSizeForm._heigth.Value;
		}

	}
}
