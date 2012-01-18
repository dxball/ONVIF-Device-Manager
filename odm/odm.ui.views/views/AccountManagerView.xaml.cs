using System;
using odm.ui.controls;
using odm.ui.viewModels;
using utils;
using System.Windows.Controls;

namespace odm.ui.views {
    /// <summary>
    /// Interaction logic for AccauntManagerView.xaml
    /// </summary>
	public partial class AccountManagerView : DialogWindow {
        public AccountManagerView(AccountManagerViewModel viewModel) {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.ApplyComplete += new EventHandler(viewModel_ApplyComplete);

            Loaded += new System.Windows.RoutedEventHandler(AccountManagerView_Loaded);

            accountsList.SelectionChanged += new SelectionChangedEventHandler(accountsList_SelectionChanged);
        }

        void accountsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            loginValue.Focus();
        }

        void AccountManagerView_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            Localization();
        }
        void viewModel_ApplyComplete(object sender, EventArgs e) {
            ((AccountManagerViewModel)this.DataContext).ApplyComplete -= viewModel_ApplyComplete;
            this.Close();
        }
        public LocalAccount Strings { get { return LocalAccount.instance; } }
        public LocalButtons ButtonsStrings { get { return LocalButtons.instance; } }
        void Localization() {
			this.Header = LocalTitles.instance.accountmanager;
            btnApply.CreateBinding(Button.ContentProperty, ButtonsStrings, s=>s.apply);
            btnDelete.CreateBinding(Button.ContentProperty, ButtonsStrings, s => s.delete);
            btnNew.CreateBinding(Button.ContentProperty, ButtonsStrings, s => s.create);

            loginCaption.CreateBinding(Button.ContentProperty, Strings, s => s.loginCaption);
            passwordCaption.CreateBinding(Button.ContentProperty, Strings, s => s.passwordCaption);
        }
    }
}
