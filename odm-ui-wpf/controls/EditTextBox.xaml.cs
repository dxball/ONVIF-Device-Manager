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
	/// Interaction logic for EditTextBox.xaml
	/// </summary>
	public partial class EditTextBox : UserControl {
		public EditTextBox() {
			InitializeComponent();
		}
		public TextBlock lName {
			get {
				return _label;
			}
		}
		public TextBox eText {
			get {
				return _textbox;
			}
		}
	}
}
