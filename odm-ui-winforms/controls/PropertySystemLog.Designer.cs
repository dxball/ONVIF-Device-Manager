namespace odm.controls {
	partial class PropertySystemLog {
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
			this._title = new odm.controls.GroupBoxControl();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this._gridMessages = new System.Windows.Forms.DataGridView();
			this._tbctrlMessageDetail = new System.Windows.Forms.TabControl();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._gridMessages)).BeginInit();
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
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 32);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this._gridMessages);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this._tbctrlMessageDetail);
			this.splitContainer1.Size = new System.Drawing.Size(424, 388);
			this.splitContainer1.SplitterDistance = 141;
			this.splitContainer1.TabIndex = 2;
			// 
			// _gridMessages
			// 
			this._gridMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._gridMessages.Dock = System.Windows.Forms.DockStyle.Fill;
			this._gridMessages.Location = new System.Drawing.Point(0, 0);
			this._gridMessages.Name = "_gridMessages";
			this._gridMessages.Size = new System.Drawing.Size(424, 141);
			this._gridMessages.TabIndex = 0;
			// 
			// _tbctrlMessageDetail
			// 
			this._tbctrlMessageDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tbctrlMessageDetail.Location = new System.Drawing.Point(0, 0);
			this._tbctrlMessageDetail.Name = "_tbctrlMessageDetail";
			this._tbctrlMessageDetail.SelectedIndex = 0;
			this._tbctrlMessageDetail.Size = new System.Drawing.Size(424, 243);
			this._tbctrlMessageDetail.TabIndex = 0;
			// 
			// PropertySystemLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this._title);
			this.Name = "PropertySystemLog";
			this.Size = new System.Drawing.Size(430, 423);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._gridMessages)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private GroupBoxControl _title;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView _gridMessages;
		private System.Windows.Forms.TabControl _tbctrlMessageDetail;
	}
}
