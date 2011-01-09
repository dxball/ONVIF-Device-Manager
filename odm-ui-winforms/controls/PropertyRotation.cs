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
	public partial class PropertyRotation : BasePropertyControl {
		public override void ReleaseUnmanaged() { _vidPlayer.ReleaseUnmanaged(); }
		VideoPlayerControl _vidPlayer;
		PropertyRotationStrings _strings = new PropertyRotationStrings();
		AnnotationsModel _devMod;

		public PropertyRotation(AnnotationsModel devMod) {
            InitializeComponent();
			this.Disposed += (sender, args) => {
				this.ReleaseAll();
			};
			_devMod = devMod;

			Load += new EventHandler(PropertyRotation_Load);
        }

		void PropertyRotation_Load(object sender, EventArgs e) {
			//_vidPlayer = new VideoPlayerControl(_devMod.mediaUri) { Dock = DockStyle.Fill };
			//panel1.Controls.Add(_vidPlayer);

			BackColor = ColorDefinition.colControlBackground;
			_title.BackColor = ColorDefinition.colTitleBackground;

			Localization();
			ResetBackGround();
			BindData();
		}
		void Localization(){
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_lblF0H.CreateBinding(x => x.Text, _strings, x => x.flipH0);
			_lblF0V.CreateBinding(x => x.Text, _strings, x => x.flipV0);
			_lblFH.CreateBinding(x => x.Text, _strings, x => x.flipH);
			_lblFlip.CreateBinding(x => x.Text, _strings, x => x.flip);
			_lblFV.CreateBinding(x => x.Text, _strings, x => x.flipV);
			_lblR0.CreateBinding(x => x.Text, _strings, x => x.rotate0);
			_lblR180.CreateBinding(x => x.Text, _strings, x => x.rotate180);
			_lblR270.CreateBinding(x => x.Text, _strings, x => x.rotate270);
			_lblR90.CreateBinding(x => x.Text, _strings, x => x.rotate90);
			_lblRotaton.CreateBinding(x => x.Text, _strings, x => x.rotate);
		}
		void BindData() {
			_imgF0H.Tag = RotateFlipType.RotateNoneFlipNone;
			_imgF0V.Tag = RotateFlipType.RotateNoneFlipNone;
			_imgR0.Tag = RotateFlipType.RotateNoneFlipNone;

			_imgFHotizont.Tag = RotateFlipType.RotateNoneFlipX;
			_imgFHotizont._imgBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
			_imgFVertical.Tag = RotateFlipType.RotateNoneFlipY;
			_imgFVertical._imgBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
			
			_imgR180.Tag = RotateFlipType.Rotate180FlipNone;
			_imgR180._imgBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
			_imgR270.Tag = RotateFlipType.Rotate270FlipNone;
			_imgR270._imgBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
			_imgR90.Tag = RotateFlipType.Rotate90FlipNone;
			_imgR90._imgBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
		}

		void ResetBackGround() {
			_imgF0H.BackColor = ColorDefinition.colControlBackground;
			_imgF0V.BackColor = ColorDefinition.colControlBackground;
			_imgFHotizont.BackColor = ColorDefinition.colControlBackground;
			_imgFVertical.BackColor = ColorDefinition.colControlBackground;
			_imgR0.BackColor = ColorDefinition.colControlBackground;
			_imgR180.BackColor = ColorDefinition.colControlBackground;
			_imgR270.BackColor = ColorDefinition.colControlBackground;
			_imgR90.BackColor = ColorDefinition.colControlBackground;
		}
		public override void ReleaseAll() {
			if(_vidPlayer!=null){
				ReleaseUnmanaged();
				_vidPlayer.ReleaseAll();	
			}
		}
	}
}
