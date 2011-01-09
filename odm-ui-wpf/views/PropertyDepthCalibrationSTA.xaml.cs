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
using odm.controls.GraphEditor;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyDepthCalibrationSTA.xaml
	/// </summary>
	public partial class PropertyDepthCalibrationSTA : Window {
		public PropertyDepthCalibrationSTA(DepthCalibrationModel devModel, MemoryMappedFile memFile, Action exit) {
			InitializeComponent();
			Exit = exit;
			Loaded += new RoutedEventHandler(PropertyDepthCalibrationSTA_Loaded);

			_devModel = devModel;
			_videoPlayer.memFile = memFile;
		}
		Action Exit;
		public Action Save { get; set; }
		public Action Cancel { get; set; }
		public Action<Exception, string> onBindingError { get; set; }
		public Action<string> onVideoInitializationError { get; set; } 
		DepthCalibrationModel _devModel;
		PropertyDepthCalibrationSTAcontrol controls;
		PropertyDepthCalibrationStrings _strings = new PropertyDepthCalibrationStrings();
		
		void PropertyDepthCalibrationSTA_Loaded(object sender, RoutedEventArgs e) {
			controls = new PropertyDepthCalibrationSTAcontrol(onExit, onCalibrate);
			InitControls();
			Localization();
			controls.Owner = this;
			controls.ShowInTaskbar = false;
			controls.Show();

			BindData(_devModel);
			LoadRegionEditor();
			LoadMarkers();
		}
		void Localization() {
			
			controls.EditFocalL.lName.CreateBinding(TextBlock.TextProperty, _strings, x => x.focalLength);
			controls.EditMatrix.lText.CreateBinding(Label.ContentProperty, _strings, x => x.matrixFormat);

			controls.rb2d.CreateBinding(RadioButton.ContentProperty, _strings, x => x.marker2D);
			controls.rb1d.CreateBinding(RadioButton.ContentProperty, _strings, x => x.heightMarker);

			controls.SaveCancel.Cancel.CreateBinding(Button.ContentProperty, _strings, x => x.cancel);
			controls.SaveCancel.Save.CreateBinding(Button.ContentProperty, _strings, x => x.save);

			controls.MarkerSize.lheigth.CreateBinding(Label.ContentProperty, _strings, x => x.heigth);
			controls.MarkerSize.lwidth.CreateBinding(Label.ContentProperty, _strings, x => x.width);
			controls.MarkerSize.title.CreateBinding(Label.ContentProperty, _strings, x => x.physHeight);
			controls.MarkerSize.setSize.CreateBinding(Button.ContentProperty, _strings, x => x.physHeightSave);
			controls.CreateBinding(Window.TitleProperty, _strings, x=>x.physHeightTitle);
		}
		void InitControls() {

			controls.MarkerSize.setSize.Click += new RoutedEventHandler(setSize_Click);

			if (!_devModel.is2DmarkerSupported)
				controls.rb2d.IsEnabled = false;
			else {
				controls.rb2d.IsChecked = _devModel.use2DMarkers;
				if (controls.rb2d.IsChecked.Value)
					controls.groupBox1.IsEnabled = false;
			}

			controls.rb2D.Click += new RoutedEventHandler(rb2D_Click);
			controls.rb1D.Click += new RoutedEventHandler(rb1D_Click);

			_videoPlayer.Background = new SolidColorBrush(Colors.Black);
			Rect ret = new Rect(0, 0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height);

			//_graphEditor.InitResolution(ret);
			//_videoPlayer.Refresh =_graphEditor.InitResolution;
			_videoPlayer.InitPlayback(ret);
		}

		void setSize_Click(object sender, RoutedEventArgs e) {
			if (m2dList.Count != 0) {
				m2dList.ForEach(x=>{
					double width = 0;
					double heigth = 0;
					if(!double.TryParse(controls.MarkerSize.tbWidth.Text, out width))
						width = 0;
					if (!double.TryParse(controls.MarkerSize.tbHeigth.Text, out heigth))
						heigth = 0;
					x.PhysicalSize = new Size(width, heigth);
				});
			} else {
				m1dList.ForEach(x => {
					double width = 0;
					double heigth = 0;
					if (!double.TryParse(controls.MarkerSize.tbWidth.Text, out width))
						width = 0;
					if (!double.TryParse(controls.MarkerSize.tbHeigth.Text, out heigth))
						heigth = 0;
					x.PhysicalSize = new Size(width, heigth);
				});
			}
		}
		void BindData(DepthCalibrationModel devModel) {
			try {
				controls.editFocalLength.eText.CreateBinding(TextBox.TextProperty, devModel, x => x.focalLength, (m, v) => { m.focalLength = v; });
			} catch (Exception err) {
				string strValue;
				strValue = devModel.focalLength.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindFocalLength + strValue);
			}
			try {
				controls.editMatrixFormat.eCombo.ItemsSource = MatrixTable.MatrixTbl;
				controls.editMatrixFormat.eCombo.DisplayMemberPath = "Name";
				controls.editMatrixFormat.eCombo.SelectedValuePath = "Value";
				controls.editMatrixFormat.eCombo.SelectedItem = MatrixTable.MatrixTbl[0];

				MatrixTable.MatrixTbl.ToList<MatrixValue>().ForEach(x => {
					if (x.Name == _devModel.matrixFormat) {
						controls.editMatrixFormat.eCombo.SelectedItem = x;
						return;
					}
				});
			} catch (Exception err) {
				string strValue;
				strValue = devModel.photosensorPixelSize.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindSensorPixelSize + strValue);
			}

			if (_devModel.is2DmarkerSupported) {
				controls.rb2d.IsChecked = _devModel.use2DMarkers;
				controls.rb1d.IsChecked = !_devModel.use2DMarkers;
			} else {
				controls.rb2d.IsEnabled = false;
				controls.rb1d.IsEnabled = false;
			}

			controls.editMatrixFormat.eCombo.SelectionChanged += new SelectionChangedEventHandler(eCombo_SelectionChanged);

			//_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			controls.SaveCancel.Cancel.IsEnabled = true;
			controls.SaveCancel.Save.CreateBinding(Button.IsEnabledProperty, devModel, x => x.isModified);
		}
		List<marker2dEditor> m2dList = new List<marker2dEditor>();
		List<marker1dEditor> m1dList = new List<marker1dEditor>();
		void RefreshMarkers() {
			m2dList.ForEach(x=>{
				if(mainGrid.Children.Contains(x))
					mainGrid.Children.Remove(x);
			});
			m1dList.ForEach(x => {
				if (mainGrid.Children.Contains(x))
					mainGrid.Children.Remove(x);
			});
			m2dList.Clear();
			m1dList.Clear();
		}
		void AddHeigthMarker(Point P1, Point P2, Size size) {
			Point nP1 = EditorConverter.StreamToScreen(P1, EditorConverter.GetVideoBounds(new Rect(0, 0, Width, Height), EditorConverter.FromWinForms(_devModel.bounds)), new Size(_devModel.encoderResolution.Width, _devModel.encoderResolution.Height));
			Point nP2 = EditorConverter.StreamToScreen(P2, EditorConverter.GetVideoBounds(new Rect(0, 0, Width, Height), EditorConverter.FromWinForms(_devModel.bounds)), new Size(_devModel.encoderResolution.Width, _devModel.encoderResolution.Height));
			if (controls.rb2d.IsChecked.Value) {
				marker2dEditor m2d = new marker2dEditor();
				m2d.Init(nP1, nP2, EditorConverter.GetVideoBounds(new Rect(0, 0, Width, Height), EditorConverter.FromWinForms(_devModel.bounds)));
				m2d.PhysicalSize = size;
				m2dList.Add(m2d);
				mainGrid.Children.Add(m2d);
				controls.MarkerSize.tbWidth.Text = size.Width.ToString();
				controls.MarkerSize.tbHeigth.Text = size.Height.ToString();
			} else {
				marker1dEditor m1d = new marker1dEditor();
				nP1.X = (nP1.X + nP2.X) / 2;
				nP2.X = nP1.X;
				m1d.Init(nP1, nP2, EditorConverter.GetVideoBounds(new Rect(0, 0, Width, Height), EditorConverter.FromWinForms(_devModel.bounds)));
				m1d.PhysicalSize = size;
				m1dList.Add(m1d);
				mainGrid.Children.Add(m1d);
				controls.MarkerSize.tbWidth.Text = size.Width.ToString();
				controls.MarkerSize.tbHeigth.Text = size.Height.ToString();
			}
			//_graphEditor.AddHeightMarker(P1, P2, size);
		}
		void LoadRegionEditor() {
			if (_devModel.region != null) {
				List<Point> plst = new List<Point>();
				_devModel.region.ForEach(x => {
					var p = new Point((int)x.X, (int)x.Y);
					Point np = EditorConverter.StreamToScreen(p, EditorConverter.GetVideoBounds(new Rect(0, 0, Width, Height), EditorConverter.FromWinForms(_devModel.bounds)), new Size(_devModel.encoderResolution.Width, _devModel.encoderResolution.Height));
					plst.Add(np);
				});
				regionEditor.Init(plst, EditorConverter.GetVideoBounds(new Rect(0, 0, Width, Height), EditorConverter.FromWinForms(_devModel.bounds)));
				//_graphEditor.AddRegionEditor(plst);
			}

			//dbg.Assert(_devModel.markers != null);
			//dbg.Assert(_devModel.markers[0].line1 != null);
		}
		void LoadMarkers() {
			if (_devModel.markers != null && _devModel.markers.Count() > 0) {
				if (_devModel.markers[0].size != null) {
					int pH = ((int)_devModel.markers[0].size.y);
					int pW = ((int)_devModel.markers[0].size.x);

					//convert from mm to cm
					pH = pH / 10;
					pW = pW / 10;

					if (_devModel.markers[0].line1 != null) {
						Point p1UL = new Point() { X = (int)_devModel.markers[0].line1.Point[1].x, Y = (int)_devModel.markers[0].line1.Point[1].y };
						Point p1BR = new Point() { X = (int)_devModel.markers[0].line1.Point[0].x, Y = (int)_devModel.markers[0].line1.Point[0].y };

						AddHeigthMarker(p1UL, p1BR, new Size(pW, pH));
					} else {
						MessageBox.Show("Marker 1 is null");
					}
					if (_devModel.markers[0].line2 != null) {
						Point p2UL = new Point() { X = (int)_devModel.markers[0].line2.Point[1].x, Y = (int)_devModel.markers[0].line2.Point[1].y };
						Point p2BR = new Point() { X = (int)_devModel.markers[0].line2.Point[0].x, Y = (int)_devModel.markers[0].line2.Point[0].y };

						AddHeigthMarker(p2UL, p2BR, new Size(pW, pH));
					} else {
						MessageBox.Show("Marker 2 is null");
					}
				} else {
					//dbg.Assert(_devModel.markers[0].size != null, "_devModel.markers[0].size == null");
				}
			}
		}

		void onExit() {
			if (controls != null)
				controls.Close();
			if (Exit != null)
				Exit();
		}
		void onCalibrate() {
			_devModel.use2DMarkers = controls.rb2d.IsChecked.Value;
			_devModel.region.Clear();
			var videoBound = EditorConverter.GetVideoBounds(new Rect(0, 0, Width, Height), EditorConverter.FromWinForms(_devModel.bounds));
			var Resolution = new Size(_devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			regionEditor.GetRegion().ForEach(x => {
				var rpt = EditorConverter.ScreenToStream(x, videoBound, Resolution);
				_devModel.region.Add(new System.Drawing.Point((int)rpt.X, (int)rpt.Y));
			});

			Point m1p1;
			Point m1p2;
			Point m2p1;
			Point m2p2;
			Size phSize;
			if (m2dList.Count == 2 || m1dList.Count ==2) {
				if (controls.rb2d.IsChecked.Value) {
					m1p1 = EditorConverter.ScreenToStream(m2dList[0].Top,videoBound, Resolution);
					m1p2 = EditorConverter.ScreenToStream(m2dList[0].Bottom,videoBound, Resolution);
					m2p1 = EditorConverter.ScreenToStream(m2dList[1].Top,videoBound, Resolution);
					m2p2 = EditorConverter.ScreenToStream(m2dList[1].Bottom,videoBound, Resolution);
					phSize = m2dList[0].PhysicalSize;
				} else {
					m1p1 = EditorConverter.ScreenToStream(m1dList[0].Top,videoBound, Resolution);
					m1p2 = EditorConverter.ScreenToStream(m1dList[0].Bottom,videoBound, Resolution);
					m2p1 = EditorConverter.ScreenToStream(m1dList[1].Top,videoBound, Resolution);
					m2p2 = EditorConverter.ScreenToStream(m1dList[1].Bottom, videoBound, Resolution);
					phSize = m1dList[0].PhysicalSize;
				}
				////convert from cm to mm
				//retval[0].pheight *= 10;
				//retval[0].pwidth *= 10;
				//retval[1].pheight *= 10;
				//retval[1].pwidth *= 10;
				_devModel.markers[0].size.y = (int)phSize.Height *10;
				_devModel.markers[0].size.x = (int)phSize.Width *10;
				_devModel.markers[0].line1.Point[0] = new global::onvif.types.Vector() { x = (float)m1p1.X, y = (float)m1p1.Y };
				_devModel.markers[0].line1.Point[1] = new global::onvif.types.Vector() { x = (float)m1p2.X, y = (float)m1p2.Y };


				_devModel.markers[0].line2.Point[0] = new global::onvif.types.Vector() { x = (float)m2p1.X, y = (float)m2p1.Y };
				_devModel.markers[0].line2.Point[1] = new global::onvif.types.Vector() { x = (float)m2p2.X, y = (float)m2p2.Y };
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
			controls.groupBox1.IsEnabled = !controls.rb2d.IsChecked.Value;
			controls.markerPhysSize.tbWidth.IsEnabled = controls.rb2d.IsChecked.Value;
		}
		void eCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			_devModel.matrixFormat = ((MatrixValue)controls.editMatrixFormat.eCombo.SelectedItem).Name;
			controls.SaveCancel.Save.IsEnabled = true;
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
