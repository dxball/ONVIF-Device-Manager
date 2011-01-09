using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.utils;
using System.Drawing;
using odm.utils.controlsUIProvider;

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
			//_mainWindow._mainFramePanel.IsEnabled = false;
		}
		public void EnableControls() {
			//_mainWindow._mainFramePanel.IsEnabled = true;
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
