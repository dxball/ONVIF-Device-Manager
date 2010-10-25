namespace nvc.controls {
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
			this._size = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this._size)).BeginInit();
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
			this._btnSetSize.Location = new System.Drawing.Point(97, 27);
			this._btnSetSize.Name = "_btnSetSize";
			this._btnSetSize.Size = new System.Drawing.Size(59, 23);
			this._btnSetSize.TabIndex = 2;
			this._btnSetSize.Text = "Set";
			this._btnSetSize.UseVisualStyleBackColor = true;
			this._btnSetSize.Click += new System.EventHandler(this._btnSetSize_Click);
			// 
			// _size
			// 
			this._size.Location = new System.Drawing.Point(12, 30);
			this._size.Maximum = new decimal(new int[] {
            320000,
            0,
            0,
            0});
			this._size.Name = "_size";
			this._size.Size = new System.Drawing.Size(79, 20);
			this._size.TabIndex = 3;
			// 
			// SetSize
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(165, 59);
			this.ControlBox = false;
			this.Controls.Add(this._size);
			this.Controls.Add(this._btnSetSize);
			this.Controls.Add(this._lblSize);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetSize";
			this.ShowInTaskbar = false;
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this._size)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _lblSize;
		private System.Windows.Forms.Button _btnSetSize;
		public System.Windows.Forms.NumericUpDown _size;
	}
}