namespace odm.controls
{
    partial class DeviceControl
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
			this._panelMain = new System.Windows.Forms.Panel();
			this._title = new odm.controls.GroupBoxControl();
			this._grpBox = new System.Windows.Forms.Panel();
			this._flowPanelLinksList = new System.Windows.Forms.FlowLayoutPanel();
			this._imgBox = new System.Windows.Forms.PictureBox();
			this._panelMain.SuspendLayout();
			this._grpBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._imgBox)).BeginInit();
			this.SuspendLayout();
			// 
			// _panelMain
			// 
			this._panelMain.Controls.Add(this._title);
			this._panelMain.Controls.Add(this._grpBox);
			this._panelMain.Controls.Add(this._imgBox);
			this._panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelMain.Location = new System.Drawing.Point(0, 0);
			this._panelMain.Name = "_panelMain";
			this._panelMain.Size = new System.Drawing.Size(325, 139);
			this._panelMain.TabIndex = 0;
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(140, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(185, 23);
			this._title.TabIndex = 3;
			// 
			// _grpBox
			// 
			this._grpBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._grpBox.Controls.Add(this._flowPanelLinksList);
			this._grpBox.Location = new System.Drawing.Point(140, 26);
			this._grpBox.Name = "_grpBox";
			this._grpBox.Size = new System.Drawing.Size(185, 110);
			this._grpBox.TabIndex = 2;
			// 
			// _flowPanelLinksList
			// 
			this._flowPanelLinksList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._flowPanelLinksList.BackColor = System.Drawing.SystemColors.Control;
			this._flowPanelLinksList.Dock = System.Windows.Forms.DockStyle.Fill;
			this._flowPanelLinksList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this._flowPanelLinksList.Location = new System.Drawing.Point(0, 0);
			this._flowPanelLinksList.Name = "_flowPanelLinksList";
			this._flowPanelLinksList.Size = new System.Drawing.Size(185, 110);
			this._flowPanelLinksList.TabIndex = 0;
			this._flowPanelLinksList.WrapContents = false;
			// 
			// _imgBox
			// 
			this._imgBox.BackColor = System.Drawing.SystemColors.Control;
			this._imgBox.Image = global::odm.utils.properties.Resources.other;
			this._imgBox.Location = new System.Drawing.Point(3, 3);
			this._imgBox.Name = "_imgBox";
			this._imgBox.Size = new System.Drawing.Size(137, 100);
			this._imgBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._imgBox.TabIndex = 1;
			this._imgBox.TabStop = false;
			// 
			// DeviceControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._panelMain);
			this.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.Name = "DeviceControl";
			this.Size = new System.Drawing.Size(325, 139);
			this.MouseEnter += new System.EventHandler(this.DeviceControl_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.DeviceControl_MouseLeave);
			this._panelMain.ResumeLayout(false);
			this._grpBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._imgBox)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _panelMain;
        private System.Windows.Forms.PictureBox _imgBox;
        private System.Windows.Forms.FlowLayoutPanel _flowPanelLinksList;
        private GroupBoxControl _title;
        private System.Windows.Forms.Panel _grpBox;
    }
}
