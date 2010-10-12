#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Discovery;
using System.Threading;
using System.ServiceModel;
using System.Xml;
using System.Windows.Threading;
using nvc.utils;
using System.ComponentModel;
using System.Concurrency;
using System.Disposables;

using dev=onvif.services.device;
using nvc.models;
using nvc.rx;

namespace nvc.onvif {

	public class NvcHelper {
		public const string OnvifNameScope = @"onvif://www.onvif.org/name/";
		public const string OnvifLocationScope = @"onvif://www.onvif.org/location/";
		public const string SynesisNameScope = @"http://synesis.com/name/";
		public const string SynesisLocationScope = @"http://synesis.com/location/";
		public const string SynesisProfileScope = @"http://synesis.com/profile/";
		

		public static string GetName(IEnumerable<string> scopes, string scopePrefix) {
			if (scopes == null) {
				return null;
			}
			var name = String.Join(", ", scopes
				.Where(x => x.StartsWith(scopePrefix))
				.Select(x => x.Substring(scopePrefix.Length))
			);
			return Uri.UnescapeDataString(name);
		}

		public static string GetName(IEnumerable<string> scopes) {

			var name = GetName(scopes, SynesisNameScope);
			if (!String.IsNullOrEmpty(name)) {
				return name;
			}

			return GetName(scopes, OnvifNameScope);
		}

		public static string GetLocation(IEnumerable<string> scopes, string scopePrefix) {
			if (scopes == null) {
				return null;
			}
			var location = String.Join(", ", scopes
				.Where(x => x.StartsWith(scopePrefix))
				.Select(x => x.Substring(scopePrefix.Length))
			);
			return Uri.UnescapeDataString(location);
		}

		public static string GetLocation(IEnumerable<string> scopes) {

			var name = GetLocation(scopes, SynesisLocationScope);
			if (!String.IsNullOrEmpty(name)) {
				return name;
			}

			return GetLocation(scopes, OnvifLocationScope);
		}

		public static string GetChannelProfileToken(string videoSourceToken) {
			return String.Concat(SynesisProfileScope, videoSourceToken);
		}
	}
	


    public class DeviceDiscovery{
		private class DescoveredDeviceDescription : DeviceDescription {
			private EndpointDiscoveryMetadata m_epMetadata;
			public DescoveredDeviceDescription(EndpointDiscoveryMetadata epMetadata) {
				m_epMetadata = epMetadata;
			}
			public override string Id {
				get {
					return m_epMetadata.Address.Uri.OriginalString;
				}
			}
			public override string Name {
				get {
					var name = NvcHelper.GetName(m_epMetadata.Scopes.Select(x=>x.OriginalString));
					return name;
				}
			}
			public override string Location {
				get {
					var location = NvcHelper.GetLocation(m_epMetadata.Scopes.Select(x => x.OriginalString));
					return location;
				}
			}
			public override IEnumerable<Uri> Uris {
				get {
					return m_epMetadata.ListenUris;
				}
			}
			public override IEnumerable<string> Scopes {
				get {
					return m_epMetadata.Scopes.Select(x=>x.OriginalString);
				}
			}
		}

		private WsDiscoveryObservable m_wsDiscovery;
		public TimeSpan Duration = TimeSpan.FromSeconds(5);
		
		private FindCriteria m_getFindCriteria(){
			var fc = new FindCriteria();
			//fc.Scopes.Add(new Uri("onvif://www.onvif.org/type/video_encoder"));
			//fc.Scopes.Add(new Uri("onvif://www.onvif.org"));
			fc.ContractTypeNames.Add(new XmlQualifiedName("NetworkVideoTransmitter", @"http://www.onvif.org/ver10/network/wsdl"));
			fc.Duration = Duration;
			return fc;
		}

		private ResolveCriteria m_getResolveCriteria(string id) {
			var rc = new ResolveCriteria();
			rc.Address = new EndpointAddress(id);
			rc.Duration = Duration;
			return rc;
		}
		
