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
using System.Net;

using odm.models;
using odm.utils.extensions;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyNetworkSettings.xaml
	/// </summary>
	public partial class PropertyNetworkSettings : BasePropertyControl {
		public PropertyNetworkSettings(DeviceNetworkSettingsModel model) {
			InitializeComponent();
			this.model = model;

			Loaded += new RoutedEventHandler(PropertyNetworkSettings_Loaded);
		}

		public PropertyNetworkSettingsStrings strings {
			get {
				return PropertyNetworkSettingsStrings.Instance;
			}
		}

		DeviceNetworkSettingsModel model;
		public Action Save;
		public Action Cancel;

		void PropertyNetworkSettings_Loaded(object sender, RoutedEventArgs e) {
			BindData(model);
			InitControls();
		}
		
		OnOff on = new OnOff("On", true);
		OnOff off = new OnOff("Off", false);
		void BindData(DeviceNetworkSettingsModel devModel) {
			cbDhcp.SelectedValuePath = "Value";
			cbDhcp.DisplayMemberPath = "Name";

			cbDhcp.Items.Clear();
			cbDhcp.Items.Add(on);
			cbDhcp.Items.Add(off);
			
			tbMacAddr.CreateBinding(TextBox.TextProperty, model, m => m.mac);
			tbDns.CreateBinding(TextBox.TextProperty, model, m => m.staticDns.ToString(), (m, v)=>{m.staticDns = IPAddress.Parse(v);});
			tbIpMask.CreateBinding(TextBox.TextProperty, model, m => m.subnetMask.ToString(), (m, v) => {m.subnetMask = IPAddress.Parse(v);});
			tbGateway.CreateBinding(TextBox.TextProperty, model, m => m.staticGateway.ToString(), (m, v) => {m.staticGateway = IPAddress.Parse(v);});
			tbIpAddr.CreateBinding(TextBox.TextProperty, model, m => m.staticIp.ToString(), (m, v) => {m.staticIp = IPAddress.Parse(v);});
			cbDhcp.CreateBinding(ComboBox.SelectedItemProperty, model, m => m.dhcp?on:off,	(m, v) => {m.dhcp = v.Value;});
			
			tbDns.CreateBinding(TextBox.IsEnabledProperty, model, m=>!m.dhcp);
			tbGateway.CreateBinding(TextBox.IsEnabledProperty, model, m => !m.dhcp);
			tbIpAddr.CreateBinding(TextBox.IsEnabledProperty, model, m => !m.dhcp);
			tbIpMask.CreateBinding(TextBox.IsEnabledProperty, model, m => !m.dhcp);

			_saveCancelControl.Cancel.CreateBinding(Button.IsEnabledProperty, model, m => m.isModified);
			_saveCancelControl.Save.CreateBinding(Button.IsEnabledProperty, model, m => m.isModified);

		}

		void Localization() {
			title.CreateBinding(Title.ContentProperty, strings, x => x.title);
			lbDhcp.CreateBinding(Label.ContentProperty, strings, x => x.dhcp);
			lbDns.CreateBinding(Label.ContentProperty, strings, x => x.dnsAddr);
			lbGateway.CreateBinding(Label.ContentProperty, strings, x => x.gateAddr);
			lbIpAddr.CreateBinding(Label.ContentProperty, strings, x => x.ipAddr);
			lbMacAddr.CreateBinding(Label.ContentProperty, strings, x => x.macAddr);
			lbIpMask.CreateBinding(Label.ContentProperty, strings, x => x.subnetMask);
		}

		void InitControls() {
			Localization();

			_saveCancelControl.Cancel.Click += new RoutedEventHandler(Cancel_Click);
			_saveCancelControl.Save.Click += new RoutedEventHandler(Save_Click);
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			Save();
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			Cancel();
		}
	}

	public class OnOff {
		public OnOff(string name, bool val) {
			Name = name;
			Value = val;
		}
		public string Name { get; set; }
		public bool Value { get; set; }
	}
}
