namespace odm.controls {
	partial class InformationForm {
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
			this._webBrowser = new System.Windows.Forms.WebBrowser();
			this.savingSettingsControl1 = new odm.controls.SavingSettingsControl();
			this.SuspendLayout();
			// 
			// _webBrowser
			// 
			this._webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._webBrowser.Location = new System.Drawing.Point(0, 105);
			this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this._webBrowser.Name = "_webBrowser";
			this._webBrowser.Size = new System.Drawing.Size(683, 420);
			this._webBrowser.TabIndex = 1;
			// 
			// savingSettingsControl1
			// 
			this.savingSettingsControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.savingSettingsControl1.BackColor = System.Drawing.SystemColors.Control;
			this.savingSettingsControl1.Location = new System.Drawing.Point(0, 0);
			this.savingSettingsControl1.Name = "savingSettingsControl1";
			this.savingSettingsControl1.Size = new System.Drawing.Size(683, 99);
			this.savingSettingsControl1.TabIndex = 0;
			// 
			// InformationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(683, 528);
			this.ControlBox = false;
			this.Controls.Add(this._webBrowser);
			this.Controls.Add(this.savingSettingsControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InformationForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SavingSettingsForm";
			this.Load += new System.EventHandler(this.SavingSettingsForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private SavingSettingsControl savingSettingsControl1;
		private System.Windows.Forms.WebBrowser _webBrowser;
	}
}