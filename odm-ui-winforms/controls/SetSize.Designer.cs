namespace odm.controls {
	partial class SetSize {
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
			this._lblSize = new System.Windows.Forms.Label();
			this._btnSetSize = new System.Windows.Forms.Button();
			this._heigth = new System.Windows.Forms.NumericUpDown();
			this._width = new System.Windows.Forms.NumericUpDown();
			this._lblHeigth = new System.Windows.Forms.TextBox();
			this._lblWidth = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this._heigth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._width)).BeginInit();
			this.SuspendLayout();
			// 
			// _lblSize
			// 
			this._lblSize.AutoSize = true;
			this._lblSize.Location = new System.Drawing.Point(12, 9);
			this._lblSize.Name = "_lblSize";
			this._lblSize.Size = new System.Drawing.Size(113, 13);
			this._lblSize.TabIndex = 1;
			this._lblSize.Text = "Set physical size in cm";
			// 
			// _btnSetSize
			// 
			this._btnSetSize.Location = new System.Drawing.Point(169, 39);
			this._btnSetSize.Name = "_btnSetSize";
			this._btnSetSize.Size = new System.Drawing.Size(59, 23);
			this._btnSetSize.TabIndex = 2;
			this._btnSetSize.Text = "Set";
			this._btnSetSize.UseVisualStyleBackColor = true;
			this._btnSetSize.Click += new System.EventHandler(this._btnSetSize_Click);
			// 
			// _heigth
			// 
			this._heigth.Location = new System.Drawing.Point(84, 39);
			this._heigth.Maximum = new decimal(new int[] {
            320000,
            0,
            0,
            0});
			this._heigth.Name = "_heigth";
			this._heigth.Size = new System.Drawing.Size(79, 20);
			this._heigth.TabIndex = 3;
			// 
			// _width
			// 
			this._width.Location = new System.Drawing.Point(84, 65);
			this._width.Maximum = new decimal(new int[] {
            320000,
            0,
            0,
            0});
			this._width.Name = "_width";
			this._width.Size = new System.Drawing.Size(79, 20);
			this._width.TabIndex = 5;
			// 
			// _lblHeigth
			// 
			this._lblHeigth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblHeigth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblHeigth.Location = new System.Drawing.Point(12, 39);
			this._lblHeigth.Name = "_lblHeigth";
			this._lblHeigth.ReadOnly = true;
			this._lblHeigth.Size = new System.Drawing.Size(66, 20);
			this._lblHeigth.TabIndex = 17;
			this._lblHeigth.TabStop = false;
			this._lblHeigth.Text = "Heigth";
			// 
			// _lblWidth
			// 
			this._lblWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblWidth.Location = new System.Drawing.Point(12, 65);
			this._lblWidth.Name = "_lblWidth";
			this._lblWidth.ReadOnly = true;
			this._lblWidth.Size = new System.Drawing.Size(66, 20);
			this._lblWidth.TabIndex = 18;
			this._lblWidth.TabStop = false;
			this._lblWidth.Text = "Width";
			// 
			// SetSize
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(229, 103);
			this.ControlBox = false;
			this.Controls.Add(this._lblWidth);
			this.Controls.Add(this._lblHeigth);
			this.Controls.Add(this._width);
			this.Controls.Add(this._heigth);
			this.Controls.Add(this._btnSetSize);
			this.Controls.Add(this._lblSize);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetSize";
			this.ShowInTaskbar = false;
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this._heigth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._width)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _lblSize;
		private System.Windows.Forms.Button _btnSetSize;
		public System.Windows.Forms.NumericUpDown _heigth;
		public System.Windows.Forms.NumericUpDown _width;
		private System.Windows.Forms.TextBox _lblHeigth;
		private System.Windows.Forms.TextBox _lblWidth;
	}
}