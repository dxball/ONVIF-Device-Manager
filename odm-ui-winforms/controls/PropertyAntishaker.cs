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
using odm.controls.regionEditor;

namespace odm.controls {
	public partial class PropertyAntishaker : BasePropertyControl {
		PropertyAntishakerStrings _strings = new PropertyAntishakerStrings();
		public Action Save { get; set; }
		public Action Cancel { get; set; }
		LiveVideoModel _devModel;
		//AntishakerModel _devModel;
		GraphEditor _regionEditor;
		public override void ReleaseUnmanaged() { 
		//	_vidPlayer.ReleaseUnmanaged(); 
		}
		VideoPlayerControl _vidPlayer;

		public PropertyAntishaker(LiveVideoModel devModel) {
		//public PropertyAntishaker(AntishakerModel devModel) {
			InitializeComponent();
			_devModel = devModel;

			//panel1.Paint += new PaintEventHandler(panel1_Paint);
			Load += new EventHandler(PropertyAntishaker_Load);
		}

		void PropertyAntishaker_Load(object sender, EventArgs e) {
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

			InitControls();
		}
		void panel1_Paint(object sender, PaintEventArgs e) {
			panel1.Paint -= panel1_Paint;

			_regionEditor = new GraphEditor(new Rectangle(0,0,1024,768));//_devModel.bounds);
			InitUrl();
		}
		void LoadRegionEditor() {
			//_regionEditor.SetParent(_vidPlayer.m_VlcControl);

			//_regionEditor.AddRectangleEditor(new Rectangle(10, 10, 100, 100));
		}
		public void InitUrl() {
			dbg.Assert(SynchronizationContext.Current != null);
			//Start Workaround
			try {
				CreateStandAloneVLC(_devModel.mediaUri, _devModel.encoderResolution);
				pBox = new UserPictureBox() { Dock = DockStyle.Fill };
				panel1.Controls.Add(pBox);
				_tmr = new System.Windows.Forms.Timer();
				_tmr.Interval = 10; // refresh 100 time per second
				_tmr.Tick += new EventHandler(_tmr_Tick);
				drwaAction = _regionEditor.FillBitmap;
				_tmr.Start();
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
			//Stop Workaround
			//_vidPlayer = new VideoPlayerControl(_devModel.mediaUri) { Dock = DockStyle.Fill};//, _mediaStreamSize = _devModel.encoderResolution };
			//_vidPlayer.Dock = DockStyle.Fill;
			//panel1.CausesValidation = false;
			//_vidPlayer.CausesValidation = false;
			//_vidPlayer._action = _regionEditor.FillBitmap;
			//panel1.Controls.Add(_vidPlayer);
			//panel1.Capture = true;

			LoadRegionEditor();
		}

		void Localization() {
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
		}

		void InitControls() {
			Localization();

			//Color
			_title.BackColor = ColorDefinition.colTitleBackground;
			BackColor = ColorDefinition.colControlBackground;

			_saveCancelControl._btnCancel.Enabled = false;
			_saveCancelControl._btnSave.Enabled = true;

			_saveCancelControl.ButtonClickedCancel += new EventHandler(_saveCancelControl_ButtonClickedCancel);
			_saveCancelControl.ButtonClickedSave += new EventHandler(_saveCancelControl_ButtonClickedSave);
		}

		void _saveCancelControl_ButtonClickedSave(object sender, EventArgs e) {
			
		}

		void _saveCancelControl_ButtonClickedCancel(object sender, EventArgs e) {
			
		}
		public override void ReleaseAll() {
			if (_vidPlayer != null) {
				ReleaseUnmanaged();
				_vidPlayer.ReleaseAll();
			}
			base.ReleaseAll();
		}
	}
}
