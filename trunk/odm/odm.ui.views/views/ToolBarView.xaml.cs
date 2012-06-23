using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using odm.ui.activities;
using odm.ui.core;
using odm.ui.views;
using utils;

namespace odm.ui {
	/// <summary>
	/// Interaction logic for ToolBarView.xaml
	/// </summary>
	public partial class ToolBarView : UserControl {
		public static readonly RoutedCommand AppSettingsCommand = new RoutedCommand("AppSettings", typeof(ToolBarView));
		public static readonly RoutedCommand SecuritySettingsCommand = new RoutedCommand("SecuritySettings", typeof(ToolBarView));
		public static readonly RoutedCommand AboutCommand = new RoutedCommand("About", typeof(ToolBarView));
		public static readonly RoutedCommand BackgroundTasksCommand = new RoutedCommand("BackgroundTasks", typeof(ToolBarView));
		public static readonly RoutedCommand AccountSettingsCommand = new RoutedCommand("AccountSettings", typeof(ToolBarView));
		public ToolBarView(IUnityContainer container) {

			eventAggregator = container.Resolve<IEventAggregator>();

			this.CommandBindings.Add(
				new CommandBinding(
					ToolBarView.AppSettingsCommand,
					(s, a) => {
						var evarg = new DeviceLinkEventArgs();
						evarg.currentAccount = AccountManager.CurrentAccount;
						evarg.session = null;
						eventAggregator.GetEvent<AppSettingsClick>().Publish(true);
					}
				)
			);
            this.CommandBindings.Add(
                new CommandBinding(
                    ToolBarView.AccountSettingsCommand,
                    (s, a) => {
                        var evarg = new DeviceLinkEventArgs();
                        evarg.currentAccount = AccountManager.CurrentAccount;
                        evarg.session = null;
                        eventAggregator.GetEvent<AccountManagerClick>().Publish(evarg);
                    }
                )
            );
            
			this.CommandBindings.Add(
				new CommandBinding(
					ToolBarView.BackgroundTasksCommand,
					(s, a) => {
                        var evarg = new DeviceLinkEventArgs();
                        evarg.currentAccount = AccountManager.CurrentAccount;
                        evarg.session = null;
                        eventAggregator.GetEvent<BackgroundTasksClick>().Publish(true);
					}
				)
			);

			this.CommandBindings.Add(
				 new CommandBinding(
					  ToolBarView.AboutCommand,
					  (s, a) => {
						  var evarg = new DeviceLinkEventArgs();
						  evarg.currentAccount = AccountManager.CurrentAccount;
						  evarg.session = null;
						  eventAggregator.GetEvent<AboutClick>().Publish(evarg);
					  }
				 )
			);
            
            Accounts = new ObservableCollection<IAccount>();

			InitializeComponent();

			eventAggregator.GetEvent<InitAccounts>().Subscribe(ret => {
                //IsNotApply = true;
				InitAccounts(ret);
			});

            InitLocalization();
			InitAccounts();
		}
        void InitLocalization() {
            IEnumerable<odm.localization.Language> langs = odm.localization.Language.AvailableLanguages;
            odm.ui.controls.ListItem<odm.localization.Language>[] list = langs.Select(x => odm.ui.controls.ListItem.Wrap(x, y => y.DisplayName)).Where(u => u.Unwrap().iso3 != null).ToArray();

            var defItem = list.Where(x => x.Unwrap().iso3 == odm.ui.Properties.Settings.Default.DefaultLocaleIso3).FirstOrDefault();

            if (defItem == null) {
                defItem = odm.ui.controls.ListItem.Wrap(odm.localization.Language.Default, x => "english");
            }

            odm.localization.Language.Current = defItem.Unwrap();
        }
        
		public LocalDevice Strings { get { return LocalDevice.instance; } }
		public LocalTitles Titles { get { return LocalTitles.instance; } }
		IAccount anonymous;
		bool isNotApply;
		bool IsNotApply {
			get { 
				return isNotApply; 
			}
			set {
				isNotApply = value;
			}
		}
		IEventAggregator eventAggregator;
		public ObservableCollection<IAccount> Accounts { get; set; }
		
		void InitAccounts(InitAccountEventArgs args) {
			Accounts.Clear();

			anonymous = new AccountDescriptor(new odmAccount() { Name = "<Anonymous>" });
			Accounts.Add(anonymous);
			AccountManager.Load().ForEach(x => Accounts.Add(x));

			IsNotApply = !args.needrefresh;
			
			try {
				if (args.currentAccount == null || Accounts.Where(ac => ac.Name == args.currentAccount.Name && ac.Password == args.currentAccount.Password).First() == null)
					CurrentAccount = anonymous;
				else
					CurrentAccount = Accounts.Where(ac => ac.Name == args.currentAccount.Name && ac.Password == args.currentAccount.Password).First();      
			} catch (Exception err) {
			    dbg.Error(err);
			}
		}
		void InitAccounts() {
			IsNotApply = true;
			Accounts.Clear();

			anonymous = new AccountDescriptor(new odmAccount() { Name = "<Anonymous>" });
			Accounts.Add(anonymous);
			AccountManager.Load().ForEach(x => Accounts.Add(x));

			try {
				var curracc = AccountManager.CurrentAccount;
				if (curracc == null || Accounts.Where(ac => ac.Name == curracc.Name && ac.Password == curracc.Password).FirstOrDefault() == null)
					CurrentAccount = anonymous;
				else
					CurrentAccount = Accounts.Where(ac => ac.Name == curracc.Name && ac.Password == curracc.Password).FirstOrDefault();         
  			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		void OnAccountApply() {
			if (CurrentAccount == anonymous) {
				AccountManager.SetCurrent(new DefAccountDescriptor());
			} else {
				AccountManager.SetCurrent(CurrentAccount);
			}
			//AccountManager.SetCurrent(CurrentAccount);
			eventAggregator.GetEvent<Refresh>().Publish(true);
		}

		#region Commands
		void InitCommands() {
			ApplyClick = new DelegateCommand(() => {
				OnAccountApply();
			});
		}

		public DelegateCommand ApplyClick {
			get { return (DelegateCommand)GetValue(ApplyClickProperty); }
			set { SetValue(ApplyClickProperty, value); }
		}
		public static readonly DependencyProperty ApplyClickProperty =
			 DependencyProperty.Register("ApplyClick", typeof(DelegateCommand), typeof(ToolBarView));

		#endregion

       // IAccount SystemAccount;
		public IAccount CurrentAccount {get { return (IAccount)GetValue(CurrentAccountProperty); }set { SetValue(CurrentAccountProperty, value); }}
		public static readonly DependencyProperty CurrentAccountProperty =
			 DependencyProperty.Register("CurrentAccount", typeof(IAccount), typeof(ToolBarView), new PropertyMetadata((obj, evarg) => {
				 var vm = ((ToolBarView)obj);
				 if (!vm.IsNotApply)
					 vm.OnAccountApply();
				 vm.IsNotApply = false;
			 }));
	}
}
