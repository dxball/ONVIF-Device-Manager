namespace odm.utils.controls {
	partial class PropertyDepthCalibrationSTAControls {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._cbMatrixFormat = new System.Windows.Forms.ComboBox();
			this._tbFocalLength = new System.Windows.Forms.TextBox();
			this._lblMatrixFormat = new System.Windows.Forms.TextBox();
			this._lblFocalLength = new System.Windows.Forms.TextBox();
			this._rb2D = new System.Windows.Forms.RadioButton();
			this._rbHeight = new System.Windows.Forms.RadioButton();
			this._saveCancelControl = new odm.controls.SaveCancelControl();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._cbMatrixFormat);
			this.groupBox1.Controls.Add(this._tbFocalLength);
			this.groupBox1.Controls.Add(this._lblMatrixFormat);
			this.groupBox1.Controls.Add(this._lblFocalLength);
			this.groupBox1.Location = new System.Drawing.Point(112, -1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(302, 69);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			// 
			// _cbMatrixFormat
			// 
			this._cbMatrixFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cbMatrixFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cbMatrixFormat.FormattingEnabled = true;
			this._cbMatrixFormat.Location = new System.Drawing.Point(151, 37);
			this._cbMatrixFormat.Name = "_cbMatrixFormat";
			this._cbMatrixFormat.Size = new System.Drawing.Size(145, 21);
			this._cbMatrixFormat.TabIndex = 28;
			// 
			// _tbFocalLength
			// 
			this._tbFocalLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbFocalLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbFocalLength.Location = new System.Drawing.Point(151, 13);
			this._tbFocalLength.Name = "_tbFocalLength";
			this._tbFocalLength.Size = new System.Drawing.Size(145, 20);
			this._tbFocalLength.TabIndex = 27;
			// 
			// _lblMatrixFormat
			// 
			this._lblMatrixFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblMatrixFormat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblMatrixFormat.Location = new System.Drawing.Point(6, 38);
			this._lblMatrixFormat.Name = "_lblMatrixFormat";
			this._lblMatrixFormat.ReadOnly = true;
			this._lblMatrixFormat.Size = new System.Drawing.Size(139, 20);
			this._lblMatrixFormat.TabIndex = 25;
			this._lblMatrixFormat.TabStop = false;
			this._lblMatrixFormat.Text = "Matrix format";
			// 
			// _lblFocalLength
			// 
			this._lblFocalLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblFocalLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblFocalLength.Location = new System.Drawing.Point(6, 13);
			this._lblFocalLength.Name = "_lblFocalLength";
			this._lblFocalLength.ReadOnly = true;
			this._lblFocalLength.Size = new System.Drawing.Size(139, 20);
			this._lblFocalLength.TabIndex = 21;
			this._lblFocalLength.TabStop = false;
			this._lblFocalLength.Text = "Focal length, mm";
			// 
			// _rb2D
			// 
			this._rb2D.AutoSize = true;
			this._rb2D.Location = new System.Drawing.Point(4, 36);
			this._rb2D.Name = "_rb2D";
			this._rb2D.Size = new System.Drawing.Size(74, 17);
			this._rb2D.TabIndex = 7;
			this._rb2D.Text = "2D marker";
			this._rb2D.UseVisualStyleBackColor = true;
			// 
			// _rbHeight
			// 
			this._rbHeight.AutoSize = true;
			this._rbHeight.Checked = true;
			this._rbHeight.Location = new System.Drawing.Point(4, 13);
			this._rbHeight.Name = "_rbHeight";
			this._rbHeight.Size = new System.Drawing.Size(91, 17);
			this._rbHeight.TabIndex = 6;
			this._rbHeight.TabStop = true;
			this._rbHeight.Text = "Height marker";
			this._rbHeight.UseVisualStyleBackColor = true;
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(4, 71);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(206, 23);
			this._saveCancelControl.TabIndex = 8;
			// 
			// PropertyDepthCalibrationSTAControls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(420, 96);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._rb2D);
			this.Controls.Add(this._rbHeight);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PropertyDepthCalibrationSTAControls";
			this.ShowInTaskbar = false;
			this.Text = "PropertyDepthCalibrationSTAControls";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.ComboBox _cbMatrixFormat;
		public System.Windows.Forms.TextBox _tbFocalLength;
		public System.Windows.Forms.TextBox _lblMatrixFormat;
		public System.Windows.Forms.TextBox _lblFocalLength;
		public odm.controls.SaveCancelControl _saveCancelControl;
		public System.Windows.Forms.RadioButton _rb2D;
		public System.Windows.Forms.RadioButton _rbHeight;
		public System.Windows.Forms.GroupBox groupBox1;
	}
}