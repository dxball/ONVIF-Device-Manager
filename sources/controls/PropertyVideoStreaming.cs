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
//
//----------------------------------------------------------------------------------------------------------------

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
		DeviceChannel _devModel;
		VideoPlayerControl _vidPlayer;
        public PropertyVideoStreaming(DeviceChannel devModel)
        {
			_devModel = devModel;
            InitializeComponent();

			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};

			this.Disposed += (sender, args) => {
				//this.ReleaseAll();
			};

            IniControls();
			LoadData();

			_vidPlayer = new VideoPlayerControl(_devModel.MediaStreamUri);
			panel1.Controls.Add(_vidPlayer);
        }
		public void ReleaseAll() {
			_vidPlayer.ReleaseAll();
		}

		void Localisation(){
			_title.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyVideoStreamingTitle"));
			_lblBitrate.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyVideoStreamingBitrate"));
			_lblEncoder.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyVideoStreamingEncoder"));
			_lblFrameRate.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyVideoStreamingFrameRate"));
			_lblPriority.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyVideoStreamingPrioriy"));
			_lblResolution.DataBindings.Add(new Binding("Text", Constants.Instance, "sPropertyVideoStreamingResolution"));
		}
        protected void IniControls(){
			Localisation();

			_saveCancelControl.EnableSave(false);
			_saveCancelControl.EnableCancel(false);

            _numtbFPS.Minimum = Defaults.iPropertyVideoStreamingFrameRateMin;
            _numtbFPS.Maximum = Defaults.iPropertyVideoStreamingFrameRateMax;
            
            _numtbBitrate.Minimum = Defaults.iPropertyVideoStreamingBitrateMin;
            _numtbBitrate.Maximum = Defaults.iPropertyVideoStreamingBitrateMax;


			foreach (var value in _devModel.ResolutionsList) {
				_cmbResolution.Items.Add(new ResolutionView(value));
			}

			foreach (var value in _devModel.EncodersList) {
				_cmbEncoder.Items.Add(new EncoderView(value));
			}

			_cmbPriority.Items.Add(Constants.Instance.sVideoPriorityStreaming);
			_cmbPriority.Items.Add(Constants.Instance.sVideoPriorityAnalytics);
			//_cmbResolution.Items.Add(Constants.Instance.sVideoResolution720x576);

			//Colors
			BackColor = ColorDefinition.colControlBackground;
			_title.BackColor = ColorDefinition.colTitleBackground;
        }

		void LoadData() {

			//var resolution = _devModel.GetModelChannel().resolution.width + "x" + _devModel.GetModelChannel().resolution.height;
			EncoderView[] encodersArr = new EncoderView[_cmbEncoder.Items.Count];
			_cmbEncoder.Items.CopyTo(encodersArr, 0);
			_cmbEncoder.SelectedItem = encodersArr.Where(x => x.GetVideoEncoder().name == _devModel.GetModelChannel().encoder.name)
				.Single();
			
			_cmbPriority.SelectedItem = Constants.Instance.sVideoPriorityStreaming;

			var val = _devModel.GetResolutionByString(_devModel.GetModelChannel().resolution.ToString());
			_cmbResolution.SelectedItem = new ResolutionView(val);

			_numtbBitrate.Value = _devModel.GetModelChannel().bitrate;

			_numtbFPS.Value = _devModel.GetModelChannel().frameRate;
		}
    }
	public class EncoderView {
		public EncoderView(VideoEncoder enc) {
			_encoder = enc;
		}
		nvc.models.VideoEncoder _encoder;
		public nvc.models.VideoEncoder GetVideoEncoder() {
			return _encoder;
		}
		public void SetVideoEncoder(nvc.models.VideoEncoder value) {
			_encoder = value;
		}
		public override string ToString() {
			return _encoder.name;
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
			return Resolution.Resolution.width + Resolution.Resolution.height;
		}
		public ResolutionView(AvailableResolution resol) {
			Resolution = resol;
		}
		public AvailableResolution Resolution{get;set;}
		public override string ToString() {
			return Resolution.ResolutionString;
		}
	}
}
