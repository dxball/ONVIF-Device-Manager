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
using odm.ui.controls.GraphEditor;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyAntishaker.xaml
	/// </summary>
	public partial class PropertyAntishaker : BasePropertyControl {
		public PropertyAntishaker(AntishakerModel devModel) {
			InitializeComponent();
			model = devModel;
			Localization();
			InitControl();
			Loaded += new RoutedEventHandler(PropertyAntishaker_Loaded);
			this.SizeChanged += new SizeChangedEventHandler(PropertyAntishaker_SizeChanged);
			
			region.SizeChanged += new SizeChangedEventHandler(region_SizeChanged);
		}
		bool isInit = true;
		Size clientSize;
		void region_SizeChanged(object sender, SizeChangedEventArgs e) {
			clientSize = e.NewSize;
			if (isInit) {
				InitDrawing(e.NewSize);
				isInit = false;
			} else {
				Rect sreamRect = new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height);
				Rect clientRect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
				region.Resize(EditorConverter.GetVideoBounds(clientRect, sreamRect), sreamRect.Size);
			}
		}
		void PropertyAntishaker_SizeChanged(object sender, SizeChangedEventArgs e) {
			
		}

		void PropertyAntishaker_Loaded(object sender, RoutedEventArgs e) {
			//InitDrawing();
		}
		AntishakerModel model;
		public MemoryMappedFile memFile { get; set; }
		public Action Save { get; set; }
		public Action Cancel { get; set; }
		PropertyAntishakerStrings strings = new PropertyAntishakerStrings();
		LinkButtonsStrings titles = new LinkButtonsStrings();

		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.cantishaker);
		}
		public void InitPlayback() {
			player.memFile = memFile;
			Rect sreamRect = new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height);
			player.InitPlayback(sreamRect);
		}
		public void InitDrawing(Size sz) {
			Rect sreamRect = new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height);
			Rect clientRect = new Rect(0, 0, sz.Width, sz.Height);
			Rect cropRectRaw = DrawingConverter.RectangleToRect(model.croppingRectangle);
			
			Rect cropRect = EditorConverter.StreamToScreenR(cropRectRaw, EditorConverter.GetVideoBounds(clientRect, sreamRect) , sreamRect.Size);

			region.ClearEllipses();
			region.Init(cropRect, EditorConverter.GetVideoBounds(clientRect, sreamRect));
		}
		void InitControl() {
			saveCancel.Save.Click += new RoutedEventHandler(Save_Click);
			saveCancel.Cancel.Click += new RoutedEventHandler(Cancel_Click);
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			if (Cancel != null) { 
			}

			InitDrawing(clientSize);
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			Rect sreamRect = new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height);
			Rect clientRect = new Rect(clientSize);
			Rect crop = EditorConverter.ScreenToStreamR(region.GetCroppingRect(), EditorConverter.GetVideoBounds(clientRect, sreamRect), sreamRect.Size);
			model.croppingRectangle = EditorConverter.ToWinForms(crop);
			if (Save != null)
				Save();
		}

		public override void ReleaseAll() {
			player.ReleaseAll();
			base.ReleaseAll();
		}
	}
}
