namespace nvc.controls {
	partial class PropertyRuleEngine {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.panel1 = new System.Windows.Forms.Panel();
			this._lblName = new System.Windows.Forms.TextBox();
			this._tbName = new System.Windows.Forms.TextBox();
			this._gbRule = new System.Windows.Forms.GroupBox();
			this._lblmeters = new System.Windows.Forms.Label();
			this._lblSeconds = new System.Windows.Forms.Label();
			this._cbAbandoning = new System.Windows.Forms.CheckBox();
			this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
			this._lblmeters2 = new System.Windows.Forms.Label();
			this._cbRunning = new System.Windows.Forms.CheckBox();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this._lblwhithin = new System.Windows.Forms.Label();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this._cbLoitering = new System.Windows.Forms.CheckBox();
			this._lbldirection = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this._cbMoving = new System.Windows.Forms.CheckBox();
			this._directionRose = new nvc.controls.DirectionRose();
			this._lblspeed = new System.Windows.Forms.Label();
			this._btnAdd = new System.Windows.Forms.Button();
			this._btnRemove = new System.Windows.Forms.Button();
			this._gbAction = new System.Windows.Forms.GroupBox();
			this._cmbRelays = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
			this._lblPTZ = new System.Windows.Forms.CheckBox();
			this._lblRecord = new System.Windows.Forms.CheckBox();
			this._lblAAnalogue = new System.Windows.Forms.CheckBox();
			this._lblARelay = new System.Windows.Forms.CheckBox();
			this._lblAFramerate = new System.Windows.Forms.CheckBox();
			this._lblAOnvif = new System.Windows.Forms.CheckBox();
			this._title = new nvc.controls.GroupBoxControl();
			this._saveCancelControl = new nvc.controls.SaveCancelControl();
			this._tblRules = new System.Windows.Forms.DataGridView();
			this._gbRule.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this._gbAction.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._tblRules)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.panel1.Location = new System.Drawing.Point(3, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(483, 178);
			this.panel1.TabIndex = 2;
			// 
			// _lblName
			// 
			this._lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblName.Location = new System.Drawing.Point(102, 216);
			this._lblName.Name = "_lblName";
			this._lblName.ReadOnly = true;
			this._lblName.Size = new System.Drawing.Size(158, 20);
			this._lblName.TabIndex = 17;
			this._lblName.Text = "Name";
			// 
			// _tbName
			// 
			this._tbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbName.Location = new System.Drawing.Point(266, 216);
			this._tbName.Name = "_tbName";
			this._tbName.Size = new System.Drawing.Size(221, 20);
			this._tbName.TabIndex = 16;
			// 
			// _gbRule
			// 
			this._gbRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._gbRule.Controls.Add(this._lblmeters);
			this._gbRule.Controls.Add(this._lblSeconds);
			this._gbRule.Controls.Add(this._cbAbandoning);
			this._gbRule.Controls.Add(this.numericUpDown4);
			this._gbRule.Controls.Add(this._lblmeters2);
			this._gbRule.Controls.Add(this._cbRunning);
			this._gbRule.Controls.Add(this.numericUpDown3);
			this._gbRule.Controls.Add(this._lblwhithin);
			this._gbRule.Controls.Add(this.numericUpDown2);
			this._gbRule.Controls.Add(this._cbLoitering);
			this._gbRule.Controls.Add(this._lbldirection);
			this._gbRule.Controls.Add(this.numericUpDown1);
			this._gbRule.Controls.Add(this._cbMoving);
			this._gbRule.Controls.Add(this._directionRose);
			this._gbRule.Controls.Add(this._lblspeed);
			this._gbRule.Location = new System.Drawing.Point(102, 242);
			this._gbRule.Name = "_gbRule";
			this._gbRule.Size = new System.Drawing.Size(189, 208);
			this._gbRule.TabIndex = 19;
			this._gbRule.TabStop = false;
			this._gbRule.Text = "groupBox1";
			// 
			// _lblmeters
			// 
			this._lblmeters.AutoSize = true;
			this._lblmeters.Location = new System.Drawing.Point(122, 20);
			this._lblmeters.Name = "_lblmeters";
			this._lblmeters.Size = new System.Drawing.Size(15, 13);
			this._lblmeters.TabIndex = 30;
			this._lblmeters.Text = "m";
			// 
			// _lblSeconds
			// 
			this._lblSeconds.AutoSize = true;
			this._lblSeconds.Location = new System.Drawing.Point(134, 112);
			this._lblSeconds.Name = "_lblSeconds";
			this._lblSeconds.Size = new System.Drawing.Size(12, 13);
			this._lblSeconds.TabIndex = 29;
			this._lblSeconds.Text = "s";
			// 
			// _cbAbandoning
			// 
			this._cbAbandoning.AutoSize = true;
			this._cbAbandoning.Location = new System.Drawing.Point(6, 185);
			this._cbAbandoning.Name = "_cbAbandoning";
			this._cbAbandoning.Size = new System.Drawing.Size(80, 17);
			this._cbAbandoning.TabIndex = 28;
			this._cbAbandoning.Text = "checkBox4";
			this._cbAbandoning.UseVisualStyleBackColor = true;
			// 
			// numericUpDown4
			// 
			this.numericUpDown4.Location = new System.Drawing.Point(123, 162);
			this.numericUpDown4.Name = "numericUpDown4";
			this.numericUpDown4.Size = new System.Drawing.Size(41, 20);
			this.numericUpDown4.TabIndex = 27;
			// 
			// _lblmeters2
			// 
			this._lblmeters2.AutoSize = true;
			this._lblmeters2.Location = new System.Drawing.Point(134, 138);
			this._lblmeters2.Name = "_lblmeters2";
			this._lblmeters2.Size = new System.Drawing.Size(15, 13);
			this._lblmeters2.TabIndex = 26;
			this._lblmeters2.Text = "m";
			// 
			// _cbRunning
			// 
			this._cbRunning.AutoSize = true;
			this._cbRunning.Location = new System.Drawing.Point(6, 163);
			this._cbRunning.Name = "_cbRunning";
			this._cbRunning.Size = new System.Drawing.Size(119, 17);
			this._cbRunning.TabIndex = 26;
			this._cbRunning.Text = "Running faster than";
			this._cbRunning.UseVisualStyleBackColor = true;
			// 
			// numericUpDown3
			// 
			this.numericUpDown3.Location = new System.Drawing.Point(92, 136);
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(41, 20);
			this.numericUpDown3.TabIndex = 25;
			// 
			// _lblwhithin
			// 
			this._lblwhithin.AutoSize = true;
			this._lblwhithin.Location = new System.Drawing.Point(51, 138);
			this._lblwhithin.Name = "_lblwhithin";
			this._lblwhithin.Size = new System.Drawing.Size(34, 13);
			this._lblwhithin.TabIndex = 24;
			this._lblwhithin.Text = "within";
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Location = new System.Drawing.Point(92, 110);
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(41, 20);
			this.numericUpDown2.TabIndex = 23;
			// 
			// _cbLoitering
			// 
			this._cbLoitering.AutoSize = true;
			this._cbLoitering.Location = new System.Drawing.Point(6, 111);
			this._cbLoitering.Name = "_cbLoitering";
			this._cbLoitering.Size = new System.Drawing.Size(66, 17);
			this._cbLoitering.TabIndex = 22;
			this._cbLoitering.Text = "Loitering";
			this._cbLoitering.UseVisualStyleBackColor = true;
			// 
			// _lbldirection
			// 
			this._lbldirection.AutoSize = true;
			this._lbldirection.Location = new System.Drawing.Point(6, 43);
			this._lbldirection.Name = "_lbldirection";
			this._lbldirection.Size = new System.Drawing.Size(66, 13);
			this._lbldirection.TabIndex = 21;
			this._lbldirection.Text = "in directions:";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(81, 18);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(41, 20);
			this.numericUpDown1.TabIndex = 20;
			// 
			// _cbMoving
			// 
			this._cbMoving.AutoSize = true;
			this._cbMoving.Location = new System.Drawing.Point(6, 19);
			this._cbMoving.Margin = new System.Windows.Forms.Padding(0);
			this._cbMoving.Name = "_cbMoving";
			this._cbMoving.Size = new System.Drawing.Size(61, 17);
			this._cbMoving.TabIndex = 19;
			this._cbMoving.Text = "Moving";
			this._cbMoving.UseVisualStyleBackColor = true;
			// 
			// _directionRose
			// 
			this._directionRose.Location = new System.Drawing.Point(116, 41);
			this._directionRose.Name = "_directionRose";
			this._directionRose.Size = new System.Drawing.Size(67, 63);
			this._directionRose.TabIndex = 18;
			// 
			// _lblspeed
			// 
			this._lblspeed.AutoSize = true;
			this._lblspeed.Location = new System.Drawing.Point(164, 165);
			this._lblspeed.Margin = new System.Windows.Forms.Padding(0);
			this._lblspeed.Name = "_lblspeed";
			this._lblspeed.Size = new System.Drawing.Size(25, 13);
			this._lblspeed.TabIndex = 31;
			this._lblspeed.Text = "m/s";
			// 
			// _btnAdd
			// 
			this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._btnAdd.Font = new System.Drawing.Font("Times New Roman", 6.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._btnAdd.Location = new System.Drawing.Point(4, 375);
			this._btnAdd.Name = "_btnAdd";
			this._btnAdd.Size = new System.Drawing.Size(45, 23);
			this._btnAdd.TabIndex = 27;
			this._btnAdd.Text = "Add";
			this._btnAdd.UseVisualStyleBackColor = true;
			this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
			// 
			// _btnRemove
			// 
			this._btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._btnRemove.Font = new System.Drawing.Font("Times New Roman", 6.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._btnRemove.Location = new System.Drawing.Point(51, 375);
			this._btnRemove.Margin = new System.Windows.Forms.Padding(0);
			this._btnRemove.Name = "_btnRemove";
			this._btnRemove.Size = new System.Drawing.Size(45, 23);
			this._btnRemove.TabIndex = 28;
			this._btnRemove.Text = "Remove";
			this._btnRemove.UseVisualStyleBackColor = true;
			this._btnRemove.Click += new System.EventHandler(this._btnRemove_Click);
			// 
			// _gbAction
			// 
			this._gbAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._gbAction.Controls.Add(this._cmbRelays);
			this._gbAction.Controls.Add(this.label1);
			this._gbAction.Controls.Add(this.numericUpDown5);
			this._gbAction.Controls.Add(this._lblPTZ);
			this._gbAction.Controls.Add(this._lblRecord);
			this._gbAction.Controls.Add(this._lblAAnalogue);
			this._gbAction.Controls.Add(this._lblARelay);
			this._gbAction.Controls.Add(this._lblAFramerate);
			this._gbAction.Controls.Add(this._lblAOnvif);
			this._gbAction.Location = new System.Drawing.Point(292, 242);
			this._gbAction.Name = "_gbAction";
			this._gbAction.Size = new System.Drawing.Size(195, 208);
			this._gbAction.TabIndex = 29;
			this._gbAction.TabStop = false;
			this._gbAction.Text = "groupBox2";
			// 
			// _cmbRelays
			// 
			this._cmbRelays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbRelays.FormattingEnabled = true;
			this._cmbRelays.Location = new System.Drawing.Point(109, 63);
			this._cmbRelays.Name = "_cmbRelays";
			this._cmbRelays.Size = new System.Drawing.Size(84, 21);
			this._cmbRelays.TabIndex = 26;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(172, 43);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(21, 13);
			this.label1.TabIndex = 25;
			this.label1.Text = "fps";
			// 
			// numericUpDown5
			// 
			this.numericUpDown5.Location = new System.Drawing.Point(131, 41);
			this.numericUpDown5.Name = "numericUpDown5";
			this.numericUpDown5.Size = new System.Drawing.Size(41, 20);
			this.numericUpDown5.TabIndex = 24;
			// 
			// _lblPTZ
			// 
			this._lblPTZ.AutoSize = true;
			this._lblPTZ.Enabled = false;
			this._lblPTZ.Location = new System.Drawing.Point(6, 134);
			this._lblPTZ.Name = "_lblPTZ";
			this._lblPTZ.Size = new System.Drawing.Size(86, 17);
			this._lblPTZ.TabIndex = 5;
			this._lblPTZ.Text = "checkBox10";
			this._lblPTZ.UseVisualStyleBackColor = true;
			// 
			// _lblRecord
			// 
			this._lblRecord.AutoSize = true;
			this._lblRecord.Enabled = false;
			this._lblRecord.Location = new System.Drawing.Point(6, 111);
			this._lblRecord.Name = "_lblRecord";
			this._lblRecord.Size = new System.Drawing.Size(80, 17);
			this._lblRecord.TabIndex = 4;
			this._lblRecord.Text = "checkBox9";
			this._lblRecord.UseVisualStyleBackColor = true;
			// 
			// _lblAAnalogue
			// 
			this._lblAAnalogue.AutoSize = true;
			this._lblAAnalogue.Location = new System.Drawing.Point(6, 88);
			this._lblAAnalogue.Name = "_lblAAnalogue";
			this._lblAAnalogue.Size = new System.Drawing.Size(80, 17);
			this._lblAAnalogue.TabIndex = 3;
			this._lblAAnalogue.Text = "checkBox8";
			this._lblAAnalogue.UseVisualStyleBackColor = true;
			// 
			// _lblARelay
			// 
			this._lblARelay.AutoSize = true;
			this._lblARelay.Location = new System.Drawing.Point(6, 65);
			this._lblARelay.Name = "_lblARelay";
			this._lblARelay.Size = new System.Drawing.Size(80, 17);
			this._lblARelay.TabIndex = 2;
			this._lblARelay.Text = "checkBox7";
			this._lblARelay.UseVisualStyleBackColor = true;
			// 
			// _lblAFramerate
			// 
			this._lblAFramerate.AutoSize = true;
			this._lblAFramerate.Location = new System.Drawing.Point(6, 42);
			this._lblAFramerate.Name = "_lblAFramerate";
			this._lblAFramerate.Size = new System.Drawing.Size(80, 17);
			this._lblAFramerate.TabIndex = 1;
			this._lblAFramerate.Text = "checkBox6";
			this._lblAFramerate.UseVisualStyleBackColor = true;
			// 
			// _lblAOnvif
			// 
			this._lblAOnvif.AutoSize = true;
			this._lblAOnvif.Location = new System.Drawing.Point(6, 19);
			this._lblAOnvif.Name = "_lblAOnvif";
			this._lblAOnvif.Size = new System.Drawing.Size(80, 17);
			this._lblAOnvif.TabIndex = 0;
			this._lblAOnvif.Text = "checkBox5";
			this._lblAOnvif.UseVisualStyleBackColor = true;
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(483, 23);
			this._title.TabIndex = 1;
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(3, 462);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(157, 23);
			this._saveCancelControl.TabIndex = 30;
			// 
			// _tblRules
			// 
			this._tblRules.AllowUserToAddRows = false;
			this._tblRules.AllowUserToDeleteRows = false;
			this._tblRules.AllowUserToOrderColumns = true;
			this._tblRules.AllowUserToResizeRows = false;
			this._tblRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tblRules.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this._tblRules.ColumnHeadersVisible = false;
			this._tblRules.Location = new System.Drawing.Point(4, 216);
			this._tblRules.MultiSelect = false;
			this._tblRules.Name = "_tblRules";
			this._tblRules.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._tblRules.RowHeadersVisible = false;
			this._tblRules.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this._tblRules.ShowCellErrors = false;
			this._tblRules.ShowCellToolTips = false;
			this._tblRules.ShowEditingIcon = false;
			this._tblRules.ShowRowErrors = false;
			this._tblRules.Size = new System.Drawing.Size(95, 154);
			this._tblRules.TabIndex = 32;
			// 
			// PropertyRuleEngine
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tblRules);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._gbAction);
			this.Controls.Add(this._btnRemove);
			this.Controls.Add(this._btnAdd);
			this.Controls.Add(this._gbRule);
			this.Controls.Add(this._lblName);
			this.Controls.Add(this._tbName);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._title);
			this.Name = "PropertyRuleEngine";
			this.Size = new System.Drawing.Size(490, 494);
			this._gbRule.ResumeLayout(false);
			this._gbRule.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this._gbAction.ResumeLayout(false);
			this._gbAction.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._tblRules)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _title;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox _lblName;
		private System.Windows.Forms.TextBox _tbName;
		private DirectionRose _directionRose;
		private System.Windows.Forms.GroupBox _gbRule;
		private System.Windows.Forms.Label _lblSeconds;
		private System.Windows.Forms.CheckBox _cbAbandoning;
		private System.Windows.Forms.NumericUpDown numericUpDown4;
		private System.Windows.Forms.Label _lblmeters2;
		private System.Windows.Forms.CheckBox _cbRunning;
		private System.Windows.Forms.NumericUpDown numericUpDown3;
		private System.Windows.Forms.Label _lblwhithin;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.CheckBox _cbLoitering;
		private System.Windows.Forms.Label _lbldirection;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.CheckBox _cbMoving;
		private System.Windows.Forms.Button _btnAdd;
		private System.Windows.Forms.Button _btnRemove;
		private System.Windows.Forms.GroupBox _gbAction;
		private System.Windows.Forms.CheckBox _lblRecord;
		private System.Windows.Forms.CheckBox _lblAAnalogue;
		private System.Windows.Forms.CheckBox _lblARelay;
		private System.Windows.Forms.CheckBox _lblAFramerate;
		private System.Windows.Forms.CheckBox _lblAOnvif;
		private System.Windows.Forms.CheckBox _lblPTZ;
		private System.Windows.Forms.Label _lblmeters;
		private System.Windows.Forms.Label _lblspeed;
		private System.Windows.Forms.ComboBox _cmbRelays;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown5;
		private SaveCancelControl _saveCancelControl;
		private System.Windows.Forms.DataGridView _tblRules;

	}
}
