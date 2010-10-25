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
using nvc.controllers;
using System.Threading;

namespace nvc.controls {
	public partial class PropertyObjectTracker : BasePropertyControl {
		public override void ReleaseUnmanaged() { _vidPlayer.ReleaseUnmanaged(); }
		public PropertyObjectTracker(ObjectTrackerModel devModel) {
			InitializeComponent();
			_devModel = devModel;
			
			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};

			BindData(devModel);
			InitControls();
			InitUrl();
		}
		
		ObjectTrackerModel _devModel;
		VideoPlayerControl _vidPlayer;

		public Action Save { get; set; }
		public Action Cancel { get; set; }

		void BindData(ObjectTrackerModel devModel) {
			_numtbContrast.CreateBinding(x => x.Value, devModel, x => x.contrastSensitivity);
			_numtbAreaMin.CreateBinding(x => x.Value, devModel, x => x.minObjectArea);
			_numtbAreaMin.CreateBinding(x => x.Maximum, devModel, x => x.maxObjectArea);
			_numtbAreaMax.CreateBinding(x => x.Value, devModel, x => x.maxObjectArea);
			_numtbAreaMax.CreateBinding(x => x.Minimum, devModel, x => x.minObjectArea);
			_numtbSpeedMax.CreateBinding(x => x.Value, devModel, x => x.maxObjectSpeed);
			_numtbTime.CreateBinding(x => x.Value, devModel, x => x.stabilizationTime);
			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyObjectTrackerTitle);
			_lblContrast.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyObjectTrackerContrast);
			_tbObjAreaMax.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyObjectTrackerAreaMax);
			_tbObjAreaMin.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyObjectTrackerAreaMin);
			_tbSpeedMax.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyObjectTrackerSpeedMax);
			_tbStabilization.CreateBinding(x => x.Text, Constants.Instance, x => x.sPropertyObjectTrackerStabilization);
		}

		void InitControls() {
			Localization();

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
			//Save
			Save();
		}
		public void InitUrl() {
			DebugHelper.Assert(SynchronizationContext.Current != null);
			_vidPlayer = new VideoPlayerControl(_devModel.mediaUri) { Dock = DockStyle.Fill };
			
			_vidPlayer.Dock = DockStyle.Fill;
			panel1.CausesValidation = false;
			_vidPlayer.CausesValidation = false;
			panel1.Controls.Add(_vidPlayer);
			panel1.Capture = true;			
		}

		public void ReleaseAll() {
			if(_vidPlayer!=null)
				_vidPlayer.ReleaseAll();
		}

	}
}
