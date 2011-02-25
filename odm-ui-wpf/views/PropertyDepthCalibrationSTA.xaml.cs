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
using System.Windows.Shapes;
using System.IO.MemoryMappedFiles;

using odm.models;
using odm.utils.extensions;
using odm.ui.controls.GraphEditor;
using Microsoft.Windows.Controls;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyDepthCalibrationSTA.xaml
	/// </summary>
	public partial class PropertyDepthCalibrationSTA : Window {
		public PropertyDepthCalibrationSTA(DepthCalibrationModel devModel, MemoryMappedFile memFile, Action exit) {
			InitializeComponent();
			Exit = exit;
			Loaded += new RoutedEventHandler(PropertyDepthCalibrationSTA_Loaded);

			model = devModel;
			_videoPlayer.memFile = memFile;
		}

		Action Exit;
		public Action Save { get; set; }
		public Action Cancel { get; set; }
		public Action<Exception, string> onBindingError { get; set; }
		public Action<string> onVideoInitializationError { get; set; } 
		DepthCalibrationModel model;
		PropertyDepthCalibrationStrings _strings = new PropertyDepthCalibrationStrings();
		Size PhisSize;
		void ShiftGraphEditor() {
			//Rect pos = EditorConverter.GetVideoBounds(new Rect(0, 0, _videoPlayer.ActualWidth, _videoPlayer.ActualHeight), 
			//    new Rect(0,0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height));

			//Canvas.SetLeft(regionEditor, pos.X);
			//Canvas.SetTop(regionEditor, pos.Y);
			//regionEditor.Width = pos.Width;
			//regionEditor.Height = pos.Height;
			//regionEditor.Background = new SolidColorBrush(Color.FromArgb(100, 200, 100, 100));

			//markerHolder.Margin = new Thickness(pos.X, pos.Y, 0, 0);
			markerHolder.Width = model.encoderResolution.Width;
			markerHolder.Height = model.encoderResolution.Height;
			//markerHolder.Background = new SolidColorBrush(Color.FromArgb(100, 200, 100, 100));
			
			//var translate = new TranslateTransform(pos.X, pos.Y);
			//var scale = new ScaleTransform();
			//scale.ScaleX = pos.Width / _devModel.encoderResolution.Width;
			//scale.ScaleY = pos.Height / _devModel.encoderResolution.Height;
			//var group = new TransformGroup();
			//group.Children.Add(scale);
			//group.Children.Add(translate);
			//markerHolder.RenderTransform = group;

			//var rect = new Rectangle();
			//rect.Width = 720;
			//rect.Height = 576;
			//Canvas.SetTop(rect, 0);
			//Canvas.SetLeft(rect, 0);
			//rect.Stroke = Brushes.GreenYellow;
			//markerHolder.Children.Add(rect);

			//var rect = new Rectangle();
			//rect.Width = _videoPlayer.ActualWidth;
			//rect.Height = _videoPlayer.ActualHeight;
			//Canvas.SetTop(rect, 0);
			//Canvas.SetLeft(rect, 0);
			//rect.Stroke = Brushes.GreenYellow;
			//markerHolder.Children.Add(rect);
				
			//Canvas.SetLeft(mainGrid, pos.X);
			//Canvas.SetTop(mainGrid, pos.Y);
			//mainGrid.Width = pos.Width;
			//mainGrid.Height = pos.Height;
			//mainGrid.Background = new SolidColorBrush(Color.FromArgb(100, 200, 100, 100));

		}
		void PropertyDepthCalibrationSTA_Loaded(object sender, RoutedEventArgs e) {
			InitControls();
			Localization();
			
			BindData(model);

			ShiftGraphEditor();

			LoadRegionEditor();
			LoadMarkers();

		}
		void Localization() {

			uiControl.focalLengthCaption.CreateBinding(Label.ContentProperty, _strings, x => x.focalLength);
			uiControl.matrixFormatCaption.CreateBinding(Label.ContentProperty, _strings, x => x.matrixFormat);

			uiControl.rb2d.CreateBinding(RadioButton.ContentProperty, _strings, x => x.marker2D);
			uiControl.rb1d.CreateBinding(RadioButton.ContentProperty, _strings, x => x.heightMarker);

			uiControl.saveCancel.Cancel.CreateBinding(Button.ContentProperty, _strings, x => x.cancel);
			uiControl.saveCancel.Save.CreateBinding(Button.ContentProperty, _strings, x => x.save);

			uiControl.phisHeigthCaption.CreateBinding(Label.ContentProperty, _strings, x => x.heigth);
			uiControl.phisWidthCaption.CreateBinding(Label.ContentProperty, _strings, x => x.width);
			uiControl.setSizeGroup.CreateBinding(GroupBox.HeaderProperty, _strings, x => x.physHeight);
			uiControl.phisSizeSave.CreateBinding(Button.ContentProperty, _strings, x => x.physHeightSave);
		}
		void InitControls() {
			uiControl.phisSizeSave.Click += new RoutedEventHandler(setSize_Click);

			if (!model.is2DmarkerSupported)
				uiControl.rb2d.IsEnabled = false;
			else {
				uiControl.rb2d.IsChecked = model.use2DMarkers;
				if (uiControl.rb2d.IsChecked.Value)
					uiControl.manualParams.IsEnabled = false;
			}

			uiControl.rb2d.Click += new RoutedEventHandler(rb2D_Click);
			uiControl.rb1d.Click += new RoutedEventHandler(rb1D_Click);
			uiControl.saveCancel.Cancel.Click += new RoutedEventHandler(Cancel_Click);
			uiControl.saveCancel.Save.Click += new RoutedEventHandler(Save_Click);

			_videoPlayer.Background = new SolidColorBrush(Colors.Black);
			Rect ret = new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height);

			//_graphEditor.InitResolution(ret);
			//_videoPlayer.Refresh =_graphEditor.InitResolution;
			_videoPlayer.InitPlayback(ret);
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			onCalibrate();
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			onExit();
		}

		void setSize_Click(object sender, RoutedEventArgs e) {
			if (m2dList.Count != 0) {
				m2dList.ForEach(x=>{
					double wdth = Convert.ToDouble(uiControl.phisWidthValue.Value);
					double hgth = Convert.ToDouble(uiControl.phisHeigthValue.Value);
					int w = (int)wdth;
					int h = (int)hgth;
					x.PhysicalSize = new Size((double)w, (double)h);
					PhisSize = x.PhysicalSize;
					x.Refresh();
				});
			} else {
				m1dList.ForEach(x => {
					double wdth = Convert.ToDouble(uiControl.phisWidthValue.Value);
					double hgth = Convert.ToDouble(uiControl.phisHeigthValue.Value);
					int w = (int)wdth;
					int h = (int)hgth;
					x.PhysicalSize = new Size((double)w, (double)h);
					PhisSize = x.PhysicalSize;
				});
			}
		}
		void BindData(DepthCalibrationModel devModel) {
			try {
				uiControl.focalLengthValue.CreateBinding(NumericUpDown.ValueProperty, devModel, x => (double)x.focalLength, (m, v) => { m.focalLength = (int)v; });
			} catch (Exception err) {
				string strValue;
				strValue = devModel.focalLength.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindFocalLength + strValue);
			}
			try {
				uiControl.matrixFormatValue.ItemsSource = MatrixTable.MatrixTbl;
				uiControl.matrixFormatValue.DisplayMemberPath = "Name";
				uiControl.matrixFormatValue.SelectedValuePath = "Value";
				uiControl.matrixFormatValue.SelectedItem = MatrixTable.MatrixTbl[0];

				MatrixTable.MatrixTbl.ToList<MatrixValue>().ForEach(x => {
					if (x.Name == model.matrixFormat) {
						uiControl.matrixFormatValue.SelectedItem = x;
						return;
					}
				});
			} catch (Exception err) {
				string strValue;
				strValue = devModel.photosensorPixelSize.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindSensorPixelSize + strValue);
			}

			if (model.is2DmarkerSupported) {
				uiControl.rb2d.IsChecked = model.use2DMarkers;
				uiControl.rb1d.IsChecked = !model.use2DMarkers;
			} else {
				uiControl.rb2d.IsEnabled = false;
				uiControl.rb1d.IsEnabled = false;
			}

			uiControl.matrixFormatValue.SelectionChanged += new SelectionChangedEventHandler(eCombo_SelectionChanged);

			//_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			uiControl.saveCancel.Cancel.IsEnabled = true;
			uiControl.saveCancel.Save.CreateBinding(Button.IsEnabledProperty, devModel, x => x.isModified);
		}
		List<marker2dEditor> m2dList = new List<marker2dEditor>();
		List<marker1dEditor> m1dList = new List<marker1dEditor>();
		void RefreshMarkers() {
			m2dList.ForEach(x=>{
				if (markerHolder.Children.Contains(x))
					markerHolder.Children.Remove(x);
			});
			m1dList.ForEach(x => {
				if (markerHolder.Children.Contains(x))
					markerHolder.Children.Remove(x);
			});
			m2dList.Clear();
			m1dList.Clear();
		}
		void AddHeigthMarker(Point P1, Point P2, Size size) {
			Point nP1 = P1;
			Point nP2 = P2;
			if (uiControl.rb2d.IsChecked.Value) {
				marker2dEditor m2d = new marker2dEditor();
				markerHolder.Children.Add(m2d);

				m2d.Init(nP1, nP2, new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height), size);
				m2d.PhysicalSize = size;
				m2dList.Add(m2d);
				
				uiControl.phisWidthValue.Value = (int)size.Width;
				uiControl.phisHeigthValue.Value = (int)size.Height;
			} else {
				marker1dEditor m1d = new marker1dEditor();
				markerHolder.Children.Add(m1d);

				nP1.X = (nP1.X + nP2.X) / 2;
				nP2.X = nP1.X;
				m1d.Init(nP1, nP2, new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height));
				m1d.PhysicalSize = size;
				m1dList.Add(m1d);
				
				uiControl.phisWidthValue.Value = (int)size.Width;
				uiControl.phisHeigthValue.Value = (int)size.Height;
			}
			//_graphEditor.AddHeightMarker(P1, P2, size);
		}

		void InitPhisSize() {
			if (model.markers != null && model.markers.Length != 0) {
				int pH = ((int)model.markers[0].size.y);
				int pW = ((int)model.markers[0].size.x);

				//convert from mm to cm
				pH = pH / 10;
				pW = pW / 10;

				PhisSize.Width = pW;
				PhisSize.Height = pH;
			}
		}
		void LoadRegionEditor() {
			if (model.region != null) {
				List<Point> plst = new List<Point>();
				model.region.ForEach(x => {
					var p = new Point((int)x.X, (int)x.Y);
					//Point np = EditorConverter.StreamToScreen(p, new Rect(0, 0, markerHolder.ActualWidth, markerHolder.ActualHeight), new Size(_devModel.encoderResolution.Width, _devModel.encoderResolution.Height));
					Point np = p;
					plst.Add(np);
				});
				regionEditor.Init(plst, new Rect(0, 0, model.encoderResolution.Width, model.encoderResolution.Height));
				
				
				//regionEditor.RenderTransform = scale;
				//_graphEditor.AddRegionEditor(plst);
			}

			//dbg.Assert(_devModel.markers != null);
			//dbg.Assert(_devModel.markers[0].line1 != null);
		}
		void LoadMarkers() {
			if (model.markers != null && model.markers.Count() > 0) {
				if (model.markers[0].size != null) {
					int pH = ((int)model.markers[0].size.y);
					int pW = ((int)model.markers[0].size.x);

					//convert from mm to cm
					pH = pH / 10;
					pW = pW / 10;

					if (model.markers[0].line1 != null) {
						Point p1UL = new Point() { X = (int)model.markers[0].line1.Point[1].x, Y = (int)model.markers[0].line1.Point[1].y };
						Point p1BR = new Point() { X = (int)model.markers[0].line1.Point[0].x, Y = (int)model.markers[0].line1.Point[0].y };

						AddHeigthMarker(p1UL, p1BR, new Size(pW, pH));
					} else {
						System.Windows.MessageBox.Show("Marker 1 is null");
					}
					if (model.markers[0].line2 != null) {
						Point p2UL = new Point() { X = (int)model.markers[0].line2.Point[1].x, Y = (int)model.markers[0].line2.Point[1].y };
						Point p2BR = new Point() { X = (int)model.markers[0].line2.Point[0].x, Y = (int)model.markers[0].line2.Point[0].y };

						AddHeigthMarker(p2UL, p2BR, new Size(pW, pH));
					} else {
						System.Windows.MessageBox.Show("Marker 2 is null");
					}
				} else {
					//dbg.Assert(_devModel.markers[0].size != null, "_devModel.markers[0].size == null");
				}
			}
		}

		void onExit() {
			//if (controls != null)
			//    controls.Close();
			ReleaseAll();
			if (Exit != null)
				Exit();
		}
		void onCalibrate() {
			model.use2DMarkers = uiControl.rb2d.IsChecked.Value;
			model.region.Clear();
			regionEditor.GetRegion().ForEach(x => {
				model.region.Add(new System.Drawing.Point((int)x.X, (int)x.Y));
			});

			Point m1p1;
			Point m1p2;
			Point m2p1;
			Point m2p2;
			Size phSize;
			if (m2dList.Count == 2 || m1dList.Count ==2) {
				if (uiControl.rb2d.IsChecked.Value) {
					m1p1 = m2dList[0].Top;
					m1p2 = m2dList[0].Bottom;
					m2p1 = m2dList[1].Top;
					m2p2 = m2dList[1].Bottom;
					phSize = m2dList[0].PhysicalSize;
				} else {
					m1p1 = m1dList[0].Top;
					m1p2 = m1dList[0].Bottom;
					m2p1 = m1dList[1].Top;
					m2p2 = m1dList[1].Bottom;
					phSize = m1dList[0].PhysicalSize;
				}
				////convert from cm to mm
				//retval[0].pheight *= 10;
				//retval[0].pwidth *= 10;
				//retval[1].pheight *= 10;
				//retval[1].pwidth *= 10;
				model.markers[0].size.y = (int)phSize.Height *10;
				model.markers[0].size.x = (int)phSize.Width *10;
				model.markers[0].line1.Point[0] = new global::onvif.types.Vector() { x = (float)(int)m1p1.X, y = (float)(int)m1p1.Y };
				model.markers[0].line1.Point[1] = new global::onvif.types.Vector() { x = (float)(int)m1p2.X, y = (float)(int)m1p2.Y };


				model.markers[0].line2.Point[0] = new global::onvif.types.Vector() { x = (float)(int)m2p1.X, y = (float)(int)m2p1.Y };
				model.markers[0].line2.Point[1] = new global::onvif.types.Vector() { x = (float)(int)m2p2.X, y = (float)(int)m2p2.Y };
			}
			
			Save();
			onExit();
		}

		void rb1D_Click(object sender, RoutedEventArgs e) {
			Set2DView();
			RefreshMarkers();
			LoadMarkers();
		}
		void rb2D_Click(object sender, RoutedEventArgs e) {
			Set2DView();
			RefreshMarkers();
			LoadMarkers();
		}
		void Set2DView() {
			uiControl.manualParams.IsEnabled = !uiControl.rb2d.IsChecked.Value;
			uiControl.phisWidthValue.IsEnabled = uiControl.rb2d.IsChecked.Value;
		}
		void eCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			model.matrixFormat = ((MatrixValue)uiControl.matrixFormatValue.SelectedItem).Name;
			uiControl.saveCancel.Save.IsEnabled = true;
		}

		public void ReleaseAll(){
			_videoPlayer.ReleaseAll();
			
		}
		protected virtual void VideoOperationError(string message) {
			if (onVideoInitializationError != null) {
				onVideoInitializationError(message);
			}
		}
		protected virtual void BindingError(Exception err, string message) {
			if (onBindingError != null) {
				onBindingError(err, message);
			}
		}
	}
}
