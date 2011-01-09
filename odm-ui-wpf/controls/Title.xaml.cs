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
	/// Interaction logic for Title.xaml
	/// </summary>
	public partial class Title : ContentControl {
		public Title() {
			InitializeComponent();
		}
		//bool m_contentLoaded = false;
		//public void InitializeComponent() {
		//    if (!m_contentLoaded) {
		//        m_contentLoaded = true;
		//        Uri resourceLocator = new Uri("/odm;component/controls/title.xaml", UriKind.Relative);
		//        Application.LoadComponent(this, resourceLocator);
		//    }
		//}
	}
}
