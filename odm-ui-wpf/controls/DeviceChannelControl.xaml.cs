#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

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
using odm.controllers;
using onvif;
using odm.utils.extensions;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for DeviceChannelControl.xaml
	/// </summary>
	public partial class DeviceChannelControl : UserControl {
		public DeviceChannelControl(VideoSourceToken chantoken) {
			InitializeComponent();
			_chanhelToken = chantoken;
			infoCaption.CreateBinding(TextBlock.TextProperty, CommonApplicationStrings.Instance, x => x.loadingData);
			//InitControls();
		}
		public VideoSourceToken _chanhelToken;
		public ChannelModel _devChannel;
		public Action<ChannelModel> ChannelSelected;
		List<LinkCheckButton> _lBtnsList = new List<LinkCheckButton>();

		public void InitControls(ChannelModel model) {
			_devChannel = model;
			infoBar.Visibility = System.Windows.Visibility.Collapsed;
			content.Visibility = System.Windows.Visibility.Visible;
			SetChannelName(_devChannel.name);

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
			title.Title = name;
			//title.Content = name;
		}
		public void ResetLinkSelection() {
			_lBtnsList.ForEach(x => { ((LinkCheckButton)x).SetUnclicked(); });
		}
	}
}