namespace odm.controls {
	partial class PropertyObjectTracker {
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
			this._title = new odm.controls.GroupBoxControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this._numtbContrast = new System.Windows.Forms.NumericUpDown();
			this._lblContrast = new System.Windows.Forms.TextBox();
			this._numtbAreaMin = new System.Windows.Forms.NumericUpDown();
			this._tbObjAreaMin = new System.Windows.Forms.TextBox();
			this._numtbAreaMax = new System.Windows.Forms.NumericUpDown();
			this._tbObjAreaMax = new System.Windows.Forms.TextBox();
			this._numtbSpeedMax = new System.Windows.Forms.NumericUpDown();
			this._tbSpeedMax = new System.Windows.Forms.TextBox();
			this._numtbTime = new System.Windows.Forms.NumericUpDown();
			this._tbStabilization = new System.Windows.Forms.TextBox();
			this._saveCancelControl = new odm.controls.SaveCancelControl();
			this._directionRose = new odm.controls.DirectionRose();
			this._lbldirection = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this._numtbContrast)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._numtbAreaMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._numtbAreaMax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._numtbSpeedMax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._numtbTime)).BeginInit();
			this.SuspendLayout();
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(418, 23);
			this._title.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(3, 27);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(418, 271);
			this.panel1.TabIndex = 1;
			// 
			// _numtbContrast
			// 
			this._numtbContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._numtbContrast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._numtbContrast.Location = new System.Drawing.Point(318, 304);
			this._numtbContrast.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this._numtbContrast.Name = "_numtbContrast";
			this._numtbContrast.Size = new System.Drawing.Size(95, 20);
			this._numtbContrast.TabIndex = 11;
			// 
			// _lblContrast
			// 
			this._lblContrast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblContrast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblContrast.Location = new System.Drawing.Point(104, 304);
			this._lblContrast.Name = "_lblContrast";
			this._lblContrast.ReadOnly = true;
			this._lblContrast.Size = new System.Drawing.Size(208, 20);
			this._lblContrast.TabIndex = 10;
			this._lblContrast.Text = "Contrast";
			// 
			// _numtbAreaMin
			// 
			this._numtbAreaMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._numtbAreaMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._numtbAreaMin.DecimalPlaces = 2;
			this._numtbAreaMin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this._numtbAreaMin.Location = new System.Drawing.Point(318, 330);
			this._numtbAreaMin.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this._numtbAreaMin.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            131072});
			this._numtbAreaMin.Name = "_numtbAreaMin";
			this._numtbAreaMin.Size = new System.Drawing.Size(95, 20);
			this._numtbAreaMin.TabIndex = 13;
			this._numtbAreaMin.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
			// 
			// _tbObjAreaMin
			// 
			this._tbObjAreaMin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbObjAreaMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbObjAreaMin.Location = new System.Drawing.Point(104, 330);
			this._tbObjAreaMin.Name = "_tbObjAreaMin";
			this._tbObjAreaMin.ReadOnly = true;
			this._tbObjAreaMin.Size = new System.Drawing.Size(208, 20);
			this._tbObjAreaMin.TabIndex = 12;
			this._tbObjAreaMin.Text = "Object area min";
			// 
			// _numtbAreaMax
			// 
			this._numtbAreaMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._numtbAreaMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._numtbAreaMax.DecimalPlaces = 2;
			this._numtbAreaMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this._numtbAreaMax.Location = new System.Drawing.Point(318, 356);
			this._numtbAreaMax.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this._numtbAreaMax.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            131072});
			this._numtbAreaMax.Name = "_numtbAreaMax";
			this._numtbAreaMax.Size = new System.Drawing.Size(95, 20);
			this._numtbAreaMax.TabIndex = 15;
			this._numtbAreaMax.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
			// 
			// _tbObjAreaMax
			// 
			this._tbObjAreaMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbObjAreaMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbObjAreaMax.Location = new System.Drawing.Point(104, 356);
			this._tbObjAreaMax.Name = "_tbObjAreaMax";
			this._tbObjAreaMax.ReadOnly = true;
			this._tbObjAreaMax.Size = new System.Drawing.Size(208, 20);
			this._tbObjAreaMax.TabIndex = 14;
			this._tbObjAreaMax.Text = "Object area max";
			// 
			// _numtbSpeedMax
			// 
			this._numtbSpeedMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._numtbSpeedMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._numtbSpeedMax.Location = new System.Drawing.Point(318, 382);
			this._numtbSpeedMax.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this._numtbSpeedMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numtbSpeedMax.Name = "_numtbSpeedMax";
			this._numtbSpeedMax.Size = new System.Drawing.Size(95, 20);
			this._numtbSpeedMax.TabIndex = 17;
			this._numtbSpeedMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// _tbSpeedMax
			// 
			this._tbSpeedMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbSpeedMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbSpeedMax.Location = new System.Drawing.Point(104, 382);
			this._tbSpeedMax.Name = "_tbSpeedMax";
			this._tbSpeedMax.ReadOnly = true;
			this._tbSpeedMax.Size = new System.Drawing.Size(208, 20);
			this._tbSpeedMax.TabIndex = 16;
			this._tbSpeedMax.Text = "Speed max";
			// 
			// _numtbTime
			// 
			this._numtbTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._numtbTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._numtbTime.Location = new System.Drawing.Point(318, 408);
			this._numtbTime.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
			this._numtbTime.Name = "_numtbTime";
			this._numtbTime.Size = new System.Drawing.Size(95, 20);
			this._numtbTime.TabIndex = 19;
			this._numtbTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// _tbStabilization
			// 
			this._tbStabilization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbStabilization.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbStabilization.Location = new System.Drawing.Point(104, 408);
			this._tbStabilization.Name = "_tbStabilization";
			this._tbStabilization.ReadOnly = true;
			this._tbStabilization.Size = new System.Drawing.Size(208, 20);
			this._tbStabilization.TabIndex = 18;
			this._tbStabilization.Text = "Stabilization Time";
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(3, 438);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(202, 23);
			this._saveCancelControl.TabIndex = 20;
			// 
			// _directionRose
			// 
			this._directionRose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._directionRose.Location = new System.Drawing.Point(6, 322);
			this._directionRose.Name = "_directionRose";
			this._directionRose.Size = new System.Drawing.Size(80, 80);
			this._directionRose.TabIndex = 21;
			// 
			// _lbldirection
			// 
			this._lbldirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lbldirection.AutoSize = true;
			this._lbldirection.Location = new System.Drawing.Point(3, 306);
			this._lbldirection.Name = "_lbldirection";
			this._lbldirection.Size = new System.Drawing.Size(66, 13);
			this._lbldirection.TabIndex = 22;
			this._lbldirection.Text = "in directions:";
			// 
			// PropertyObjectTracker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._lbldirection);
			this.Controls.Add(this._directionRose);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._numtbTime);
			this.Controls.Add(this._tbStabilization);
			this.Controls.Add(this._numtbSpeedMax);
			this.Controls.Add(this._tbSpeedMax);
			this.Controls.Add(this._numtbAreaMax);
			this.Controls.Add(this._tbObjAreaMax);
			this.Controls.Add(this._numtbAreaMin);
			this.Controls.Add(this._tbObjAreaMin);
			this.Controls.Add(this._numtbContrast);
			this.Controls.Add(this._lblContrast);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._title);
			this.Name = "PropertyObjectTracker";
			this.Size = new System.Drawing.Size(424, 467);
			((System.ComponentModel.ISupportInitialize)(this._numtbContrast)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._numtbAreaMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._numtbAreaMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._numtbSpeedMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._numtbTime)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _title;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.NumericUpDown _numtbContrast;
		private System.Windows.Forms.TextBox _lblContrast;
		private System.Windows.Forms.NumericUpDown _numtbAreaMin;
		private System.Windows.Forms.TextBox _tbObjAreaMin;
		private System.Windows.Forms.NumericUpDown _numtbAreaMax;
		private System.Windows.Forms.TextBox _tbObjAreaMax;
		private System.Windows.Forms.NumericUpDown _numtbSpeedMax;
		private System.Windows.Forms.TextBox _tbSpeedMax;
		private System.Windows.Forms.TextBox _tbStabilization;
		private SaveCancelControl _saveCancelControl;
		private System.Windows.Forms.NumericUpDown _numtbTime;
		private DirectionRose _directionRose;
		private System.Windows.Forms.Label _lbldirection;
	}
}
