namespace odm.controls
{
    partial class LinkCheckButton
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
            this._checkBox = new System.Windows.Forms.CheckBox();
            this._linkLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _checkBox
            // 
            this._checkBox.AutoSize = true;
            this._checkBox.Location = new System.Drawing.Point(0, 0);
            this._checkBox.Name = "_checkBox";
            this._checkBox.Size = new System.Drawing.Size(15, 14);
            this._checkBox.TabIndex = 1;
            this._checkBox.UseVisualStyleBackColor = true;
            this._checkBox.CheckedChanged += new System.EventHandler(this._checkBox_CheckedChanged);
            // 
            // _linkLabel
            // 
            this._linkLabel.AutoSize = true;
            this._linkLabel.ForeColor = System.Drawing.Color.Blue;
            this._linkLabel.Location = new System.Drawing.Point(21, 0);
            this._linkLabel.Name = "_linkLabel";
            this._linkLabel.Size = new System.Drawing.Size(35, 13);
            this._linkLabel.TabIndex = 3;
            this._linkLabel.Text = "label1";
            // 
            // LinkCheckButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._linkLabel);
            this.Controls.Add(this._checkBox);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LinkCheckButton";
            this.Size = new System.Drawing.Size(183, 15);
            this.Load += new System.EventHandler(this.LinkCheckButton_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _checkBox;
        private System.Windows.Forms.Label _linkLabel;

    }
}
