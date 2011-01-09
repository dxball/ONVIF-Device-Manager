using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using odm.models;
using odm.utils;
using odm.controllers;
using System.Threading;

namespace odm.controls {
	public partial class PropertyObjectTracker : BasePropertyControl {
		public override void ReleaseUnmanaged() {}
		public PropertyObjectTracker(ObjectTrackerModel devModel) {
			InitializeComponent();
			_devModel = devModel;
			
			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};
			Load += new EventHandler(PropertyObjectTracker_Load);
		}

		void PropertyObjectTracker_Load(object sender, EventArgs e) {
			BindData(_devModel);
			InitControls();
			InitUrl();			
		}

		PropertyObjectTrackerStrings _strings = new PropertyObjectTrackerStrings();

		ObjectTrackerModel _devModel;

		public Action Save { get; set; }
		public Action Cancel { get; set; }

		void BindData(ObjectTrackerModel devModel) {
			try {
				_numtbContrast.CreateBinding(x => x.Value, devModel, x => x.contrastSensitivity);
			} catch (Exception err) {
				BindingError(err, ExceptionStrings.Instance.errBindContrastSensitivity + devModel.contrastSensitivity);
			}

			try {
				_numtbAreaMin.CreateBinding(x => x.Value, devModel, x => x.minObjectArea);
				_numtbAreaMin.CreateBinding(x => x.Maximum, devModel, x => x.maxObjectArea);
			} catch (Exception err) {
				if(devModel.minObjectArea >= devModel.maxObjectArea)
					BindingError(err, ExceptionStrings.Instance.errBindMinMaxObjectArea);
				else
					BindingError(err, ExceptionStrings.Instance.errBindObjectArea);
			}

			try {
				_numtbAreaMax.CreateBinding(x => x.Value, devModel, x => x.maxObjectArea);
				_numtbAreaMax.CreateBinding(x => x.Minimum, devModel, x => x.minObjectArea);
			} catch (Exception err) {
				if (devModel.minObjectArea >= devModel.maxObjectArea)
					BindingError(err, ExceptionStrings.Instance.errBindMinMaxObjectArea);
				else
					BindingError(err, ExceptionStrings.Instance.errBindObjectArea);
			}

			try {
				_numtbSpeedMax.CreateBinding(x => x.Value, devModel, x => x.maxObjectSpeed);
			} catch (Exception err) {
				BindingError(err, ExceptionStrings.Instance.errBindMaxObjectSpeed + devModel.maxObjectSpeed.ToString());
			}

			try {
				_numtbTime.CreateBinding(x => x.Value, devModel, x => x.stabilizationTime);
			} catch (Exception err) {
				BindingError(err, ExceptionStrings.Instance.errBindStabilizationTime + devModel.stabilizationTime.ToString());
			}

			//Init Direction Rose
			_directionRose.SelectionChanged = GetDirectionRoseValues;
			InitDirectionRose();
			
			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}
		bool ConvertToBool(float val) {
			if (val >= 0.5)
				return true;
			else
				return false;
		}
		float ConvertToFloat(bool val) {
			if (val)
				return 1.0f;
			else
				return 0.0f;
		}
		void GetDirectionRoseValues() {
			_devModel.rose_right = ConvertToFloat(_directionRose.drE);
			_devModel.rose_left = ConvertToFloat(_directionRose.drW);

			_devModel.rose_up = ConvertToFloat(_directionRose.drN);
			_devModel.rose_up_right = ConvertToFloat(_directionRose.drNE);
			_devModel.rose_up_left = ConvertToFloat(_directionRose.drNW);

			_devModel.rose_down = ConvertToFloat(_directionRose.drS);
			_devModel.rose_down_right = ConvertToFloat(_directionRose.drSE);
			_devModel.rose_down_left = ConvertToFloat(_directionRose.drSW);

			_saveCancelControl._btnSave.Enabled = true;
		}
		void InitDirectionRose() {
			_directionRose.drE = ConvertToBool(_devModel.rose_right);
			_directionRose.drW = ConvertToBool(_devModel.rose_left);

			_directionRose.drN = ConvertToBool(_devModel.rose_up);
			_directionRose.drNE = ConvertToBool(_devModel.rose_up_right);
			_directionRose.drNW = ConvertToBool(_devModel.rose_up_left);

			_directionRose.drS = ConvertToBool(_devModel.rose_down);
			_directionRose.drSE = ConvertToBool(_devModel.rose_down_right);
			_directionRose.drSW = ConvertToBool(_devModel.rose_down_left);
		}
		void Localization() {
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_lblContrast.CreateBinding(x => x.Text, _strings, x => x.contrast);
			_tbObjAreaMax.CreateBinding(x => x.Text, _strings, x => x.areaMax);
			_tbObjAreaMin.CreateBinding(x => x.Text, _strings, x => x.areaMin);
			_tbSpeedMax.CreateBinding(x => x.Text, _strings, x => x.speedMax);
			_tbStabilization.CreateBinding(x => x.Text, _strings, x => x.stabilization);
			_lbldirection.CreateBinding(x => x.Text, _strings, x => x.direction);
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
			//Save
			Save();
		}
		public void InitUrl() {
			dbg.Assert(SynchronizationContext.Current != null);
		
			try {
				CreateStandAloneVLC(_devModel.mediaUri, _devModel.encoderResolution);
				pBox = new UserPictureBox() { Dock = DockStyle.Fill };
				panel1.Controls.Add(pBox);
				_tmr = new System.Windows.Forms.Timer();
				_tmr.Interval = 10; // refresh 10 time per second
				_tmr.Tick += new EventHandler(_tmr_Tick);
				_tmr.Start();
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
		}

		public override void ReleaseAll() {
			base.ReleaseAll();
		}

	}
}
