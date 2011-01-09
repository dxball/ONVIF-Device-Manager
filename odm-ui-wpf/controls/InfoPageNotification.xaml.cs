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

namespace odm.controls {
	/// <summary>
	/// Interaction logic for InfoPageNotification.xaml
	/// </summary>
	public partial class InfoPageNotification : BasePropertyControl {
		public InfoPageNotification() {
			InitializeComponent();

			details.Click += new RoutedEventHandler(details_Click);
		}

		void details_Click(object sender, RoutedEventArgs e) {
			if(detailsView.Visibility == System.Windows.Visibility.Hidden)
				detailsView.Visibility = System.Windows.Visibility.Visible;
			else
				detailsView.Visibility = System.Windows.Visibility.Hidden;
		}
		public enum InfoType {
			ErroreDtails,
			Other
		};
		public InfoType infotype = InfoType.Other;
		
		Action _onClickAction;
		public Action OnClickAction {
			get {
				return _onClickAction;
			}
			set {
				IsButtonVisible = true;
				_progressBar.Visibility = System.Windows.Visibility.Hidden;
				_onClickAction = value;
			}
		}
		public string Title {
			set {
				title.Content = value;
			}
		}
		public bool IsDetails {
			set {
				details.Visibility = System.Windows.Visibility.Visible;
			}
		}
		public bool IsProgressLoadingVisible {
			set {
				if (value)
					_progressBar.Visibility = System.Windows.Visibility.Visible;
				else
					_progressBar.Visibility = System.Windows.Visibility.Hidden;
			}
		}
		public bool IsButtonVisible {
			set {
				if(value)
					_btnClose.Visibility = System.Windows.Visibility.Visible;
				else
					_btnClose.Visibility = System.Windows.Visibility.Hidden;
			}
		}
		
		public string Message {
			get {
				return "info";//_infoTextBox.Text;
			}
			set {
				lText.Text = value; 
				//temp = value;
			}
		}
	}
}
