using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using odm.utils;
using System.Configuration;
using odm.config;

namespace odm.utils {
	public partial class ODMLogger : Form {
		public ODMLogger(List<LogMessage> msgList) {
			InitializeComponent();

			InitControl(msgList);

			//dataGridView1.SetDoubleBuffered(true);

			Load += new EventHandler(ODMLogger_Load);

			//dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
			//dataGridView1.MouseClick += new MouseEventHandler(dataGridView1_MouseClick);
			
			FormClosing += new FormClosingEventHandler(ODMLogger_FormClosing);
		}

		void ODMLogger_Load(object sender, EventArgs e) {
		}
		LoggerConfig _lc;
		LoggerConfig lc {
			get { 
				if(_lc == null)
					_lc = LoggerConfigAPI.Load();
				return _lc;
			}
		}
		void dataGridView1_MouseClick(object sender, MouseEventArgs e) {
			//if (e.Button == System.Windows.Forms.MouseButtons.Left) {
			//    selectedMeggage = (LogMessage)dataGridView1.CurrentRow.Tag;
			//    tabControl1.TabPages.Clear();
			//    InitTabPages(selectedMeggage);
			//} else {
			//    selectedMeggage = null;
			//    tabControl1.TabPages.Clear();

			//    dataGridView1.ClearSelection();
			//    //InitTabPages(selectedMeggage);
			//}
		}
		public MainWindow _parent;
		LogMessage selectedMeggage;
		void dataGridView1_SelectionChanged(object sender, EventArgs e) {
		//    selectedMeggage = (LogMessage)dataGridView1.CurrentRow.Tag;
		//    tabControl1.TabPages.Clear();
		//    InitTabPages(selectedMeggage);
		}
		void InitTabPages(LogMessage msg) {
			lc.tabs.ForEach<LoggerTabConfig>(tab => {
				TabPage tp = new TabPage();
				tp.Text = tab.name;
				
				ODMLoggerPage loggerPage = new ODMLoggerPage("dd") { Dock = DockStyle.Fill };
				tp.Controls.Add(loggerPage);
				tabControl1.TabPages.Add(tp);
			});
		}

		void ODMLogger_FormClosing(object sender, FormClosingEventArgs e) {
			_parent.AddMessageToUI = null;
			_parent.RemoveMessageFromUI = null;
		}
		public void AddMessage(LogMessage msg) {
			//DataGridViewRow row = new DataGridViewRow();
			

			//row.Tag = msg;

			//Color TextColor = Color.Black;
			//Color Backgr = Color.White;
			//bool isBold = false;

			//if (msg.eventType == System.Diagnostics.TraceEventType.Error) {
			//    TextColor = Color.Red;
			//    isBold = true;
			//} else if (msg.eventType == System.Diagnostics.TraceEventType.Warning) {
			//    TextColor = Color.YellowGreen;
			//    isBold = true;
			//}
			
			//lc.columns.ForEach(col=>{
			//    var ret = msg.EvalXPath(col.xpath, col.xmlns);
			//    var cell = new DataGridViewTextBoxCell() { Value = ret };
				
			//    row.Cells.Add(cell);
			//});
			//row.DefaultCellStyle.ForeColor = TextColor;
			//if (isBold) {
			//    Font fnt = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
			//    row.DefaultCellStyle.Font = fnt;
			//}

			//dataGridView1.SuspendLayout();
			
			//dataGridView1.Rows.Add(row);

			//if (dataGridView1.SelectedRows.Count == 0) {
			//    int lastrows = dataGridView1.RowCount - dataGridView1.DisplayedRowCount(true);
			//    if (lastrows > 0) {
			//        if (lastrows == 1)
			//            lastrows = 0;
			//        dataGridView1.FirstDisplayedScrollingRowIndex = lastrows;
			//    }
			//}
			////RecolorGrid(row);
			//dataGridView1.ResumeLayout();
		}
		void RecolorGrid(DataGridViewRow row) {
			//int res;
			//Math.DivRem(dataGridView1.RowCount, 2, out res);
			//if(res == 0)
			//    row.DefaultCellStyle.BackColor = Color.LightCyan;
		}
		public void RemoveMessage(LogMessage msg) {
			//dataGridView1.Rows.RemoveAt(0);
			//dataGridView1.Rows.ForEach(x => {
			//    if (((DataGridViewRow)x).Tag == msg)
			//        dataGridView1.Rows.Remove((DataGridViewRow)x);
			//    return;
			//});
		}

		void CreateColumn(string name) {
			//DataGridViewColumn dCol = new DataGridViewTextBoxColumn();
			//dCol.SortMode = DataGridViewColumnSortMode.NotSortable;
			//dCol.Name = name;
			//dataGridView1.Columns.Add(dCol);
		}

		void InitControl(List<LogMessage> msgList) {
			lc.columns.ForEach(col => {
				CreateColumn(col.name);
			});

			msgList.ForEach(msg => AddMessage(msg));
		}
	}
}
