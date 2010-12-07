using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
//using vlc_net;

namespace nvc.controls.regionEditor {
	public class GraphEditor {
		public GraphEditor(Rectangle resolution) {
			_mediaResolution = resolution;
			List<BaseRegion> _regions = new List<BaseRegion>();
		}
		public bool Is2D = false;
		//public void SetParent(VlcControlInner parent) {
		public void SetParent(Control parent) {
			_parent = parent;
		}
		void UnSubsvribeToEvents() {
			_parent.MouseDoubleClick -= _parent_MouseDoubleClick;
			_parent.MouseDown -= _parent_MouseDown;
			_parent.MouseUp -= _parent_MouseUp;
			_parent.MouseMove -= _parent_MouseMove;
			_parent.MouseClick -= _parent_MouseClick;
		}
		void SubsvribeToEvents() {
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
					SubsvribeToEvents();
				}
				return _regions;
			}
		}
		public void AddRectangleEditor(Rectangle rect) {

		}
		public void AddHeightMarker(Point top, Point bottom, Size physicalSize) {
			BaseMarker hReg;
			if (Is2D) {
				//top = new Point(top.X - 10, top.Y);
				//bottom = new Point(bottom.X + 10, bottom.Y);
				hReg = new HeightMarker2DRegion(top, bottom, physicalSize) { Parent = _parent };
				hReg.ClientRect = _parent.ClientRectangle;
				hReg.Resolution = _mediaResolution.Size;
			} else {
				hReg = new HeightMarkerRegion(top, bottom, physicalSize) { Parent = _parent };
				hReg.ClientRect = _parent.ClientRectangle;
				hReg.Resolution = _mediaResolution.Size;
			}
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
			Regions.Clear();
		}

		Rectangle _mediaResolution;
		//VlcControlInner _parent;
		Control _parent;
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
}
