using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using odm.onvif;
using odm.utils;

using onvif.services.device;
using onvif.types;

using dev = onvif.services.device;
using tt = onvif.types;
using net = System.Net;
using System.Net.NetworkInformation;

namespace odm.models {
	public partial class DeviceNetworkSettingsModel : ModelBase<DeviceNetworkSettingsModel> {
		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<DeviceNetworkSettingsModel> observer) {
			NetworkSettings netSettings = new NetworkSettings();
			//PhysicalAddress mac = null;
			NetworkStatus netstat = new NetworkStatus();
			//yield return Observable.Merge(
			//    session.GetNetworkSettings().Handle(x => netSettings = x),
			//    session.GetNetworkStatus().Handle(x => netstat = x)
			//);

			NetworkGateway gateway = null;
			DNSInformation dns = null;
			tt::NetworkInterface[] nics = null;

			yield return Observable.Merge(
				session.GetNetworkDefaultGateway().Handle(x => gateway = x).IgnoreError(),
				session.GetDNS().Handle(x => dns = x).IgnoreError(),
				session.GetNetworkInterfaces().Handle(x => nics = x)
			);

			dbg.Assert(gateway != null);
			dbg.Assert(dns != null);
			dbg.Assert(nics != null);
						
			if (gateway != null && gateway.IPv4Address != null && gateway.IPv4Address.Count() > 0) {
				net::IPAddress defaultGateway = net::IPAddress.None;
				net::IPAddress.TryParse(gateway.IPv4Address[0], out defaultGateway);
				netSettings.defaultGateway = defaultGateway;
			}

			if (dns != null && dns.DNSManual != null && dns.DNSManual.Count() > 0 && !String.IsNullOrWhiteSpace(dns.DNSManual[0].IPv4Address)) {
				netSettings.staticDns = net::IPAddress.Parse(dns.DNSManual[0].IPv4Address);
			} else if (dns != null && dns.DNSFromDHCP != null && dns.DNSFromDHCP.Count() > 0) {
				netSettings.staticDns = net::IPAddress.Parse(dns.DNSFromDHCP[0].IPv4Address);
			}

			var nic = nics.Where(x => x.Enabled).FirstOrDefault();
			if (nic != null) {
				var nic_cfg = nic.IPv4.Config;
				netstat.mac = PhysicalAddress.Parse(nic.Info.HwAddress.Replace(':', '-'));
				netSettings.dhcp = nic.IPv4.Config.DHCP;

				if (nic_cfg.Manual.Count() > 0) {
					netSettings.staticIp = net::IPAddress.Parse(nic_cfg.Manual[0].Address);
					netSettings.subnetPrefix = nic_cfg.Manual[0].PrefixLength;
				} else if (nic_cfg.FromDHCP != null) {
					netSettings.staticIp = net::IPAddress.Parse(nic_cfg.FromDHCP.Address);
					netSettings.subnetPrefix = nic_cfg.FromDHCP.PrefixLength;
				}
			}
			
			m_dhcp.SetBoth(netSettings.dhcp);
			m_staticIp.SetBoth(netSettings.staticIp ?? new net::IPAddress(0));
			m_subnetMask.SetBoth(NetMaskHelper.PrefixToMask(netSettings.subnetPrefix) ?? new net::IPAddress(0));
			m_staticGateway.SetBoth(netSettings.defaultGateway ?? new net::IPAddress(0));
			m_staticDns.SetBoth(netSettings.staticDns ?? new net::IPAddress(0));
			mac = BitConverter.ToString(netstat.mac.GetAddressBytes());

			NotifyPropertyChanged(x => x.staticIp);
			NotifyPropertyChanged(x => x.subnetMask);
			NotifyPropertyChanged(x => x.staticGateway);
			NotifyPropertyChanged(x => x.staticDns);
			
			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<DeviceNetworkSettingsModel> observer) {
			if (!isModified) {
				observer.OnNext(this);
				yield break;
			}

			var dhcp_enabled = dhcp;
				
			if (m_staticDns.isModified || m_dhcp.isModified) {
				var dns_addresses = new tt::IPAddress[] { 
					new tt::IPAddress(){ 
						Type = tt::IPType.IPv4, 
						IPv4Address = staticDns.ToString()
					}
				};
				Exception error = null;
				yield return session.SetDNS(dhcp_enabled, dns_addresses)
					.Idle()
					.HandleError(err => {
						dbg.Error(err);
						error = err;
					});
			}

			if (m_staticGateway.isModified) {
				var gateway_addresses = new string[]{
					staticGateway.ToString()
				};
				Exception error = null;
				yield return session.SetNetworkDefaultGateway(gateway_addresses, null)
					.Idle()
					.HandleError(err=>{
						dbg.Error(err);
						error = err;
					});
				

				if (error!=null) {
					//TODO: workaround for axis p3301
					yield return Observable.Timer(TimeSpan.FromSeconds(6)).Idle();
				}
			}

			if (m_staticIp.isModified || m_subnetMask.isModified || m_dhcp.isModified) {
				tt::NetworkInterface[] nics = null;
				yield return session.GetNetworkInterfaces().Handle(x => nics = x);
				dbg.Assert(nics != null);

				var nic = nics.Where(x => x.Enabled).First();
				
				var nic_set = new tt::NetworkInterfaceSetConfiguration();

				nic_set.Enabled = true;
				nic_set.EnabledSpecified = true;
				if (nic.Info != null) {
					nic_set.MTUSpecified = nic.Info.MTUSpecified;
					nic_set.MTU = nic.Info.MTU;
				} else {
					nic_set.MTUSpecified = false;
				}

				nic_set.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
				nic_set.IPv4.DHCP = dhcp;
				nic_set.IPv4.DHCPSpecified = true;
				nic_set.IPv4.Enabled = true;
				nic_set.IPv4.EnabledSpecified = true;
				nic_set.IPv4.Manual = new tt::PrefixedIPv4Address[]{
					new PrefixedIPv4Address(){
						Address = staticIp.ToString(),					
						PrefixLength = NetMaskHelper.MaskToPrefix(subnetMask.ToString())
					}
				};

				if (nic.Link != null) {
					//nic_set_cfg.Link = new NetworkInterfaceConnectionSetting();
					if (nic.Link.AdminSettings != null) {
						nic_set.Link = nic.Link.AdminSettings;
					} else if (nic.Link.OperSettings != null) {
						nic_set.Link = nic.Link.OperSettings;
					}
				}

				bool isRebootNeeded = false;
				yield return session.SetNetworkInterfaces(nic.token, nic_set).Handle(x => isRebootNeeded = x);

				if (isRebootNeeded) {
					//yield return session.SystemReboot().Idle();					
				}

			}			
		}

