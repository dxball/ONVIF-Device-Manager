namespace odm.controls {
	partial class PropertyDepthCalibration {
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
			this._rbHeight = new System.Windows.Forms.RadioButton();
			this._rb2D = new System.Windows.Forms.RadioButton();
			this._saveCancelControl = new odm.controls.SaveCancelControl();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._cbMatrixFormat = new System.Windows.Forms.ComboBox();
			this._tbFocalLength = new System.Windows.Forms.TextBox();
			this._lblMatrixFormat = new System.Windows.Forms.TextBox();
			this._lblFocalLength = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
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
			this.panel1.Location = new System.Drawing.Point(0, 29);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(421, 287);
			this.panel1.TabIndex = 1;
			// 
			// _rbHeight
			// 
			this._rbHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._rbHeight.AutoSize = true;
			this._rbHeight.Checked = true;
			this._rbHeight.Location = new System.Drawing.Point(3, 336);
			this._rbHeight.Name = "_rbHeight";
			this._rbHeight.Size = new System.Drawing.Size(91, 17);
			this._rbHeight.TabIndex = 2;
			this._rbHeight.TabStop = true;
			this._rbHeight.Text = "Height marker";
			this._rbHeight.UseVisualStyleBackColor = true;
			// 
			// _rb2D
			// 
			this._rb2D.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._rb2D.AutoSize = true;
			this._rb2D.Location = new System.Drawing.Point(3, 359);
			this._rb2D.Name = "_rb2D";
			this._rb2D.Size = new System.Drawing.Size(74, 17);
			this._rb2D.TabIndex = 3;
			this._rb2D.Text = "2D marker";
			this._rb2D.UseVisualStyleBackColor = true;
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(3, 394);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(206, 23);
			this._saveCancelControl.TabIndex = 4;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this._cbMatrixFormat);
			this.groupBox1.Controls.Add(this._tbFocalLength);
			this.groupBox1.Controls.Add(this._lblMatrixFormat);
			this.groupBox1.Controls.Add(this._lblFocalLength);
			this.groupBox1.Location = new System.Drawing.Point(111, 322);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(310, 69);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			// 
			// _cbMatrixFormat
			// 
			this._cbMatrixFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cbMatrixFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cbMatrixFormat.FormattingEnabled = true;
			this._cbMatrixFormat.Location = new System.Drawing.Point(159, 37);
			this._cbMatrixFormat.Name = "_cbMatrixFormat";
			this._cbMatrixFormat.Size = new System.Drawing.Size(145, 21);
			this._cbMatrixFormat.TabIndex = 28;
			// 
			// _tbFocalLength
			// 
			this._tbFocalLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbFocalLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbFocalLength.Location = new System.Drawing.Point(159, 13);
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
			this._lblMatrixFormat.Size = new System.Drawing.Size(147, 20);
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
			this._lblFocalLength.Size = new System.Drawing.Size(147, 20);
			this._lblFocalLength.TabIndex = 21;
			this._lblFocalLength.TabStop = false;
			this._lblFocalLength.Text = "Focal length, mm";
			// 
			// PropertyDepthCalibration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._rb2D);
			this.Controls.Add(this._rbHeight);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._title);
			this.Name = "PropertyDepthCalibration";
			this.Size = new System.Drawing.Size(424, 427);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _title;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton _rbHeight;
		private System.Windows.Forms.RadioButton _rb2D;
		private SaveCancelControl _saveCancelControl;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox _lblMatrixFormat;
		private System.Windows.Forms.TextBox _lblFocalLength;
		private System.Windows.Forms.TextBox _tbFocalLength;
		private System.Windows.Forms.ComboBox _cbMatrixFormat;
	}
}
