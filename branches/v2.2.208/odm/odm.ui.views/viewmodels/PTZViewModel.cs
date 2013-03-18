using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.core;
using odm.infra;
using odm.models;
using odm.ui.activities;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views;
using utils;
using onvif.services;
using System.Timers;
using System.ComponentModel;
using System.Windows.Threading;

namespace odm.ui.viewModels {
	//public interface IPtzInfo {
	//    string ProfileToken { get; set; }
	//}
	//public class PtzInfo:IPtzInfo {
	//    public string ProfileToken { get; set; }
	//}
	public class PTZViewModel : DependencyObject, IDisposable, INotifyPropertyChanged {
		public String ChannelToken;
		public string profileToken;
		public IUnityContainer container;
		public INvtSession CurrentSession;
		public Account CurrentAccount;

		public readonly Dispatcher dispatch;
		public CompositeDisposable subscription;

		public PTZViewModel(IUnityContainer container, PtzView.Model model, Action<PtzView.Model> apply){
			this.container = container;
			dispatch = Dispatcher.CurrentDispatcher;
			subscription = new CompositeDisposable();
			
			Nodes = new ObservableCollection<PTZNode>();
            Presets = new ObservableCollection<PTZPreset>();

            CurrentSession = container.Resolve<INvtSession>();
            //ChannelToken = container.Resolve<VideoSourceToken>();
			var ptzInfo = container.Resolve<IPtzInfo>();

			profileToken = ptzInfo.ProfileToken;
            
            this.model = model;
            this.apply = apply;
            if (apply == null)
                throw new ArgumentNullException("Action<PtzModel> apply");

            InitSubscribe(CurrentSession);
            InitAbsoluteRelative();

            ErrorMessage = "";
        }

        public PropertyPTZStrings Strings { get { return PropertyPTZStrings.instance; } }
        IMovementState movement;
        
        PtzView.Model model;

        Action<PtzView.Model> apply;
        
        void Reload() {
            subscription.Dispose();
            subscription = new CompositeDisposable();

            InitSubscribe(CurrentSession);
        }

        void InitAbsoluteRelative() {
            if (IsAbsolute) {
                this.CreateBinding(ContentRelativeMoveProperty, Strings, x => x.absolute);
                this.CreateBinding(ContentZoomProperty, Strings, x => x.absoluteZoom); 
                movement = new AbsoluteState(CurrentSession, profileToken);
            } else {
                this.CreateBinding(ContentRelativeMoveProperty, Strings, x => x.relativemove);
                this.CreateBinding(ContentZoomProperty, Strings, x => x.zoom);
                movement = new RelativeState(CurrentSession, profileToken);
            }
        }

        void InitData(PtzView.Model  model) {
            Nodes.Clear();
            Presets.Clear();
            //model.nodes.ForEach(x => { Nodes.Add(x); });
			//this.CreateBinding(SelectedNodeProperty, model, x => { 
			//    return x.currentNode;
			//}, (m, v) => {
			//    m.currentNode = v;
			//});
            model.presets.ForEach(x => { Presets.Add(x); });
        }

        void InitSubscribe(INvtSession session) {
            InitData(model);
        }

