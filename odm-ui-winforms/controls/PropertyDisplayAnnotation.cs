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
	public partial class PropertyDisplayAnnotation  : BasePropertyControl{
		public override void ReleaseUnmanaged() { _vidPlayer.ReleaseUnmanaged(); }
		VideoPlayerControl _vidPlayer;
		PropertyDisplayAnnotationStrings _strings = new PropertyDisplayAnnotationStrings();
		AnnotationsModel _devMod;


		public Action Save;
		public Action Cancel;

		public PropertyDisplayAnnotation(AnnotationsModel devMod) {
            InitializeComponent();
			_devMod = devMod;
			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};

			Load += new EventHandler(PropertyDisplayAnnotation_Load);
        }

		void PropertyDisplayAnnotation_Load(object sender, EventArgs e) {
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
			//_vidPlayer = new VideoPlayerControl(_devMod.mediaUri) { Dock = DockStyle.Fill };
			//panel1.Controls.Add(_vidPlayer);

			BackColor = ColorDefinition.colControlBackground;
			_title.BackColor = ColorDefinition.colTitleBackground;

			Localization();
			BindData(_devMod);			
		}
		void Localization(){
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_cbObjects.CreateBinding(x => x.Text, _strings, x => x.objects);
			_cbTimeStamp.CreateBinding(x => x.Text, _strings, x => x.timestamp);
			_cbTrajectories.CreateBinding(x => x.Text, _strings, x => x.trajectories);
			_cbSpeed.CreateBinding(x => x.Text, _strings, x => x.speed);
			_cbUserRegion.CreateBinding(x => x.Text, _strings, x => x.userRegion);
		}
		public void RefreshMediaURI() {
			//_vidPlayer.Stop();
			//_vidPlayer.Play(_devMod.mediaUri);
		}
		void BindData(AnnotationsModel devMod) {
			try{
			_cbObjects.CreateBinding(x => x.Checked, devMod, x => x.movingRects);
			}catch(Exception err){
				string strValue;
				if (devMod.movingRects == null)
					strValue = "Null";
				else
					strValue = devMod.movingRects.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindMovingRects + strValue);
			}
			try{
			_cbTimeStamp.CreateBinding(x => x.Checked, devMod, x => x.timestamp);
			}catch(Exception err){
				string strValue;
				if (devMod.timestamp == null)
					strValue = "Null";
				else
					strValue = devMod.timestamp.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindTimeStamp + strValue);
			}
			try{
			_cbTrajectories.CreateBinding(x => x.Checked, devMod, x => x.tracking);
			}catch(Exception err){
				string strValue;
				if (devMod.tracking == null)
					strValue = "Null";
				else
					strValue = devMod.tracking.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindTracking + strValue);
			}
			try{
			_cbSpeed.CreateBinding(x => x.Checked, devMod, x => x.speed);
			}catch(Exception err){
				string strValue;
				if (devMod.speed == null)
					strValue = "Null";
				else
					strValue = devMod.speed.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindSpeed + strValue);
			}
			try{
			_cbUserRegion.CreateBinding(x => x.Checked, devMod, x => x.userRegion);
			} catch (Exception err) {
				string strValue;
				if (devMod.userRegion == null)
					strValue = "Null";
				else
					strValue = devMod.userRegion.ToString();
				BindingError(err, ExceptionStrings.Instance.errBindUserRegion + strValue);
			}
			// Channel Name
			//_cbUserRegion.CreateBinding(x => x.Checked, devMod, x => x.userRegion);
			//} catch (Exception err) {
			//    string strValue;
			//    if (devMod.userRegion == null)
			//        strValue = "Null";
			//    else
			//        strValue = devMod.userRegion.ToString();
			//    BindingError(err, ExceptionStrings.Instance.errBindChannelName + strValue);
			//}

			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devMod, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devMod, x => x.isModified);

			_saveCancelControl._btnCancel.Click += new EventHandler(_btnCancel_Click);
			_saveCancelControl._btnSave.Click += new EventHandler(_btnSave_Click);
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
			if(_vidPlayer != null)
				_vidPlayer.ReleaseAll();
			base.ReleaseAll();
		}
    }
}
