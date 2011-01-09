using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using odm.controllers;
using odm.controlsUIProvider;
using odm.controls;
using odm.utils.controlsUIProvider;
using System.Reflection;
using System.ComponentModel;
using odm.utils;

namespace odm {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		WorkflowController _controller;
		WPFUIProvider _uiProvider;
		public MainWindow() {
			InitializeComponent();

			_uiProvider = WPFUIProvider.Instance;
			UIProvider.provider = (IUIProvider)_uiProvider;
			_uiProvider.MainWindowProvider._mainWindow = this;
			_controller = WorkflowController.Instance;
			_controller.GetMainWindowController().InitMainWindow();

			_mainFramePanel.Content = new StartUpImage();

			Closing += new CancelEventHandler(MainWindow_Closing);
		
			InitLocalization();
			Localization();
		}

		void MainWindow_Closing(object sender, CancelEventArgs e) {
			odm.Properties.Settings.Default.Save();
		}

		CommonApplicationStrings _strings = new CommonApplicationStrings();

		void InitLocalization() {
			var list = odm.localization.Language.AvailableLanguages.Select(x => odm.controls.ListItem.Wrap(x, y => y.DisplayName)).Where(u => u.Unwrap().iso3 != null).ToArray();

			var defItem = list.Where(x => x.Unwrap().iso3 == odm.Properties.Settings.Default.DefaultLocaleIso3).FirstOrDefault();

			foreach (var item in list)
				cbLocale.Items.Add(item);
			
			if (defItem == null) {
				defItem = odm.controls.ListItem.Wrap(odm.localization.Language.Default, x => "english");
				cbLocale.Items.Add(defItem);
			}
			cbLocale.SelectedItem = defItem;
		}

		public void Refresh() {
			//_mainFramePanel.Children.Clear();
			_mainFramePanel.Content = null;
		}

		void Localization() {
			//Title = _strings.applicationName;
			var ver = Assembly.GetExecutingAssembly().GetName().Version;
			Title = String.Format("{0} v{1}.{2}.{3}", _strings.applicationName, ver.Major, ver.Minor, ver.Build);
		}
		public void InitLeftFrame(UserControl ctrl) {
			frame1.Navigate(ctrl);
		}
		public void InitFrame(UserControl ctrl) {
			//_mainFramePanel.Children.Clear();
			//_mainFramePanel.Children.Add(ctrl);
			_mainFramePanel.Content = ctrl;
		}

		protected void ShowAboutWindow() {
			var dlgWin = new About();
			dlgWin.Owner = this;
			dlgWin.ShowInTaskbar = false;
			dlgWin.ShowDialog();
		}

		private void cbLocale_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var selection = (ListItem<odm.localization.Language>)cbLocale.SelectedItem;
			if (selection == null) {
				odm.localization.Language.Current = null;
			} else {
				odm.localization.Language.Current = selection.Unwrap();
				odm.Properties.Settings.Default.DefaultLocaleIso3 = odm.localization.Language.Current.iso3;
				odm.Properties.Settings.Default.Save();
			}
		}

		private void CmdHelpHandler(object sender, ExecutedRoutedEventArgs e) {
			ShowAboutWindow();
		}
	
	}
}
