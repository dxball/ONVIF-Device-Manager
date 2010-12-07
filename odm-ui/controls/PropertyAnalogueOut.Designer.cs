namespace nvc.controls {
	partial class PropertyAnalogueOut {
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
			this._title = new nvc.controls.GroupBoxControl();
			this._saveCancelControl = new nvc.controls.SaveCancelControl();
			this._lblLoop = new System.Windows.Forms.RadioButton();
			this._lbloff = new System.Windows.Forms.RadioButton();
			this._lblDigital = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(322, 23);
			this._title.TabIndex = 2;
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(13, 105);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(208, 23);
			this._saveCancelControl.TabIndex = 23;
			// 
			// _lblLoop
			// 
			this._lblLoop.AutoSize = true;
			this._lblLoop.Location = new System.Drawing.Point(13, 32);
			this._lblLoop.Name = "_lblLoop";
			this._lblLoop.Size = new System.Drawing.Size(85, 17);
			this._lblLoop.TabIndex = 24;
			this._lblLoop.TabStop = true;
			this._lblLoop.Text = "radioButton1";
			this._lblLoop.UseVisualStyleBackColor = true;
			// 
			// _lbloff
			// 
			this._lbloff.AutoSize = true;
			this._lbloff.Location = new System.Drawing.Point(13, 78);
			this._lbloff.Name = "_lbloff";
			this._lbloff.Size = new System.Drawing.Size(85, 17);
			this._lbloff.TabIndex = 25;
			this._lbloff.TabStop = true;
			this._lbloff.Text = "radioButton2";
			this._lbloff.UseVisualStyleBackColor = true;
			// 
			// _lblDigital
			// 
			this._lblDigital.AutoSize = true;
			this._lblDigital.Location = new System.Drawing.Point(13, 55);
			this._lblDigital.Name = "_lblDigital";
			this._lblDigital.Size = new System.Drawing.Size(85, 17);
			this._lblDigital.TabIndex = 25;
			this._lblDigital.TabStop = true;
			this._lblDigital.Text = "radioButton2";
			this._lblDigital.UseVisualStyleBackColor = true;
			// 
			// PropertyAnalogueOut
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._lblDigital);
			this.Controls.Add(this._lbloff);
			this.Controls.Add(this._lblLoop);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._title);
			this.Name = "PropertyAnalogueOut";
			this.Size = new System.Drawing.Size(328, 141);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _title;
		private SaveCancelControl _saveCancelControl;
		private System.Windows.Forms.RadioButton _lblLoop;
		private System.Windows.Forms.RadioButton _lbloff;
		private System.Windows.Forms.RadioButton _lblDigital;
	}
}
