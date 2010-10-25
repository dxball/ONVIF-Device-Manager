namespace nvc.controls
{
    partial class DeviceChannelControl
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
			this.panel1 = new System.Windows.Forms.Panel();
			this._imgBox = new System.Windows.Forms.PictureBox();
			this._flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this._grpBox = new System.Windows.Forms.Panel();
			this._title = new nvc.controls.GroupBoxControl();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._imgBox)).BeginInit();
			this._grpBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this._imgBox);
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(140, 100);
			this.panel1.TabIndex = 0;
			// 
			// _imgBox
			// 
			this._imgBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._imgBox.Location = new System.Drawing.Point(0, 0);
			this._imgBox.Name = "_imgBox";
			this._imgBox.Size = new System.Drawing.Size(140, 100);
			this._imgBox.TabIndex = 0;
			this._imgBox.TabStop = false;
			// 
			// _flowLayoutPanel
			// 
			this._flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this._flowLayoutPanel.Name = "_flowLayoutPanel";
			this._flowLayoutPanel.Size = new System.Drawing.Size(182, 153);
			this._flowLayoutPanel.TabIndex = 0;
			// 
			// _grpBox
			// 
			this._grpBox.Controls.Add(this._flowLayoutPanel);
			this._grpBox.Location = new System.Drawing.Point(143, 26);
			this._grpBox.Name = "_grpBox";
			this._grpBox.Size = new System.Drawing.Size(182, 153);
			this._grpBox.TabIndex = 1;
			// 
			// _title
			// 
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(143, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(182, 23);
			this._title.TabIndex = 2;
			// 
			// DeviceChannelControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._grpBox);
			this.Controls.Add(this._title);
			this.Controls.Add(this.panel1);
			this.Name = "DeviceChannelControl";
			this.Size = new System.Drawing.Size(325, 183);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._imgBox)).EndInit();
			this._grpBox.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel _flowLayoutPanel;
        private System.Windows.Forms.Panel _grpBox;
        private GroupBoxControl _title;
        private System.Windows.Forms.PictureBox _imgBox;
    }
}
