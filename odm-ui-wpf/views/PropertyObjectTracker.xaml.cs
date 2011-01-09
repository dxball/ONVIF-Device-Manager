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

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyObjectTracker.xaml
	/// </summary>
	public partial class PropertyObjectTracker : BasePropertyControl {
		public PropertyObjectTracker(ObjectTrackerModel devModel) {
			InitializeComponent();
			_devModel = devModel;

			Localization();
			Binding();
			InitControls();
		}
		PropertyObjectTrackerStrings _strings = new PropertyObjectTrackerStrings();
		ObjectTrackerModel _devModel;
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
			title.CreateBinding(Label.ContentProperty, _strings, x => x.title);

			directionRose.btnAll.CreateBinding(Button.ContentProperty, _strings, x=>x.all);
			directionRose.btnNone.CreateBinding(Button.ContentProperty, _strings, x => x.none);

			indirection.CreateBinding(Label.ContentProperty, _strings, x => x.direction);

			tbAreaMax.CreateBinding(Label.ContentProperty, _strings, x => x.areaMax);
			tbAreaMin.CreateBinding(Label.ContentProperty, _strings, x => x.areaMin);
			tbContrastSensitivity.CreateBinding(Label.ContentProperty, _strings, x => x.contrast);
			tbSpeedMax.CreateBinding(Label.ContentProperty, _strings, x => x.speedMax);
			tbStabilization.CreateBinding(Label.ContentProperty, _strings, x => x.stabilization);
		}
		void Binding() {
			directionRose.IfbE = _devModel.rose_right > 0;
			directionRose.IfbN = _devModel.rose_up > 0;
			directionRose.IfbNE = _devModel.rose_up_right > 0;
			directionRose.IfbNW = _devModel.rose_up_left > 0;
			directionRose.IfbS = _devModel.rose_down > 0;
			directionRose.IfbSE = _devModel.rose_down_right > 0;
			directionRose.IfbSW = _devModel.rose_down_left > 0;
			directionRose.IfbW = _devModel.rose_left > 0;

			numAreaMax.CreateBinding(NumericUpDown.ValueProperty, _devModel, x => x.maxObjectArea, (m, v) => { 
				m.maxObjectSpeed = (int)v; 
			});
			numAreaMin.CreateBinding(NumericUpDown.ValueProperty, _devModel, x => x.minObjectArea, (m, v) => {
				m.maxObjectArea = (int)v;
			});
			numContrastSensitivity.CreateBinding(NumericUpDown.ValueProperty, _devModel, x => x.contrastSensitivity, (m, v) => {
				m.contrastSensitivity = (int)v;
			});
			numSpeedMax.CreateBinding(NumericUpDown.ValueProperty, _devModel, x => x.maxObjectSpeed, (m, v) => {
				m.maxObjectSpeed = (int)v;
			});
			numStabilization.CreateBinding(NumericUpDown.ValueProperty, _devModel, x => x.stabilizationTime, (m, v) => {
				m.stabilizationTime = (int)v;
			});
		}
		void InitControls() {
			Rect ret = new Rect(0, 0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height);
			_videoPlayer.InitPlayback(ret);

			directionRose.SelectionChanged = GetSelection;

			saveCancelControl.Save.Click += new RoutedEventHandler(Save_Click);
		}
		void GetSelection() {
			_devModel.rose_right = directionRose.IfbE ? 1 : 0;
			_devModel.rose_up = directionRose.IfbN ? 1 : 0;
			_devModel.rose_up_right = directionRose.IfbNE ? 1 : 0;
			_devModel.rose_up_left = directionRose.IfbNW ? 1 : 0;
			_devModel.rose_down = directionRose.IfbS ? 1 : 0;
			_devModel.rose_down_right = directionRose.IfbSE ? 1 : 0;
			_devModel.rose_down_left = directionRose.IfbSW ? 1 : 0;
			_devModel.rose_left = directionRose.IfbW ? 1 : 0;
		}
		void Save_Click(object sender, RoutedEventArgs e) {
			if (Save != null)
				Save();
		}
	}
}
