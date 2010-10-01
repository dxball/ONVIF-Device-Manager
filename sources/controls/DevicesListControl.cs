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
//
//----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using nvc.entities;
using nvc.controllers;

namespace nvc.controls
{
    public delegate void DeviceListItemSelectedDelegate(string ID);
    public partial class DevicesListControl : UserControl
    {
        public event DeviceListItemSelectedDelegate _onDeviceItemSelected;
        public event EventHandler RefreshDeviceList;

		ColumnHeaderBindable _columnHeaderName = new ColumnHeaderBindable();
		ColumnHeaderBindable _columnHeaderIP = new ColumnHeaderBindable();
		ColumnHeaderBindable _columnHeaderType = new ColumnHeaderBindable();

        public DevicesListControl()
        {
            InitializeComponent();
            InitDevicesListView();
            InitEvents();
			Localisation();
        }

		public void Localisation() {
			_title.DataBindings.Add(new Binding("Text", Constants.Instance, "sDevicesListControlTitle"));
			_btnRefresh.DataBindings.Add(new Binding("Text", Constants.Instance, "sDevicesListControlRefresh"));

			_columnHeaderName.DataBindings.Add(new Binding("Text", Constants.Instance, "sDevicesListControlColumnName"));
			_columnHeaderIP.DataBindings.Add(new Binding("Text", Constants.Instance, "sDevicesListControlColumnIPadress"));
			_columnHeaderType.DataBindings.Add(new Binding("Text", Constants.Instance, "sDevicesListControlColumnType"));
		}
        #region Iitialisation
        protected void InitDevicesListView()
        {
            // Colors
            Color bckColor = ColorDefinition.colControlBackground;
            BackColor = bckColor;
            _title.BackColor = ColorDefinition.colTitleBackground;
            _btnRefresh.BackColor = bckColor;

            //Controls
            _columnHeaderName.Width = Defaults.iDevicesListControlHeaderNameWidth;            
            _columnHeaderIP.Width = Defaults.iDevicesListControlHeaderIPWidth;            
            _columnHeaderType.Width = Defaults.iDevicesListControlHeaderTypeWidth;

            _lviewDevices.Columns.Add(_columnHeaderName);
            _lviewDevices.Columns.Add(_columnHeaderIP);
            _lviewDevices.Columns.Add(_columnHeaderType);

            _lviewDevices.View = View.Details;
        }
        protected void InitEvents()
        {
            _lviewDevices.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(_lviewDevices_ItemSelectionChanged);
        }
        #endregion Iitialisation

        #region events handlers
        private void _btnRefresh_Click(object sender, EventArgs e)
        {
            ClearItems();
            if (RefreshDeviceList != null)
                RefreshDeviceList(sender, e);
        }
        void _lviewDevices_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
			if(_lviewDevices.SelectedItems.Count != 0){
				if (!CheckSameSelection()) {
					currentSelection = e.Item;
					if (_onDeviceItemSelected != null)
						_onDeviceItemSelected(e.Item.SubItems[1].Text);
				}
			}
        }
        #endregion events handlers

        #region Items list
		public void SelectItem(DeviceModelInfo devInfo) {
			ListViewItem item;
			if (_lviewDevices.Items.Find(devInfo.DeviceId, false).Any()) {
				item = _lviewDevices.Items.Find(devInfo.DeviceId, false).FirstOrDefault();
				item.Selected = true;
			}
		}
		public void RemoveItem(DeviceModelInfo devInfo) {
			ListViewItem item;
			if (_lviewDevices.Items.Find(devInfo.DeviceId, false).Any()) {
				item = _lviewDevices.Items.Find(devInfo.DeviceId, false).FirstOrDefault();
				_lviewDevices.Items.Remove(item);
			}
		}
        public void AddItem(DeviceModelInfo devInfo, bool isCheck){
			ListViewItem lvItem = new ListViewItem();
			lvItem.Tag = devInfo;
			lvItem.Name = devInfo.DeviceId;
			lvItem.Text = devInfo.Name;

			var lvSubItemIP = new System.Windows.Forms.ListViewItem.ListViewSubItem();
			lvSubItemIP.Text = devInfo.IpAddress;

			var lvSubItemType = new System.Windows.Forms.ListViewItem.ListViewSubItem();
			lvSubItemType.Text = devInfo.Firmware;

			lvItem.SubItems.Add(lvSubItemIP);
			lvItem.SubItems.Add(lvSubItemType);

			_lviewDevices.Items.Add(lvItem);
			if(isCheck)
				CheckDefaultSelection();
        }
		ListViewItem currentSelection;
		bool CheckSameSelection() {
			if(currentSelection != null)
				if (_lviewDevices.SelectedItems.Count != 0) {
					if(_lviewDevices.SelectedItems.Contains(currentSelection))
						return true;
				}
			return false;
		}
		bool CheckDefaultSelection() {
			if (_lviewDevices.SelectedItems.Count == 0) {
				if(_lviewDevices.Items.Count != 0){
					_lviewDevices.TopItem.Selected = true;
				}
				return false;
			}
			return true;
		}
        private void ClearItems()
        {
            _lviewDevices.Items.Clear();
        }
        #endregion Items list

		Action<Uri> _addAction;
		public void SubscribeToManualAdding(Action<Uri> addAction) {
			_addAction = addAction;
		}

		private void _btnAddNew_Click(object sender, EventArgs e) {
			var button = ((Button)sender);
			var addForm = new AddDeviceForm();

			if (addForm.ShowDialog(this) == DialogResult.OK) {
				_addAction(addForm.DeviceURI);
			};

		}
    }
}
