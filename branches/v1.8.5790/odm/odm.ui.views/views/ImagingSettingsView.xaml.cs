using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls;
using odm.infra;
using odm.player;
using odm.ui.controls;
using odm.ui.core;
using onvif.services;
using utils;

namespace odm.ui.activities {
    /// <summary>
    /// Interaction logic for PTZView.xaml
    /// </summary>
    public partial class ImagingSettingsView : UserControl, IDisposable, IPlaybackController {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new ImagingSettingsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		private CompositeDisposable disposables = new CompositeDisposable();
        public SaveCancelStrings ButtonsStrings { get { return SaveCancelStrings.instance; } }
		public LocalImaging Strings { get { return LocalImaging.instance; } }
        public LinkButtonsStrings LinkStrings { get { return LinkButtonsStrings.instance; } }
		public Model model;

		void Localization() {
			captionAutoBalanceMode.CreateBinding(Label.ContentProperty, Strings, s => s.whitemode);
			captionBrightness.CreateBinding(Label.ContentProperty, Strings, s=>s.brightness);
			captionCb.CreateBinding(Label.ContentProperty, Strings, s => s.whiteBalanceCb);
			//captionCompensation.CreateBinding(Label.ContentProperty, Strings, s => s);
			//captionCompensationMode.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			captionContrast.CreateBinding(Label.ContentProperty, Strings, s => s.contrast);
			captionCr.CreateBinding(Label.ContentProperty, Strings, s => s.whiteBalanceCr);
			captionExposureGain.CreateBinding(Label.ContentProperty, Strings, s => s.gain);
			captionExposureIris.CreateBinding(Label.ContentProperty, Strings, s => s.iris);
			//captionExposureMode.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			//captionExposurePriority.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			//captionExposureTime.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			//captionSaturation.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			//captionSharpness.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
			//captionWideDynamicRange.CreateBinding(Label.ContentProperty, Strings, s => s.brightness);
		}
		enum ValueMode { 
			HIDDEN = 0,
			DEFAULT = 2,
			FIXED = 1,
			EDITABLE = 3
		}
		ValueMode GetMode(bool settSpecified, object options) {
			if (settSpecified)
				return GetMode(new object(), options);
			else
				return GetMode(null, options);
		}
		ValueMode GetMode(object settings, object options) {
			int sett = settings == null ? 0 : 1;
			int opt = options == null ? 0 : 1;
			opt = opt << 1;
			return (ValueMode)sett + opt;
		}
		//void Bind(ValueMode mode) {
		//    switch (mode) {
		//        case ValueMode.DEFAULT: //default value assigned
				//    break;
				//case ValueMode.EDITABLE: //value editable
				//    break;
				//case ValueMode.FIXED: //Read only mode
				//    break;
				//case ValueMode.HIDDEN: //Not supported 
		//            break;
		//    }
		//}
		void BindBrightness(ValueMode mode) {
			switch (mode) {
 				case ValueMode.DEFAULT: //default value assigned
					valueBrightness.Minimum = model.options.Brightness.Min;
					valueBrightness.Maximum = model.options.Brightness.Max;
					valueBrightness.CreateBinding(Slider.ValueProperty, model.settings, x => x.Brightness, (m, v) => {
						m.Brightness = v;
					});
					valueBrightness.Value = model.options.Brightness.Min;
					
					if (model.options.Brightness.Min == model.options.Brightness.Max) {
						valueBrightness.IsEnabled = false;
					}
					break;
				case ValueMode.EDITABLE: //value editable
					valueBrightness.Minimum = model.options.Brightness.Min;
					valueBrightness.Maximum = model.options.Brightness.Max;
					valueBrightness.CreateBinding(Slider.ValueProperty, model.settings, x => x.Brightness, (m, v) => {m.Brightness = v;});
					
					if (model.options.Brightness.Min == model.options.Brightness.Max) {
						valueBrightness.IsEnabled = false;
					}
					break;
				case ValueMode.FIXED: //Read only mode
					valueBrightness.Value = model.settings.Brightness;
					valueBrightness.IsEnabled = false;
					break;
				case ValueMode.HIDDEN: //Not supported 
					captionBrightness.Visibility = System.Windows.Visibility.Collapsed;
					valueBrightness.Visibility = System.Windows.Visibility.Collapsed;
					break;
			}
			
		}
		void BindSaturation(ValueMode mode) { 
			switch (mode) {
				case ValueMode.DEFAULT: //default value assigned
					valueSaturation.Minimum = model.options.ColorSaturation.Min;
					valueSaturation.Maximum = model.options.ColorSaturation.Max;
					valueSaturation.CreateBinding(Slider.ValueProperty, model.settings, x => x.ColorSaturation, (m, v) => { m.ColorSaturation = v; });
					valueSaturation.Value = model.options.ColorSaturation.Min;
					break;
				case ValueMode.EDITABLE: //value editable
					valueSaturation.Minimum = model.options.ColorSaturation.Min;
					valueSaturation.Maximum = model.options.ColorSaturation.Max;
					valueSaturation.CreateBinding(Slider.ValueProperty, model.settings, x => x.ColorSaturation, (m, v) => { m.ColorSaturation = v; });
					break;
				case ValueMode.FIXED: //Read only mode
					valueSaturation.Value = model.settings.ColorSaturation;
					valueSaturation.IsEnabled = false;
					break;
				case ValueMode.HIDDEN: //Not supported
					valueSaturation.Visibility = System.Windows.Visibility.Collapsed;
					captionSaturation.Visibility = System.Windows.Visibility.Collapsed;
					break;
			}
		}
		void BindContrast(ValueMode mode) {
			switch (mode) {
				case ValueMode.DEFAULT: //default value assigned
					valueContrast.Minimum = model.options.Contrast.Min;
					valueContrast.Maximum = model.options.Contrast.Max;
					valueContrast.CreateBinding(Slider.ValueProperty, model.settings, x => x.Contrast, (m, v) => { m.Contrast = v; });
					valueContrast.Value = model.options.Contrast.Min;
					break;
				case ValueMode.EDITABLE: //value editable
					valueContrast.Minimum = model.options.Contrast.Min;
					valueContrast.Maximum = model.options.Contrast.Max;
					valueContrast.CreateBinding(Slider.ValueProperty, model.settings, x => x.Contrast, (m, v) => { m.Contrast = v; });
					break;
				case ValueMode.FIXED: //Read only mode
					valueContrast.Value = model.settings.Contrast;
					valueContrast.IsEnabled = false;
					break;
				case ValueMode.HIDDEN: //Not supported 
					valueContrast.Visibility = System.Windows.Visibility.Collapsed;
					captionContrast.Visibility = System.Windows.Visibility.Collapsed;
					break;
			}
		}
		void BindSharpness(ValueMode mode) {
			switch (mode) {
				case ValueMode.DEFAULT: //default value assigned
					valueSharpness.Minimum = model.options.Sharpness.Min;
					valueSharpness.Maximum = model.options.Sharpness.Max;
					valueSharpness.CreateBinding(Slider.ValueProperty, model.settings, x => x.Sharpness, (m, v) => { m.Sharpness = v; });
					valueSharpness.Value = model.options.Sharpness.Min;
					break;
				case ValueMode.EDITABLE: //value editable
					valueSharpness.Minimum = model.options.Sharpness.Min;
					valueSharpness.Maximum = model.options.Sharpness.Max;
					valueSharpness.CreateBinding(Slider.ValueProperty, model.settings, x => x.Sharpness, (m, v) => { m.Sharpness = v; });
					break;
				case ValueMode.FIXED: //Read only mode
					valueSharpness.Value = model.settings.Sharpness;
					valueSharpness.IsEnabled = false;
					break;
				case ValueMode.HIDDEN: //Not supported 
					captionSharpness.Visibility = System.Windows.Visibility.Collapsed;
					valueSharpness.Visibility = System.Windows.Visibility.Collapsed;
					break;
			    }
		}
		void BindBacklight(ValueMode mode) { 
			switch (mode) {
				case ValueMode.DEFAULT: //default value assigned
					captionCompensation.Visibility = System.Windows.Visibility.Collapsed;
					valueCompensation.Visibility = System.Windows.Visibility.Collapsed;
					captionCompensationMode.Visibility = System.Windows.Visibility.Collapsed;
					comboBackCompensationMode.Visibility = System.Windows.Visibility.Collapsed;
					break;
				case ValueMode.EDITABLE: //value editable
					if (model.options.BacklightCompensation.Level != null) {
						valueCompensation.Minimum = model.options.BacklightCompensation.Level.Min;
						valueCompensation.Maximum = model.options.BacklightCompensation.Level.Max;

						if (model.options.BacklightCompensation.Level.Min == model.options.BacklightCompensation.Level.Max) {
							valueCompensation.Value = model.options.BacklightCompensation.Level.Min;
							valueCompensation.IsEnabled = false;
						} else {
							valueCompensation.CreateBinding(Slider.ValueProperty, model.settings.BacklightCompensation, x => x.Level, (m, v) => { m.Level = v; });
						}
					} else {
						valueCompensation.Value = model.settings.BacklightCompensation.Level;
						valueCompensation.IsEnabled = false;
					}

					if (model.options.BacklightCompensation.Mode != null) {
						comboBackCompensationMode.Items.Add(global::onvif.services.BacklightCompensationMode.ON);
						comboBackCompensationMode.Items.Add(global::onvif.services.BacklightCompensationMode.OFF);
						comboBackCompensationMode.SelectionChanged += new SelectionChangedEventHandler((o, evarg) => {
							if (comboBackCompensationMode.SelectedValue == null && model.settings.BacklightCompensation != null) {
								if (model.settings.BacklightCompensation.Mode == global::onvif.services.BacklightCompensationMode.OFF)
									valueCompensation.IsEnabled = false;
								else
									valueCompensation.IsEnabled = true;
								return;
							}
							if (model.settings.BacklightCompensation != null) {
								if ((global::onvif.services.BacklightCompensationMode)comboBackCompensationMode.SelectedValue == global::onvif.services.BacklightCompensationMode.ON) {
									valueCompensation.IsEnabled = true;
									model.settings.BacklightCompensation.Mode = global::onvif.services.BacklightCompensationMode.ON;
								} else if ((global::onvif.services.BacklightCompensationMode)comboBackCompensationMode.SelectedValue == global::onvif.services.BacklightCompensationMode.OFF) {
									valueCompensation.IsEnabled = false;
									model.settings.BacklightCompensation.Mode = global::onvif.services.BacklightCompensationMode.OFF;
								}
							}
						});
						comboBackCompensationMode.SelectedValue = model.settings.BacklightCompensation.Mode;

					} else {
						captionCompensationMode.Visibility = System.Windows.Visibility.Collapsed;
						comboBackCompensationMode.Visibility = System.Windows.Visibility.Collapsed;
					}
					break;
				case ValueMode.FIXED: //Read only mode
					captionCompensation.Visibility = System.Windows.Visibility.Collapsed;
					valueCompensation.Visibility = System.Windows.Visibility.Collapsed;
					captionCompensationMode.Visibility = System.Windows.Visibility.Collapsed;
					comboBackCompensationMode.Visibility = System.Windows.Visibility.Collapsed;
					break;
				case ValueMode.HIDDEN: //Not supported 
					captionCompensation.Visibility = System.Windows.Visibility.Collapsed;
					valueCompensation.Visibility = System.Windows.Visibility.Collapsed;
					captionCompensationMode.Visibility = System.Windows.Visibility.Collapsed;
					comboBackCompensationMode.Visibility = System.Windows.Visibility.Collapsed;
					break;
			}
		}
		void SetWhiteBalanceVisibility() {
			if ((global::onvif.services.WhiteBalanceMode)comboAutoBalance.SelectedValue == global::onvif.services.WhiteBalanceMode.AUTO) {
				valueCb.Visibility = System.Windows.Visibility.Collapsed;
				valueCr.Visibility = System.Windows.Visibility.Collapsed;
				captionCb.Visibility = System.Windows.Visibility.Collapsed;
				captionCr.Visibility = System.Windows.Visibility.Collapsed;
				model.settings.WhiteBalance.Mode = global::onvif.services.WhiteBalanceMode.AUTO;
			} else if ((global::onvif.services.WhiteBalanceMode)comboAutoBalance.SelectedValue == global::onvif.services.WhiteBalanceMode.MANUAL) {
				valueCb.Visibility = System.Windows.Visibility.Visible;
				valueCr.Visibility = System.Windows.Visibility.Visible;
				captionCb.Visibility = System.Windows.Visibility.Visible;
				captionCr.Visibility = System.Windows.Visibility.Visible;
				model.settings.WhiteBalance.Mode = global::onvif.services.WhiteBalanceMode.MANUAL;
			}
		}
		void BindAutoBalance(ValueMode mode) {
			switch (mode) {
				case ValueMode.DEFAULT: //default value assigned
					valueCb.Visibility = System.Windows.Visibility.Collapsed;
					valueCr.Visibility = System.Windows.Visibility.Collapsed;
					captionCb.Visibility = System.Windows.Visibility.Collapsed;
					captionCr.Visibility = System.Windows.Visibility.Collapsed;
					comboAutoBalance.Visibility = System.Windows.Visibility.Collapsed;
					captionAutoBalanceMode.Visibility = System.Windows.Visibility.Collapsed;
					break;
				case ValueMode.EDITABLE: //value editable
					if (model.options.WhiteBalance.Mode != null) {
						model.options.WhiteBalance.Mode.ForEach(md => {
							comboAutoBalance.Items.Add(md);
						});
						comboAutoBalance.SelectedItem = model.settings.WhiteBalance.Mode;
					} else {
						comboAutoBalance.Items.Add(model.settings.WhiteBalance.Mode);
						comboAutoBalance.IsEditable = false;
					}
					comboAutoBalance.SelectionChanged += new SelectionChangedEventHandler((o, evarg) => {
						if (comboAutoBalance.SelectedValue == null) {
							if (model.settings.WhiteBalance.Mode == global::onvif.services.WhiteBalanceMode.AUTO) {
								valueCb.Visibility = System.Windows.Visibility.Collapsed;
								valueCr.Visibility = System.Windows.Visibility.Collapsed;
								captionCb.Visibility = System.Windows.Visibility.Collapsed;
								captionCr.Visibility = System.Windows.Visibility.Collapsed;
							} else {
								valueCb.Visibility = System.Windows.Visibility.Visible;
								valueCr.Visibility = System.Windows.Visibility.Visible;
								captionCb.Visibility = System.Windows.Visibility.Visible;
								captionCr.Visibility = System.Windows.Visibility.Visible;
							}
							//SetWhiteBalanceVisibility();
							return;
						} else {
							SetWhiteBalanceVisibility();
						}
						
					});

					valueCb.Visibility = model.settings.WhiteBalance.CbGainSpecified ? Visibility.Visible : System.Windows.Visibility.Collapsed;
					captionCb.Visibility = model.settings.WhiteBalance.CbGainSpecified ? Visibility.Visible : System.Windows.Visibility.Collapsed;
					if (model.options.WhiteBalance != null && model.options.WhiteBalance.YbGain != null) {
						valueCb.Minimum = model.options.WhiteBalance.YbGain.Min;
						valueCb.Maximum = model.options.WhiteBalance.YbGain.Max;
					}
					valueCb.CreateBinding(Slider.ValueProperty, model.settings.WhiteBalance, x => x.CbGain, (m, v) => {
						m.CbGain = v;
					});
					valueCr.Visibility = model.settings.WhiteBalance.CrGainSpecified ? Visibility.Visible : System.Windows.Visibility.Collapsed;
					captionCr.Visibility = model.settings.WhiteBalance.CrGainSpecified ? Visibility.Visible : System.Windows.Visibility.Collapsed;
					if (model.options.WhiteBalance != null && model.options.WhiteBalance.YrGain != null) {
						valueCr.Minimum = model.options.WhiteBalance.YrGain.Min;
						valueCr.Maximum = model.options.WhiteBalance.YrGain.Max;
					}
					valueCr.CreateBinding(Slider.ValueProperty, model.settings.WhiteBalance, x => x.CrGain, (m, v) => {
						m.CrGain = v;
					});


					comboAutoBalance.SelectedItem = model.settings.WhiteBalance.Mode;
					SetWhiteBalanceVisibility();

					break;
				case ValueMode.FIXED: //Read only mode
					valueCb.Visibility = System.Windows.Visibility.Collapsed;
					valueCr.Visibility = System.Windows.Visibility.Collapsed;
					captionCb.Visibility = System.Windows.Visibility.Collapsed;
					captionCr.Visibility = System.Windows.Visibility.Collapsed;
					comboAutoBalance.Visibility = System.Windows.Visibility.Collapsed;
					captionAutoBalanceMode.Visibility = System.Windows.Visibility.Collapsed;
					break;
				case ValueMode.HIDDEN: //Not supported 
					valueCb.Visibility = System.Windows.Visibility.Collapsed;
					valueCr.Visibility = System.Windows.Visibility.Collapsed;
					captionCb.Visibility = System.Windows.Visibility.Collapsed;
					captionCr.Visibility = System.Windows.Visibility.Collapsed;
					comboAutoBalance.Visibility = System.Windows.Visibility.Collapsed;
					captionAutoBalanceMode.Visibility = System.Windows.Visibility.Collapsed;
					break;
			}
		}
		void BindExposure(ValueMode mode) {
			switch (mode) {
				case ValueMode.DEFAULT: //default value assigned
					break;
				case ValueMode.EDITABLE: //value editable
					//Priority
					valueExposurePriority.Items.Add(global::onvif.services.ExposurePriority.FrameRate);
					valueExposurePriority.Items.Add(global::onvif.services.ExposurePriority.LowNoise);
					valueExposurePriority.CreateBinding(ComboBox.SelectedValueProperty, model.settings.Exposure, x => {
						return x.Priority;
					}, (m, v) => {
						m.Priority = v;
					});

					//Mode
					//rowExpMode.CreateBinding(RowDefinition.HeightProperty, model.settings.Exposure, x => { return x.PrioritySpecified ? new GridLength(0, GridUnitType.Auto) : new GridLength(0); });
					valueExposureMode.Items.Add(global::onvif.services.ExposureMode.AUTO);
					valueExposureMode.Items.Add(global::onvif.services.ExposureMode.MANUAL);
					valueExposureMode.CreateBinding(ComboBox.SelectedValueProperty, model.settings.Exposure, x => {
						return x.Mode;
					}, (m, v) => {
						m.Mode = v;
					});

					//Time Gain Iris auto
					panelExposureTimeManual.CreateBinding(Grid.VisibilityProperty, model.settings.Exposure, x => x.Mode == ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed);

					//time visibility
					captionExposureTime.CreateBinding(Label.VisibilityProperty, model.settings.Exposure, 
						x => {
							if (model.settings.Exposure.ExposureTimeSpecified) {
								return x.Mode != ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed;
							} else {
								return Visibility.Collapsed;
							}
						}
					);
					valueExposureTime.CreateBinding(Slider.VisibilityProperty, model.settings.Exposure, 
						x => {
							if (model.settings.Exposure.ExposureTimeSpecified) {
								return x.Mode != ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed;
							} else {
								return Visibility.Collapsed;
							}
						}
					);
					//gain visibility
					captionExposureGain.CreateBinding(Label.VisibilityProperty, model.settings.Exposure,
						x => {
							if(model.settings.Exposure.GainSpecified){
								return x.Mode != ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed;
							} else {
								return Visibility.Collapsed;
							}
						}
					);
					valueExposureGain.CreateBinding(Slider.VisibilityProperty, model.settings.Exposure,
						x => {
							if(model.settings.Exposure.GainSpecified){
								return x.Mode != ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed;
							} else {
								return Visibility.Collapsed;
							}
						}
					);
					//iris visibility	
					captionExposureIris.CreateBinding(Label.VisibilityProperty, model.settings.Exposure,
						x => {
							if (model.settings.Exposure.IrisSpecified) {
								return x.Mode != ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed;
							} else {
								return Visibility.Collapsed;
							}
						}
					);
					valueExposureIris.CreateBinding(Slider.VisibilityProperty, model.settings.Exposure,
						x => {
							if (model.settings.Exposure.IrisSpecified) {
								return x.Mode != ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed;
							} else {
								return Visibility.Collapsed;
							}
						}
					);
					//Priority
					valueExposurePriority.CreateBinding(Slider.VisibilityProperty, model.settings.Exposure,
						x => {
							if (model.settings.Exposure.PrioritySpecified) {
								return x.Mode == ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed;
							} else {
								return Visibility.Collapsed;
							}
						}
					);
					captionExposurePriority.CreateBinding(Slider.VisibilityProperty, model.settings.Exposure,
						x => {
							if (model.settings.Exposure.PrioritySpecified) {
								return x.Mode == ExposureMode.AUTO ? Visibility.Visible : Visibility.Collapsed;
							} else {
								return Visibility.Collapsed;
							}
						}
					);

					//if (model.options.Exposure.ExposureTime != null) {
					//} else {
					//    captionExposureTime.Visibility = System.Windows.Visibility.Collapsed;
					//    valueExposureTime.Visibility = System.Windows.Visibility.Collapsed;
					//}
					//if (model.options.Exposure.Gain != null) {
					//} else {
					//    captionExposureGain.Visibility = System.Windows.Visibility.Collapsed;
					//    valueExposureGain.Visibility = System.Windows.Visibility.Collapsed;
					//}
					//if (model.options.Exposure.Iris != null) {
					//} else {
					//    captionExposureIris.Visibility = System.Windows.Visibility.Collapsed;
					//    valueExposureIris.Visibility = System.Windows.Visibility.Collapsed;
					//}
					if (model.options.Exposure.ExposureTime != null) {
						valueExposureTime.Minimum = model.options.Exposure.ExposureTime.Min;
						valueExposureTime.Maximum = model.options.Exposure.ExposureTime.Max;
					}
					if (model.options.Exposure.Gain != null) {
						valueExposureGain.Minimum = model.options.Exposure.Gain.Min;
						valueExposureGain.Maximum = model.options.Exposure.Gain.Max;
					}
					if (model.options.Exposure.Iris != null) {
						valueExposureIris.Minimum = model.options.Exposure.Iris.Min;
						valueExposureIris.Maximum = model.options.Exposure.Iris.Max;
					}


					valueExposureTime.CreateBinding(Slider.ValueProperty, model.settings.Exposure, x => x.ExposureTime, (m, v) => { m.ExposureTime = v; });
					valueExposureGain.CreateBinding(Slider.ValueProperty, model.settings.Exposure, x => x.Gain, (m, v) => { m.Gain = v; });
					valueExposureIris.CreateBinding(Slider.ValueProperty, model.settings.Exposure, x => x.Iris, (m, v) => { m.Iris = v; });

					if (model.options.Exposure != null && model.options.Exposure.MaxExposureTime != null) {
						valueMaxExposureTime.Minimum = model.options.Exposure.MaxExposureTime.Min;
						valueMaxExposureTime.Maximum = model.options.Exposure.MaxExposureTime.Max;

						valueMaxExposureGain.Minimum = model.options.Exposure.MaxGain.Min;
						valueMaxExposureGain.Maximum = model.options.Exposure.MaxGain.Max;

						valueMaxExposureIris.Minimum = model.options.Exposure.MaxIris.Min;
						valueMaxExposureIris.Maximum = model.options.Exposure.MaxIris.Max;
					}
					valueMaxExposureTime.CreateBinding(DoubleUpDown.ValueProperty, model.settings.Exposure, x => x.MaxExposureTime, (m, v) => { m.MaxExposureTime = v; });
					valueMaxExposureGain.CreateBinding(DoubleUpDown.ValueProperty, model.settings.Exposure, x => x.MaxGain, (m, v) => { m.MaxGain = v; });
					valueMaxExposureIris.CreateBinding(DoubleUpDown.ValueProperty, model.settings.Exposure, x => x.MaxIris, (m, v) => { m.MaxIris = v; });

					if (model.options.Exposure != null && model.options.Exposure.MinExposureTime != null) {
						valueMinExposureTime.Minimum = model.options.Exposure.MinExposureTime.Min;
						valueMinExposureTime.Maximum = model.options.Exposure.MinExposureTime.Max;

						valueMinExposureGain.Minimum = model.options.Exposure.MinGain.Min;
						valueMinExposureGain.Maximum = model.options.Exposure.MinGain.Max;

						valueMinExposureIris.Minimum = model.options.Exposure.MinIris.Min;
						valueMinExposureIris.Maximum = model.options.Exposure.MinIris.Max;
					}
					valueMinExposureTime.CreateBinding(DoubleUpDown.ValueProperty, model.settings.Exposure, x => x.MinExposureTime, (m, v) => { m.MinExposureTime = v; });
					valueMinExposureGain.CreateBinding(DoubleUpDown.ValueProperty, model.settings.Exposure, x => x.MinGain, (m, v) => { m.MinGain = v; });
					valueMinExposureIris.CreateBinding(DoubleUpDown.ValueProperty, model.settings.Exposure, x => x.MinIris, (m, v) => { m.MinIris = v; });

					break;
				case ValueMode.FIXED: //Read only mode
					captionExposureGain.Visibility = System.Windows.Visibility.Collapsed;
					captionExposureIris.Visibility = System.Windows.Visibility.Collapsed;
					captionExposureTime.Visibility = System.Windows.Visibility.Collapsed;
					captionExposurePriority.Visibility = System.Windows.Visibility.Collapsed;

					valueExposureTime.Visibility = System.Windows.Visibility.Collapsed;
					valueExposureGain.Visibility = System.Windows.Visibility.Collapsed;
					valueExposureIris.Visibility = System.Windows.Visibility.Collapsed;
					valueExposurePriority.Visibility = System.Windows.Visibility.Collapsed;

					captionExposureMode.Visibility = System.Windows.Visibility.Collapsed;
					valueExposureMode.Visibility = System.Windows.Visibility.Collapsed;

					panelExposureTimeManual.Visibility = System.Windows.Visibility.Collapsed;
					break;
				case ValueMode.HIDDEN: //Not supported 
					captionExposureGain.Visibility = System.Windows.Visibility.Collapsed;
					captionExposureIris.Visibility = System.Windows.Visibility.Collapsed;
					captionExposureTime.Visibility = System.Windows.Visibility.Collapsed;
					captionExposurePriority.Visibility = System.Windows.Visibility.Collapsed;

					valueExposureTime.Visibility = System.Windows.Visibility.Collapsed;
					valueExposureGain.Visibility = System.Windows.Visibility.Collapsed;
					valueExposureIris.Visibility = System.Windows.Visibility.Collapsed;
					valueExposurePriority.Visibility = System.Windows.Visibility.Collapsed;

					captionExposureMode.Visibility = System.Windows.Visibility.Collapsed;
					valueExposureMode.Visibility = System.Windows.Visibility.Collapsed;

					panelExposureTimeManual.Visibility = System.Windows.Visibility.Collapsed;
					break;
			}
		}
        void BindData(Model model) {
				try {
					BindBrightness(GetMode(model.settings.BrightnessSpecified, model.options.Brightness));
				} catch (Exception err) {dbg.Error(err);}
				try {
					BindSaturation(GetMode(model.settings.ColorSaturationSpecified, model.options.ColorSaturation));
				} catch (Exception err) { dbg.Error(err); }
				try {
					BindContrast(GetMode(model.settings.ContrastSpecified, model.options.Contrast));
				} catch (Exception err) { dbg.Error(err); }
				try {
					BindSharpness(GetMode(model.settings.SharpnessSpecified, model.options.Sharpness));
				} catch (Exception err) { dbg.Error(err); }
				try {
					BindBacklight(GetMode(model.settings.BacklightCompensation, model.options.BacklightCompensation));
				} catch (Exception err) {dbg.Error(err);}
				try {
					BindAutoBalance(GetMode(model.settings.WhiteBalance, model.options.WhiteBalance));
				} catch (Exception err) { dbg.Error(err); }
				try {
					BindExposure(GetMode(model.settings.Exposure, model.options.Exposure));
				} catch (Exception err) { dbg.Error(err); }
				

				////Focus
				//if (model.settings.Focus != null) {
				//    rowDefaultSpeed.CreateBinding(RowDefinition.HeightProperty, model.settings, x => { return x.Focus.DefaultSpeedSpecified ? new GridLength(0, GridUnitType.Auto) : new GridLength(0); });
				//    if (model.options.Focus != null && model.options.Focus.DefaultSpeed != null) {
				//        valueDefSpeed.Maximum = model.options.Focus.DefaultSpeed.Max;
				//        valueDefSpeed.Minimum = model.options.Focus.DefaultSpeed.Min;
				//    }
				//    valueDefSpeed.CreateBinding(DoubleUpDown.ValueProperty, model.settings, x => x.Focus.DefaultSpeed, (m, v) => { m.Focus.DefaultSpeed = v; });

				//    rowFarLimit.CreateBinding(RowDefinition.HeightProperty, model.settings, x => { return x.Focus.FarLimitSpecified ? new GridLength(0, GridUnitType.Auto) : new GridLength(0); });
				//    if (model.options.Focus != null && model.options.Focus.FarLimit != null) {
				//        valueFarLimit.Maximum = model.options.Focus.FarLimit.Max;
				//        valueFarLimit.Minimum = model.options.Focus.FarLimit.Min;
				//    }
				//    valueFarLimit.CreateBinding(DoubleUpDown.ValueProperty, model.settings, x => x.Focus.FarLimit, (m, v) => { m.Focus.FarLimit = v; });

				//    rowNearLimit.CreateBinding(RowDefinition.HeightProperty, model.settings, x => { return x.Focus.NearLimitSpecified ? new GridLength(0, GridUnitType.Auto) : new GridLength(0); });
				//    if (model.options.Focus != null && model.options.Focus.NearLimit != null) {
				//        valueNearLimit.Maximum = model.options.Focus.NearLimit.Max;
				//        valueNearLimit.Minimum = model.options.Focus.NearLimit.Min;
				//    }
				//    valueNearLimit.CreateBinding(DoubleUpDown.ValueProperty, model.settings, x => x.Focus.NearLimit, (m, v) => { m.Focus.NearLimit = v; });
				//} else {
				//    groupFocus.Visibility = System.Windows.Visibility.Collapsed;
				//}
                
				////IrCutFilter
				//if (model.settings.IrCutFilterSpecified) {
				//    valueIrCutFilter.Items.Add(global::onvif.services.IrCutFilterMode.AUTO);
				//    valueIrCutFilter.Items.Add(global::onvif.services.IrCutFilterMode.OFF);
				//    valueIrCutFilter.Items.Add(global::onvif.services.IrCutFilterMode.ON);
				//    valueIrCutFilter.CreateBinding(ComboBox.SelectedValueProperty, model.settings, x => x.IrCutFilter, (m, v) => {
				//        m.IrCutFilter = v;
				//    });
				//    groupIr.Visibility = model.settings.IrCutFilterSpecified ? Visibility.Visible : System.Windows.Visibility.Collapsed;
				//} else {
				//    groupIr.Visibility = System.Windows.Visibility.Collapsed;
				//}

				////Wide
				//if (model.settings.WideDynamicRange != null) {
				//    captionWideDynamicRange.Visibility = model.settings.WideDynamicRange.LevelSpecified ? Visibility.Visible : System.Windows.Visibility.Collapsed;
				//    valueWideLevel.Visibility = model.settings.WideDynamicRange.LevelSpecified ? Visibility.Visible : System.Windows.Visibility.Collapsed;
				//    if (model.options.WideDynamicRange != null && model.options.WideDynamicRange.Level != null) {
				//        valueWideLevel.Maximum = model.options.WideDynamicRange.Level.Max;
				//        valueWideLevel.Minimum = model.options.WideDynamicRange.Level.Min;
				//    }
				//    valueWideLevel.CreateBinding(DoubleUpDown.ValueProperty, model.settings, x => x.WideDynamicRange.Level, (m, v) => { m.WideDynamicRange.Level = v; });

				//    valueWideMode.Items.Add(global::onvif.services.WideDynamicMode.OFF);
				//    valueWideMode.Items.Add(global::onvif.services.WideDynamicMode.ON);
				//    valueWideMode.CreateBinding(ComboBox.SelectedValueProperty, model.settings, x => x.WideDynamicRange.Mode, (m, v) => { m.WideDynamicRange.Mode = v; });
				//} else {
				//    groupWideDynamicRange.Visibility = System.Windows.Visibility.Collapsed;
				//}
				
                //model.settings.WhiteBalance.CbGain;
                //model.settings.WhiteBalance.CrGain;
                //model.settings.WhiteBalance.Mode;

				//isAutoBalance.CreateBinding(CheckBox.IsCheckedProperty, model.settings, x => {
				//    return x.WhiteBalance.Mode == global::onvif.services.WhiteBalanceMode.AUTO;
				//}, (m, v) => {
				//    if (v)
				//        m.WhiteBalance.Mode = global::onvif.services.WhiteBalanceMode.AUTO;
				//    else
				//        m.WhiteBalance.Mode = global::onvif.services.WhiteBalanceMode.MANUAL;
				//});

            //valueCb;
            //valueCr;
        }
		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.model = model;
			this.DataContext = model;

