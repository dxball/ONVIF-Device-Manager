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
using odm.models;
using odm.utils;
using System.Threading;
using odm.controllers;
using odm.controls.regionEditor;

namespace odm.controls {
	public partial class PropertyRuleEngine : BasePropertyControl {
		PropertyRuleEngineStrings _strings = new PropertyRuleEngineStrings();
		public override void ReleaseUnmanaged() {
			//if (_vidPlayer != null) {
			//    _vidPlayer.ReleaseUnmanaged();
			//}
		}
		public PropertyRuleEngine(RuleEngineModel devModel) {
			InitializeComponent();
			_devModel = devModel;
			
			Load += new EventHandler(PropertyRuleEngine_Load);
		}

		void PropertyRuleEngine_Load(object sender, EventArgs e) {
			panel1.Paint += new PaintEventHandler(panel1_Paint);
			_tblRules.RowsAdded += new DataGridViewRowsAddedEventHandler(_tblRules_RowsAdded);
			Localization();
			InitControls();

			BindData(_devModel);
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
		RuleDescriptor currentRule = new RuleDescriptor();

		void panel1_Paint(object sender, PaintEventArgs e) {
			panel1.Paint -= panel1_Paint;

			_regionEditor = new GraphEditor(new Rectangle(0, 0, _devModel.encoderResolution.Width, _devModel.encoderResolution.Height));
			InitUrl();
		}
		void LoadRegionEditor() {
			_regionEditor.SetParent(pBox);
		}

		void InitRegion() {
			_regionEditor.AddRegionEditor(currentRule.ruleRegion);
		}

		public void InitUrl() {
			dbg.Assert(SynchronizationContext.Current != null);
			//_vidPlayer = new VideoPlayerControl(_devModel.mediaUri) {
			//    Dock = DockStyle.Fill,
			//    _mediaStreamSize = _devModel.encoderResolution
			//};
			//_vidPlayer.Dock = DockStyle.Fill;
			//panel1.CausesValidation = false;
			//_vidPlayer.CausesValidation = false;
			//_vidPlayer._action = _regionEditor.FillBitmap;
			//panel1.Controls.Add(_vidPlayer);
			//panel1.Capture = true;
			//Start Workaround
			try {
				CreateStandAloneVLC(_devModel.mediaUri, _devModel.encoderResolution);
				pBox = new UserPictureBox() { Dock = DockStyle.Fill };
				panel1.Controls.Add(pBox);
				_tmr = new System.Windows.Forms.Timer();
				_tmr.Interval = 10; // refresh 10 time per second
				_tmr.Tick += new EventHandler(_tmr_Tick);
				_tmr.Start();
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
			//Stop Workaround
			LoadRegionEditor();
		}

		void FillData() {
			_tbName.Text = currentRule.name;
			_cbAbandoning.Checked = currentRule.enabled;
			//Moving
			_cbMoving.Checked = currentRule.movRule.enabled;
			_nmMoveDist.Value = currentRule.movRule.distance;
			_directionRose.FillDirections(currentRule);
			//Runing
			_cbRunning.Enabled = currentRule.runRule.enabled;
			_nmRunningSpeed.Value =currentRule.runRule.runningSpeed;
			//Loitering
			_cbLoitering.Checked = currentRule.loitRule.enabled;
			_nmLoiteringDistance.Value = currentRule.loitRule.loiteringDistance;
			_nmLoiteringTime.Value = currentRule.loitRule.loiteringTime;
			//Entering
			_cbEntering.Checked = currentRule.enterRule.enabled;
			//Leaving
			_cbLeaving.Checked = currentRule.leavingRule.enabled;
			//Actions
			_lblAAnalogue.Enabled = currentRule.actionAnalogueOut.analogueOut;
			_lblAFramerate.Enabled = currentRule.actionFrameRate.setFrame;
			_lblAOnvif.Enabled = currentRule.actionOnvif.sendOnvifMessage;
			_lblARelay.Enabled = currentRule.actionTurnRele.turnRele;
			_nmAsetFps.Value = currentRule.actionFrameRate.frameRate;
			_cmbRelays.SelectedValue = currentRule.actionTurnRele.relayID;
		}

		void GetData() {
			currentRule.name = _tbName.Text;
			currentRule.enabled = _cbAbandoning.Checked;
			//Moving
			currentRule.movRule.enabled = _cbMoving.Checked;
			currentRule.movRule.distance = (int)_nmMoveDist.Value;
			currentRule = _directionRose.GetDirections(currentRule);
			//Runing
			currentRule.runRule.enabled = _cbRunning.Enabled;
			currentRule.runRule.runningSpeed = (int)_nmRunningSpeed.Value;
			//Loitering
			currentRule.loitRule.enabled = _cbLoitering.Checked;
			currentRule.loitRule.loiteringDistance = (int)_nmLoiteringDistance.Value;
			currentRule.loitRule.loiteringTime = (int)_nmLoiteringTime.Value;
			//Entering
			currentRule.enterRule.enabled = _cbEntering.Checked;
			//Leaving
			currentRule.leavingRule.enabled = _cbLeaving.Checked;
			//Actions
			currentRule.actionAnalogueOut.analogueOut = _lblAAnalogue.Enabled;
			currentRule.actionFrameRate.setFrame = _lblAFramerate.Enabled;
			currentRule.actionOnvif.sendOnvifMessage = _lblAOnvif.Enabled;
			currentRule.actionTurnRele.turnRele = _lblARelay.Enabled;
			currentRule.actionFrameRate.frameRate = (int)_nmAsetFps.Value;
			currentRule.actionTurnRele.relayID = _cmbRelays.SelectedValue.ToString();

			currentRule.ruleRegion = _regionEditor.GetRegion();
		}

		void BindData(RuleEngineModel devModel) {
			CreateHeaders();

			FillData();

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
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_btnAdd.CreateBinding(x => x.Text, _strings, x => x.btnAdd);
			_btnRemove.CreateBinding(x => x.Text, _strings, x => x.btnRemove);
			_cbAbandoning.CreateBinding(x => x.Text, _strings, x => x.abandoning);
			_cbLoitering.CreateBinding(x => x.Text, _strings, x => x.loitering);
			_cbMoving.CreateBinding(x => x.Text, _strings, x => x.moving);
			_cbRunning.CreateBinding(x => x.Text, _strings, x => x.running);
			_gbAction.CreateBinding(x => x.Text, _strings, x => x.groupAction);
			_gbRule.CreateBinding(x => x.Text, _strings, x => x.groupRule);
			_lblAAnalogue.CreateBinding(x => x.Text, _strings, x => x.turnOnAnalog);
			_lblAFramerate.CreateBinding(x => x.Text, _strings, x => x.setFramerate);
			_lblAOnvif.CreateBinding(x => x.Text, _strings, x => x.sendOnvif);
			_lblARelay.CreateBinding(x => x.Text, _strings, x => x.turnOnRelay);
			_lbldirection.CreateBinding(x => x.Text, _strings, x => x.movingInDirections);
			_lblmeters.CreateBinding(x => x.Text, _strings, x => x.meters);
			_lblmeters2.CreateBinding(x => x.Text, _strings, x => x.meters);
			_lblName.CreateBinding(x => x.Text, _strings, x => x.name);
			_lblAPTZ.CreateBinding(x => x.Text, _strings, x => x.ptz);
			_lblARecord.CreateBinding(x => x.Text, _strings, x => x.record);
			_lblSeconds.CreateBinding(x => x.Text, _strings, x => x.seconds);
			_lblspeed.CreateBinding(x => x.Text, _strings, x => x.speed);
			_lblwhithin.CreateBinding(x => x.Text, _strings, x => x.within);
		}

		void InitControls(){
			//Color
			_title.BackColor = ColorDefinition.colTitleBackground;
			BackColor = ColorDefinition.colControlBackground;

			_tblRules.SelectionChanged += new EventHandler(_tblRules_SelectionChanged);

			_saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
			_saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
		}

		void _tblRules_SelectionChanged(object sender, EventArgs e) {
			if (_tblRules.SelectedRows.Count != 0) {
				_regionEditor.ReleaseAll();
				currentRule = ((RuleDescriptor)_tblRules.SelectedRows[0].Tag);
				FillData();
				InitRegion();
			}
		}
		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			//Cancel
			Cancel();
		}
		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			GetData();
			Save();
		}
		public override void ReleaseAll() {
			if (_regionEditor != null)
				_regionEditor.ReleaseAll();
			if (_vidPlayer != null) {
				ReleaseUnmanaged();
				_vidPlayer.ReleaseAll();
			}
		}
		void InitRulesList() {
			if (_devModel.Rules == null)
				_devModel.Rules = new List<RuleDescriptor>();
			_devModel.Rules.ForEach(x => {
				var row = new DataGridViewRow();

				row.Tag = x;

				var cellName = new DataGridViewTextBoxCellBindable();
				cellName.Value = x.name;

				var cellIsChecked = new DataGridViewCheckBoxCellBindable();
				cellIsChecked.Value = x.enabled;

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
			//Can't remove last rule ??
			if (_tblRules.SelectedRows.Count > 1) {
				if (RemoveRule != null)
					RemoveRule((RuleDescriptor)_tblRules.SelectedRows[0].Tag);
			}

			UpdateTable();
		}
	}
}

