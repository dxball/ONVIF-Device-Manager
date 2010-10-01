namespace nvc.controls
{
    partial class PropertyVideoStreaming
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
            this._cmbPriority = new System.Windows.Forms.ComboBox();
            this._numtbBitrate = new System.Windows.Forms.NumericUpDown();
            this._cmbEncoder = new System.Windows.Forms.ComboBox();
            this._numtbFPS = new System.Windows.Forms.NumericUpDown();
            this._cmbResolution = new System.Windows.Forms.ComboBox();
			this._saveCancelControl = new nvc.controls.SaveCancelControl();
            this._lblBitrate = new System.Windows.Forms.TextBox();
            this._lblPriority = new System.Windows.Forms.TextBox();
            this._lblResolution = new System.Windows.Forms.TextBox();
            this._lblEncoder = new System.Windows.Forms.TextBox();
            this._lblFrameRate = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
			this._title = new nvc.controls.GroupBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this._numtbBitrate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numtbFPS)).BeginInit();
            this.SuspendLayout();
            // 
            // _cmbPriority
            // 
            this._cmbPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbPriority.FormattingEnabled = true;
            this._cmbPriority.Location = new System.Drawing.Point(328, 372);
            this._cmbPriority.Name = "_cmbPriority";
            this._cmbPriority.Size = new System.Drawing.Size(95, 21);
            this._cmbPriority.TabIndex = 12;
            // 
            // _numtbBitrate
            // 
            this._numtbBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._numtbBitrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._numtbBitrate.Location = new System.Drawing.Point(328, 346);
            this._numtbBitrate.Name = "_numtbBitrate";
            this._numtbBitrate.Size = new System.Drawing.Size(95, 20);
            this._numtbBitrate.TabIndex = 11;
            // 
            // _cmbEncoder
            // 
            this._cmbEncoder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbEncoder.FormattingEnabled = true;
            this._cmbEncoder.Location = new System.Drawing.Point(328, 320);
            this._cmbEncoder.Name = "_cmbEncoder";
            this._cmbEncoder.Size = new System.Drawing.Size(95, 21);
            this._cmbEncoder.TabIndex = 10;
            // 
            // _numtbFPS
            // 
            this._numtbFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._numtbFPS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._numtbFPS.Location = new System.Drawing.Point(328, 294);
            this._numtbFPS.Name = "_numtbFPS";
            this._numtbFPS.Size = new System.Drawing.Size(95, 20);
            this._numtbFPS.TabIndex = 9;
            // 
            // _cmbResolution
            // 
            this._cmbResolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbResolution.FormattingEnabled = true;
            this._cmbResolution.Location = new System.Drawing.Point(328, 268);
            this._cmbResolution.Name = "_cmbResolution";
            this._cmbResolution.Size = new System.Drawing.Size(95, 21);
            this._cmbResolution.TabIndex = 8;
            // 
            // _saveCancelControl
            // 
            this._saveCancelControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._saveCancelControl.Location = new System.Drawing.Point(3, 406);
            this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
            this._saveCancelControl.Name = "_saveCancelControl";
            this._saveCancelControl.Size = new System.Drawing.Size(157, 23);
            this._saveCancelControl.TabIndex = 7;
            // 
            // _lblBitrate
            // 
            this._lblBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblBitrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblBitrate.Location = new System.Drawing.Point(3, 347);
            this._lblBitrate.Name = "_lblBitrate";
            this._lblBitrate.ReadOnly = true;
            this._lblBitrate.Size = new System.Drawing.Size(319, 20);
            this._lblBitrate.TabIndex = 6;
            this._lblBitrate.Text = "Target bitrate, kbps";
            // 
            // _lblPriority
            // 
            this._lblPriority.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblPriority.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblPriority.Location = new System.Drawing.Point(3, 373);
            this._lblPriority.Name = "_lblPriority";
            this._lblPriority.ReadOnly = true;
            this._lblPriority.Size = new System.Drawing.Size(319, 20);
            this._lblPriority.TabIndex = 5;
            this._lblPriority.Text = "Priority";
            // 
            // _lblResolution
            // 
            this._lblResolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblResolution.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblResolution.Location = new System.Drawing.Point(3, 269);
            this._lblResolution.Name = "_lblResolution";
            this._lblResolution.ReadOnly = true;
            this._lblResolution.Size = new System.Drawing.Size(319, 20);
            this._lblResolution.TabIndex = 4;
            this._lblResolution.Text = "Resolution, pixels";
            // 
            // _lblEncoder
            // 
            this._lblEncoder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblEncoder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblEncoder.Location = new System.Drawing.Point(3, 321);
            this._lblEncoder.Name = "_lblEncoder";
            this._lblEncoder.ReadOnly = true;
            this._lblEncoder.Size = new System.Drawing.Size(319, 20);
            this._lblEncoder.TabIndex = 3;
            this._lblEncoder.Text = "Encoder";
            // 
            // _lblFrameRate
            // 
            this._lblFrameRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblFrameRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblFrameRate.Location = new System.Drawing.Point(3, 295);
            this._lblFrameRate.Name = "_lblFrameRate";
            this._lblFrameRate.ReadOnly = true;
            this._lblFrameRate.Size = new System.Drawing.Size(319, 20);
            this._lblFrameRate.TabIndex = 2;
            this._lblFrameRate.Text = "Frame rate, fps";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(3, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(420, 237);
            this.panel1.TabIndex = 1;
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
            // PropertyVideoStreaming
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._cmbPriority);
            this.Controls.Add(this._numtbBitrate);
            this.Controls.Add(this._cmbEncoder);
            this.Controls.Add(this._numtbFPS);
            this.Controls.Add(this._cmbResolution);
            this.Controls.Add(this._saveCancelControl);
            this.Controls.Add(this._lblBitrate);
            this.Controls.Add(this._lblPriority);
            this.Controls.Add(this._lblResolution);
            this.Controls.Add(this._lblEncoder);
            this.Controls.Add(this._lblFrameRate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._title);
            this.Name = "PropertyVideoStreaming";
            this.Size = new System.Drawing.Size(426, 437);
            ((System.ComponentModel.ISupportInitialize)(this._numtbBitrate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numtbFPS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBoxControl _title;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox _lblFrameRate;
        private System.Windows.Forms.TextBox _lblEncoder;
        private System.Windows.Forms.TextBox _lblResolution;
        private System.Windows.Forms.TextBox _lblPriority;
        private System.Windows.Forms.TextBox _lblBitrate;
        private SaveCancelControl _saveCancelControl;
        private System.Windows.Forms.ComboBox _cmbResolution;
        private System.Windows.Forms.NumericUpDown _numtbFPS;
        private System.Windows.Forms.ComboBox _cmbEncoder;
        private System.Windows.Forms.NumericUpDown _numtbBitrate;
        private System.Windows.Forms.ComboBox _cmbPriority;
    }
}
