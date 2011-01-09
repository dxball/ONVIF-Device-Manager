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

namespace odm.controls {
	public partial class PropertyTamperingDetectors  : BasePropertyControl{
		public override void ReleaseUnmanaged() { _vidPlayer.ReleaseUnmanaged(); }
		VideoPlayerControl _vidPlayer;
		PropertyTamperingDetectorsStrings _strings = new PropertyTamperingDetectorsStrings();
		AnnotationsModel _devMod;


		public Action Save;
		public Action Cancel;

		public PropertyTamperingDetectors(AnnotationsModel devMod) {
            InitializeComponent();
			_devMod = devMod;
			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};

			Load += new EventHandler(PropertyTamperingDetectors_Load);

			BackColor = ColorDefinition.colControlBackground;
			_title.BackColor = ColorDefinition.colTitleBackground;

			Localization();
			BindData(devMod);
        }

		void PropertyTamperingDetectors_Load(object sender, EventArgs e) {
			InitURI();
		}
		void InitURI() {
			//Start Workaround
			try {
				CreateStandAloneVLC(_devMod.mediaUri, _devMod.encoderResolution);
				pBox = new UserPictureBox() { Dock = DockStyle.Fill };
				panel1.Controls.Add(pBox);
				_tmr = new Timer();
				_tmr.Interval = 10; // refresh 10 time per second
				_tmr.Tick += new EventHandler(_tmr_Tick);
				_tmr.Start();
			} catch (Exception err) {
				VideoOperationError(err.Message);
			}
			//Stop Workaround
		}
		void Localization(){
			_title.CreateBinding(x => x.Text, _strings, x => x.title);

			_cbCameraDisplaced.CreateBinding(x => x.Text, _strings, x => x.cameraDisplaced);
			_cbFildOfView.CreateBinding(x => x.Text, _strings, x => x.fieldObstructed);
			_cbGlogal.CreateBinding(x => x.Text, _strings, x => x.globalChange);
			_cbOutFocus.CreateBinding(x => x.Text, _strings, x => x.outFocus);
			_cbSceneBright.CreateBinding(x => x.Text, _strings, x => x.sceneBright);
			_cbSceneDark.CreateBinding(x => x.Text, _strings, x => x.sceneDark);
			_cbSceneNoise.CreateBinding(x => x.Text, _strings, x => x.sceneNoisy);
			_cbSignalLost.CreateBinding(x => x.Text, _strings, x => x.signalLost);
		}
		//public void RefreshMediaURI() {
		//    _vidPlayer.Stop();
		//    _vidPlayer.Play(_devMod.mediaUri);
		//}
		void BindData(AnnotationsModel devMod) {
			//_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devMod, x => x.isModified);
			//_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devMod, x => x.isModified);

			//_saveCancelControl._btnCancel.Click += new EventHandler(_btnCancel_Click);
			//_saveCancelControl._btnSave.Click += new EventHandler(_btnSave_Click);
		}

		void _btnSave_Click(object sender, EventArgs e) {
			if (Save != null)
				Save();
		}

		void _btnCancel_Click(object sender, EventArgs e) {
			if (Cancel != null)
				Cancel();
		}
		public override void ReleaseAll() {
			base.ReleaseAll();
		}
    }
}
