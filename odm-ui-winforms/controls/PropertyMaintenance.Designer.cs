namespace odm.controls
{
    partial class PropertyMaintenance
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
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this._procBarUpgrade = new System.Windows.Forms.ProgressBar();
			this._linkSupport = new System.Windows.Forms.LinkLabel();
			this._tbFirmwareVer = new System.Windows.Forms.TextBox();
			this._btnUpgrade = new System.Windows.Forms.Button();
			this._btnDiagnostics = new System.Windows.Forms.Button();
			this._btnHardReset = new System.Windows.Forms.Button();
			this._btnSoftReset = new System.Windows.Forms.Button();
			this._btnRestore = new System.Windows.Forms.Button();
			this._btnBackup = new System.Windows.Forms.Button();
			this._lblUpgrade = new System.Windows.Forms.TextBox();
			this._lblFirmwareVer = new System.Windows.Forms.TextBox();
			this._lblDiagnostics = new System.Windows.Forms.TextBox();
			this._lblFactoryReset = new System.Windows.Forms.TextBox();
			this._lblConfig = new System.Windows.Forms.TextBox();
			this._title = new odm.controls.GroupBoxControl();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.linkLabel1);
			this.panel1.Controls.Add(this._procBarUpgrade);
			this.panel1.Controls.Add(this._linkSupport);
			this.panel1.Controls.Add(this._tbFirmwareVer);
			this.panel1.Controls.Add(this._btnUpgrade);
			this.panel1.Controls.Add(this._btnDiagnostics);
			this.panel1.Controls.Add(this._btnHardReset);
			this.panel1.Controls.Add(this._btnSoftReset);
			this.panel1.Controls.Add(this._btnRestore);
			this.panel1.Controls.Add(this._btnBackup);
			this.panel1.Controls.Add(this._lblUpgrade);
			this.panel1.Controls.Add(this._lblFirmwareVer);
			this.panel1.Controls.Add(this._lblDiagnostics);
			this.panel1.Controls.Add(this._lblFactoryReset);
			this.panel1.Controls.Add(this._lblConfig);
			this.panel1.Location = new System.Drawing.Point(3, 26);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(497, 161);
			this.panel1.TabIndex = 18;
			// 
			// linkLabel1
			// 
			this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Enabled = false;
			this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.linkLabel1.Location = new System.Drawing.Point(339, 110);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(156, 13);
			this.linkLabel1.TabIndex = 29;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://magicbox.agrg.ru/firmware";
			this.linkLabel1.Visible = false;
			// 
			// _procBarUpgrade
			// 
			this._procBarUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._procBarUpgrade.Location = new System.Drawing.Point(3, 133);
			this._procBarUpgrade.Name = "_procBarUpgrade";
			this._procBarUpgrade.Size = new System.Drawing.Size(491, 23);
			this._procBarUpgrade.TabIndex = 28;
			// 
			// _linkSupport
			// 
			this._linkSupport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._linkSupport.AutoSize = true;
			this._linkSupport.Enabled = false;
			this._linkSupport.Location = new System.Drawing.Point(342, 57);
			this._linkSupport.Name = "_linkSupport";
			this._linkSupport.Size = new System.Drawing.Size(86, 13);
			this._linkSupport.TabIndex = 27;
			this._linkSupport.TabStop = true;
			this._linkSupport.Text = "support@agrg.ru";
			this._linkSupport.Visible = false;
			// 
			// _tbFirmwareVer
			// 
			this._tbFirmwareVer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbFirmwareVer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbFirmwareVer.Location = new System.Drawing.Point(187, 81);
			this._tbFirmwareVer.Name = "_tbFirmwareVer";
			this._tbFirmwareVer.ReadOnly = true;
			this._tbFirmwareVer.Size = new System.Drawing.Size(152, 20);
			this._tbFirmwareVer.TabIndex = 26;
			// 
			// _btnUpgrade
			// 
			this._btnUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnUpgrade.Location = new System.Drawing.Point(187, 104);
			this._btnUpgrade.Name = "_btnUpgrade";
			this._btnUpgrade.Size = new System.Drawing.Size(152, 23);
			this._btnUpgrade.TabIndex = 25;
			this._btnUpgrade.Text = "Upgrade";
			this._btnUpgrade.UseVisualStyleBackColor = true;
			// 
			// _btnDiagnostics
			// 
			this._btnDiagnostics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnDiagnostics.Enabled = false;
			this._btnDiagnostics.Location = new System.Drawing.Point(187, 52);
			this._btnDiagnostics.Name = "_btnDiagnostics";
			this._btnDiagnostics.Size = new System.Drawing.Size(152, 23);
			this._btnDiagnostics.TabIndex = 24;
			this._btnDiagnostics.Text = "Diagnostics and support";
			this._btnDiagnostics.UseVisualStyleBackColor = true;
			// 
			// _btnHardReset
			// 
			this._btnHardReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnHardReset.Enabled = false;
			this._btnHardReset.Location = new System.Drawing.Point(342, 26);
			this._btnHardReset.Name = "_btnHardReset";
			this._btnHardReset.Size = new System.Drawing.Size(152, 23);
			this._btnHardReset.TabIndex = 23;
			this._btnHardReset.Text = "Hard reset";
			this._btnHardReset.UseVisualStyleBackColor = true;
			// 
			// _btnSoftReset
			// 
			this._btnSoftReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnSoftReset.Location = new System.Drawing.Point(187, 26);
			this._btnSoftReset.Name = "_btnSoftReset";
			this._btnSoftReset.Size = new System.Drawing.Size(152, 23);
			this._btnSoftReset.TabIndex = 22;
			this._btnSoftReset.Text = "Soft reset";
			this._btnSoftReset.UseVisualStyleBackColor = true;
			// 
			// _btnRestore
			// 
			this._btnRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnRestore.Enabled = false;
			this._btnRestore.Location = new System.Drawing.Point(342, 0);
			this._btnRestore.Name = "_btnRestore";
			this._btnRestore.Size = new System.Drawing.Size(152, 23);
			this._btnRestore.TabIndex = 21;
			this._btnRestore.Text = "Restore";
			this._btnRestore.UseVisualStyleBackColor = true;
			// 
			// _btnBackup
			// 
			this._btnBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnBackup.Enabled = false;
			this._btnBackup.Location = new System.Drawing.Point(187, 0);
			this._btnBackup.Name = "_btnBackup";
			this._btnBackup.Size = new System.Drawing.Size(152, 23);
			this._btnBackup.TabIndex = 20;
			this._btnBackup.Text = "Backup";
			this._btnBackup.UseVisualStyleBackColor = true;
			// 
			// _lblUpgrade
			// 
			this._lblUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblUpgrade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblUpgrade.Location = new System.Drawing.Point(3, 107);
			this._lblUpgrade.Name = "_lblUpgrade";
			this._lblUpgrade.ReadOnly = true;
			this._lblUpgrade.Size = new System.Drawing.Size(178, 20);
			this._lblUpgrade.TabIndex = 19;
			this._lblUpgrade.Text = "Upgrade firmware";
			// 
			// _lblFirmwareVer
			// 
			this._lblFirmwareVer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblFirmwareVer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblFirmwareVer.Location = new System.Drawing.Point(3, 81);
			this._lblFirmwareVer.Name = "_lblFirmwareVer";
			this._lblFirmwareVer.ReadOnly = true;
			this._lblFirmwareVer.Size = new System.Drawing.Size(178, 20);
			this._lblFirmwareVer.TabIndex = 18;
			this._lblFirmwareVer.Text = "Current firmware version";
			// 
			// _lblDiagnostics
			// 
			this._lblDiagnostics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblDiagnostics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblDiagnostics.Location = new System.Drawing.Point(3, 55);
			this._lblDiagnostics.Name = "_lblDiagnostics";
			this._lblDiagnostics.ReadOnly = true;
			this._lblDiagnostics.Size = new System.Drawing.Size(178, 20);
			this._lblDiagnostics.TabIndex = 17;
			this._lblDiagnostics.Text = "Diagnostics and support";
			// 
			// _lblFactoryReset
			// 
			this._lblFactoryReset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblFactoryReset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblFactoryReset.Location = new System.Drawing.Point(3, 29);
			this._lblFactoryReset.Name = "_lblFactoryReset";
			this._lblFactoryReset.ReadOnly = true;
			this._lblFactoryReset.Size = new System.Drawing.Size(178, 20);
			this._lblFactoryReset.TabIndex = 16;
			this._lblFactoryReset.Text = "Factory reset";
			// 
			// _lblConfig
			// 
			this._lblConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblConfig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblConfig.Location = new System.Drawing.Point(3, 3);
			this._lblConfig.Name = "_lblConfig";
			this._lblConfig.ReadOnly = true;
			this._lblConfig.Size = new System.Drawing.Size(178, 20);
			this._lblConfig.TabIndex = 15;
			this._lblConfig.Text = "Configuration";
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(497, 23);
			this._title.TabIndex = 21;
			// 
			// PropertyMaintenance
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._title);
			this.Controls.Add(this.panel1);
			this.Name = "PropertyMaintenance";
			this.Size = new System.Drawing.Size(505, 190);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private GroupBoxControl _title;
        private System.Windows.Forms.TextBox _lblUpgrade;
        private System.Windows.Forms.TextBox _lblFirmwareVer;
        private System.Windows.Forms.TextBox _lblDiagnostics;
        private System.Windows.Forms.TextBox _lblFactoryReset;
        private System.Windows.Forms.TextBox _lblConfig;
        private System.Windows.Forms.ProgressBar _procBarUpgrade;
        private System.Windows.Forms.LinkLabel _linkSupport;
        private System.Windows.Forms.TextBox _tbFirmwareVer;
        private System.Windows.Forms.Button _btnUpgrade;
        private System.Windows.Forms.Button _btnDiagnostics;
        private System.Windows.Forms.Button _btnHardReset;
        private System.Windows.Forms.Button _btnSoftReset;
        private System.Windows.Forms.Button _btnRestore;
        private System.Windows.Forms.Button _btnBackup;
		private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
