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
using System.Text;
using System.Windows.Forms;
using nvc.controls;
using nvc.models;

namespace nvc.controllers {
	public class MainWindowController {
		MainWindow MainView { get { return _mainWindow; } }
		MainWindow _mainWindow;
		//MainFrameController _controller;

		#region Init section
		public MainWindowController() {
			var mainWindow = new MainWindow();
			_mainWindow = mainWindow;
			_mainWindow.InitLeftFrame(WorkflowController.Instance.GetDeviceListController().CreateDeviceListControl());
		}

		public void RunErrorFrame(UserControl errorFrame) {
			MainView.InitFrame(errorFrame);
		}
		public void Refrersh() {
			if (_mainWindow != null)
				_mainWindow.InitControls();
		}
		public void ClearMainFrame() {
			MainView.InitFrame();
		}
		public void RunMainFrame(DeviceDescriptionModel devModel) {
			var _controller = WorkflowController.Instance.GetMainFrameController();

			DeviceCapabilityModel devCapability= new DeviceCapabilityModel();
			MainView.InitFrame(_controller.CreateMainFrame(devCapability, devModel.session));
		}
		public MainWindow GetWindowRun() {
			return _mainWindow;
		}
		#endregion

		public void SetStatusBarText1(string value) {
			_mainWindow._lblStatus1.Text = value;
		}
		public void SetStatusBarText2(string value) {
			_mainWindow._lblStatus2.Text = value;
		}
		public void SetStatusBarText3(string value) {
			_mainWindow._lblStatus3.Text = value;
		}
	}
}
