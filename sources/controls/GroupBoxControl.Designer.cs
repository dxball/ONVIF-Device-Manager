namespace nvc.controls
{
    partial class GroupBoxControl
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
			this._panelTitle = new System.Windows.Forms.Panel();
			this._lblTitle = new System.Windows.Forms.Label();
			this._panelTitle.SuspendLayout();
			this.SuspendLayout();
			// 
			// _panelTitle
			// 
			this._panelTitle.Controls.Add(this._lblTitle);
			this._panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this._panelTitle.Location = new System.Drawing.Point(0, 0);
			this._panelTitle.Name = "_panelTitle";
			this._panelTitle.Size = new System.Drawing.Size(281, 22);
			this._panelTitle.TabIndex = 1;
			// 
			// _lblTitle
			// 
			this._lblTitle.AutoSize = true;
			this._lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._lblTitle.Location = new System.Drawing.Point(5, 2);
			this._lblTitle.Name = "_lblTitle";
			this._lblTitle.Size = new System.Drawing.Size(46, 17);
			this._lblTitle.TabIndex = 0;
			this._lblTitle.Text = "label1";
			// 
			// GroupBoxControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.Controls.Add(this._panelTitle);
			this.Name = "GroupBoxControl";
			this.Size = new System.Drawing.Size(281, 23);
			this._panelTitle.ResumeLayout(false);
			this._panelTitle.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Panel _panelTitle;
		public System.Windows.Forms.Label _lblTitle;
    }
}
