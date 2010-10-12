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
using nvc.controllers;

namespace nvc.controls
{
    public partial class PropertyEvents : BasePropertyControl
    {
        public PropertyEvents()
        {
            InitializeComponent();

            InitConrols();
        }
		ColumnHeader _columnHeaderId = new ColumnHeader();
		ColumnHeader _columnHeaderDate = new ColumnHeader();
		ColumnHeader _columnHeaderType = new ColumnHeader();
		ColumnHeader _columnHeaderDetails = new ColumnHeader();
        //IEntities Entity = WorkflowController.Instance;
		void Localization() {
			_title.CreateBinding(x=>x.Text, Constants.Instance, x=>x.sPropertyEventsTitle);
			_lviewEvents.DataBindings.Add(new Binding("Columns[0].Text", Constants.Instance, "sPropertyEventsColumnID"));
			_lviewEvents.DataBindings.Add(new Binding("Columns[1].Text", Constants.Instance, "sPropertyEventsColumnType"));
			_lviewEvents.DataBindings.Add(new Binding("Columns[2].Text", Constants.Instance, "sPropertyEventsColumnDate"));
			_lviewEvents.DataBindings.Add(new Binding("Columns[3].Text", Constants.Instance, "sPropertyEventsColumnDetails"));
		}

        protected void InitConrols()
        {
			InitTable();
			FillListView();

			Localization();

            _imgBox.SizeMode = PictureBoxSizeMode.Zoom;
            //_imgBox.Image;// = WorkflowController.Instance.GetCurrentDevice().GetDeviceImage();
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
        }
        protected void FillListView()
        {
            //foreach (var item in Entity.GetCurrentDevice().CurrentChannel.GetEventsList())
            //{
            //    AddListItem(item);
            //}
        }
        private void AddListItem(EventDescriptor eventDescr)
        {
			//ListViewItem lvItem = new ListViewItem();
			//lvItem.Text = eventDescr.RuleID.ToString();

			//var lvSubItemDate = new System.Windows.Forms.ListViewItem.ListViewSubItem();
			//lvSubItemDate.Text = eventDescr.TimeDate.ToString();

			//var lvSubItemType = new System.Windows.Forms.ListViewItem.ListViewSubItem();
			//lvSubItemType.Text = eventDescr.Type;

			//var lvSubItemDetails = new System.Windows.Forms.ListViewItem.ListViewSubItem();
			//lvSubItemDetails.Text = eventDescr.Details;

			//lvItem.SubItems.Add(lvSubItemDate);
			//lvItem.SubItems.Add(lvSubItemType);
			//lvItem.SubItems.Add(lvSubItemDetails);

			//_lviewEvents.Items.Add(lvItem);
        }
    }
}
