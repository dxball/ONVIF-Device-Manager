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

namespace nvc.controls
{
    public partial class PropertyVideoStreaming : BasePropertyControl
    {
		VideoStreamingModel _devModel;
		VideoPlayerControl _vidPlayer;
		
		public Action Save { get; set; }
		public Action Cancel { get; set; }

		public PropertyVideoStreaming(VideoStreamingModel devModel)
        {
            InitializeComponent();
			_devModel = devModel;
			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};

			BindData(devModel);
            IniControls();
			InitUrl();

			SubscribeToEvents();
        }
		public void StopStreaming() {
			if (_vidPlayer != null) {
				_vidPlayer.Stop();
			}
		}
		public void PlayStreaming() {
			if (_vidPlayer != null) {
				_vidPlayer.Play(_devModel.mediaUri);
			}
		}
		public void InitUrl() {
			_vidPlayer = new VideoPlayerControl(_devModel.mediaUri) { Dock = DockStyle.Fill};
			panel1.Controls.Add(_vidPlayer);
		}

		void SubscribeToEvents() {
			_saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
			_saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
		}

		public void ReleaseAll() {
			_vidPlayer.ReleaseAll();
		}

		void BindData(VideoStreamingModel devModel) {
			_numtbBitrate.Maximum = 100000;// devModel.maxBitrate;
			_numtbBitrate.Minimum = 0; // devModel.minBitrate;
			_numtbFPS.Maximum = 100000;// devModel.maxFrameRate;
			_numtbFPS.Minimum = 0;// devModel.minFrameRate;

			_numtbBitrate.CreateBinding(x => x.Value, devModel, x => x.bitrate);
			_numtbFPS.CreateBinding(x => x.Value, devModel, x => x.frameRate);

			if(devModel.supportedEncoders!= null)
				_cmbEncoder.Items.AddRange(devModel.supportedEncoders.ToArray());
			_cmbEncoder.CreateBinding(x => x.SelectedItem, devModel, x => x.currentEncoder);
			_cmbEncoder.SelectedValueChanged += (sender, args) => {
				var value = _cmbEncoder.SelectedItem as VideoEncoder;
				if(value != null){
					devModel.currentEncoder = value;
				}
			};
			if (devModel.supportedResolutions != null)
				_cmbResolution.Items.AddRange(devModel.supportedResolutions.ToArray());
			_cmbResolution.CreateBinding(x => x.SelectedItem, devModel, x => x.currentResolution);
			_cmbResolution.SelectedValueChanged += (sender, args) => {
				var value = _cmbResolution.SelectedItem as VideoResolution;
				if (value != null) {
					devModel.currentResolution = value;
				}
			};

			_cmbPriority.Enabled = false;

			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}
		void Localization(){
			_title.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyVideoStreamingTitle);
			_lblBitrate.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyVideoStreamingBitrate);
			_lblEncoder.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyVideoStreamingEncoder);
			_lblFrameRate.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyVideoStreamingFrameRate);
			_lblPriority.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyVideoStreamingPrioriy);
			_lblResolution.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyVideoStreamingResolution);
		}
        protected void IniControls(){
			Localization();

			_cmbPriority.Items.Add(Constants.Instance.sVideoPriorityStreaming);
			_cmbPriority.Items.Add(Constants.Instance.sVideoPriorityAnalytics);
			//_cmbResolution.Items.Add(Constants.Instance.sVideoResolution720x576);

			//Colors
			BackColor = ColorDefinition.colControlBackground;
			_title.BackColor = ColorDefinition.colTitleBackground;
        }

		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			Cancel();
		}

		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
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
}
