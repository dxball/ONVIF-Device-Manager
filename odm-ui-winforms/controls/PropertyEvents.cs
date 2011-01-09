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
using odm.controllers;
using odm.models;
using System.Collections;

using odm.utils;

namespace odm.controls
{
    public partial class PropertyEvents : BasePropertyControl{
		PropertyChannelEventsStrings _strings = new PropertyChannelEventsStrings();
		public override void ReleaseUnmanaged() { }
		public PropertyEvents()
        {
            InitializeComponent();

			Load += new EventHandler(PropertyEvents_Load);
        }

		void PropertyEvents_Load(object sender, EventArgs e) {
			InitConrols();
		}
		public Action AddEvent;

		ColumnHeaderBindable _columnHeaderId = new ColumnHeaderBindable();
		ColumnHeaderBindable _columnHeaderDate = new ColumnHeaderBindable();
		ColumnHeaderBindable _columnHeaderType = new ColumnHeaderBindable();
		ColumnHeaderBindable _columnHeaderDetails = new ColumnHeaderBindable();

        //IEntities Entity = WorkflowController.Instance;
		
		void Localization() {
			_title.CreateBinding(x => x.Text, _strings, x => x.title);

			_columnHeaderId.CreateBinding(x => x.Text, _strings, x => x.eventID);
			_columnHeaderDate.CreateBinding(x => x.Text, _strings, x => x.dateTime);
			_columnHeaderType.CreateBinding(x => x.Text, _strings, x => x.type);
			_columnHeaderDetails.CreateBinding(x => x.Text, _strings, x => x.details);
		}

        protected void InitConrols()
        {
			InitTable();
			Localization();
			//FillListView();

            _imgBox.SizeMode = PictureBoxSizeMode.Zoom;

			_lviewEvents.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(_lviewEvents_ItemSelectionChanged);
        }

		void _lviewEvents_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			if (e.Item != null)
				if (e.Item.Tag != null) {
					_imgBox.DataBindings.Clear();
					try {
						_imgBox.CreateBinding(x => x.Image, (odm.controllers.EventDescriptor)e.Item.Tag, x => x.Screen);
					} catch (Exception err) {
						BindingError(err, ExceptionStrings.Instance.errBindEventSnapShot);
					}
				}
		}
        protected void InitTable()
        {
			_columnHeaderId.Width = Defaults.iPropertyEventsHeaderIDWidth;            
			_columnHeaderDate.Width = Defaults.iPropertyEventsHeaderDateWidth;
			_columnHeaderType.Width = Defaults.iPropertyEventsHeaderTypeWidth;
			_columnHeaderDetails.Width = Defaults.iPropertyEventsHeaderDetailsWidth;

            _lviewEvents.Columns.Add(_columnHeaderId);
            _lviewEvents.Columns.Add(_columnHeaderDate);
            _lviewEvents.Columns.Add(_columnHeaderType);
            _lviewEvents.Columns.Add(_columnHeaderDetails);

			_lviewEvents.View = View.Details;

			_lviewEvents.ColumnClick += new ColumnClickEventHandler(_lviewEvents_ColumnClick);
        }

		void _lviewEvents_ColumnClick(object sender, ColumnClickEventArgs e) {
			var columnNmber = e.Column;
			_lviewEvents.ListViewItemSorter = new ListViewItemComparer(e.Column);
		}
        public void FillListView(List<odm.controllers.EventDescriptor> lstEvents)
        {
			_lviewEvents.Items.Clear();
			lstEvents.ForEach(x => { AddListItem(x); });
        }
		public void RemoveListViewItem(odm.controllers.EventDescriptor evDescr) {
			_lviewEvents.Items.ForEach(itm => { if (((ListViewItemBindable)itm).Tag == evDescr) ((ListViewItemBindable)itm).Remove(); });
		}
		public void AddListItem(odm.controllers.EventDescriptor evDescr)
        {
			ListViewItemBindable lvItem = new ListViewItemBindable();
			lvItem.Tag = evDescr;
			lvItem.Text = evDescr.EventID;

			var lvSubItemDate = new ListViewSubItemBindeble();
			lvSubItemDate.Text = evDescr.Date;

			var lvSubItemType = new ListViewSubItemBindeble();
			lvSubItemType.Text = evDescr.Type;

			var lvSubItemDetails = new ListViewSubItemBindeble();
			lvSubItemDetails.Text = evDescr.Details;

			lvItem.SubItems.Add(lvSubItemDate);
			lvItem.SubItems.Add(lvSubItemType);
			lvItem.SubItems.Add(lvSubItemDetails);

			_lviewEvents.Items.Add(lvItem);
        }
    }
	// Implements the manual sorting of items by columns.
	class ListViewItemComparer : IComparer {
		private int col;
		public ListViewItemComparer() {
			col = 0;
		}
		public ListViewItemComparer(int column) {
			col = column;
		}
		public int Compare(object x, object y) {
			return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
		}
	}
}