        public void Load(INvtSession session, String chanToken, string profileToken, Account account, IVideoInfo videoInfo) {
        }
        #region ContinuesProcess
		public void MoveUpContinues() {
            var speed = new PTZSpeed();
            speed.panTilt = new Vector2D();
            speed.zoom = new Vector1D();
            speed.panTilt.x = 0;
            speed.panTilt.y = 1;
            speed.zoom.x = 0;

            MoveContinues(speed);
        }
		public void MoveDownContinues() {
            var speed = new PTZSpeed();
            speed.panTilt = new Vector2D();
            speed.zoom = new Vector1D();
            speed.panTilt.x = 0;
            speed.panTilt.y = -1;
            speed.zoom.x = 0;

            MoveContinues(speed);
        }
		public void MoveRightContinues() {
            var speed = new PTZSpeed();
            speed.panTilt = new Vector2D();
            speed.zoom = new Vector1D();
            speed.panTilt.x = 1;
            speed.panTilt.y = 0;
            speed.zoom.x = 0;

            MoveContinues(speed);
        }
		public void MoveLeftContinues() {
            var speed = new PTZSpeed();
            speed.panTilt = new Vector2D();
            speed.zoom = new Vector1D();
            speed.panTilt.x = -1;
            speed.panTilt.y = 0;
            speed.zoom.x = 0;

            MoveContinues(speed);
        }
		public void ZoomOutContinues() {
            var speed = new PTZSpeed();
            speed.panTilt = new Vector2D();
            speed.zoom = new Vector1D();
            speed.panTilt.x = 0;
            speed.panTilt.y = 0;
            speed.zoom.x = -1;

            MoveContinues(speed);
        }
		public void ZoomInContinues() {
            var speed = new PTZSpeed();
            speed.panTilt = new Vector2D();
            speed.zoom = new Vector1D();
            speed.panTilt.x = 0;
            speed.panTilt.y = 0;
            speed.zoom.x = 1;

            MoveContinues(speed);
        }
		public void MoveContinues(PTZSpeed speed) {
            CurrentSession.ContinuousMove(profileToken, speed, global::onvif.services.XsDuration.FromSeconds(10))
                .ObserveOnCurrentDispatcher()
                .Subscribe(unit => {

                }, err => {
                    dbg.Error(err);
                    SetErrorMessage(err.Message);
                });
        }
		public void StopZoom() {
            CurrentSession.Stop(profileToken, false, true)
            .ObserveOnCurrentDispatcher()
            .Subscribe(unit => {
            }, err => {
                dbg.Error(err);
                SetErrorMessage(err.Message);
            });
        }
		public void StopMovement() {
            CurrentSession.Stop(profileToken, true, false)
                .ObserveOnCurrentDispatcher()
                .Subscribe(unit => {
                }, err => {
                    dbg.Error(err);
                    SetErrorMessage(err.Message);
                });
        }

		public void MouseUpZoomContinuesMinus() {
            StopZoom();
        }
		public void MouseDownZoomContinuesMinus() {
            ZoomOutContinues(); 
        }
		public void MouseUpZoomContinuesPlus() {
            StopZoom();
        }
		public void MouseDownZoomContinusePlus() {
            ZoomInContinues(); 
        }

		public void MouseUpContinuesDown() {
            StopMovement();
        }
		public void MouseDownContinuesDown() {
            MoveDownContinues();
        }
		public void MouseUpContinuesLeft() {
            StopMovement();
        }

		public void MouseDownContinuesLeft() {
            MoveLeftContinues();
        }
		public void MouseUpContinuesRight() {
            StopMovement();
        }
		public void MouseDownContinuesRight() {
            MoveRightContinues();
        }
		public void MouseUpContinuesUp() {
            StopMovement();
        }
		public void MouseDownContinuesUp() {
            MoveUpContinues();
        }
        #endregion ContinuesProcess        

        void ReloadPresets() {
            Reload();
        }