		private ChangeTrackingProperty<bool> m_dhcp = new ChangeTrackingProperty<bool>(false);
		private ChangeTrackingProperty<net::IPAddress> m_staticIp = new ChangeTrackingProperty<net::IPAddress>(new net::IPAddress(0));
		private ChangeTrackingProperty<net::IPAddress> m_subnetMask = new ChangeTrackingProperty<net::IPAddress>(new net::IPAddress(0));
		private ChangeTrackingProperty<net::IPAddress> m_staticGateway = new ChangeTrackingProperty<net::IPAddress>(new net::IPAddress(0));
		private ChangeTrackingProperty<net::IPAddress> m_staticDns = new ChangeTrackingProperty<net::IPAddress>(new net::IPAddress(0));

		private string m_mac;

		public override void RevertChanges() {
			m_dhcp.Revert();
			m_staticIp.Revert();
			m_subnetMask.Revert();
			m_staticGateway.Revert();
			m_staticDns.Revert();

			NotifyPropertyChanged(x => x.staticIp);
			NotifyPropertyChanged(x => x.subnetMask);
			NotifyPropertyChanged(x => x.staticGateway);
			NotifyPropertyChanged(x => x.staticDns);
		}

		public bool dhcp {
			get {
				return m_dhcp.current;
			}
			set {
				if (m_dhcp.current != value) {
					m_dhcp.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.dhcp);
				}
			}
		}
		public net::IPAddress staticIp {
			get {
				return m_staticIp.current;
			}
			set {
				if (m_staticIp.current != value) {
					m_staticIp.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.staticIp);
				}
			}
		}
		public net::IPAddress subnetMask {
			get {
				return m_subnetMask.current;
			}
			set {
				if (m_subnetMask.current != value) {
					m_subnetMask.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.subnetMask);
				}
			}
		}
		public net::IPAddress staticGateway {
			get {
				return m_staticGateway.current;
			}
			set {
				if (m_staticGateway.current != value) {
					m_staticGateway.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.staticGateway);
				}
			}
		}
		public net::IPAddress staticDns {
			get {
				return m_staticDns.current;
			}
			set {
				if (m_staticDns.current != value) {
					m_staticDns.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.staticDns);
				}
			}
		}
		public string mac {
			get {
				return m_mac;
			}
			private set {
				if (m_mac != value) {
					m_mac = value;
					NotifyPropertyChanged(x => x.mac);
				}
			}
		}
	}
}
