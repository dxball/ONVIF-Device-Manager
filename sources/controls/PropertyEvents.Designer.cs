namespace nvc.controls
{
    partial class PropertyEvents
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
            this._gridPanel = new System.Windows.Forms.Panel();
            this._lviewEvents = new System.Windows.Forms.ListView();
            this._imgBox = new System.Windows.Forms.PictureBox();
			this._title = new nvc.controls.GroupBoxControl();
            this.panel1.SuspendLayout();
            this._gridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._imgBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this._gridPanel);
            this.panel1.Controls.Add(this._imgBox);
            this.panel1.Location = new System.Drawing.Point(3, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(420, 392);
            this.panel1.TabIndex = 1;
            // 
            // _gridPanel
            // 
            this._gridPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gridPanel.Controls.Add(this._lviewEvents);
            this._gridPanel.Location = new System.Drawing.Point(3, 183);
            this._gridPanel.Name = "_gridPanel";
            this._gridPanel.Size = new System.Drawing.Size(414, 206);
            this._gridPanel.TabIndex = 1;
            // 
            // _lviewEvents
            // 
            this._lviewEvents.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this._lviewEvents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lviewEvents.Cursor = System.Windows.Forms.Cursors.Hand;
            this._lviewEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lviewEvents.FullRowSelect = true;
            this._lviewEvents.GridLines = true;
            this._lviewEvents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lviewEvents.Location = new System.Drawing.Point(0, 0);
            this._lviewEvents.MultiSelect = false;
            this._lviewEvents.Name = "_lviewEvents";
            this._lviewEvents.ShowGroups = false;
            this._lviewEvents.Size = new System.Drawing.Size(414, 206);
            this._lviewEvents.TabIndex = 0;
            this._lviewEvents.UseCompatibleStateImageBehavior = false;
            // 
            // _imgBox
            // 
            this._imgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._imgBox.Location = new System.Drawing.Point(3, 3);
            this._imgBox.Name = "_imgBox";
            this._imgBox.Size = new System.Drawing.Size(414, 174);
            this._imgBox.TabIndex = 0;
            this._imgBox.TabStop = false;
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
            // PropertyEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._title);
            this.Name = "PropertyEvents";
            this.Size = new System.Drawing.Size(426, 421);
            this.panel1.ResumeLayout(false);
            this._gridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._imgBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBoxControl _title;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox _imgBox;
        private System.Windows.Forms.Panel _gridPanel;
        private System.Windows.Forms.ListView _lviewEvents;
    }
}
