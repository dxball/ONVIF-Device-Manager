namespace odm.controls {
	partial class PropertyMetadata {
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
			//this._tbMetadata = new System.Windows.Forms.TextBox();
			this._tbMetadata = new UserTextBox();
			this.SuspendLayout();
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(279, 23);
			this._title.TabIndex = 1;
			// 
			// _tbMetadata
			// 
			this._tbMetadata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbMetadata.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._tbMetadata.Location = new System.Drawing.Point(3, 32);
			this._tbMetadata.Multiline = true;
			this._tbMetadata.Name = "_tbMetadata";
			this._tbMetadata.ReadOnly = true;
			this._tbMetadata.Size = new System.Drawing.Size(276, 169);
			this._tbMetadata.TabIndex = 2;
			this._tbMetadata.TabStop = false;
			// 
			// PropertyMetadata
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tbMetadata);
			this.Controls.Add(this._title);
			this.Name = "PropertyMetadata";
			this.Size = new System.Drawing.Size(282, 204);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _title;
		//private System.Windows.Forms.TextBox _tbMetadata;
		private UserTextBox _tbMetadata;
	}
}
