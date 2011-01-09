namespace odm.controls {
	partial class PropertyDisplayAnnotation {
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
			this._title = new odm.controls.GroupBoxControl();
			this._cbTimeStamp = new System.Windows.Forms.CheckBox();
			this._cbTrajectories = new System.Windows.Forms.CheckBox();
			this._cbObjects = new System.Windows.Forms.CheckBox();
			this._saveCancelControl = new odm.controls.SaveCancelControl();
			this._cbSpeed = new System.Windows.Forms.CheckBox();
			this._cbUserRegion = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(3, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(562, 247);
			this.panel1.TabIndex = 3;
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(562, 23);
			this._title.TabIndex = 2;
			// 
			// _cbTimeStamp
			// 
			this._cbTimeStamp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._cbTimeStamp.AutoSize = true;
			this._cbTimeStamp.Location = new System.Drawing.Point(3, 295);
			this._cbTimeStamp.Name = "_cbTimeStamp";
			this._cbTimeStamp.Size = new System.Drawing.Size(80, 17);
			this._cbTimeStamp.TabIndex = 4;
			this._cbTimeStamp.Text = "Time stamp";
			this._cbTimeStamp.UseVisualStyleBackColor = true;
			// 
			// _cbTrajectories
			// 
			this._cbTrajectories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._cbTrajectories.AutoSize = true;
			this._cbTrajectories.Location = new System.Drawing.Point(3, 341);
			this._cbTrajectories.Name = "_cbTrajectories";
			this._cbTrajectories.Size = new System.Drawing.Size(81, 17);
			this._cbTrajectories.TabIndex = 5;
			this._cbTrajectories.Text = "Trejectories";
			this._cbTrajectories.UseVisualStyleBackColor = true;
			// 
			// _cbObjects
			// 
			this._cbObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._cbObjects.AutoSize = true;
			this._cbObjects.Location = new System.Drawing.Point(3, 318);
			this._cbObjects.Name = "_cbObjects";
			this._cbObjects.Size = new System.Drawing.Size(62, 17);
			this._cbObjects.TabIndex = 6;
			this._cbObjects.Text = "Objects";
			this._cbObjects.UseVisualStyleBackColor = true;
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(3, 426);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(202, 23);
			this._saveCancelControl.TabIndex = 24;
			// 
			// _cbSpeed
			// 
			this._cbSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._cbSpeed.AutoSize = true;
			this._cbSpeed.Location = new System.Drawing.Point(3, 364);
			this._cbSpeed.Name = "_cbSpeed";
			this._cbSpeed.Size = new System.Drawing.Size(57, 17);
			this._cbSpeed.TabIndex = 26;
			this._cbSpeed.Text = "Speed";
			this._cbSpeed.UseVisualStyleBackColor = true;
			// 
			// _cbUserRegion
			// 
			this._cbUserRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._cbUserRegion.AutoSize = true;
			this._cbUserRegion.Location = new System.Drawing.Point(3, 387);
			this._cbUserRegion.Name = "_cbUserRegion";
			this._cbUserRegion.Size = new System.Drawing.Size(80, 17);
			this._cbUserRegion.TabIndex = 25;
			this._cbUserRegion.Text = "User region";
			this._cbUserRegion.UseVisualStyleBackColor = true;
			// 
			// PropertyDisplayAnnotation
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._cbSpeed);
			this.Controls.Add(this._cbUserRegion);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._cbObjects);
			this.Controls.Add(this._cbTrajectories);
			this.Controls.Add(this._cbTimeStamp);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._title);
			this.Name = "PropertyDisplayAnnotation";
			this.Size = new System.Drawing.Size(568, 470);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private GroupBoxControl _title;
		private System.Windows.Forms.CheckBox _cbTimeStamp;
		private System.Windows.Forms.CheckBox _cbTrajectories;
		private System.Windows.Forms.CheckBox _cbObjects;
		private SaveCancelControl _saveCancelControl;
		private System.Windows.Forms.CheckBox _cbSpeed;
		private System.Windows.Forms.CheckBox _cbUserRegion;
	}
}
