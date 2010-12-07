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

namespace nvc {

    using nvc.controls;
	using System.Reflection;
	using nvc.controllers;
	using nvc.controlsUIProvider;
	using onvifdm.utils;
	using System.Threading;
	using System.Runtime.InteropServices;
	using System.Diagnostics;
    public partial class MainWindow : Form
    {
		CommonApplicationStrings _strings = new CommonApplicationStrings();
		public Action<Size, FormWindowState> onSize{get;set;}
		public Action<int> SplitContainerLeftSize { get; set; }
		public bool IsMaximized;
		int _splitterDistance;
		public int SplitterDistance { 
			private get {
				return _splitterDistance;
			} 
			set {
				_splitterDistance = value;
			} 
		}
		//variable tocheck if the form is loaded to avoid uncorrect setings for size
		bool loaded;

		public MainWindow() 
        {
			this.DoubleBuffered = true;

			InitializeComponent();
            InitControls();

			Localization();
            
            BackColor = ColorDefinition.colControlBackground;
            _langPanel.BackColor = ColorDefinition.colControlBackground;
            _mainStatusStrip.BackColor = ColorDefinition.colControlBackground;
            _lblStatus1.BackColor = ColorDefinition.colControlBackground;
            _lblStatus2.BackColor = ColorDefinition.colControlBackground;
            _lblStatus3.BackColor = ColorDefinition.colControlBackground;

			SizeChanged += new EventHandler(MainWindow_SizeChanged);
			Load += new EventHandler(MainWindow_Load);
			FormClosing += new FormClosingEventHandler(MainWindow_FormClosing);

			_splitContainerA.Panel1.SizeChanged += new EventHandler(Panel1_SizeChanged);
		}

		void Panel1_SizeChanged(object sender, EventArgs e) {
			if (SplitContainerLeftSize != null && loaded) {
				SplitContainerLeftSize(_splitContainerA.Panel1.Width);
			}
		}

		void MainWindow_Load(object sender, EventArgs e) {
			LogSubscription();
			loaded = true;
			if (IsMaximized)
				WindowState = FormWindowState.Maximized;

			_splitContainerA.SplitterDistance = _splitterDistance;
		}

		void MainWindow_SizeChanged(object sender, EventArgs e) {
			if (onSize != null && loaded) {
				onSize(Size, WindowState);
			}
		}

		void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
			if (logSubscription != null)
				logSubscription.Dispose();
			UIProvider.Instance.ReleaseAll();
			WorkflowController.Instance.ReleaseControllers();
		}

		void Localization(){
			var ver = Assembly.GetExecutingAssembly().GetName().Version;
			Text = String.Format("{0} v{1}.{2}.{3}", _strings.applicationName, ver.Major, ver.Minor, ver.Build);
		}

		EmptyNotifierControl _emptyCtrl;
		public SplitContainer MainSplitContainer {
			get {
				return _splitContainerA;
			}
		}
        public void InitControls()
        {
            //Fills some data if no devices founded
			_emptyCtrl = new EmptyNotifierControl();
			_emptyCtrl.Dock = DockStyle.Fill;
			_splitContainerA.Panel2.Controls.Add(_emptyCtrl);
			
			

            //Colors
            Color bckColor = ColorDefinition.colMainWindowBackkground;
            _splitContainerA.BackColor = bckColor;
        }

		public void InitFrame() {
			_splitContainerA.Panel2.Controls.ForEach(x => { ((UserControl)x).Dispose(); });
			_splitContainerA.Panel2.Controls.Clear();
		}
        public void InitFrame(UserControl ctrl)
        {
			_splitContainerA.Panel2.Controls.ForEach(x => { ((UserControl)x).Dispose(); });
            _splitContainerA.Panel2.Controls.Clear();
            _splitContainerA.Panel2.Controls.Add(ctrl);
        } 
        public void InitLeftFrame(UserControl ctrl)
        {
            _splitContainerA.Panel1.Controls.Add(ctrl);
        }

		ODMLogger _logger;
		ODMLogger Logger { 
			get { 
				if(_logger == null ){
					_logger = new ODMLogger(LogMessages.ToList());
					AddMessageToUI = _logger.AddMessage;
					RemoveMessageFromUI = _logger.RemoveMessage;
				}
				if (_logger.IsDisposed) {
					_logger = new ODMLogger(LogMessages.ToList());
					AddMessageToUI = _logger.AddMessage;
					RemoveMessageFromUI = _logger.RemoveMessage;
				}
				return _logger;
			} 
		}
		private void button1_Click(object sender, EventArgs e) {
			if (Logger.Created)
				return;
			else {
				Logger._parent = this;
				Logger.Show();
			}

		}

		int buffer = 100;
		Queue<LogMessage> LogMessages = new Queue<LogMessage>();
		public Action<LogMessage> AddMessageToUI;
		public Action<LogMessage> RemoveMessageFromUI;

		void AddMessage(LogMessage msg) {
			if (LogMessages.Count > buffer){
				if(RemoveMessageFromUI != null)
					RemoveMessageFromUI(LogMessages.Dequeue());
				else
					LogMessages.Dequeue();
			}
			LogMessages.Enqueue(msg);
			if (AddMessageToUI != null)
				AddMessageToUI(msg);
		}
		IDisposable logSubscription;
		void LogSubscription() {
			logSubscription = ObservableTraceListener.GetLogMessages().ObserveOn(SynchronizationContext.Current)
				.Subscribe(logMsg => {
					AddMessage(logMsg);
				});

		}
	}
}
