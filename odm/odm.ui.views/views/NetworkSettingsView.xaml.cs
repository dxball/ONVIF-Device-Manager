using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.infra;
using odm.ui.controls;
using onvif.services;
using utils;

namespace odm.ui.activities {

	public partial class NetworkSettingsView : UserControl, IDisposable {

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new NetworkSettingsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		public ICommand CancelCommand { get; private set; }

		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
		public LocalButtons ButtonsLocales { get { return LocalButtons.instance; } }
		public LocalNetworkSettings Strings { get { return LocalNetworkSettings.instance; } }

		Model model;
		private void Init(Model model) {
			this.DataContext = model;
			this.model = model;

			var applyCmd = new DelegateCommand(
				() => {
					if (!model.dhcp) {
						model.useNtpFromDhcp = false;
						model.useDnsFromDhcp = false;
					}
					GetProtocolData();
					Success(new Result.Apply(model));
				},
				() => true
			);
			ApplyCommand = applyCmd;

			var cancelCmd = new DelegateCommand(
				() => model.RevertChanges(),
				() => true
			);
			CancelCommand = cancelCmd;

			InitializeComponent();

			Localization();
			BindData(model);
		}

		void Localization() {
			dhcpCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.dhcp);
			dnsCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.dns);
			dnsFromDhcpCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.dnsFromDhcp);
			gatewayCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.gateway);
			ipAddressCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.ipAddress);
			ipMaskCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.ipMask);
			ntpCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.ntp);
			ntpFromDhcpCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.ntpFromDhcp);
			applyButton.CreateBinding(Button.ContentProperty, ButtonsLocales, x => x.apply);
			cancelButton.CreateBinding(Button.ContentProperty, ButtonsLocales, x => x.cancel);
			zeroCaption.CreateBinding(TextBlock.TextProperty, Strings, x => x.zeroCaption);
			captionPortsHttp.CreateBinding(TextBlock.TextProperty, Strings, s => s.ports);
			captionPortsHttps.CreateBinding(TextBlock.TextProperty, Strings, s => s.ports);
			captionPortsRtsp.CreateBinding(TextBlock.TextProperty, Strings, s => s.ports);
		}
		
		void FillProtocolsData(Model model) {
			valueEnableHttp.IsChecked = false;
			valueEnableHttps.IsChecked = false;
			valueEnableRtsp.IsChecked = false;

			model.netProtocols.ForEach(prot => {
				if (prot.name == NetworkProtocolType.HTTP.ToString()) {
					if (prot.enabled) {
						if (valueEnableHttp.IsChecked.Value) {
							valuePortsHttp.Text +=";" + prot.ports ?? "";
						} else {
							valueEnableHttp.IsChecked = true;
							valuePortsHttp.Text = prot.ports ?? "";
						}
					} else if (!valueEnableHttp.IsChecked.Value) {
						valuePortsHttp.Text += ";" + prot.ports ?? "";
					}

					valuePortsHttp.Text.TrimStart(';');
				} else if (prot.name == NetworkProtocolType.HTTPS.ToString()) {
					if (prot.enabled) {
						if (valueEnableHttps.IsChecked.Value) {
							valuePortsHttps.Text += ";" + prot.ports ?? "";
						} else {
							valueEnableHttps.IsChecked = true;
							valuePortsHttps.Text = prot.ports ?? "";
						}
					} else if (!valueEnableHttps.IsChecked.Value) {
						valuePortsHttps.Text += ";" + prot.ports ?? "";
					}

					valuePortsHttps.Text.TrimStart(';');
				} else if (prot.name == NetworkProtocolType.RTSP.ToString()) {
					if (prot.enabled) {
						if (valueEnableRtsp.IsChecked.Value) {
							valuePortsRtsp.Text += ";" + prot.ports ?? "";
						} else {
							valueEnableRtsp.IsChecked = true;
							valuePortsRtsp.Text = prot.ports ?? "";
						}
					} else if (!valueEnableRtsp.IsChecked.Value) {
						valuePortsRtsp.Text += ";" + prot.ports ?? "";
					}

					valuePortsRtsp.Text.TrimStart(';');
				}
			});
		}
		void GetProtocolData() {
			NetworkProtocol httpProt = new NetworkProtocol(NetworkProtocolType.HTTP.ToString(), valuePortsHttp.Text, valueEnableHttp.IsChecked.Value);
			NetworkProtocol httpsProt = new NetworkProtocol(NetworkProtocolType.HTTPS.ToString(), valuePortsHttps.Text, valueEnableHttps.IsChecked.Value);
			NetworkProtocol rtspProt = new NetworkProtocol(NetworkProtocolType.RTSP.ToString(), valuePortsRtsp.Text, valueEnableRtsp.IsChecked.Value);
			model.netProtocols = new NetworkProtocol[] { httpProt, httpsProt, rtspProt};
		}
		void BindData(Model model) {
			if (!model.zeroConfSupported) {
				zeroValue.IsEnabled = false;
				zeroIp.Text = "Not supported";
			} else {
				zeroValue.CreateBinding(CheckBox.IsCheckedProperty, model,
					x => x.zeroConfEnabled,
					(m, v) => {
						m.zeroConfEnabled = v;
					});
				zeroIp.Text = model.zeroConfIp;
			}

			FillProtocolsData(model);

			dhcpValue.CreateBinding(ComboBox.SelectedValueProperty, model, m => m.dhcp, (m, v) => m.dhcp = v);

			ipAddressValue.CreateBinding(TextBox.IsReadOnlyProperty, model, x => x.dhcp);
			ipAddressValue.CreateBinding(TextBox.TextProperty, model, x => x.ip, (m, v) => { m.ip = v; });

			ipMaskValue.CreateBinding(TextBox.IsReadOnlyProperty, model, x => x.dhcp);
			ipMaskValue.CreateBinding(TextBox.TextProperty, model, x => x.subnet, (m, v) => { m.subnet = v; });

			gatewayValue.CreateBinding(TextBox.IsReadOnlyProperty, model, x => x.dhcp);
			gatewayValue.CreateBinding(TextBox.TextProperty, model, x => x.gateway, (m, v) => { m.gateway = v; });

			hostValue.CreateBinding(TextBox.IsReadOnlyProperty, model, m=>m.useHostFromDhcp);
			hostValue.CreateBinding(TextBox.TextProperty, model, m => m.host, (m, v) => { m.host = v; });
			hostFromDhcpValue.CreateBinding(CheckBox.IsCheckedProperty, model, m => m.useHostFromDhcp, (m, v) => { m.useHostFromDhcp = v; });

			ntpValue.CreateBinding(TextBox.TextProperty, model, x => x.ntpServers, (m, v) => { m.ntpServers = v; });

			dnsValue.CreateBinding(TextBox.TextProperty, model, x => x.dns, 
				(m, v) => { 
					m.dns = v; 
				});

			dnsFromDhcpValue.CreateBinding(CheckBox.IsEnabledProperty, model, x => x.dhcp);
			dnsFromDhcpValue.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.useDnsFromDhcp, (m, v) => { m.useDnsFromDhcp = v; });

			ntpFromDhcpValue.CreateBinding(CheckBox.IsEnabledProperty, model, x => x.dhcp);
			ntpFromDhcpValue.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.useNtpFromDhcp, (m, v) => { m.useNtpFromDhcp = v; });
		}

		public void Dispose() {
			Cancel();
		}
	}
}
