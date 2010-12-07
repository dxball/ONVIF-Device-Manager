namespace nvc.controls
{
    partial class ErrorMessageForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._btnClose = new System.Windows.Forms.Button();
            this._errorControlPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _btnClose
            // 
            this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnClose.Location = new System.Drawing.Point(208, 236);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(75, 23);
            this._btnClose.TabIndex = 1;
            this._btnClose.Text = "OK";
            this._btnClose.UseVisualStyleBackColor = true;
            // 
            // _errorControlPanel
            // 
            this._errorControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._errorControlPanel.Location = new System.Drawing.Point(12, 12);
            this._errorControlPanel.Name = "_errorControlPanel";
            this._errorControlPanel.Size = new System.Drawing.Size(260, 218);
            this._errorControlPanel.TabIndex = 2;
            // 
            // ErrorMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this._errorControlPanel);
            this.Controls.Add(this._btnClose);
            this.Name = "ErrorMessageForm";
            this.Text = "ErrorMessageForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnClose;
        private System.Windows.Forms.Panel _errorControlPanel;
    }
}