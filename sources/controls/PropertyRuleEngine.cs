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
using nvc.models;
using onvifdm.utils;
using System.Threading;
using nvc.controllers;

namespace nvc.controls {
	public partial class PropertyRuleEngine : BasePropertyControl {
		private RuleEngineStrings strings = RuleEngineStrings.Instance;
		public override void ReleaseUnmanaged() {
			if (_vidPlayer != null) {
				_vidPlayer.ReleaseUnmanaged();
			}
		}
		public PropertyRuleEngine(RuleEngineModel devModel) {
			InitializeComponent();
			_devModel = devModel;
			panel1.Paint += new PaintEventHandler(panel1_Paint);
			_tblRules.RowsAdded += new DataGridViewRowsAddedEventHandler(_tblRules_RowsAdded);
			Localization();
			InitControls();

			BindData(devModel);
		}

		void _tblRules_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			if (_tblRules.FirstDisplayedScrollingColumnHiddenWidth != 0)
				_columnID.Width = Defaults.iRuleEngineHeaderNameWithScroll;
		}
		
		VideoPlayerControl _vidPlayer;
		RuleEngineModel _devModel;
		GraphEditor _regionEditor;
		DataGridViewColumn _columnID;
		DataGridViewColumn _columnIsChecked;

		public Action Save { get; set; }
		public Action Cancel { get; set; }
		public Action AddRule { get; set; }
		public Action<RuleDescriptor> RemoveRule { get; set; }

		void panel1_Paint(object sender, PaintEventArgs e) {
			panel1.Paint -= panel1_Paint;

			_regionEditor = new GraphEditor(new Rectangle(10,10,10,10));// _devModel.bounds);
			InitUrl();
		}
		void LoadRegionEditor() {
			_regionEditor.SetParent(_vidPlayer.m_VlcControl);

			//_regionEditor.AddRegionEditor(_devModel.region);
		}
		public void InitUrl() {
			DebugHelper.Assert(SynchronizationContext.Current != null);
			//_vidPlayer = new VideoPlayerControl(_devModel.mediaUri) { Dock = DockStyle.Fill, _mediaStreamSize = _devModel.encoderResolution };
			//_vidPlayer.Dock = DockStyle.Fill;
			//panel1.CausesValidation = false;
			//_vidPlayer.CausesValidation = false;
			//_vidPlayer._action = _regionEditor.FillBitmap;
			//panel1.Controls.Add(_vidPlayer);
			//panel1.Capture = true;

			//LoadRegionEditor();
		}
		void BindData(RuleEngineModel devModel) {
			CreateHeaders();
			InitRulesList();
		}
		void CreateHeaders() {
			_columnID = new DataGridViewColumn() { Width = Defaults.iRuleEngineHeaderName };
			_columnIsChecked = new DataGridViewColumn() { Width = Defaults.iRuleEngineHeaderIsChecked };
			_tblRules.Columns.Add(_columnID);
			_tblRules.Columns.Add(_columnIsChecked);

			_tblRules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, strings, x => x.title);
			_btnAdd.CreateBinding(x => x.Text, strings, x => x.btnAdd);
			_btnRemove.CreateBinding(x => x.Text, strings, x => x.btnRemove);
			_cbAbandoning.CreateBinding(x => x.Text, strings, x => x.abandoning);
			_cbLoitering.CreateBinding(x => x.Text, strings, x => x.loitering);
			_cbMoving.CreateBinding(x => x.Text, strings, x => x.moving);
			_cbRunning.CreateBinding(x => x.Text, strings, x => x.running);
			_gbAction.CreateBinding(x => x.Text, strings, x => x.groupAction);
			_gbRule.CreateBinding(x => x.Text, strings, x => x.groupRule);
			_lblAAnalogue.CreateBinding(x => x.Text, strings, x => x.turnOnAnalog);
			_lblAFramerate.CreateBinding(x => x.Text, strings, x => x.setFramerate);
			_lblAOnvif.CreateBinding(x => x.Text, strings, x => x.sendOnvif);
			_lblARelay.CreateBinding(x => x.Text, strings, x => x.turnOnRelay);
			_lbldirection.CreateBinding(x => x.Text, strings, x => x.movingInDirections);
			_lblmeters.CreateBinding(x => x.Text, strings, x => x.meters);
			_lblmeters2.CreateBinding(x => x.Text, strings, x => x.meters);
			_lblName.CreateBinding(x => x.Text, strings, x => x.name);
			_lblPTZ.CreateBinding(x => x.Text, strings, x => x.ptz);
			_lblRecord.CreateBinding(x => x.Text, strings, x => x.record);
			_lblSeconds.CreateBinding(x => x.Text, strings, x => x.seconds);
			_lblspeed.CreateBinding(x => x.Text, strings, x => x.speed);
			_lblwhithin.CreateBinding(x => x.Text, strings, x => x.within);
		}

		void InitControls(){
			//Color
			_title.BackColor = ColorDefinition.colTitleBackground;
			BackColor = ColorDefinition.colControlBackground;

			_saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
			_saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
		}
		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			//Cancel
			Cancel();
		}
		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			Save();
		}
		public void ReleaseAll() {
			if (_regionEditor != null)
				_regionEditor.ReleaseAll();
			if (_vidPlayer != null)
				_vidPlayer.ReleaseAll();
		}
		void InitRulesList() {
			if (_devModel.Rules == null)
				_devModel.Rules = new List<RuleDescriptor>();
			_devModel.Rules.ForEach(x => {
				var row = new DataGridViewRow();

				row.Tag = x;

				var cellName = new DataGridViewTextBoxCellBindable();
				//cellName.CreateBinding(c=>c.Value, x, x.name);
				cellName.Value = x.name;

				var cellIsChecked = new DataGridViewCheckBoxCellBindable();
				//cellIsChecked.CreateBinding(c => c.Value, x, x.enabld);
				cellIsChecked.Value = x.enabld;

				row.Cells.Add(cellName);
				row.Cells.Add(cellIsChecked);

				_tblRules.Rows.Add(row);

			});
		}
		void UpdateTable() {
			_tblRules.Rows.Clear();
			InitRulesList();
		}

		private void _btnAdd_Click(object sender, EventArgs e) {
			if (AddRule != null)
				AddRule();
			UpdateTable();
		}

		private void _btnRemove_Click(object sender, EventArgs e) {
			if (_tblRules.SelectedRows.Count != 0) {
				if (RemoveRule != null)
					RemoveRule((RuleDescriptor)_tblRules.SelectedRows[0].Tag);
			}

			UpdateTable();
		}
	}
}

