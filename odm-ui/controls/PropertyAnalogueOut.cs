using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nvc.controllers;
using nvc.models;

namespace nvc.controls {
	public partial class PropertyAnalogueOut: BasePropertyControl{
		PropertyAnalogueOutputStrings _strings = new PropertyAnalogueOutputStrings();
		public PropertyAnalogueOut(DeviceIdentificationModel devMod) {

            InitializeComponent();

			BackColor = ColorDefinition.colControlBackground;
			_title.BackColor = ColorDefinition.colTitleBackground;

			Localization();
        }
		void Localization(){
			_title.CreateBinding(x => x.Text, _strings, x => x.title);
			_lblDigital.CreateBinding(x => x.Text, _strings, x => x.digital);
			_lblLoop.CreateBinding(x => x.Text, _strings, x => x.loop);
			_lbloff.CreateBinding(x => x.Text, _strings, x => x.off);
		}
		public override void ReleaseAll() {
		}
	}
}
