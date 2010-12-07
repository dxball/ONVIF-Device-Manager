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
using nvc.entities;
using nvc.models;
using onvifdm.utils;
using System.Threading;

namespace nvc.controls
{
    public partial class PropertyVideoStreaming : BasePropertyControl
    {
		PropertyVideoStreamingStrings _strings = new PropertyVideoStreamingStrings();
		VideoStreamingModel _devModel;

		//Initial values
		int _initialEncodingInteval;
		
		public Action Save { get; set; }
		public Action Cancel { get; set; }

		public override void ReleaseUnmanaged() { }
		public PropertyVideoStreaming(VideoStreamingModel devModel)
        {
            InitializeComponent();
			_devModel = devModel;
			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};

			Load += new EventHandler(PropertyVideoStreaming_Load);
        }

		void PropertyVideoStreaming_Load(object sender, EventArgs e) {
			IniControls();
			InitUrl();

			BindData(_devModel);

			SubscribeToEvents();
		}

		void _cbFPS_SelectionChangeCommitted(object sender, EventArgs e) {
			_saveCancelControl.EnableSave(true);
			//_saveCancelControl.EnableCancel(true);
		}

		//public void StopdStreaming() {
		//    //if (_vlcProcess != null)
		//    //    _vlcProcess.Kill();
		//}
		//public void PlaydStreaming() {
		//    //InitUrl();
		//}
		public void InitUrl() {
			DebugHelper.Assert(SynchronizationContext.Current != null);

			//Start Workaround
			try {
				CreateStandAloneVLC(_devModel.mediaUri, new Size(_devModel.currentResolution.width, _devModel.currentResolution.height));
				if (_tmr != null)
					_tmr.Dispose();
				_tmr = new System.Windows.Forms.Timer();
				_tmr.Interval = 10; // refresh 100 time per second
				_tmr.Tick += new EventHandler(_tmr_Tick);
				_tmr.Start();
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
			//Stop Workaround
		}

		void SubscribeToEvents() {
			_saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
			_saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
		}

		public override void ReleaseAll() {
			base.ReleaseAll();
		}
		void InitFps(VideoEncoder encoder) {
			try {
				if (encoder.maxFrameRate == encoder.minFrameRate) {
					_numtbFPS.Value = encoder.maxFrameRate;
					_numtbFPS.Enabled = false;
				} else {
					_numtbFPS.Enabled = true;
					try {
						_numtbFPS.Maximum = _devModel.frameRate > encoder.maxFrameRate ? _devModel.frameRate : encoder.maxFrameRate;
						_numtbFPS.Minimum = _devModel.frameRate < encoder.minFrameRate ? _devModel.frameRate : encoder.minFrameRate;
						_numtbFPS.DataBindings.Clear();
						_numtbFPS.CreateBinding(x => x.Value, _devModel, x => x.frameRate);
					} catch (Exception err) {
						if (encoder == null) {
							BindingError(err, ExceptionStrings.Instance.errBindVStrCurrentEncoder + "Null");
						} else {
							if (encoder.maxFrameRate < encoder.minFrameRate)
								BindingError(err, ExceptionStrings.Instance.errBindVStrFrameRateMinMax);
							else {
								BindingError(err, ExceptionStrings.Instance.errBindVStrFrameRate + _devModel.frameRate);
							}
						}
					}
				}
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
		}
		void InitEncodingInterval(VideoEncoder encoder) {
			_trackBar.Minimum = _devModel.encodingInterval < encoder.minEncodingInterval ? _devModel.encodingInterval : encoder.minEncodingInterval;
			_trackBar.Maximum = _devModel.encodingInterval > encoder.maxEncodingInterval? _devModel.encodingInterval : encoder.maxEncodingInterval;
			_trackBar.Value = _devModel.encodingInterval;
			_initialEncodingInteval = _devModel.encodingInterval;
			SetEncodingIntervalText();
		}
		void SetEncodingIntervalText() {
			_lblEncodingValue.Text = _trackBar.Value.ToString();
		}
		public void RefershBindings() {
			_numtbBitrate.DataBindings.Clear();
			_cmbEncoder.DataBindings.Clear();
			_numtbFPS.DataBindings.Clear();
			_cmbResolution.DataBindings.Clear();
			_cbMetadata.DataBindings.Clear();
			_tbChannelName.DataBindings.Clear();
			_saveCancelControl._btnCancel.DataBindings.Clear();
			_saveCancelControl._btnSave.DataBindings.Clear();
			
			BindData(_devModel);
		}
		void BindData(VideoStreamingModel devModel) {
			try {
				_numtbBitrate.Maximum = 100000;// devModel.maxBitrate;
				_numtbBitrate.Minimum = 0; // devModel.minBitrate;
				_numtbBitrate.CreateBinding(x => x.Value, devModel, x => x.bitrate);
			} catch(Exception err) {
				//if (devModel.Minimum >= devModel.Maximum)
				//    BindingError(err, ExceptionStrings.Instance.errBindVStrBitrateMinMax);
				//else
					BindingError(err, ExceptionStrings.Instance.errBindVStrBitrate + devModel.bitrate);
			}
			try {
				InitFps(devModel.currentEncoder);
				InitEncodingInterval(devModel.currentEncoder);
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}

			_cmbEncoder.Items.Clear();
			if (devModel.supportedEncoders != null) {
				_cmbEncoder.Items.AddRange(devModel.supportedEncoders.ToArray());
			}
			try {
				_cmbEncoder.CreateBinding(x => x.SelectedItem, devModel, x => x.currentEncoder);
			} catch (Exception err) {
				if (devModel.currentEncoder == null)
					BindingError(err, ExceptionStrings.Instance.errBindVStrCurrentEncoder + "Null");
				else
					BindingError(err, ExceptionStrings.Instance.errBindVStrBitrate + devModel.currentEncoder.ToString());
			}
			_cmbEncoder.SelectedValueChanged += (sender, args) => {
				var value = _cmbEncoder.SelectedItem as VideoEncoder;
				if(value != null){
					devModel.currentEncoder = value;
				}
			};
			_cmbResolution.Items.Clear();
			if (devModel.supportedResolutions != null)
				_cmbResolution.Items.AddRange(devModel.supportedResolutions.ToArray());
			try {
				_cmbResolution.CreateBinding(x => x.SelectedItem, devModel, x => x.currentResolution);
			} catch (Exception err) {
				if (devModel.currentResolution == null)
					BindingError(err, ExceptionStrings.Instance.errBindVStrcurrentResolution + "Null");
				else
					BindingError(err, ExceptionStrings.Instance.errBindVStrcurrentResolution + devModel.currentResolution.ToString());
			}
			_cmbResolution.SelectedValueChanged += (sender, args) => {
				var value = _cmbResolution.SelectedItem as VideoResolution;
				if (value != null) {
					devModel.currentResolution = value;
				}
			};

			_cbMetadata.CreateBinding(x => x.Checked, devModel, x => x.metadata);
			
			try {
				_tbChannelName.CreateBinding(x => x.Text, devModel, x => x.channelName);
			} catch (Exception err) {
				if (devModel.channelName == null)
					BindingError(err, ExceptionStrings.Instance.errBindVStrChannelName + "Null");
				else
					BindingError(err, ExceptionStrings.Instance.errBindVStrChannelName + devModel.channelName);
			}

			_cmbPriority.Enabled = false;

			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}

		void Localization(){
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_lblBitrate.CreateBinding(x => x.Text, _strings, x => x.bitrate);
			_lblEncoder.CreateBinding(x => x.Text, _strings, x => x.encoder);
			_lblFrameRate.CreateBinding(x => x.Text, _strings, x => x.frameRate);
			_lblEncodingInterval.CreateBinding(x => x.Text, _strings, x => x.encodingInterval);
			_lblPriority.CreateBinding(x => x.Text, _strings, x => x.prioriy);
			_lblResolution.CreateBinding(x => x.Text, _strings, x => x.resolution);
			_tbMetadata.CreateBinding(x => x.Text, _strings, x => x.metadata);
			_lblChannelName.CreateBinding(x => x.Text, _strings, x => x.channelName);
		}
        protected void IniControls(){
			Localization();
			
			pBox = new UserPictureBox() { Dock = DockStyle.Fill};
			panel1.Controls.Add(pBox);

			_cmbEncoder.SelectionChangeCommitted += new EventHandler(_cmbEncoder_SelectionChangeCommitted);
			_trackBar.ValueChanged += new EventHandler(_trackBar_ValueChanged);

			//Colors
			BackColor = ColorDefinition.colControlBackground;
			_title.BackColor = ColorDefinition.colTitleBackground;
        }

		void _trackBar_ValueChanged(object sender, EventArgs e) {
			SetEncodingIntervalText();
			if (_initialEncodingInteval != _trackBar.Value) {
				_saveCancelControl._btnSave.Enabled = true;
				_saveCancelControl._btnCancel.Enabled = true;
			}
		}

		void _cmbEncoder_SelectionChangeCommitted(object sender, EventArgs e) {
			var encInterval = (VideoEncoder)_cmbEncoder.SelectedItem;
			InitFps(encInterval);
			InitEncodingInterval(encInterval);
		}

		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			_trackBar.Value = _initialEncodingInteval;
			Cancel();
		}

		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			_devModel.encodingInterval = _trackBar.Value;
			Save();
		}

    }
	public class EncoderView {
		public EncoderView(nvc.models.VideoEncoder.Encoding enc) {
			_encoder = new VideoEncoder(enc);
		}
		nvc.models.VideoEncoder _encoder;
		public nvc.models.VideoEncoder GetVideoEncoder() {
			return _encoder;
		}
		public void SetVideoEncoder(nvc.models.VideoEncoder value) {
			_encoder = value;
		}
		public override string ToString() {
			return _encoder.encoding.ToString();
		}
	}
	public class ResolutionView {
		public override bool Equals(object obj) {
			if (obj.GetType() == typeof(ResolutionView))
				return ((ResolutionView)obj).ToString() == this.ToString();
			else
				return false;
		}
		public override int GetHashCode() {
			return Resolution.width + Resolution.height;
		}
		public ResolutionView(VideoResolution resol) {
			Resolution = resol;
		}
		public VideoResolution Resolution { get; set; }
		public override string ToString() {
			return Resolution.ToString();
		}
	}
	public class FPSview {
		public FPSview(int missFrame, int name) {
			MissingFrames = missFrame;
			Text = name.ToString();
		}
		public int MissingFrames { get; private set; }
		public string Text { get; set; }
	}
}
