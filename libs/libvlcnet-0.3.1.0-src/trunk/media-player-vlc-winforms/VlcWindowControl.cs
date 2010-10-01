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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DZ.MediaPlayer.Io;

namespace DZ.MediaPlayer.Vlc.WindowsForms
{
    /// <summary>
    /// User control represents a window can be used to render video.
    /// </summary>
    [ToolboxBitmap(typeof (VlcWindowControl), "vlc_icon.png")]
    public partial class VlcWindowControl : UserControl
    {
        private readonly VlcPlayerControlWindow window;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VlcWindowControl() {
            InitializeComponent();
            //
            window = new VlcPlayerControlWindow(panel1, panel2, panelHost, this);
        }

        /// <summary>
        /// Window can be passed to the VLC <see cref="Player"/>
        /// </summary>
        public MediaWindow Window {
            get {
                return (window);
            }
        }

        /// <summary>
        /// Is player's subwindow visible on the control's surface.
        /// </summary>
        [Browsable(true)]
        public bool PlayerVisible {
            get {
                return (window.Visible);
            }
            set {
                window.Visible = value;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                window.Dispose();
            }
            //
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Nested type : VlcPlayerControlWindow

        private sealed class VlcPlayerControlWindow : DoubleWindowBase
        {
            private readonly Panel panel1;
            private readonly Panel panel2;
            private readonly Panel panelHost;
            private readonly VlcWindowControl controlHost;

            private bool isFirstPanelActive;
            private bool isVisible;

            public VlcPlayerControlWindow(Panel panel1, Panel panel2, Panel panelHost, VlcWindowControl control) {
                if (panel1 == null) {
                    throw new ArgumentNullException("panel1");
                }
                if (panel2 == null) {
                    throw new ArgumentNullException("panel2");
                }
                if (panelHost == null) {
                    throw new ArgumentNullException("panelHost");
                }
                if (control == null) {
                    throw new ArgumentNullException("control");
                }
                //
                this.panel1 = panel1;
                this.panel2 = panel2;
                this.panelHost = panelHost;
                controlHost = control;
                //
                isFirstPanelActive = true;
                // Default visibility is true
                playerVisible = true;
                isVisible = true;
                setVisible(true);
            }

            public override bool Visible {
                get {
                    return (isVisible);
                }
                set {
                    if (isVisible != value) {
                        isVisible = value;
                        //
                        setVisible(isVisible);
                    }
                }
            }

            private void setVisible(bool visible) {
                if (!visible) {
                    setPlayerVisible(false);
                    //
                    panelHost.Visible = false;
                    controlHost.Visible = false;
                } else {
                    controlHost.Visible = true;
                    panelHost.Visible = true;
                    //
                    setPlayerVisible(playerVisible);
                }
            }

            private Panel getActivePanel() {
                return (isFirstPanelActive ? panel1 : panel2);
            }

            private Panel getInactivePanel() {
                return (isFirstPanelActive ? panel2 : panel1);
            }

            protected override IntPtr GetActiveWindowHandle() {
                return (getActivePanel().Handle);
            }

            protected override IntPtr GetInactiveWindowHandle() {
                return (getInactivePanel().Handle);
            }

            protected override void SwitchWindows() {
                isFirstPanelActive = !isFirstPanelActive;
                //
                setVisible(isVisible);
            }

            public override int Width {
                get {
                    return (controlHost.Width);
                }
                set {
                    controlHost.Width = value;
                }
            }

            public override int Height {
                get {
                    return (controlHost.Height);
                }
                set {
                    controlHost.Height = value;
                }
            }

            public override int Left {
                get {
                    return (controlHost.Left);
                }
                set {
                    controlHost.Left = value;
                }
            }

            public override int Top {
                get {
                    return (controlHost.Top);
                }
                set {
                    controlHost.Top = value;
                }
            }

            public override uint BackgroundColor {
                get {
                    return unchecked((uint) (controlHost.BackColor.ToArgb()));
                }
                set {
                    controlHost.BackColor = Color.FromArgb(unchecked((int) value));
                }
            }

            public override string BackgroundImageFilePath {
                get {
                    throw new NotSupportedException("Comming soon.");
                }
                set {
                    throw new NotSupportedException("Comming soon.");
                }
            }

            private bool playerVisible;

            protected override bool PlayerVisible {
                get {
                    return (playerVisible);
                }
                set {
                    if (playerVisible != value) {
                        playerVisible = value;
                        //
                        setPlayerVisible(playerVisible);
                    }
                }
            }

            private void setPlayerVisible(bool isPlayerVisible) {
                getInactivePanel().Visible = false;
                //
                if (isPlayerVisible) {
                    getActivePanel().Visible = true;
                } else {
                    getActivePanel().Visible = false;
                }
            }
        }

        #endregion
    }
}