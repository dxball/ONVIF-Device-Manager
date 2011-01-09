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

namespace odm.controls {
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
		public MemoryMappedFile memFile {
			set {
				_videoPlayer.memFile = value;
			}
		}
		PropertySensorSettingsStrings strings = new PropertySensorSettingsStrings();
		ImagingSettingsModel _devModel;
		public Action Save { get; set; }
		public Action Cancel { get; set; }
		public override void ReleaseAll() {
			_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}

		void InitControls() {
			Rect ret = new Rect(0, 0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);

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
			title.CreateBinding(Title.ContentProperty, strings, x => x.title);
			slBrightness.lable.CreateBinding(Label.ContentProperty, strings, x=>x.brightness);
			slCb.lable.CreateBinding(Label.ContentProperty, strings, x => x.whiteBalanceCb);
			slContrast.lable.CreateBinding(Label.ContentProperty, strings, x => x.contrast);
			slCr.lable.CreateBinding(Label.ContentProperty, strings, x => x.whiteBalanceCr);
			slSaturation.lable.CreateBinding(Label.ContentProperty, strings, x => x.saturation);
			slSharpness.lable.CreateBinding(Label.ContentProperty, strings, x => x.sharpness);
			tbAutoWhite.CreateBinding(Label.ContentProperty, strings, x => x.whitemode);
		}
		void BindData() {
			try {
				slBrightness.slider.Minimum = _devModel.brightnessMin;
				slBrightness.slider.Maximum = _devModel.brightnessMax;
				slBrightness.slider.CreateBinding(Slider.ValueProperty, _devModel, x=>x.brightness, (m,v)=>{
					m.brightness = (float)v;
				});
			}catch(Exception err){
				dbg.Info(err.Message);
			}
			try{
				slContrast.slider.Minimum = _devModel.contrastMin;
				slContrast.slider.Maximum = _devModel.contrastMax;
				slContrast.slider.CreateBinding(Slider.ValueProperty, _devModel, x => x.contrast, (m, v) => {
					m.contrast = (float)v;
				});
			} catch (Exception err) {
				dbg.Info(err.Message);
				slContrast.IsEnabled = false;
			}
			try{
				slSaturation.slider.Minimum = _devModel.colorSaturationMin;
				slSaturation.slider.Maximum = _devModel.colorSaturationMax;
				slSaturation.slider.CreateBinding(Slider.ValueProperty, _devModel, x => x.colorSaturation, (m, v) => {
					m.colorSaturation = (float)v;
				});
			} catch (Exception err) {
				dbg.Info(err.Message);
			}
			try{
				slSharpness.slider.Minimum = _devModel.sharpnessMin;
				slSharpness.slider.Maximum = _devModel.sharpnessMax;
				slSharpness.slider.CreateBinding(Slider.ValueProperty, _devModel, x => x.sharpness, (m, v) => {
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
					slCb.slider.Maximum = _devModel.whiteBalanceOptions.YbGain.Max;
					slCb.slider.Minimum = _devModel.whiteBalanceOptions.YbGain.Min;
					slCb.slider.CreateBinding(Slider.ValueProperty, _devModel.whiteBalance, x => x.CbGain, (m, v) => {
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
					slCr.slider.Maximum = _devModel.whiteBalanceOptions.YrGain.Max;
					slCr.slider.Minimum = _devModel.whiteBalanceOptions.YrGain.Min;
					slCb.slider.CreateBinding(Slider.ValueProperty, _devModel.whiteBalance, x => x.CrGain, (m, v) => {
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
