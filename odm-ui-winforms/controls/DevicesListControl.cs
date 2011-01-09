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
using System.Xml.XPath;
using System.Diagnostics;

using XmlExplorer.Controls;

using odm.utils.entities;
using odm.controllers;
using odm.models;
using odm.utils;

namespace odm.controls
{
    public partial class DevicesListControl : UserControl
    {
		protected DevicesListControlStrings strings = DevicesListControlStrings.Instance;
		ColumnHeaderBindable _columnHeaderName = new ColumnHeaderBindable();
		ColumnHeaderBindable _columnHeaderIP = new ColumnHeaderBindable();
		ColumnHeaderBindable _columnHeaderType = new ColumnHeaderBindable();

		public Action CreateDump;
		public Action<DeviceDescriptionModel> ItemSelected;
		public Action RefreshDeviceList;

		public DevicesListControl(Action<DeviceDescriptionModel> itemSelected, Action refreshDevicesList, Action CreateD)
        {
			CreateDump = CreateD;
			ItemSelected = itemSelected;
			RefreshDeviceList = refreshDevicesList;

            InitializeComponent();
            InitDevicesListView();
            InitEvents();
			Localization();
			_btnGetDump.Visible = false;

			DebugInfo();
        }
		[Conditional("DEBUG")]
		void DebugInfo() {
			_btnGetDump.Visible = true;
		}
		public void Localization() {
			_title.CreateBinding(x => x.Text, strings, x => x.title);
			_btnRefresh.CreateBinding(x => x.Text, strings, x => x.refresh);

			_columnHeaderName.CreateBinding(x => x.Text, strings, x => x.columnName);
			_columnHeaderIP.CreateBinding(x => x.Text, strings, x => x.columnIPadress);
			_columnHeaderType.CreateBinding(x => x.Text, strings, x => x.columnType);
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
			var settings = odm.utils.properties.Settings.Default;
            _columnHeaderName.Width = settings.DevicesListControlHeaderNameWidth;
			_columnHeaderIP.Width = settings.DevicesListControlHeaderIPWidth;
			_columnHeaderType.Width = settings.DevicesListControlHeaderTypeWidth;

            _lviewDevices.Columns.Add(_columnHeaderName);
            _lviewDevices.Columns.Add(_columnHeaderIP);
            _lviewDevices.Columns.Add(_columnHeaderType);

            _lviewDevices.View = View.Details;

			_lviewDevices.ColumnWidthChanged += new ColumnWidthChangedEventHandler(_lviewDevices_ColumnWidthChanged);
        }

		void _lviewDevices_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e) {
			odm.utils.properties.Settings.Default.DevicesListControlHeaderIPWidth = _columnHeaderIP.Width;
			odm.utils.properties.Settings.Default.DevicesListControlHeaderNameWidth = _columnHeaderName.Width;
			odm.utils.properties.Settings.Default.DevicesListControlHeaderTypeWidth = _columnHeaderType.Width;
			odm.utils.properties.Settings.Default.Save();
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
                RefreshDeviceList();
        }
        void _lviewDevices_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
			_lviewDevices.Items.ForEach(x => {
				((ListViewItem)x).BackColor = Color.FromKnownColor(KnownColor.Window);
				((ListViewItem)x).ForeColor = Color.FromKnownColor(KnownColor.WindowText);
			});
			e.Item.BackColor = Color.FromKnownColor(KnownColor.Highlight);
			e.Item.ForeColor = Color.FromKnownColor(KnownColor.HighlightText);

			if(_lviewDevices.SelectedItems.Count != 0){
				if (!CheckSameSelection()) {
					currentSelection = e.Item;
					if (ItemSelected != null)
						ItemSelected((DeviceDescriptionModel)e.Item.Tag);
				}
			}
        }
        #endregion events handlers

        #region Items list
		public void RefreshItems() {
			_lviewDevices.Items.Clear();
		}

		public void RemoveItem(DeviceDescriptionModel devModel) {
			_lviewDevices.Items.ForEach(x => {
				if (((ListViewItemBindable)x).Tag == devModel)
					((ListViewItemBindable)x).Remove();
			});
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
					_lviewDevices.Items.ForEach(x => {
						((ListViewItem)x).BackColor = Color.FromKnownColor(KnownColor.Window);
						((ListViewItem)x).ForeColor = Color.FromKnownColor(KnownColor.WindowText);
					});
					_lviewDevices.TopItem.BackColor = Color.FromKnownColor(KnownColor.Highlight);
					_lviewDevices.TopItem.ForeColor = Color.FromKnownColor(KnownColor.HighlightText);
				}
				return false;
			}
			return true;
		}
        #endregion Items list

		private void _btnGetDump_Click(object sender, EventArgs e) {
			if (CreateDump != null)
				CreateDump();
			
		}
    }
}
