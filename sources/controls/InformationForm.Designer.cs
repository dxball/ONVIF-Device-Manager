namespace nvc.controls {
	partial class InformationForm {
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
			this.savingSettingsControl1 = new nvc.controls.SavingSettingsControl();
			this.SuspendLayout();
			// 
			// savingSettingsControl1
			// 
			this.savingSettingsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.savingSettingsControl1.Location = new System.Drawing.Point(0, 0);
			this.savingSettingsControl1.Name = "savingSettingsControl1";
			this.savingSettingsControl1.Size = new System.Drawing.Size(683, 103);
			this.savingSettingsControl1.TabIndex = 0;
			// 
			// InformationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(683, 103);
			this.ControlBox = false;
			this.Controls.Add(this.savingSettingsControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "InformationForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SavingSettingsForm";
			this.Load += new System.EventHandler(this.SavingSettingsForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private SavingSettingsControl savingSettingsControl1;
	}
}