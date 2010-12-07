using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Discovery;
using System.ServiceModel;
using System.Disposables;


using onvifdm.utils;
using System.Xml;


namespace nvc.onvif {
	public class DeviceManager {


		private class DeviceDescriptionImpl : DeviceDescription {
			public EndpointDiscoveryMetadata m_epMetadata;
			public AsyncSubject<Unit> m_removalSubj = new AsyncSubject<Unit>();

			public DeviceDescriptionImpl(EndpointDiscoveryMetadata epMetadata) {
				m_epMetadata = epMetadata;
			}

			public override IObservable<Unit> removal {
				get {
					return m_removalSubj as IObservable<Unit>;
				}
			}
			public override string id {
				get {
					return m_epMetadata.Address.Uri.OriginalString;
				}
			}
			public override string name {
				get {
					var name = NvcHelper.GetName(scopes);
					return name;
				}
			}
			public override string location {
				get {
					var location = NvcHelper.GetLocation(scopes);
					return location;
				}
			}
			public override IEnumerable<Uri> uris {
				get {
					return m_epMetadata.ListenUris;
				}
			}
			public override string deviceConfigId {
				get {
					return NvcHelper.GetDeviceId(scopes);
				}
			}
			public override IEnumerable<string> scopes {
				get {
					return m_epMetadata.Scopes.Select(x => x.OriginalString);
				}
			}
		}

		AnnouncementService m_announcementService = new AnnouncementService();
		ServiceHost m_host = null;
		Dictionary<string, DeviceDescriptionImpl> m_dict = new Dictionary<string, DeviceDescriptionImpl>();
		Subject<DeviceDescription> m_subj = new Subject<DeviceDescription>();
		MutableDisposable m_discoverySubscription = null;
		int m_subscriberCnt = 0;
		object m_gate = new object();

		public DeviceManager() {
			m_announcementService.OnlineAnnouncementReceived += (sender, args) => {
				var epMeta = args.EndpointDiscoveryMetadata;
				if(epMeta.ContractTypeNames.Contains(new XmlQualifiedName("NetworkVideoTransmitter", @"http://www.onvif.org/ver10/network/wsdl"))){
					ProcessDeviceDiscovery(epMeta);
				}
			};
			m_announcementService.OfflineAnnouncementReceived += (sender, args) => {
				ProcessDeviceRemoval(args.EndpointDiscoveryMetadata);
			};
		}

		private void ProcessDeviceDiscovery(EndpointDiscoveryMetadata epMeta) {
			DebugHelper.Assert(m_subscriberCnt > 0);
			DeviceDescriptionImpl devDescr = null;
			lock (m_gate) {
				string id = epMeta.Address.Uri.OriginalString;
				if (!m_dict.ContainsKey(id)) {
					devDescr = new DeviceDescriptionImpl(epMeta);
					//devDescr.id = id;
					m_dict.Add(id, devDescr);
				}
			}
			if (devDescr != null) {
				try {
					m_subj.OnNext(devDescr);
				} catch (Exception err) {
					DebugHelper.Error(err);
				}
			}
		}

		private void ProcessDeviceRemoval(EndpointDiscoveryMetadata epMeta) {
			DebugHelper.Assert(m_subscriberCnt > 0);
			DeviceDescriptionImpl devDescr = null;
			lock (m_gate) {
				string id = epMeta.Address.Uri.OriginalString;
				if (m_dict.TryGetValue(id, out devDescr)) {
					m_dict.Remove(id);
				}
			}
			if (devDescr != null) {
				try {
					devDescr.m_removalSubj.OnNext(new Unit());
				} catch (Exception err) {
					DebugHelper.Error(err);
					//swallow error;
				}
				try {
					devDescr.m_removalSubj.OnCompleted();
				} catch (Exception err) {
					DebugHelper.Error(err);
					//swallow error;
				}
			}
		}		

		public IObservable<DeviceDescription> Discover(TimeSpan duration) {
			return Observable.CreateWithDisposable<DeviceDescription>(observer => {

				var discoveryDuration = duration;

				lock (m_gate) {
					if (m_subscriberCnt++ == 0) {
						var ep = new UdpDiscoveryEndpoint(DiscoveryVersion.WSDiscoveryApril2005);
						ep.MaxResponseDelay = discoveryDuration;
						ep.TransportSettings.TimeToLive = 5; //BUGFIX: for VPN
						var disc = new WsDiscoveryObservable(ep);

						var fc = new FindCriteria();
						fc.ContractTypeNames.Add(new XmlQualifiedName("NetworkVideoTransmitter", @"http://www.onvif.org/ver10/network/wsdl"));
						fc.Duration = TimeSpan.MaxValue;
						fc.MaxResults = int.MaxValue;

						m_discoverySubscription = new MutableDisposable();

						m_discoverySubscription.Disposable = disc.Find(fc).Timeout(discoveryDuration, Observable.Empty<EndpointDiscoveryMetadata>()).Subscribe(d => {
							ProcessDeviceDiscovery(d);
						}, err => {
							DebugHelper.Error(err);
						}, () => {
							
						});

						//TODO: create async variant
						m_host = new ServiceHost(m_announcementService);
						m_host.AddServiceEndpoint(new UdpAnnouncementEndpoint(DiscoveryVersion.WSDiscoveryApril2005));
						m_host.Open();
					}

				}

				MutableDisposable subscription = new MutableDisposable();
				subscription.Disposable = m_subj.Subscribe(observer);
				return Disposable.Create(() => {
					subscription.Dispose();
					lock (m_gate) {
						if (--m_subscriberCnt == 0) {
							m_discoverySubscription.Dispose();
							m_discoverySubscription = null;
							//TODO: create async variant
							m_host.Close();
							m_host = null;
							m_dict.Clear();
						}
					}
				});
			});
		}
	}
}
