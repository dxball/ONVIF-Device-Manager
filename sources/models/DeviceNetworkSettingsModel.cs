using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using nvc.onvif;
using nvc.utils;
using nvc.entities;
using dev=onvif.services.device;

namespace nvc.models {
	public partial class DeviceNetworkSettingsModel : ModelBase<DeviceNetworkSettingsModel> {
		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<DeviceNetworkSettingsModel> observer) {
			NetworkSettings netSettings = null;
			NetworkStatus netstat = null;
			yield return Observable.Merge(
				session.GetNetworkSettings().Handle(x => netSettings = x),
				session.GetNetworkStatus().Handle(x => netstat = x)
			);
			DebugHelper.Assert(netSettings != null);
			DebugHelper.Assert(netstat != null);

			m_dhcp.SetBoth(netSettings.dhcp);
			m_staticIp.SetBoth(netSettings.staticIp);
			m_subnetMask.SetBoth(NetMaskHelper.PrefixToMask(netSettings.subnetPrefix));
			m_staticGateway.SetBoth(netSettings.defaultGateway);
			m_staticDns.SetBoth(netSettings.staticDns);
			
			NotifyPropertyChanged(x=>x.staticIp);
			NotifyPropertyChanged(x=>x.subnetMask);
			NotifyPropertyChanged(x=>x.staticGateway);
			NotifyPropertyChanged(x=>x.staticDns);

			mac = BitConverter.ToString(netstat.mac.GetAddressBytes());	

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		protected override IEnumerable<IObservable<Object>> ApplyChangesImpl(Session session, IObserver<DeviceNetworkSettingsModel> observer) {
			if (!isModified) {
				yield break;
			}

			//var proxy = session.device;
			//var id = session.deviceDescription.Id;

			var dhcp_enabled = dhcp;

			var dns_addresses = new dev::IPAddress[] { 
			    new dev::IPAddress(){ 
			        Type = dev::IPType.IPv4, 
			        IPv4Address = staticDns.ToString()
			    } 
			};

			var gateway_addresses = new string[]{
			    staticGateway.ToString()
			};

			dev::NetworkInterface[] nics = null;
			//yield return Observable.Merge(
			bool success = true;
				yield return session.SetDNS(dhcp_enabled, dns_addresses)
					.OnError(err => {
						DebugHelper.Error(err);
						success = false;
					})
					.Idle().IgnoreError();
				//TODO: synesis 6407 start rebooting here
				//if (!success) {
					
				//}
				
				success = true;
				yield return session.SetNetworkDefaultGateway(gateway_addresses, null)
				.OnError(err => {
					DebugHelper.Error(err);
					success = false;
				})
				.Idle().IgnoreError();
				
				if (!success) {
					//TODO: work around for axis p3301
					yield return Observable.Timer(TimeSpan.FromSeconds(6)).Idle();
				}

				yield return session.GetNetworkInterfaces().Handle(x => nics = x);
			//);
			DebugHelper.Assert(nics != null);
			
			var nic = nics.Where(x => x.Enabled).First();


			var nic_set = new dev::NetworkInterfaceSetConfiguration();

			nic_set.Enabled = true;
			nic_set.EnabledSpecified = true;
			if (nic.Info != null) {
				nic_set.MTUSpecified = nic.Info.MTUSpecified;
				nic_set.MTU = nic.Info.MTU;
			} else {
				nic_set.MTUSpecified = false;
			}
			
			nic_set.IPv4 = new dev.IPv4NetworkInterfaceSetConfiguration();
			nic_set.IPv4.DHCP = dhcp;
			nic_set.IPv4.DHCPSpecified = true;
			nic_set.IPv4.Enabled = true;
			nic_set.IPv4.EnabledSpecified = true;
			nic_set.IPv4.Manual = new dev::PrefixedIPv4Address[]{
			    new dev::PrefixedIPv4Address(){
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
			yield return session.SetNetworkInterfaces(nic.token, nic_set).Handle(x=>isRebootNeeded = x);

			if (isRebootNeeded) {
				//yield return session.SystemReboot().Idle();				
			}
			
			//    if (isRebootNeeded) {
			//        var message = proxy.SystemReboot().First();
			//        //DebugHelper.Break();
			//        return null;
			//    }
			//    var resolver = new DeviceDiscovery() {
			//        Duration = TimeSpan.FromSeconds(5)
			//    }.Resolve(id);
			//    try {
			//        var devDescr = resolver.First();
			//        return devDescr;
			//    } catch {
			//        return null;
			//    }
			
			if (observer != null) {
				observer.OnNext(this);
			}
		}

		private ChangeTrackingProperty<bool> m_dhcp = new ChangeTrackingProperty<bool>();
		private ChangeTrackingProperty<IPAddress> m_staticIp = new ChangeTrackingProperty<IPAddress>();
		private ChangeTrackingProperty<IPAddress> m_subnetMask = new ChangeTrackingProperty<IPAddress>();
		private ChangeTrackingProperty<IPAddress> m_staticGateway = new ChangeTrackingProperty<IPAddress>();
		private ChangeTrackingProperty<IPAddress> m_staticDns = new ChangeTrackingProperty<IPAddress>();

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
		public IPAddress staticIp {
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
		public IPAddress subnetMask {
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
		public IPAddress staticGateway {
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
		public IPAddress staticDns {
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
