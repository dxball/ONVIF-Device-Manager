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

namespace odm.controls {
	/// <summary>
	/// Interaction logic for DeviceListControl.xaml
	/// </summary>
	public partial class DeviceListControl : UserControl {
		public DeviceListControl() {
			InitializeComponent();
		}
		public DeviceListControl(Action<DeviceDescriptionModel> itemSelected, Action refreshDevicesList, Action CreateD) {
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
			_title.CreateBinding(Title.ContentProperty, DevicesListControlStrings.Instance, x => x.title);
			_refreshBtn.CreateBinding(Button.ContentProperty, DevicesListControlStrings.Instance, x => x.refresh);

			//_columnHeaderName.CreateBinding(x => x.Text, strings, x => x.columnName);
			//_columnHeaderIP.CreateBinding(x => x.Text, strings, x => x.columnIPadress);
			//_columnHeaderType.CreateBinding(x => x.Text, strings, x => x.columnType);
		}
		ObservableCollection<DeviceDescriptionModel> _devicesCollection = new ObservableCollection<DeviceDescriptionModel>();
		ContextMenu ctxMenu;
		protected void InitDevicesListView() {
			//Name
			DataGridTextColumn dgcol = new DataGridTextColumn();
			
			//Create a context menu var cm = new ContextMenu(); cm.Items.Add(new MenutItem{Header="SampleItem"});
			dgcol.Header = strings.columnName;
			dgcol.Binding = new Binding("Name");
			_dataGrid.Columns.Add(dgcol);
			//Address
			dgcol = new DataGridTextColumn();
			dgcol.Header = strings.columnIPadress;
			dgcol.Binding = new Binding("Address");
			_dataGrid.Columns.Add(dgcol);
			//Firmware
			dgcol = new DataGridTextColumn();
			dgcol.Header = strings.columnType;
			dgcol.Binding = new Binding("Firmware");
			_dataGrid.Columns.Add(dgcol);

			_dataGrid.ItemsSource = _devicesCollection;

			Label getDump = new Label();
			getDump.Content = "Get Dump";

			ctxMenu = new ContextMenu();
			ctxMenu.Items.Add(getDump);

			_dataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(_dataGrid_LoadingRow);

			getDump.MouseDown += new MouseButtonEventHandler(getDump_MouseDown);
		}

		void _dataGrid_LoadingRow(object sender, DataGridRowEventArgs e) {
			e.Row.ContextMenu = ctxMenu;
		}

		void getDump_MouseDown(object sender, MouseButtonEventArgs e) {

			var dump = new GetDump();
			dump.Show();
			dump.Init(((DeviceDescriptionModel)_dataGrid.SelectedItem).session);
		}

		void _dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			DeviceDescriptionModel itm = (DeviceDescriptionModel)_dataGrid.SelectedItem;
			if(itm != null){
			if (ItemSelected != null)
				ItemSelected(itm);
			}
		}
		protected void InitEvents() {
			_dataGrid.SelectionChanged += new SelectionChangedEventHandler(_dataGrid_SelectionChanged);
		}
		public void RefreshItems() {
			foreach (var cl in _dataGrid.Columns) { 
			}
		}

		public void RemoveItem(DeviceDescriptionModel devModel) {
			_devicesCollection.Remove(devModel);
		}
		public void AddItem(DeviceDescriptionModel devModel) {
			_devicesCollection.Add(devModel);
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
			_devicesCollection.Clear();
			if (RefreshDeviceList != null)
				RefreshDeviceList();
		}
	}
}
