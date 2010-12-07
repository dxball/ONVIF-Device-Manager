#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nvc.onvif;
using nvc.models;
using System.IO;
using System.Diagnostics;
using nvc.controllers;

namespace nvc.controls {
	public partial class InformationForm : Form {

		InfoFormStrings _strings = new InfoFormStrings();

		public InformationForm(string errorMessage) {
			InitializeComponent();
			Text = errorMessage;
			//savingSettingsControl1.SetErrorMessage(errorMessage);
			savingSettingsControl1.OnClose += new EventHandler(savingSettingsControl1_OnClose);

			InitialView();
		}
		public InformationForm() {
			InitializeComponent();
			Localization();
			InitialView();
			savingSettingsControl1.OnClose += new EventHandler(savingSettingsControl1_OnClose);
		}

		void InitialView() {
			Height = Defaults.iInformationFormInitialHeight;
			_webBrowser.Visible = false;
			//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		}
		void XmlView() {
			Height = Defaults.iInformationFormXMLViewHeight;
			_webBrowser.Visible = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
		}

		Action KillEveryBody;
		void savingSettingsControl1_OnClose(object sender, EventArgs e) {
			if (KillEveryBody != null)
				KillEveryBody();
			this.Close();
		}

		void Localization(){
			this.CreateBinding(x => x.Text, _strings, x => x.titleSave);
		}

		public void SetErrorMessage(string errText) {
			savingSettingsControl1.SetErrorMessage(errText);
		}
		[Conditional("DEBUG")]
		public void SetEttorXML(Exception error) {
			XmlView();
			ErrorInfoModel errModel = new ErrorInfoModel(error);
			string OutXml = errModel.htmlError;
			//HtmlDocument 
			_webBrowser.DocumentText = OutXml;
		}
		public void ShowCloseButton(Action killall) {
			KillEveryBody = killall;
			savingSettingsControl1.ShowCloseButton();
		}

		private void SavingSettingsForm_Load(object sender, EventArgs e) {

		}
	}
}
