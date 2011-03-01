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
using odm.utils;
using System.ComponentModel;
using System.Concurrency;
using System.Disposables;

using dev=onvif.services.device;
using odm.models;
using odm.utils.rx;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery.VersionApril2005;
using onvif;

namespace odm.onvif {

	public class NvcHelper {
		public const string defaultProfileName = @"odm-dp-{0}";
		public const string onvifNameScope = @"onvif://www.onvif.org/name/";
		public const string onvifLocationScope = @"onvif://www.onvif.org/location/";
		public const string odmNameScope = @"urn:odm:name/";
		public const string odmLocationScope = @"urn:odm:location/";
		//public const string odmProfileScope = @"urn:odm:profile/";
		public const string odmDeviceIdScope = @"urn:odm:device-id/";

		public static string[] GetScopeValues(IEnumerable<string> scopes, string scopePrefix) {
			if (scopes == null) {
				return null;
			}
			//var value = String.Join(", ", scopes
			//    .Where(x => x.StartsWith(scopePrefix))
			//    .Select(x => x.Substring(scopePrefix.Length))
			//);
			//return Uri.UnescapeDataString(value);
			return scopes
				.Where(x => x.StartsWith(scopePrefix))
				.Select(x => x.Substring(scopePrefix.Length))
				.Select(x => Uri.UnescapeDataString(x))
				.ToArray();
		}

		public static string GetName(IEnumerable<string> scopes) {

			var names = GetScopeValues(scopes, odmNameScope);
			if (names.Length>0) {
				return names[names.Length-1];
			}
			names = GetScopeValues(scopes, onvifNameScope);
			if (names.Length <= 0) {
				return null;
			}
			return names[names.Length - 1];
		}

		public static string GetLocation(IEnumerable<string> scopes) {

			var locations = GetScopeValues(scopes, odmLocationScope);
			if (locations.Length>0) {
				return locations[locations.Length - 1];
			}
			locations = GetScopeValues(scopes, onvifLocationScope);
			if (locations.Length <= 0) {
				return null;
			}
			return locations[locations.Length - 1];
		}

		public static string GetDeviceId(IEnumerable<string> scopes) {
			return GetScopeValues(scopes, odmDeviceIdScope).Single();
		}

		public static ProfileToken GetChannelProfileToken(VideoSourceToken videoSourceToken) {
			//var ch = Convert.ToBase64String(Encoding.UTF8.GetBytes(videoSourceToken.value));
			var ch = videoSourceToken.value;
			return new ProfileToken(String.Format(defaultProfileName, ch));
		}
	}
	


    public class DeviceDiscovery{
		
		private class DescoveredDeviceDescription : IDeviceDescription {
			private EndpointDiscoveryMetadata m_epMetadata;
			public DescoveredDeviceDescription(EndpointDiscoveryMetadata epMetadata) {
				m_epMetadata = epMetadata;
			}
			public IObservable<Unit> removal {
				get {
					throw new NotImplementedException();
				}
			}
			public string id {
				get {
					return m_epMetadata.Address.Uri.OriginalString;
				}
			}
			public string name {
				get {
					return NvcHelper.GetName(scopes);
				}
			}
			public string location {
				get {
					return NvcHelper.GetLocation(scopes);;
				}
			}
			public string deviceConfigId {
				get {
					return NvcHelper.GetLocation(scopes);
				}
			}
			public IEnumerable<Uri> uris {
				get {
					return m_epMetadata.ListenUris;
				}
			}
			public IEnumerable<string> scopes {
				get {
					return m_epMetadata.Scopes.Select(x=>x.OriginalString);
				}
			}

			
		}

		private WsDiscoveryObservable m_wsDiscovery;
		//public TimeSpan Duration = TimeSpan.FromSeconds(5);

			
		private FindCriteria m_getFindCriteria(){
			var fc = new FindCriteria();
			//fc.Scopes.Add(new Uri("onvif://www.onvif.org/type/video_encoder"));
			//fc.Scopes.Add(new Uri("onvif://www.onvif.org"));
			fc.ContractTypeNames.Add(new XmlQualifiedName("NetworkVideoTransmitter", @"http://www.onvif.org/ver10/network/wsdl"));
			//fc.Duration = Duration;
			fc.Duration = TimeSpan.MaxValue;
			fc.MaxResults = int.MaxValue;
			return fc;
		}

		private ResolveCriteria m_getResolveCriteria(string id) {
			var rc = new ResolveCriteria();
			rc.Address = new EndpointAddress(id);
			//rc.Duration = Duration;
			rc.Duration = TimeSpan.MaxValue;
			return rc;
		}
		
		public DeviceDiscovery() : this(TimeSpan.FromSeconds(5)){
		}

		public DeviceDiscovery(TimeSpan duration) {
			//var binding = new CustomBinding();
			//binding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8));
			//binding.Elements.Add(new HttpTransportBindingElement());

			var ep = new UdpDiscoveryEndpoint(DiscoveryVersion.WSDiscoveryApril2005);
			ep.MaxResponseDelay = duration;
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

		public IObservable<IDeviceDescription> Find() {
			return m_wsDiscovery.Find(m_getFindCriteria()).Select(x => new DescoveredDeviceDescription(x));
			//return Observable.CreateWithDisposable<DeviceDescription>(FindSubscribe);
		}

		public IObservable<IDeviceDescription> Resolve(string id) {
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
						dbg.Assert(_subscription != Disposable.Empty);
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
