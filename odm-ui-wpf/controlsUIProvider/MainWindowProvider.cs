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
using System.Drawing;

using odm.ui;
using odm.utils.controlsUIProvider;
using odm.utils;

namespace odm.controlsUIProvider {
	public class MainWindowProvider : BaseUIProvider, IMainWindowProvider {
		public MainWindow _mainWindow;

		public MainWindow MainView { 
			get {
				//if (_mainWindow == null) {
				//    _mainWindow = new MainWindow() { onSize = SizeChanged, SplitContainerLeftSize = SplitContainerLeftSizeChanged};
				//    _mainWindow.Size = Settings.Default.MainWindowSize;
				//    _mainWindow.SplitterDistance = Settings.Default.MainWindowLeftPanelSize;
				//    _mainWindow.IsMaximized = Settings.Default.MainWindowMaximize;
				//}
				return _mainWindow; 
			} 
		}

		void SplitContainerLeftSizeChanged(int width) {
			//Settings.Default.MainWindowLeftPanelSize = width;
			//Settings.Default.Save();
		}
		//void SizeChanged(Size size,  state){
		//    //if (state == FormWindowState.Maximized)
		//    //	Settings.Default.MainWindowMaximize = true;
		//    //else {
		//    //	Settings.Default.MainWindowMaximize = false;
		//    //	Settings.Default.MainWindowSize = size;
		//    //}

		//    //Settings.Default.Save();
		//}

		public void InitLeftFrame() {
			MainView.InitLeftFrame(WPFUIProvider.Instance.DevicesListProvider.DevListControl);
		}
		public void Refrersh() {
			if (_mainWindow != null)
				_mainWindow.Refresh();
		}

		public void DisableControls() {
			_mainWindow._mainFramePanel.IsEnabled = false;
		}
		public void EnableControls() {
			_mainWindow._mainFramePanel.IsEnabled = true;
		}


		//Initialise right working frame. Create MainFrame controller and add to Main Window split container
		public void InitFrame() {
			MainView.InitFrame(WPFUIProvider.Instance.MainFrameProvider.MainFrameControl);
		}

		public void SetStatusBarText1(string value) {
			//MainView._lblStatus1.Text = value;
		}
		public void SetStatusBarText2(string value) {
			//MainView._lblStatus2.Text = value;
		}
		public void SetStatusBarText3(string value) {
			//MainView._lblStatus3.Text = value;
		}

		public override void ReleaseUI() {
			//_mainWindow._mainFramePanel.Children.Clear();
			_mainWindow._mainFramePanel.Content = null;
		}
	}
}
