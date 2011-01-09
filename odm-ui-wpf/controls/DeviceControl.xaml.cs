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
	/// Interaction logic for DeviceControl.xaml
	/// </summary>
	public partial class DeviceControl : UserControl {
		public DeviceControl(DeviceCapabilityModel devModel, ImageSource devImg) {
			InitializeComponent();

			SetControlName(devModel.devInfo.Name);
			SetControlImage(devImg);
		}

		List<LinkCheckButton> _lBtnsList = new List<LinkCheckButton>();

		public void SetControlImage(ImageSource devImg) {
			_deviceImage.Source = devImg;
		}
		public void SetControlName(string name) {
			title.Content = name;
		}

		public void AddLinkButton(LinkCheckButton lbtn) {
			//lbtn.eMouseEnter += new EventHandler(DeviceControl_MouseEnter);
			//lbtn.eMouseLeave += new EventHandler(DeviceControl_MouseLeave);
			_lBtnsList.Add(lbtn);

			_deviceLinksPanel.Children.Add(lbtn);
		}
		public void ResetLinkSelection() {
			_lBtnsList.ForEach(x => { ((LinkCheckButton)x).SetUnclicked(); });
		}
	}
}