        public void ButtonGoHome() {
            PTZSpeed speed = new PTZSpeed();
            speed.panTilt = new Vector2D();
            speed.panTilt.x = 1;
            speed.panTilt.y = 1;
            speed.zoom = new Vector1D();
            speed.zoom.x = 1;
            try {
                CurrentSession.GotoHomePosition(profileToken, speed).ObserveOnCurrentDispatcher().Subscribe(unit => { }, err => { dbg.Error(err); });
            } catch (Exception err) {
                dbg.Error(err);
                SetErrorMessage(err.Message);
            }
        }
		public void ButtonRelLeft() { 
            int val;
            if(int.TryParse(ValueRelMove, out val))
                val = 0;
            movement.MoveLeft(val)
                .ObserveOnCurrentDispatcher()
                .Subscribe(unit => { 
                }, err => {
                    SetErrorMessage(err.Message);
                });
        }
		public void ButtonRelRight() {
            int val;
            if (int.TryParse(ValueRelMove, out val))
                val = 0;
            movement.MoveRight(val)
                .ObserveOnCurrentDispatcher()
                .Subscribe(unit => {
                }, err => {
                    SetErrorMessage(err.Message);
                });
        }
		public void ButtonRelTop() {
            int val;
            if (int.TryParse(ValueRelMove, out val))
                val = 0;
            movement.MoveUp(val)
                .ObserveOnCurrentDispatcher()
                .Subscribe(unit => {
                }, err => {
                    SetErrorMessage(err.Message);
                });
        }
		public void ButtonRelDown() {
            int val;
            if (int.TryParse(ValueRelMove, out val))
                val = 0;
            movement.MoveDown(val)
                .ObserveOnCurrentDispatcher()
                .Subscribe(unit => {
                }, err => {
                    SetErrorMessage(err.Message);
                });
        }
		public void ButtonSetHome() {
            CurrentSession.SetHomePosition(profileToken)
				.ObserveOnCurrentDispatcher()
				.Subscribe(
				unit => { }, 
				err => { 
					SetErrorMessage(err.Message); 
					dbg.Error(err); 
				});
        }
		public void ButtonSetPreset() {
            string defName = "Preset" + System.DateTime.Now.Ticks.ToString();
            if(PresetName != ""){
                defName = PresetName;
            }
            CurrentSession.SetPreset(profileToken, defName, null)
                .ObserveOnCurrentDispatcher()
                .Subscribe(presetTok => {

                    model.presets.Append(new PTZPreset() { token = presetTok });
                    ReloadPresets();

                }, err => {
                    dbg.Error(err);
                    SetErrorMessage(err.Message);
                });
        }
		public void ButtonZoomMinus() { }
		public void ButtonZoomPlus() { }
		public void PresetGoTo() {
            if (SelectedPreset == null)
                return;
            var speed = new PTZSpeed();
            speed.panTilt = new Vector2D();
            speed.zoom = new Vector1D();
            speed.panTilt.x = 1;
            speed.panTilt.y = 1;
            speed.zoom.x = 1;
            try {
                CurrentSession.GotoPreset(profileToken, SelectedPreset.token, speed)
                    .ObserveOnCurrentDispatcher()
                    .Subscribe(unit => {
                    }, err => {
                        SetErrorMessage(err.Message);
                    });
            } catch (Exception err) {
                SetErrorMessage(err.Message);
                dbg.Error(err);
            }
        }
		public void PresetDelete() {
            try {
                CurrentSession.RemovePreset(profileToken, SelectedPreset.token)
                    .ObserveOnCurrentDispatcher()
                    .Subscribe(presetTok => {
                        ReloadPresets();
                    }, err => {
                        SetErrorMessage(err.Message);
                        dbg.Error(err);
                    });
            } catch (Exception err) {
                dbg.Error(err);
                //SetErrorMessage(err.Message);
            }
        }
		public void ButtonContentApply() {
			apply((PtzView.Model)model);
		}

        Timer errorTmr = new Timer(5000);

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

        void errorTmr_Elapsed(object sender, ElapsedEventArgs e) {
            dispatch.BeginInvoke(() => {
                ErrorMessage = "";
            }); 
        }

        public void InitCommands() {
            //Zoom
            OnMouseUpZoomContinuesMinus = new DelegateCommand(() => {
                MouseUpZoomContinuesMinus();
            });
            OnMouseDownZoomContinuesMinus = new DelegateCommand(() => {
                MouseDownZoomContinuesMinus();
            });
            OnMouseUpZoomContinuesPlus = new DelegateCommand(() => {
                MouseUpZoomContinuesPlus();
            });
            OnMouseDownZoomContinuesPlus = new DelegateCommand(() => {
                MouseDownZoomContinusePlus();
            });

            //Continues Move
            OnMouseUpContinuesDown = new DelegateCommand(() => {
                MouseUpContinuesDown();
            });
            OnMouseDownContinuesDown = new DelegateCommand(() => {
                MouseDownContinuesDown();
            });
            OnMouseUpContinuesLeft = new DelegateCommand(() => {
                MouseUpContinuesLeft();
            });
            OnMouseDownContinuesLeft = new DelegateCommand(() => {
                MouseDownContinuesLeft();
            });
            OnMouseUpContinuesRight = new DelegateCommand(() => {
                MouseUpContinuesRight();
            });
            OnMouseDownContinuesRight = new DelegateCommand(() => {
                MouseDownContinuesRight();
            });
            OnMouseUpContinuesUp = new DelegateCommand(() => {
                MouseUpContinuesUp();
            });
            OnMouseDownContinuesUp = new DelegateCommand(() => {
                MouseDownContinuesUp();
            });

            //Other
			//OnButtonGoHome = new DelegateCommand(() => {
			//    ButtonGoHome();
			//});
            OnButtonRelLeft = new DelegateCommand(() => {
                ButtonRelLeft();
            });
            OnButtonRelRight = new DelegateCommand(() => {
                ButtonRelRight();
            });
            OnButtonRelTop = new DelegateCommand(() => {
                ButtonRelTop();
            });
            OnButtonRelDown = new DelegateCommand(() => {
                ButtonRelDown();
            });
            OnButtonSetHome = new DelegateCommand(() => {
                ButtonSetHome();
            });
            OnButtonSetPreset = new DelegateCommand(() => {
                ButtonSetPreset();
            });
            OnButtonZoomMinus = new DelegateCommand(() => {
                ButtonZoomMinus();
            });
            OnButtonZoomPlus = new DelegateCommand(() => {
                ButtonZoomPlus();
            });
            OnPresetDelete = new DelegateCommand(() => {
                PresetDelete();
            });
            OnPresetGoTo = new DelegateCommand(() => {
                PresetGoTo();
            });
            OnContentApply = new DelegateCommand(() => {
                //ButtonContentApply();
                apply((PtzView.Model)model);
            });
        }
        #region buttons

