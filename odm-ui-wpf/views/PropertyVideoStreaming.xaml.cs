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
using System.Threading;
using Microsoft.Windows.Controls;
using System.IO.MemoryMappedFiles;
using MessageBox = System.Windows.MessageBox;

using odm.models;
using odm.utils;
using odm.utils.extensions;
using System.ComponentModel;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyVideoStreaming.xaml
	/// </summary>
	public partial class PropertyVideoStreaming : BasePropertyControl {
		PropertyVideoStreamingStrings _strings = new PropertyVideoStreamingStrings();
		VideoStreamingModel model;

		//Initial values
		int _initialEncodingInteval;

		public Action Save {
			get;
			set;
		}
		public Action Cancel {
			get;
			set;
		}

		public override void ReleaseUnmanaged() {
		}
		
		public PropertyVideoStreaming():this(null) {
		}

		public PropertyVideoStreaming(VideoStreamingModel devModel) {
			InitializeComponent();
			model = devModel;
			
			Loaded += PropertyVideoStreaming_Load;
			
		}

		public MemoryMappedFile memFile {
			set {
				_videoPlayer.memFile = value;
			}
		}

		void PropertyVideoStreaming_Load(object sender, EventArgs e) {
			IniControls();
			InitUrl();

			BindData();

			SubscribeToEvents();
		}

		void _cbFPS_SelectionChangeCommitted(object sender, EventArgs e) {
			_saveCancelControl.Save.IsEnabled=true;
			//_saveCancelControl.EnableCancel(true);
		}

		//public void StopdStreaming() {
		//    //if (_vlcProcess != null)
		//    //    _vlcProcess.Kill();
		//}
		//public void PlaydStreaming() {
		//    //InitUrl();
		//}
		public void InitUrl() {
			dbg.Assert(SynchronizationContext.Current != null);
			Rect ret = new Rect(0,0,model.currentResolution.width, model.currentResolution.height);
			_videoPlayer.InitPlayback(ret);
		}

		void SubscribeToEvents() {
			_saveCancelControl.Save.Click += _saveCancelControl_ButtonClickedSave;
			_saveCancelControl.Cancel.Click += _saveCancelControl_ButtonClickedCancel;
		}

		public override void ReleaseAll() {
			_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
		void InitFps(VideoEncoder encoder) {
			try {
				if (encoder.maxFrameRate == encoder.minFrameRate) {
					frameRate.Value = encoder.maxFrameRate;
					frameRate.IsEnabled = false;
				} else {
					frameRate.IsEnabled = true;
					try {
						frameRate.Maximum = model.frameRate > encoder.maxFrameRate ? model.frameRate : encoder.maxFrameRate;
						frameRate.Minimum = model.frameRate < encoder.minFrameRate ? model.frameRate : encoder.minFrameRate;
						//frameRate.DataBindings.Clear();
						frameRate.CreateBinding(NumericUpDown.ValueProperty, model, x => (double)x.frameRate, (m, v)=>{m.frameRate = (int)v;});

					} catch (Exception err) {
						if (encoder == null) {
							BindingError(err, ExceptionStrings.Instance.errBindVStrCurrentEncoder + "Null");
						} else {
							if (encoder.maxFrameRate < encoder.minFrameRate)
								BindingError(err, ExceptionStrings.Instance.errBindVStrFrameRateMinMax);
							else {
								BindingError(err, ExceptionStrings.Instance.errBindVStrFrameRate + model.frameRate);
							}
						}
					}
				}
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
		}

		void InitEncodingInterval(VideoEncoder encoder) {

			encodingInterval.Minimum = model.encodingInterval < encoder.minEncodingInterval ? model.encodingInterval : encoder.minEncodingInterval;
			encodingInterval.Maximum = model.encodingInterval > encoder.maxEncodingInterval ? model.encodingInterval : encoder.maxEncodingInterval;
			encodingInterval.Value = model.encodingInterval;
			//encodingInterval.IsSnapToTickEnabled = true;
			_initialEncodingInteval = model.encodingInterval;
			//SetEncodingIntervalText();
		}
		//void SetEncodingIntervalText() {
		//    encodingValue.Content = encodingInterval.Value.ToString();
		//}
		public void RefershBindings() {
			//bitrate.DataBindings.Clear();
			//encoder.DataBindings.Clear();
			//_numtbFPS.DataBindings.Clear();
			//resolution.DataBindings.Clear();
			//metadata.DataBindings.Clear();
			//channelName.DataBindings.Clear();

			//_saveCancelControl._btnCancel.DataBindings.Clear();
			//_saveCancelControl._btnSave.DataBindings.Clear();

			BindData();
		}
		void BindData() {
			bitrate.Maximum = 100000;// devModel.maxBitrate;
			bitrate.Minimum = 0; // devModel.minBitrate;
			bitrate.CreateBinding(NumericUpDown.ValueProperty, model, m => (double)m.bitrate, (m, v) => {m.bitrate = (int)v;});

			//try {
				//InitFps(model.currentEncoder);
				//InitEncodingInterval(model.currentEncoder);
			//} catch (Exception err) {
			//	VideoOperationError(err.Message);
			//}

			encoder.Items.Clear();
			if (model.supportedEncoders != null) {
				model.supportedEncoders.ForEach(x=>{
					encoder.Items.Add(x);
				});
			}
			
			encoder.CreateBinding(ComboBox.SelectedItemProperty, model, x => x.currentEncoder, (m, v)=>{m.currentEncoder = (VideoEncoder)v;});
			
			resolution.Items.Clear();
			if (model.supportedResolutions != null) {
				model.supportedResolutions.ForEach(x => {
					resolution.Items.Add(x);
				});
			}

			resolution.CreateBinding(ComboBox.SelectedItemProperty, model, m => m.currentResolution, (m, v)=>{m.currentResolution = (VideoResolution)v;});
			metadata.CreateBinding(CheckBox.IsCheckedProperty, model, m => m.metadata, (m, v)=>{ m.metadata = ((bool?)v).Value;});
			channelName.CreateBinding(TextBox.TextProperty, model, m => m.channelName, (m, v)=>{ m.channelName = v;});			

			priority.IsEnabled = false;

			_saveCancelControl.Cancel.CreateBinding(Button.IsEnabledProperty, model, x => x.isModified);
			_saveCancelControl.Save.CreateBinding(Button.IsEnabledProperty, model, x => x.isModified);
		}

		void Localization() {
			title.CreateBinding(Label.ContentProperty, _strings, x => x.title);
			bitrateCaption.CreateBinding(Label.ContentProperty, _strings, x => x.bitrate);
			encoderCaption.CreateBinding(Label.ContentProperty, _strings, x => x.encoder);
			frameRateCaption.CreateBinding(Label.ContentProperty, _strings, x => x.frameRate);
			encodingIntervalCaption.CreateBinding(Label.ContentProperty, _strings, x => x.encodingInterval);
			priorityCaption.CreateBinding(Label.ContentProperty, _strings, x => x.prioriy);
			resolutionCaption.CreateBinding(Label.ContentProperty, _strings, x => x.resolution);
			channelNameCaption.CreateBinding(Label.ContentProperty, _strings, x => x.channelName);
			metadataCaption.CreateBinding(Label.ContentProperty, _strings, x => x.metadata);
		}
		protected void IniControls() {
			Localization();

			bitrate.ValueChanged += _numtbBitrate_ValueChanged;
			encoder.SelectionChanged += _cmbEncoder_SelectionChangeCommitted;
			encodingInterval.ValueChanged += _trackBar_ValueChanged;
		}

		void _numtbBitrate_ValueChanged(object sender, EventArgs e) {
			_saveCancelControl.Save.IsEnabled = true;
		}

		void _trackBar_ValueChanged(object sender, EventArgs e) {
			//SetEncodingIntervalText();
			if (_initialEncodingInteval != encodingInterval.Value) {
				_saveCancelControl.Save.IsEnabled = true;
				_saveCancelControl.Cancel.IsEnabled = true;
			}
		}

		void _cmbEncoder_SelectionChangeCommitted(object sender, EventArgs e) {
			var encInterval = (VideoEncoder)encoder.SelectedItem;
			InitFps(encInterval);
			InitEncodingInterval(encInterval);
		}

		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			encodingInterval.Value = _initialEncodingInteval;
			Cancel();
		}

		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			model.encodingInterval = (int)encodingInterval.Value;
			Save();
		}

	}
	public class EncoderView {
		public EncoderView(odm.models.VideoEncoder.Encoding enc) {
			_encoder = new VideoEncoder(enc);
		}
		odm.models.VideoEncoder _encoder;
		public odm.models.VideoEncoder GetVideoEncoder() {
			return _encoder;
		}
		public void SetVideoEncoder(odm.models.VideoEncoder value) {
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
		public VideoResolution Resolution {
			get;
			set;
		}
		public override string ToString() {
			return Resolution.ToString();
		}
	}
	public class FPSview {
		public FPSview(int missFrame, int name) {
			MissingFrames = missFrame;
			Text = name.ToString();
		}
		public int MissingFrames {
			get;
			private set;
		}
		public string Text {
			get;
			set;
		}
	}
}
