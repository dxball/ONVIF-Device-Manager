namespace nvc.controls {
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
			this._title = new nvc.controls.GroupBoxControl();
			this._progressLoading = new System.Windows.Forms.ProgressBar();
			this._infoTextBox = new System.Windows.Forms.TextBox();
			this._btnClose = new System.Windows.Forms.Button();
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
			this._progressLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._progressLoading.Location = new System.Drawing.Point(3, 112);
			this._progressLoading.Name = "_progressLoading";
			this._progressLoading.Size = new System.Drawing.Size(424, 23);
			this._progressLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this._progressLoading.TabIndex = 2;
			// 
			// _infoTextBox
			// 
			this._infoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
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
			this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnClose.Location = new System.Drawing.Point(352, 141);
			this._btnClose.Name = "_btnClose";
			this._btnClose.Size = new System.Drawing.Size(75, 23);
			this._btnClose.TabIndex = 4;
			this._btnClose.Text = "button1";
			this._btnClose.UseVisualStyleBackColor = true;
			// 
			// InfoPageNotification
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._btnClose);
			this.Controls.Add(this._infoTextBox);
			this.Controls.Add(this._progressLoading);
			this.Controls.Add(this._title);
			this.Name = "InfoPageNotification";
			this.Size = new System.Drawing.Size(430, 180);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _title;
		private System.Windows.Forms.ProgressBar _progressLoading;
		private System.Windows.Forms.TextBox _infoTextBox;
		private System.Windows.Forms.Button _btnClose;
	}
}
