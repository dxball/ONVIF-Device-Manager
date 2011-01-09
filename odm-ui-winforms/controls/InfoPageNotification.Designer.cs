namespace odm.controls {
	partial class InfoPageNotification {
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
			this._progressLoading = new System.Windows.Forms.ProgressBar();
			this._infoTextBox = new System.Windows.Forms.TextBox();
			this._btnClose = new System.Windows.Forms.Button();
			this._btnViewDetails = new System.Windows.Forms.Button();
			this._DetailBrowser = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(424, 23);
			this._title.TabIndex = 1;
			// 
			// _progressLoading
			// 
			this._progressLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._progressLoading.Location = new System.Drawing.Point(6, 112);
			this._progressLoading.Name = "_progressLoading";
			this._progressLoading.Size = new System.Drawing.Size(424, 23);
			this._progressLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this._progressLoading.TabIndex = 2;
			// 
			// _infoTextBox
			// 
			this._infoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._infoTextBox.BackColor = System.Drawing.SystemColors.Control;
			this._infoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._infoTextBox.Location = new System.Drawing.Point(3, 32);
			this._infoTextBox.Multiline = true;
			this._infoTextBox.Name = "_infoTextBox";
			this._infoTextBox.ReadOnly = true;
			this._infoTextBox.Size = new System.Drawing.Size(424, 74);
			this._infoTextBox.TabIndex = 3;
			this._infoTextBox.TabStop = false;
			// 
			// _btnClose
			// 
			this._btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._btnClose.Location = new System.Drawing.Point(352, 141);
			this._btnClose.Name = "_btnClose";
			this._btnClose.Size = new System.Drawing.Size(75, 23);
			this._btnClose.TabIndex = 4;
			this._btnClose.Text = "button1";
			this._btnClose.UseVisualStyleBackColor = true;
			// 
			// _btnViewDetails
			// 
			this._btnViewDetails.Location = new System.Drawing.Point(6, 169);
			this._btnViewDetails.Name = "_btnViewDetails";
			this._btnViewDetails.Size = new System.Drawing.Size(75, 23);
			this._btnViewDetails.TabIndex = 5;
			this._btnViewDetails.Text = "Details";
			this._btnViewDetails.UseVisualStyleBackColor = true;
			this._btnViewDetails.Visible = false;
			// 
			// _DetailBrowser
			// 
			this._DetailBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._DetailBrowser.Location = new System.Drawing.Point(6, 198);
			this._DetailBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this._DetailBrowser.Name = "_DetailBrowser";
			this._DetailBrowser.Size = new System.Drawing.Size(421, 210);
			this._DetailBrowser.TabIndex = 6;
			this._DetailBrowser.Visible = false;
			// 
			// InfoPageNotification
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._DetailBrowser);
			this.Controls.Add(this._btnViewDetails);
			this.Controls.Add(this._btnClose);
			this.Controls.Add(this._infoTextBox);
			this.Controls.Add(this._progressLoading);
			this.Controls.Add(this._title);
			this.Name = "InfoPageNotification";
			this.Size = new System.Drawing.Size(430, 420);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _title;
		private System.Windows.Forms.ProgressBar _progressLoading;
		private System.Windows.Forms.TextBox _infoTextBox;
		private System.Windows.Forms.Button _btnClose;
		private System.Windows.Forms.Button _btnViewDetails;
		public System.Windows.Forms.WebBrowser _DetailBrowser;
	}
}
