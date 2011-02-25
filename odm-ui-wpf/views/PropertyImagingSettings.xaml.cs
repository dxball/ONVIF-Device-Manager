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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.MemoryMappedFiles;

using odm.models;
using odm.utils;
using odm.utils.extensions;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyImagingSettings.xaml
	/// </summary>
	public partial class PropertyImagingSettings : BasePropertyControl {
		public PropertyImagingSettings(ImagingSettingsModel devModel) {
			InitializeComponent();
			_devModel = devModel;
			InitControls();
			Localization();
			BindData();
		}
		
		PropertySensorSettingsStrings strings = new PropertySensorSettingsStrings();
		LinkButtonsStrings titles = new LinkButtonsStrings();
		ImagingSettingsModel _devModel;
		public Action Save { get; set; }
		public Action Cancel { get; set; }
		public override void ReleaseAll() {
			_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
		public void InitPlayBack(MemoryMappedFile mem) {
			_videoPlayer.memFile = mem;
			Rect ret = new Rect(0, 0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);

		}
		void InitControls() {
			saveCancelControl.Save.Click += new RoutedEventHandler(Save_Click);
			saveCancelControl.Cancel.Click += new RoutedEventHandler(Cancel_Click);
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			if (Cancel != null)
				Cancel();
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			if (Save != null)
				Save();
		}
		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.sensorSettings);
			brightnessCaption.CreateBinding(Label.ContentProperty, strings, x=>x.brightness);
			cbCaption.CreateBinding(Label.ContentProperty, strings, x => x.whiteBalanceCb);
			contrastCaption.CreateBinding(Label.ContentProperty, strings, x => x.contrast);
			crCaption.CreateBinding(Label.ContentProperty, strings, x => x.whiteBalanceCr);
			saturationCaption.CreateBinding(Label.ContentProperty, strings, x => x.saturation);
			sharpnessCaption.CreateBinding(Label.ContentProperty, strings, x => x.sharpness);
			autoWhiteCaption.CreateBinding(Label.ContentProperty, strings, x => x.whitemode);
		}
		void BindData() {
			try {
				slBrightness.Minimum = _devModel.brightnessMin;
				slBrightness.Maximum = _devModel.brightnessMax;
				slBrightness.CreateBinding(Slider.ValueProperty, _devModel, x=>x.brightness, (m,v)=>{
					m.brightness = (float)v;
				});
			}catch(Exception err){
				dbg.Info(err.Message);
			}
			try{
				slContrast.Minimum = _devModel.contrastMin;
				slContrast.Maximum = _devModel.contrastMax;
				slContrast.CreateBinding(Slider.ValueProperty, _devModel, x => x.contrast, (m, v) => {
					m.contrast = (float)v;
				});
			} catch (Exception err) {
				dbg.Info(err.Message);
				slContrast.IsEnabled = false;
			}
			try{
				slSaturation.Minimum = _devModel.colorSaturationMin;
				slSaturation.Maximum = _devModel.colorSaturationMax;
				slSaturation.CreateBinding(Slider.ValueProperty, _devModel, x => x.colorSaturation, (m, v) => {
					m.colorSaturation = (float)v;
				});
			} catch (Exception err) {
				dbg.Info(err.Message);
			}
			try{
				slSharpness.Minimum = _devModel.sharpnessMin;
				slSharpness.Maximum = _devModel.sharpnessMax;
				slSharpness.CreateBinding(Slider.ValueProperty, _devModel, x => x.sharpness, (m, v) => {
					m.sharpness = (float)v;
				});
			} catch (Exception err) {
				dbg.Info(err.Message);
			}

				if (_devModel.whiteBalance.Mode == global::onvif.services.imaging.WhiteBalanceMode.AUTO) {
					chbAuto.IsChecked = true;
					SetCBCROff();
				} else {
					chbAuto.IsChecked = false;
					SetCBCROn();
				}
				chbAuto.Click += new RoutedEventHandler(chbAuto_Checked);

				saveCancelControl.Cancel.CreateBinding(Button.IsEnabledProperty, _devModel, x => x.isModified);
				saveCancelControl.Save.CreateBinding(Button.IsEnabledProperty, _devModel, x => x.isModified);
		}

		void chbAuto_Checked(object sender, RoutedEventArgs e) {
			if (chbAuto.IsChecked.Value) {
				_devModel.whiteBalance.Mode = global::onvif.services.imaging.WhiteBalanceMode.AUTO;
				saveCancelControl.Save.IsEnabled = true;
				SetCBCROff();
			} else {
				_devModel.whiteBalance.Mode = global::onvif.services.imaging.WhiteBalanceMode.MANUAL;
				saveCancelControl.Save.IsEnabled = true;
				SetCBCROn();
			}
		}
		void SetCBCROff() {
			slCb.IsEnabled = false;
			slCr.IsEnabled = false;
		}
		void SetCBCROn(){
			slCb.IsEnabled = true;
			slCr.IsEnabled = true;


			try {
				if (!float.IsNaN(_devModel.whiteBalanceOptions.YbGain.Max) && !float.IsNaN(_devModel.whiteBalanceOptions.YbGain.Min)) {
					slCb.Maximum = _devModel.whiteBalanceOptions.YbGain.Max;
					slCb.Minimum = _devModel.whiteBalanceOptions.YbGain.Min;
					slCb.CreateBinding(Slider.ValueProperty, _devModel.whiteBalance, x => x.CbGain, (m, v) => {
						m.CbGain = (float)v;
					});
				} else {
					slCb.IsEnabled = false;
				}
			} catch (Exception err) {
				//BindingError(err, "White balance Cb gain value error. val:" + _devModel.whiteBalance.CbGain);
			}
			try {
				if (!float.IsNaN(_devModel.whiteBalanceOptions.YrGain.Max) && !float.IsNaN(_devModel.whiteBalanceOptions.YrGain.Min)) {
					slCr.Maximum = _devModel.whiteBalanceOptions.YrGain.Max;
					slCr.Minimum = _devModel.whiteBalanceOptions.YrGain.Min;
					slCb.CreateBinding(Slider.ValueProperty, _devModel.whiteBalance, x => x.CrGain, (m, v) => {
						m.CrGain = (float)v;
					});
				} else {
					slCr.IsEnabled = false;
				}
			} catch (Exception err) {
				//BindingError(err, "White balance Cr gain value error. val:" + _devModel.whiteBalance.CrGain);
			}
		}
	}
}