        #region ContinuesZoom
        public ICommand OnMouseUpZoomContinuesMinus {
            get { return (ICommand)GetValue(OnMouseUpZoomContinuesMinusProperty); }
            set { SetValue(OnMouseUpZoomContinuesMinusProperty, value); }
        }
        public static readonly DependencyProperty OnMouseUpZoomContinuesMinusProperty =
            DependencyProperty.Register("OnMouseUpZoomContinuesMinus", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseDownZoomContinuesMinus {
            get { return (ICommand)GetValue(OnMouseDownZoomContinuesMinusProperty); }
            set { SetValue(OnMouseDownZoomContinuesMinusProperty, value); }
        }
        public static readonly DependencyProperty OnMouseDownZoomContinuesMinusProperty =
            DependencyProperty.Register("OnMouseDownZoomContinuesMinus", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseUpZoomContinuesPlus {
            get { return (ICommand)GetValue(OnMouseUpZoomContinuesPlusProperty); }
            set { SetValue(OnMouseUpZoomContinuesPlusProperty, value); }
        }
        public static readonly DependencyProperty OnMouseUpZoomContinuesPlusProperty =
            DependencyProperty.Register("OnMouseUpZoomContinuesPlus", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseDownZoomContinuesPlus {
            get { return (ICommand)GetValue(OnMouseDownZoomContinuesPlusProperty); }
            set { SetValue(OnMouseDownZoomContinuesPlusProperty, value); }
        }
        public static readonly DependencyProperty OnMouseDownZoomContinuesPlusProperty =
            DependencyProperty.Register("OnMouseDownZoomContinuesPlus", typeof(ICommand), typeof(PTZViewModel));
        #endregion ContinuesZoom

        #region ContinuesMoove
        public ICommand OnMouseUpContinuesDown {
            get { return (ICommand)GetValue(OnMouseUpContinuesDownProperty); }
            set { SetValue(OnMouseUpContinuesDownProperty, value); }
        }
        public static readonly DependencyProperty OnMouseUpContinuesDownProperty =
            DependencyProperty.Register("OnMouseUpContinuesDown", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseDownContinuesDown {
            get { return (ICommand)GetValue(OnMouseDownContinuesDownProperty); }
            set { SetValue(OnMouseDownContinuesDownProperty, value); }
        }
        public static readonly DependencyProperty OnMouseDownContinuesDownProperty =
            DependencyProperty.Register("OnMouseDownContinuesDown", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseUpContinuesLeft {
            get { return (ICommand)GetValue(OnMouseUpContinuesLeftProperty); }
            set { SetValue(OnMouseUpContinuesLeftProperty, value); }
        }
        public static readonly DependencyProperty OnMouseUpContinuesLeftProperty =
            DependencyProperty.Register("OnMouseUpContinuesLeft", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseDownContinuesLeft {
            get { return (ICommand)GetValue(OnMouseDownContinuesLeftProperty); }
            set { SetValue(OnMouseDownContinuesLeftProperty, value); }
        }
        public static readonly DependencyProperty OnMouseDownContinuesLeftProperty =
            DependencyProperty.Register("OnMouseDownContinuesLeft", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseUpContinuesRight {
            get { return (ICommand)GetValue(OnMouseUpContinuesRightProperty); }
            set { SetValue(OnMouseUpContinuesRightProperty, value); }
        }
        public static readonly DependencyProperty OnMouseUpContinuesRightProperty =
            DependencyProperty.Register("OnMouseUpContinuesRight", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseDownContinuesRight {
            get { return (ICommand)GetValue(OnMouseDownContinuesRightProperty); }
            set { SetValue(OnMouseDownContinuesRightProperty, value); }
        }
        public static readonly DependencyProperty OnMouseDownContinuesRightProperty =
            DependencyProperty.Register("OnMouseDownContinuesRight", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseUpContinuesUp {
            get { return (ICommand)GetValue(OnMouseUpContinuesUpProperty); }
            set { SetValue(OnMouseUpContinuesUpProperty, value); }
        }
        public static readonly DependencyProperty OnMouseUpContinuesUpProperty =
            DependencyProperty.Register("OnMouseUpContinuesUp", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnMouseDownContinuesUp {
            get { return (ICommand)GetValue(OnMouseDownContinuesUpProperty); }
            set { SetValue(OnMouseDownContinuesUpProperty, value); }
        }
        public static readonly DependencyProperty OnMouseDownContinuesUpProperty =
            DependencyProperty.Register("OnMouseDownContinuesUp", typeof(ICommand), typeof(PTZViewModel));
        #endregion ContinuesMoove


        public ICommand OnContentApply {
            get { return (ICommand)GetValue(OnContentApplyProperty); }
            set { SetValue(OnContentApplyProperty, value); }
        }
        public static readonly DependencyProperty OnContentApplyProperty =
            DependencyProperty.Register("OnContentApply", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnButtonSetPreset {
            get { return (ICommand)GetValue(OnButtonSetPresetProperty); }
            set { SetValue(OnButtonSetPresetProperty, value); }
        }
        public static readonly DependencyProperty OnButtonSetPresetProperty =
            DependencyProperty.Register("OnButtonSetPreset", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnButtonZoomMinus {
            get { return (ICommand)GetValue(OnButtonZoomMinusProperty); }
            set { SetValue(OnButtonZoomMinusProperty, value); }
        }
        public static readonly DependencyProperty OnButtonZoomMinusProperty =
            DependencyProperty.Register("OnButtonZoomMinus", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnButtonZoomPlus {
            get { return (ICommand)GetValue(OnButtonZoomPlusProperty); }
            set { SetValue(OnButtonZoomPlusProperty, value); }
        }
        public static readonly DependencyProperty OnButtonZoomPlusProperty =
            DependencyProperty.Register("OnButtonZoomPlus", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnButtonSetHome {
            get { return (ICommand)GetValue(OnButtonSetHomeProperty); }
            set { SetValue(OnButtonSetHomeProperty, value); }
        }
        public static readonly DependencyProperty OnButtonSetHomeProperty =
            DependencyProperty.Register("OnButtonSetHome", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnButtonRelRight {
            get { return (ICommand)GetValue(OnButtonRelRightProperty); }
            set { SetValue(OnButtonRelRightProperty, value); }
        }
        public static readonly DependencyProperty OnButtonRelRightProperty =
            DependencyProperty.Register("OnButtonRelRight", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnButtonRelLeft {
            get { return (ICommand)GetValue(OnButtonRelLeftProperty); }
            set { SetValue(OnButtonRelLeftProperty, value); }
        }
        public static readonly DependencyProperty OnButtonRelLeftProperty =
            DependencyProperty.Register("OnButtonRelLeft", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnButtonRelDown {
            get { return (ICommand)GetValue(OnButtonRelDownProperty); }
            set { SetValue(OnButtonRelDownProperty, value); }
        }
        public static readonly DependencyProperty OnButtonRelDownProperty =
            DependencyProperty.Register("OnButtonRelDown", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnButtonRelTop {
            get { return (ICommand)GetValue(OnButtonRelTopProperty); }
            set { SetValue(OnButtonRelTopProperty, value); }
        }
        public static readonly DependencyProperty OnButtonRelTopProperty =
            DependencyProperty.Register("OnButtonRelTop", typeof(ICommand), typeof(PTZViewModel));

		//public ICommand OnButtonGoHome {
		//    get { return (ICommand)GetValue(OnButtonGoHomeProperty); }
		//    set { SetValue(OnButtonGoHomeProperty, value); }
		//}
		//public static readonly DependencyProperty OnButtonGoHomeProperty =
		//    DependencyProperty.Register("OnButtonGoHome", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnPresetDelete {
            get { return (ICommand)GetValue(OnPresetDeleteProperty); }
            set { SetValue(OnPresetDeleteProperty, value); }
        }
        public static readonly DependencyProperty OnPresetDeleteProperty =
            DependencyProperty.Register("OnPresetDelete", typeof(ICommand), typeof(PTZViewModel));

        public ICommand OnPresetGoTo {
            get { return (ICommand)GetValue(OnPresetGoToProperty); }
            set { SetValue(OnPresetGoToProperty, value); }
        }
        public static readonly DependencyProperty OnPresetGoToProperty =
            DependencyProperty.Register("OnPresetGoTo", typeof(ICommand), typeof(PTZViewModel));
        #endregion buttons

        #region values

        public string ValueRelMove {
            get { return (string)GetValue(ValueRelMoveProperty); }
            set { SetValue(ValueRelMoveProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValueRelMove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueRelMoveProperty =
            DependencyProperty.Register("ValueRelMove", typeof(string), typeof(PTZViewModel), new UIPropertyMetadata("0"));

        
        public string ValueZoom {
            get { return (string)GetValue(ValueZoomProperty); }
            set { SetValue(ValueZoomProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValueZoom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueZoomProperty =
            DependencyProperty.Register("ValueZoom", typeof(string), typeof(PTZViewModel));

        public int MaxZoomSpeed {
            get { return (int)GetValue(MaxZoomSpeedProperty); }
            set { SetValue(MaxZoomSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MaxZoomSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxZoomSpeedProperty =
            DependencyProperty.Register("MaxZoomSpeed", typeof(int), typeof(PTZViewModel));

        public int MinZoomSpeed {
            get { return (int)GetValue(MinZoomSpeedProperty); }
            set { SetValue(MinZoomSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MinZoomSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinZoomSpeedProperty =
            DependencyProperty.Register("MinZoomSpeed", typeof(int), typeof(PTZViewModel));

        public int ValueZoomSpeed {
            get { return (int)GetValue(ValueZoomSpeedProperty); }
            set { SetValue(ValueZoomSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValueZoomSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueZoomSpeedProperty =
            DependencyProperty.Register("ValueZoomSpeed", typeof(int), typeof(PTZViewModel));

        public int MinTiltSpeed {
            get { return (int)GetValue(MinTiltSpeedProperty); }
            set { SetValue(MinTiltSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MinTiltSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinTiltSpeedProperty =
            DependencyProperty.Register("MinTiltSpeed", typeof(int), typeof(PTZViewModel));

        public int MaxTiltSpeed {
            get { return (int)GetValue(MaxTiltSpeedProperty); }
            set { SetValue(MaxTiltSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MaxTiltSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxTiltSpeedProperty =
            DependencyProperty.Register("MaxTiltSpeed", typeof(int), typeof(PTZViewModel));

        public int ValueTiltSpeed {
            get { return (int)GetValue(ValueTiltSpeedProperty); }
            set { SetValue(ValueTiltSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValueTiltSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueTiltSpeedProperty =
            DependencyProperty.Register("ValueTiltSpeed", typeof(int), typeof(PTZViewModel));

        public int MinPanSpeed {
            get { return (int)GetValue(MinPanSpeedProperty); }
            set { SetValue(MinPanSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MinPanSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinPanSpeedProperty =
            DependencyProperty.Register("MinPanSpeed", typeof(int), typeof(PTZViewModel));

        public int MaxPanSpeed {
            get { return (int)GetValue(MaxPanSpeedProperty); }
            set { SetValue(MaxPanSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MaxPanSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxPanSpeedProperty =
            DependencyProperty.Register("MaxPanSpeed", typeof(int), typeof(PTZViewModel));

        public int ValuePanSpeed {
            get { return (int)GetValue(ValuePanSpeedProperty); }
            set { SetValue(ValuePanSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValuePanSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValuePanSpeedProperty =
            DependencyProperty.Register("ValuePanSpeed", typeof(int), typeof(PTZViewModel));

        #endregion values

        public string PTZErrorMessage {
            get { return (string)GetValue(PTZErrorMessageProperty); }
            set { SetValue(PTZErrorMessageProperty, value); }
        }
        public static readonly DependencyProperty PTZErrorMessageProperty =
            DependencyProperty.Register("PTZErrorMessage", typeof(string), typeof(PTZViewModel), new UIPropertyMetadata(""));

        public string ContentZoom {
            get { return (string)GetValue(ContentZoomProperty); }
            set { SetValue(ContentZoomProperty, value); }
        }
        public static readonly DependencyProperty ContentZoomProperty =
            DependencyProperty.Register("ContentZoom", typeof(string), typeof(PTZViewModel), new UIPropertyMetadata(""));
        
        public string ContentRelativeMove {
            get { return (string)GetValue(ContentRelativeMoveProperty); }
            set { SetValue(ContentRelativeMoveProperty, value); }
        }
        public static readonly DependencyProperty ContentRelativeMoveProperty =
            DependencyProperty.Register("ContentRelativeMove", typeof(string), typeof(PTZViewModel), new UIPropertyMetadata(""));

        public string PresetName {
            get { return (string)GetValue(PresetNameProperty); }
            set { SetValue(PresetNameProperty, value); }
        }
        public static readonly DependencyProperty PresetNameProperty =
            DependencyProperty.Register("PresetName", typeof(string), typeof(PTZViewModel), new PropertyMetadata("", (obj, evarg)=>{}));

        public string InfoString {
            get { return (string)GetValue(InfoStringProperty); }
            set { SetValue(InfoStringProperty, value); }
        }
        public static readonly DependencyProperty InfoStringProperty =
            DependencyProperty.Register("InfoString", typeof(string), typeof(PTZViewModel));

        public Visibility InfoVisible {
            get { return (Visibility)GetValue(InfoVisibleProperty); }
            set { SetValue(InfoVisibleProperty, value); }
        }
        public static readonly DependencyProperty InfoVisibleProperty =
            DependencyProperty.Register("InfoVisible", typeof(Visibility), typeof(PTZViewModel), new UIPropertyMetadata(Visibility.Hidden));

        public bool IsAbsolute {
            get { return (bool)GetValue(IsAbsoluteProperty); }
            set { SetValue(IsAbsoluteProperty, value); }
        }
        public static readonly DependencyProperty IsAbsoluteProperty =
            DependencyProperty.Register("IsAbsolute", typeof(bool), typeof(PTZViewModel), new UIPropertyMetadata(false, (obj, evarg) => {
                var control = (PTZViewModel)obj;
                //if ((bool)evarg.OldValue == true) {
                //    //control.IsAbsolute = true;
                //    return;
                //}
                control.IsRelative = !((bool)evarg.NewValue);

                control.InitAbsoluteRelative();
            }));

        public bool IsRelative {
            get { return (bool)GetValue(IsRelativeProperty); }
            set { SetValue(IsRelativeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsRelative.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRelativeProperty =
            DependencyProperty.Register("IsRelative", typeof(bool), typeof(PTZViewModel), new UIPropertyMetadata(true, (obj, evarg) => {
                var control = (PTZViewModel)obj;
                //if ((bool)evarg.OldValue == true) {
                //    //control.IsRelative = true;
                //    return;
                //}
                control.IsAbsolute = !((bool)evarg.NewValue);
            }));

        public PTZNode SelectedNode {
            get { return (PTZNode)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SelectedNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedNodeProperty =
            DependencyProperty.Register("SelectedNode", typeof(PTZNode), typeof(PTZViewModel), new PropertyMetadata(null, (obj, evarg) => {
                var viewModel = (PTZViewModel)obj;
                if (evarg.NewValue == null) {
                    viewModel.InfoVisible = Visibility.Visible;
                    viewModel.InfoString = "Node is not selectevd. Please configure node.";
                } else {
                    viewModel.InfoVisible = Visibility.Hidden;
                }
            }));

        public ObservableCollection<PTZNode> Nodes {
            get { return (ObservableCollection<PTZNode>)GetValue(NodesProperty); }
            set { SetValue(NodesProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Nodes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodesProperty =
            DependencyProperty.Register("Nodes", typeof(ObservableCollection<PTZNode>), typeof(PTZViewModel));

        public PTZPreset SelectedPreset {
            get { return (PTZPreset)GetValue(SelectedPresetProperty); }
            set { SetValue(SelectedPresetProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SelectedPreset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPresetProperty =
            DependencyProperty.Register("SelectedPreset", typeof(PTZPreset), typeof(PTZViewModel));

        public ObservableCollection<PTZPreset> Presets {
            get { return (ObservableCollection<PTZPreset>)GetValue(PresetsProperty); }
            set { SetValue(PresetsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Presets.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PresetsProperty =
            DependencyProperty.Register("Presets", typeof(ObservableCollection<PTZPreset>), typeof(PTZViewModel));

        //public IVideoInfo VideoInfo {
        //    get { return (IVideoInfo)GetValue(VideoInfoProperty); }
        //    set { SetValue(VideoInfoProperty, value); }
        //}
        //// Using a DependencyProperty as the backing store for VideoInfo.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty VideoInfoProperty =
        //    DependencyProperty.Register("VideoInfo", typeof(IVideoInfo), typeof(PTZViewModel));
        
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
                position.panTilt = new Vector2D();
                position.panTilt.x = -val;
                PTZSpeed speed = new PTZSpeed();
                speed.panTilt = new Vector2D();
                speed.panTilt.x = 1;
                return session.AbsoluteMove(profToken, position, speed);
            }

            public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveRight(int val) {
                PTZVector position = new PTZVector();
                position.panTilt = new Vector2D();
                position.panTilt.x = val;
                PTZSpeed speed = new PTZSpeed();
                speed.panTilt = new Vector2D();
                speed.panTilt.x = 1;
                return session.AbsoluteMove(profToken, position, speed);
            }

            public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveUp(int val) {
                PTZVector position = new PTZVector();
                position.panTilt = new Vector2D();
                position.panTilt.y = -val;
                PTZSpeed speed = new PTZSpeed();
                speed.panTilt = new Vector2D();
                speed.panTilt.y = 1;
                return session.AbsoluteMove(profToken, position, speed);
            }

            public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveDown(int val) {
                PTZVector position = new PTZVector();
                position.panTilt = new Vector2D();
                position.panTilt.y = val;
                PTZSpeed speed = new PTZSpeed();
                speed.panTilt = new Vector2D();
                speed.panTilt.y= 1;
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
                position.panTilt = new Vector2D();
                position.panTilt.x = -val;
                PTZSpeed speed = new PTZSpeed();
                speed.panTilt = new Vector2D();
                speed.panTilt.x = 1;
                return session.RelativeMove(profToken, position, speed);
            }

            public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveRight(int val) {
                PTZVector position = new PTZVector();
                position.panTilt = new Vector2D();
                position.panTilt.x = val;
                PTZSpeed speed = new PTZSpeed();
                speed.panTilt = new Vector2D();
                speed.panTilt.x = 1;
                return session.RelativeMove(profToken, position, speed);
            }

            public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveUp(int val) {
                PTZVector position = new PTZVector();
                position.panTilt = new Vector2D();
                position.panTilt.y = -val;
                PTZSpeed speed = new PTZSpeed();
                speed.panTilt = new Vector2D();
                speed.panTilt.y = 1;
                return session.RelativeMove(profToken, position, speed);
            }

            public FSharpAsync<Microsoft.FSharp.Core.Unit> MoveDown(int val) {
                PTZVector position = new PTZVector();
                position.panTilt = new Vector2D();
                position.panTilt.y = val;
                PTZSpeed speed = new PTZSpeed();
                speed.panTilt = new Vector2D();
                speed.panTilt.y= 1;
                return session.RelativeMove(profToken, position, speed);
            }
        }

		public string ErrorMessage {
			get { return (string)GetValue(ErrorMessageProperty); }
			set { SetValue(ErrorMessageProperty, value); }
		}
		public static readonly DependencyProperty ErrorMessageProperty =
			DependencyProperty.Register("ErrorMessage", typeof(string), typeof(PTZViewModel), new PropertyMetadata((obj, evargs) => {
				int o = 0;
			}));

		public event PropertyChangedEventHandler PropertyChanged;

		public void Dispose() {
			subscription.Dispose();
			subscription = new CompositeDisposable();
		}
	}
}
