namespace nvc.controls
{
    partial class SaveCancelControl
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
			this._btnSave = new System.Windows.Forms.Button();
			this._btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _btnSave
			// 
			this._btnSave.Location = new System.Drawing.Point(0, 0);
			this._btnSave.Name = "_btnSave";
			this._btnSave.Size = new System.Drawing.Size(85, 23);
			this._btnSave.TabIndex = 0;
			this._btnSave.Text = "Save";
			this._btnSave.UseVisualStyleBackColor = true;
			// 
			// _btnCancel
			// 
			this._btnCancel.Location = new System.Drawing.Point(91, 0);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(85, 23);
			this._btnCancel.TabIndex = 1;
			this._btnCancel.Text = "Cancel";
			this._btnCancel.UseVisualStyleBackColor = true;
			// 
			// SaveCancelControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._btnSave);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SaveCancelControl";
			this.Size = new System.Drawing.Size(181, 23);
			this.ResumeLayout(false);

        }

        #endregion

		public System.Windows.Forms.Button _btnSave;
		public System.Windows.Forms.Button _btnCancel;

	}
}
