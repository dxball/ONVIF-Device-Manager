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
using System.IO.MemoryMappedFiles;

using odm.models;
using odm.utils.extensions;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyDisplayAnnotation.xaml
	/// </summary>
	public partial class PropertyDisplayAnnotation : BasePropertyControl {
		public PropertyDisplayAnnotation(AnnotationsModel devModel) {
			InitializeComponent();

			_devModel = devModel;
			InitControls();
			BindData();
			Localization();
		}
		AnnotationsModel _devModel;
		public Action Save;
		public Action Cancel;
		public MemoryMappedFile memFile {
			set {
				_videoPlayer.memFile = value;
			}
		}

		void InitControls(){
			Rect ret = new Rect(0, 0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);
		}

		void BindData() {
			cbCalibrationNet.CreateBinding(CheckBox.IsCheckedProperty, _devModel, x => x.calibrationResults, (m, v) => {
				m.calibrationResults = ((bool?)v).Value;
			});
			cbMovingRects.CreateBinding(CheckBox.IsCheckedProperty, _devModel, x => x.movingRects, (m, v) => { 
				m.movingRects = ((bool?)v).Value;
			});
			cbRegoinOfInterests.CreateBinding(CheckBox.IsCheckedProperty, _devModel, x => x.userRegion, (m, v) => {
				m.userRegion = ((bool?)v).Value;
			});
			cbSpeed.CreateBinding(CheckBox.IsCheckedProperty, _devModel, x => x.speed, (m, v) => {
				m.speed = ((bool?)v).Value;
			});
			cbTimeStamp.CreateBinding(CheckBox.IsCheckedProperty, _devModel, x => x.timestamp, (m, v) => {
				m.timestamp = ((bool?)v).Value;
			});
			cbTrajectories.CreateBinding(CheckBox.IsCheckedProperty, _devModel, x => x.tracking, (m, v) => {
				m.tracking = ((bool?)v).Value;
			});
			cbChannelName.CreateBinding(CheckBox.IsCheckedProperty, _devModel, x => x.channelName, (m, v) => {
				m.channelName = ((bool?)v).Value;
			});
			cbSysInfo.CreateBinding(CheckBox.IsCheckedProperty, _devModel, x => x.systemInfo, (m, v) => {
				m.systemInfo = ((bool?)v).Value;
			});
			//_saveCancelControl.Cancel.CreateBinding(Button.IsEnabledProperty, _devModel, x => x.isModified);
			_saveCancelControl.Cancel.IsEnabled = false;
			_saveCancelControl.Save.CreateBinding(Button.IsEnabledProperty, _devModel, x => x.isModified);
			_saveCancelControl.Save.Click += new RoutedEventHandler(Save_Click);
			_saveCancelControl.Cancel.Click += new RoutedEventHandler(Cancel_Click);
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			if (Cancel != null)
				Cancel();
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			if (Save != null)
				Save();			
		}
		LinkButtonsStrings titles = new LinkButtonsStrings();
		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.displayAnnotation);
			lblCalibrationNet.CreateBinding(Label.ContentProperty, PropertyDisplayAnnotationStrings.Instance, x => x.calibrationNet);
			lblChannelName.CreateBinding(Label.ContentProperty, PropertyDisplayAnnotationStrings.Instance, x => x.channelName);
			lblMovingRects.CreateBinding(Label.ContentProperty, PropertyDisplayAnnotationStrings.Instance, x => x.objects);
			lblRegoinOfInterests.CreateBinding(Label.ContentProperty, PropertyDisplayAnnotationStrings.Instance, x => x.userRegion);
			lblSpeed.CreateBinding(Label.ContentProperty, PropertyDisplayAnnotationStrings.Instance, x => x.speed);
			lblTimeStamp.CreateBinding(Label.ContentProperty, PropertyDisplayAnnotationStrings.Instance, x => x.timestamp);
			lblTrajectories.CreateBinding(Label.ContentProperty, PropertyDisplayAnnotationStrings.Instance, x => x.trajectories);
		}
		public override void ReleaseAll() {
			_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
	}
}
