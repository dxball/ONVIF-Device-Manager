namespace nvc.controls
{
    partial class DevicesListControl
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
			this._btnGetDump = new System.Windows.Forms.Button();
			this._title = new nvc.controls.GroupBoxControl();
			this._btnRefresh = new System.Windows.Forms.Button();
			this._panelListView = new System.Windows.Forms.Panel();
			this._lviewDevices = new System.Windows.Forms.ListView();
			this._panelMain.SuspendLayout();
			this._panelListView.SuspendLayout();
			this.SuspendLayout();
			// 
			// _panelMain
			// 
			this._panelMain.Controls.Add(this._btnGetDump);
			this._panelMain.Controls.Add(this._title);
			this._panelMain.Controls.Add(this._btnRefresh);
			this._panelMain.Controls.Add(this._panelListView);
			this._panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelMain.Location = new System.Drawing.Point(0, 0);
			this._panelMain.Name = "_panelMain";
			this._panelMain.Size = new System.Drawing.Size(341, 310);
			this._panelMain.TabIndex = 0;
			// 
			// _btnGetDump
			// 
			this._btnGetDump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnGetDump.Location = new System.Drawing.Point(260, 284);
			this._btnGetDump.Name = "_btnGetDump";
			this._btnGetDump.Size = new System.Drawing.Size(75, 23);
			this._btnGetDump.TabIndex = 3;
			this._btnGetDump.Text = "Get Dump";
			this._btnGetDump.UseVisualStyleBackColor = true;
			this._btnGetDump.Click += new System.EventHandler(this._btnGetDump_Click);
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(335, 23);
			this._title.TabIndex = 2;
			// 
			// _btnRefresh
			// 
			this._btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._btnRefresh.Location = new System.Drawing.Point(3, 284);
			this._btnRefresh.Name = "_btnRefresh";
			this._btnRefresh.Size = new System.Drawing.Size(75, 23);
			this._btnRefresh.TabIndex = 1;
			this._btnRefresh.Text = "Refresh";
			this._btnRefresh.UseVisualStyleBackColor = true;
			this._btnRefresh.Click += new System.EventHandler(this._btnRefresh_Click);
			// 
			// _panelListView
			// 
			this._panelListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._panelListView.Controls.Add(this._lviewDevices);
			this._panelListView.Location = new System.Drawing.Point(3, 26);
			this._panelListView.Name = "_panelListView";
			this._panelListView.Size = new System.Drawing.Size(335, 252);
			this._panelListView.TabIndex = 0;
			// 
			// _lviewDevices
			// 
			this._lviewDevices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lviewDevices.CausesValidation = false;
			this._lviewDevices.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lviewDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._lviewDevices.FullRowSelect = true;
			this._lviewDevices.GridLines = true;
			this._lviewDevices.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._lviewDevices.Location = new System.Drawing.Point(0, 0);
			this._lviewDevices.MultiSelect = false;
			this._lviewDevices.Name = "_lviewDevices";
			this._lviewDevices.ShowGroups = false;
			this._lviewDevices.Size = new System.Drawing.Size(335, 252);
			this._lviewDevices.TabIndex = 0;
			this._lviewDevices.UseCompatibleStateImageBehavior = false;
			// 
			// DevicesListControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._panelMain);
			this.Name = "DevicesListControl";
			this.Size = new System.Drawing.Size(341, 310);
			this._panelMain.ResumeLayout(false);
			this._panelListView.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _panelMain;
        private System.Windows.Forms.Button _btnRefresh;
        private System.Windows.Forms.Panel _panelListView;
        private System.Windows.Forms.ListView _lviewDevices;
		private GroupBoxControl _title;
		private System.Windows.Forms.Button _btnGetDump;
    }
}
