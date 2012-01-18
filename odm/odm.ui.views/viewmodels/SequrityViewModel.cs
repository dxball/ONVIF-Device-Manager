using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using odm.infra;
using odm.models;
using onvif.utils;
using utils;

namespace odm.ui.viewModels {
    public class SequrityViewModel : ViewModelDeviceBase {
        public SequrityViewModel(IUnityContainer container)
            : base(container) {
                Certificates = new ObservableCollection<CertificateDescriptor>();
        }

        //public SequrityStrings Strings { get { return SequrityStrings.instance; } }
        public ObservableCollection<CertificateDescriptor> Certificates { get; set; }
        OdmSession facade;
        IChangeTrackable<ICertificateManagementModel> model;

        public override void Load(odm.core.INvtSession session, core.IAccount account) {
            Current = States.Loading;
            CurrentSession = session;
            facade = new OdmSession(session);

            subscription.Add(facade.GetCertificateSettings(() => new CertificateManagementModel())
                .ObserveOnCurrentDispatcher()
                .Subscribe(model => {
                    Current = States.Common;
                    this.model = model;
                    BindModel(model);
                }, err => {
					dbg.Error(err);
					ErrorMessage = err.Message;
                    Current = States.Error;
                }));
        }

        void BindModel(IChangeTrackable<ICertificateManagementModel> model) {
            this.CreateBinding(EnableClientAuthentificationProperty, model.current, x => {
                return x.clientAuthentication;
            }, (m, v) => {
                m.clientAuthentication = v;
            });
            model.current.serverCertificates.ForEach(x=>Certificates.Add(new CertificateDescriptor(){Name = x}));
            Certificates.ForEach(x => {
                if (x.Name == model.current.activeCertificateId) {
                    x.IsEnabled = true;
                    ActiveCertificate = x;
                }
            });
        }
        
        void UploadCertificate() {
            var dlg = new OpenFileDialog();
            //dlg.Filter = "Backup files|*.backup";
            //dlg.Title = "Open backup file";
			var a = dlg.ShowDialog();
            if (dlg.ShowDialog() == true){
                try{
                } catch (Exception error) {
                    dbg.Error(error);
                }
            }
        }
        void Apply() {
            if (facade == null) {
                dbg.Error("facade = null");
                return;
            }
            if (ActiveCertificate != null) {
                model.current.activeCertificateId = ActiveCertificate.Name;
            }
            Current = States.Loading;
            subscription.Add(facade.SetCertificateSettings(model)
                .ObserveOnCurrentDispatcher()
                .Subscribe(unit => {
                    Current = States.Common;
                },err=>{
                    ErrorMessage = err.Message;
                    Current = States.Error;
                }));
        }
        void Activate() {
            ActiveCertificate = SelectedCertificate;
            Certificates.ForEach(x=>x.IsEnabled = false);
            ActiveCertificate.IsEnabled = true;

			OnActivate.RaiseCanExecuteChanged();
			OnDeActivate.RaiseCanExecuteChanged();
        }

		void DeActivate() {
			SelectedCertificate.IsEnabled = false;
			OnActivate.RaiseCanExecuteChanged();
			OnDeActivate.RaiseCanExecuteChanged();
		}

        #region Commands
        public override void InitCommands() {
            base.InitCommands();

            OnApply = new DelegateCommand(() => {
                Apply();
            }, () => {
                return true;
            });
			OnDeActivate = new DelegateCommand(()=>{
				DeActivate();
			}, () => {
				if (SelectedCertificate == null)
					return false;
				return SelectedCertificate.IsEnabled;
			});
            OnActivate = new DelegateCommand(() => {
                Activate();
            }, () => {
                if (SelectedCertificate == null)
                    return false;
                return !SelectedCertificate.IsEnabled;
            });

            OnUploadCertificate = new DelegateCommand(() => {
                UploadCertificate();
            });
        }

        public DelegateCommand OnApply {
			get { return (DelegateCommand)GetValue(OnApplyProperty); }
            set { SetValue(OnApplyProperty, value); }
        }
        // Using a DependencyProperty as the backing store for OnApply.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnApplyProperty =
			DependencyProperty.Register("OnApply", typeof(DelegateCommand), typeof(SequrityViewModel));

        
        public ICommand OnUploadCertificate {
            get { return (ICommand)GetValue(OnUploadCertificateProperty); }
            set { SetValue(OnUploadCertificateProperty, value); }
        }
        // Using a DependencyProperty as the backing store for OnUploadCertificate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnUploadCertificateProperty =
            DependencyProperty.Register("OnUploadCertificate", typeof(ICommand), typeof(SequrityViewModel));

		public DelegateCommand OnDeActivate {
			get { return (DelegateCommand)GetValue(OnDeActivateProperty); }
			set { SetValue(OnDeActivateProperty, value); }
		}
		public static readonly DependencyProperty OnDeActivateProperty =
			DependencyProperty.Register("OnDeActivate", typeof(DelegateCommand), typeof(SequrityViewModel));


		public DelegateCommand OnActivate {
			get { return (DelegateCommand)GetValue(OnActivateProperty); }
            set { SetValue(OnActivateProperty, value); }
        }
        // Using a DependencyProperty as the backing store for OnActivate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnActivateProperty =
			DependencyProperty.Register("OnActivate", typeof(DelegateCommand), typeof(SequrityViewModel));
        #endregion Commands

        #region Properties

        public CertificateDescriptor ActiveCertificate {
            get { return (CertificateDescriptor)GetValue(ActiveCertificateProperty); }
            set { SetValue(ActiveCertificateProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ActiveCertificate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveCertificateProperty =
            DependencyProperty.Register("ActiveCertificate", typeof(CertificateDescriptor), typeof(SequrityViewModel));

        public CertificateDescriptor SelectedCertificate {
            get { return (CertificateDescriptor)GetValue(SelectedCertificateProperty); }
            set { SetValue(SelectedCertificateProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SelectedCertificate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCertificateProperty =
            DependencyProperty.Register("SelectedCertificate", typeof(CertificateDescriptor), typeof(SequrityViewModel), new UIPropertyMetadata(null, (obj, evargs) => {
                var vm = (SequrityViewModel)obj;
                (vm.OnActivate as DelegateCommand).RaiseCanExecuteChanged();
				(vm.OnDeActivate as DelegateCommand).RaiseCanExecuteChanged();
            }));

        public bool EnableClientAuthentification {
            get { return (bool)GetValue(EnableClientAuthentificationProperty); }
            set { SetValue(EnableClientAuthentificationProperty, value); }
        }
        // Using a DependencyProperty as the backing store for EnableClientAuthentification.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableClientAuthentificationProperty =
            DependencyProperty.Register("EnableClientAuthentification", typeof(bool), typeof(SequrityViewModel));
        
        #endregion
    }

    public class CertificateDescriptor:DependencyObject {

        public bool IsEnabled {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(CertificateDescriptor));

        public string Name {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(CertificateDescriptor));
    }
}
