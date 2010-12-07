using nvc.controllers;
namespace nvc.controls
{
    partial class PropertyNetworkSettings
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
			this._tbmDNS = new nvc.controls.IPAddressControl();
			this._tbmGate = new nvc.controls.IPAddressControl();
			this._tbmSubnet = new nvc.controls.IPAddressControl();
			this._tbmIPaddr = new nvc.controls.IPAddressControl();
			this._cbDHCP = new System.Windows.Forms.ComboBox();
			this._lblMACaddr = new System.Windows.Forms.TextBox();
			this._lblDNSaddr = new System.Windows.Forms.TextBox();
			this._lblGateAddr = new System.Windows.Forms.TextBox();
			this._lblSubnetMask = new System.Windows.Forms.TextBox();
			this._lblIPaddress = new System.Windows.Forms.TextBox();
			this._lblDHCP = new System.Windows.Forms.TextBox();
			this._saveCancelControl = new nvc.controls.SaveCancelControl();
			this._tbMACaddr = new System.Windows.Forms.TextBox();
			this._title = new nvc.controls.GroupBoxControl();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this._tbmDNS);
			this.panel1.Controls.Add(this._tbmGate);
			this.panel1.Controls.Add(this._tbmSubnet);
			this.panel1.Controls.Add(this._tbmIPaddr);
			this.panel1.Controls.Add(this._cbDHCP);
			this.panel1.Controls.Add(this._lblMACaddr);
			this.panel1.Controls.Add(this._lblDNSaddr);
			this.panel1.Controls.Add(this._lblGateAddr);
			this.panel1.Controls.Add(this._lblSubnetMask);
			this.panel1.Controls.Add(this._lblIPaddress);
			this.panel1.Controls.Add(this._lblDHCP);
			this.panel1.Controls.Add(this._saveCancelControl);
			this.panel1.Controls.Add(this._tbMACaddr);
			this.panel1.Location = new System.Drawing.Point(3, 26);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(418, 194);
			this.panel1.TabIndex = 17;
			// 
			// _tbmDNS
			// 
			this._tbmDNS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbmDNS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbmDNS.IPAddress = null;
			this._tbmDNS.Location = new System.Drawing.Point(166, 107);
			this._tbmDNS.Name = "_tbmDNS";
			this._tbmDNS.Size = new System.Drawing.Size(232, 20);
			this._tbmDNS.TabIndex = 5;
			// 
			// _tbmGate
			// 
			this._tbmGate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbmGate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbmGate.IPAddress = null;
			this._tbmGate.Location = new System.Drawing.Point(166, 81);
			this._tbmGate.Name = "_tbmGate";
			this._tbmGate.Size = new System.Drawing.Size(232, 20);
			this._tbmGate.TabIndex = 4;
			// 
			// _tbmSubnet
			// 
			this._tbmSubnet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbmSubnet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbmSubnet.IPAddress = null;
			this._tbmSubnet.Location = new System.Drawing.Point(166, 54);
			this._tbmSubnet.Name = "_tbmSubnet";
			this._tbmSubnet.Size = new System.Drawing.Size(232, 20);
			this._tbmSubnet.TabIndex = 3;
			// 
			// _tbmIPaddr
			// 
			this._tbmIPaddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbmIPaddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbmIPaddr.IPAddress = null;
			this._tbmIPaddr.Location = new System.Drawing.Point(166, 28);
			this._tbmIPaddr.Name = "_tbmIPaddr";
			this._tbmIPaddr.Size = new System.Drawing.Size(232, 20);
			this._tbmIPaddr.TabIndex = 2;
			// 
			// _cbDHCP
			// 
			this._cbDHCP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._cbDHCP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cbDHCP.FormattingEnabled = true;
			this._cbDHCP.Location = new System.Drawing.Point(166, 3);
			this._cbDHCP.Name = "_cbDHCP";
			this._cbDHCP.Size = new System.Drawing.Size(249, 21);
			this._cbDHCP.TabIndex = 1;
			// 
			// _lblMACaddr
			// 
			this._lblMACaddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblMACaddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblMACaddr.Location = new System.Drawing.Point(3, 133);
			this._lblMACaddr.Name = "_lblMACaddr";
			this._lblMACaddr.ReadOnly = true;
			this._lblMACaddr.Size = new System.Drawing.Size(157, 20);
			this._lblMACaddr.TabIndex = 20;
			this._lblMACaddr.TabStop = false;
			this._lblMACaddr.Text = "MAC";
			// 
			// _lblDNSaddr
			// 
			this._lblDNSaddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblDNSaddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblDNSaddr.Location = new System.Drawing.Point(3, 107);
			this._lblDNSaddr.Name = "_lblDNSaddr";
			this._lblDNSaddr.ReadOnly = true;
			this._lblDNSaddr.Size = new System.Drawing.Size(157, 20);
			this._lblDNSaddr.TabIndex = 19;
			this._lblDNSaddr.TabStop = false;
			this._lblDNSaddr.Text = "DNS";
			// 
			// _lblGateAddr
			// 
			this._lblGateAddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblGateAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblGateAddr.Location = new System.Drawing.Point(3, 81);
			this._lblGateAddr.Name = "_lblGateAddr";
			this._lblGateAddr.ReadOnly = true;
			this._lblGateAddr.Size = new System.Drawing.Size(157, 20);
			this._lblGateAddr.TabIndex = 18;
			this._lblGateAddr.TabStop = false;
			this._lblGateAddr.Text = "Gate";
			// 
			// _lblSubnetMask
			// 
			this._lblSubnetMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblSubnetMask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblSubnetMask.Location = new System.Drawing.Point(3, 55);
			this._lblSubnetMask.Name = "_lblSubnetMask";
			this._lblSubnetMask.ReadOnly = true;
			this._lblSubnetMask.Size = new System.Drawing.Size(157, 20);
			this._lblSubnetMask.TabIndex = 17;
			this._lblSubnetMask.TabStop = false;
			this._lblSubnetMask.Text = "Subnet";
			// 
			// _lblIPaddress
			// 
			this._lblIPaddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblIPaddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblIPaddress.Location = new System.Drawing.Point(3, 29);
			this._lblIPaddress.Name = "_lblIPaddress";
			this._lblIPaddress.ReadOnly = true;
			this._lblIPaddress.Size = new System.Drawing.Size(157, 20);
			this._lblIPaddress.TabIndex = 16;
			this._lblIPaddress.TabStop = false;
			this._lblIPaddress.Text = "IP";
			// 
			// _lblDHCP
			// 
			this._lblDHCP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblDHCP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblDHCP.Location = new System.Drawing.Point(3, 3);
			this._lblDHCP.Name = "_lblDHCP";
			this._lblDHCP.ReadOnly = true;
			this._lblDHCP.Size = new System.Drawing.Size(157, 20);
			this._lblDHCP.TabIndex = 15;
			this._lblDHCP.TabStop = false;
			this._lblDHCP.Text = "DHCP";
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(3, 167);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(206, 23);
			this._saveCancelControl.TabIndex = 14;
			// 
			// _tbMACaddr
			// 
			this._tbMACaddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tbMACaddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tbMACaddr.Location = new System.Drawing.Point(166, 133);
			this._tbMACaddr.Name = "_tbMACaddr";
			this._tbMACaddr.ReadOnly = true;
			this._tbMACaddr.Size = new System.Drawing.Size(249, 20);
			this._tbMACaddr.TabIndex = 11;
			this._tbMACaddr.TabStop = false;
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(418, 23);
			this._title.TabIndex = 18;
			this._title.TabStop = false;
			// 
			// PropertyNetworkSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._title);
			this.Controls.Add(this.panel1);
			this.Name = "PropertyNetworkSettings";
			this.Size = new System.Drawing.Size(422, 224);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox _lblMACaddr;
        private System.Windows.Forms.TextBox _lblDNSaddr;
        private System.Windows.Forms.TextBox _lblGateAddr;
        private System.Windows.Forms.TextBox _lblSubnetMask;
        private System.Windows.Forms.TextBox _lblIPaddress;
        private System.Windows.Forms.TextBox _lblDHCP;
        private SaveCancelControl _saveCancelControl;
        private System.Windows.Forms.TextBox _tbMACaddr;
        private GroupBoxControl _title;
		private System.Windows.Forms.ComboBox _cbDHCP;
		//private IPAddrMaskedTextBox _tbmIPaddr;
		//private IPTextBox _tbmIPaddr;
		private IPAddressControl _tbmIPaddr;
		private IPAddressControl _tbmDNS;
		private IPAddressControl _tbmGate;
		private IPAddressControl _tbmSubnet;
    }
}
