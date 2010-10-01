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

using DZ.MediaPlayer.Vlc.WindowsForms;

namespace SimplePlayer
{
    partial class VideoWindow
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoWindow));
            this.vlcPlayerControl = new VlcPlayerControl();
            this.SuspendLayout();
            // 
            // vlcPlayerControl
            // 
            this.vlcPlayerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcPlayerControl.Location = new System.Drawing.Point(0, 0);
            this.vlcPlayerControl.Name = "vlcPlayerControl";
            this.vlcPlayerControl.Position = 0;
            this.vlcPlayerControl.Size = new System.Drawing.Size(426, 319);
            this.vlcPlayerControl.TabIndex = 0;
            this.vlcPlayerControl.Time = System.TimeSpan.Parse("00:00:00");
            this.vlcPlayerControl.Volume = 50;
            // 
            // VideoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 319);
            this.Controls.Add(this.vlcPlayerControl);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "VideoWindow";
            this.Text = "VideoWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private VlcPlayerControl vlcPlayerControl;
    }
}