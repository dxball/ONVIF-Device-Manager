namespace nvc.controls {
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
			this._title = new nvc.controls.GroupBoxControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this._lblBrightnessVal = new System.Windows.Forms.Label();
			this._tbarBrightness = new nvc.controls.FloatTrackBar();
			this._lblBrightness = new System.Windows.Forms.TextBox();
			this._saveCancelControl = new nvc.controls.SaveCancelControl();
			this._tbarSharpness = new nvc.controls.FloatTrackBar();
			this._lblSharpness = new System.Windows.Forms.TextBox();
			this._tbarContrast = new nvc.controls.FloatTrackBar();
			this._lblContrast = new System.Windows.Forms.TextBox();
			this._tbarSaturation = new nvc.controls.FloatTrackBar();
			this._lblSaturation = new System.Windows.Forms.TextBox();
			this._lblSaturationVal = new System.Windows.Forms.Label();
			this._lblContrastVal = new System.Windows.Forms.Label();
			this._lblSharpnessVal = new System.Windows.Forms.Label();
			this._lblWhiteBalanceMode = new System.Windows.Forms.TextBox();
			this._lblCrVal = new System.Windows.Forms.Label();
			this._lblCbVal = new System.Windows.Forms.Label();
			this._lblWhiteBalanceCB = new System.Windows.Forms.TextBox();
			this._tbarWhiteBalanceCR = new nvc.controls.FloatTrackBar();
			this._lblWhiteBalanceCR = new System.Windows.Forms.TextBox();
			this._tbarWhiteBalanceCB = new nvc.controls.FloatTrackBar();
			this._chbAutoWhiteBalance = new System.Windows.Forms.CheckBox();
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
			// _lblBrightnessVal
			// 
			this._lblBrightnessVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblBrightnessVal.AutoSize = true;
			this._lblBrightnessVal.Location = new System.Drawing.Point(362, 224);
			this._lblBrightnessVal.Name = "_lblBrightnessVal";
			this._lblBrightnessVal.Size = new System.Drawing.Size(35, 13);
			this._lblBrightnessVal.TabIndex = 22;
			this._lblBrightnessVal.Text = "label1";
			// 
			// _tbarBrightness
			// 
			this._tbarBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbarBrightness.fMaximum = 10F;
			this._tbarBrightness.fMinimum = 0F;
			this._tbarBrightness.fValue = 0F;
			this._tbarBrightness.Location = new System.Drawing.Point(234, 222);
			this._tbarBrightness.Name = "_tbarBrightness";
			this._tbarBrightness.Size = new System.Drawing.Size(134, 45);
			this._tbarBrightness.TabIndex = 21;
			this._tbarBrightness.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblBrightness
			// 
			this._lblBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblBrightness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblBrightness.Location = new System.Drawing.Point(5, 222);
			this._lblBrightness.Name = "_lblBrightness";
			this._lblBrightness.ReadOnly = true;
			this._lblBrightness.Size = new System.Drawing.Size(223, 20);
			this._lblBrightness.TabIndex = 20;
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
			this._tbarSharpness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbarSharpness.fMaximum = 10F;
			this._tbarSharpness.fMinimum = 0F;
			this._tbarSharpness.fValue = 0F;
			this._tbarSharpness.Location = new System.Drawing.Point(234, 300);
			this._tbarSharpness.Name = "_tbarSharpness";
			this._tbarSharpness.Size = new System.Drawing.Size(134, 45);
			this._tbarSharpness.TabIndex = 25;
			this._tbarSharpness.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblSharpness
			// 
			this._lblSharpness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblSharpness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblSharpness.Location = new System.Drawing.Point(5, 300);
			this._lblSharpness.Name = "_lblSharpness";
			this._lblSharpness.ReadOnly = true;
			this._lblSharpness.Size = new System.Drawing.Size(223, 20);
			this._lblSharpness.TabIndex = 24;
			this._lblSharpness.Text = "Sharpness";
			// 
			// _tbarContrast
			// 
			this._tbarContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbarContrast.fMaximum = 10F;
			this._tbarContrast.fMinimum = 0F;
			this._tbarContrast.fValue = 0F;
			this._tbarContrast.Location = new System.Drawing.Point(234, 275);
			this._tbarContrast.Name = "_tbarContrast";
			this._tbarContrast.Size = new System.Drawing.Size(134, 45);
			this._tbarContrast.TabIndex = 27;
			this._tbarContrast.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblContrast
			// 
			this._lblContrast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblContrast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblContrast.Location = new System.Drawing.Point(5, 275);
			this._lblContrast.Name = "_lblContrast";
			this._lblContrast.ReadOnly = true;
			this._lblContrast.Size = new System.Drawing.Size(223, 20);
			this._lblContrast.TabIndex = 26;
			this._lblContrast.Text = "Contrast";
			// 
			// _tbarSaturation
			// 
			this._tbarSaturation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbarSaturation.fMaximum = 10F;
			this._tbarSaturation.fMinimum = 0F;
			this._tbarSaturation.fValue = 0F;
			this._tbarSaturation.Location = new System.Drawing.Point(234, 249);
			this._tbarSaturation.Name = "_tbarSaturation";
			this._tbarSaturation.Size = new System.Drawing.Size(134, 45);
			this._tbarSaturation.TabIndex = 29;
			this._tbarSaturation.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblSaturation
			// 
			this._lblSaturation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblSaturation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblSaturation.Location = new System.Drawing.Point(5, 249);
			this._lblSaturation.Name = "_lblSaturation";
			this._lblSaturation.ReadOnly = true;
			this._lblSaturation.Size = new System.Drawing.Size(223, 20);
			this._lblSaturation.TabIndex = 28;
			this._lblSaturation.Text = "Color saturation";
			// 
			// _lblSaturationVal
			// 
			this._lblSaturationVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblSaturationVal.AutoSize = true;
			this._lblSaturationVal.Location = new System.Drawing.Point(362, 251);
			this._lblSaturationVal.Name = "_lblSaturationVal";
			this._lblSaturationVal.Size = new System.Drawing.Size(35, 13);
			this._lblSaturationVal.TabIndex = 30;
			this._lblSaturationVal.Text = "label1";
			// 
			// _lblContrastVal
			// 
			this._lblContrastVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblContrastVal.AutoSize = true;
			this._lblContrastVal.Location = new System.Drawing.Point(362, 277);
			this._lblContrastVal.Name = "_lblContrastVal";
			this._lblContrastVal.Size = new System.Drawing.Size(35, 13);
			this._lblContrastVal.TabIndex = 31;
			this._lblContrastVal.Text = "label1";
			// 
			// _lblSharpnessVal
			// 
			this._lblSharpnessVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblSharpnessVal.AutoSize = true;
			this._lblSharpnessVal.Location = new System.Drawing.Point(362, 302);
			this._lblSharpnessVal.Name = "_lblSharpnessVal";
			this._lblSharpnessVal.Size = new System.Drawing.Size(35, 13);
			this._lblSharpnessVal.TabIndex = 32;
			this._lblSharpnessVal.Text = "label1";
			// 
			// _lblWhiteBalanceMode
			// 
			this._lblWhiteBalanceMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblWhiteBalanceMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblWhiteBalanceMode.Location = new System.Drawing.Point(5, 324);
			this._lblWhiteBalanceMode.Name = "_lblWhiteBalanceMode";
			this._lblWhiteBalanceMode.ReadOnly = true;
			this._lblWhiteBalanceMode.Size = new System.Drawing.Size(223, 20);
			this._lblWhiteBalanceMode.TabIndex = 33;
			this._lblWhiteBalanceMode.Text = "White balance mode";
			// 
			// _lblCrVal
			// 
			this._lblCrVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblCrVal.AutoSize = true;
			this._lblCrVal.Location = new System.Drawing.Point(362, 381);
			this._lblCrVal.Name = "_lblCrVal";
			this._lblCrVal.Size = new System.Drawing.Size(35, 13);
			this._lblCrVal.TabIndex = 40;
			this._lblCrVal.Text = "label1";
			// 
			// _lblCbVal
			// 
			this._lblCbVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._lblCbVal.AutoSize = true;
			this._lblCbVal.Location = new System.Drawing.Point(362, 355);
			this._lblCbVal.Name = "_lblCbVal";
			this._lblCbVal.Size = new System.Drawing.Size(35, 13);
			this._lblCbVal.TabIndex = 39;
			this._lblCbVal.Text = "label5";
			// 
			// _lblWhiteBalanceCB
			// 
			this._lblWhiteBalanceCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblWhiteBalanceCB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblWhiteBalanceCB.Location = new System.Drawing.Point(5, 353);
			this._lblWhiteBalanceCB.Name = "_lblWhiteBalanceCB";
			this._lblWhiteBalanceCB.ReadOnly = true;
			this._lblWhiteBalanceCB.Size = new System.Drawing.Size(223, 20);
			this._lblWhiteBalanceCB.TabIndex = 37;
			this._lblWhiteBalanceCB.Text = "White balance Cb gain";
			// 
			// _tbarWhiteBalanceCR
			// 
			this._tbarWhiteBalanceCR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbarWhiteBalanceCR.fMaximum = 10F;
			this._tbarWhiteBalanceCR.fMinimum = 0F;
			this._tbarWhiteBalanceCR.fValue = 0F;
			this._tbarWhiteBalanceCR.Location = new System.Drawing.Point(234, 379);
			this._tbarWhiteBalanceCR.Name = "_tbarWhiteBalanceCR";
			this._tbarWhiteBalanceCR.Size = new System.Drawing.Size(134, 45);
			this._tbarWhiteBalanceCR.TabIndex = 36;
			this._tbarWhiteBalanceCR.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _lblWhiteBalanceCR
			// 
			this._lblWhiteBalanceCR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblWhiteBalanceCR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblWhiteBalanceCR.Location = new System.Drawing.Point(5, 379);
			this._lblWhiteBalanceCR.Name = "_lblWhiteBalanceCR";
			this._lblWhiteBalanceCR.ReadOnly = true;
			this._lblWhiteBalanceCR.Size = new System.Drawing.Size(223, 20);
			this._lblWhiteBalanceCR.TabIndex = 35;
			this._lblWhiteBalanceCR.Text = "White balance Cr gain";
			// 
			// _tbarWhiteBalanceCB
			// 
			this._tbarWhiteBalanceCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tbarWhiteBalanceCB.fMaximum = 10F;
			this._tbarWhiteBalanceCB.fMinimum = 0F;
			this._tbarWhiteBalanceCB.fValue = 0F;
			this._tbarWhiteBalanceCB.Location = new System.Drawing.Point(234, 353);
			this._tbarWhiteBalanceCB.Name = "_tbarWhiteBalanceCB";
			this._tbarWhiteBalanceCB.Size = new System.Drawing.Size(134, 45);
			this._tbarWhiteBalanceCB.TabIndex = 38;
			this._tbarWhiteBalanceCB.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// _chbAutoWhiteBalance
			// 
			this._chbAutoWhiteBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._chbAutoWhiteBalance.AutoSize = true;
			this._chbAutoWhiteBalance.Location = new System.Drawing.Point(240, 328);
			this._chbAutoWhiteBalance.Name = "_chbAutoWhiteBalance";
			this._chbAutoWhiteBalance.Size = new System.Drawing.Size(15, 14);
			this._chbAutoWhiteBalance.TabIndex = 41;
			this._chbAutoWhiteBalance.UseVisualStyleBackColor = true;
			// 
			// PropertyImagingSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._chbAutoWhiteBalance);
			this.Controls.Add(this._lblCrVal);
			this.Controls.Add(this._lblCbVal);
			this.Controls.Add(this._lblWhiteBalanceCB);
			this.Controls.Add(this._tbarWhiteBalanceCR);
			this.Controls.Add(this._lblWhiteBalanceCR);
			this.Controls.Add(this._tbarWhiteBalanceCB);
			this.Controls.Add(this._lblWhiteBalanceMode);
			this.Controls.Add(this._lblSharpnessVal);
			this.Controls.Add(this._lblContrastVal);
			this.Controls.Add(this._lblSaturationVal);
			this.Controls.Add(this._tbarSharpness);
			this.Controls.Add(this._lblSaturation);
			this.Controls.Add(this._tbarContrast);
			this.Controls.Add(this._lblContrast);
			this.Controls.Add(this._lblSharpness);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._lblBrightnessVal);
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
		private System.Windows.Forms.Label _lblBrightnessVal;
		private FloatTrackBar _tbarBrightness;
		private System.Windows.Forms.TextBox _lblBrightness;
		private SaveCancelControl _saveCancelControl;
		private FloatTrackBar _tbarSharpness;
		private System.Windows.Forms.TextBox _lblSharpness;
		private FloatTrackBar _tbarContrast;
		private System.Windows.Forms.TextBox _lblContrast;
		private FloatTrackBar _tbarSaturation;
		private System.Windows.Forms.TextBox _lblSaturation;
		private System.Windows.Forms.Label _lblSaturationVal;
		private System.Windows.Forms.Label _lblContrastVal;
		private System.Windows.Forms.Label _lblSharpnessVal;
		private System.Windows.Forms.TextBox _lblWhiteBalanceMode;
		private System.Windows.Forms.Label _lblCrVal;
		private System.Windows.Forms.Label _lblCbVal;
		private System.Windows.Forms.TextBox _lblWhiteBalanceCB;
		private FloatTrackBar _tbarWhiteBalanceCR;
		private System.Windows.Forms.TextBox _lblWhiteBalanceCR;
		private FloatTrackBar _tbarWhiteBalanceCB;
		private System.Windows.Forms.CheckBox _chbAutoWhiteBalance;
	}
}
