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

namespace odm.controls {
	public partial class PropertyImagingSettings : BasePropertyControl {
		public PropertyImagingSettings(ImagingSettingsModel devModel) {
			InitializeComponent();

			_devModel = devModel;

			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};

			Load += new EventHandler(PropertyImagingSettings_Load);
		}

		void PropertyImagingSettings_Load(object sender, EventArgs e) {
			BindData(_devModel);
			InitControls();
			InitUrl();	
		}

		PropertySensorSettingsStrings _strings = new PropertySensorSettingsStrings();

		ImagingSettingsModel _devModel;
		public Action Save { get; set; }
		public Action Cancel { get; set; }

		void BindData(ImagingSettingsModel devModel) {
			try {
				if (!float.IsNaN(_devModel.brightnessMax) && !float.IsNaN(_devModel.brightnessMin)) {
					_tbarBrightness.fMaximum = _devModel.brightnessMax;
					_tbarBrightness.fMinimum = _devModel.brightnessMin;
					_tbarBrightness.CreateBinding(x => x.fValue, _devModel, x => x.brightness);
				} else {
					_lblBrightness.Enabled = false;
					_tbarBrightness.Enabled = false;
				}
			} catch (Exception err) {
			      BindingError(err, "brightness value error. val:" + _devModel.brightness);
			}
			try {
				if (!float.IsNaN(_devModel.contrastMax) && !float.IsNaN(_devModel.contrastMin)) {
					_tbarContrast.fMaximum = _devModel.contrastMax;
					_tbarContrast.fMinimum = _devModel.contrastMin;
					_tbarContrast.CreateBinding(x => x.fValue, _devModel, x => x.contrast);
				} else {
					_lblContrast.Enabled = false;
					_tbarContrast.Enabled = false;
				}
			} catch (Exception err) {
				BindingError(err, "contrast value error. val:" + _devModel.contrast);
			}
			try {
				if (!float.IsNaN(_devModel.colorSaturationMax) && !float.IsNaN(_devModel.colorSaturationMin)) {
					_tbarSaturation.fMaximum = _devModel.colorSaturationMax;
					_tbarSaturation.fMinimum = _devModel.colorSaturationMin;
					_tbarSaturation.CreateBinding(x => x.fValue, _devModel, x => x.colorSaturation);
				} else {
					_lblSaturation.Enabled = false;
					_tbarSaturation.Enabled = false;
				}
			} catch (Exception err) {
				BindingError(err, "color saturation value error. val:" + _devModel.colorSaturation);
			}
			try {
				if (!float.IsNaN(_devModel.sharpnessMax) && !float.IsNaN(_devModel.sharpnessMin)) {
					_tbarSharpness.fMaximum = _devModel.sharpnessMax;
					_tbarSharpness.fMinimum = _devModel.sharpnessMin;
					_tbarSharpness.CreateBinding(x => x.fValue, _devModel, x => x.sharpness);
				} else {
					_lblSharpness.Enabled = false;
					_tbarSharpness.Enabled = false;
				}
			} catch (Exception err) {
				BindingError(err, "Sharpness value error. val:" + _devModel.sharpness);
			}
			_chbAutoWhiteBalance.CheckedChanged += new EventHandler(_chbAutoWhiteBalance_CheckedChanged);
			if (_devModel.whiteBalance.Mode == global::onvif.services.imaging.WhiteBalanceMode.AUTO) {
				_chbAutoWhiteBalance.Checked = true;
				SetCBCROff();
			} else {
				_chbAutoWhiteBalance.Checked = false;
				SetCBCROn();
			}
			
			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}

