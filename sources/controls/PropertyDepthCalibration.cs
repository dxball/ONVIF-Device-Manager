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
using nvc.models;
using System.Threading;
using onvifdm.utils;

namespace nvc.controls {
	public partial class PropertyDepthCalibration : BasePropertyControl {
		private DepthCalibrationStrings strings = DepthCalibrationStrings.Instance;
		public override void ReleaseUnmanaged() {
			if (_vidPlayer != null) {
				_vidPlayer.ReleaseUnmanaged();
			}
		}
		public PropertyDepthCalibration(DepthCalibrationModel devModel) {
			InitializeComponent();
			_devModel = devModel;

			panel1.Paint += new PaintEventHandler(panel1_Paint);

			BindData(devModel);
			InitControls();
		}

		DepthCalibrationModel _devModel;
		GraphEditor _regionEditor;
		VideoPlayerControl _vidPlayer;

		public Action Save { get; set; }
		public Action Cancel { get; set; }

		void panel1_Paint(object sender, PaintEventArgs e) {
			panel1.Paint -= panel1_Paint;

			_regionEditor = new GraphEditor(_devModel.bounds);
			InitUrl();
		}
		void LoadRegionEditor() {
			_regionEditor.SetParent(_vidPlayer.m_VlcControl);

			_regionEditor.AddRegionEditor(_devModel.region);

			int pH = (_devModel.markers[0].height)/10;
			DebugHelper.Assert(_devModel.markers != null);
			DebugHelper.Assert(_devModel.markers[0].line1 != null);
			Point p1UL = new Point() { X = (int)_devModel.markers[0].line1.Point[0].x, Y = (int)_devModel.markers[0].line1.Point[0].y };
			Point p1BR = new Point() { X = (int)_devModel.markers[0].line1.Point[1].x, Y = (int)_devModel.markers[0].line1.Point[1].y };

			Point p2UL = new Point() { X = (int)_devModel.markers[0].line2.Point[0].x, Y = (int)_devModel.markers[0].line2.Point[0].y };
			Point p2BR = new Point() { X = (int)_devModel.markers[0].line2.Point[1].x, Y = (int)_devModel.markers[0].line2.Point[1].y };

			_regionEditor.AddHeightMarker(p1UL, p1BR, pH);
			_regionEditor.AddHeightMarker(p2UL, p2BR, pH);
		}
		public void InitUrl() {
			DebugHelper.Assert(SynchronizationContext.Current != null);
			_vidPlayer = new VideoPlayerControl(_devModel.mediaUri) { Dock = DockStyle.Fill , _mediaStreamSize = _devModel.encoderResolution};
			_vidPlayer.Dock = DockStyle.Fill;
			panel1.CausesValidation = false;
			_vidPlayer.CausesValidation = false;
			_vidPlayer._action = _regionEditor.FillBitmap;
			panel1.Controls.Add(_vidPlayer);
			panel1.Capture = true;

			LoadRegionEditor();
		}

		void BindData(DepthCalibrationModel devModel) {
			_tbFocalLength.CreateBinding(x => x.Text, devModel, x => x.focalLength);
			_tbSensor.CreateBinding(x => x.Text, devModel, x => x.photosensorPixelSize);
			//_cbMatrix.CreateBinding(x => x.Text, devModel, x => x.matrixFormat);
			_tbMatrixFormat.CreateBinding(x => x.Text, devModel, x => x.matrixFormat);

			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, strings, x => x.title);
			_lblFocalLength.CreateBinding(x => x.Text, strings, x => x.focalLength);
			_lblMatrixFormat.CreateBinding(x => x.Text, strings, x => x.matrixFormat);
			_lblSensorPixel.CreateBinding(x => x.Text, strings, x => x.sensorPixel);
			_cbUnknown.CreateBinding(x => x.Text, strings, x => x.unknow);
			_rb2D.CreateBinding(x => x.Text, strings, x => x.marker2D);
			_rbHeight.CreateBinding(x => x.Text, strings, x => x.heightMarker);

			_saveCancelControl._btnSave.DataBindings.Clear();
			_saveCancelControl._btnSave.CreateBinding(x => x.Text, strings, x => x.save);
		}

		void InitControls() {
			Localization();

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
			_devModel.region = _regionEditor.GetRegion();
			var retval = _regionEditor.GetMarkers();
			_devModel.markers[0].height = retval[0].pheight * 10;
			_devModel.markers[0].line1.Point[0] = new global::onvif.types.Vector() { x = retval[0].top.X, y = retval[0].top.Y };
			_devModel.markers[0].line1.Point[1] = new global::onvif.types.Vector() { x = retval[0].bottom.X, y = retval[0].bottom.Y };
			
			_devModel.markers[0].height = retval[1].pheight * 10;
			_devModel.markers[0].line2.Point[0] = new global::onvif.types.Vector() { x = retval[1].top.X, y = retval[1].top.Y };
			_devModel.markers[0].line2.Point[1] = new global::onvif.types.Vector() { x = retval[1].bottom.X, y = retval[1].bottom.Y }; 

			Save();
		}
		public void ReleaseAll() {
			if (_regionEditor != null)
				_regionEditor.ReleaseAll();
			if (_vidPlayer != null)
				_vidPlayer.ReleaseAll();
		}

		private void _cbUnknown_CheckedChanged(object sender, EventArgs e) {
			_lblSensorPixel.Enabled = !_cbUnknown.Checked;
			_tbSensor.Enabled = !_cbUnknown.Checked;
		}
	}
}
