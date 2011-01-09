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
using odm.utils.extensions;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyLiveVideo.xaml
	/// </summary>
	public partial class PropertyLiveVideo : BasePropertyControl {
		public PropertyLiveVideo(LiveVideoModel devModel, MemoryMappedFile memFile, Action StartRecording, Action StopRecording) {
			InitializeComponent();
			OnStartRecording = StartRecording;
			OnStopRecording = StopRecording;
			_devModel = devModel;
			_memFile = memFile;

			_videoPlayer.memFile = memFile;
			InitControls();
			_tbVideoUrl.Text = devModel.mediaUri;

			Localization();
		}
		Action OnStartRecording;
		Action OnStopRecording;
		LiveVideoModel _devModel;
		MemoryMappedFile _memFile;

		void Localization() {
			_title.CreateBinding(Title.ContentProperty, PropertyLiveVideoStrings.Instance, x => x.title);
			
		}
		void InitControls() { 
			Rect ret = new Rect(0,0,_devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);

			checkBox1.Click += new RoutedEventHandler(checkBox1_Click);
		}

		void checkBox1_Click(object sender, RoutedEventArgs e) {
			if (checkBox1.IsChecked.Value) {
				if (OnStartRecording != null) {
					OnStartRecording();
				}
			} else {
				if (OnStopRecording != null) {
					OnStopRecording();
				}
			}

		}
		public override void ReleaseAll() {
			if (OnStopRecording != null)
				OnStopRecording();
			_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
	}
}
