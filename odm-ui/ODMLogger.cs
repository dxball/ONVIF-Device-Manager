using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using onvifdm.utils;
using System.Configuration;
using onvifdm.config;

namespace nvc {
	public partial class ODMLogger : Form {
		public ODMLogger(List<LogMessage> msgList) {
			InitializeComponent();

			InitControl(msgList);

			dataGridView1.SetDoubleBuffered(true);

			//dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
			dataGridView1.MouseClick += new MouseEventHandler(dataGridView1_MouseClick);
			
			FormClosing += new FormClosingEventHandler(ODMLogger_FormClosing);
		}

		void dataGridView1_MouseClick(object sender, MouseEventArgs e) {
			selectedMeggage = (LogMessage)dataGridView1.CurrentRow.Tag;
			tabControl1.TabPages.Clear();
			InitTabPages(selectedMeggage);
		}
		public MainWindow _parent;
		LogMessage selectedMeggage;
		void dataGridView1_SelectionChanged(object sender, EventArgs e) {
			selectedMeggage = (LogMessage)dataGridView1.CurrentRow.Tag;
			tabControl1.TabPages.Clear();
			InitTabPages(selectedMeggage);
		}
		void InitTabPages(LogMessage msg) {
			LoggerConfig lc = LoggerConfigAPI.Load();
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
			DataGridViewRow row = new DataGridViewRow();
			LoggerConfig lc = LoggerConfigAPI.Load();

			row.Tag = msg;

			Color TextColor = Color.Black;
			Color Backgr = Color.White;
			bool isBold = false;

			if (msg.eventType == System.Diagnostics.TraceEventType.Error) {
				TextColor = Color.Red;
				isBold = true;
			}
			
			lc.columns.ForEach(col=>{
				var ret = msg.EvalXPath(col.xpath);
				var cell = new DataGridViewTextBoxCell() { Value = ret };
				
				row.Cells.Add(cell);
			});
			row.DefaultCellStyle.ForeColor = TextColor;
			if (isBold) {
				Font fnt = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
				row.DefaultCellStyle.Font = fnt;
			}
			
			dataGridView1.SuspendLayout();
			dataGridView1.Rows.Add(row);
			RecolorGrid();
			dataGridView1.ResumeLayout();
		}
		void RecolorGrid() {
			dataGridView1.Rows.ForEach(row => {
				var rw = (DataGridViewRow)row;
				int isodd;
				Math.DivRem(rw.Index, 2, out isodd);
				if (isodd == 0) {
					rw.DefaultCellStyle.BackColor = Color.LightCyan;
				} else {
					rw.DefaultCellStyle.BackColor = Color.White;
				}
			});
		}
		public void RemoveMessage(LogMessage msg) {
			dataGridView1.Rows.ForEach(x => {
				if (((DataGridViewRow)x).Tag == msg)
					dataGridView1.Rows.Remove((DataGridViewRow)x);
				return;
			});
		}

		void CreateColumn(string name) {
			DataGridViewColumn dCol = new DataGridViewTextBoxColumn();
			dCol.SortMode = DataGridViewColumnSortMode.NotSortable;
			dCol.Name = name;
			dataGridView1.Columns.Add(dCol);
		}

		void InitControl(List<LogMessage> msgList) {
			LoggerConfig lc = LoggerConfigAPI.Load();
			lc.columns.ForEach(col => {
				CreateColumn(col.name);
			});

			msgList.ForEach(msg => AddMessage(msg));
		}
	}
}
