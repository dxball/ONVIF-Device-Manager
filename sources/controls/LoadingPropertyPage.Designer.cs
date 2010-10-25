namespace nvc.controls
{
    partial class LoadingPropertyPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this._progressLoading = new System.Windows.Forms.ProgressBar();
			this._title = new nvc.controls.GroupBoxControl();
			this.SuspendLayout();
			// 
			// _progressLoading
			// 
			this._progressLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._progressLoading.Location = new System.Drawing.Point(3, 36);
			this._progressLoading.Name = "_progressLoading";
			this._progressLoading.Size = new System.Drawing.Size(319, 23);
			this._progressLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this._progressLoading.TabIndex = 0;
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(319, 23);
			this._title.TabIndex = 1;
			// 
			// LoadingPropertyPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._title);
			this.Controls.Add(this._progressLoading);
			this.Name = "LoadingPropertyPage";
			this.Size = new System.Drawing.Size(325, 78);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar _progressLoading;
        private GroupBoxControl _title;
    }
}
