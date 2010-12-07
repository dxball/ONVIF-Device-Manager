using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace nvc.controls.regionEditor {
	/// <summary>
	/// Class represents the base functionality for all types of editable shapes
	/// </summary>
	public abstract class BaseRegion {
		public BaseRegion() {
			_clientRect = Rectangle.Empty;
		}
		//Parent control to draw on GraphContext
		public Control Parent { get; set; }
		/// <summary>
		/// Represents the original dimensions of the video frame
		/// </summary>
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

		/// <summary>
		/// Represents the size of the area to draw a frame
		/// </summary>
		public Size Resolution { get; set; }

		//Abstract methods
		public abstract void draw(Graphics graph);
		public abstract void mouseup(MouseEventArgs e);
		public abstract void mousedown(MouseEventArgs e);
		public abstract void mouseclick(MouseEventArgs e, Point pt);
		public abstract void mousedclick(MouseEventArgs e, Point pt);
		public abstract void mousemove(MouseEventArgs e);
		//Abstract methods for get results of drawings
		public abstract List<Point> GetRegion();
		public abstract HMarker GetMarker();
		public abstract Rectangle GetRectangle();
	}
}
