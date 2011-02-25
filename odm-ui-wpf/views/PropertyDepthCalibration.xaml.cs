#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion


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

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyDepthCalibration.xaml
	/// </summary>
	public partial class PropertyDepthCalibration : BasePropertyControl {
		public PropertyDepthCalibration(DepthCalibrationModel devModel, MemoryMappedFile memFile) {
			InitializeComponent();
			
			_devModel = devModel;
			_memFile = memFile;
			_videoPlayer.memFile = memFile;

			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.depthCalibration);
			button.CreateBinding(Button.ContentProperty, PropertyDepthCalibrationStrings.Instance, x => x.clibrate);

			InitControls();

			button.Click += new RoutedEventHandler(button_Click);
		}
		LinkButtonsStrings titles = new LinkButtonsStrings();
		DepthCalibrationModel _devModel;
		MemoryMappedFile _memFile;
		public Action Save;
		public Action Cancel;

		void InitControls() {
			Rect ret = new Rect(0, 0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);
		}
		void ExitCalibration() {
			if (dcSta != null) {
				dcSta.Close();
			}
			InitControls();
		}
		PropertyDepthCalibrationSTA dcSta;
		void button_Click(object sender, RoutedEventArgs e) {
			_videoPlayer.ReleaseAll();
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
