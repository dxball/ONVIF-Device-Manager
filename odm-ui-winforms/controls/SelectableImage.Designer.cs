namespace odm.controls {
	partial class SelectableImage {
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
			this._imgBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this._imgBox)).BeginInit();
			this.SuspendLayout();
			// 
			// _imgBox
			// 
			this._imgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._imgBox.Location = new System.Drawing.Point(3, 3);
			this._imgBox.Name = "_imgBox";
			this._imgBox.Size = new System.Drawing.Size(144, 144);
			this._imgBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._imgBox.TabIndex = 0;
			this._imgBox.TabStop = false;
			// 
			// SelectableImage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Lime;
			this.Controls.Add(this._imgBox);
			this.Name = "SelectableImage";
			((System.ComponentModel.ISupportInitialize)(this._imgBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.PictureBox _imgBox;

	}
}
