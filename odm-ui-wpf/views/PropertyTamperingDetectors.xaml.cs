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

using odm.utils.extensions;
using System.IO.MemoryMappedFiles;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyTamperingDetectors.xaml
	/// </summary>
	public partial class PropertyTamperingDetectors : BasePropertyControl {
		public PropertyTamperingDetectors(TamperingDetectorsModel devModel) {
			InitializeComponent();
			model = devModel;
			Localisation();
			InitControls();
			BindData();
		}
		TamperingDetectorsModel model;
		public MemoryMappedFile memFile { get; set; }
		public Action Save;
		public Action Cancel;
		PropertyTamperingDetectorsStrings strings = new PropertyTamperingDetectorsStrings();
		LinkButtonsStrings titles = new LinkButtonsStrings();
		bool initialVdisplacement;
		bool initialVobstruction;
		bool initialVglobalChange;
		bool initialVoutOfFocus;
		bool initialVoverexposure;
		bool initialVblackout;
		bool initialVsceneNoisy;
		bool initialVchannelstate;

		public void InitPlayback() {
			player.memFile = memFile;
			Rect sreamRect = new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height);
			player.InitPlayback(sreamRect);
		}
		void Localisation() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.tamperingDetectors);
			cameraDisplacedCaption.CreateBinding(CheckBox.ContentProperty, strings, x => x.cameraDisplaced);
			fildOfViewCaption.CreateBinding(CheckBox.ContentProperty, strings, x => x.fieldObstructed);
			globalCaption.CreateBinding(CheckBox.ContentProperty, strings, x => x.globalChange);
			outFocusCaption.CreateBinding(CheckBox.ContentProperty, strings, x => x.outFocus);
			sceneBrightCaption.CreateBinding(CheckBox.ContentProperty, strings, x => x.sceneBright);
			sceneDarkCaption.CreateBinding(CheckBox.ContentProperty, strings, x => x.sceneDark);
			sceneNoiseCaption.CreateBinding(CheckBox.ContentProperty, strings, x => x.sceneNoisy);
			signalLostCaption.CreateBinding(CheckBox.ContentProperty, strings, x => x.signalLost);
		}
		void InitControls() {
			initialVdisplacement = model.displacement;
			initialVobstruction=model.obstruction;
			//initialVglobalChange=model.;
			initialVoutOfFocus=model.outOfFocus;
			initialVoverexposure=model.overexposure;
			initialVblackout=model.blackout;
			//initialVsceneNoisy;
			initialVchannelstate=model.channelstate;

			saveCancelControl.Save.Click += new RoutedEventHandler(Save_Click);
			saveCancelControl.Cancel.Click += new RoutedEventHandler(Cancel_Click);
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			if (Cancel != null)
			    Cancel();
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			if (Save != null)
				Save();
		}
		void BindData() {
			cameraDisplacedCaption.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.displacement, (m, v) => { m.displacement = ((bool?)v).Value; });
			fildOfViewCaption.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.obstruction, (m, v) => { m.obstruction = ((bool?)v).Value; });
			//globalCaption.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.globalChange, (m, v) => { m.globalChange = ((bool?)v).Value; });
			outFocusCaption.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.outOfFocus, (m, v) => { m.outOfFocus = ((bool?)v).Value; });
			sceneBrightCaption.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.overexposure, (m, v) => { m.overexposure = ((bool?)v).Value; });
			sceneDarkCaption.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.blackout, (m, v) => { m.blackout = ((bool?)v).Value; });
			//sceneNoiseCaption.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.sceneNoisy, (m, v) => { m.displacement = ((bool?)v).Value; });
			signalLostCaption.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.channelstate, (m, v) => { m.channelstate = ((bool?)v).Value; });

			saveCancelControl.Save.CreateBinding(Button.IsEnabledProperty, model, x => x.isModified);
			saveCancelControl.Cancel.CreateBinding(Button.IsEnabledProperty, model, x => x.isModified);
		}
		public override void ReleaseAll() {
			//_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
	}
}
