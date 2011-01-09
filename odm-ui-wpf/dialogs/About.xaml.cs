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

using odm.controls;
using System.Diagnostics;

namespace odm {
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class About : CustomDialogWindow {
		public About() {
			InitializeComponent();
		}

		private void HandleRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
			var hl = sender as Hyperlink;
			//if (hl == null) {
			//    return;
			//}
			string navigateUri = hl.NavigateUri.ToString();
			// if the URI somehow came from an untrusted source, make sure to
			// validate it before calling Process.Start(), e.g. check to see
			// the scheme is HTTP, etc.
			Process.Start(new ProcessStartInfo(navigateUri));
			e.Handled = true;
		}

		
	}
}
