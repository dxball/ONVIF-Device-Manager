namespace nvc.controls
{
    partial class PropertyDigitalIO
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
            this._grpDigitalOutputs = new System.Windows.Forms.GroupBox();
            this._lampRelay = new System.Windows.Forms.RadioButton();
            this._btnTriggerRelay = new System.Windows.Forms.Button();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this._lbDigitalOutputs = new System.Windows.Forms.ListBox();
            this._lblOutputCurrentStatus = new System.Windows.Forms.TextBox();
            this._lblOutputName = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this._lblOutputIdleStatus = new System.Windows.Forms.TextBox();
            this._grpDigitalInputs = new System.Windows.Forms.GroupBox();
            this._cbRecordChannel = new System.Windows.Forms.ComboBox();
            this._cbInputCurrentStatus = new System.Windows.Forms.ComboBox();
            this._cbTriggerRelay = new System.Windows.Forms.ComboBox();
            this._cbInputNormalStatus = new System.Windows.Forms.ComboBox();
            this._checkSwitchVideo = new System.Windows.Forms.CheckBox();
            this._lblInputCurrentStatus = new System.Windows.Forms.TextBox();
            this._checkRecordChannel = new System.Windows.Forms.CheckBox();
            this._tbInputName = new System.Windows.Forms.TextBox();
            this._checkTriggerRelay = new System.Windows.Forms.CheckBox();
            this._lblInputNormalStatus = new System.Windows.Forms.TextBox();
            this._checkSendMessage = new System.Windows.Forms.CheckBox();
            this._lblInputName = new System.Windows.Forms.TextBox();
            this._lbDigitalInputs = new System.Windows.Forms.ListBox();
			this._saveCancelControl = new nvc.controls.SaveCancelControl();
			this._title = new nvc.controls.GroupBoxControl();
            this.panel1.SuspendLayout();
            this._grpDigitalOutputs.SuspendLayout();
            this._grpDigitalInputs.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this._grpDigitalOutputs);
            this.panel1.Controls.Add(this._grpDigitalInputs);
            this.panel1.Controls.Add(this._saveCancelControl);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 359);
            this.panel1.TabIndex = 1;
            // 
            // _grpDigitalOutputs
            // 
            this._grpDigitalOutputs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpDigitalOutputs.Controls.Add(this._lampRelay);
            this._grpDigitalOutputs.Controls.Add(this._btnTriggerRelay);
            this._grpDigitalOutputs.Controls.Add(this.comboBox5);
            this._grpDigitalOutputs.Controls.Add(this._lbDigitalOutputs);
            this._grpDigitalOutputs.Controls.Add(this._lblOutputCurrentStatus);
            this._grpDigitalOutputs.Controls.Add(this._lblOutputName);
            this._grpDigitalOutputs.Controls.Add(this.textBox6);
            this._grpDigitalOutputs.Controls.Add(this._lblOutputIdleStatus);
            this._grpDigitalOutputs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._grpDigitalOutputs.Location = new System.Drawing.Point(3, 214);
            this._grpDigitalOutputs.Name = "_grpDigitalOutputs";
            this._grpDigitalOutputs.Size = new System.Drawing.Size(417, 100);
            this._grpDigitalOutputs.TabIndex = 3;
            this._grpDigitalOutputs.TabStop = false;
            this._grpDigitalOutputs.Text = "groupBox3";
            // 
            // _lampRelay
            // 
            this._lampRelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._lampRelay.AutoSize = true;
            this._lampRelay.Checked = true;
            this._lampRelay.Location = new System.Drawing.Point(214, 71);
            this._lampRelay.Name = "_lampRelay";
            this._lampRelay.Size = new System.Drawing.Size(14, 13);
            this._lampRelay.TabIndex = 15;
            this._lampRelay.TabStop = true;
            this._lampRelay.UseVisualStyleBackColor = true;
            // 
            // _btnTriggerRelay
            // 
            this._btnTriggerRelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnTriggerRelay.Location = new System.Drawing.Point(238, 69);
            this._btnTriggerRelay.Name = "_btnTriggerRelay";
            this._btnTriggerRelay.Size = new System.Drawing.Size(86, 23);
            this._btnTriggerRelay.TabIndex = 13;
            this._btnTriggerRelay.Text = "Trigger relay";
            this._btnTriggerRelay.UseVisualStyleBackColor = true;
            // 
            // comboBox5
            // 
            this.comboBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(238, 45);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(173, 21);
            this.comboBox5.TabIndex = 11;
            // 
            // _lbDigitalOutputs
            // 
            this._lbDigitalOutputs.FormattingEnabled = true;
            this._lbDigitalOutputs.Location = new System.Drawing.Point(6, 19);
            this._lbDigitalOutputs.Name = "_lbDigitalOutputs";
            this._lbDigitalOutputs.Size = new System.Drawing.Size(102, 69);
            this._lbDigitalOutputs.TabIndex = 7;
            // 
            // _lblOutputCurrentStatus
            // 
            this._lblOutputCurrentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblOutputCurrentStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblOutputCurrentStatus.Location = new System.Drawing.Point(114, 71);
            this._lblOutputCurrentStatus.Name = "_lblOutputCurrentStatus";
            this._lblOutputCurrentStatus.ReadOnly = true;
            this._lblOutputCurrentStatus.Size = new System.Drawing.Size(94, 20);
            this._lblOutputCurrentStatus.TabIndex = 10;
            this._lblOutputCurrentStatus.Text = "Current status";
            // 
            // _lblOutputName
            // 
            this._lblOutputName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblOutputName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblOutputName.Location = new System.Drawing.Point(114, 19);
            this._lblOutputName.Name = "_lblOutputName";
            this._lblOutputName.ReadOnly = true;
            this._lblOutputName.Size = new System.Drawing.Size(118, 20);
            this._lblOutputName.TabIndex = 7;
            this._lblOutputName.Text = "Name";
            // 
            // textBox6
            // 
            this.textBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox6.Location = new System.Drawing.Point(238, 19);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(173, 20);
            this.textBox6.TabIndex = 9;
            // 
            // _lblOutputIdleStatus
            // 
            this._lblOutputIdleStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblOutputIdleStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblOutputIdleStatus.Location = new System.Drawing.Point(114, 45);
            this._lblOutputIdleStatus.Name = "_lblOutputIdleStatus";
            this._lblOutputIdleStatus.ReadOnly = true;
            this._lblOutputIdleStatus.Size = new System.Drawing.Size(118, 20);
            this._lblOutputIdleStatus.TabIndex = 8;
            this._lblOutputIdleStatus.Text = "Idle status";
            // 
            // _grpDigitalInputs
            // 
            this._grpDigitalInputs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpDigitalInputs.Controls.Add(this._cbRecordChannel);
            this._grpDigitalInputs.Controls.Add(this._cbInputCurrentStatus);
            this._grpDigitalInputs.Controls.Add(this._cbTriggerRelay);
            this._grpDigitalInputs.Controls.Add(this._cbInputNormalStatus);
            this._grpDigitalInputs.Controls.Add(this._checkSwitchVideo);
            this._grpDigitalInputs.Controls.Add(this._lblInputCurrentStatus);
            this._grpDigitalInputs.Controls.Add(this._checkRecordChannel);
            this._grpDigitalInputs.Controls.Add(this._tbInputName);
            this._grpDigitalInputs.Controls.Add(this._checkTriggerRelay);
            this._grpDigitalInputs.Controls.Add(this._lblInputNormalStatus);
            this._grpDigitalInputs.Controls.Add(this._checkSendMessage);
            this._grpDigitalInputs.Controls.Add(this._lblInputName);
            this._grpDigitalInputs.Controls.Add(this._lbDigitalInputs);
            this._grpDigitalInputs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._grpDigitalInputs.Location = new System.Drawing.Point(3, 3);
            this._grpDigitalInputs.Name = "_grpDigitalInputs";
            this._grpDigitalInputs.Size = new System.Drawing.Size(417, 195);
            this._grpDigitalInputs.TabIndex = 1;
            this._grpDigitalInputs.TabStop = false;
            this._grpDigitalInputs.Text = "groupBox1";
            // 
            // _cbRecordChannel
            // 
            this._cbRecordChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cbRecordChannel.FormattingEnabled = true;
            this._cbRecordChannel.Location = new System.Drawing.Point(238, 143);
            this._cbRecordChannel.Name = "_cbRecordChannel";
            this._cbRecordChannel.Size = new System.Drawing.Size(173, 21);
            this._cbRecordChannel.TabIndex = 5;
            // 
            // _cbInputCurrentStatus
            // 
            this._cbInputCurrentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cbInputCurrentStatus.FormattingEnabled = true;
            this._cbInputCurrentStatus.Location = new System.Drawing.Point(238, 72);
            this._cbInputCurrentStatus.Name = "_cbInputCurrentStatus";
            this._cbInputCurrentStatus.Size = new System.Drawing.Size(173, 21);
            this._cbInputCurrentStatus.TabIndex = 6;
            // 
            // _cbTriggerRelay
            // 
            this._cbTriggerRelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cbTriggerRelay.FormattingEnabled = true;
            this._cbTriggerRelay.Location = new System.Drawing.Point(238, 120);
            this._cbTriggerRelay.Name = "_cbTriggerRelay";
            this._cbTriggerRelay.Size = new System.Drawing.Size(173, 21);
            this._cbTriggerRelay.TabIndex = 4;
            // 
            // _cbInputNormalStatus
            // 
            this._cbInputNormalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cbInputNormalStatus.FormattingEnabled = true;
            this._cbInputNormalStatus.Location = new System.Drawing.Point(238, 45);
            this._cbInputNormalStatus.Name = "_cbInputNormalStatus";
            this._cbInputNormalStatus.Size = new System.Drawing.Size(173, 21);
            this._cbInputNormalStatus.TabIndex = 5;
            // 
            // _checkSwitchVideo
            // 
            this._checkSwitchVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._checkSwitchVideo.AutoSize = true;
            this._checkSwitchVideo.Location = new System.Drawing.Point(114, 168);
            this._checkSwitchVideo.Name = "_checkSwitchVideo";
            this._checkSwitchVideo.Size = new System.Drawing.Size(149, 17);
            this._checkSwitchVideo.TabIndex = 3;
            this._checkSwitchVideo.Text = "Switch analogue video on";
            this._checkSwitchVideo.UseVisualStyleBackColor = true;
            // 
            // _lblInputCurrentStatus
            // 
            this._lblInputCurrentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblInputCurrentStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblInputCurrentStatus.Location = new System.Drawing.Point(114, 71);
            this._lblInputCurrentStatus.Name = "_lblInputCurrentStatus";
            this._lblInputCurrentStatus.ReadOnly = true;
            this._lblInputCurrentStatus.Size = new System.Drawing.Size(118, 20);
            this._lblInputCurrentStatus.TabIndex = 4;
            this._lblInputCurrentStatus.Text = "Current status";
            // 
            // _checkRecordChannel
            // 
            this._checkRecordChannel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._checkRecordChannel.AutoSize = true;
            this._checkRecordChannel.Location = new System.Drawing.Point(114, 145);
            this._checkRecordChannel.Name = "_checkRecordChannel";
            this._checkRecordChannel.Size = new System.Drawing.Size(102, 17);
            this._checkRecordChannel.TabIndex = 2;
            this._checkRecordChannel.Text = "Record channel";
            this._checkRecordChannel.UseVisualStyleBackColor = true;
            // 
            // _tbInputName
            // 
            this._tbInputName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._tbInputName.Location = new System.Drawing.Point(238, 19);
            this._tbInputName.Name = "_tbInputName";
            this._tbInputName.Size = new System.Drawing.Size(173, 20);
            this._tbInputName.TabIndex = 3;
            // 
            // _checkTriggerRelay
            // 
            this._checkTriggerRelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._checkTriggerRelay.AutoSize = true;
            this._checkTriggerRelay.Location = new System.Drawing.Point(114, 122);
            this._checkTriggerRelay.Name = "_checkTriggerRelay";
            this._checkTriggerRelay.Size = new System.Drawing.Size(84, 17);
            this._checkTriggerRelay.TabIndex = 1;
            this._checkTriggerRelay.Text = "Trigger relay";
            this._checkTriggerRelay.UseVisualStyleBackColor = true;
            // 
            // _lblInputNormalStatus
            // 
            this._lblInputNormalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblInputNormalStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblInputNormalStatus.Location = new System.Drawing.Point(114, 45);
            this._lblInputNormalStatus.Name = "_lblInputNormalStatus";
            this._lblInputNormalStatus.ReadOnly = true;
            this._lblInputNormalStatus.Size = new System.Drawing.Size(118, 20);
            this._lblInputNormalStatus.TabIndex = 2;
            this._lblInputNormalStatus.Text = "Normal status";
            // 
            // _checkSendMessage
            // 
            this._checkSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._checkSendMessage.AutoSize = true;
            this._checkSendMessage.Location = new System.Drawing.Point(114, 99);
            this._checkSendMessage.Name = "_checkSendMessage";
            this._checkSendMessage.Size = new System.Drawing.Size(131, 17);
            this._checkSendMessage.TabIndex = 0;
            this._checkSendMessage.Text = "Send ONVIF message";
            this._checkSendMessage.UseVisualStyleBackColor = true;
            // 
            // _lblInputName
            // 
            this._lblInputName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lblInputName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lblInputName.Location = new System.Drawing.Point(114, 19);
            this._lblInputName.Name = "_lblInputName";
            this._lblInputName.ReadOnly = true;
            this._lblInputName.Size = new System.Drawing.Size(118, 20);
            this._lblInputName.TabIndex = 1;
            this._lblInputName.Text = "Name";
            // 
            // _lbDigitalInputs
            // 
            this._lbDigitalInputs.FormattingEnabled = true;
            this._lbDigitalInputs.Location = new System.Drawing.Point(6, 19);
            this._lbDigitalInputs.Name = "_lbDigitalInputs";
            this._lbDigitalInputs.Size = new System.Drawing.Size(102, 69);
            this._lbDigitalInputs.TabIndex = 0;
            // 
            // _saveCancelControl
            // 
            this._saveCancelControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._saveCancelControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._saveCancelControl.Location = new System.Drawing.Point(3, 323);
            this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
            this._saveCancelControl.Name = "_saveCancelControl";
            this._saveCancelControl.Size = new System.Drawing.Size(157, 23);
            this._saveCancelControl.TabIndex = 0;
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
            // PropertyDigitalIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._title);
            this.Name = "PropertyDigitalIO";
            this.Size = new System.Drawing.Size(426, 391);
            this.panel1.ResumeLayout(false);
            this._grpDigitalOutputs.ResumeLayout(false);
            this._grpDigitalOutputs.PerformLayout();
            this._grpDigitalInputs.ResumeLayout(false);
            this._grpDigitalInputs.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBoxControl _title;
        private System.Windows.Forms.Panel panel1;
        private SaveCancelControl _saveCancelControl;
        private System.Windows.Forms.ComboBox _cbRecordChannel;
        private System.Windows.Forms.ComboBox _cbTriggerRelay;
        private System.Windows.Forms.CheckBox _checkSwitchVideo;
        private System.Windows.Forms.CheckBox _checkRecordChannel;
        private System.Windows.Forms.CheckBox _checkTriggerRelay;
        private System.Windows.Forms.CheckBox _checkSendMessage;
        private System.Windows.Forms.GroupBox _grpDigitalOutputs;
        private System.Windows.Forms.Button _btnTriggerRelay;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ListBox _lbDigitalOutputs;
        private System.Windows.Forms.TextBox _lblOutputCurrentStatus;
        private System.Windows.Forms.TextBox _lblOutputName;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox _lblOutputIdleStatus;
        private System.Windows.Forms.GroupBox _grpDigitalInputs;
        private System.Windows.Forms.ComboBox _cbInputCurrentStatus;
        private System.Windows.Forms.ComboBox _cbInputNormalStatus;
        private System.Windows.Forms.TextBox _lblInputCurrentStatus;
        private System.Windows.Forms.TextBox _tbInputName;
        private System.Windows.Forms.TextBox _lblInputNormalStatus;
        private System.Windows.Forms.TextBox _lblInputName;
        private System.Windows.Forms.ListBox _lbDigitalInputs;
        private System.Windows.Forms.RadioButton _lampRelay;
    }
}