            var applyCmd = new DelegateCommand(
				() => Success(new Result.Apply(model)),
                () => {
                    //if (context.model != null) {
                    //    return context.model.isModified;
                    //}
                    return true;
                }
            );
            ApplyCommand = applyCmd;

			var revertCmd = new DelegateCommand(
				() => {
					if (model != null) {
						model.RevertChanges();
					}
				},
				() => {
					//if (context.model != null) {
					//    return !context.model.isModified;
					//}
					return true;
				}
			);

			InitializeComponent();

            container = activityContext.container;
            vidInfo = container.Resolve<IVideoInfo>();

            BindData(model);
			Localization();

            var vInfo = container.Resolve<IVideoInfo>();

			VideoStartup(model.profToken);
		}
        IVideoInfo vidInfo;
        IUnityContainer container;

		IPlaybackSession playbackSession;
		void VideoStartup(string token) {
			var playerAct = activityContext.container.Resolve<IVideoPlayerActivity>();

			var playerModel = new VideoPlayerActivityModel(
				profileToken: model.profToken,
				showStreamUrl: true,
				streamSetup: new StreamSetup() {
					Stream = StreamType.RTPUnicast,
					Transport = new Transport() {
						Protocol = AppDefaults.visualSettings.Transport_Type,
						Tunnel = null
					}
				}
			);

			disposables.Add(
				activityContext.container.RunChildActivity(player, playerModel, (c, m) => playerAct.Run(c, m))
			);
		}

		public void Dispose() {
			Cancel();
		}

        public IVideoInfo VideoInfo {
            get { return (IVideoInfo)GetValue(VideoInfoProperty); }
            set { SetValue(VideoInfoProperty, value); }
        }
        public static readonly DependencyProperty VideoInfoProperty = DependencyProperty.Register("VideoInfo", typeof(IVideoInfo), typeof(ImagingSettingsView));


		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Shutdown() {
		}
	}
}
