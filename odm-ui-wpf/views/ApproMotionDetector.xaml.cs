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
using odm.ui.controls;
using odm.models;
using System.IO.MemoryMappedFiles;
using odm.utils.extensions;
using odm.utils;
using System.Windows.Controls.Primitives;
using odm.ui.controls.GraphEditor;
using Microsoft.Windows.Controls;

namespace odm.ui.views {
	/// <summary>
	/// Interaction logic for ApproMotionDetector.xaml
	/// </summary>
	public partial class ApproMotionDetector : BasePropertyControl {
		public ApproMotionDetector(ApproMotionDetectorModel devModel) {
			model = devModel;
			InitializeComponent();

			Loaded += new RoutedEventHandler(ApproMotionDetector_Loaded);

			Localization();
		}

		void ApproMotionDetector_Loaded(object sender, RoutedEventArgs e) {
			InitControls();
		}
		ApproMotionDetectorModel model;
		public Action Save;
		public Action Cancel;
		PropertyObjectTrackerStrings strings = new PropertyObjectTrackerStrings();
		LinkButtonsStrings titles = new LinkButtonsStrings();

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
			tbSensetivity.CreateBinding(Label.ContentProperty, strings, x => x.sensitivity);
		}
		void InitControls() {
			Rect ret = new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);

			markerHolder.Width = model.encoderResolution.Width;
			markerHolder.Height = model.encoderResolution.Height;

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

			numSensetivity.CreateBinding(NumericUpDown.ValueProperty, model, x => (double)x.sensitivity, (m, v) => {
				float tv = (float)v;
				m.sensitivity = (int)tv; 
			});

			approMotionDetectorEditor.p020.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x020)!=0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x020;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x020);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p002.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x002) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x002;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x002);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p004.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x004) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x004;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x004);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p008.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x008) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x008;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x008);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p010.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x010) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x010;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x010);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p020.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x020) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x020;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x020);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p040.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x040) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x040;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x040);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p080.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x080) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x080;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x080);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p100.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x100) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x100;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x100);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p200.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x200) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x200;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x200);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p400.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x400) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x400;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x400);
				}
				dbg.Break();
			});
			approMotionDetectorEditor.p800.CreateBinding(ToggleButton.IsCheckedProperty, model, m => (m.regionMask & 0x800) != 0, (m, v) => {
				if (v) {
					m.regionMask = m.regionMask | 0x800;
				} else {
					m.regionMask = m.regionMask & (-1 ^ 0x800);
				}
				dbg.Break();
			});



			//approMotionDetectorEditor
		}
	}
}
