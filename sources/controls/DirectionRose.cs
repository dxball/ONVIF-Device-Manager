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
	public partial class DirectionRose : UserControl {
		public DirectionRose() {
			InitializeComponent();
			InitControls();
		}
		void InitControls() {
			_bE.AutoCheck = false;
			_bE.MouseClick += new MouseEventHandler(rBMouseClick);
			_bN.AutoCheck = false;
			_bN.MouseClick += new MouseEventHandler(rBMouseClick);
			_bNE.AutoCheck = false;
			_bNE.MouseClick += new MouseEventHandler(rBMouseClick);
			_bNW.AutoCheck = false;
			_bNW.MouseClick += new MouseEventHandler(rBMouseClick);
			_bS.AutoCheck = false;
			_bS.MouseClick += new MouseEventHandler(rBMouseClick);
			_bSE.AutoCheck = false;
			_bSE.MouseClick += new MouseEventHandler(rBMouseClick);
			_bSW.AutoCheck = false;
			_bSW.MouseClick += new MouseEventHandler(rBMouseClick);
			_bW.AutoCheck = false;
			_bW.MouseClick += new MouseEventHandler(rBMouseClick);
			_bC.AutoCheck = false;
			_bC.MouseClick += new MouseEventHandler(rBMouseClick);

			_bC.CheckedChanged += new EventHandler(_bC_CheckedChanged);
		}

		void rBMouseClick(object sender, MouseEventArgs e) {
			((RadioButton)sender).Checked = !((RadioButton)sender).Checked;
		}

		void _bC_CheckedChanged(object sender, EventArgs e) {
			_bE.Checked = _bC.Checked;
			_bN.Checked = _bC.Checked;
			_bNE.Checked = _bC.Checked;
			_bNW.Checked = _bC.Checked;
			_bS.Checked = _bC.Checked;
			_bSE.Checked = _bC.Checked;
			_bSW.Checked = _bC.Checked;
			_bW.Checked = _bC.Checked;
		}
		public void CreateBindings(RuleDescriptor rDesc) {
			//_bC.CreateBinding(x => x.Checked, rDesc, x => x);
			//_bE.CreateBinding(x => x.Checked, rDesc, x => x); ;
			//_bN;
			//_bNE;
			//_bNW;
			//_bS;
			//_bSE;
			//_bSW;
			//_bW;
		}
	}
}
