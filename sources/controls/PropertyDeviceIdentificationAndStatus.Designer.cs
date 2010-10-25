using nvc.controllers;
namespace nvc.controls
{
    partial class PropertyDeviceIdentificationAndStatus
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
			this._saveCancelControl = new nvc.controls.SaveCancelControl();
			this._tbMac = new System.Windows.Forms.TextBox();
			this._tbHardware = new System.Windows.Forms.TextBox();
			this._tbFirmware = new System.Windows.Forms.TextBox();
			this._tbDeviceId = new System.Windows.Forms.TextBox();
			this._tbName = new System.Windows.Forms.TextBox();
			this._title = new nvc.controls.GroupBoxControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this._tbIP = new nvc.controls.IPAddrMaskedTextBox();
			this._lblMac = new System.Windows.Forms.TextBox();
			this._lblIP = new System.Windows.Forms.TextBox();
			this._lblHardware = new System.Windows.Forms.TextBox();
			this._lblFirmware = new System.Windows.Forms.TextBox();
			this._lblDeviceId = new System.Windows.Forms.TextBox();
			this._lblName = new System.Windows.Forms.TextBox();
			this.textBox1 = new NumericTextBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(3, 168);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(157, 23);
			this._saveCancelControl.TabIndex = 14;
			// 
			// _tbMac
			// 
			this._tbMac.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbMac.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbMac.Location = new System.Drawing.Point(166, 133);
			this._tbMac.Name = "_tbMac";
			this._tbMac.ReadOnly = true;
			this._tbMac.Size = new System.Drawing.Size(249, 20);
			this._tbMac.TabIndex = 13;
			// 
			// _tbHardware
			// 
			this._tbHardware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbHardware.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbHardware.Location = new System.Drawing.Point(166, 29);
			this._tbHardware.Name = "_tbHardware";
			this._tbHardware.ReadOnly = true;
			this._tbHardware.Size = new System.Drawing.Size(249, 20);
			this._tbHardware.TabIndex = 9;
			// 
			// _tbFirmware
			// 
			this._tbFirmware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbFirmware.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbFirmware.Location = new System.Drawing.Point(166, 55);
			this._tbFirmware.Name = "_tbFirmware";
			this._tbFirmware.ReadOnly = true;
			this._tbFirmware.Size = new System.Drawing.Size(249, 20);
			this._tbFirmware.TabIndex = 7;
			// 
			// _tbDeviceId
			// 
			this._tbDeviceId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbDeviceId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbDeviceId.Location = new System.Drawing.Point(166, 81);
			this._tbDeviceId.Name = "_tbDeviceId";
			this._tbDeviceId.ReadOnly = true;
			this._tbDeviceId.Size = new System.Drawing.Size(249, 20);
			this._tbDeviceId.TabIndex = 5;
			// 
			// _tbName
			// 
			this._tbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbName.Location = new System.Drawing.Point(166, 3);
			this._tbName.Name = "_tbName";
			this._tbName.Size = new System.Drawing.Size(249, 20);
			this._tbName.TabIndex = 1;
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(418, 23);
			this._title.TabIndex = 15;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Controls.Add(this._tbIP);
			this.panel1.Controls.Add(this._lblMac);
			this.panel1.Controls.Add(this._lblIP);
			this.panel1.Controls.Add(this._lblHardware);
			this.panel1.Controls.Add(this._lblFirmware);
			this.panel1.Controls.Add(this._lblDeviceId);
			this.panel1.Controls.Add(this._lblName);
			this.panel1.Controls.Add(this._saveCancelControl);
			this.panel1.Controls.Add(this._tbFirmware);
			this.panel1.Controls.Add(this._tbMac);
			this.panel1.Controls.Add(this._tbDeviceId);
			this.panel1.Controls.Add(this._tbName);
			this.panel1.Controls.Add(this._tbHardware);
			this.panel1.Location = new System.Drawing.Point(3, 26);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(418, 198);
			this.panel1.TabIndex = 16;
			// 
			// _tbIP
			// 
			this._tbIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbIP.DateOnly = false;
			this._tbIP.DecimalOnly = false;
			this._tbIP.DigitOnly = false;
			this._tbIP.IPAddress = null;
			this._tbIP.IPAddrOnly = true;
			this._tbIP.Location = new System.Drawing.Point(166, 107);
			this._tbIP.Name = "_tbIP";
			this._tbIP.PhoneWithAreaCode = false;
			this._tbIP.ReadOnly = true;
			this._tbIP.Size = new System.Drawing.Size(249, 20);
			this._tbIP.SSNOnly = false;
			this._tbIP.TabIndex = 26;
			// 
			// _lblMac
			// 
			this._lblMac.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblMac.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblMac.Location = new System.Drawing.Point(3, 133);
			this._lblMac.Name = "_lblMac";
			this._lblMac.ReadOnly = true;
			this._lblMac.Size = new System.Drawing.Size(157, 20);
			this._lblMac.TabIndex = 21;
			this._lblMac.Text = "MAC address";
			// 
			// _lblIP
			// 
			this._lblIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblIP.Location = new System.Drawing.Point(3, 107);
			this._lblIP.Name = "_lblIP";
			this._lblIP.ReadOnly = true;
			this._lblIP.Size = new System.Drawing.Size(157, 20);
			this._lblIP.TabIndex = 20;
			this._lblIP.Text = "IP address";
			// 
			// _lblHardware
			// 
			this._lblHardware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblHardware.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblHardware.Location = new System.Drawing.Point(3, 29);
			this._lblHardware.Name = "_lblHardware";
			this._lblHardware.ReadOnly = true;
			this._lblHardware.Size = new System.Drawing.Size(157, 20);
			this._lblHardware.TabIndex = 19;
			this._lblHardware.Text = "Hardware";
			// 
			// _lblFirmware
			// 
			this._lblFirmware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblFirmware.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblFirmware.Location = new System.Drawing.Point(3, 55);
			this._lblFirmware.Name = "_lblFirmware";
			this._lblFirmware.ReadOnly = true;
			this._lblFirmware.Size = new System.Drawing.Size(157, 20);
			this._lblFirmware.TabIndex = 18;
			this._lblFirmware.Text = "Firmware";
			// 
			// _lblDeviceId
			// 
			this._lblDeviceId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblDeviceId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblDeviceId.Location = new System.Drawing.Point(3, 81);
			this._lblDeviceId.Name = "_lblDeviceId";
			this._lblDeviceId.ReadOnly = true;
			this._lblDeviceId.Size = new System.Drawing.Size(157, 20);
			this._lblDeviceId.TabIndex = 17;
			this._lblDeviceId.Text = "Device ID";
			// 
			// _lblName
			// 
			this._lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblName.Location = new System.Drawing.Point(3, 3);
			this._lblName.Name = "_lblName";
			this._lblName.ReadOnly = true;
			this._lblName.Size = new System.Drawing.Size(157, 20);
			this._lblName.TabIndex = 15;
			this._lblName.Text = "Name";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(272, 163);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 27;
			// 
			// PropertyDeviceIdentificationAndStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._title);
			this.Name = "PropertyDeviceIdentificationAndStatus";
			this.Size = new System.Drawing.Size(424, 227);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private SaveCancelControl _saveCancelControl;
		private System.Windows.Forms.TextBox _tbMac;
        private System.Windows.Forms.TextBox _tbHardware;
        private System.Windows.Forms.TextBox _tbFirmware;
        private System.Windows.Forms.TextBox _tbDeviceId;
        private System.Windows.Forms.TextBox _tbName;
        private GroupBoxControl _title;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox _lblMac;
        private System.Windows.Forms.TextBox _lblIP;
        private System.Windows.Forms.TextBox _lblHardware;
        private System.Windows.Forms.TextBox _lblFirmware;
        private System.Windows.Forms.TextBox _lblDeviceId;
		private System.Windows.Forms.TextBox _lblName;
		private IPAddrMaskedTextBox _tbIP;
		private System.Windows.Forms.TextBox textBox1;
    }
}
