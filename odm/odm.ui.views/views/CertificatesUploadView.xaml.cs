using System;
using System.Reactive.Disposables;
using System.Windows.Controls;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Unity;
using odm.infra;

namespace odm.ui.activities {
	/// <summary>
	/// Interaction logic for CertificatesUploadView.xaml
	/// </summary>
	public partial class CertificatesUploadView : UserControl, IDisposable {
		
		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new CertificatesUploadView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion
		
		private CompositeDisposable disposables = new CompositeDisposable();

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			InitializeComponent();
		}

		public void Dispose() {
			Cancel();
		}
	}
}
