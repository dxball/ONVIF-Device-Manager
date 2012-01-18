using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using odm.infra;
using onvif.services;
using Org.BouncyCastle.X509;
using utils;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for CertificatesView.xaml
	/// </summary>
	public partial class CertificatesView : UserControl, IDisposable, INotifyPropertyChanged {
		
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new CertificatesView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion
		private CompositeDisposable disposables = new CompositeDisposable();

		public ObservableCollection<CertificateDescr> certificates { get; set; }

		public LocalTitles Titles { get { return LocalTitles.instance; } }
		public LocalButtons ButtonsLocales { get { return LocalButtons.instance; } }
		public LocalSequrity Strings { get { return LocalSequrity.instance; } }

		public ICommand UploadCmd { get; private set; }

		Model model;

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			certificates = new ObservableCollection<CertificateDescr>();
			
			SetStatusCommand = new DelegateCommand(
				() => {
					List<string> certifIdlst = new List<string>();
					var cert = certificates.ToList().Where(x => x.isEnabled == true);
					cert.ForEach(crt=>{
						certifIdlst.Add(crt.CertificateId);
					});
					Success(new Result.SetStatus(certifIdlst.ToArray()));
				},
				() => true
			);

			UploadCmd = new DelegateCommand(
				() => {
					var dlg = new OpenFileDialog();
					dlg.Title = Strings.selectCertificateHeader;
					//dlg.InitialDirectory = Directory.GetCurrentDirectory();
					if (dlg.ShowDialog() == true) {
						Success(new Result.Upload(dlg.FileName));
					}
				},
				() => true
			);

			InitializeComponent();

			this.model = model;

			BindData(model);
			Localization();
		}

		CertificateDescr selectedCertificate;
		public CertificateDescr SelectedCertificate {
			get {
				return selectedCertificate;
			}
			set {
				selectedCertificate = value;
				NotifyPropertyChanged("SelectedCertificate");
			}
		}

		public class CertificateDescr {
			public CertificateDescr(Certificate cert, bool isEnabled){
				this.cert = cert;
				this.isenabled = isEnabled;
			}
			Certificate cert;
			
			public string ContentType {
				get {
					return cert.Certificate1.contentType;
				}
			}
			public override string ToString() {
				var certParser = new X509CertificateParser();
				var x509 = certParser.ReadCertificate(cert.Certificate1.Data);
				return x509.ToString();
			}
			bool isenabled;
			public bool isEnabled {
				get {
					return isenabled;
				}
				set {
					isenabled = value;
				}
			}
			public string CertificateId { get { return cert.CertificateID; } }
		}
		void Localization() {
			uploadCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.uploadCertificate);
			btnUpload.CreateBinding(Button.ContentProperty, Strings, s => s.btnUpload);
			setStatus.CreateBinding(Button.ContentProperty, Strings, s=>s.setStatus);
		}
		void BindData(Model model) {
			model.certificates.ForEach(cert => {
				bool isEnabled = false;
				if (model.enabled.Contains(cert.CertificateID)) {
					isEnabled = true;
				}
				CertificateDescr cdescr = new CertificateDescr(cert, isEnabled);
				certificates.Add(cdescr);
			});

			
			certificateslist.SelectionChanged+=new SelectionChangedEventHandler((obj, value)=>{
				if (certificateslist.SelectedItem != null) {
					detailsValue.Text = certificateslist.SelectedItem.ToString();
				}
			});
			certificateslist.CreateBinding(ListBox.ItemsSourceProperty, this, x => x.certificates);
		}


		public void Dispose() {
			Cancel();
		}

		private void NotifyPropertyChanged(String info) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
