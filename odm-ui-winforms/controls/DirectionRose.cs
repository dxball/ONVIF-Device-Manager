using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using odm.controllers;
using odm.models;

namespace odm.controls {
	public partial class DirectionRose : UserControl {
		public DirectionRose() {
			InitializeComponent();
			InitControls();
		}
		public Action SelectionChanged;
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
			if (SelectionChanged != null)
				SelectionChanged();
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
		public void FillDirections(RuleDescriptor rDesc) {
			rDesc.movRule.directions.ForEach(x => {
				switch (x) {
					case Directions.N:
						_bN.Checked = true;
						break;
					case Directions.NW:
						_bNW.Checked = true;
						break;
					case Directions.NE:
						_bNE.Checked = true;
						break;
					case Directions.W:
						_bW.Checked = true;
						break;
					case Directions.E:
						_bE.Checked = true;
						break;
					case Directions.S:
						_bS.Checked = true;
						break;
					case Directions.SW:
						_bSW.Checked = true;
						break;
					case Directions.SE:
						_bSE.Checked = true;
						break;
				}
			});
		}
		public RuleDescriptor GetDirections(RuleDescriptor rDesc) {
			rDesc.movRule.directions.Clear();
			if (_bN.Checked) rDesc.movRule.directions.Add(Directions.N);
			if (_bNW.Checked) rDesc.movRule.directions.Add(Directions.NW);
			if (_bNE.Checked) rDesc.movRule.directions.Add(Directions.NE);
			if (_bW.Checked) rDesc.movRule.directions.Add(Directions.W);
			if (_bE.Checked) rDesc.movRule.directions.Add(Directions.E);
			if (_bS.Checked) rDesc.movRule.directions.Add(Directions.S);
			if (_bSW.Checked) rDesc.movRule.directions.Add(Directions.SW);
			if (_bSE.Checked) rDesc.movRule.directions.Add(Directions.SE);
			return rDesc;
		}
		public bool drN {
			get { return _bN.Checked; }
			set { _bN.Checked = value; }
		}
		public bool drNW {
			get { return _bNW.Checked; }
			set { _bNW.Checked = value; }
		}
		public bool drNE {
			get { return _bNE.Checked; }
			set { _bNE.Checked = value; }
		}
		public bool drS {
			get { return _bS.Checked; }
			set { _bS.Checked = value; }
		}
		public bool drSW {
			get { return _bSW.Checked; }
			set { _bSW.Checked = value; }
		}
		public bool drSE {
			get { return _bSE.Checked; }
			set { _bSE.Checked = value; }
		}
		public bool drW {
			get { return _bW.Checked; }
			set { _bW.Checked = value; }
		}
		public bool drE {
			get { return _bE.Checked; }
			set { _bE.Checked = value; }
		}		
	}
}