using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Microsoft.Practices.Prism.Commands;

using onvif.services;

using utils;
using odm.ui.views;

namespace odm.ui.controls {
    public class ControlPTZ : ContentControl {
        public ControlPTZ() {
            MouseUp += new MouseButtonEventHandler(ControlPTZ_MouseUp);

            //Presets = new ObservableCollection<PTZPreset>();
            //Nodes = new ObservableCollection<PTZNode>();
            CreateBindings();
        }

        void ControlPTZ_MouseUp(object sender, MouseButtonEventArgs e) {
            
        }
        
        public PropertyPTZStrings Strings { get { return PropertyPTZStrings.instance; } }

        void RefreshRealativeCaption() {
            this.CreateBinding(ContentRelativeMoveProperty, Strings, x => {
                if (IsRelative)
                    return x.relativemove;
                else
                    return x.absolute;
            });
        }

        void CreateBindings() {
            RefreshRealativeCaption();
            this.CreateBinding(ContentQuickSettingsProperty, Strings, x => x.quicksets);
            this.CreateBinding(ContentSettingsProperty, Strings, x => x.settings);
            this.CreateBinding(ContentApplyProperty, Strings, x => x.apply);
            this.CreateBinding(ContentDeleteProperty, Strings, x => x.delete);
            this.CreateBinding(ContentGoToProperty, Strings, x => x.go_to);
            this.CreateBinding(ContentPanMaxProperty, Strings, x => x.panmax);
            this.CreateBinding(ContentPanMinProperty, Strings, x => x.panmin);
            this.CreateBinding(ContentPanSpeedProperty, Strings, x => x.panspeed);
            this.CreateBinding(ContentPresetsProperty, Strings, x => x.presets);
            this.CreateBinding(ContentSelectProperty, Strings, x => x.select);
            this.CreateBinding(ContentSelectNodeProperty, Strings, x => x.selectnode);
            this.CreateBinding(ContentSetHomeProperty, Strings, x => x.sethome);
            this.CreateBinding(ContentSetLimitsProperty, Strings, x => x.setlimits);
            this.CreateBinding(ContentSetPresetProperty, Strings, x => x.setpreset);
            this.CreateBinding(ContentTiltMaxProperty, Strings, x => x.tiltmax);
            this.CreateBinding(ContentTiltMinProperty, Strings, x => x.tiltmin);
            this.CreateBinding(ContentTiltSpeedProperty, Strings, x => x.tiltspeed);
            this.CreateBinding(ContentZoomProperty, Strings, x => x.zoom);
            this.CreateBinding(ContentZoomMaxProperty, Strings, x => x.zoommax);
            this.CreateBinding(ContentZoomMinProperty, Strings, x => x.zoommin);
            this.CreateBinding(ContentZoomSpeedProperty, Strings, x => x.zoomspeed);
        }

