using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.ui.viewModels;
using odm.ui.core;
using odm.player;
using utils;
using odm.ui.controls;
using Microsoft.Practices.Prism.Commands;
using onvif.services;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using odm.core;
using odm.infra;
using System.Windows.Controls.Primitives;
using System.Timers;
using System.Linq;
using odm.onvif;
using System.Globalization;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for PTZView.xaml
	/// </summary>
	public partial class PtzView : UserControl, IDisposable, IPlaybackController, INotifyPropertyChanged {
		#region Activity definition

		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new PtzView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}

		#endregion

		Model model;
		//ObservableCollection<PTZNode> Nodes;
		public ObservableCollection<PTZPreset> Presets { get; private set; }

		public readonly Dispatcher dispatch = Dispatcher.CurrentDispatcher;
		private CompositeDisposable disposables = new CompositeDisposable();
		public CompositeDisposable subscription = new CompositeDisposable();
		public INvtSession CurrentSession;
		public Account CurrentAccount;
		public String ChannelToken;
		public string profileToken;

		enum PTZMoveModes {
			Absolute,
			Relative,
			Continuous
		}
		PTZMoveModes moveMode = (PTZMoveModes)(-1);
		void SetMoveMode(PTZMoveModes mode, bool set) {
			if (this.moveMode == mode && set)
				return;

			if (set) {
				this.moveMode = mode;

				if (mode == PTZMoveModes.Absolute) {
					absPanTiltControls.Visibility = Visibility.Visible;
					absZoomControls.Visibility = Visibility.Visible;
					absoluteMovePanel.Visibility = Visibility.Visible;

					relPanTiltControls.Visibility = Visibility.Collapsed;
					relZoomControls.Visibility = Visibility.Collapsed;
					relativeMovePanel.Visibility = Visibility.Collapsed;

					contPanTiltControls.Visibility = Visibility.Collapsed;
					contZoomControls.Visibility = Visibility.Collapsed;
					continuousMovePanel.Visibility = Visibility.Collapsed;

					tglbtnAbsoluteMove.IsChecked = true;
				} else if (mode == PTZMoveModes.Relative) {
					absPanTiltControls.Visibility = Visibility.Collapsed;
					absZoomControls.Visibility = Visibility.Collapsed;
					absoluteMovePanel.Visibility = Visibility.Collapsed;

					relPanTiltControls.Visibility = Visibility.Visible;
					relZoomControls.Visibility = Visibility.Visible;
					relativeMovePanel.Visibility = Visibility.Visible;

					contPanTiltControls.Visibility = Visibility.Collapsed;
					contZoomControls.Visibility = Visibility.Collapsed;
					continuousMovePanel.Visibility = Visibility.Collapsed;

					tglbtnRelativeMove.IsChecked = true;
				} else if (mode == PTZMoveModes.Continuous) {
					absPanTiltControls.Visibility = Visibility.Collapsed;
					absZoomControls.Visibility = Visibility.Collapsed;
					absoluteMovePanel.Visibility = Visibility.Collapsed;

					relPanTiltControls.Visibility = Visibility.Collapsed;
					relZoomControls.Visibility = Visibility.Collapsed;
					relativeMovePanel.Visibility = Visibility.Collapsed;

					contPanTiltControls.Visibility = Visibility.Visible;
					contZoomControls.Visibility = Visibility.Visible;
					continuousMovePanel.Visibility = Visibility.Visible;

					tglbtnContinuousMove.IsChecked = true;
				}
			}
		}

		private void Init(Model model) {
			this.model = model;

			OnCompleted += () => {
				//TODO: release player host
				disposables.Dispose();
				subscription.Dispose();
			};
			this.DataContext = this;
			InitializeComponent();

			Presets = new ObservableCollection<PTZPreset>();

			var ptzInfo = activityContext.container.Resolve<IPtzInfo>();
			CurrentSession = activityContext.container.Resolve<INvtSession>();
			profileToken = ptzInfo.ProfileToken;

			VideoStartup();

			InitData();
			InitCapabilities();
			BindData();
			Localize();


			if (defContPanTiltVelSpace != null || defContZoomVelSpace != null)
				SetMoveMode(PTZMoveModes.Continuous, true);
			else if (defRelPanTiltTranslSpace != null || defRelZoomTranslSpace != null)
				SetMoveMode(PTZMoveModes.Relative, true);
			else if (defAbsPanTiltPosSpace != null || defAbsZoomPosSpace != null)
				SetMoveMode(PTZMoveModes.Absolute, true);
		}

		void InitCapabilities() {
			if (defAbsPanTiltPosSpace == null && defAbsZoomPosSpace == null)
				tglbtnAbsoluteMove.Visibility = Visibility.Collapsed;
			if (defRelPanTiltTranslSpace == null && defRelZoomTranslSpace == null)
				tglbtnRelativeMove.Visibility = Visibility.Collapsed;
			if (defContPanTiltVelSpace == null && defContZoomVelSpace == null)
				tglbtnContinuousMove.Visibility = Visibility.Collapsed;

			if (defAbsPanTiltPosSpace == null || defPanTiltSpeedSpace == null) {
				absPanTiltControls.IsEnabled = false;
				sliderAbsPanValue.IsEnabled = false;
				sliderAbsTiltValue.IsEnabled = false;
				sliderAbsPanTiltSpeed.IsEnabled = false;
			}
			if (defAbsZoomPosSpace == null || defZoomSpeedSpace == null) {
				absZoomControls.IsEnabled = false;
				sliderAbsZoomValue.IsEnabled = false;
				sliderAbsZoomSpeed.IsEnabled = false;
			}
			if (defRelPanTiltTranslSpace == null || defPanTiltSpeedSpace == null) {
				relPanTiltControls.IsEnabled = false;
				sliderRelPanValue.IsEnabled = false;
				sliderRelTiltValue.IsEnabled = false;
				sliderRelPanTiltSpeed.IsEnabled = false;
			}
			if (defRelZoomTranslSpace == null || defZoomSpeedSpace == null) {
				relZoomControls.IsEnabled = false;
				sliderRelZoomValue.IsEnabled = false;
				sliderRelZoomSpeed.IsEnabled = false;
			}
			if (defContPanTiltVelSpace == null) {
				contPanTiltControls.IsEnabled = false;
				sliderContPanVelocity.IsEnabled = false;
				sliderContTiltVelocity.IsEnabled = false;
			}
			if (defContZoomVelSpace == null) {
				contZoomControls.IsEnabled = false;
				sliderContZoomVelocity.IsEnabled = false;
			}
		}

		public LocalPTZ Strings { get { return LocalPTZ.instance; } }
		void Localize() {
			//TODO localize!!!
		}

		#region Spaces

		Space2DDescription defAbsPanTiltPosSpace;
		Space1DDescription defAbsZoomPosSpace;
		Space2DDescription defRelPanTiltTranslSpace;
		Space1DDescription defRelZoomTranslSpace;
		Space2DDescription defContPanTiltVelSpace;
		Space1DDescription defContZoomVelSpace;
		Space1DDescription defPanTiltSpeedSpace;
		Space1DDescription defZoomSpeedSpace;

		void InitDefaultPTZSpaces() {
			if (model.currentNode != null && model.currentNode.supportedPTZSpaces != null) {
				var spaces = model.currentNode.supportedPTZSpaces;

				defAbsPanTiltPosSpace = spaces.absolutePanTiltPositionSpace == null ? null : spaces.absolutePanTiltPositionSpace.FirstOrDefault(s => @"http://www.onvif.org/ver10/tptz/PanTiltSpaces/PositionGenericSpace".Equals(s.uri));
				defAbsZoomPosSpace = spaces.absoluteZoomPositionSpace == null ? null : spaces.absoluteZoomPositionSpace.FirstOrDefault(s => @"http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace".Equals(s.uri));
				defRelPanTiltTranslSpace = spaces.relativePanTiltTranslationSpace == null ? null : spaces.relativePanTiltTranslationSpace.FirstOrDefault(s => @"http://www.onvif.org/ver10/tptz/PanTiltSpaces/TranslationGenericSpace".Equals(s.uri));
				defRelZoomTranslSpace = spaces.relativeZoomTranslationSpace == null ? null : spaces.relativeZoomTranslationSpace.FirstOrDefault(s => @"http://www.onvif.org/ver10/tptz/ZoomSpaces/TranslationGenericSpace".Equals(s.uri));
				defContPanTiltVelSpace = spaces.continuousPanTiltVelocitySpace == null ? null : spaces.continuousPanTiltVelocitySpace.FirstOrDefault(s => @"http://www.onvif.org/ver10/tptz/PanTiltSpaces/VelocityGenericSpace".Equals(s.uri));
				defContZoomVelSpace = spaces.continuousZoomVelocitySpace == null ? null : spaces.continuousZoomVelocitySpace.FirstOrDefault(s => @"http://www.onvif.org/ver10/tptz/ZoomSpaces/VelocityGenericSpace".Equals(s.uri));

				defPanTiltSpeedSpace = spaces.panTiltSpeedSpace == null ? null : spaces.panTiltSpeedSpace.FirstOrDefault(s => @"http://www.onvif.org/ver10/tptz/PanTiltSpaces/GenericSpeedSpace".Equals(s.uri));
				defZoomSpeedSpace = spaces.zoomSpeedSpace == null ? null : spaces.zoomSpeedSpace.FirstOrDefault(s => @"http://www.onvif.org/ver10/tptz/ZoomSpaces/ZoomGenericSpeedSpace".Equals(s.uri));
			}
		}

		#endregion Spaces

		void InitData() {
			InitDefaultPTZSpaces();

			//Nodes.Clear();
			Presets.Clear();
			//model.nodes.ForEach(x => { Nodes.Add(x); });

			//this.CreateBinding(SelectedNodeProperty, model, x => {
			//    return x.currentNode;
			//}, (m, v) => {
			//    m.currentNode = v;
			//});
			model.presets.ForEach(x => { Presets.Add(x); });
		}

		void BindData() {
			//CommonData
			valuePresetName.CreateBinding(TextBox.TextProperty, this, x => x.PresetName, (m, v) => { m.PresetName = v; });
			valuePresetsList.ItemsSource = Presets;
			valuePresetsList.CreateBinding(ListBox.SelectedItemProperty, this, x => x.SelectedPreset, (m, v) => m.SelectedPreset = v);
			//ReloadPresets();

			captionErrorMessage.CreateBinding(TextBlock.TextProperty, this, x => x.ErrorMessage);

			if (defAbsPanTiltPosSpace != null && defPanTiltSpeedSpace != null) {
				sliderAbsPanValue.Minimum = defAbsPanTiltPosSpace.xRange.min;
				sliderAbsPanValue.Maximum = defAbsPanTiltPosSpace.xRange.max;
				sliderAbsPanValue.Value = (defAbsPanTiltPosSpace.xRange.min + defAbsPanTiltPosSpace.xRange.max) / 2.0;

				sliderAbsTiltValue.Minimum = defAbsPanTiltPosSpace.xRange.min;
				sliderAbsTiltValue.Maximum = defAbsPanTiltPosSpace.xRange.max;
				sliderAbsTiltValue.Value = (defAbsPanTiltPosSpace.xRange.min + defAbsPanTiltPosSpace.xRange.max) / 2.0;

				sliderAbsPanTiltSpeed.Minimum = defPanTiltSpeedSpace.xRange.min;
				sliderAbsPanTiltSpeed.Maximum = defPanTiltSpeedSpace.xRange.max;
				sliderAbsPanTiltSpeed.Value = defPanTiltSpeedSpace.xRange.max;
			}
			if (defAbsZoomPosSpace != null && defZoomSpeedSpace != null) {
				sliderAbsZoomValue.Minimum = defAbsZoomPosSpace.xRange.min;
				sliderAbsZoomValue.Maximum = defAbsZoomPosSpace.xRange.max;
				sliderAbsZoomValue.Value = defAbsZoomPosSpace.xRange.max;

				sliderAbsZoomSpeed.Minimum = defZoomSpeedSpace.xRange.min;
				sliderAbsZoomSpeed.Maximum = defZoomSpeedSpace.xRange.max;
				sliderAbsZoomSpeed.Value = defZoomSpeedSpace.xRange.max;
			}
			if (defRelPanTiltTranslSpace != null && defPanTiltSpeedSpace != null) {
				sliderRelPanValue.Minimum = 0;// defRelPanTiltTranslSpace.XRange.Min;
				sliderRelPanValue.Maximum = defRelPanTiltTranslSpace.xRange.max;
				sliderRelPanValue.Value = defRelPanTiltTranslSpace.xRange.max / 10.0;

				sliderRelTiltValue.Minimum = 0;// defRelPanTiltTranslSpace.XRange.Min;
				sliderRelTiltValue.Maximum = defRelPanTiltTranslSpace.xRange.max;
				sliderRelTiltValue.Value = defRelPanTiltTranslSpace.xRange.max / 10.0;

				sliderRelPanTiltSpeed.Minimum = defPanTiltSpeedSpace.xRange.min;
				sliderRelPanTiltSpeed.Maximum = defPanTiltSpeedSpace.xRange.max;
				sliderRelPanTiltSpeed.Value = defPanTiltSpeedSpace.xRange.max;
			}
			if (defRelZoomTranslSpace != null && defZoomSpeedSpace != null) {
				sliderRelZoomValue.Minimum = 0;// defRelZoomTranslSpace.XRange.Min;
				sliderRelZoomValue.Maximum = defRelZoomTranslSpace.xRange.max;
				sliderRelZoomValue.Value = defRelZoomTranslSpace.xRange.max / 10.0;

				sliderRelZoomSpeed.Minimum = defZoomSpeedSpace.xRange.min;
				sliderRelZoomSpeed.Maximum = defZoomSpeedSpace.xRange.max;
				sliderRelZoomSpeed.Value = defZoomSpeedSpace.xRange.max;
			}
			if (defContPanTiltVelSpace != null) {
				sliderContPanVelocity.Minimum = 0; // defContPanTiltVelSpace.XRange.Min;
				sliderContPanVelocity.Maximum = defContPanTiltVelSpace.xRange.max;
				sliderContPanVelocity.Value = defContPanTiltVelSpace.xRange.max / 2.0;

				sliderContTiltVelocity.Minimum = 0; // defContPanTiltVelSpace.XRange.Min;
				sliderContTiltVelocity.Maximum = defContPanTiltVelSpace.xRange.max;
				sliderContTiltVelocity.Value = defContPanTiltVelSpace.xRange.max / 2.0;
			}
			if (defContZoomVelSpace != null) {
				sliderContZoomVelocity.Minimum = 0; // defContZoomVelSpace.XRange.Min;
				sliderContZoomVelocity.Maximum = defContZoomVelSpace.xRange.max;
				sliderContZoomVelocity.Value = defContZoomVelSpace.xRange.max / 2.0;
			}


			//Buttons
			tglbtnAbsoluteMove.Checked += (s, e) => this.SetMoveMode(PTZMoveModes.Absolute, tglbtnAbsoluteMove.IsChecked == true);
			tglbtnRelativeMove.Checked += (s, e) => this.SetMoveMode(PTZMoveModes.Relative, tglbtnRelativeMove.IsChecked == true);
			tglbtnContinuousMove.Checked += (s, e) => this.SetMoveMode(PTZMoveModes.Continuous, tglbtnContinuousMove.IsChecked == true);


			btnContUp.PreviewMouseDown += ContinuousUp_MouseDown;
			btnContUp.PreviewMouseUp += ContinuousUp_MouseUp;
			btnContDown.PreviewMouseDown += ContinuousDown_MouseDown;
			btnContDown.PreviewMouseUp += ContinuousDown_MouseUp;
			btnContLeft.PreviewMouseDown += ContinuousLeft_MouseDown;
			btnContLeft.PreviewMouseUp += ContinuousLeft_MouseUp;
			btnContRight.PreviewMouseDown += ContinuousRight_MouseDown;
			btnContRight.PreviewMouseUp += ContinuousRight_MouseUp;
			btnContZoomMinus.PreviewMouseDown += ContinuousZoomMinus_MouseDown;
			btnContZoomMinus.PreviewMouseUp += ContinuousZoomMinus_MouseUp;
			btnContZoomPlus.PreviewMouseDown += ContinuousZoomPlus_MouseDown;
			btnContZoomPlus.PreviewMouseUp += ContinuousZoomPlus_MouseUp;


			btnRelUp.Click += RelUp_Click;
			btnRelRight.Click += RelRight_Click;
			btnRelDown.Click += RelDown_Click;
			btnRelLeft.Click += RelLeft_Click;
			btnRelZoomMinus.Click += RelZoomMinus_Click;
			btnRelZoomPlus.Click += RelZoomPlus_Click;

			btnAbsMove.Click += AbsoluteMove_Click;

			btnApplySettings.Click += new RoutedEventHandler(ApplySettings);
			btnDelete.Click += new RoutedEventHandler(Delete);
			btnGoHome.Click += new RoutedEventHandler(GoHome);
			btnGoTo.Click += new RoutedEventHandler(GoTo);
			btnSetHome.Click += new RoutedEventHandler(SetHome);
			btnSetPreset.Click += new RoutedEventHandler(SetPreset);
		}

		#region ShowError

		Timer errorTmr = new Timer(5000);
		void errorTmr_Elapsed(object sender, ElapsedEventArgs e) {
			dispatch.BeginInvoke(() => {
				ErrorMessage = "";
			});
		}
		void SetErrorMessage(string text) {
			if (ErrorMessage == "")
				ErrorMessage = text;
			else
				ErrorMessage = ErrorMessage + System.Environment.NewLine + text;

			errorTmr.Interval = 5000;

			errorTmr.AutoReset = false;
			errorTmr.Enabled = true;

			errorTmr.Elapsed -= errorTmr_Elapsed;
			errorTmr.Elapsed += new ElapsedEventHandler(errorTmr_Elapsed);
		}

		string errorMessage = "";
		public string ErrorMessage {
			get { return errorMessage; }
			set {
				errorMessage = value;
				NotifyPropertyChanged("ErrorMessage");
			}
		}

		#endregion ShowError


		#region Presets

		string presetName = "";
		public string PresetName {
			get {
				return presetName;
			}
			set {
				presetName = value;
				NotifyPropertyChanged("PresetName");
			}
		}
		PTZPreset selectedPreset = null;
		public PTZPreset SelectedPreset {
			get {
				return selectedPreset;
			}
			set {
				selectedPreset = value;
				NotifyPropertyChanged("SelectedPreset");
			}
		}

		private void ApplySettings(object sender, RoutedEventArgs e) {
			throw new NotImplementedException();
		}

		void Delete(object sender, RoutedEventArgs e) {
			if (SelectedPreset == null)
				return;
			try {
				subscription.Add(CurrentSession.RemovePreset(profileToken, SelectedPreset.token)
					 .ObserveOnCurrentDispatcher()
					 .Subscribe(presetTok => {
						 ReloadPresets();
					 }, err => {
						 SetErrorMessage(err.Message);
						 dbg.Error(err);
					 }));
			} catch (Exception err) {
				dbg.Error(err);
				//SetErrorMessage(err.Message);
			}
		}
		void GoHome(object sender, RoutedEventArgs e) {
			PTZSpeed speed = new PTZSpeed() {
				panTilt = new Vector2D() {
					x = defPanTiltSpeedSpace == null ? 0 : defPanTiltSpeedSpace.xRange.max,
					y = defPanTiltSpeedSpace == null ? 0 : defPanTiltSpeedSpace.xRange.max
				},
				zoom = new Vector1D() {
					x = defZoomSpeedSpace == null ? 0 : defZoomSpeedSpace.xRange.max
				}
			};
			try {
				subscription.Add(CurrentSession.GotoHomePosition(profileToken, speed).ObserveOnCurrentDispatcher().Subscribe(unit => { }, err => { dbg.Error(err); }));
			} catch (Exception err) {
				dbg.Error(err);
				SetErrorMessage(err.Message);
			}
		}

		void GoTo(object sender, RoutedEventArgs e) {
			if (SelectedPreset == null)
				return;

			PTZSpeed speed = new PTZSpeed() {
				panTilt = new Vector2D() {
					x = defPanTiltSpeedSpace == null ? 0 : defPanTiltSpeedSpace.xRange.max,
					y = defPanTiltSpeedSpace == null ? 0 : defPanTiltSpeedSpace.xRange.max
				},
				zoom = new Vector1D() {
					x = defZoomSpeedSpace == null ? 0 : defZoomSpeedSpace.xRange.max
				}
			};

			try {
				subscription.Add(CurrentSession.GotoPreset(profileToken, SelectedPreset.token, speed)
					 .ObserveOnCurrentDispatcher()
					 .Subscribe(unit => {
					 }, err => {
						 SetErrorMessage(err.Message);
					 }));
			} catch (Exception err) {
				SetErrorMessage(err.Message);
				dbg.Error(err);
			}
		}

		void SetHome(object sender, RoutedEventArgs e) {
			subscription.Add(CurrentSession.SetHomePosition(profileToken)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(
				 unit => { },
				 err => {
					 SetErrorMessage(err.Message);
					 dbg.Error(err);
				 }));
		}
		void ReloadPresets() {
			Presets.Clear();
			subscription.Add(CurrentSession.GetPresets(profileToken)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(presets => {
					 presets.ForEach(pres => {
						 Presets.Add(pres);
					 });
				 }, err => {
					 dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}
		void SetPreset(object sender, RoutedEventArgs e) {
			string defName = "Preset" + System.DateTime.Now.Ticks.ToString();
			if (PresetName != "") {
				defName = PresetName;
			}
			subscription.Add(CurrentSession.SetPreset(profileToken, defName, null)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(presetTok => {
					 //model.presets.Append(new PTZPreset() { token = presetTok });
					 ReloadPresets();
				 }, err => {
					 dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}

		#endregion Presets


		#region Absolute Move

		public void MoveAbsolute(PTZSpeed speed, PTZVector translat) {
			subscription.Add(CurrentSession.AbsoluteMove(profileToken, translat, null)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(unit => {

				 }, err => {
					 //dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}


		float AbsPanValue {
			get { return (float)sliderAbsPanValue.Value; }
			set { sliderAbsPanValue.Value = (float)value; }
		}
		float AbsTiltValue {
			get { return (float)sliderAbsTiltValue.Value; }
			set { sliderAbsTiltValue.Value = (float)value; }
		}
		float AbsZoomValue {
			get { return (float)sliderAbsZoomValue.Value; }
			set { sliderAbsZoomValue.Value = (float)value; }
		}
		float AbsPanTiltSpeed {
			get { return (float)sliderAbsPanTiltSpeed.Value; }
			set { sliderAbsPanTiltSpeed.Value = (float)value; }
		}
		float AbsZoomSpeed {
			get { return (float)sliderAbsZoomSpeed.Value; }
			set { sliderAbsZoomSpeed.Value = (float)value; }
		}

		PTZSpeed CreateAbsPtzSpeed() {
			return new PTZSpeed() {
				panTilt = new Vector2D() {
					x = 0,
					y = 0,
					space = defPanTiltSpeedSpace == null ? null : defPanTiltSpeedSpace.uri
				},
				zoom = new Vector1D() {
					x = 0,
					space = defZoomSpeedSpace == null ? null : defZoomSpeedSpace.uri
				}
			};
		}

		PTZVector CreateAbsPtzVector() {
			return new PTZVector() {
				panTilt = new Vector2D() {
					x = 0,
					y = 0,
					space = defAbsPanTiltPosSpace == null ? null : defAbsPanTiltPosSpace.uri
				},
				zoom = new Vector1D() {
					x = 0,
					space = defAbsZoomPosSpace == null ? null : defAbsZoomPosSpace.uri
				}
			};
		}

		void AbsoluteMove_Click(object sender, RoutedEventArgs e) {
			var speed = CreateAbsPtzSpeed();
			speed.panTilt.x = AbsPanTiltSpeed;
			speed.panTilt.y = AbsPanTiltSpeed;
			speed.zoom.x = AbsZoomSpeed;

			var vector = CreateAbsPtzVector();
			vector.panTilt.x = AbsPanValue;
			vector.panTilt.y = AbsTiltValue;
			vector.zoom.x = AbsZoomValue;

			MoveAbsolute(speed, vector);
		}

		#endregion Absolute Move

		#region Relative Move

		public void MoveRelative(PTZSpeed speed, PTZVector translat) {
			subscription.Add(CurrentSession.RelativeMove(profileToken, translat, speed)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(unit => {

				 }, err => {
					 //dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}


		float RelPanValue {
			get { return (float)sliderRelPanValue.Value; }
			set { sliderRelPanValue.Value = (float)value; }
		}
		float RelTiltValue {
			get { return (float)sliderRelTiltValue.Value; }
			set { sliderRelTiltValue.Value = (float)value; }
		}
		float RelZoomValue {
			get { return (float)sliderRelZoomValue.Value; }
			set { sliderRelZoomValue.Value = (float)value; }
		}
		float RelPanTiltSpeed {
			get { return (float)sliderRelPanTiltSpeed.Value; }
			set { sliderRelPanTiltSpeed.Value = (float)value; }
		}
		float RelZoomSpeed {
			get { return (float)sliderRelZoomSpeed.Value; }
			set { sliderRelZoomSpeed.Value = (float)value; }
		}

		PTZSpeed CreateRelPtzSpeed() {
			return new PTZSpeed() {
				panTilt = new Vector2D() {
					x = 0,
					y = 0,
					space = defPanTiltSpeedSpace == null ? null : defPanTiltSpeedSpace.uri
				},
				zoom = new Vector1D() {
					x = 0,
					space = defZoomSpeedSpace == null ? null : defZoomSpeedSpace.uri
				}
			};
		}

		PTZVector CreateRelPtzVector() {
			return new PTZVector() {
				panTilt = new Vector2D() {
					x = 0,
					y = 0,
					space = defRelPanTiltTranslSpace == null ? null : defRelPanTiltTranslSpace.uri
				},
				zoom = new Vector1D() {
					x = 0,
					space = defRelZoomTranslSpace == null ? null : defRelZoomTranslSpace.uri
				}
			};
		}

		void RelUp_Click(object sender, RoutedEventArgs e) {
			var speed = CreateRelPtzSpeed();
			speed.panTilt.y = RelPanTiltSpeed;
			speed.zoom = null;

			var translat = CreateRelPtzVector();
			translat.panTilt.y = Math.Abs(RelTiltValue);
			translat.zoom = null;

			MoveRelative(speed, translat);
		}

		void RelDown_Click(object sender, RoutedEventArgs e) {
			var speed = CreateRelPtzSpeed();
			speed.panTilt.y = RelPanTiltSpeed;
			speed.zoom = null;

			var translat = CreateRelPtzVector();
			translat.panTilt.y = -1 * Math.Abs(RelTiltValue);
			translat.zoom = null;

			MoveRelative(speed, translat);
		}

		void RelLeft_Click(object sender, RoutedEventArgs e) {
			var speed = CreateRelPtzSpeed();
			speed.panTilt.x = RelPanTiltSpeed;
			speed.zoom = null;

			var translat = CreateRelPtzVector();
			translat.panTilt.x = -1 * Math.Abs(RelPanValue);
			translat.zoom = null;

			MoveRelative(speed, translat);
		}
		void RelRight_Click(object sender, RoutedEventArgs e) {
			var speed = CreateRelPtzSpeed();
			speed.panTilt.x = RelPanTiltSpeed;
			speed.zoom = null;

			var translat = CreateRelPtzVector();
			translat.panTilt.x = Math.Abs(RelPanValue);
			translat.zoom = null;

			MoveRelative(speed, translat);
		}

		void RelZoomPlus_Click(object sender, RoutedEventArgs e) {
			var speed = CreateRelPtzSpeed();
			speed.zoom.x = RelZoomSpeed;
			speed.panTilt = null;

			var translat = CreateRelPtzVector();
			translat.zoom.x = Math.Abs(RelZoomValue);
			translat.panTilt = null;

			MoveRelative(speed, translat);
		}


		void RelZoomMinus_Click(object sender, RoutedEventArgs e) {
			var speed = CreateRelPtzSpeed();
			speed.zoom.x = RelZoomSpeed;
			speed.panTilt = null;

			var translat = CreateRelPtzVector();
			translat.zoom.x = -1 * Math.Abs(RelZoomValue);
			translat.panTilt = null;

			MoveRelative(speed, translat);
		}

		void RelativeMove_Click(object sender, RoutedEventArgs e) {
			var speed = CreateRelPtzSpeed();
			speed.panTilt.x = RelPanTiltSpeed;
			speed.panTilt.y = RelPanTiltSpeed;
			speed.zoom.x = RelZoomSpeed;

			var translat = CreateRelPtzVector();
			translat.panTilt.x = RelPanValue;
			translat.panTilt.y = RelTiltValue;
			translat.zoom.x = RelZoomValue;

			MoveRelative(speed, translat);
		}

		#endregion Relative Move


		#region Continuous Move

		public void MoveContinuous(PTZSpeed speed) {
			subscription.Add(CurrentSession.ContinuousMove(profileToken, speed, null)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(unit => {

				 }, err => {
					 //dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}

		public void StopZoom() {
			subscription.Add(CurrentSession.Stop(profileToken, false, true)
			.ObserveOnCurrentDispatcher()
			.Subscribe(unit => {
			}, err => {
				dbg.Error(err);
				SetErrorMessage(err.Message);
			}));
		}

		public void StopMovement() {
			subscription.Add(CurrentSession.Stop(profileToken, true, false)
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(unit => {
				 }, err => {
					 //dbg.Error(err);
					 SetErrorMessage(err.Message);
				 }));
		}


		float ContPanVelocity {
			get { return (float)sliderContPanVelocity.Value; }
			set { sliderContPanVelocity.Value = (float)value; }
		}
		float ContTiltVelocity {
			get { return (float)sliderContTiltVelocity.Value; }
			set { sliderContTiltVelocity.Value = (float)value; }
		}
		float ContZoomVelocity {
			get { return (float)sliderContZoomVelocity.Value; }
			set { sliderContZoomVelocity.Value = (float)value; }
		}

		void ContinuousUp_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopMovement();
		}


		PTZSpeed CreateContPtzSpeed() {
			return new PTZSpeed() {
				panTilt = new Vector2D() {
					x = 0,
					y = 0,
					space = defContPanTiltVelSpace == null ? null : defContPanTiltVelSpace.uri
				},
				zoom = new Vector1D() {
					x = 0,
					space = defContZoomVelSpace == null ? null : defContZoomVelSpace.uri
				}
			};
		}

		void ContinuousUp_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = CreateContPtzSpeed();
			speed.panTilt.y = Math.Abs(ContTiltVelocity);
			speed.zoom = null;

			MoveContinuous(speed);
		}
		void ContinuousDown_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopMovement();
		}
		void ContinuousDown_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = CreateContPtzSpeed();
			speed.panTilt.y = -1 * Math.Abs(ContTiltVelocity);
			speed.zoom = null;

			MoveContinuous(speed);
		}
		void ContinuousLeft_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopMovement();
		}
		void ContinuousLeft_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = CreateContPtzSpeed();
			speed.panTilt.x = -1 * Math.Abs(ContPanVelocity);
			speed.zoom = null;

			MoveContinuous(speed);
		}
		void ContinuousRight_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopMovement();
		}
		void ContinuousRight_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = CreateContPtzSpeed();
			speed.panTilt.x = Math.Abs(ContPanVelocity);
			speed.zoom = null;

			MoveContinuous(speed);
		}


		void ContinuousZoomMinus_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopZoom();
		}
		void ContinuousZoomMinus_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = CreateContPtzSpeed();
			speed.zoom.x = -1 * Math.Abs(ContZoomVelocity);
			speed.panTilt = null;

			MoveContinuous(speed);
		}
		void ContinuousZoomPlus_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopZoom();
		}
		void ContinuousZoomPlus_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = CreateContPtzSpeed();
			speed.zoom.x = Math.Abs(ContZoomVelocity);
			speed.panTilt = null;

			MoveContinuous(speed);
		}

		#endregion Continuous Move


		#region VideoPlayback

		IPlaybackSession playbackSession;
		void VideoStartup() {
			var playerAct = activityContext.container.Resolve<IVideoPlayerActivity>();

			var playerModel = new VideoPlayerActivityModel(
				 profileToken: model.profToken,
				 showStreamUrl: false,
				 streamSetup: new StreamSetup() {
					 stream = StreamType.rtpUnicast,
					 transport = new Transport() {
						 protocol = AppDefaults.visualSettings.Transport_Type,
						 tunnel = null
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

		public new bool Initialized(IPlaybackSession playbackSession) {
			this.playbackSession = playbackSession;
			return true;
		}

		public void Shutdown() {
		}
		#endregion VideoPlayback


		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}

	public interface IPtzInfo {
		string ProfileToken { get; set; }
	}
	public class PtzInfo : IPtzInfo {
		public string ProfileToken { get; set; }
	}
}
