namespace nvc.controls {
	partial class PropertyTimeZone {
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
			this._titleCurrentTime = new nvc.controls.GroupBoxControl();
			this._saveCancelControl = new nvc.controls.SaveCancelControl();
			this._lblCurrentDate = new System.Windows.Forms.TextBox();
			this._lblCurrentTime = new System.Windows.Forms.TextBox();
			this._titleNewTime = new nvc.controls.GroupBoxControl();
			this._lblTimeZone = new System.Windows.Forms.TextBox();
			this._cmbTimeZone = new System.Windows.Forms.ComboBox();
			this._lblAutoDaylightSave = new System.Windows.Forms.TextBox();
			this._chbAutoDaylightSave = new System.Windows.Forms.CheckBox();
			this._grpBoxTimeMode = new System.Windows.Forms.GroupBox();
			this._tbManualTime = new System.Windows.Forms.DateTimePicker();
			this._tbManualDate = new System.Windows.Forms.DateTimePicker();
			this._tbCompTime = new System.Windows.Forms.DateTimePicker();
			this._tbCompDate = new System.Windows.Forms.DateTimePicker();
			this._lblManualTime = new System.Windows.Forms.TextBox();
			this._lblManualDate = new System.Windows.Forms.TextBox();
			this._rbManual = new System.Windows.Forms.RadioButton();
			this._rbNtp = new System.Windows.Forms.RadioButton();
			this._rbComp = new System.Windows.Forms.RadioButton();
			this._lblSetManual = new System.Windows.Forms.TextBox();
			this._linkNtpServer = new System.Windows.Forms.LinkLabel();
			this._lblNtpServer = new System.Windows.Forms.TextBox();
			this._lblSynchronizeNTP = new System.Windows.Forms.TextBox();
			this._lblCompDate = new System.Windows.Forms.TextBox();
			this._lblCompTime = new System.Windows.Forms.TextBox();
			this._lblSynchronizeComp = new System.Windows.Forms.TextBox();
			this._grpBoxTimeZone = new System.Windows.Forms.GroupBox();
			this._tbCurrentDate = new System.Windows.Forms.DateTimePicker();
			this._tbCurrentTime = new System.Windows.Forms.DateTimePicker();
			this._grpBoxTimeMode.SuspendLayout();
			this._grpBoxTimeZone.SuspendLayout();
			this.SuspendLayout();
			// 
			// _titleCurrentTime
			// 
			this._titleCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._titleCurrentTime.BackColor = System.Drawing.SystemColors.ControlLight;
			this._titleCurrentTime.Location = new System.Drawing.Point(3, 3);
			this._titleCurrentTime.Name = "_titleCurrentTime";
			this._titleCurrentTime.Size = new System.Drawing.Size(437, 23);
			this._titleCurrentTime.TabIndex = 1;
			// 
			// _saveCancelControl
			// 
			this._saveCancelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this._saveCancelControl.Location = new System.Drawing.Point(3, 355);
			this._saveCancelControl.Margin = new System.Windows.Forms.Padding(0);
			this._saveCancelControl.Name = "_saveCancelControl";
			this._saveCancelControl.Size = new System.Drawing.Size(198, 23);
			this._saveCancelControl.TabIndex = 10;
			// 
			// _lblCurrentDate
			// 
			this._lblCurrentDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblCurrentDate.Location = new System.Drawing.Point(3, 32);
			this._lblCurrentDate.Name = "_lblCurrentDate";
			this._lblCurrentDate.ReadOnly = true;
			this._lblCurrentDate.Size = new System.Drawing.Size(73, 20);
			this._lblCurrentDate.TabIndex = 19;
			this._lblCurrentDate.TabStop = false;
			this._lblCurrentDate.Text = "Date";
			// 
			// _lblCurrentTime
			// 
			this._lblCurrentTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblCurrentTime.Location = new System.Drawing.Point(216, 33);
			this._lblCurrentTime.Name = "_lblCurrentTime";
			this._lblCurrentTime.ReadOnly = true;
			this._lblCurrentTime.Size = new System.Drawing.Size(73, 20);
			this._lblCurrentTime.TabIndex = 20;
			this._lblCurrentTime.TabStop = false;
			this._lblCurrentTime.Text = "Time";
			// 
			// _titleNewTime
			// 
			this._titleNewTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._titleNewTime.BackColor = System.Drawing.SystemColors.ControlLight;
			this._titleNewTime.Location = new System.Drawing.Point(3, 58);
			this._titleNewTime.Name = "_titleNewTime";
			this._titleNewTime.Size = new System.Drawing.Size(437, 23);
			this._titleNewTime.TabIndex = 23;
			// 
			// _lblTimeZone
			// 
			this._lblTimeZone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblTimeZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblTimeZone.Location = new System.Drawing.Point(7, 19);
			this._lblTimeZone.Name = "_lblTimeZone";
			this._lblTimeZone.ReadOnly = true;
			this._lblTimeZone.Size = new System.Drawing.Size(197, 20);
			this._lblTimeZone.TabIndex = 24;
			this._lblTimeZone.TabStop = false;
			this._lblTimeZone.Text = "Time zone";
			// 
			// _cmbTimeZone
			// 
			this._cmbTimeZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbTimeZone.FormattingEnabled = true;
			this._cmbTimeZone.Location = new System.Drawing.Point(213, 19);
			this._cmbTimeZone.Name = "_cmbTimeZone";
			this._cmbTimeZone.Size = new System.Drawing.Size(218, 21);
			this._cmbTimeZone.TabIndex = 25;
			// 
			// _lblAutoDaylightSave
			// 
			this._lblAutoDaylightSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblAutoDaylightSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblAutoDaylightSave.Location = new System.Drawing.Point(7, 45);
			this._lblAutoDaylightSave.Name = "_lblAutoDaylightSave";
			this._lblAutoDaylightSave.ReadOnly = true;
			this._lblAutoDaylightSave.Size = new System.Drawing.Size(397, 20);
			this._lblAutoDaylightSave.TabIndex = 26;
			this._lblAutoDaylightSave.TabStop = false;
			this._lblAutoDaylightSave.Text = "Automatically adjust for daylighy saving time changes.";
			// 
			// _chbAutoDaylightSave
			// 
			this._chbAutoDaylightSave.AutoSize = true;
			this._chbAutoDaylightSave.Location = new System.Drawing.Point(416, 46);
			this._chbAutoDaylightSave.Name = "_chbAutoDaylightSave";
			this._chbAutoDaylightSave.Size = new System.Drawing.Size(15, 14);
			this._chbAutoDaylightSave.TabIndex = 27;
			this._chbAutoDaylightSave.UseVisualStyleBackColor = true;
			// 
			// _grpBoxTimeMode
			// 
			this._grpBoxTimeMode.Controls.Add(this._tbManualTime);
			this._grpBoxTimeMode.Controls.Add(this._tbManualDate);
			this._grpBoxTimeMode.Controls.Add(this._tbCompTime);
			this._grpBoxTimeMode.Controls.Add(this._tbCompDate);
			this._grpBoxTimeMode.Controls.Add(this._lblManualTime);
			this._grpBoxTimeMode.Controls.Add(this._lblManualDate);
			this._grpBoxTimeMode.Controls.Add(this._rbManual);
			this._grpBoxTimeMode.Controls.Add(this._rbNtp);
			this._grpBoxTimeMode.Controls.Add(this._rbComp);
			this._grpBoxTimeMode.Controls.Add(this._lblSetManual);
			this._grpBoxTimeMode.Controls.Add(this._linkNtpServer);
			this._grpBoxTimeMode.Controls.Add(this._lblNtpServer);
			this._grpBoxTimeMode.Controls.Add(this._lblSynchronizeNTP);
			this._grpBoxTimeMode.Controls.Add(this._lblCompDate);
			this._grpBoxTimeMode.Controls.Add(this._lblCompTime);
			this._grpBoxTimeMode.Controls.Add(this._lblSynchronizeComp);
			this._grpBoxTimeMode.Location = new System.Drawing.Point(3, 167);
			this._grpBoxTimeMode.Name = "_grpBoxTimeMode";
			this._grpBoxTimeMode.Size = new System.Drawing.Size(437, 185);
			this._grpBoxTimeMode.TabIndex = 28;
			this._grpBoxTimeMode.TabStop = false;
			this._grpBoxTimeMode.Text = "Time mode:";
			// 
			// _tbManualTime
			// 
			this._tbManualTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this._tbManualTime.Location = new System.Drawing.Point(309, 146);
			this._tbManualTime.Name = "_tbManualTime";
			this._tbManualTime.ShowUpDown = true;
			this._tbManualTime.Size = new System.Drawing.Size(122, 20);
			this._tbManualTime.TabIndex = 49;
			// 
			// _tbManualDate
			// 
			this._tbManualDate.CustomFormat = "dd MMM yyyy";
			this._tbManualDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this._tbManualDate.Location = new System.Drawing.Point(309, 121);
			this._tbManualDate.Name = "_tbManualDate";
			this._tbManualDate.Size = new System.Drawing.Size(122, 20);
			this._tbManualDate.TabIndex = 48;
			// 
			// _tbCompTime
			// 
			this._tbCompTime.Enabled = false;
			this._tbCompTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this._tbCompTime.Location = new System.Drawing.Point(309, 45);
			this._tbCompTime.Name = "_tbCompTime";
			this._tbCompTime.ShowUpDown = true;
			this._tbCompTime.Size = new System.Drawing.Size(122, 20);
			this._tbCompTime.TabIndex = 47;
			// 
			// _tbCompDate
			// 
			this._tbCompDate.CustomFormat = "dd MMM yyyy";
			this._tbCompDate.Enabled = false;
			this._tbCompDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this._tbCompDate.Location = new System.Drawing.Point(309, 20);
			this._tbCompDate.Name = "_tbCompDate";
			this._tbCompDate.Size = new System.Drawing.Size(122, 20);
			this._tbCompDate.TabIndex = 46;
			// 
			// _lblManualTime
			// 
			this._lblManualTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblManualTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblManualTime.Location = new System.Drawing.Point(230, 147);
			this._lblManualTime.Name = "_lblManualTime";
			this._lblManualTime.ReadOnly = true;
			this._lblManualTime.Size = new System.Drawing.Size(73, 20);
			this._lblManualTime.TabIndex = 43;
			this._lblManualTime.TabStop = false;
			this._lblManualTime.Text = "Time";
			// 
			// _lblManualDate
			// 
			this._lblManualDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblManualDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblManualDate.Location = new System.Drawing.Point(230, 121);
			this._lblManualDate.Name = "_lblManualDate";
			this._lblManualDate.ReadOnly = true;
			this._lblManualDate.Size = new System.Drawing.Size(73, 20);
			this._lblManualDate.TabIndex = 42;
			this._lblManualDate.TabStop = false;
			this._lblManualDate.Text = "Date";
			// 
			// _rbManual
			// 
			this._rbManual.AutoSize = true;
			this._rbManual.Location = new System.Drawing.Point(210, 122);
			this._rbManual.Name = "_rbManual";
			this._rbManual.Size = new System.Drawing.Size(14, 13);
			this._rbManual.TabIndex = 41;
			this._rbManual.UseVisualStyleBackColor = true;
			// 
			// _rbNtp
			// 
			this._rbNtp.AutoSize = true;
			this._rbNtp.Location = new System.Drawing.Point(210, 84);
			this._rbNtp.Name = "_rbNtp";
			this._rbNtp.Size = new System.Drawing.Size(14, 13);
			this._rbNtp.TabIndex = 40;
			this._rbNtp.UseVisualStyleBackColor = true;
			// 
			// _rbComp
			// 
			this._rbComp.AutoSize = true;
			this._rbComp.Checked = true;
			this._rbComp.Location = new System.Drawing.Point(210, 20);
			this._rbComp.Name = "_rbComp";
			this._rbComp.Size = new System.Drawing.Size(14, 13);
			this._rbComp.TabIndex = 39;
			this._rbComp.TabStop = true;
			this._rbComp.UseVisualStyleBackColor = true;
			// 
			// _lblSetManual
			// 
			this._lblSetManual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblSetManual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblSetManual.Location = new System.Drawing.Point(6, 121);
			this._lblSetManual.Name = "_lblSetManual";
			this._lblSetManual.ReadOnly = true;
			this._lblSetManual.Size = new System.Drawing.Size(198, 20);
			this._lblSetManual.TabIndex = 38;
			this._lblSetManual.TabStop = false;
			this._lblSetManual.Text = "Set manually";
			// 
			// _linkNtpServer
			// 
			this._linkNtpServer.AutoSize = true;
			this._linkNtpServer.Location = new System.Drawing.Point(306, 84);
			this._linkNtpServer.Name = "_linkNtpServer";
			this._linkNtpServer.Size = new System.Drawing.Size(98, 13);
			this._linkNtpServer.TabIndex = 37;
			this._linkNtpServer.TabStop = true;
			this._linkNtpServer.Text = "No server specified";
			// 
			// _lblNtpServer
			// 
			this._lblNtpServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblNtpServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblNtpServer.Location = new System.Drawing.Point(230, 82);
			this._lblNtpServer.Name = "_lblNtpServer";
			this._lblNtpServer.ReadOnly = true;
			this._lblNtpServer.Size = new System.Drawing.Size(73, 20);
			this._lblNtpServer.TabIndex = 36;
			this._lblNtpServer.TabStop = false;
			this._lblNtpServer.Text = "Ntp server";
			// 
			// _lblSynchronizeNTP
			// 
			this._lblSynchronizeNTP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblSynchronizeNTP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblSynchronizeNTP.Location = new System.Drawing.Point(6, 82);
			this._lblSynchronizeNTP.Name = "_lblSynchronizeNTP";
			this._lblSynchronizeNTP.ReadOnly = true;
			this._lblSynchronizeNTP.Size = new System.Drawing.Size(198, 20);
			this._lblSynchronizeNTP.TabIndex = 34;
			this._lblSynchronizeNTP.TabStop = false;
			this._lblSynchronizeNTP.Text = "Synchronize with Ntp server";
			// 
			// _lblCompDate
			// 
			this._lblCompDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblCompDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblCompDate.Location = new System.Drawing.Point(230, 19);
			this._lblCompDate.Name = "_lblCompDate";
			this._lblCompDate.ReadOnly = true;
			this._lblCompDate.Size = new System.Drawing.Size(73, 20);
			this._lblCompDate.TabIndex = 31;
			this._lblCompDate.TabStop = false;
			this._lblCompDate.Text = "Date";
			// 
			// _lblCompTime
			// 
			this._lblCompTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblCompTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblCompTime.Location = new System.Drawing.Point(230, 45);
			this._lblCompTime.Name = "_lblCompTime";
			this._lblCompTime.ReadOnly = true;
			this._lblCompTime.Size = new System.Drawing.Size(73, 20);
			this._lblCompTime.TabIndex = 30;
			this._lblCompTime.TabStop = false;
			this._lblCompTime.Text = "Time";
			// 
			// _lblSynchronizeComp
			// 
			this._lblSynchronizeComp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._lblSynchronizeComp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lblSynchronizeComp.Location = new System.Drawing.Point(6, 19);
			this._lblSynchronizeComp.Name = "_lblSynchronizeComp";
			this._lblSynchronizeComp.ReadOnly = true;
			this._lblSynchronizeComp.Size = new System.Drawing.Size(198, 20);
			this._lblSynchronizeComp.TabIndex = 28;
			this._lblSynchronizeComp.TabStop = false;
			this._lblSynchronizeComp.Text = "Synchronize with computer time";
			// 
			// _grpBoxTimeZone
			// 
			this._grpBoxTimeZone.Controls.Add(this._lblTimeZone);
			this._grpBoxTimeZone.Controls.Add(this._cmbTimeZone);
			this._grpBoxTimeZone.Controls.Add(this._chbAutoDaylightSave);
			this._grpBoxTimeZone.Controls.Add(this._lblAutoDaylightSave);
			this._grpBoxTimeZone.Location = new System.Drawing.Point(3, 87);
			this._grpBoxTimeZone.Name = "_grpBoxTimeZone";
			this._grpBoxTimeZone.Size = new System.Drawing.Size(437, 74);
			this._grpBoxTimeZone.TabIndex = 29;
			this._grpBoxTimeZone.TabStop = false;
			this._grpBoxTimeZone.Text = "Time zone:";
			// 
			// _tbCurrentDate
			// 
			this._tbCurrentDate.CustomFormat = "dd MMM yyyy";
			this._tbCurrentDate.Enabled = false;
			this._tbCurrentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this._tbCurrentDate.Location = new System.Drawing.Point(82, 33);
			this._tbCurrentDate.Name = "_tbCurrentDate";
			this._tbCurrentDate.Size = new System.Drawing.Size(128, 20);
			this._tbCurrentDate.TabIndex = 30;
			// 
			// _tbCurrentTime
			// 
			this._tbCurrentTime.Enabled = false;
			this._tbCurrentTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this._tbCurrentTime.Location = new System.Drawing.Point(295, 33);
			this._tbCurrentTime.Name = "_tbCurrentTime";
			this._tbCurrentTime.ShowUpDown = true;
			this._tbCurrentTime.Size = new System.Drawing.Size(145, 20);
			this._tbCurrentTime.TabIndex = 31;
			// 
			// PropertyTimeZone
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tbCurrentTime);
			this.Controls.Add(this._tbCurrentDate);
			this.Controls.Add(this._grpBoxTimeZone);
			this.Controls.Add(this._grpBoxTimeMode);
			this.Controls.Add(this._titleNewTime);
			this.Controls.Add(this._lblCurrentTime);
			this.Controls.Add(this._lblCurrentDate);
			this.Controls.Add(this._saveCancelControl);
			this.Controls.Add(this._titleCurrentTime);
			this.Name = "PropertyTimeZone";
			this.Size = new System.Drawing.Size(443, 392);
			this._grpBoxTimeMode.ResumeLayout(false);
			this._grpBoxTimeMode.PerformLayout();
			this._grpBoxTimeZone.ResumeLayout(false);
			this._grpBoxTimeZone.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GroupBoxControl _titleCurrentTime;
		private SaveCancelControl _saveCancelControl;
		private System.Windows.Forms.TextBox _lblCurrentDate;
		private System.Windows.Forms.TextBox _lblCurrentTime;
		private GroupBoxControl _titleNewTime;
		private System.Windows.Forms.TextBox _lblTimeZone;
		private System.Windows.Forms.ComboBox _cmbTimeZone;
		private System.Windows.Forms.TextBox _lblAutoDaylightSave;
		private System.Windows.Forms.CheckBox _chbAutoDaylightSave;
		private System.Windows.Forms.GroupBox _grpBoxTimeMode;
		private System.Windows.Forms.TextBox _lblManualTime;
		private System.Windows.Forms.TextBox _lblManualDate;
		private System.Windows.Forms.RadioButton _rbManual;
		private System.Windows.Forms.RadioButton _rbNtp;
		private System.Windows.Forms.RadioButton _rbComp;
		private System.Windows.Forms.TextBox _lblSetManual;
		private System.Windows.Forms.LinkLabel _linkNtpServer;
		private System.Windows.Forms.TextBox _lblNtpServer;
		private System.Windows.Forms.TextBox _lblSynchronizeNTP;
		private System.Windows.Forms.TextBox _lblCompDate;
		private System.Windows.Forms.TextBox _lblCompTime;
		private System.Windows.Forms.TextBox _lblSynchronizeComp;
		private System.Windows.Forms.GroupBox _grpBoxTimeZone;
		private System.Windows.Forms.DateTimePicker _tbCurrentDate;
		private System.Windows.Forms.DateTimePicker _tbCurrentTime;
		private System.Windows.Forms.DateTimePicker _tbManualTime;
		private System.Windows.Forms.DateTimePicker _tbManualDate;
		private System.Windows.Forms.DateTimePicker _tbCompTime;
		private System.Windows.Forms.DateTimePicker _tbCompDate;
	}
}
