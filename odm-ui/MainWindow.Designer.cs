namespace nvc {
	partial class MainWindow {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this._mainStatusStrip = new System.Windows.Forms.StatusStrip();
			this._lblStatus1 = new System.Windows.Forms.ToolStripStatusLabel();
			this._lblStatus2 = new System.Windows.Forms.ToolStripStatusLabel();
			this._lblStatus3 = new System.Windows.Forms.ToolStripStatusLabel();
			this._langPanel = new System.Windows.Forms.Panel();
			this.titleBar1 = new nvc.controls.TitleBar();
			this._splitContainerA = new System.Windows.Forms.SplitContainer();
			this.button1 = new System.Windows.Forms.Button();
			this._mainStatusStrip.SuspendLayout();
			this._langPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._splitContainerA)).BeginInit();
			this._splitContainerA.SuspendLayout();
			this.SuspendLayout();
			// 
			// _mainStatusStrip
			// 
			this._mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lblStatus1,
            this._lblStatus2,
            this._lblStatus3});
			this._mainStatusStrip.Location = new System.Drawing.Point(0, 640);
			this._mainStatusStrip.Name = "_mainStatusStrip";
			this._mainStatusStrip.Size = new System.Drawing.Size(1046, 22);
			this._mainStatusStrip.TabIndex = 2;
			this._mainStatusStrip.Text = "_mainStatusStrip";
			// 
			// _lblStatus1
			// 
			this._lblStatus1.Name = "_lblStatus1";
			this._lblStatus1.Size = new System.Drawing.Size(0, 17);
			// 
			// _lblStatus2
			// 
			this._lblStatus2.Name = "_lblStatus2";
			this._lblStatus2.Size = new System.Drawing.Size(0, 17);
			// 
			// _lblStatus3
			// 
			this._lblStatus3.Name = "_lblStatus3";
			this._lblStatus3.Size = new System.Drawing.Size(0, 17);
			// 
			// _langPanel
			// 
			this._langPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._langPanel.Controls.Add(this.titleBar1);
			this._langPanel.Location = new System.Drawing.Point(818, 5);
			this._langPanel.Margin = new System.Windows.Forms.Padding(0);
			this._langPanel.Name = "_langPanel";
			this._langPanel.Size = new System.Drawing.Size(228, 25);
			this._langPanel.TabIndex = 3;
			// 
			// titleBar1
			// 
			this.titleBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.titleBar1.Location = new System.Drawing.Point(122, 0);
			this.titleBar1.Name = "titleBar1";
			this.titleBar1.Size = new System.Drawing.Size(103, 22);
			this.titleBar1.TabIndex = 0;
			// 
			// _splitContainerA
			// 
			this._splitContainerA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._splitContainerA.Location = new System.Drawing.Point(0, 33);
			this._splitContainerA.Name = "_splitContainerA";
			this._splitContainerA.Panel2MinSize = 820;
			this._splitContainerA.Size = new System.Drawing.Size(1046, 604);
			this._splitContainerA.SplitterDistance = 169;
			this._splitContainerA.TabIndex = 4;
			this._splitContainerA.TabStop = false;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.FlatAppearance.BorderSize = 0;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(805, -50);
			this.button1.Margin = new System.Windows.Forms.Padding(0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(10, 10);
			this.button1.TabIndex = 5;
			this.button1.Text = "&l";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1046, 662);
			this.Controls.Add(this.button1);
			this.Controls.Add(this._splitContainerA);
			this.Controls.Add(this._langPanel);
			this.Controls.Add(this._mainStatusStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1054, 700);
			this.Name = "MainWindow";
			this.Text = "Video console";
			this._mainStatusStrip.ResumeLayout(false);
			this._mainStatusStrip.PerformLayout();
			this._langPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._splitContainerA)).EndInit();
			this._splitContainerA.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.StatusStrip _mainStatusStrip;
        public System.Windows.Forms.ToolStripStatusLabel _lblStatus1;
        public System.Windows.Forms.ToolStripStatusLabel _lblStatus2;
		public System.Windows.Forms.ToolStripStatusLabel _lblStatus3;
		private System.Windows.Forms.Panel _langPanel;
		private controls.TitleBar titleBar1;
		private System.Windows.Forms.SplitContainer _splitContainerA;
		private System.Windows.Forms.Button button1;
	}
}