		void _chbAutoWhiteBalance_CheckedChanged(object sender, EventArgs e) {
			if (_chbAutoWhiteBalance.Checked) {
				_devModel.whiteBalance.Mode = global::onvif.services.imaging.WhiteBalanceMode.AUTO;
				_saveCancelControl._btnSave.Enabled = true;
				SetCBCROff();
			} else {
				_devModel.whiteBalance.Mode = global::onvif.services.imaging.WhiteBalanceMode.MANUAL;
				_saveCancelControl._btnSave.Enabled = true;
				SetCBCROn();
			}
		}
		void SetCBCROff(){
			_lblWhiteBalanceCB.Enabled = false;
			_lblWhiteBalanceCR.Enabled = false;
			_tbarWhiteBalanceCB.Enabled = false;
			_tbarWhiteBalanceCR.Enabled = false;
		}
		void SetCBCROn() {
			_tbarWhiteBalanceCB.Enabled = true;
			_tbarWhiteBalanceCR.Enabled = true;
			_lblWhiteBalanceCB.Enabled = true;
			_lblWhiteBalanceCR.Enabled = true;

			_tbarWhiteBalanceCB.DataBindings.Clear();
			_lblMaxCb.DataBindings.Clear();
			_tbarWhiteBalanceCR.DataBindings.Clear();
			_lblMaxCr.DataBindings.Clear();

			try {
				if (!float.IsNaN(_devModel.whiteBalanceOptions.YbGain.Max) && !float.IsNaN(_devModel.whiteBalanceOptions.YbGain.Min)) {
					_tbarWhiteBalanceCB.fMaximum = _devModel.whiteBalanceOptions.YbGain.Max;
					_tbarWhiteBalanceCB.fMinimum = _devModel.whiteBalanceOptions.YbGain.Min;
					_tbarWhiteBalanceCB.CreateBinding(x => x.fValue, _devModel.whiteBalance, x => x.CbGain);
				} else {
					_lblWhiteBalanceCB.Enabled = false;
					_tbarWhiteBalanceCB.Enabled = false;
				}
			} catch (Exception err) {
				//BindingError(err, "White balance Cb gain value error. val:" + _devModel.whiteBalance.CbGain);
			}
			try {
				if (!float.IsNaN(_devModel.whiteBalanceOptions.YrGain.Max) && !float.IsNaN(_devModel.whiteBalanceOptions.YrGain.Min)) {
					_tbarWhiteBalanceCR.fMaximum = _devModel.whiteBalanceOptions.YrGain.Max;
					_tbarWhiteBalanceCR.fMinimum = _devModel.whiteBalanceOptions.YrGain.Min;
					_tbarWhiteBalanceCR.CreateBinding(x => x.fValue, _devModel.whiteBalance, x => x.CrGain);
				} else {
					_lblWhiteBalanceCR.Enabled = false;
					_tbarWhiteBalanceCR.Enabled = false;
				}
			} catch (Exception err) {
				//BindingError(err, "White balance Cr gain value error. val:" + _devModel.whiteBalance.CrGain);
			}
		}

		void _cmbWhiteBalance_SelectionChangeCommitted(object sender, EventArgs e) {
			////
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_lblBrightness.CreateBinding(x => x.Text, _strings, x => x.brightness);
			_lblContrast.CreateBinding(x => x.Text, _strings, x => x.contrast);
			_lblSaturation.CreateBinding(x => x.Text, _strings, x => x.saturation);
			_lblSharpness.CreateBinding(x => x.Text, _strings, x => x.sharpness);
			_lblWhiteBalanceCB.CreateBinding(x => x.Text, _strings, x => x.whiteBalanceCb);
			_lblWhiteBalanceCR.CreateBinding(x => x.Text, _strings, x => x.whiteBalanceCr);
			_lblWhiteBalanceMode.CreateBinding(x => x.Text, _strings, x => x.whitemode);
			
			_lblMaxCb.CreateBinding(x => x.Text, _strings, x => x.max);
			_lblMaxCr.CreateBinding(x => x.Text, _strings, x => x.max);
			_lblMaxBr.CreateBinding(x => x.Text, _strings, x => x.max);
			_lblMaxContr.CreateBinding(x => x.Text, _strings, x => x.max);
			_lblMaxSatur.CreateBinding(x => x.Text, _strings, x => x.max);
			_lblMaxSharp.CreateBinding(x => x.Text, _strings, x => x.max);
			
			_lblMinBr.CreateBinding(x => x.Text, _strings, x => x.min);
			_lblMinCb.CreateBinding(x => x.Text, _strings, x => x.min);
			_lblMinContr.CreateBinding(x => x.Text, _strings, x => x.min);
			_lblMinCr.CreateBinding(x => x.Text, _strings, x => x.min);
			_lblMinSatur.CreateBinding(x => x.Text, _strings, x => x.min);
			_lblMinSharp.CreateBinding(x => x.Text, _strings, x => x.min);
		}
		void InitControls() {
			Localization();

			//Color
			_title.BackColor = ColorDefinition.colTitleBackground;
			BackColor = ColorDefinition.colControlBackground;

			_tbarWhiteBalanceCR.ValueChanged += new EventHandler(_tbarWhiteBalanceCR_ValueChanged);
			_tbarWhiteBalanceCB.ValueChanged += new EventHandler(_tbarWhiteBalanceCR_ValueChanged);

			_saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
			_saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
		}

		void _tbarWhiteBalanceCR_ValueChanged(object sender, EventArgs e) {
			_saveCancelControl._btnSave.Enabled = true;
		}

		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			Save();
		}

		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			Cancel();
		}
		public void InitUrl() {
			dbg.Assert(SynchronizationContext.Current != null);

			try {
				CreateStandAloneVLC(_devModel.mediaUri, _devModel.encoderResolution);
				pBox = new UserPictureBox() { Dock = DockStyle.Fill };
				panel1.Controls.Add(pBox);
				_tmr = new System.Windows.Forms.Timer();
				_tmr.Interval = 5; // refresh 10 time per second
				_tmr.Tick += new EventHandler(_tmr_Tick);
				_tmr.Start();
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
		}

	}
}
