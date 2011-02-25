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
using odm.ui.controls;
using odm.utils.controlsUIProvider;
using System.Reflection;
using System.ComponentModel;
using odm.utils;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using odm.utils.extensions;

namespace odm.ui {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : CustomWindow, INotifyPropertyChanged {
		
		WorkflowController _controller;
		WPFUIProvider _uiProvider;
		
		public MainWindow() {
			Init();
		}

		public MainWindow(string theme) {
			m_currentTheme = theme;
			Init();
		}

		protected void Init() {
			InitializeComponent();

			_uiProvider = WPFUIProvider.Instance;
			UIProvider.provider = (IUIProvider)_uiProvider;
			_uiProvider.MainWindowProvider._mainWindow = this;
			_controller = WorkflowController.Instance;
			_controller.GetMainWindowController().InitMainWindow();

			_mainFramePanel.Content = new StartUpImage();

			Closing += new CancelEventHandler(MainWindow_Closing);


			WindowState = System.Windows.WindowState.Normal;

			InitLocalization();
			Localization();
		}

		void MainWindow_Closing(object sender, CancelEventArgs e) {
			odm.ui.Properties.Settings.Default.Save();
			WorkflowController.Instance.KillEveryBody();
		}

		void InitLocalization() {
			var list = odm.localization.Language.AvailableLanguages.Select(x => odm.ui.controls.ListItem.Wrap(x, y => y.DisplayName)).Where(u => u.Unwrap().iso3 != null).ToArray();

			var defItem = list.Where(x => x.Unwrap().iso3 == odm.ui.Properties.Settings.Default.DefaultLocaleIso3).FirstOrDefault();

			foreach (var item in list)
				cbLocale.Items.Add(item);

			if (defItem == null) {
				defItem = odm.ui.controls.ListItem.Wrap(odm.localization.Language.Default, x => "english");
				cbLocale.Items.Add(defItem);
			}
			cbLocale.SelectedItem = defItem;
		}

		public void Refresh() {
			_mainFramePanel.Content = null;
		}

		void Localization() {
			var ver = Assembly.GetExecutingAssembly().GetName().Version;
			this.Title = String.Format("{0} v{1}.{2}.{3}", CommonApplicationStrings.Instance.applicationName, ver.Major, ver.Minor, ver.Build);
		}
		public void InitLeftFrame(UserControl ctrl) {
			frame1.Content = ctrl;
		}
		public void InitFrame(UserControl ctrl) {
			_mainFramePanel.Content = ctrl;
		}

		protected void ShowAboutWindow() {
			var dlgWin = new About();
			dlgWin.Owner = this;
			TextOptions.SetTextFormattingMode(dlgWin, TextFormattingMode.Display);
			TextOptions.SetTextRenderingMode(dlgWin, TextRenderingMode.Aliased);
			dlgWin.ShowInTaskbar = false;
			dlgWin.ShowDialog();
		}

		private void cbLocale_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var selection = (ListItem<odm.localization.Language>)cbLocale.SelectedItem;
			if (selection == null) {
				odm.localization.Language.Current = null;
			} else {
				odm.localization.Language.Current = selection.Unwrap();
				odm.ui.Properties.Settings.Default.DefaultLocaleIso3 = odm.localization.Language.Current.iso3;
				odm.ui.Properties.Settings.Default.Save();
				Localization();
			}
		}

		private void CmdHelpHandler(object sender, ExecutedRoutedEventArgs e) {
			ShowAboutWindow();
		}

		private void button1_Click(object sender, RoutedEventArgs e) {
			GC.Collect();
		}

		public static DependencyProperty ToolBarProperty = DependencyProperty.RegisterAttached("ToolBar", typeof(FrameworkElement), typeof(MainWindow));
		public static FrameworkElement GetToolBar(DependencyObject ToolBar) {
			return (FrameworkElement)ToolBar.GetValue(ToolBarProperty);
		}
		public static void SetToolBar(DependencyObject ToolBar, Boolean value) {
			ToolBar.SetValue(ToolBarProperty, value);
		}
		public FrameworkElement ToolBar {
			get {
				return (FrameworkElement)GetValue(ToolBarProperty);
			}
			set {
				SetValue(ToolBarProperty, value);
			}
		}

		void AddDictionary(Uri uri) {
			ResourceDictionary dict = new ResourceDictionary();
			dict.Source = uri;
			Application.Current.Resources.MergedDictionaries.Add(dict);
		}

		public void NotifyPropertyChanged(string prop) {
			if(PropertyChanged!=null){
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		public string m_currentTheme = "origin-theme";
		public string currentTheme {
			get {
				return m_currentTheme;
			}
			set {
				if (value != m_currentTheme) {
					m_currentTheme = value;
					NotifyPropertyChanged("currentTheme");

					if (value == "swamp-theme") {
						Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
						Application.Current.MainWindow.Close();
						Application.Current.Resources.MergedDictionaries.Clear();
						AddDictionary(new Uri(@"CommonStyle.xaml", UriKind.Relative));
						AddDictionary(new Uri(@"TimePickerStyle.xaml", UriKind.Relative));
						AddDictionary(new Uri(@"GraphEditorResource.xaml", UriKind.Relative));
						AddDictionary(new Uri(@"themes/swamp/theme.xaml", UriKind.Relative));
						AddDictionary(new Uri(@"controls/GraphEditor/ApproStyle.xaml", UriKind.Relative));
						Application.Current.MainWindow = new MainWindow(value);
						Application.Current.MainWindow.Show();
						Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

					} else if (value == "origin-theme") {

						Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
						Application.Current.MainWindow.Close();
						Application.Current.Resources.MergedDictionaries.Clear();
						AddDictionary(new Uri(@"CommonStyle.xaml", UriKind.Relative));
						AddDictionary(new Uri(@"TimePickerStyle.xaml", UriKind.Relative));
						AddDictionary(new Uri(@"GraphEditorResource.xaml", UriKind.Relative));
						AddDictionary(new Uri(@"themes/origin/theme.xaml", UriKind.Relative));
						AddDictionary(new Uri(@"controls/GraphEditor/ApproStyle.xaml", UriKind.Relative));
						Application.Current.MainWindow = new MainWindow(value);
						Application.Current.MainWindow.Show();
						Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

					}
				}
			}
		}

		private void ThemeSelectionChanged(object sender, SelectionChangedEventArgs e) {
			var theme = (themeSelector.SelectedValue as ComboBoxItem).Content as string;

			if (theme == currentTheme) {
				return;
			}

			
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
