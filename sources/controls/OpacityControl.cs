using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nvc.controls {
	public partial class OpacityControl : Form {
		public OpacityControl() {
			InitializeComponent();
			
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.ResizeRedraw, true);

			BackColor = Color.Transparent;
		}

		void NodesLayer_Load(object sender, EventArgs e) {
			Refresh();
		}

		List<Point> _points;

		protected override CreateParams CreateParams {
			get {
				const int WS_EX_TRANSPARENT = 0x20;
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS_EX_TRANSPARENT;
				return cp;
			}
		}
		protected override void OnPaintBackground(PaintEventArgs e) {
			
		}

		protected override void OnPaint(PaintEventArgs e) {
			Brush brush = new SolidBrush(Color.Red);
			Pen pen = new Pen(brush, 1);
			e.Graphics.FillRectangle(brush, this.ClientRectangle);
			//Point lastpoint = Point.Empty;
			//foreach (var val in _points) {
			//    Point point = new Point(val.X - 3, val.Y - 3);
			//    Rectangle rect = new Rectangle(point, new Size(6, 6));
			//    e.Graphics.FillRectangle(brush, rect);
			//    if (lastpoint != Point.Empty)
			//        e.Graphics.DrawLine(pen, lastpoint, val);
			//    lastpoint = val;
			//}
			//e.Graphics.DrawLine(pen, _points.First(), _points.Last());
		}

		//void InitGraphics(List<Point> points) {
		//    foreach (var val in points) {
		//        Point point = new Point(val.X - 3, val.Y - 3);
		//        Rectangle rect = new Rectangle(point, new Size(6, 6));
		//    }
		}
}
