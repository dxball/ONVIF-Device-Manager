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

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyDepthCalibrationSTAcontrol.xaml
	/// </summary>
	public partial class PropertyDepthCalibrationSTAcontrol : CustomDialogWindow {
		public PropertyDepthCalibrationSTAcontrol(Action exit, Action calibrate) {
			InitializeComponent();
			Calibrate = calibrate;
			Exit = exit;
			_saveCancelControl.Cancel.Click += new RoutedEventHandler(Cancel_Click);
			_saveCancelControl.Save.Click += new RoutedEventHandler(Save_Click);

			Closed += new EventHandler(PropertyDepthCalibrationSTAcontrol_Closed);
		}

		void PropertyDepthCalibrationSTAcontrol_Closed(object sender, EventArgs e) {
			if (Exit != null)
				Exit();
		}

		public Action Exit;
		public Action Calibrate;

		public MarkerPhysSize MarkerSize {
			get {
				return markerPhysSize;
			}
		}

		public SaveCancelControl SaveCancel {
			get {
				return _saveCancelControl;
			}
		}
		public EditComboBox EditMatrix {
			get {
				return editMatrixFormat;
			}
		}
		public EditTextBox EditFocalL {
			get {
				return editFocalLength;
			}
		}
		public RadioButton rb2D {
			get {
				return rb2d;
			}
		}
		public RadioButton rb1D {
			get {
				return rb1d;
			}
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			if (Calibrate != null)
				Calibrate();
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			if (Exit != null)
				Exit();
		}
	}
}
