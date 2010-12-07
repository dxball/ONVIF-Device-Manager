namespace nvc.controls
{
    partial class EmptyNotifierControl
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
			this._tbNitifier = new System.Windows.Forms.TextBox();
			this._textBoxTitle = new System.Windows.Forms.TextBox();
			this._tbContacts = new System.Windows.Forms.TextBox();
			this._imgLogo = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this._imgLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// _tbNitifier
			// 
			this._tbNitifier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this._tbNitifier.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._tbNitifier.Location = new System.Drawing.Point(3, 42);
			this._tbNitifier.Multiline = true;
			this._tbNitifier.Name = "_tbNitifier";
			this._tbNitifier.ReadOnly = true;
			this._tbNitifier.Size = new System.Drawing.Size(234, 235);
			this._tbNitifier.TabIndex = 0;
			this._tbNitifier.TabStop = false;
			// 
			// _textBoxTitle
			// 
			this._textBoxTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._textBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._textBoxTitle.ForeColor = System.Drawing.Color.DimGray;
			this._textBoxTitle.Location = new System.Drawing.Point(3, 3);
			this._textBoxTitle.Multiline = true;
			this._textBoxTitle.Name = "_textBoxTitle";
			this._textBoxTitle.ReadOnly = true;
			this._textBoxTitle.Size = new System.Drawing.Size(234, 33);
			this._textBoxTitle.TabIndex = 2;
			// 
			// _tbContacts
			// 
			this._tbContacts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this._tbContacts.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._tbContacts.Location = new System.Drawing.Point(3, 283);
			this._tbContacts.Multiline = true;
			this._tbContacts.Name = "_tbContacts";
			this._tbContacts.ReadOnly = true;
			this._tbContacts.Size = new System.Drawing.Size(234, 78);
			this._tbContacts.TabIndex = 3;
			// 
			// _imgLogo
			// 
			this._imgLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._imgLogo.Image = global::nvc.properties.Resources.onvif_slogan;
			this._imgLogo.Location = new System.Drawing.Point(243, 3);
			this._imgLogo.Name = "_imgLogo";
			this._imgLogo.Size = new System.Drawing.Size(228, 358);
			this._imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this._imgLogo.TabIndex = 1;
			this._imgLogo.TabStop = false;
			// 
			// EmptyNotifierControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tbContacts);
			this.Controls.Add(this._textBoxTitle);
			this.Controls.Add(this._imgLogo);
			this.Controls.Add(this._tbNitifier);
			this.Name = "EmptyNotifierControl";
			this.Size = new System.Drawing.Size(478, 370);
			((System.ComponentModel.ISupportInitialize)(this._imgLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _tbNitifier;
        private System.Windows.Forms.PictureBox _imgLogo;
        private System.Windows.Forms.TextBox _textBoxTitle;
        private System.Windows.Forms.TextBox _tbContacts;
    }
}
