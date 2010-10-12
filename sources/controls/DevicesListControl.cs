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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using nvc.entities;
using nvc.controllers;
using nvc.models;

namespace nvc.controls
{
	public delegate void DeviceListItemSelectedDelegate(DeviceDescriptionModel devModel);
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
			Localization();
        }

		public void Localization() {
			_title.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sDevicesListControlTitle);
			_btnRefresh.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sDevicesListControlRefresh);

			_columnHeaderName.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sDevicesListControlColumnName);
			_columnHeaderIP.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sDevicesListControlColumnIPadress);
			_columnHeaderType.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sDevicesListControlColumnType);
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
            if (RefreshDeviceList != null)
                RefreshDeviceList(sender, e);
        }
        void _lviewDevices_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
			if(_lviewDevices.SelectedItems.Count != 0){
				if (!CheckSameSelection()) {
					currentSelection = e.Item;
					if (_onDeviceItemSelected != null)
						_onDeviceItemSelected((DeviceDescriptionModel)e.Item.Tag);
				}
			}
        }
        #endregion events handlers

        #region Items list
		public void RefreshItems() {
			_lviewDevices.Items.Clear();
		}

		public void RemoveItem(DeviceDescriptionModel devModel) {
			ListViewItem[] lvitemsarr = new ListViewItem[_lviewDevices.Items.Count];
			_lviewDevices.Items.CopyTo(lvitemsarr, 0);
			ListViewItem item = lvitemsarr.Where(x => x.Tag == devModel).FirstOrDefault();
			if(item != null){
				_lviewDevices.Items.Remove(item);
			}
		}
        public void AddItem(DeviceDescriptionModel devModel){
			ListViewItemBindable lvItem = new ListViewItemBindable();


			lvItem.Tag = devModel;
			lvItem.CreateBinding(x => x.Text, devModel, x => x.Name);

			var lvSubItemIP = new ListViewSubItemBindeble();
			lvSubItemIP.CreateBinding(x=>x.Text, devModel,  x=>x.Address);

			var lvSubItemType = new ListViewSubItemBindeble();
			lvSubItemType.CreateBinding(x=>x.Text, devModel,  x=>x.Firmware);

			lvItem.SubItems.Add(lvSubItemIP);
			lvItem.SubItems.Add(lvSubItemType);

			_lviewDevices.Items.Add(lvItem);

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
        #endregion Items list
    }
}
