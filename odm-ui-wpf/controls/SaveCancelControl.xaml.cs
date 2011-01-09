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
	/// Interaction logic for SaveCancelControl.xaml
	/// </summary>
	public partial class SaveCancelControl : UserControl {
		public SaveCancelControl() {
			InitializeComponent();
			btnSave.CreateBinding(Button.ContentProperty, SaveCancelStrings.Instance, x => x.save);
			btnCancel.CreateBinding(Button.ContentProperty, SaveCancelStrings.Instance, x => x.cancel);
		}
		public Button Save { get { return btnSave; } }
		public Button Cancel { get { return btnCancel; } }
	}
}
