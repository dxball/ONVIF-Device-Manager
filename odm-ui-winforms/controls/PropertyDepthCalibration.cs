#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using odm.models;
using System.Threading;
using odm.utils;
using odm.controls.regionEditor;

namespace odm.controls {
	public partial class PropertyDepthCalibration : BasePropertyControl {
		PropertyDepthCalibrationStrings _strings = new PropertyDepthCalibrationStrings();
		public override void ReleaseUnmanaged() {
		}
		public PropertyDepthCalibration(DepthCalibrationModel devModel) {
			InitializeComponent();
			_devModel = devModel;
			
			_rb2D.Click += new EventHandler(_rb2D_Click);
			_rbHeight.Click += new EventHandler(_rbHeight_Click);
			
			Load += new EventHandler(PropertyDepthCalibration_Load);
		}

		void _rbHeight_Click(object sender, EventArgs e) {
			Set2DView();
			_regionEditor.ReleaseAll();
			LoadRegionEditor();
		}

		void Set2DView() {
			groupBox1.Enabled = !_rb2D.Checked;
		}

		void _rb2D_Click(object sender, EventArgs e) {
			Set2DView();
			_regionEditor.ReleaseAll();
			LoadRegionEditor();
		}

		void PropertyDepthCalibration_Load(object sender, EventArgs e) {
			panel1.Paint += new PaintEventHandler(panel1_Paint);
			BindData(_devModel);
			InitControls();			
		}

		DepthCalibrationModel _devModel;
		GraphEditor _regionEditor;

		public Action Save { get; set; }
		public Action Cancel { get; set; }

		void panel1_Paint(object sender, PaintEventArgs e) {
			panel1.Paint -= panel1_Paint;

			_regionEditor = new GraphEditor(_devModel.bounds);
			InitUrl();
		}
		void AddHeigthMarker(Point P1, Point P2, Size size) {
			_regionEditor.Is2D = _rb2D.Checked;
			_regionEditor.AddHeightMarker(P1, P2, size);
		}
		void LoadRegionEditor() {
			_regionEditor.SetParent(pBox);

			if(_devModel.region != null)
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
		public void InitUrl() {
			dbg.Assert(SynchronizationContext.Current != null);

			try {
				CreateStandAloneVLC(_devModel.mediaUri, _devModel.encoderResolution);
				pBox = new UserPictureBox() { Dock = DockStyle.Fill};
				panel1.Controls.Add(pBox);
				_tmr = new System.Windows.Forms.Timer();				
				_tmr.Interval = 10; // refresh 100 time per second
				_tmr.Tick += new EventHandler(_tmr_Tick);
				drwaAction = _regionEditor.FillBitmap;
				_tmr.Start();
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}

			LoadRegionEditor();
		}

		void BindData(DepthCalibrationModel devModel) {
			try {
				_tbFocalLength.CreateBinding(x => x.Text, devModel, x => x.focalLength);
			} catch (Exception err) {
				string strValue;
				strValue = devModel.focalLength.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindFocalLength + strValue);
			}
			try {
				_cbMatrixFormat.DataSource = MatrixTable.MatrixTbl;
				_cbMatrixFormat.DisplayMember = "Name";
				_cbMatrixFormat.ValueMember = "Value";

				MatrixTable.MatrixTbl.ForEach<MatrixValue>(x => {
					if (x.Name == _devModel.matrixFormat) {
						_cbMatrixFormat.SelectedItem = x;
						return;
					}
				});
			} catch (Exception err) {
				string strValue;
				strValue = devModel.photosensorPixelSize.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindSensorPixelSize + strValue);
			}
			_cbMatrixFormat.SelectedValueChanged += new EventHandler(_cbMatrixFormat_SelectedValueChanged);
			
			//_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnCancel.Enabled = false;
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}
		
		void _cbMatrixFormat_SelectedValueChanged(object sender, EventArgs e) {
			_devModel.matrixFormat = ((MatrixValue)_cbMatrixFormat.SelectedItem).Name;
			_saveCancelControl._btnSave.Enabled = true;
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_lblFocalLength.CreateBinding(x => x.Text, _strings, x => x.focalLength);
			_lblMatrixFormat.CreateBinding(x => x.Text, _strings, x => x.matrixFormat);
			//_lblSensorPixel.CreateBinding(x => x.Text, _strings, x => x.sensorPixel);
			//_cbUnknown.CreateBinding(x => x.Text, _strings, x => x.unknow);
			_rb2D.CreateBinding(x => x.Text, _strings, x => x.marker2D);
			_rbHeight.CreateBinding(x => x.Text, _strings, x => x.heightMarker);

			_saveCancelControl._btnSave.DataBindings.Clear();
			_saveCancelControl._btnSave.CreateBinding(x => x.Text, _strings, x => x.save);
		}

		void InitControls() {
			Localization();

			if (!_devModel.is2DmarkerSupported)
				_rb2D.Enabled = false;
			else {
				_rb2D.Checked = _devModel.use2DMarkers;
				if (_rb2D.Checked)
					groupBox1.Enabled = false;
			}
			//Color
			_title.BackColor = ColorDefinition.colTitleBackground;
			BackColor = ColorDefinition.colControlBackground;

			_saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
			_saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
		}

		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			//Cancel
			Cancel();
		}
		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			_devModel.use2DMarkers = _rb2D.Checked;
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
		public override void ReleaseAll() {
			if (_regionEditor != null)
				_regionEditor.ReleaseAll();
			base.ReleaseAll();
		}

		private void _cbUnknown_CheckedChanged(object sender, EventArgs e) {
			//_lblSensorPixel.Enabled = !_cbUnknown.Checked;
			//_tbSensor.Enabled = !_cbUnknown.Checked;
		}
	}
}