		public DeviceDiscovery() {
			var ep = new UdpDiscoveryEndpoint(DiscoveryVersion.WSDiscoveryApril2005);
			m_wsDiscovery = new WsDiscoveryObservable(ep);
		}

		//private static IObservable<Tuple<DeviceInfo, dev::Capabilities>> FulfillDeviceInfo(DeviceObservable proxy) {
		//    return Observable.Join(
		//        proxy.GetDeviceInformation()
		//        .And(proxy.GetCapabilities())
		//        .And(proxy.GetScopes())
		//        .Then((devInvo, capabilities, scopes) => new Tuple<DeviceInfo, dev::Capabilities>(new DeviceInfo() {
		//            Manufacturer = devInvo.Manufacturer,
		//            Model = devInvo.Model,
		//            FirmwareVersion = devInvo.FirmwareVersion,
		//            HardwareId = devInvo.HardwareId,
		//            SerialNumber = devInvo.SerialNumber,
		//            Name = DeviceInfoExtensions.GetName(scopes)
		//        }, capabilities))
		//    );			
		//}

		//private IObservable<DeviceDescription> StartRace(DeviceDescription devDescr) {
		//    return Observable.CreateWithDisposable<DeviceDescription>(observer => {
				
		//        var listenUris = devDescr.epMetadata.ListenUris;
		//        //int gate=0;
		//        var state = ObserverState.Create();

		//        var asyncOp = BatchOperation.Create<DeviceDescription>(batch => {
		//            listenUris.Select(x =>
		//                //CreateDeviceClient(x)
		//                DeviceDescription.CreateDeviceClient(x)
		//            ).ForEach(proxy => {
		//                batch.Join(FulfillDeviceInfo(proxy))
		//                    .TakeWhile(_ => state.transit(ObserverState.subscribed, ObserverState.completed) )
		//                    .Subscribe(tuple => {
		//                        batch.Dispose();
		//                        devDescr.devInfo = tuple.Item1;
		//                        devDescr.capabilities = tuple.Item2;
		//                        devDescr.deviceUri = proxy.uri;
		//                        observer.OnNext(devDescr);
		//                        observer.OnCompleted();
		//                    }, err => {
		//                        //swallow error
		//                        DebugHelper.Error(err);
		//                    });
		//                });
		//            return devDescr;
		//        });

		//        return asyncOp
		//            .Finally(() => {
		//                var noWinners = state.transit(ObserverState.subscribed, ObserverState.completed);
		//                if (noWinners) {
		//                    observer.OnError(new Exception("device is inaccessible"));
		//                }					
		//            })
		//            .OnError(err=>{
		//                DebugHelper.Error(err);
		//            })
		//            .Subscribe();

		//    });
		//}

		public IObservable<DeviceDescription> Find() {
			return m_wsDiscovery.Find(m_getFindCriteria()).Select(x => new DescoveredDeviceDescription(x));
			//return Observable.CreateWithDisposable<DeviceDescription>(FindSubscribe);
		}

		public IObservable<DeviceDescription> Resolve(string id) {
			return m_wsDiscovery.Resolve(m_getResolveCriteria(id)).Select(x => new DescoveredDeviceDescription(x));
			//return Observable.CreateWithDisposable<DeviceDescription>(observer=>ResolveSubscribe(observer, id));
		}

		//private IDisposable FindSubscribe(IObserver<DeviceDescription> observer) {
			
		//    var asyncOp = BatchOperation.Create<Unit>(batch => {
		//        batch.Join(m_wsDiscovery.Find(m_getFindCriteria()))
		//            .Subscribe(ep => {
		//                var devDescr = new DescoveredDeviceDescription(ep);
		//                batch.Join(
		//                    StartRace(devDescr).OnError(err=>{
		//                        observer.OnNext(devDescr);
		//                    })
		//                ).Subscribe(dev => {
		//                    observer.OnNext(dev);
		//                });
		//            });
		//        return new Unit();		 
		//    });

