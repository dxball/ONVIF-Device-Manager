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
	/// Interaction logic for DeviceChannelControl.xaml
	/// </summary>
	public partial class DeviceChannelControl : UserControl {
		public DeviceChannelControl(ChannelDescription channel) {
			InitializeComponent();
			_devChannel = channel;

			InitControls();
		}
		public ChannelDescription _devChannel;
		public Action<ChannelDescription> ChannelSelected;
		List<LinkCheckButton> _lBtnsList = new List<LinkCheckButton>();
	
		public void InitControls() {
			SetChannelName(_devChannel.Name);

			//SetUnActiveColors();

			//InitForSelectionEvents();

			//_imgBox.Image = _devChannel.snapshot;
		}

		public void AddLinkButton(LinkCheckButton lbtn) {
			//lbtn.eMouseEnter += new EventHandler(DeviceControl_MouseEnter);
			//lbtn.eMouseLeave += new EventHandler(DeviceControl_MouseLeave);
			_lBtnsList.Add(lbtn);

			_channelLinksPanel.Children.Add(lbtn);
		}
		public void SetChannelImage(System.Drawing.Bitmap img) {
			if (img != null)
				_img.Source = ImageConversion.ToImageSource(img);
			//else
			//    _imgBox.Image = odm.utils.properties.Resources.imageError;
		}
		public void SetChannelName(string name) {
			//title.label.Content = name;
			title.Content = name;
		}
		public void ResetLinkSelection() {
			_lBtnsList.ForEach(x => { ((LinkCheckButton)x).SetUnclicked(); });
		}
	}
}
