namespace odm.controls {
	partial class TitleBar {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TitleBar));
			this._pbAbout = new System.Windows.Forms.PictureBox();
			this._cmbLocale = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this._pbAbout)).BeginInit();
			this.SuspendLayout();
			// 
			// _pbAbout
			// 
			this._pbAbout.Image = ((System.Drawing.Image)(resources.GetObject("_pbAbout.Image")));
			this._pbAbout.Location = new System.Drawing.Point(79, 0);
			this._pbAbout.Name = "_pbAbout";
			this._pbAbout.Size = new System.Drawing.Size(22, 20);
			this._pbAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._pbAbout.TabIndex = 3;
			this._pbAbout.TabStop = false;
			this._pbAbout.Click += new System.EventHandler(this._pbAbout_Click);
			// 
			// _cmbLocale
			// 
			this._cmbLocale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbLocale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._cmbLocale.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this._cmbLocale.FormattingEnabled = true;
			this._cmbLocale.Location = new System.Drawing.Point(0, 0);
			this._cmbLocale.Margin = new System.Windows.Forms.Padding(0);
			this._cmbLocale.Name = "_cmbLocale";
			this._cmbLocale.Size = new System.Drawing.Size(76, 21);
			this._cmbLocale.Sorted = true;
			this._cmbLocale.TabIndex = 2;
			this._cmbLocale.SelectedIndexChanged += new System.EventHandler(this._cmbLocale_SelectedIndexChanged);
			// 
			// TitleBar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._pbAbout);
			this.Controls.Add(this._cmbLocale);
			this.Name = "TitleBar";
			this.Size = new System.Drawing.Size(103, 22);
			((System.ComponentModel.ISupportInitialize)(this._pbAbout)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox _pbAbout;
		private System.Windows.Forms.ComboBox _cmbLocale;
	}
}
