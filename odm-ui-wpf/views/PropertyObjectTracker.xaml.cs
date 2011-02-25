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
using Microsoft.Windows.Controls;

using odm.models;
using odm.utils.extensions;
using System.Windows.Controls.Primitives;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyObjectTracker.xaml
	/// </summary>
	public partial class PropertyObjectTracker : BasePropertyControl {
		public PropertyObjectTracker(ObjectTrackerModel devModel) {
			InitializeComponent();
			model = devModel;

			Localization();
			Binding();
			InitControls();
		}
		PropertyObjectTrackerStrings strings = new PropertyObjectTrackerStrings();
		LinkButtonsStrings titles = new LinkButtonsStrings();
		ObjectTrackerModel model;
		public Action Save;
		public Action Cancel;
		public override void ReleaseAll() {
			_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
		public MemoryMappedFile memFile {
			set {
				_videoPlayer.memFile = value;
			}
		}
		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.objectTracker);

			directionRose.btnAll.CreateBinding(Button.ContentProperty, strings, x => x.all);
			directionRose.btnNone.CreateBinding(Button.ContentProperty, strings, x => x.none);

			indirection.CreateBinding(Label.ContentProperty, strings, x => x.direction);

			tbAreaMax.CreateBinding(Label.ContentProperty, strings, x => x.areaMax);
			tbAreaMin.CreateBinding(Label.ContentProperty, strings, x => x.areaMin);
			tbContrastSensitivity.CreateBinding(Label.ContentProperty, strings, x => x.contrast);
			tbSpeedMax.CreateBinding(Label.ContentProperty, strings, x => x.speedMax);
			tbStabilization.CreateBinding(Label.ContentProperty, strings, x => x.stabilization);
			tbDisplacement.CreateBinding(Label.ContentProperty, strings, x => x.displacement);
		}
		void Binding() {
			directionRose.bE.CreateBinding(ToggleButton.IsCheckedProperty, model, x => (x.rose_right > 0), (m, v) => { m.rose_right = v ? 1 : 0; });
			directionRose.bN.CreateBinding(ToggleButton.IsCheckedProperty, model, x => (x.rose_up > 0), (m, v) => { m.rose_up = v ? 1 : 0; });
			directionRose.bNE.CreateBinding(ToggleButton.IsCheckedProperty, model, x => (x.rose_up_right > 0), (m, v) => { m.rose_up_right = v ? 1 : 0; });
			directionRose.bNW.CreateBinding(ToggleButton.IsCheckedProperty, model, x => (x.rose_up_left > 0), (m, v) => { m.rose_up_left = v ? 1 : 0; });
			directionRose.bS.CreateBinding(ToggleButton.IsCheckedProperty, model, x => (x.rose_down > 0), (m, v) => { m.rose_down = v ? 1 : 0; });
			directionRose.bSE.CreateBinding(ToggleButton.IsCheckedProperty, model, x => (x.rose_down_right > 0), (m, v) => { m.rose_down_right = v ? 1 : 0; });
			directionRose.bSW.CreateBinding(ToggleButton.IsCheckedProperty, model, x => (x.rose_down_left > 0), (m, v) => { m.rose_down_left = v ? 1 : 0; });
			directionRose.bW.CreateBinding(ToggleButton.IsCheckedProperty, model, x => (x.rose_left > 0), (m, v) => { m.rose_left = v ? 1 : 0; });

			numAreaMax.CreateBinding(NumericUpDown.ValueProperty, model, x => (double)x.maxObjectArea, (m, v) => {m.maxObjectArea = (float)v;});
			numAreaMin.CreateBinding(NumericUpDown.ValueProperty, model, x => (double)x.minObjectArea, (m, v) => {m.minObjectArea = (float)v;});

			numDisplacement.CreateBinding(NumericUpDown.ValueProperty, model, x => x.displacementSensitivity, (m, v) => { m.displacementSensitivity = v; });
			numContrastSensitivity.CreateBinding(NumericUpDown.ValueProperty, model, x => (double)x.contrastSensitivity, (m, v) => {m.contrastSensitivity = (int)v;});
			numSpeedMax.CreateBinding(NumericUpDown.ValueProperty, model, x => (double)x.maxObjectSpeed, (m, v) => {m.maxObjectSpeed = (int)v;});
			numStabilization.CreateBinding(NumericUpDown.ValueProperty, model, x => (double)x.stabilizationTime, (m, v) => {m.stabilizationTime = (float)v;});
			
			saveCancel.Cancel.CreateBinding(Button.IsEnabledProperty, model, m => m.isModified);
			saveCancel.Save.CreateBinding(Button.IsEnabledProperty, model, m => m.isModified);
		}
		void InitControls() {
			numAreaMax.Increment = 0.01f;
			numAreaMax.FormatString = "F2";
			//numAreaMax.Minimum = 0.25;
			numAreaMax.Maximum = 100.0f;
			numAreaMin.Increment = 0.01f;
			numAreaMin.FormatString = "F2";
			numAreaMin.Minimum = 0.0f;
			//numAreaMin.Maximum = 10.0;
			numContrastSensitivity.Minimum = 0;
			numContrastSensitivity.Maximum = 15;
			numSpeedMax.Minimum = 1;
			numSpeedMax.Maximum = 30;
			numStabilization.Maximum = 10000;
			numStabilization.Minimum = 40;

			numDisplacement.Maximum=4;
			numDisplacement.Minimum=0;

			Rect ret = new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);

			directionRose.SelectionChanged = GetSelection;

			saveCancel.Save.Click += (s, e) => {
				if (Save != null) {
					Save();
				}
			};
			saveCancel.Cancel.Click += (s, e) => {
				if (Cancel != null) {
					Cancel();
				}
			};
			saveCancel.Cancel.Click += new RoutedEventHandler(Cancel_Click);
		}
		void GetSelection() {
			//model.rose_right = directionRose.IfbE ? 1 : 0;
			//model.rose_up = directionRose.IfbN ? 1 : 0;
			//model.rose_up_right = directionRose.IfbNE ? 1 : 0;
			//model.rose_up_left = directionRose.IfbNW ? 1 : 0;
			//model.rose_down = directionRose.IfbS ? 1 : 0;
			//model.rose_down_right = directionRose.IfbSE ? 1 : 0;
			//model.rose_down_left = directionRose.IfbSW ? 1 : 0;
			//model.rose_left = directionRose.IfbW ? 1 : 0;
		}		
		void Cancel_Click(object sender, RoutedEventArgs e) {
			if (Cancel != null)
				Cancel();
		}

	}
}
