using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using odm.controllers;
using odm.utils;

namespace odm.controls {
	public partial class DeviceEventsControl : BasePropertyControl{
		CommonEventsStrings _strings = new CommonEventsStrings();
		public override void ReleaseUnmanaged() { }
		public DeviceEventsControl()
        {
            InitializeComponent();
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
			//lvItem.CreateBinding(x => x.Text, evDescr, x => x.id);

			var lvSubItemDate = new ListViewSubItemBindeble();
			lvSubItemDate.Text = evDescr.Date;
			//lvSubItemDate.CreateBinding(x => x.Text, evDescr, x => x.datetime);

			var lvSubItemType = new ListViewSubItemBindeble();
			lvSubItemType.Text = evDescr.Type;
			//lvSubItemType.CreateBinding(x => x.Text, evDescr, x => x.type);

			var lvSubItemDetails = new ListViewSubItemBindeble();
			lvSubItemDetails.Text = evDescr.Details;
			//lvSubItemDetails.CreateBinding(x => x.Text, evDescr, x => x.details);

			lvItem.SubItems.Add(lvSubItemDate);
			lvItem.SubItems.Add(lvSubItemType);
			lvItem.SubItems.Add(lvSubItemDetails);

			_lviewEvents.Items.Add(lvItem);
        }
    }
}
