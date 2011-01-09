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
	/// Interaction logic for PropertyMetadata.xaml
	/// </summary>
	public partial class PropertyMetadata : BasePropertyControl {
		public PropertyMetadata() {
			InitializeComponent();
			Localization();
		}
		void Localization() {
			title.CreateBinding(Title.ContentProperty, PropertyMetadataStrings.Instance, x => x.title);
		}
		public void AppendData(string data) {
			if (metaBox.Text.Length > 19048)
				metaBox.Text = "";
			metaBox.Text += metaBox.Text + data;
		}
	}
}