        #region Strings
        public string ContentZoomSpeed {
            get { return (string)GetValue(ContentZoomSpeedProperty); }
            set { SetValue(ContentZoomSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentZoomSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentZoomSpeedProperty =
            DependencyProperty.Register("ContentZoomSpeed", typeof(string), typeof(ControlPTZ));

        public string ContentZoomMin {
            get { return (string)GetValue(ContentZoomMinProperty); }
            set { SetValue(ContentZoomMinProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentZoomMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentZoomMinProperty =
            DependencyProperty.Register("ContentZoomMin", typeof(string), typeof(ControlPTZ));

        public string ContentZoomMax {
            get { return (string)GetValue(ContentZoomMaxProperty); }
            set { SetValue(ContentZoomMaxProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentZoomMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentZoomMaxProperty =
            DependencyProperty.Register("ContentZoomMax", typeof(string), typeof(ControlPTZ));

        public string ContentZoom {
            get { return ( string)GetValue(ContentZoomProperty); }
            set { SetValue(ContentZoomProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentZoom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentZoomProperty =
            DependencyProperty.Register("ContentZoom", typeof( string), typeof(ControlPTZ));

        public string ContentTiltSpeed {
            get { return (string)GetValue(ContentTiltSpeedProperty); }
            set { SetValue(ContentTiltSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentTiltSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentTiltSpeedProperty =
            DependencyProperty.Register("ContentTiltSpeed", typeof(string), typeof(ControlPTZ));

        public string ContentTiltMin {
            get { return (string)GetValue(ContentTiltMinProperty); }
            set { SetValue(ContentTiltMinProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentTiltMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentTiltMinProperty =
            DependencyProperty.Register("ContentTiltMin", typeof(string), typeof(ControlPTZ));

        public string ContentTiltMax {
            get { return (string)GetValue(ContentTiltMaxProperty); }
            set { SetValue(ContentTiltMaxProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentTiltMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentTiltMaxProperty =
            DependencyProperty.Register("ContentTiltMax", typeof(string), typeof(ControlPTZ));

        public string ContentSetPreset {
            get { return (string)GetValue(ContentSetPresetProperty); }
            set { SetValue(ContentSetPresetProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentSetPreset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentSetPresetProperty =
            DependencyProperty.Register("ContentSetPreset", typeof(string), typeof(ControlPTZ));

        public string ContentSetLimits {
            get { return (string)GetValue(ContentSetLimitsProperty); }
            set { SetValue(ContentSetLimitsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentSetLimits.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentSetLimitsProperty =
            DependencyProperty.Register("ContentSetLimits", typeof(string), typeof(ControlPTZ));

        public string ContentSetHome {
            get { return (string)GetValue(ContentSetHomeProperty); }
            set { SetValue(ContentSetHomeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentSetHome.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentSetHomeProperty =
            DependencyProperty.Register("ContentSetHome", typeof(string), typeof(ControlPTZ));

        public string ContentSelectNode {
            get { return (string)GetValue(ContentSelectNodeProperty); }
            set { SetValue(ContentSelectNodeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentSelectNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentSelectNodeProperty =
            DependencyProperty.Register("ContentSelectNode", typeof(string), typeof(ControlPTZ));

        public string ContentSelect {
            get { return (string)GetValue(ContentSelectProperty); }
            set { SetValue(ContentSelectProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentSelect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentSelectProperty =
            DependencyProperty.Register("ContentSelect", typeof(string), typeof(ControlPTZ));

        public string ContentRelativeMove {
            get { return (string)GetValue(ContentRelativeMoveProperty); }
            set { SetValue(ContentRelativeMoveProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentRelativeMove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentRelativeMoveProperty =
            DependencyProperty.Register("ContentRelativeMove", typeof(string), typeof(ControlPTZ));

        public string ContentPresets {
            get { return (string)GetValue(ContentPresetsProperty); }
            set { SetValue(ContentPresetsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentPresets.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentPresetsProperty =
            DependencyProperty.Register("ContentPresets", typeof(string), typeof(ControlPTZ));

        public string ContentPanSpeed {
            get { return (string)GetValue(ContentPanSpeedProperty); }
            set { SetValue(ContentPanSpeedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentPanSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentPanSpeedProperty =
            DependencyProperty.Register("ContentPanSpeed", typeof(string), typeof(ControlPTZ));

        public string ContentPanMin {
            get { return ( string)GetValue(ContentPanMinProperty); }
            set { SetValue(ContentPanMinProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentPanMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentPanMinProperty =
            DependencyProperty.Register("ContentPanMin", typeof( string), typeof(ControlPTZ));

        public string ContentPanMax {
            get { return (string)GetValue(ContentPanMaxProperty); }
            set { SetValue(ContentPanMaxProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentPanMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentPanMaxProperty =
            DependencyProperty.Register("ContentPanMax", typeof(string), typeof(ControlPTZ));

        public string ContentGoTo {
            get { return (string)GetValue(ContentGoToProperty); }
            set { SetValue(ContentGoToProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentGoTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentGoToProperty =
            DependencyProperty.Register("ContentGoTo", typeof(string), typeof(ControlPTZ));

        public string ContentDelete {
            get { return (string)GetValue(ContentDeleteProperty); }
            set { SetValue(ContentDeleteProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentDelete.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentDeleteProperty =
            DependencyProperty.Register("ContentDelete", typeof(string), typeof(ControlPTZ));

        public string ContentApply {
            get { return (string)GetValue(ContentApplyProperty); }
            set { SetValue(ContentApplyProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentApply.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentApplyProperty =
            DependencyProperty.Register("ContentApply", typeof(string), typeof(ControlPTZ));

        public string ContentSettings {
            get { return (string)GetValue(ContentSettingsProperty); }
            set { SetValue(ContentSettingsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentSettingsProperty =
            DependencyProperty.Register("ContentSettings", typeof(string), typeof(ControlPTZ));

        public string ContentQuickSettings {
            get { return (string)GetValue(ContentQuickSettingsProperty); }
            set { SetValue(ContentQuickSettingsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentQuickSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentQuickSettingsProperty =
            DependencyProperty.Register("ContentQuickSettings", typeof(string), typeof(ControlPTZ));
        #endregion
        
        void ButtonGoHome() { }
        void ButtonRelLeft() { }
        void ButtonRelRight() { }
        void ButtonRelTop() { }
        void ButtonRelDown() { }
        void ButtonSetHome() { }
        void ButtonSetPreset() { }
        void ButtonZoomMinus() { }
        void ButtonZoomPlus() { }
        void PresetGoTo() { }
        void PresetDelete() { }
        void ButtonContentApply(){}
        
        void MouseUpZoomAbsMinus(){}
        void MouseDownZoomAbsMinus(){}
        void MouseUpZoomAbsPlus(){}
        void MouseDownZoomAbsPlus() { }

        void MouseUpAbsDown(){}
        void MouseDownAbsDown(){}
        void MouseUpAbsLeft(){}
        void MouseDownAbsLeft(){}
        void MouseUpAbsRight(){}
        void MouseDownAbsRight(){}
        void MouseUpAbsUp(){}
        void MouseDownAbsUp() { }

        #region buttons

        #region ContinuesZoom

		public DelegateCommand btnUpZoomMinus {
			get { return (DelegateCommand)GetValue(btnUpZoomMinusProperty); }
			set { SetValue(btnUpZoomMinusProperty, value); }
		}
		public static readonly DependencyProperty btnUpZoomMinusProperty = DependencyProperty.Register("btnUpZoomMinus", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseDownZoomAbsMinus {
			get { return (DelegateCommand)GetValue(btnMouseDownZoomAbsMinusProperty); }
			set { SetValue(btnMouseDownZoomAbsMinusProperty, value); }
		}
		public static readonly DependencyProperty btnMouseDownZoomAbsMinusProperty = DependencyProperty.Register("btnMouseDownZoomAbsMinus", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseUpZoomAbsPlus {
			get { return (DelegateCommand)GetValue(btnMouseUpZoomAbsPlusProperty); }
			set { SetValue(btnMouseUpZoomAbsPlusProperty, value); }
		}
		public static readonly DependencyProperty btnMouseUpZoomAbsPlusProperty = DependencyProperty.Register("btnMouseUpZoomAbsPlus", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseDownZoomAbsPlus {
			get { return (DelegateCommand)GetValue(btnMouseDownZoomAbsPlusProperty); }
			set { SetValue(btnMouseDownZoomAbsPlusProperty, value); }
		}
		public static readonly DependencyProperty btnMouseDownZoomAbsPlusProperty =
			DependencyProperty.Register("btnMouseDownZoomAbsPlus", typeof(DelegateCommand), typeof(ControlPTZ));

        #endregion ContinuesZoom

        #region ContinuesMoove

		public DelegateCommand btnMouseUpAbsDown {
			get { return (DelegateCommand)GetValue(btnMouseUpAbsDownProperty); }
			set { SetValue(btnMouseUpAbsDownProperty, value); }
		}
		public static readonly DependencyProperty btnMouseUpAbsDownProperty = DependencyProperty.Register("btnMouseUpAbsDown", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseDownAbsDown {
			get { return (DelegateCommand)GetValue(btnMouseDownAbsDownProperty); }
			set { SetValue(btnMouseDownAbsDownProperty, value); }
		}
		public static readonly DependencyProperty btnMouseDownAbsDownProperty = DependencyProperty.Register("btnMouseDownAbsDown", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseUpAbsLeft {
			get { return (DelegateCommand)GetValue(btnMouseUpAbsLeftProperty); }
			set { SetValue(btnMouseUpAbsLeftProperty, value); }
		}
		public static readonly DependencyProperty btnMouseUpAbsLeftProperty = DependencyProperty.Register("btnMouseUpAbsLeft", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseDownAbsLeft {
			get { return (DelegateCommand)GetValue(btnMouseDownAbsLeftProperty); }
			set { SetValue(btnMouseDownAbsLeftProperty, value); }
		}
		public static readonly DependencyProperty btnMouseDownAbsLeftProperty = DependencyProperty.Register("btnMouseDownAbsLeft", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseUpAbsRight {
			get { return (DelegateCommand)GetValue(btnMouseUpAbsRightProperty); }
			set { SetValue(btnMouseUpAbsRightProperty, value); }
		}
		public static readonly DependencyProperty btnMouseUpAbsRightProperty = DependencyProperty.Register("btnMouseUpAbsRight", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseDownAbsRight {
			get { return (DelegateCommand)GetValue(btnMouseDownAbsRightProperty); }
			set { SetValue(btnMouseDownAbsRightProperty, value); }
		}
		public static readonly DependencyProperty btnMouseDownAbsRightProperty = DependencyProperty.Register("btnMouseDownAbsRight", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseUpAbsUp {
			get { return (DelegateCommand)GetValue(btnMouseUpAbsUpProperty); }
			set { SetValue(btnMouseUpAbsUpProperty, value); }
		}
		public static readonly DependencyProperty btnMouseUpAbsUpProperty = DependencyProperty.Register("btnMouseUpAbsUp", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnMouseDownAbsUp {
			get { return (DelegateCommand)GetValue(btnMouseDownAbsUpProperty); }
			set { SetValue(btnMouseDownAbsUpProperty, value); }
		}
		public static readonly DependencyProperty btnMouseDownAbsUpProperty = DependencyProperty.Register("btnMouseDownAbsUp", typeof(DelegateCommand), typeof(ControlPTZ));

        #endregion ContinuesMoove

		public DelegateCommand btnContentApply {
			get { return (DelegateCommand)GetValue(btnContentApplyProperty); }
			set { SetValue(btnContentApplyProperty, value); }
		}
		public static readonly DependencyProperty btnContentApplyProperty = DependencyProperty.Register("btnContentApply", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnButtonSetPreset {
			get { return (DelegateCommand)GetValue(btnButtonSetPresetProperty); }
			set { SetValue(btnButtonSetPresetProperty, value); }
		}
		public static readonly DependencyProperty btnButtonSetPresetProperty = DependencyProperty.Register("btnButtonSetPreset", typeof(DelegateCommand), typeof(ControlPTZ));
		
		public DelegateCommand btnButtonZoomMinus {
			get { return (DelegateCommand)GetValue(btnButtonZoomMinusProperty); }
			set { SetValue(btnButtonZoomMinusProperty, value); }
		}
		public static readonly DependencyProperty btnButtonZoomMinusProperty = DependencyProperty.Register("btnButtonZoomMinus", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnButtonZoomPlus {
			get { return (DelegateCommand)GetValue(btnButtonZoomPlusProperty); }
			set { SetValue(btnButtonZoomPlusProperty, value); }
		}
		public static readonly DependencyProperty btnButtonZoomPlusProperty = DependencyProperty.Register("btnButtonZoomPlus", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnButtonSetHome {
			get { return (DelegateCommand)GetValue(btnButtonSetHomeProperty); }
			set { SetValue(btnButtonSetHomeProperty, value); }
		}
		public static readonly DependencyProperty btnButtonSetHomeProperty = DependencyProperty.Register("btnButtonSetHome", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnButtonRelRight {
			get { return (DelegateCommand)GetValue(btnButtonRelRightProperty); }
			set { SetValue(btnButtonRelRightProperty, value); }
		}
		public static readonly DependencyProperty btnButtonRelRightProperty = DependencyProperty.Register("btnButtonRelRight", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnButtonRelLeft {
			get { return (DelegateCommand)GetValue(btnButtonRelLeftProperty); }
			set { SetValue(btnButtonRelLeftProperty, value); }
		}
		public static readonly DependencyProperty btnButtonRelLeftProperty = DependencyProperty.Register("btnButtonRelLeft", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnButtonRelDown {
			get { return (DelegateCommand)GetValue(btnButtonRelDownProperty); }
			set { SetValue(btnButtonRelDownProperty, value); }
		}
		public static readonly DependencyProperty btnButtonRelDownProperty = DependencyProperty.Register("btnButtonRelDown", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnButtonRelTop {
			get { return (DelegateCommand)GetValue(btnButtonRelTopProperty); }
			set { SetValue(btnButtonRelTopProperty, value); }
		}
		public static readonly DependencyProperty btnButtonRelTopProperty = DependencyProperty.Register("btnButtonRelTop", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnGoHome {
			get { return (DelegateCommand)GetValue(btnGoHomeProperty); }
			set { SetValue(btnGoHomeProperty, value); }
		}
		public static readonly DependencyProperty btnGoHomeProperty = DependencyProperty.Register("btnGoHome", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnPresetDelete {
			get { return (DelegateCommand)GetValue(btnPresetDeleteProperty); }
			set { SetValue(btnPresetDeleteProperty, value); }
		}
		public static readonly DependencyProperty btnPresetDeleteProperty = DependencyProperty.Register("btnPresetDelete", typeof(DelegateCommand), typeof(ControlPTZ));

		public DelegateCommand btnPresetGoTo {
			get { return (DelegateCommand)GetValue(btnPresetGoToProperty); }
			set { SetValue(btnPresetGoToProperty, value); }
		}
		public static readonly DependencyProperty btnPresetGoToProperty = DependencyProperty.Register("btnPresetGoTo", typeof(DelegateCommand), typeof(ControlPTZ));

        #endregion buttons

        #region captions
        public string ValueRelMove {
            get { return (string)GetValue(ValueRelMoveProperty); }
            set { SetValue(ValueRelMoveProperty, value); }
        } 
        public static readonly DependencyProperty ValueRelMoveProperty = DependencyProperty.Register("ValueRelMove", typeof(string), typeof(ControlPTZ));

        public string TextZoomMax {
            get { return (string)GetValue(TextZoomMaxProperty); }
            set { SetValue(TextZoomMaxProperty, value); }
        } 
        public static readonly DependencyProperty TextZoomMaxProperty = DependencyProperty.Register("TextZoomMax", typeof(string), typeof(ControlPTZ));

        public string TextZoomMin {
            get { return (string)GetValue(TextZoomMinProperty); }
            set { SetValue(TextZoomMinProperty, value); }
        } 
        public static readonly DependencyProperty TextZoomMinProperty = DependencyProperty.Register("TextZoomMin", typeof(string), typeof(ControlPTZ));

        public string TextPanMax {
            get { return (string)GetValue(TextPanMaxProperty); }
            set { SetValue(TextPanMaxProperty, value); }
        } 
        public static readonly DependencyProperty TextPanMaxProperty = DependencyProperty.Register("TextPanMax", typeof(string), typeof(ControlPTZ));

        public string TextPanMin {
            get { return (string)GetValue(TextPanMinProperty); }
            set { SetValue(TextPanMinProperty, value); }
        } 
        public static readonly DependencyProperty TextPanMinProperty = DependencyProperty.Register("TextPanMin", typeof(string), typeof(ControlPTZ));

        public string TextTiltMax {
            get { return (string)GetValue(TextTiltMaxProperty); }
            set { SetValue(TextTiltMaxProperty, value); }
        } 
        public static readonly DependencyProperty TextTiltMaxProperty = DependencyProperty.Register("TextTiltMax", typeof(string), typeof(ControlPTZ));

        public string TextTiltMin {
            get { return (string)GetValue(TextTiltMinProperty); }
            set { SetValue(TextTiltMinProperty, value); }
        }
        public static readonly DependencyProperty TextTiltMinProperty = DependencyProperty.Register("TextTiltMin", typeof(string), typeof(ControlPTZ));

        public string PresetName {
            get { return (string)GetValue(PresetNameProperty); }
            set { SetValue(PresetNameProperty, value); }
        }
        public static readonly DependencyProperty PresetNameProperty = DependencyProperty.Register("PresetName", typeof(string), typeof(ControlPTZ), new PropertyMetadata("def"));

        #endregion captions

        #region values

        public string ValueZoom {
            get { return (string)GetValue(ValueZoomProperty); }
            set { SetValue(ValueZoomProperty, value); }
        }
        public static readonly DependencyProperty ValueZoomProperty = DependencyProperty.Register("ValueZoom", typeof(string), typeof(ControlPTZ));

        public int MaxZoomSpeed {
            get { return (int)GetValue(MaxZoomSpeedProperty); }
            set { SetValue(MaxZoomSpeedProperty, value); }
        }
        public static readonly DependencyProperty MaxZoomSpeedProperty = DependencyProperty.Register("MaxZoomSpeed", typeof(int), typeof(ControlPTZ));

        public int MinZoomSpeed {
            get { return (int)GetValue(MinZoomSpeedProperty); }
            set { SetValue(MinZoomSpeedProperty, value); }
        }
        public static readonly DependencyProperty MinZoomSpeedProperty = DependencyProperty.Register("MinZoomSpeed", typeof(int), typeof(ControlPTZ));

        public int ValueZoomSpeed {
            get { return (int)GetValue(ValueZoomSpeedProperty); }
            set { SetValue(ValueZoomSpeedProperty, value); }
        }
        public static readonly DependencyProperty ValueZoomSpeedProperty = DependencyProperty.Register("ValueZoomSpeed", typeof(int), typeof(ControlPTZ));

        public int MinTiltSpeed {
            get { return (int)GetValue(MinTiltSpeedProperty); }
            set { SetValue(MinTiltSpeedProperty, value); }
        }
        public static readonly DependencyProperty MinTiltSpeedProperty = DependencyProperty.Register("MinTiltSpeed", typeof(int), typeof(ControlPTZ));

        public int MaxTiltSpeed {
            get { return (int)GetValue(MaxTiltSpeedProperty); }
            set { SetValue(MaxTiltSpeedProperty, value); }
        }
        public static readonly DependencyProperty MaxTiltSpeedProperty = DependencyProperty.Register("MaxTiltSpeed", typeof(int), typeof(ControlPTZ));

        public int ValueTiltSpeed {
            get { return (int)GetValue(ValueTiltSpeedProperty); }
            set { SetValue(ValueTiltSpeedProperty, value); }
        }
        public static readonly DependencyProperty ValueTiltSpeedProperty = DependencyProperty.Register("ValueTiltSpeed", typeof(int), typeof(ControlPTZ));

        public int MinPanSpeed {
            get { return (int)GetValue(MinPanSpeedProperty); }
            set { SetValue(MinPanSpeedProperty, value); }
        }
        public static readonly DependencyProperty MinPanSpeedProperty = DependencyProperty.Register("MinPanSpeed", typeof(int), typeof(ControlPTZ));

        public int MaxPanSpeed {
            get { return (int)GetValue(MaxPanSpeedProperty); }
            set { SetValue(MaxPanSpeedProperty, value); }
        }
        public static readonly DependencyProperty MaxPanSpeedProperty = DependencyProperty.Register("MaxPanSpeed", typeof(int), typeof(ControlPTZ));

        public int ValuePanSpeed {
            get { return (int)GetValue(ValuePanSpeedProperty); }
            set { SetValue(ValuePanSpeedProperty, value); }
        } 
        public static readonly DependencyProperty ValuePanSpeedProperty = DependencyProperty.Register("ValuePanSpeed", typeof(int), typeof(ControlPTZ));
        
        #endregion values

        public Visibility InfoVisible {
            get { return (Visibility)GetValue(InfoVisibleProperty); }
            set { SetValue(InfoVisibleProperty, value); }
        }
        public static readonly DependencyProperty InfoVisibleProperty = DependencyProperty.Register("InfoVisible", typeof(Visibility), typeof(ControlPTZ), new UIPropertyMetadata(Visibility.Hidden));

        public string InfoString {
            get { return (string)GetValue(InfoStringProperty); }
            set { SetValue(InfoStringProperty, value); }
        }
        public static readonly DependencyProperty InfoStringProperty = DependencyProperty.Register("InfoString", typeof(string), typeof(ControlPTZ));

        public bool IsAbsolute {
            get { return (bool)GetValue(IsAbsoluteProperty); }
            set { SetValue(IsAbsoluteProperty, value); }
        }
        public static readonly DependencyProperty IsAbsoluteProperty = DependencyProperty.Register("IsAbsolute", typeof(bool), typeof(ControlPTZ));

        public bool IsRelative {
            get { return (bool)GetValue(IsRelativeProperty); }
            set { SetValue(IsRelativeProperty, value); }
        }
        public static readonly DependencyProperty IsRelativeProperty = DependencyProperty.Register("IsRelative", typeof(bool), typeof(ControlPTZ));

        public PTZNode SelectedNode {
            get { return (PTZNode)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(PTZNode), typeof(ControlPTZ));

        public ObservableCollection<PTZNode> Nodes {
            get { return (ObservableCollection<PTZNode>)GetValue(NodesProperty); }
            set { SetValue(NodesProperty, value); }
        }
        public static readonly DependencyProperty NodesProperty = DependencyProperty.Register("Nodes", typeof(ObservableCollection<PTZNode>), typeof(ControlPTZ));

        public ObservableCollection<PTZPreset> Presets {
            get { return (ObservableCollection<PTZPreset>)GetValue(PresetsProperty); }
            set { SetValue(PresetsProperty, value); }
        }
        public static readonly DependencyProperty PresetsProperty = DependencyProperty.Register("Presets", typeof(ObservableCollection<PTZPreset>), typeof(ControlPTZ));

        public PTZPreset SelectedPreset {
            get { return (PTZPreset)GetValue(SelectedPresetProperty); }
            set { SetValue(SelectedPresetProperty, value); }
        }
        public static readonly DependencyProperty SelectedPresetProperty = DependencyProperty.Register("SelectedPreset", typeof(PTZPreset), typeof(ControlPTZ));

        
    }
}
