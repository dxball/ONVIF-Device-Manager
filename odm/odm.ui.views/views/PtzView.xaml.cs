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
		ObservableCollection<PTZNode> Nodes;
		public ObservableCollection<PTZPreset> Presets { get; private set; }
		
		public readonly Dispatcher dispatch = Dispatcher.CurrentDispatcher;
		private CompositeDisposable disposables = new CompositeDisposable();
		public CompositeDisposable subscription = new CompositeDisposable();
		public INvtSession CurrentSession;
		public IAccount CurrentAccount;
		public String ChannelToken;
		public string profileToken;
		IMovementState movement;
		bool isAbsolute = false;
		public bool IsAbsolute {
			get {
				return isAbsolute;
			}
			set {
				isAbsolute = value;
				NotifyPropertyChanged("IsAbsolute");
			}
		}

		//public PTZViewModel viewModel { get; private set; }

		private void Init(Model model) {
			this.model = model;

			OnCompleted += () => {
				//TODO: release player host
				disposables.Dispose();
				subscription.Dispose();
				//viewModel.Dispose();
			};
			//viewModel = new PTZViewModel(activityContext.container, model, mod => {
			//    //success(new Result.Close());
			//});
			this.DataContext = this;
			InitializeComponent();

			Nodes = new ObservableCollection<PTZNode>();
			Presets = new ObservableCollection<PTZPreset>();

			var ptzInfo = activityContext.container.Resolve<IPtzInfo>();
			CurrentSession = activityContext.container.Resolve<INvtSession>();
			profileToken = ptzInfo.ProfileToken;

			VideoStartup(model);
			InitData(model);

			BindData(model);
			Localize();
		}
		public LocalPTZ Strings { get { return LocalPTZ.instance; } }
		void Localize() { 
		}
		Space2DDescription pantiltSpace;
		Space1DDescription zoomSpace;
		void InitData(PtzView.Model model) {
			pantiltSpace = model.currentNode.SupportedPTZSpaces.AbsolutePanTiltPositionSpace.FirstOrDefault();
			if (pantiltSpace == null) {
				dbg.Error("No tilt spaces available");
				return;
			}
			zoomSpace = model.currentNode.SupportedPTZSpaces.AbsoluteZoomPositionSpace.FirstOrDefault();
			if (pantiltSpace == null) {
				dbg.Error("No zoom spaces available");
				return;
			}

			Nodes.Clear();
			Presets.Clear();
			model.nodes.ForEach(x => { Nodes.Add(x); });

			this.CreateBinding(SelectedNodeProperty, model, x => {
				return x.currentNode;
			}, (m, v) => {
				m.currentNode = v;
			});
			model.presets.ForEach(x => { Presets.Add(x); });
			
			movement = new RelativeState(CurrentSession, profileToken);
			IsAbsolute = false;
		}
		void SetAbsolute() {
			movement = new AbsoluteState(CurrentSession, profileToken);
		}
		void SetRelative() {
			movement = new RelativeState(CurrentSession, profileToken);
		}


		interface IMovementState {
			FSharpAsync<Microsoft.FSharp.Core.Unit> MoveLeft(int val);
			FSharpAsync<Microsoft.FSharp.Core.Unit> MoveRight(int val);
			FSharpAsync<Microsoft.FSharp.Core.Unit> MoveUp(int val);
			FSharpAsync<Microsoft.FSharp.Core.Unit> MoveDown(int val);
		}
		public class AbsoluteState : IMovementState {
			public AbsoluteState(INvtSession session, string profToken) {
				this.session = session;
				this.profToken = profToken;
			}
			INvtSession session;
			string profToken;
			public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveLeft(int val) {
				PTZVector position = new PTZVector();
				position.PanTilt = new Vector2D();
				position.PanTilt.x = -val;
				PTZSpeed speed = new PTZSpeed(); 
				speed.PanTilt = new Vector2D();
				speed.PanTilt.x = 1;
				return session.AbsoluteMove(profToken, position, speed);
			}

			public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveRight(int val) {
				PTZVector position = new PTZVector();
				position.PanTilt = new Vector2D();
				position.PanTilt.x = val;
				PTZSpeed speed = new PTZSpeed();
				speed.PanTilt = new Vector2D();
				speed.PanTilt.x = 1;
				return session.AbsoluteMove(profToken, position, speed);
			}

			public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveUp(int val) {
				PTZVector position = new PTZVector();
				position.PanTilt = new Vector2D();
				position.PanTilt.y = -val;
				PTZSpeed speed = new PTZSpeed();
				speed.PanTilt = new Vector2D();
				speed.PanTilt.y = 1;
				return session.AbsoluteMove(profToken, position, speed);
			}

			public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveDown(int val) {
				PTZVector position = new PTZVector();
				position.PanTilt = new Vector2D();
				position.PanTilt.y = val;
				PTZSpeed speed = new PTZSpeed();
				speed.PanTilt = new Vector2D();
				speed.PanTilt.y = 1;
				return session.AbsoluteMove(profToken, position, speed);
			}
		}
		class RelativeState : IMovementState {
			public RelativeState(INvtSession session, string profToken) {
				this.session = session;
				this.profToken = profToken;
			}
			INvtSession session;
			string profToken;
			public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveLeft(int val) {
				PTZVector position = new PTZVector();
				position.PanTilt = new Vector2D();
				position.PanTilt.x = -val;
				PTZSpeed speed = new PTZSpeed();
				speed.PanTilt = new Vector2D();
				speed.PanTilt.x = 1;
				return session.RelativeMove(profToken, position, speed);
			}

			public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveRight(int val) {
				PTZVector position = new PTZVector();
				position.PanTilt = new Vector2D();
				position.PanTilt.x = val;
				PTZSpeed speed = new PTZSpeed();
				speed.PanTilt = new Vector2D();
				speed.PanTilt.x = 1;
				return session.RelativeMove(profToken, position, speed);
			}

			public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveUp(int val) {
				PTZVector position = new PTZVector();
				position.PanTilt = new Vector2D();
				position.PanTilt.y = -val;
				PTZSpeed speed = new PTZSpeed();
				speed.PanTilt = new Vector2D();
				speed.PanTilt.y = 1;
				return session.RelativeMove(profToken, position, speed);
			}

			public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveDown(int val) {
				PTZVector position = new PTZVector();
				position.PanTilt = new Vector2D();
				position.PanTilt.y = val;
				PTZSpeed speed = new PTZSpeed();
				speed.PanTilt = new Vector2D();
				speed.PanTilt.y = 1;
				return session.RelativeMove(profToken, position, speed);
			}
		}

		void BindData(PtzView.Model model) {
			//CommonData
			valuePresetName.CreateBinding(TextBox.TextProperty, this, x => x.PresetName, (m, v) => { m.PresetName = v; });
			valuePresetsList.ItemsSource = Presets;
			valuePresetsList.CreateBinding(ListBox.SelectedItemProperty, this, x => x.SelectedPreset, (m, v) => {
				m.SelectedPreset = v;
			});
			//ReloadPresets();

			captionErrorMessage.CreateBinding(TextBlock.TextProperty, this, x => x.ErrorMessage);

			PanSpeed = model.panMax;
			TiltSpeed = model.tiltMax;
			ZoomSpeed = model.zoomMax;

			valueRelAbsMove.Text = model.panMax.ToString();
			valueRelAbsZoom.Text = model.zoomMax.ToString();

			sliderPanSpeed.Minimum = pantiltSpace.XRange.Min;
			sliderPanSpeed.Maximum = pantiltSpace.XRange.Max;
			sliderPanSpeed.CreateBinding(Slider.ValueProperty, this,
				x => { 
					return (double)x.PanSpeed; 
				}, (m, v) => {
					float val = (float)v;
					m.PanSpeed = val; 
				});
			sliderTiltSpeed.Minimum = pantiltSpace.YRange.Min;
			sliderTiltSpeed.Maximum = pantiltSpace.YRange.Max;
			sliderTiltSpeed.CreateBinding(Slider.ValueProperty, this,
				x => {
					return (double)x.TiltSpeed;
				}, (m, v) => {
					float val = (float)v;
					m.TiltSpeed = val; 
				});
			sliderZoomSpeed.Minimum = zoomSpace.XRange.Min;
			sliderZoomSpeed.Maximum = zoomSpace.XRange.Max;
			sliderZoomSpeed.CreateBinding(Slider.ValueProperty, this,
				x => {
					return (double)x.ZoomSpeed;
				}, (m, v) => {
					float val = (float)v;
					m.ZoomSpeed = val; 
				});

			//Buttons
			tglbtnAbsoluteMove.CreateBinding(ToggleButton.IsCheckedProperty, this, x => x.IsAbsolute, (m, v) => { 
				m.IsAbsolute = v;
				if (m.IsAbsolute)
					m.SetAbsolute();
				else
					m.SetRelative();
			});
			tglbtnRelativeMove.CreateBinding(ToggleButton.IsCheckedProperty, this, x => !x.IsAbsolute, (m,v)=>{
				m.IsAbsolute = !v;
				if (m.IsAbsolute)
					m.SetAbsolute();
				else
					m.SetRelative();
			});

			btnContinuseUp.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(ContinuesUp_MouseDown);
			btnContinuseUp.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(ContinuesUp_MouseUp);
			btnContinuseDown.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(ContinuesDown_MouseDown);
			btnContinuseDown.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(ContinuesDown_MouseUp);
			btnContinuesLeft.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(ContinuesLeft_MouseDown);
			btnContinuesLeft.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(ContinuesLeft_MouseUp);
			btnContinuesRight.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(ContinuesRight_MouseDown);
			btnContinuesRight.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(ContinuesRight_MouseUp);
			btnContinuesZoomMinus.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(ContinuesZoomMinus_MouseDown);
			btnContinuesZoomMinus.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(ContinuesZoomMinus_MouseUp);
			btnContinuesZoomPlus.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(ContinuesZoomPlus_MouseDown);
			btnContinuesZoomPlus.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(ContinuesZoomPlus_MouseUp);

			btnRelAbsDown.Click += new RoutedEventHandler(RelAbsDown);
			btnRelAbsLeft.Click += new RoutedEventHandler(RelAbsLeft);
			btnRelAbsRight.Click += new RoutedEventHandler(RelAbsRight);
			btnRelAbsUp.Click += new RoutedEventHandler(RelAbsUp);
			btnRelAbsZoomMinus.Click += new RoutedEventHandler(RelAbsZoomMinus);
			btnRelAbsZoomPlus.Click += new RoutedEventHandler(RelAbsZoomPlus);

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
#endregion ShowError
		public void MoveRelative(PTZSpeed speed, PTZVector translat) {
			subscription.Add(CurrentSession.RelativeMove(profileToken, translat, speed)
				.ObserveOnCurrentDispatcher()
				.Subscribe(unit => {

				}, err => {
					//dbg.Error(err);
					SetErrorMessage(err.Message);
				}));
		}
		public void MoveAbsolute(PTZSpeed speed, PTZVector translat) {
			subscription.Add(CurrentSession.AbsoluteMove(profileToken, translat, speed)
				.ObserveOnCurrentDispatcher()
				.Subscribe(unit => {

				}, err => {
					//dbg.Error(err);
					SetErrorMessage(err.Message);
				}));
		}
		public void MoveContinues(PTZSpeed speed) {
			subscription.Add(CurrentSession.ContinuousMove(profileToken, speed, global::onvif.services.Duration.FromSeconds(10))
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

		float zoomSpeed;
		float ZoomSpeed {
			get { return zoomSpeed; }
			set {
				zoomSpeed = value;
				NotifyPropertyChanged("ZoomSpeed");
			}
		}
		float tiltSpeed;
		float TiltSpeed {
			get { return tiltSpeed; }
			set {
				tiltSpeed = value;
				NotifyPropertyChanged("TiltSpeed");
			}
		}
		float panSpeed;
		float PanSpeed {
			get { return panSpeed; }
			set {
				panSpeed = value;
				NotifyPropertyChanged("PanSpeed");
			}
		}
		string errorMessage = "";
		public string ErrorMessage {
			get { return errorMessage; }
			set {
				errorMessage = value;
				NotifyPropertyChanged("ErrorMessage");
			}
		}
		public PTZNode SelectedNode {get { return (PTZNode)GetValue(SelectedNodeProperty); }set { SetValue(SelectedNodeProperty, value); }}
		public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(PTZNode), typeof(PtzView));

#region RelativeAbsoluteMooveZoom
		int RelAbsMove {
			get {
				int i = 0;
				Int32.TryParse(valueRelAbsMove.Text, out i);
				return i;
			}
		}
		int RelAbsZoom {
			get {
				int i = 0;
				Int32.TryParse(valueRelAbsZoom.Text, out i);
				return i;
			}
		}
		void RelAbsUp(object sender, RoutedEventArgs e){
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = GetTiltSpeed();
			speed.Zoom.x = 0;

			PTZVector translat = new PTZVector();
			translat.PanTilt = new Vector2D();
			translat.PanTilt.x = 0;
			translat.PanTilt.y = RelAbsMove;
			translat.Zoom = new Vector1D();
			translat.Zoom.x = 0;

			if (IsAbsolute) {
				MoveAbsolute(speed, translat);
			} else {
				MoveRelative(speed, translat);
			}
		}
		void RelAbsDown(object sender, RoutedEventArgs e){
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = GetTiltSpeed();
			speed.Zoom.x = 0;

			PTZVector translat = new PTZVector();
			translat.PanTilt = new Vector2D();
			translat.PanTilt.x = 0;
			translat.PanTilt.y = (-1)*RelAbsMove;
			translat.Zoom = new Vector1D();
			translat.Zoom.x = 0;

			if (IsAbsolute) {
				MoveAbsolute(speed, translat);
			} else {
				MoveRelative(speed, translat);
			}
		}
		void RelAbsLeft(object sender, RoutedEventArgs e){
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = GetTiltSpeed();
			speed.Zoom.x = 0;

			PTZVector translat = new PTZVector();
			translat.PanTilt = new Vector2D();
			translat.PanTilt.x = (-1)*RelAbsMove;
			translat.PanTilt.y = 0;
			translat.Zoom = new Vector1D();
			translat.Zoom.x = 0;

			if (IsAbsolute) {
				MoveAbsolute(speed, translat);
			} else {
				MoveRelative(speed, translat);
			}
		}
		void RelAbsRight(object sender, RoutedEventArgs e){
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = GetTiltSpeed();
			speed.Zoom.x = 0;

			PTZVector translat = new PTZVector();
			translat.PanTilt = new Vector2D();
			translat.PanTilt.x = RelAbsMove;
			translat.PanTilt.y = 0;
			translat.Zoom = new Vector1D();
			translat.Zoom.x = 0;

			if (IsAbsolute) {
				MoveAbsolute(speed, translat);
			} else {
				MoveRelative(speed, translat);
			}
		}
		void RelAbsZoomPlus(object sender, RoutedEventArgs e){
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = GetTiltSpeed();
			speed.Zoom.x = 0;

			PTZVector translat = new PTZVector();
			translat.PanTilt = new Vector2D();
			translat.PanTilt.x = 0;
			translat.PanTilt.y = 0;
			translat.Zoom = new Vector1D();
			translat.Zoom.x = RelAbsZoom;

			if (IsAbsolute) {
				MoveAbsolute(speed, translat);
			} else {
				MoveRelative(speed, translat);
			}
		}
		void RelAbsZoomMinus(object sender, RoutedEventArgs e){
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = GetTiltSpeed();
			speed.Zoom.x = 0;

			PTZVector translat = new PTZVector();
			translat.PanTilt = new Vector2D();
			translat.PanTilt.x = 0;
			translat.PanTilt.y = 0;
			translat.Zoom = new Vector1D();
			translat.Zoom.x = (-1) * RelAbsZoom;

			if (IsAbsolute) {
				MoveAbsolute(speed, translat);
			} else {
				MoveRelative(speed, translat);
			}
		}
#endregion RelativeAbsoluteMooveZoom
#region ContinuesMooveZoom
		float GetPanSpeed() {
			if (pantiltSpace.XRange.Max - pantiltSpace.XRange.Min == 0)
				return 0;
			else
				return PanSpeed;
		}
		float GetTiltSpeed() {
			if (pantiltSpace.YRange.Max - pantiltSpace.YRange.Min == 0)
				return 0;
			else
				return TiltSpeed;
		}
		float GetZoomSpeed() {
			if (zoomSpace.XRange.Max - zoomSpace.XRange.Min == 0)
				return 0;
			else
				return ZoomSpeed;
		}
		void ContinuesUp_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopMovement();
		}
		void ContinuesUp_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = GetTiltSpeed();
			speed.Zoom.x = 0;

			MoveContinues(speed);
		}
		void ContinuesDown_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopMovement();
		}
		void ContinuesDown_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = -1 * GetTiltSpeed();
			speed.Zoom.x = 0;

			MoveContinues(speed);
		}
		void ContinuesLeft_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopMovement();
		}
		void ContinuesLeft_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = -1 * GetPanSpeed();
			speed.PanTilt.y = 0;
			speed.Zoom.x = 0;

			MoveContinues(speed);
		}
		void ContinuesRight_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopMovement();
		}
		void ContinuesRight_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = GetPanSpeed();
			speed.PanTilt.y = 0;
			speed.Zoom.x = 0;

			MoveContinues(speed);
		}
		void ContinuesZoomMinus_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopZoom();
		}
		void ContinuesZoomMinus_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = 0;
			speed.Zoom.x = -1 * GetZoomSpeed();

			MoveContinues(speed);
		}
		void ContinuesZoomPlus_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			StopZoom();
		}
		void ContinuesZoomPlus_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			//speed.PanTilt.space = pantiltSpace.URI;
			speed.Zoom = new Vector1D();
			//speed.Zoom.space = zoomSpace.URI;
			speed.PanTilt.x = 0;
			speed.PanTilt.y = 0;
			speed.Zoom.x = GetZoomSpeed();

			MoveContinues(speed);
		}
#endregion ContinuesMooveZoom
#region settings
		void ApplySettings(object sender, RoutedEventArgs e) {
			
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
			PTZSpeed speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			speed.PanTilt.x = pantiltSpace.XRange.Max;
			speed.PanTilt.y = pantiltSpace.YRange.Max;
			speed.Zoom = new Vector1D();
			speed.Zoom.x = zoomSpace.XRange.Max;
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
			var speed = new PTZSpeed();
			speed.PanTilt = new Vector2D();
			speed.Zoom = new Vector1D();
			speed.PanTilt.x = pantiltSpace.XRange.Max;
			speed.PanTilt.y = pantiltSpace.YRange.Max;
			speed.Zoom.x = zoomSpace.XRange.Max;
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
#endregion settings

#region VideoPlayback
		IPlaybackSession playbackSession;
		void VideoStartup(Model model) {
			var playerAct = activityContext.container.Resolve<IVideoPlayerActivity>();

			var playerModel = new VideoPlayerActivityModel(
				profileToken: model.profToken,
				showStreamUrl: false,
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
