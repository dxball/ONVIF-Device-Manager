using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace odm.controls {
	public class FloatTrackBar : TrackBar, INotifyPropertyChanged {
		public FloatTrackBar():base(){
			base.ValueChanged += new EventHandler(FloatTrackBar_ValueChanged);
		}

		void FloatTrackBar_ValueChanged(object sender, EventArgs e) {
			NotifyPropertyChanged("fValue");
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String info) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		public float fMinimum {
			get {
				return (float)base.Minimum;
			}
			set {
				base.Minimum = (int)value;
			}
		}
		public float fMaximum {
			get {
				return (float)base.Maximum;
			}
			set {
				base.Maximum = (int)value;
			}
		}
		public float fValue {
			get {
				return (float)base.Value;
			}
			set {
				base.Value = (int)value;
			}
		}
	}
}
