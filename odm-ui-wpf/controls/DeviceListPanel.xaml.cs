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
using System.Collections.ObjectModel;

using odm.models;
using odm.utils.extensions;
using odm.dialogs;
using odm.utils;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for DeviceListControl.xaml
	/// </summary>
	public partial class DeviceListPanel : UserControl {
		public DeviceListPanel() {
			InitializeComponent();
		}
		public DeviceListPanel(Action<DeviceDescriptionModel> itemSelected, Action refreshDevicesList, Action CreateD) {
			InitializeComponent();

			CreateDump = CreateD;
			ItemSelected = itemSelected;
			RefreshDeviceList = refreshDevicesList;

			InitializeComponent();
			InitDevicesListView();
			InitEvents();
			Localization();
		}
		protected DevicesListControlStrings strings = DevicesListControlStrings.Instance;
		public Action CreateDump;
		public Action<DeviceDescriptionModel> ItemSelected;
		public Action RefreshDeviceList;

		public void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, DevicesListControlStrings.Instance, x => x.title);
			_refreshBtn.CreateBinding(Button.ContentProperty, DevicesListControlStrings.Instance, x => x.refresh);
		}
		ContextMenu ctxMenu;
		IDisposable subscription;
		protected void InitDevicesListView() {
			deviceList.Devices = new DeviceDescriptionCollection();

			dbg.Assert(subscription == null);
			subscription = deviceList.SelectedDeviceObservable.Subscribe(sel => {
				if (sel != null) {
					if (ItemSelected != null)
						ItemSelected(sel as DeviceDescriptionModel);
				}
			});

			//Label getDump = new Label();
			//getDump.Content = "Get Dump";

			//ctxMenu = new ContextMenu();
			//ctxMenu.Items.Add(getDump);

			//_dataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(_dataGrid_LoadingRow);

			//getDump.MouseDown += new MouseButtonEventHandler(getDump_MouseDown);
		}
		GetDump dump;
		void getDump_MouseDown(object sender, MouseButtonEventArgs e) {

			//dump  = new GetDump();
			//dump.Show();
			//dump.Init(((DeviceDescriptionModel)_dataGrid.SelectedItem).session);
		}

		void _dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			//DeviceDescriptionModel itm = (DeviceDescriptionModel)devicesList.SelectedItem;
			//if (itm != null) {
			//    if (ItemSelected != null)
			//        ItemSelected(itm);
			//}
		}
		protected void InitEvents() {
			//_dataGrid.SelectionChanged += new SelectionChangedEventHandler(_dataGrid_SelectionChanged);
			//devicesList.SelectionChanged += new SelectionChangedEventHandler(_dataGrid_SelectionChanged);
		}
		public void RefreshItems() {
			//foreach (var cl in _dataGrid.Columns) { 
			//}
		}

		public void RemoveItem(IDeviceDescriptionModel devModel) {
			deviceList.Devices.Remove(devModel);
		}
		public void AddItem(IDeviceDescriptionModel devModel) {
			deviceList.Devices.Add(devModel);
			CheckDefaultSelection();
		}
		bool CheckSameSelection() {
			//if (currentSelection != null)
			//    if (_lviewDevices.SelectedItems.Count != 0) {
			//        if (_lviewDevices.SelectedItems.Contains(currentSelection))
			//            return true;
			//    }
			return false;
		}
		bool CheckDefaultSelection() {
			//if (_lviewDevices.SelectedItems.Count == 0) {
			//    if (_lviewDevices.Items.Count != 0) {
			//        _lviewDevices.TopItem.Selected = true;
			//        _lviewDevices.Items.ForEach(x => {
			//            ((ListViewItem)x).BackColor = Color.FromKnownColor(KnownColor.Window);
			//            ((ListViewItem)x).ForeColor = Color.FromKnownColor(KnownColor.WindowText);
			//        });
			//        _lviewDevices.TopItem.BackColor = Color.FromKnownColor(KnownColor.Highlight);
			//        _lviewDevices.TopItem.ForeColor = Color.FromKnownColor(KnownColor.HighlightText);
			//    }
			//    return false;
			//}
			return true;
		}

		private void _refreshBtn_Click(object sender, RoutedEventArgs e) {
			deviceList.Devices.Clear();
			if (RefreshDeviceList != null)
				RefreshDeviceList();
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
			if (subscription != null) {
				subscription.Dispose();
				subscription = null;
			}
			if (dump != null) {
				dump.Close();
			}
		}
	}
}
