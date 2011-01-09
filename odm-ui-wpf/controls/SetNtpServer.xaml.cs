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

using odm.utils.extensions;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for SetNtpServer.xaml
	/// </summary>
	public partial class SetNtpServer : CustomDialogWindow {
		public SetNtpServer() {
			InitializeComponent();
			Localization();

			setButton.Click += new RoutedEventHandler(setButton_Click);
		}

		void setButton_Click(object sender, RoutedEventArgs e) {
			if (setServer != null)
				setServer(ntpAddress.Text);
		}

		public Action<string> setServer { get; set; }

		void Localization() {
			this.Title = PropertyTimeZoneStrings.Instance.ntpServerSetupTitle;
			setButton.CreateBinding(Button.ContentProperty, PropertyTimeZoneStrings.Instance, x => x.ntpServerSetupSet);
		}
	}
}
