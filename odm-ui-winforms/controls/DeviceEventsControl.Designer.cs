namespace odm.controls {
	partial class DeviceEventsControl {
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
			this._title = new odm.controls.GroupBoxControl();
			this._lviewEvents = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.BackColor = System.Drawing.SystemColors.ControlLight;
			this._title.Location = new System.Drawing.Point(3, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(403, 23);
			this._title.TabIndex = 2;
			// 
			// _lviewEvents
			// 
			this._lviewEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lviewEvents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lviewEvents.CausesValidation = false;
			this._lviewEvents.Cursor = System.Windows.Forms.Cursors.Default;
			this._lviewEvents.FullRowSelect = true;
			this._lviewEvents.GridLines = true;
			this._lviewEvents.HideSelection = false;
			this._lviewEvents.Location = new System.Drawing.Point(0, 32);
			this._lviewEvents.MultiSelect = false;
			this._lviewEvents.Name = "_lviewEvents";
			this._lviewEvents.ShowGroups = false;
			this._lviewEvents.Size = new System.Drawing.Size(406, 312);
			this._lviewEvents.TabIndex = 1;
			this._lviewEvents.UseCompatibleStateImageBehavior = false;
			this._lviewEvents.View = System.Windows.Forms.View.Details;
			// 
			// DeviceEventsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._title);
			this.Controls.Add(this._lviewEvents);
			this.Name = "DeviceEventsControl";
			this.Size = new System.Drawing.Size(409, 347);
			this.ResumeLayout(false);

		}

		#endregion

		private GroupBoxControl _title;
		private System.Windows.Forms.ListView _lviewEvents;
	}
}
