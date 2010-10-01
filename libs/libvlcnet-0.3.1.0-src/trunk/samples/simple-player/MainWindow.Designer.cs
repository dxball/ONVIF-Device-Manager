using SimplePlayer.Playlist;

namespace SimplePlayer {
    partial class MainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonPlayback = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPlaynext = new System.Windows.Forms.Button();
            this.menustripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.playerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.trackbarPosition = new System.Windows.Forms.TrackBar();
            this.trackbarVolume = new System.Windows.Forms.TrackBar();
            this.playlistEditorControl = new PlaylistEditorControl();
            this.menustripMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.trackbarPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackbarVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            this.buttonPlay.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlay.Location = new System.Drawing.Point(0, 312);
            this.buttonPlay.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(56, 23);
            this.buttonPlay.TabIndex = 2;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonPlayback
            // 
            this.buttonPlayback.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlayback.Location = new System.Drawing.Point(60, 312);
            this.buttonPlayback.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPlayback.Name = "buttonPlayback";
            this.buttonPlayback.Size = new System.Drawing.Size(56, 23);
            this.buttonPlayback.TabIndex = 3;
            this.buttonPlayback.Text = "<<";
            this.buttonPlayback.UseVisualStyleBackColor = true;
            this.buttonPlayback.Click += new System.EventHandler(this.buttonPlayback_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Location = new System.Drawing.Point(120, 312);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(56, 23);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPlaynext
            // 
            this.buttonPlaynext.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlaynext.Location = new System.Drawing.Point(180, 312);
            this.buttonPlaynext.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPlaynext.Name = "buttonPlaynext";
            this.buttonPlaynext.Size = new System.Drawing.Size(56, 23);
            this.buttonPlaynext.TabIndex = 5;
            this.buttonPlaynext.Text = ">>";
            this.buttonPlaynext.UseVisualStyleBackColor = true;
            this.buttonPlaynext.Click += new System.EventHandler(this.buttonPlaynext_Click);
            // 
            // menustripMain
            // 
            this.menustripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menustripMain.Location = new System.Drawing.Point(0, 0);
            this.menustripMain.Name = "menustripMain";
            this.menustripMain.Size = new System.Drawing.Size(292, 24);
            this.menustripMain.TabIndex = 6;
            this.menustripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFilesToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFilesToolStripMenuItem
            // 
            this.openFilesToolStripMenuItem.Name = "openFilesToolStripMenuItem";
            this.openFilesToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.openFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFilesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.openFilesToolStripMenuItem.Text = "OpenFile file(s)..";
            this.openFilesToolStripMenuItem.Click += new System.EventHandler(this.openFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playerStatus,
            this.currentTime});
            this.statusStrip.Location = new System.Drawing.Point(0, 344);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(292, 22);
            this.statusStrip.TabIndex = 7;
            // 
            // playerStatus
            // 
            this.playerStatus.Name = "playerStatus";
            this.playerStatus.Size = new System.Drawing.Size(37, 17);
            this.playerStatus.Text = "Status";
            // 
            // currentTime
            // 
            this.currentTime.Name = "currentTime";
            this.currentTime.Size = new System.Drawing.Size(240, 17);
            this.currentTime.Spring = true;
            this.currentTime.Text = "00:01:40";
            this.currentTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackbarPosition
            // 
            this.trackbarPosition.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackbarPosition.Location = new System.Drawing.Point(0, 268);
            this.trackbarPosition.Margin = new System.Windows.Forms.Padding(0);
            this.trackbarPosition.Maximum = 1000;
            this.trackbarPosition.Name = "trackbarPosition";
            this.trackbarPosition.Size = new System.Drawing.Size(247, 42);
            this.trackbarPosition.TabIndex = 8;
            this.trackbarPosition.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackbarPosition.ValueChanged += new System.EventHandler(this.trackbarPosition_ValueChanged);
            // 
            // trackbarVolume
            // 
            this.trackbarVolume.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trackbarVolume.Location = new System.Drawing.Point(250, 268);
            this.trackbarVolume.Maximum = 15;
            this.trackbarVolume.Name = "trackbarVolume";
            this.trackbarVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackbarVolume.Size = new System.Drawing.Size(42, 74);
            this.trackbarVolume.TabIndex = 9;
            this.trackbarVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackbarVolume.ValueChanged += new System.EventHandler(this.trackbarVolume_ValueChanged);
            // 
            // playlistEditorControl
            // 
            this.playlistEditorControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistEditorControl.AutoSize = true;
            this.playlistEditorControl.BackColor = System.Drawing.Color.Black;
            this.playlistEditorControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playlistEditorControl.Location = new System.Drawing.Point(0, 26);
            this.playlistEditorControl.Margin = new System.Windows.Forms.Padding(2);
            this.playlistEditorControl.Name = "playlistEditorControl";
            this.playlistEditorControl.Size = new System.Drawing.Size(292, 240);
            this.playlistEditorControl.TabIndex = 1;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 366);
            this.Controls.Add(this.trackbarVolume);
            this.Controls.Add(this.trackbarPosition);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.buttonPlaynext);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonPlayback);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.playlistEditorControl);
            this.Controls.Add(this.menustripMain);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menustripMain;
            this.MinimumSize = new System.Drawing.Size(300, 171);
            this.Name = "MainWindow";
            this.Text = "SimplePlayer";
            this.menustripMain.ResumeLayout(false);
            this.menustripMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.trackbarPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackbarVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PlaylistEditorControl playlistEditorControl;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonPlayback;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonPlaynext;
        private System.Windows.Forms.MenuStrip menustripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel playerStatus;
        private System.Windows.Forms.ToolStripStatusLabel currentTime;
        private System.Windows.Forms.TrackBar trackbarPosition;
        private System.Windows.Forms.TrackBar trackbarVolume;
    }
}

