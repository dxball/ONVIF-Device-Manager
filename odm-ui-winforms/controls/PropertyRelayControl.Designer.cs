namespace nvc.Controls
{
    partial class PropertyRelayControl
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
            this._title = new nvc.Controls.GroupBoxControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this._lblSubnetMask = new System.Windows.Forms.TextBox();
            this._lblIPaddress = new System.Windows.Forms.TextBox();
            this._tbSubnetMask = new System.Windows.Forms.TextBox();
            this._tbIPaddress = new System.Windows.Forms.TextBox();
            this._lboxRelayList = new System.Windows.Forms.ListBox();
            this._saveCancelControl = new nvc.Controls.SaveCancelControl();
            this.panel1.SuspendLayout();
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
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this._lblSubnetMask);
            this.panel1.Controls.Add(this._lblIPaddress);
            this.panel1.Controls.Add(this._tbSubnetMask);
            this.panel1.Controls.Add(this._tbIPaddress);
            this.panel1.Controls.Add(this._lboxRelayList);
            this.panel1.Controls.Add(this._saveCancelControl);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 161);
            this.panel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(129, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // _lblSubnetMask
            // 
            this._lblSubnetMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._lblSubnetMask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblSubnetMask.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._lblSubnetMask.Location = new System.Drawing.Point(129, 32);
            this._lblSubnetMask.Name = "_lblSubnetMask";
            this._lblSubnetMask.ReadOnly = true;
            this._lblSubnetMask.Size = new System.Drawing.Size(147, 20);
            this._lblSubnetMask.TabIndex = 21;
            // 
            // _lblIPaddress
            // 
            this._lblIPaddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._lblIPaddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblIPaddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._lblIPaddress.Location = new System.Drawing.Point(129, 6);
            this._lblIPaddress.Name = "_lblIPaddress";
            this._lblIPaddress.ReadOnly = true;
            this._lblIPaddress.Size = new System.Drawing.Size(147, 20);
            this._lblIPaddress.TabIndex = 20;
            // 
            // _tbSubnetMask
            // 
            this._tbSubnetMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._tbSubnetMask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tbSubnetMask.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._tbSubnetMask.Location = new System.Drawing.Point(282, 32);
            this._tbSubnetMask.Name = "_tbSubnetMask";
            this._tbSubnetMask.ReadOnly = true;
            this._tbSubnetMask.Size = new System.Drawing.Size(138, 20);
            this._tbSubnetMask.TabIndex = 19;
            // 
            // _tbIPaddress
            // 
            this._tbIPaddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._tbIPaddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tbIPaddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._tbIPaddress.Location = new System.Drawing.Point(282, 6);
            this._tbIPaddress.Name = "_tbIPaddress";
            this._tbIPaddress.ReadOnly = true;
            this._tbIPaddress.Size = new System.Drawing.Size(138, 20);
            this._tbIPaddress.TabIndex = 18;
            // 
            // _lboxRelayList
            // 
            this._lboxRelayList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lboxRelayList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lboxRelayList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._lboxRelayList.FormattingEnabled = true;
            this._lboxRelayList.ItemHeight = 16;
            this._lboxRelayList.Items.AddRange(new object[] {
            "relay 1",
            "relay 2",
            "relay 3",
            "relay 4"});
            this._lboxRelayList.Location = new System.Drawing.Point(3, 6);
            this._lboxRelayList.Name = "_lboxRelayList";
            this._lboxRelayList.Size = new System.Drawing.Size(120, 82);
            this._lboxRelayList.TabIndex = 1;
            this._lboxRelayList.SelectedIndexChanged += new System.EventHandler(this._lboxRelayList_SelectedIndexChanged);
            // 
            // _saveCancelControl
            // 
            this._saveCancelControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._saveCancelControl.Location = new System.Drawing.Point(3, 103);
            this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
            this._saveCancelControl.Name = "_saveCancelControl";
            this._saveCancelControl.Size = new System.Drawing.Size(157, 23);
            this._saveCancelControl.TabIndex = 0;
            // 
            // PropertyRelayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._title);
            this.Name = "PropertyRelayControl";
            this.Size = new System.Drawing.Size(426, 190);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBoxControl _title;
        private System.Windows.Forms.Panel panel1;
        private SaveCancelControl _saveCancelControl;
        private System.Windows.Forms.ListBox _lboxRelayList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox _lblSubnetMask;
        private System.Windows.Forms.TextBox _lblIPaddress;
        private System.Windows.Forms.TextBox _tbSubnetMask;
        private System.Windows.Forms.TextBox _tbIPaddress;
    }
}
