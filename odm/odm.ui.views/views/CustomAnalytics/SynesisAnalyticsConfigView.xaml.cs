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
using utils;
using onvif.services;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Xml;
using odm.ui.controls;

namespace odm.ui.views.CustomAnalytics {
    /// <summary>
    /// Interaction logic for SynesisAnalyticsConfigView.xaml
    /// </summary>
    public partial class SynesisAnalyticsConfigView : UserControl, IDisposable {
        public SynesisAnalyticsConfigView() {
            InitializeComponent();

            //tabTampering.CreateBinding(TamperingDetectorsView.isCameraObstructedEnabledProperty, tabObjectTracker, x=>
        }

        public string title = "Analytics module configuration";
        odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr;
        odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr;
        SynesisAnalyticsModel model = new SynesisAnalyticsModel();
        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }

        Size videoSourceSize;
        Size videoEncoderSize;

        public class SynesisAnalyticsModel:INotifyPropertyChanged {
            public SynesisAnalyticsModel (){
            }
            bool useObjectTracker = false;
            public bool UseObjectTracker {
                get {
                    return useObjectTracker;
                }
                set {
                    useObjectTracker = value;
                    NotifyPropertyChanged("UseObjectTracker");
                }
            }
            public const float ObjectAreaValueMin = 0.0f;
            public const float ObjectAreaValueMax = 100.0f;
            float maxObjectArea;
            public float MaxObjectArea{
                get { return maxObjectArea; }
                set {
                    maxObjectArea = value;
                    NotifyPropertyChanged("MaxObjectArea");
                }
            }
            float minObjectArea;
            public float MinObjectArea{
                get { return minObjectArea; }
                set {
                    minObjectArea = value;
                    NotifyPropertyChanged("MinObjectArea");
                }
            }

            public const float ObjectSpeedValueMin = 0.0f;
            public const float ObjectSpeedValueMax = 100.0f;
            float maxObjectSpeed;
            public float MaxObjectSpeed {
                get { return maxObjectSpeed; }
                set {
                    maxObjectSpeed = value;
                    NotifyPropertyChanged("MaxObjectSpeed");
                }
            }
            public const int DisplacementSensitivityValueMin = 0;
            public const int DisplacementSensitivityValueMax = 5;
            int displacementSensivity;
            public int DisplacementSensivity {
                get { return displacementSensivity; }
                set {
                    displacementSensivity = value;
                    NotifyPropertyChanged("DisplacementSensivity");
                }
            }
            public const int StabilizationTimeValueMin = 40;
            public const int StabilizationTimeValueMax = 10000;
            int stabilizationTime;
            public int StabilizationTime {
                get { return stabilizationTime; }
                set {
                    stabilizationTime = value;
                    NotifyPropertyChanged("StabilizationTime");
                }
            }
            bool useAntishaker;
            public bool UseAntishaker {
                get { return useAntishaker; }
                set {
                    useAntishaker = value;
                    NotifyPropertyChanged("UseAntishaker");
                }
            }
            bool cameraRedirected;
            public bool CameraRedirected {
                get { return cameraRedirected; }
                set {
                    cameraRedirected = value;
                    NotifyPropertyChanged("CameraRedirected");
                }
            }
            bool shiftOutputPicture;
            public bool ShiftOutputPicture {
                get { return shiftOutputPicture; }
                set {
                    shiftOutputPicture = value;
                    NotifyPropertyChanged("ShiftOutputPicture");
                }
            }
            bool cameraObstructed;
            public bool CameraObstructed {
                get { return cameraObstructed; }
                set {
                    cameraObstructed = value;
                    NotifyPropertyChanged("CameraObstructed");
                }
            }
            public const int ContrastSensitivityValueMin = 1;
            public const int ContrastSensitivityValueMax = 15;
            int contrastSensivity;
            public int ContrastSensivity {
                get { return contrastSensivity; }
                set {
                    contrastSensivity = value;
                    NotifyPropertyChanged("ContrastSensivity");
                }
            }
            bool imageTooDark;
            public bool ImageTooDark {
                get { return imageTooDark; }
                set {
                    imageTooDark = value;
                    NotifyPropertyChanged("ImageTooDark");
                }
            }
            bool imageTooBright;
            public bool ImageTooBright {
                get { return imageTooBright; }
                set {
                    imageTooBright = value;
                    NotifyPropertyChanged("ImageTooBright");
                }
            }
            bool imageTooNoisy;
            public bool ImageTooNoisy {
                get { return imageTooNoisy; }
                set {
                    imageTooNoisy = value;
                    NotifyPropertyChanged("ImageTooNoisy");
                }
            }
            bool imageTooBlurry;
            public bool ImageTooBlurry {
                get { return imageTooBlurry; }
                set {
                    imageTooBlurry = value;
                    NotifyPropertyChanged("ImageTooBlurry");
                }
            }

