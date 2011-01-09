namespace odm.controls {
	partial class TriggerControl {
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
			this._trigger = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// _trigger
			// 
			this._trigger.AutoSize = true;
			this._trigger.Location = new System.Drawing.Point(0, 0);
			this._trigger.Margin = new System.Windows.Forms.Padding(0);
			this._trigger.Name = "_trigger";
			this._trigger.Size = new System.Drawing.Size(14, 13);
			this._trigger.TabIndex = 0;
			this._trigger.TabStop = true;
			this._trigger.UseVisualStyleBackColor = true;
			// 
			// TriggerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._trigger);
			this.Name = "TriggerControl";
			this.Size = new System.Drawing.Size(14, 14);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton _trigger;
	}
}
