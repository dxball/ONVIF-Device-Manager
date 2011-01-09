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

using odm.utils.extensions;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyTamperingDetectors.xaml
	/// </summary>
	public partial class PropertyTamperingDetectors : BasePropertyControl {
		public PropertyTamperingDetectors(AnnotationsModel devModel) {
			InitializeComponent();

			Localisation();
		}
		public Action Save;
		public Action Cancel;
		PropertyTamperingDetectorsStrings _strings = new PropertyTamperingDetectorsStrings();

		void Localisation() {
			title.CreateBinding(Label.ContentProperty, _strings, x => x.title);
			cameraDisplacedCaption.CreateBinding(CheckBox.ContentProperty, _strings, x => x.cameraDisplaced);
			fildOfViewCaption.CreateBinding(CheckBox.ContentProperty, _strings, x => x.fieldObstructed);
			globalCaption.CreateBinding(CheckBox.ContentProperty, _strings, x => x.globalChange);
			outFocusCaption.CreateBinding(CheckBox.ContentProperty, _strings, x => x.outFocus);
			sceneBrightCaption.CreateBinding(CheckBox.ContentProperty, _strings, x => x.sceneBright);
			sceneDarkCaption.CreateBinding(CheckBox.ContentProperty, _strings, x => x.sceneDark);
			sceneNoiseCaption.CreateBinding(CheckBox.ContentProperty, _strings, x => x.sceneNoisy);
			signalLostCaption.CreateBinding(CheckBox.ContentProperty, _strings, x => x.signalLost);
		}

		public override void ReleaseAll() {
			//_videoPlayer.ReleaseAll();
			base.ReleaseAll();
		}
	}
}
