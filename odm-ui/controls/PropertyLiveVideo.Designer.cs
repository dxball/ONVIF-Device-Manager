namespace nvc.controls
{
    partial class PropertyLiveVideo
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
			this._title = new nvc.controls.GroupBoxControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this._uriBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(420, 23);
			this._title.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(3, 26);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(420, 273);
			this.panel1.TabIndex = 1;
			// 
			// _uriBox
			// 
			this._uriBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._uriBox.Location = new System.Drawing.Point(3, 305);
			this._uriBox.Name = "_uriBox";
			this._uriBox.Size = new System.Drawing.Size(420, 20);
			this._uriBox.TabIndex = 2;
			// 
			// PropertyLiveVideo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._uriBox);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._title);
			this.Name = "PropertyLiveVideo";
			this.Size = new System.Drawing.Size(426, 328);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private GroupBoxControl _title;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox _uriBox;
    }
}
