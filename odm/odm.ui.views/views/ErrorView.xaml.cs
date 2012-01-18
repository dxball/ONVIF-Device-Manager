using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Controls;
using System.Xml;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using utils;

namespace odm.ui.activities {
	public partial class ErrorView : UserControl {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Exception error) {
			var model = new Model(error);
			return container.StartViewActivity<Result>(context => {
				var view = new ErrorView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public Model model;

		public ErrorView(Exception error){
			Init(new Model(error));
		}

		private void Init(Model model) {
			this.model = model;
			this.DataContext = model;

			InitializeComponent();

			Localization();

			okButton.Command = new DelegateCommand(
				() => Success(new Result.Ok()),
				() => true
			);

			var err = CorrectError(model.error);
			message.Text = err.Message;
		}

		void Localization() {
			detailsButton.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.details);
			okButton.CreateBinding(Button.ContentProperty, LocalButtons.instance, s => s.close);
		}

		protected Exception CorrectError(Exception error){
			var protoException = error as ProtocolException;
			if (protoException == null) {
				return error;
			}
			var webException = error.InnerException as WebException;
			if (webException == null) {
				return error;
			}
			var response = webException.Response as HttpWebResponse;
			if (response == null) {
				return error;
			}
			try{
				using (var stream = response.GetResponseStream()) {
					using(var xmlReader = XmlReader.Create(stream)){
						var msg = Message.CreateMessage(xmlReader, int.MaxValue, MessageVersion.Soap12WSAddressing10);
						if (msg.IsFault) {
							var fault = MessageFault.CreateFault(msg, Int16.MaxValue);
							return new FaultException(fault);
						}
					}
				}
			}catch{
				return error;
			}
			
			return error;
		}

		public void Dispose() {
			Cancel();
		}
	}
}
