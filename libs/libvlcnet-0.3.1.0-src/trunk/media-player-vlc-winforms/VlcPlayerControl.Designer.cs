// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.

namespace DZ.MediaPlayer.Vlc.WindowsForms
{
    partial class VlcPlayerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.vlcWindowControl = new DZ.MediaPlayer.Vlc.WindowsForms.VlcWindowControl();
            this.SuspendLayout();
            // 
            // vlcWindowControl
            // 
            this.vlcWindowControl.AutoSize = true;
            this.vlcWindowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcWindowControl.Location = new System.Drawing.Point(0, 0);
            this.vlcWindowControl.Name = "vlcWindowControl";
            this.vlcWindowControl.PlayerVisible = true;
            this.vlcWindowControl.Size = new System.Drawing.Size(150, 150);
            this.vlcWindowControl.TabIndex = 0;
            // 
            // VlcPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vlcWindowControl);
            this.Name = "VlcPlayerControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public VlcWindowControl vlcWindowControl;
    }
}