using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.views;
using utils;

namespace odm.ui.viewModels {
    public class AccountManagerViewModel : DependencyObject, IDisposable{
        public event EventHandler ApplyComplete;
        public AccountManagerViewModel(IUnityContainer container) {
            this.container = container;
            Accounts = new ObservableCollection<IAccount>();
            InitCommands();
            Init();
        }

        public ObservableCollection<IAccount> Accounts { get; private set; }
        IUnityContainer container;
        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		IAccount initialAccount;

        void Init() {
            Accounts.Clear();
            List<IAccount> acclst = AccountManager.Load();
            acclst.ForEach(x => { Accounts.Add(x); });

			var acc = AccountManager.CurrentAccount;
			SelectedAccount = acclst.Where(a => a.Name == acc.Name && a.Password == acc.Password).FirstOrDefault();
			if (SelectedAccount != null)
				initialAccount = new AccountDescriptor(new odmAccount() { Name = SelectedAccount.Name, Password = SelectedAccount.Password});
        }
        //bool LoadAccountSettings() {   
        //    return CurrentAccount == null ? false : true;
        //}
        IAccount LoadCurrent() {
            return AccountManager.CurrentAccount;
        }
        void SetCurrent(IAccount account, bool useCustom) {
			//AccountManager.SetCurrent(account, useCustom);
        }

        #region Commands
        void InitCommands() {
            //SetClick = new DelegateCommand(() => {
            //    if (SelectedAccount == null)
            //        return;
            //    SelectedAccount.Name = AccountName;
            //    SelectedAccount.Password = AccountPassword;
            //}, () => {
            //    return IsEditable;
            //});
            DeleteClick = new DelegateCommand(() => {
                Accounts.Remove(SelectedAccount);
                //SelectedAccount = null;
            }, () => {
                return IsEditable;
            });
            NewClick = new DelegateCommand(() => {
                IAccount akk = new AccountDescriptor(new odmAccount() { Name = "New Account", Password = "123" });
                Accounts.Add(akk);
                SelectedAccount = akk;
            });
            ApplyClick = new DelegateCommand(() => {
                SaveToDisk();
                var eventAggregator = container.Resolve<IEventAggregator>();

                try {
					bool needToRefresh = true;
					if(initialAccount != null && SelectedAccount.Name == initialAccount.Name && SelectedAccount.Password == initialAccount.Password)
						needToRefresh = false;
					eventAggregator.GetEvent<InitAccounts>().Publish(new InitAccountEventArgs() { currentAccount = SelectedAccount, needrefresh = needToRefresh });
                    //eventAggregator.GetEvent<Refresh>().Publish(true);
                } catch (Exception err) {
                    dbg.Error(err);
                }
                if (ApplyComplete != null)
                    ApplyComplete(this, EventArgs.Empty);
            });
        }
        void SaveToDisk() {
            AccountManager.Save(Accounts.ToList());
			AccountManager.SetCurrent(SelectedAccount);
        }
        void ShowAccountDialog() {
            // eventAggregator.GetEvent<AccountManagerClick>().Publish(0);
        }

        public ICommand ApplyClick {
            get { return (ICommand)GetValue(ApplyClickProperty); }
            set { SetValue(ApplyClickProperty, value); }
        }
        public static readonly DependencyProperty ApplyClickProperty =
            DependencyProperty.Register("ApplyClick", typeof(ICommand), typeof(AccountManagerViewModel));

        public ICommand SetClick {
            get { return (ICommand)GetValue(SetClickProperty); }
            set { SetValue(SetClickProperty, value); }
        }
        public static readonly DependencyProperty SetClickProperty =
            DependencyProperty.Register("SetClick", typeof(ICommand), typeof(AccountManagerViewModel));
       
        public ICommand DeleteClick {
            get { return (ICommand)GetValue(DeleteClickProperty); }
            set { SetValue(DeleteClickProperty, value); }
        }
        public static readonly DependencyProperty DeleteClickProperty =
            DependencyProperty.Register("DeleteClick", typeof(ICommand), typeof(AccountManagerViewModel));
        
        public ICommand NewClick {
            get { return (ICommand)GetValue(NewClickProperty); }
            set { SetValue(NewClickProperty, value); }
        }
        public static readonly DependencyProperty NewClickProperty =
            DependencyProperty.Register("NewClickClick", typeof(ICommand), typeof(AccountManagerViewModel));

        #endregion

        //public string AccountName {
        //    get { return (string)GetValue(AccountNameProperty); }
        //    set { SetValue(AccountNameProperty, value); }
        //}
        //// Using a DependencyProperty as the backing store for AccountName.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty AccountNameProperty =
        //    DependencyProperty.Register("AccountName", typeof(string), typeof(AccountManagerViewModel));

        //public string AccountPassword {
        //    get { return (string)GetValue(AccountPasswordProperty); }
        //    set { SetValue(AccountPasswordProperty, value); }
        //}
        //// Using a DependencyProperty as the backing store for AccountPassword.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty AccountPasswordProperty =
        //    DependencyProperty.Register("AccountPassword", typeof(string), typeof(AccountManagerViewModel));

        public bool IsEditable {get { return (bool)GetValue(IsEditableProperty); }set { SetValue(IsEditableProperty, value); }}
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(AccountManagerViewModel), new UIPropertyMetadata(false));

        public IAccount SelectedAccount {get { return (IAccount)GetValue(SelectedAccountProperty); }set { SetValue(SelectedAccountProperty, value); }}
        public static readonly DependencyProperty SelectedAccountProperty =
            DependencyProperty.Register("SelectedAccount", typeof(IAccount), typeof(AccountManagerViewModel), new PropertyMetadata((obj, ev) => {
                var am = (AccountManagerViewModel)obj;
                if (am.SelectedAccount != null) {
                    //am.AccountName = am.SelectedAccount.Name;
                    //am.AccountPassword = am.SelectedAccount.Password;
                    am.IsEditable = true;
                } else {
                    //am.AccountName = "";
                    //am.AccountPassword = "";
                    am.IsEditable = false;
                }
                //(am.SetClick as DelegateCommand).RaiseCanExecuteChanged();
                (am.DeleteClick as DelegateCommand).RaiseCanExecuteChanged();
            }));

        public void Dispose() {
            
        }

    }
}
