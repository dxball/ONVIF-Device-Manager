using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Discovery;
using System.ServiceModel;
using System.Disposables;


using odm.utils;
using System.Xml;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;


namespace odm.onvif {

	public class DeviceManager {
		//public class MsgInterceptor : IClientMessageInspector, IDispatchMessageInspector {
		//    public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState) {
		//        return;
		//    }

		//    public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel) {
		//        return null;
		//    }

		//    public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel, InstanceContext instanceContext) {
		//        return null;
		//    }

		//    public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState) {
		//        return;
		//    }
		//}

		//public class EpBeh : IEndpointBehavior {
		//    public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) {
		//        return;
		//    }

		//    public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime) {
		//        clientRuntime.MessageInspectors.Add(new MsgInterceptor());
		//    }

		//    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher) {
		//        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new MsgInterceptor());
		//    }

		//    public void Validate(ServiceEndpoint endpoint) {
		//        return;
		//    }
		//}

		//public class MyAnnouncementService: AnnouncementService{
		//    protected override IAsyncResult OnBeginOnlineAnnouncement(DiscoveryMessageSequence messageSequence, EndpointDiscoveryMetadata endpointDiscoveryMetadata, AsyncCallback callback, object state) {
		//        return base.OnBeginOnlineAnnouncement(messageSequence, endpointDiscoveryMetadata, callback, state);
		//    }

		//    protected override void OnEndOnlineAnnouncement(IAsyncResult result) {
		//        base.OnEndOnlineAnnouncement(result);
		//    }
		//}

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

		AnnouncementService m_announcementService = null;
		ServiceHost m_host = null;
		Dictionary<string, DeviceDescriptionImpl> m_dict = new Dictionary<string, DeviceDescriptionImpl>();
		Subject<DeviceDescription> m_subj = new Subject<DeviceDescription>();
		MutableDisposable m_discoverySubscription = null;
		int m_subscriberCnt = 0;
		object m_gate = new object();

		public DeviceManager() {
			m_announcementService = new AnnouncementService();
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
			dbg.Assert(m_subscriberCnt > 0);
			DeviceDescriptionImpl devDescr = null;
			IEnumerable<DeviceDescriptionImpl> removedDevices = null;
			lock (m_gate) {
				string id = epMeta.Address.Uri.OriginalString;
				if (!m_dict.ContainsKey(id)) {
					
					var devicesToRemove = m_dict.Where(pair => {
						return pair.Value.uris.Intersect(epMeta.ListenUris).Any();
					}).ToArray();

					devicesToRemove.ForEach(x => {
						m_dict.Remove(x.Key);
					});

					removedDevices = devicesToRemove.Select(x => x.Value);

					devDescr = new DeviceDescriptionImpl(epMeta);
					//devDescr.id = id;
					m_dict.Add(id, devDescr);
				}
			}

			if (removedDevices != null) {
				removedDevices.ForEach(x => {
					NotifyDeviceRemoval(x);
				});
			}
			if (devDescr != null) {
				try {
					m_subj.OnNext(devDescr);
				} catch (Exception err) {
					dbg.Error(err);
				}
			}
		}

		private void NotifyDeviceRemoval(DeviceDescriptionImpl devDescr) {
			dbg.Assert(devDescr != null);

			try {
				devDescr.m_removalSubj.OnNext(new Unit());
			} catch (Exception err) {
				dbg.Error(err);
				//swallow error;
			}
			try {
				devDescr.m_removalSubj.OnCompleted();
			} catch (Exception err) {
				dbg.Error(err);
				//swallow error;
			}	
			
		}

		private void ProcessDeviceRemoval(EndpointDiscoveryMetadata epMeta) {
			dbg.Assert(m_subscriberCnt > 0);
			DeviceDescriptionImpl devDescr = null;
			lock (m_gate) {
				string id = epMeta.Address.Uri.OriginalString;
				if (m_dict.TryGetValue(id, out devDescr)) {
					m_dict.Remove(id);
				}
			}
			if (devDescr != null) {
				NotifyDeviceRemoval(devDescr);
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
							dbg.Error(err);
						}, () => {
							
						});
						
						var announcementEp = new UdpAnnouncementEndpoint(DiscoveryVersion.WSDiscoveryApril2005);

						//foreach (OperationDescription od in announcementEp.Contract.Operations) {
						//    od.Behaviors.Add(new MyOperationBehavior());
						//}

						//announcementEp.Behaviors.Add(new EpBeh());
						//TODO: create async variant
						m_host = new ServiceHost(m_announcementService);
						m_host.UnknownMessageReceived += (sender, args) => {
							try {								
								log.WriteError(String.Format("UnknownMessageReceived: ({0})", args.Message.Headers.Action), "AnnouncementService");
							} catch {
								//swallow error
							}
						};
						m_host.AddServiceEndpoint(announcementEp);
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
