using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using odm.controls;
using System.IO.MemoryMappedFiles;
using odm.models;
using odm.controls.regionEditor;
using System.Threading;
using odm.utils.controls.wpfControls;

namespace odm.utils.controls {
	public partial class PropertyDepthCalibrationSTA : Form {

		PropertyDepthCalibrationStrings _strings = new PropertyDepthCalibrationStrings();
		public PropertyDepthCalibrationSTA(DepthCalibrationModel devModel) {
			InitializeComponent();
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			_devModel = devModel;

			Load += new EventHandler(PropertyDepthCalibrationSTA_Load);
		}
		public Action<Exception, string> onBindingError { get; set; }
		public Action<string> onVideoInitializationError { get; set; }

		PropertyDepthCalibrationSTAControls _controls;
		DepthCalibrationModel _devModel;
		GraphEditor _regionEditor;

		public Action Save { get; set; }
		public Action Cancel { get; set; }

		protected virtual void VideoOperationError(string message) {
			if (onVideoInitializationError != null) {
				onVideoInitializationError(message);
			}
		}
		protected virtual void BindingError(Exception err, string message) {
			if (onBindingError != null) {
				onBindingError(err, message);
			}
		}

		void InitControls() {
			Localization();
			if (!_devModel.is2DmarkerSupported)
				_controls._rb2D.Enabled = false;
			else {
				_controls._rb2D.Checked = _devModel.use2DMarkers;
				if (_controls._rb2D.Checked)
					_controls.groupBox1.Enabled = false;
			}
			//Color
			BackColor = ColorDefinition.colControlBackground;

			_controls._saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
			_controls._saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
		}
		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			//Cancel
			Cancel();
		}
		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			_devModel.use2DMarkers = _controls._rb2D.Checked;
			_devModel.region = _regionEditor.GetRegion();
			var retval = _regionEditor.GetMarkers();

			//convert from cm to mm
			retval[0].pheight *= 10;
			retval[0].pwidth *= 10;
			retval[1].pheight *= 10;
			retval[1].pwidth *= 10;

			_devModel.markers[0].size.y = retval[0].pheight;
			_devModel.markers[0].size.x = retval[0].pwidth;
			_devModel.markers[0].line1.Point[1] = new global::onvif.types.Vector() { x = retval[0].P1.X, y = retval[0].P1.Y };
			_devModel.markers[0].line1.Point[0] = new global::onvif.types.Vector() { x = retval[0].P2.X, y = retval[0].P2.Y };

			_devModel.markers[0].size.y = retval[1].pheight;
			_devModel.markers[0].size.x = retval[1].pwidth;
			_devModel.markers[0].line2.Point[1] = new global::onvif.types.Vector() { x = retval[1].P1.X, y = retval[1].P1.Y };
			_devModel.markers[0].line2.Point[0] = new global::onvif.types.Vector() { x = retval[1].P2.X, y = retval[1].P2.Y };

