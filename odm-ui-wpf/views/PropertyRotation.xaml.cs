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
using odm.models;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyRotation.xaml
	/// </summary>
	public partial class PropertyRotation : BasePropertyControl {
		public PropertyRotation(AnnotationsModel devModel) {
			InitializeComponent();
		}

		public override void ReleaseAll() {
			//_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
	}
}