            public synesis.AntishakerCrop AntishakerCrop { get; set; }
            public synesis.MarkerCalibration Markers { get; set; }
            public synesis.UserRegion UserRegion{get;set;}
            
            private void NotifyPropertyChanged(String info) {
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        void GetSimpleItems() {
            modulDescr.config.Parameters.SimpleItem.ForEach(x => {
                switch (x.Name) {
                    case "StabilizationTime":
                        x.Value = XmlConvert.ToString(model.StabilizationTime);
                        break;
                    case "ShiftOutputPicture":
                        x.Value = XmlConvert.ToString(model.ShiftOutputPicture);
                        break;
                    case "UseObjectTracker":
                        x.Value = XmlConvert.ToString(model.UseObjectTracker);
                        break;
                    case "MaxObjectArea":
                        x.Value = XmlConvert.ToString(model.MaxObjectArea);
                        break;
                    case "MinObjectArea":
                        x.Value = XmlConvert.ToString(model.MinObjectArea);
                        break;
                    case "MaxObjectSpeed":
                        x.Value = XmlConvert.ToString(model.MaxObjectSpeed);
                        break;
                    case "DisplacementSensitivity":
                        x.Value = XmlConvert.ToString(model.DisplacementSensivity);
                        break;
                    case "CameraObstructed":
                        x.Value = XmlConvert.ToString(model.CameraObstructed);
                        break;
                    case "UseAntishaker":
                        x.Value = XmlConvert.ToString(model.UseAntishaker); 
                        break;
                    case "CameraRedirected":
                        x.Value = XmlConvert.ToString(model.CameraRedirected);
                        break;
                    case "ContrastSensitivity":
                        x.Value = XmlConvert.ToString(model.ContrastSensivity); 
                        break;
                    case "ImageTooDark":
                        x.Value = XmlConvert.ToString(model.ImageTooDark); 
                        break;
                    case "ImageTooBlurry":
                        x.Value = XmlConvert.ToString(model.ImageTooBlurry);
                        break;
                    case "ImageTooBright":
                        x.Value = XmlConvert.ToString(model.ImageTooBright);
                        break;
                    case "ImageTooNoisy":
                        x.Value = XmlConvert.ToString(model.ImageTooNoisy);
                        break;
                }
            });
        }
        void GetElementItems() {
            modulDescr.config.Parameters.ElementItem.ForEach(x => {
                switch (x.Name) {
                    case "AntishakerCrop":
                        ScaleAntishakerCropOutput(model.AntishakerCrop);
                        x.Any = model.AntishakerCrop.Serialize();
                        break;
                    case "MarkerCalibration":
                        ScaleMarkersOutput(model.Markers);
                        x.Any = model.Markers.Serialize();
                        break;
                    case "UserRegion":
                        ScaleUserRegionOutput(model.UserRegion);
                        x.Any = model.UserRegion.Serialize();
                        break;
                }
            });
        }
        void FillSimpleItems(ItemListSimpleItem[] simpleItems, SynesisAnalyticsModel model) {
            simpleItems.ForEach(x => {
                switch (x.Name) {
                    case "StabilizationTime":
                        model.StabilizationTime = DataConverter.StringToInt(x.Value);
                        break;
                    case "ShiftOutputPicture":
                        model.ShiftOutputPicture = DataConverter.StringToBool(x.Value);
                        break;
                    case "UseObjectTracker":
                        model.UseObjectTracker = DataConverter.StringToBool(x.Value);
                        break;
                    case "MaxObjectArea":
                        model.MaxObjectArea = DataConverter.StringToFloat(x.Value);
                        break;
                    case "MinObjectArea":
                        model.MinObjectArea = DataConverter.StringToFloat(x.Value);
                        break;
                    case "MaxObjectSpeed":
                        model.MaxObjectSpeed = DataConverter.StringToFloat(x.Value);
                        break;
                    case "DisplacementSensitivity":
                        model.DisplacementSensivity = DataConverter.StringToInt(x.Value);
                        break;
                    case "CameraObstructed":
                        model.CameraObstructed = DataConverter.StringToBool(x.Value);
                        break;
                    case "UseAntishaker":
                        model.UseAntishaker = DataConverter.StringToBool(x.Value);
                        break;
                    case "CameraRedirected":
                        model.CameraRedirected = DataConverter.StringToBool(x.Value);
                        break;
                    case "ContrastSensitivity":
                        model.ContrastSensivity = DataConverter.StringToInt(x.Value);
                        break;
                    case "ImageTooDark":
                        model.ImageTooDark = DataConverter.StringToBool(x.Value);
                        break;
                    case "ImageTooBlurry":
                        model.ImageTooBlurry = DataConverter.StringToBool(x.Value);
                        break;
                    case "ImageTooBright":
                        model.ImageTooBright = DataConverter.StringToBool(x.Value);
                        break;
                    case "ImageTooNoisy":
                        model.ImageTooNoisy = DataConverter.StringToBool(x.Value);
                        break;
                }
            });
        }

        void FillElementItems(ItemListElementItem[] elementItems, SynesisAnalyticsModel model) {
            elementItems.ForEach(x => {
                switch (x.Name) {
                    case "AntishakerCrop":
                        model.AntishakerCrop = x.Any.Deserialize<synesis.AntishakerCrop>();
                        ScaleAntishakerCropInput(model.AntishakerCrop);
                        break;
                    case "MarkerCalibration":
                        model.Markers = x.Any.Deserialize<synesis.MarkerCalibration>();
                        ScaleMarkersInput(model.Markers);
                        break;
                    case "UserRegion":
                        model.UserRegion = x.Any.Deserialize<synesis.UserRegion>();
                        ScaleUserRegionInput(model.UserRegion);
                        break;
                }
            });
        }
        
        void ScaleAntishakerCropOutput(synesis.AntishakerCrop crop) {
            double valueX = crop.XOffs;
            double valueY = crop.YOffs;
            double width = crop.CropWidth;
            double height = crop.CropHeight;
            //convert from video sourve resolution to encoder resolution
            double scalex = videoDescr.videoSourceResolution.Width / videoDescr.videoInfo.Resolution.Width;
            double scaley = videoDescr.videoSourceResolution.Height / videoDescr.videoInfo.Resolution.Height;
            valueX = valueX * scalex;
            valueY = valueY * scaley;
            height = height * scaley;
            width = width * scalex;
            //scale from visible to [-1;1]
            double heightValue = videoDescr.videoSourceResolution.Height - 1;
            double widthValue = videoDescr.videoSourceResolution.Width - 1;
            crop.XOffs = (float)(((valueX * 2) / widthValue) - 1);
            crop.YOffs = (float)((((heightValue - valueY) * 2) / heightValue) - 1);
            crop.CropWidth = (float)((width * 2) / widthValue);
            crop.CropHeight = (float)((height * 2) / heightValue);
        }
        void ScaleAntishakerCropInput(synesis.AntishakerCrop crop) {
            //scale from [-1;1] to visible dimensions
            double valueX = (videoDescr.videoSourceResolution.Width / 2) * (crop.XOffs + 1);
            double valueY = videoDescr.videoSourceResolution.Height - (videoDescr.videoSourceResolution.Height / 2) * (crop.YOffs + 1);
            double width = (videoDescr.videoSourceResolution.Width / 2) * (crop.CropWidth);
            double height = (videoDescr.videoSourceResolution.Height / 2) * (crop.CropHeight);
            //convert ftrom video sourve resolution to encoder resolution
            double scalex = videoDescr.videoInfo.Resolution.Width / (videoDescr.videoSourceResolution.Width == 0 ? 1 : videoDescr.videoSourceResolution.Width);
            double scaley = videoDescr.videoInfo.Resolution.Height / (videoDescr.videoSourceResolution.Height == 0 ? 1 : videoDescr.videoSourceResolution.Height);
            valueX = valueX * scalex;
            valueY = valueY * scaley;
            height = height * scaley;
            width = width * scalex;
            crop.XOffs = (float)valueX;
            crop.YOffs = (float)valueY;
            crop.CropHeight = (float)height;
            crop.CropWidth = (float)width;
        }
        void ScaleMarkersOutput(synesis.MarkerCalibration markers) {
            var cmarkerCalibration = markers.Item as synesis.CombinedMarkerCalibration;
            if (cmarkerCalibration != null) {
                //2d
                cmarkerCalibration.CombinedMarkers.ForEach(cm => {
                    cm.Rectangles.ForEach(rect => {
                        ScaleRectOutput(rect);
                    });
                });
            } else {
                //1d
                var hmarkerCalibration = markers.Item as synesis.HeightMarkerCalibration;
                hmarkerCalibration.HeightMarkers.ForEach(hm => {
                    hm.SurfaceNormals.ForEach(sn => {
                        DataConverter.ScalePointOutput(sn.Point, videoSourceSize, videoEncoderSize);
                        sn.Height = ScaleHeigthOutput(sn.Height);
                    });
                });
            }
        }
        void ScaleMarkersInput(synesis.MarkerCalibration markers) {
            var cmarkerCalibration = markers.Item as synesis.CombinedMarkerCalibration;
            if (cmarkerCalibration != null) {
                //2d
                cmarkerCalibration.CombinedMarkers.ForEach(cm => {
                    cm.Rectangles.ForEach(rect => {
                        ScaleRectInput(rect);
                    });
                });
            } else {
                //1d
                var hmarkerCalibration = markers.Item as synesis.HeightMarkerCalibration;
                hmarkerCalibration.HeightMarkers.ForEach(hm => {
                    hm.SurfaceNormals.ForEach(sn => {
                        DataConverter.ScalePointInput(sn.Point, videoSourceSize, videoEncoderSize);
                        sn.Height = ScaleHeigthInput(sn.Height);
                    });
                });
            }
        }
        void ScaleRectOutput(synesis.Rect rect) {
            DataConverter.ScalePointOutput(rect.LeftTop, videoSourceSize, videoEncoderSize);
            DataConverter.ScalePointOutput(rect.RightBottom, videoSourceSize, videoEncoderSize);
        }
        void ScaleRectInput(synesis.Rect rect) {
            DataConverter.ScalePointInput(rect.LeftTop, videoSourceSize, videoEncoderSize);
            DataConverter.ScalePointInput(rect.RightBottom, videoSourceSize, videoEncoderSize);
        }
        float ScaleHeigthOutput(float dval) {
            var val = (double)dval;
            double scaley = videoDescr.videoSourceResolution.Height / videoDescr.videoInfo.Resolution.Height;
            val = val * scaley;
            //scale from visible to [-1;1]
            double heightValue = videoDescr.videoSourceResolution.Height;
            return (float)((val * 2) / heightValue);
        }
        float ScaleHeigthInput(float dval) {
            //scale from [-1;1] to visible dimensions
            var val = (videoDescr.videoSourceResolution.Height / 2) * (dval);

            double scaley = videoDescr.videoInfo.Resolution.Height / videoDescr.videoSourceResolution.Height;
            val = val * scaley;
            return (float)val;
        }
        void ScaleUserRegionInput(synesis.UserRegion uregion) {
            uregion.Points.ForEach(x => {
                DataConverter.ScalePointInput(x, videoSourceSize, videoEncoderSize);
            });
        }
        void ScaleUserRegionOutput(synesis.UserRegion uregion) {
            uregion.Points.ForEach(x => {
                DataConverter.ScalePointOutput(x, videoSourceSize, videoEncoderSize);
            });
        }
        public void Apply() {
            try {
                pageObjectTracker.Apply();
                pageAntishaker.Apply();
                pageDepthCalibration.Apply();
                pageTampering.Apply();

                GetSimpleItems();
                GetElementItems();
            } catch (Exception err) {
                dbg.Error(err);
            }
        }

        public bool Init(IUnityContainer container, odm.ui.activities.ConfigureAnalyticView.ModuleDescriptor modulDescr, odm.ui.activities.ConfigureAnalyticView.AnalyticsVideoDescriptor videoDescr) {
            this.modulDescr = modulDescr;
            this.videoDescr = videoDescr;

            videoSourceSize = new Size(videoDescr.videoSourceResolution.Width, videoDescr.videoSourceResolution.Height);
            videoEncoderSize = new Size(videoDescr.videoInfo.Resolution.Width, videoDescr.videoInfo.Resolution.Height);

            model = new SynesisAnalyticsModel();

            try {
                FillSimpleItems(modulDescr.config.Parameters.SimpleItem, model);
            } catch (Exception err) {
                dbg.Error(err);
                return false;
            }
            try{
                FillElementItems(modulDescr.config.Parameters.ElementItem, model);
            } catch (Exception err) {
                dbg.Error(err);
                return false;
            }
            try{
				pageAntishaker.Init(container, model, videoDescr.videoInfo, videoDescr.profileToken);
            }catch(Exception err) {
                dbg.Error(err);
                return false;
            }
            try{
				pageDepthCalibration.Init(container, model, videoDescr.videoInfo, videoDescr.profileToken);
            }catch(Exception err) {
                dbg.Error(err);
                return false;
            }
            try{
				pageObjectTracker.Init(container, model, videoDescr.videoInfo, videoDescr.profileToken);
            }catch(Exception err) {
                dbg.Error(err);
                return false;
            }
            try{
				pageTampering.Init(container, model, videoDescr.videoInfo, videoDescr.profileToken);
            } catch (Exception err) {
                dbg.Error(err);
                return false;
            }

            //TODO: Stub fix for #225 Remove this with plugin functionality
			last = container.Resolve<odm.ui.activities.LastEditedModule>();
			analyticsTabCtrl.SelectionChanged += new SelectionChangedEventHandler((obj, arg) => {
				var selection = analyticsTabCtrl.SelectedItem as TabItem;
				var antishaker = selection.Content as AntishakerView;
				if (antishaker != null) {
					last.Tag = "pageAntishaker";
				}
				var calibration = selection.Content as DepthCalibrationView;
				if (calibration != null) {
					last.Tag = "pageDepthCalibration";
				}
				var objecttracker = selection.Content as ObjectTrackerView;
				if (objecttracker != null) {
					last.Tag = "pageObjectTracker";
				}
				var tampering = selection.Content as TamperingDetectorsView;
				if (tampering != null) {
					last.Tag = "pageTampering";
				}
			});
			if (last.Tag != "") {
				switch (last.Tag) {
					case "pageAntishaker":
						analyticsTabCtrl.SelectedItem = tabAntishaker;
						break;
					case "pageDepthCalibration":
						analyticsTabCtrl.SelectedItem = tabDepthCalibration;
						break;
					case "pageObjectTracker":
						analyticsTabCtrl.SelectedItem = tabObjectTracker;
						break;
					case "pageTampering":
						analyticsTabCtrl.SelectedItem = tabTampering;
						break;
				}
			}
            //

            return true;
        }
		//TODO: Stub fix for #225 Remove this with plugin functionality
        odm.ui.activities.LastEditedModule last;
		//
        public void Dispose() {
            pageAntishaker.Dispose();
            pageDepthCalibration.Dispose();
            pageObjectTracker.Dispose();
            pageTampering.Dispose();
        }
    }
}
