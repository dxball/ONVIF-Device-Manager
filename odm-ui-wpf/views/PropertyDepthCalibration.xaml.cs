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
using odm.models;
using System.IO.MemoryMappedFiles;

using odm.utils.extensions;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyDepthCalibration.xaml
	/// </summary>
	public partial class PropertyDepthCalibration : BasePropertyControl {
		public PropertyDepthCalibration(DepthCalibrationModel devModel, MemoryMappedFile memFile) {
			InitializeComponent();
			
			_devModel = devModel;
			_memFile = memFile;
			_videoPlayer.memFile = memFile;

			title.CreateBinding(Label.ContentProperty, PropertyDepthCalibrationStrings.Instance, x => x.title);

			InitControls();

			button.Click += new RoutedEventHandler(button_Click);
		}
		DepthCalibrationModel _devModel;
		MemoryMappedFile _memFile;
		public Action Save;
		public Action Cancel;

		void InitControls() {
			Rect ret = new Rect(0, 0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);
		}
		void ExitCalibration() {
			dcSta.ReleaseAll();
			if (dcSta != null) {
				dcSta.Close();
			}
		}
		PropertyDepthCalibrationSTA dcSta;
		void button_Click(object sender, RoutedEventArgs e) {
			dcSta = new PropertyDepthCalibrationSTA(_devModel, _memFile, ExitCalibration) {
				Save = Save,
				Cancel = Cancel
			};
			dcSta.Topmost = true;
			dcSta.ShowActivated = true;
			dcSta.Show();
		}

		public override void ReleaseAll() {
			_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
	}
}
