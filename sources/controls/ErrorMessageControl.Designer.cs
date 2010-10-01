namespace nvc.controls
{
    partial class ErrorMessageControl
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
			this._title = new nvc.controls.GroupBoxControl();
            this._tbErrorMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _title
            // 
            this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._title.BackColor = System.Drawing.SystemColors.ControlLight;
            this._title.Location = new System.Drawing.Point(3, 3);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(293, 23);
            this._title.TabIndex = 0;
            // 
            // _tbErrorMsg
            // 
            this._tbErrorMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tbErrorMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._tbErrorMsg.Location = new System.Drawing.Point(3, 32);
            this._tbErrorMsg.Multiline = true;
            this._tbErrorMsg.Name = "_tbErrorMsg";
            this._tbErrorMsg.ReadOnly = true;
            this._tbErrorMsg.Size = new System.Drawing.Size(293, 182);
            this._tbErrorMsg.TabIndex = 1;
            // 
            // ErrorMessageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tbErrorMsg);
            this.Controls.Add(this._title);
            this.Name = "ErrorMessageControl";
            this.Size = new System.Drawing.Size(299, 217);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBoxControl _title;
        private System.Windows.Forms.TextBox _tbErrorMsg;
    }
}
