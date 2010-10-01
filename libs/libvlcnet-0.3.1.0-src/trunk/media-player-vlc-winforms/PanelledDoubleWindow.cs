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

#region Usings

using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace DZ.MediaPlayer.Vlc.WindowsForms {
    /// <summary>
    /// Represents a window based on two WinForms Panels.
    /// </summary>
	public sealed class PanelledDoubleWindow : DoubleWindowBase {
		private readonly Thread thread;
		private readonly bool useParentWindowsFormsContext;
		private readonly WindowsFormsThreadTask windowsFormsThreadTask;
		private string backgroundImageFilePath = String.Empty;

		private bool firstWindowIsActive = true;
		private bool playerVisible = true;
		private bool visible;

		#region Nested "private sealed class WindowsFormsThreadTask"

		#region Nested type: FormInvokerDelegate

		private delegate void FormInvokerDelegate();

		private delegate void FormInvokerDelegate<T>(T value);

		#endregion

		#region Nested type: WindowsFormsThreadTask

		private sealed class WindowsFormsThreadTask {
			private readonly EventWaitHandle handleCreatedEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
			internal Form form;
			internal Panel panel1;

		    private bool panel1Created;
            private bool panel2Created;

			internal IntPtr panel1Handle;
			internal Panel panel2;
		    
			internal IntPtr panel2Handle;

			internal void internalCreateForm() {
				form = new Form();
				form.FormBorderStyle = FormBorderStyle.None;
				form.StartPosition = FormStartPosition.WindowsDefaultLocation;
				form.Location = new Point(0, 0);
				// Important : HandleCreated event can be raised only after Visible will be set to true
				// (http://msdn.microsoft.com/en-us/library/system.windows.forms.control.handlecreated.aspx)
				form.Visible = false;
				form.TopMost = true;
				form.ShowInTaskbar = false;
				//
				if (!form.IsHandleCreated) {
					form.HandleCreated += internalFormHandleCreatedHandler;
				} else {
					internalFormHandleCreatedHandler(this, EventArgs.Empty);
				}
				form.Visible = true;
			}

			[STAThread]
			public void WindowsFormsThreadMain() {
				internalCreateForm();
				//
				Application.Run(form);
			}

			private void internalFormHandleCreatedHandler(object sender, EventArgs e) {
				form.MouseEnter += MouseEnterHandler;
				form.MouseLeave += MouseLeaveHandler;
				//
				panel1 = new Panel();
				panel1.Parent = form;
				panel1.Location = new Point(0, 0);
				panel1.Width = form.Width;
				panel1.Height = form.Height;
				panel1.Visible = false;
				//
				if (!panel1.IsHandleCreated) {
					panel1.HandleCreated += internalPanel1HandleCreated;
				} else {
					internalPanel1HandleCreated(this, EventArgs.Empty);
				}
				panel1.Visible = true;
				//
				panel2 = new Panel();
				panel2.Parent = form;
				panel2.Location = new Point(0, 0);
				panel2.Width = form.Width;
				panel2.Height = form.Height;
				panel2.Visible = false;
				//
				if (!panel2.IsHandleCreated) {
					panel2.HandleCreated += internalPanel2HandleCreated;
				} else {
					internalPanel2HandleCreated(this, EventArgs.Empty);
				}
				panel2.Visible = true;
			}

			private void internalPanel1HandleCreated(object sender, EventArgs e) {
				panel1Handle = panel1.Handle;
				//
				panel1.MouseEnter += MouseEnterHandler;
				panel1.MouseLeave += MouseLeaveHandler;
				//
				panel1Created = true;
				//
				if (panel1Created && panel2Created) {
					handleCreatedEvent.Set();
				}
			}

			private void internalPanel2HandleCreated(object sender, EventArgs e) {
				panel2Handle = panel2.Handle;
				//
				panel2.MouseEnter += MouseEnterHandler;
				panel2.MouseLeave += MouseLeaveHandler;
				//
				panel2Created = true;
				//
				if (panel1Created && panel2Created) {
					handleCreatedEvent.Set();
				}
			}

			private static void MouseEnterHandler(object sender, EventArgs e) {
				Cursor.Hide();
			}

			private static void MouseLeaveHandler(object sender, EventArgs e) {
				Cursor.Show();
			}

			public bool WaitForWindowsCreation(TimeSpan timeout) {
				while (!panel1Created && !panel2Created) {
					if (!handleCreatedEvent.WaitOne(timeout, false)) {
						return (false);
					}
				}
				return (true);
			}
		}

		#endregion

		#endregion

		#region Constructors & Destructors

		private bool isDisposed;

		/// <summary>
		/// Creates a window. Visible = false by default, and creates a separate thread to manage Windows messages.
		/// </summary>
		public PanelledDoubleWindow() :
			this(TimeSpan.FromSeconds(2), true) {
		}

		/// <summary>
		/// Creates a window. Visible = false by default.
		/// </summary>
		/// <param name="operationTimeout">Timeout to waiting a window creation.</param>
		/// <param name="useParentWindowsFormsContext">Creates a separate thread with Application.Run() if false.</param>
		public PanelledDoubleWindow(TimeSpan operationTimeout, bool useParentWindowsFormsContext) {
			this.useParentWindowsFormsContext = useParentWindowsFormsContext;
			//
			if (useParentWindowsFormsContext) {
				windowsFormsThreadTask = new WindowsFormsThreadTask();
				windowsFormsThreadTask.internalCreateForm();
			} else {
				//
				// Starting a new message dispatcher thread.
				// After that we have to call form.Invoke() to access its properties.
				windowsFormsThreadTask = new WindowsFormsThreadTask();
				thread = new Thread(windowsFormsThreadTask.WindowsFormsThreadMain);
				thread.IsBackground = false;
				
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
			}

			if (!windowsFormsThreadTask.WaitForWindowsCreation(operationTimeout)) {
				Dispose(false);
				throw new InvalidOperationException("Cannot create window handles.");
			}
			//
			// Initial visibility.
			windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate(formInvokeDelegate));
		}

		private void formInvokeDelegate() {
			windowsFormsThreadTask.form.Visible = visible;
			getActivePanel().Visible = true;
			getInactivePanel().Visible = false;
		}

        /// <summary>
        /// Cleanup resourcs.
        /// </summary>
        /// <param name="isDisposing"></param>
		protected override void Dispose(bool isDisposing) {
            if (isDisposed)
                return;
			//
			isDisposed = true;
			if (isDisposing) {
				if (!useParentWindowsFormsContext) {
					if (!thread.IsAlive) {
						throw new InvalidOperationException("Thread is already stopped.");
					}
				}
				//
				windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate(closeHandler));
				//
				if (!useParentWindowsFormsContext) {
					thread.Join();
				}
				//
				GC.SuppressFinalize(this);
			}
		}

		private void closeHandler() {
			windowsFormsThreadTask.form.Close();
		}

        /// <summary>
        /// TODO : Refactor this !
        /// Finalizer.
        /// </summary>
		~PanelledDoubleWindow() {
			Dispose(false);
		}

		#endregion

        private Panel getActivePanel() {
            return firstWindowIsActive ? windowsFormsThreadTask.panel1 : windowsFormsThreadTask.panel2;
        }

        private Panel getInactivePanel() {
            return firstWindowIsActive ? windowsFormsThreadTask.panel2 : windowsFormsThreadTask.panel1;
        }

        /// <summary>
        /// Window width.
        /// </summary>
        public override int Width {
			get {
				return windowsFormsThreadTask.form.Width;
			}
			set {
				windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate<int>(setWidthHandler), value);
			}
		}

        /// <summary>
        /// Window height.
        /// </summary>
        public override int Height {
			get {
				return windowsFormsThreadTask.form.Height;
			}
			set {
				windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate<int>(setHeightHandler), value);
			}
		}

        private void setWidthHandler(int value) {
            windowsFormsThreadTask.form.Width = value;
            windowsFormsThreadTask.panel1.Width = value;
            windowsFormsThreadTask.panel2.Width = value;
        }

        private void setHeightHandler(int value) {
			windowsFormsThreadTask.form.Height = value;
           	windowsFormsThreadTask.panel1.Height = value;
           	windowsFormsThreadTask.panel2.Height = value;
		}

        private void setLeftHandler(int value) {
            windowsFormsThreadTask.form.Left = value;
        }

        /// <summary>
        /// Window left position.
        /// </summary>
		public override int Left {
			get {
				return windowsFormsThreadTask.form.Left;
			}
			set {
				windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate<int>(setLeftHandler), value);
			}
		}

        /// <summary>
        /// Window top position.
        /// </summary>
        public override int Top {
			get {
				return windowsFormsThreadTask.form.Top;
			}
			set {
				windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate<int>(setTopHandler), value);
			}
		}

		private void setTopHandler(int value) {
			windowsFormsThreadTask.form.Top = value;
		}

	    private uint backColor;

        /// <summary>
        /// Background color code.
        /// </summary>
        public override uint BackgroundColor {
			get {
				return unchecked((uint) windowsFormsThreadTask.form.BackColor.ToArgb());
			}
			set {
			    backColor = value;
				windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate(setBackgroundColorHandler));
			}
		}

		private void setBackgroundColorHandler() {
            windowsFormsThreadTask.form.BackColor = Color.FromArgb(unchecked((int) backColor));
		}

        /// <summary>
        /// Background image for window.
        /// </summary>
		public override string BackgroundImageFilePath {
			get {
				return backgroundImageFilePath;
			}
			set {
				if (backgroundImageFilePath != value) {
					backgroundImageFilePath = value;
					windowsFormsThreadTask.form.Invoke(
						new FormInvokerDelegate(setBackgroundImageFilePathHandler));
				}
			}
		}

		private void setBackgroundImageFilePathHandler() {
			windowsFormsThreadTask.form.BackgroundImage = new Bitmap(backgroundImageFilePath);
		}

        /// <summary>
        /// Is window visible.
        /// </summary>
		public override bool Visible {
			get {
				return visible;
			}
			set {
				visible = value;
				//
				windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate(setVisibleHandler));
			}
		}

		private void setVisibleHandler() {
			if (!windowsFormsThreadTask.form.Visible) {
           		windowsFormsThreadTask.form.Visible = visible;
           	}
           	getInactivePanel().Visible = false;
           	getActivePanel().Visible = visible;
		}

        /// <summary>
        /// Is active panel visible.
        /// </summary>
		protected override bool PlayerVisible {
			get {
				return playerVisible;
			}
			set {
				playerVisible = value;
				//
				windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate(setPlayerVisibleHandler));
			}
		}

		private void setPlayerVisibleHandler() {
			getInactivePanel().Visible = false;
			getActivePanel().Visible = playerVisible;
		}

        /// <summary>
        /// Get handle of active panel.
        /// </summary>
		protected override IntPtr GetActiveWindowHandle() {
			return firstWindowIsActive ? windowsFormsThreadTask.panel1Handle : windowsFormsThreadTask.panel2Handle;
		}

        /// <summary>
        /// Get handle of inactive panel.
        /// </summary>
		protected override IntPtr GetInactiveWindowHandle() {
			return firstWindowIsActive ? windowsFormsThreadTask.panel2Handle : windowsFormsThreadTask.panel1Handle;
		}

        /// <summary>
        /// Switches active and inactive windows.
        /// </summary>
		protected override void SwitchWindows() {
			firstWindowIsActive = !firstWindowIsActive;
			windowsFormsThreadTask.form.Invoke(new FormInvokerDelegate(switchWindowsHandler));
		}

		private void switchWindowsHandler() {
			getInactivePanel().Visible = false;
			getActivePanel().Visible = playerVisible;
		}
	}
}