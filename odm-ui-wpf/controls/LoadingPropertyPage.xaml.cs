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

using odm.utils.extensions;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for LoadingPropertyPage.xaml
	/// </summary>
	public partial class LoadingPropertyPage : BasePropertyControl {
		public LoadingPropertyPage() {
			InitializeComponent();

			Localization();
		}
		CommonApplicationStrings _strings = new CommonApplicationStrings();

		void Localization() {
			title.CreateBinding(Title.ContentProperty, _strings, x => x.loadingData);
		}
	}
}
