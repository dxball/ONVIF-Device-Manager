namespace odm.controls {
	partial class PropertyImagingSettings {
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
			this.panel1 = new System.Windows.Forms.Panel();
			this._lblMaxBr = new System.Windows.Forms.Label();
			this._tbarBrightness = new odm.controls.FloatTrackBar();
			this._lblBrightness = new System.Windows.Forms.TextBox();
			this._saveCancelControl = new odm.controls.SaveCancelControl();
			this._tbarSharpness = new odm.controls.FloatTrackBar();
			this._lblSharpness = new System.Windows.Forms.TextBox();
			this._tbarContrast = new odm.controls.FloatTrackBar();
			this._lblContrast = new System.Windows.Forms.TextBox();
			this._tbarSaturation = new odm.controls.FloatTrackBar();
			this._lblSaturation = new System.Windows.Forms.TextBox();
			this._lblMaxSatur = new System.Windows.Forms.Label();
			this._lblMaxContr = new System.Windows.Forms.Label();
			this._lblMaxSharp = new System.Windows.Forms.Label();
			this._lblWhiteBalanceMode = new System.Windows.Forms.TextBox();
			this._lblMaxCr = new System.Windows.Forms.Label();
			this._lblMaxCb = new System.Windows.Forms.Label();
			this._lblWhiteBalanceCB = new System.Windows.Forms.TextBox();
			this._tbarWhiteBalanceCR = new odm.controls.FloatTrackBar();
			this._lblWhiteBalanceCR = new System.Windows.Forms.TextBox();
			this._tbarWhiteBalanceCB = new odm.controls.FloatTrackBar();
			this._chbAutoWhiteBalance = new System.Windows.Forms.CheckBox();
			this._lblMinCr = new System.Windows.Forms.Label();
			this._lblMinCb = new System.Windows.Forms.Label();
			this._lblMinSharp = new System.Windows.Forms.Label();
			this._lblMinContr = new System.Windows.Forms.Label();
			this._lblMinSatur = new System.Windows.Forms.Label();
			this._lblMinBr = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this._tbarBrightness)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarSharpness)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarContrast)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarSaturation)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarWhiteBalanceCR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarWhiteBalanceCB)).BeginInit();
			this.SuspendLayout();
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(397, 23);
			this._title.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(3, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(394, 184);
			this.panel1.TabIndex = 2;
			// 
			// _lblMaxBr
			// 
			this._lblMaxBr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblMaxBr.AutoSize = true;
			this._lblMaxBr.Location = new System.Drawing.Point(362, 224);
			this._lblMaxBr.Name = "_lblMaxBr";
			this._lblMaxBr.Size = new System.Drawing.Size(35, 13);
			this._lblMaxBr.TabIndex = 0;
			this._lblMaxBr.Text = "label1";
			// 
			// _tbarBrightness
			// 
			this._tbarBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbarBrightness.fMaximum = 10F;
			this._tbarBrightness.fMinimum = 0F;
			this._tbarBrightness.fValue = 0F;
			this._tbarBrightness.Location = new System.Drawing.Point(188, 222);
			this._tbarBrightness.Name = "_tbarBrightness";
			this._tbarBrightness.Size = new System.Drawing.Size(180, 45);
			this._tbarBrightness.TabIndex = 21;
			this._tbarBrightness.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblBrightness
			// 
			this._lblBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblBrightness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblBrightness.Location = new System.Drawing.Point(5, 222);
			this._lblBrightness.Name = "_lblBrightness";
			this._lblBrightness.ReadOnly = true;
			this._lblBrightness.Size = new System.Drawing.Size(150, 20);
			this._lblBrightness.TabIndex = 20;
			this._lblBrightness.TabStop = false;
			this._lblBrightness.Text = "Brightness";
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(3, 423);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(188, 23);
			this._saveCancelControl.TabIndex = 23;
			// 
			// _tbarSharpness
			// 
			this._tbarSharpness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbarSharpness.fMaximum = 10F;
			this._tbarSharpness.fMinimum = 0F;
			this._tbarSharpness.fValue = 0F;
			this._tbarSharpness.Location = new System.Drawing.Point(188, 300);
			this._tbarSharpness.Name = "_tbarSharpness";
			this._tbarSharpness.Size = new System.Drawing.Size(180, 45);
			this._tbarSharpness.TabIndex = 25;
			this._tbarSharpness.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblSharpness
			// 
			this._lblSharpness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblSharpness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblSharpness.Location = new System.Drawing.Point(5, 300);
			this._lblSharpness.Name = "_lblSharpness";
			this._lblSharpness.ReadOnly = true;
			this._lblSharpness.Size = new System.Drawing.Size(150, 20);
			this._lblSharpness.TabIndex = 24;
			this._lblSharpness.TabStop = false;
			this._lblSharpness.Text = "Sharpness";
			// 
			// _tbarContrast
			// 
			this._tbarContrast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbarContrast.fMaximum = 10F;
			this._tbarContrast.fMinimum = 0F;
			this._tbarContrast.fValue = 0F;
			this._tbarContrast.Location = new System.Drawing.Point(188, 275);
			this._tbarContrast.Name = "_tbarContrast";
			this._tbarContrast.Size = new System.Drawing.Size(180, 45);
			this._tbarContrast.TabIndex = 27;
			this._tbarContrast.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblContrast
			// 
			this._lblContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblContrast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblContrast.Location = new System.Drawing.Point(5, 275);
			this._lblContrast.Name = "_lblContrast";
			this._lblContrast.ReadOnly = true;
			this._lblContrast.Size = new System.Drawing.Size(150, 20);
			this._lblContrast.TabIndex = 26;
			this._lblContrast.TabStop = false;
			this._lblContrast.Text = "Contrast";
			// 
			// _tbarSaturation
			// 
			this._tbarSaturation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbarSaturation.fMaximum = 10F;
			this._tbarSaturation.fMinimum = 0F;
			this._tbarSaturation.fValue = 0F;
			this._tbarSaturation.Location = new System.Drawing.Point(188, 249);
			this._tbarSaturation.Name = "_tbarSaturation";
			this._tbarSaturation.Size = new System.Drawing.Size(180, 45);
			this._tbarSaturation.TabIndex = 29;
			this._tbarSaturation.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblSaturation
			// 
			this._lblSaturation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblSaturation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblSaturation.Location = new System.Drawing.Point(5, 249);
			this._lblSaturation.Name = "_lblSaturation";
			this._lblSaturation.ReadOnly = true;
			this._lblSaturation.Size = new System.Drawing.Size(150, 20);
			this._lblSaturation.TabIndex = 28;
			this._lblSaturation.TabStop = false;
			this._lblSaturation.Text = "Color saturation";
			// 
			// _lblMaxSatur
			// 
			this._lblMaxSatur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblMaxSatur.AutoSize = true;
			this._lblMaxSatur.Location = new System.Drawing.Point(362, 251);
			this._lblMaxSatur.Name = "_lblMaxSatur";
			this._lblMaxSatur.Size = new System.Drawing.Size(35, 13);
			this._lblMaxSatur.TabIndex = 0;
			this._lblMaxSatur.Text = "label1";
			// 
			// _lblMaxContr
			// 
			this._lblMaxContr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblMaxContr.AutoSize = true;
			this._lblMaxContr.Location = new System.Drawing.Point(362, 277);
			this._lblMaxContr.Name = "_lblMaxContr";
			this._lblMaxContr.Size = new System.Drawing.Size(35, 13);
			this._lblMaxContr.TabIndex = 0;
			this._lblMaxContr.Text = "label1";
			// 
			// _lblMaxSharp
			// 
			this._lblMaxSharp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblMaxSharp.AutoSize = true;
			this._lblMaxSharp.Location = new System.Drawing.Point(362, 302);
			this._lblMaxSharp.Name = "_lblMaxSharp";
			this._lblMaxSharp.Size = new System.Drawing.Size(35, 13);
			this._lblMaxSharp.TabIndex = 0;
			this._lblMaxSharp.Text = "label1";
			// 
			// _lblWhiteBalanceMode
			// 
			this._lblWhiteBalanceMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblWhiteBalanceMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblWhiteBalanceMode.Location = new System.Drawing.Point(5, 324);
			this._lblWhiteBalanceMode.Name = "_lblWhiteBalanceMode";
			this._lblWhiteBalanceMode.ReadOnly = true;
			this._lblWhiteBalanceMode.Size = new System.Drawing.Size(150, 20);
			this._lblWhiteBalanceMode.TabIndex = 33;
			this._lblWhiteBalanceMode.TabStop = false;
			this._lblWhiteBalanceMode.Text = "White balance mode";
			// 
			// _lblMaxCr
			// 
			this._lblMaxCr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblMaxCr.AutoSize = true;
			this._lblMaxCr.Location = new System.Drawing.Point(362, 381);
			this._lblMaxCr.Name = "_lblMaxCr";
			this._lblMaxCr.Size = new System.Drawing.Size(35, 13);
			this._lblMaxCr.TabIndex = 0;
			this._lblMaxCr.Text = "label1";
			// 
			// _lblMaxCb
			// 
			this._lblMaxCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblMaxCb.AutoSize = true;
			this._lblMaxCb.Location = new System.Drawing.Point(362, 355);
			this._lblMaxCb.Name = "_lblMaxCb";
			this._lblMaxCb.Size = new System.Drawing.Size(35, 13);
			this._lblMaxCb.TabIndex = 0;
			this._lblMaxCb.Text = "label5";
			// 
			// _lblWhiteBalanceCB
			// 
			this._lblWhiteBalanceCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblWhiteBalanceCB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblWhiteBalanceCB.Location = new System.Drawing.Point(5, 353);
			this._lblWhiteBalanceCB.Name = "_lblWhiteBalanceCB";
			this._lblWhiteBalanceCB.ReadOnly = true;
			this._lblWhiteBalanceCB.Size = new System.Drawing.Size(150, 20);
			this._lblWhiteBalanceCB.TabIndex = 37;
			this._lblWhiteBalanceCB.TabStop = false;
			this._lblWhiteBalanceCB.Text = "White balance Cb gain";
			// 
			// _tbarWhiteBalanceCR
			// 
			this._tbarWhiteBalanceCR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbarWhiteBalanceCR.fMaximum = 10F;
			this._tbarWhiteBalanceCR.fMinimum = 0F;
			this._tbarWhiteBalanceCR.fValue = 0F;
			this._tbarWhiteBalanceCR.Location = new System.Drawing.Point(188, 379);
			this._tbarWhiteBalanceCR.Name = "_tbarWhiteBalanceCR";
			this._tbarWhiteBalanceCR.Size = new System.Drawing.Size(180, 45);
			this._tbarWhiteBalanceCR.TabIndex = 36;
			this._tbarWhiteBalanceCR.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblWhiteBalanceCR
			// 
			this._lblWhiteBalanceCR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblWhiteBalanceCR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblWhiteBalanceCR.Location = new System.Drawing.Point(5, 379);
			this._lblWhiteBalanceCR.Name = "_lblWhiteBalanceCR";
			this._lblWhiteBalanceCR.ReadOnly = true;
			this._lblWhiteBalanceCR.Size = new System.Drawing.Size(150, 20);
			this._lblWhiteBalanceCR.TabIndex = 35;
			this._lblWhiteBalanceCR.TabStop = false;
			this._lblWhiteBalanceCR.Text = "White balance Cr gain";
			// 
			// _tbarWhiteBalanceCB
			// 
			this._tbarWhiteBalanceCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tbarWhiteBalanceCB.fMaximum = 10F;
			this._tbarWhiteBalanceCB.fMinimum = 0F;
			this._tbarWhiteBalanceCB.fValue = 0F;
			this._tbarWhiteBalanceCB.Location = new System.Drawing.Point(188, 353);
			this._tbarWhiteBalanceCB.Name = "_tbarWhiteBalanceCB";
			this._tbarWhiteBalanceCB.Size = new System.Drawing.Size(180, 45);
			this._tbarWhiteBalanceCB.TabIndex = 38;
			this._tbarWhiteBalanceCB.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _chbAutoWhiteBalance
			// 
			this._chbAutoWhiteBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._chbAutoWhiteBalance.AutoSize = true;
			this._chbAutoWhiteBalance.Location = new System.Drawing.Point(161, 326);
			this._chbAutoWhiteBalance.Name = "_chbAutoWhiteBalance";
			this._chbAutoWhiteBalance.Size = new System.Drawing.Size(15, 14);
			this._chbAutoWhiteBalance.TabIndex = 41;
			this._chbAutoWhiteBalance.UseVisualStyleBackColor = true;
			// 
			// _lblMinCr
			// 
			this._lblMinCr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblMinCr.AutoSize = true;
			this._lblMinCr.Location = new System.Drawing.Point(156, 381);
			this._lblMinCr.Name = "_lblMinCr";
			this._lblMinCr.Size = new System.Drawing.Size(35, 13);
			this._lblMinCr.TabIndex = 0;
			this._lblMinCr.Text = "label1";
			// 
			// _lblMinCb
			// 
			this._lblMinCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblMinCb.AutoSize = true;
			this._lblMinCb.Location = new System.Drawing.Point(156, 355);
			this._lblMinCb.Name = "_lblMinCb";
			this._lblMinCb.Size = new System.Drawing.Size(35, 13);
			this._lblMinCb.TabIndex = 0;
			this._lblMinCb.Text = "label5";
			// 
			// _lblMinSharp
			// 
			this._lblMinSharp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblMinSharp.AutoSize = true;
			this._lblMinSharp.Location = new System.Drawing.Point(156, 302);
			this._lblMinSharp.Name = "_lblMinSharp";
			this._lblMinSharp.Size = new System.Drawing.Size(35, 13);
			this._lblMinSharp.TabIndex = 0;
			this._lblMinSharp.Text = "label1";
			// 
			// _lblMinContr
			// 
			this._lblMinContr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblMinContr.AutoSize = true;
			this._lblMinContr.Location = new System.Drawing.Point(156, 277);
			this._lblMinContr.Name = "_lblMinContr";
			this._lblMinContr.Size = new System.Drawing.Size(35, 13);
			this._lblMinContr.TabIndex = 0;
			this._lblMinContr.Text = "label1";
			// 
			// _lblMinSatur
			// 
			this._lblMinSatur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblMinSatur.AutoSize = true;
			this._lblMinSatur.Location = new System.Drawing.Point(156, 251);
			this._lblMinSatur.Name = "_lblMinSatur";
			this._lblMinSatur.Size = new System.Drawing.Size(35, 13);
			this._lblMinSatur.TabIndex = 0;
			this._lblMinSatur.Text = "label1";
			// 
			// _lblMinBr
			// 
			this._lblMinBr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._lblMinBr.AutoSize = true;
			this._lblMinBr.Location = new System.Drawing.Point(156, 224);
			this._lblMinBr.Name = "_lblMinBr";
			this._lblMinBr.Size = new System.Drawing.Size(35, 13);
			this._lblMinBr.TabIndex = 0;
			this._lblMinBr.Text = "label1";
			// 
			// PropertyImagingSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._lblMinCr);
			this.Controls.Add(this._lblMinCb);
			this.Controls.Add(this._lblMinSharp);
			this.Controls.Add(this._lblMinContr);
			this.Controls.Add(this._lblMinSatur);
			this.Controls.Add(this._lblMinBr);
			this.Controls.Add(this._chbAutoWhiteBalance);
			this.Controls.Add(this._lblMaxCr);
			this.Controls.Add(this._lblMaxCb);
			this.Controls.Add(this._lblWhiteBalanceCB);
			this.Controls.Add(this._tbarWhiteBalanceCR);
			this.Controls.Add(this._lblWhiteBalanceCR);
			this.Controls.Add(this._tbarWhiteBalanceCB);
			this.Controls.Add(this._lblWhiteBalanceMode);
			this.Controls.Add(this._lblMaxSharp);
			this.Controls.Add(this._lblMaxContr);
			this.Controls.Add(this._lblMaxSatur);
			this.Controls.Add(this._tbarSharpness);
			this.Controls.Add(this._lblSaturation);
			this.Controls.Add(this._tbarContrast);
			this.Controls.Add(this._lblContrast);
			this.Controls.Add(this._lblSharpness);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._lblMaxBr);
			this.Controls.Add(this._lblBrightness);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this._title);
			this.Controls.Add(this._tbarSaturation);
			this.Controls.Add(this._tbarBrightness);
			this.Name = "PropertyImagingSettings";
			this.Size = new System.Drawing.Size(400, 456);
			((System.ComponentModel.ISupportInitialize)(this._tbarBrightness)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarSharpness)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarContrast)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarSaturation)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarWhiteBalanceCR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._tbarWhiteBalanceCB)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _title;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label _lblMaxBr;
		private FloatTrackBar _tbarBrightness;
		private System.Windows.Forms.TextBox _lblBrightness;
		private SaveCancelControl _saveCancelControl;
		private FloatTrackBar _tbarSharpness;
		private System.Windows.Forms.TextBox _lblSharpness;
		private FloatTrackBar _tbarContrast;
		private System.Windows.Forms.TextBox _lblContrast;
		private FloatTrackBar _tbarSaturation;
		private System.Windows.Forms.TextBox _lblSaturation;
		private System.Windows.Forms.Label _lblMaxSatur;
		private System.Windows.Forms.Label _lblMaxContr;
		private System.Windows.Forms.Label _lblMaxSharp;
		private System.Windows.Forms.TextBox _lblWhiteBalanceMode;
		private System.Windows.Forms.Label _lblMaxCr;
		private System.Windows.Forms.Label _lblMaxCb;
		private System.Windows.Forms.TextBox _lblWhiteBalanceCB;
		private FloatTrackBar _tbarWhiteBalanceCR;
		private System.Windows.Forms.TextBox _lblWhiteBalanceCR;
		private FloatTrackBar _tbarWhiteBalanceCB;
		private System.Windows.Forms.CheckBox _chbAutoWhiteBalance;
		private System.Windows.Forms.Label _lblMinCr;
		private System.Windows.Forms.Label _lblMinCb;
		private System.Windows.Forms.Label _lblMinSharp;
		private System.Windows.Forms.Label _lblMinContr;
		private System.Windows.Forms.Label _lblMinSatur;
		private System.Windows.Forms.Label _lblMinBr;
	}
}