		//    return asyncOp
		//        .OnCompleted(()=>{
		//            observer.OnCompleted();
		//        })
		//        .Subscribe();
		//}
		
		//private IDisposable ResolveSubscribe(IObserver<DeviceDescription> observer, Uri id) {
						
		//    var asyncOp = BatchOperation.Create<Unit>(batch => {
		//        batch.Join(m_wsDiscovery.Resolve(m_getResolveCriteria(id)).OnError(err=>observer.OnError(err)))
		//            .Subscribe(ep => {
		//                var devDescr = new DeviceDescription(ep);
		//                batch.Join(
		//                    StartRace(devDescr).OnError(err => {
		//                        observer.OnNext(devDescr);
		//                    })
		//                ).Subscribe(dev => {
		//                    observer.OnNext(dev);
		//                });
		//            });
		//        return new Unit();
		//    });

		//    return asyncOp
		//        //.OnDispose(()=>{
		//        //    subscription.Dispose();
		//        //})
		//        .OnCompleted(() => {
		//            observer.OnCompleted();
		//        })
		//        .Subscribe();
		//}
    }

	public class SubjectA<T> : Subject<T>, ISubject<T> {
		private Notification<T> m_lastNotification = null;
		private bool m_isStopped = false;
		private object m_sync = new object();
		private Queue<Action> m_queue = new Queue<Action>();
		private bool m_queueIsBeingProcessed = false;

		public new void OnNext(T value) {
			base.OnNext(value);
			//if (!m_isStopped) {
			//    m_lastNotification = new Notification<T>.OnNext(value);
			//}
		}

		private void ProcessQueue() {
			Action _pendingAction;
			while (true) {
				lock (m_sync) {
					if (m_queue.IsEmpty()) {
						m_queueIsBeingProcessed = false;
						return;
					}
					_pendingAction = m_queue.Dequeue();
				}
				_pendingAction();
			}
		}

		public new void OnCompleted() {
			var _queueMustBeProcessed = false;

			lock (m_sync) {
				m_queue.Enqueue(() => {
					if (!m_isStopped) {
						m_isStopped = true;
						m_lastNotification = new Notification<T>.OnCompleted();
						base.OnCompleted();
					}
				});
				if (!m_queueIsBeingProcessed) {
					m_queueIsBeingProcessed = true;
					_queueMustBeProcessed = true;
				}
			}

			if (_queueMustBeProcessed) {
				ProcessQueue();
			}

		}

		public new void OnError(Exception error) {
			var _queueMustBeProcessed = false;

			lock (m_sync) {
				m_queue.Enqueue(() => {
					if (!m_isStopped) {
						m_isStopped = true;
						m_lastNotification = new Notification<T>.OnError(error);
						base.OnError(error);
					}
				});
				if (!m_queueIsBeingProcessed) {
					m_queueIsBeingProcessed = true;
					_queueMustBeProcessed = true;
				}
			}

			if (_queueMustBeProcessed) {
				ProcessQueue();
			}
		}

		public new IDisposable Subscribe(IObserver<T> observer) {
			var _queueMustBeProcessed = false;
			MutableDisposable _subscription = new MutableDisposable();

			lock (m_sync) {
				m_queue.Enqueue(() => {
					if (!m_isStopped) {
						_subscription.Disposable = base.Subscribe(observer);
						DebugHelper.Assert(_subscription != Disposable.Empty);
					} else {
						m_lastNotification.Accept(observer);
						//_subscription.Disposable = Disposable.Empty;
					}
				});
				if (!m_queueIsBeingProcessed) {
					m_queueIsBeingProcessed = true;
					_queueMustBeProcessed = true;
				}
			}

			if (_queueMustBeProcessed) {
				ProcessQueue();
			}

			return _subscription;

		}
	}

}
