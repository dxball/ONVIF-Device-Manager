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
using System.Linq;
using System.Windows.Forms;
using odm.controllers;
using System.Reflection;
using System.IO;
using System.Threading;
using odm.utils;
using System.Runtime.ExceptionServices;
using System.Xml;
using System.Diagnostics;
using odm.utils.controlsUIProvider;

namespace odm.utils {
	public static class Program {
		public static string MapPath(string path) {
			var assembly = Assembly.GetExecutingAssembly();
			var baseDir = Path.GetDirectoryName(assembly.Location);
			string fullPath = null;
			if (path.StartsWith("~/")) {
				fullPath = Path.Combine(baseDir, path.Substring(2, path.Length - 2).Replace('/', '\\'));
			} else {
				fullPath = path.Replace('/', '\\');
			}
			return fullPath;
		}
		static Thread s_uiThread = null;
		public static Thread uiThread {
			get {
				dbg.Assert(s_uiThread != null);
				return s_uiThread;
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
#if GENERATE_FAKE_LOGMESSAGES
			Observable
				.Interval(TimeSpan.FromMilliseconds(500))
				.Subscribe(t => {
					LogUtils.WriteWarning("some warning....");
					LogUtils.WriteError("some error....");

					//DebugHelper.Assert(false);
					//DebugHelper.Error(new Exception("some exception error"));
					//DebugHelper.Info("some debug info");

					Trace.TraceError("some error message");
					Trace.TraceWarning("some warning message");
					Trace.TraceInformation("some inforamtion message");
				});
#endif
			s_uiThread = Thread.CurrentThread;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            var _controller = WorkflowController.Instance;
            _controller.GetMainWindowController().InitMainWindow();
			//[FINDME]
            //Application.Run(UIProvider.Instance.GetMainWindowProvider().MainView);
		}
	}
}
