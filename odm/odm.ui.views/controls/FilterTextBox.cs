using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using utils;

namespace odm.ui.controls {
	public class FilterTextBox :TextBox, INotifyPropertyChanged{
		public FilterTextBox() {
			//this.CreateBinding(FilterTextBox.DefaultCaptionProperty, LocalDeviceList.instance, x => x.search);
			DefaultCaption = "dddddddddddddddd";
		}

		public string DefaultCaption {get { return (string)GetValue(DefaultCaptionProperty); }set { SetValue(DefaultCaptionProperty, value); }}
		public static readonly DependencyProperty DefaultCaptionProperty = DependencyProperty.Register("DefaultCaption", typeof(string), typeof(FilterTextBox));

		private void NotifyPropertyChanged(String info) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