			Save();
		}
		protected wpfViewer _wpfControl { get; set; }
		public void InitUrl() {
			dbg.Assert(SynchronizationContext.Current != null);

			try {
				CreateStandAloneVLC(_devModel.mediaUri, _devModel.encoderResolution);
				pBox = new UserPictureBox() { Dock = DockStyle.Fill };
				_wpfControl = new wpfViewer();
				_wpfHost.Child = _wpfControl;
				panel1.Controls.Add(pBox);
				_tmr = new System.Windows.Forms.Timer();
				_tmr.Interval = 10;
				_tmr.Tick += new EventHandler(_tmr_Tick);
				drwaAction = _regionEditor.FillBitmap;
				_tmr.Start();
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
			//try {
			//    CreateStandAloneVLC(_devModel.mediaUri, _devModel.encoderResolution);
			//    pBox = new UserPictureBox() { Dock = DockStyle.Fill };
			//    panel1.Controls.Add(pBox);
			//    _tmr = new System.Windows.Forms.Timer();
			//    _tmr.Interval = 10; // refresh 100 time per second
			//    _tmr.Tick += new EventHandler(_tmr_Tick);
			//    drwaAction = _regionEditor.FillBitmap;
			//    _tmr.Start();
			//} catch (Exception err) {
			//    VideoOperationError(err.Message);
			//}

			LoadRegionEditor();
		}
		void BindData(DepthCalibrationModel devModel) {
			try {
				_controls._tbFocalLength.CreateBinding(x => x.Text, devModel, x => x.focalLength);
			} catch (Exception err) {
				string strValue;
				strValue = devModel.focalLength.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindFocalLength + strValue);
			}
			try {
				_controls._cbMatrixFormat.DataSource = MatrixTable.MatrixTbl;
				_controls._cbMatrixFormat.DisplayMember = "Name";
				_controls._cbMatrixFormat.ValueMember = "Value";

				MatrixTable.MatrixTbl.ForEach<MatrixValue>(x => {
					if (x.Name == _devModel.matrixFormat) {
						_controls._cbMatrixFormat.SelectedItem = x;
						return;
					}
				});
			} catch (Exception err) {
				string strValue;
				strValue = devModel.photosensorPixelSize.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindSensorPixelSize + strValue);
			}
			_controls._cbMatrixFormat.SelectedValueChanged += new EventHandler(_cbMatrixFormat_SelectedValueChanged);

			//_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_controls._saveCancelControl._btnCancel.Enabled = true;
			_controls._saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}
		void panel1_Paint(object sender, PaintEventArgs e) {
			panel1.Paint -= panel1_Paint;

			_regionEditor = new GraphEditor(_devModel.bounds);
			InitUrl();
		}
		void Set2DView() {
			_controls.groupBox1.Enabled = !_controls._rb2D.Checked;
		}
		void _rb2D_Click(object sender, EventArgs e) {
			Set2DView();
			_regionEditor.ReleaseAll();
			LoadRegionEditor();
		}
		void _rbHeight_Click(object sender, EventArgs e) {
			Set2DView();
			_regionEditor.ReleaseAll();
			LoadRegionEditor();
		}		
		void PropertyDepthCalibrationSTA_Load(object sender, EventArgs e) {
			_controls = new PropertyDepthCalibrationSTAControls();
			_controls.Show(this);

			_controls._rb2D.Click += new EventHandler(_rb2D_Click);
			_controls._rbHeight.Click += new EventHandler(_rbHeight_Click);

			panel1.Paint += new PaintEventHandler(panel1_Paint);
			BindData(_devModel);
			InitControls();
		}
		void Localization() {
			this.CreateBinding(x => x.Text, _strings, x => x.title);
			_controls._lblFocalLength.CreateBinding(x => x.Text, _strings, x => x.focalLength);
			_controls._lblMatrixFormat.CreateBinding(x => x.Text, _strings, x => x.matrixFormat);
			//_lblSensorPixel.CreateBinding(x => x.Text, _strings, x => x.sensorPixel);
			//_cbUnknown.CreateBinding(x => x.Text, _strings, x => x.unknow);
			_controls._rb2D.CreateBinding(x => x.Text, _strings, x => x.marker2D);
			_controls._rbHeight.CreateBinding(x => x.Text, _strings, x => x.heightMarker);

			_controls._saveCancelControl._btnCancel.DataBindings.Clear();
			_controls._saveCancelControl._btnCancel.CreateBinding(x => x.Text, _strings, x => x.cancel);
			_controls._saveCancelControl._btnSave.DataBindings.Clear();
			_controls._saveCancelControl._btnSave.CreateBinding(x => x.Text, _strings, x => x.save);
		}
		void AddHeigthMarker(Point P1, Point P2, Size size) {
			_regionEditor.Is2D = _controls._rb2D.Checked;
			_regionEditor.AddHeightMarker(P1, P2, size);
		}
		void LoadRegionEditor() {
			_regionEditor.SetParent(pBox);

			if (_devModel.region != null)
				_regionEditor.AddRegionEditor(_devModel.region);

			dbg.Assert(_devModel.markers != null);
			dbg.Assert(_devModel.markers[0].line1 != null);

			if (_devModel.markers != null && _devModel.markers.Count() > 0) {
				if (_devModel.markers[0].size != null) {
					int pH = ((int)_devModel.markers[0].size.y);
					int pW = ((int)_devModel.markers[0].size.x);

					//convert from mm to cm
					pH = pH / 10;
					pW = pW / 10;

					if (_devModel.markers[0].line1 != null) {
						Point p1UL = new Point() { X = (int)_devModel.markers[0].line1.Point[1].x, Y = (int)_devModel.markers[0].line1.Point[1].y };
						Point p1BR = new Point() { X = (int)_devModel.markers[0].line1.Point[0].x, Y = (int)_devModel.markers[0].line1.Point[0].y };

						AddHeigthMarker(p1UL, p1BR, new Size(pW, pH));
					} else {
						MessageBox.Show("Marker 1 is null");
					}
					if (_devModel.markers[0].line2 != null) {
						Point p2UL = new Point() { X = (int)_devModel.markers[0].line2.Point[1].x, Y = (int)_devModel.markers[0].line2.Point[1].y };
						Point p2BR = new Point() { X = (int)_devModel.markers[0].line2.Point[0].x, Y = (int)_devModel.markers[0].line2.Point[0].y };

						AddHeigthMarker(p2UL, p2BR, new Size(pW, pH));
					} else {
						MessageBox.Show("Marker 2 is null");
					}
				} else {
					dbg.Assert(_devModel.markers[0].size != null, "_devModel.markers[0].size == null");
				}
			}
		}

		void _cbMatrixFormat_SelectedValueChanged(object sender, EventArgs e) {
			_devModel.matrixFormat = ((MatrixValue)_controls._cbMatrixFormat.SelectedItem).Name;
			_controls._saveCancelControl._btnSave.Enabled = true;
		}
		public virtual void ReleaseAll() {
			if (_tmr != null)
				_tmr.Dispose();
		}

		#region DRAW
		//protected Process _vlcProcess;
		//int streamSize;
		//protected string dataFileName;

		public MemoryMappedFile memFile;
		Size _resolution;
		protected void CreateStandAloneVLC(string uri, Size resolution) {
			_resolution = resolution;
		}
		protected System.Windows.Forms.Timer _tmr;
		protected int _width = 0;
		protected int _heigth = 0;
		protected UserPictureBox pBox;
		protected double fps = 0;
		protected long fpsLastUpdateTime;
		protected Stopwatch stopWatch = new Stopwatch();
		protected Bitmap middleBmp = null;
		protected Buffer<long> frameTimespans = new Buffer<long>(100);

		protected Action<Graphics, Rectangle> drwaAction;
		protected void _tmr_Tick(object sender, EventArgs e) {
			//Refresh image
			try {
				var resolution = _resolution;
				if (!stopWatch.IsRunning) {
					frameTimespans.Clear();
					stopWatch.Start();
					fpsLastUpdateTime = stopWatch.ElapsedMilliseconds;
					fps = 1.0;
				}

				frameTimespans.Push(stopWatch.ElapsedMilliseconds);
				//if (stopWatch.ElapsedMilliseconds - fpsLastUpdateTime > 300) {
				fpsLastUpdateTime = stopWatch.ElapsedMilliseconds;
				var time = frameTimespans.last - frameTimespans.first;
				if (time != 0) {
					fps = ((double)frameTimespans.length * 1000) / (double)time;
				} else {
					fps = 0;
				}
				//}


				if (middleBmp == null) {
					middleBmp = new Bitmap(pBox.Width, pBox.Height);
				}

				if (middleBmp.Width != pBox.Width || middleBmp.Height != pBox.Height) {
					if (middleBmp != null) {
						middleBmp.Dispose();
					}
					middleBmp = new Bitmap(pBox.Width, pBox.Height);
				}

				using (var f = memFile.CreateViewStream()) {
					var scan0 = f.SafeMemoryMappedViewHandle.DangerousGetHandle();
					_width = resolution.Width;
					_heigth = resolution.Height;

					using (Bitmap bmp = new Bitmap(_width, _heigth, _width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, scan0)) {

						Rectangle boundRect = GetVideoBounds(pBox.ClientRectangle, new Rectangle(0, 0, _width, _heigth));

						using (var middleGraphics = Graphics.FromImage(middleBmp)) {
							middleGraphics.Clear(Color.Black);
							//middleGraphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, pBox.Width, pBox.Height));
							middleGraphics.DrawImage(bmp, boundRect);

#if SHOW_FPS
							middleGraphics.DrawString(String.Format("rendering fps: {0:F2}", fps), new Font(FontFamily.GenericSerif, 10, FontStyle.Regular), new SolidBrush(Color.LightGreen), 0, 0);
#endif
							if (drwaAction != null) {
								drwaAction(middleGraphics, boundRect);
							}
						}
					}

					if (_wpfControl == null) {
						using (var contolGraphix = pBox.CreateGraphics()) {
							contolGraphix.DrawImageUnscaled(middleBmp, 0, 0);
						}
					} else {
						_wpfControl.SetImage(middleBmp, new System.Windows.Rect(new System.Windows.Size(pBox.Size.Width, pBox.Size.Height)));
					}

				}
			} catch (Exception err) {
				dbg.Error(err);
				string msg = err.Message;
			}
		}
		public Rectangle GetVideoBounds(Rectangle clientRect, Rectangle videoRect) {
			Rectangle r = new Rectangle();

			double kx = ((Double)clientRect.Width) / ((Double)videoRect.Width);
			double ky = ((Double)clientRect.Height) / ((Double)videoRect.Height);

			if (ky > kx) {
				var h = videoRect.Height * kx;
				r.Width = clientRect.Width;
				r.Height = (int)h;
				r.X = 0;
				r.Y = (int)((clientRect.Height - h) * 0.5);
				return r;
			}

			if (kx > ky) {
				var w = videoRect.Width * ky;
				r.Width = (int)w;
				r.Height = clientRect.Height;
				r.X = (int)((clientRect.Width - w) * 0.5);
				r.Y = 0;
				return r;
			}

			r.Width = clientRect.Width;
			r.Height = clientRect.Height;
			r.X = 0;
			r.Y = 0;

			return r;
		}
		#endregion DRAW
	}
}
