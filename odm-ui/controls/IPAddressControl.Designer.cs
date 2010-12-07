namespace nvc.controls {
	partial class IPAddressControl {
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
			this.components = new System.ComponentModel.Container();
			this._IPtextBox = new System.Windows.Forms.TextBox();
			this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// _IPtextBox
			// 
			this._IPtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._IPtextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._IPtextBox.Location = new System.Drawing.Point(-1, -1);
			this._IPtextBox.Margin = new System.Windows.Forms.Padding(0);
			this._IPtextBox.Name = "_IPtextBox";
			this._IPtextBox.Size = new System.Drawing.Size(187, 20);
			this._IPtextBox.TabIndex = 0;
			// 
			// _errorProvider
			// 
			this._errorProvider.ContainerControl = this;
			// 
			// IPAddressControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._IPtextBox);
			this.Name = "IPAddressControl";
			this.Size = new System.Drawing.Size(185, 20);
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _IPtextBox;
		private System.Windows.Forms.ErrorProvider _errorProvider;

	}
}
