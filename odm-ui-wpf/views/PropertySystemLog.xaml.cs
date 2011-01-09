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
	/// Interaction logic for PropertySystemLog.xaml
	/// </summary>
	public partial class PropertySystemLog : BasePropertyControl {
		public PropertySystemLog() {
			InitializeComponent();

			Localization();
		}
		SystemLogStrings strings = new SystemLogStrings();
		void Localization() {
			title.CreateBinding(Label.ContentProperty, strings, x => x.title);
		}
	}
}
