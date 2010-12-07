using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nvc.controllers;

namespace nvc.controls {
	public partial class InfoPageNotification : BasePropertyControl {
		public InfoPageNotification() {
			InitializeComponent();
			_btnClose.CreateBinding(x => x.Text, InfoFormStrings.Instance, x=>x.close);
			_btnClose.Click += new EventHandler(_btnClose_Click);
		}

		void _btnClose_Click(object sender, EventArgs e) {
			if (OnClickAction != null)
				OnClickAction();
		}
		InfoFormStrings _strings = new InfoFormStrings();
		Action _onClickAction;
		public string Title {
			set {
				_title.Text = value;
			}
		}
		public Action OnClickAction { 
			get {
				return _onClickAction;
			} 
			set {
				IsButtonVisible = true;
				_progressLoading.Visible = false;
				_onClickAction = value;
			} 
		}
		public bool IsProgressLoadingVisible {
			set {
				_progressLoading.Visible = value;
			}
		}
		public bool IsButtonVisible { 
			set {
				_btnClose.Visible = value;
			} 
		}
		public string Message {
			get {
				return _infoTextBox.Text;
			}
			set { _infoTextBox.Text = value; }
		}

		void Localization() {
			_btnClose.CreateBinding(x => x.Text, _strings, x=>x.close);
		}
	}
}
