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
//
//----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Discovery;
using System.Disposables;
using System.Threading;

using nvc.utils;
using nvc.rx;

namespace nvc {	
	public class WsDiscoveryObservable {	

		protected Func<DiscoveryClient> m_getDiscoveryClient = null;

		public WsDiscoveryObservable(DiscoveryEndpoint discoveryEndpoint){
			if(discoveryEndpoint==null){
				throw new ArgumentNullException("discoveryEndpoint");
			}
			m_getDiscoveryClient = () => {
				return new DiscoveryClient(discoveryEndpoint);
			};
		}
		public WsDiscoveryObservable(Func<DiscoveryClient> factory) {
			if (factory == null) {
				throw new ArgumentNullException("factory");
			}
			m_getDiscoveryClient = factory;
		}

		public IObservable<EndpointDiscoveryMetadata> Find(FindCriteria findCriteria){
			
			return Observable.CreateWithDisposable<EndpointDiscoveryMetadata>(observer=>{

				var dc = m_getDiscoveryClient();
				var state = ObserverState.Create();
				object sync = new Object();
				
				//dc.Open();
				dc.FindCompleted += (sender, e) => {
					bool completing = state.transit(ObserverState.subscribed, ObserverState.completed);
					DebugHelper.Assert(state.isDisposed() == e.Cancelled);
					if (!completing) {
						return;
					}

					DebugHelper.Assert(!e.Cancelled);

					if (e.Error != null) {
						observer.OnError(e.Error);
					} else {
						observer.OnCompleted();
					}
					dc.Close();
				};

	            dc.FindProgressChanged += (sender, e) => {
					DebugHelper.Assert(!state.isDisposed());
					state.value.ToString();
					observer.OnNext(e.EndpointDiscoveryMetadata);
			    };

				dc.FindAsync(findCriteria, sync);
				
				return Disposable.Create(()=>{
					DebugHelper.Assert(!state.isDisposed());
					var disposing = state.transit(ObserverState.subscribed, ObserverState.disposed);
					if (disposing) {
						dc.CancelAsync(sync);
						dc.Close();
					}	
				});			   
			});
		}


		public IObservable<EndpointDiscoveryMetadata> Resolve(ResolveCriteria resolveCriteria) {

			return Observable.CreateWithDisposable<EndpointDiscoveryMetadata>(observer => {
				//var stream = new Subject<EndpointDiscoveryMetadata>();
				var dc = m_getDiscoveryClient();
				var state = ObserverState.Create();
				object sync = new Object();

				dc.Open();
				
				dc.ResolveCompleted += (sender, e) => {
					DebugHelper.Assert(!state.isCompleted());
					bool completing = state.transit(ObserverState.subscribed, ObserverState.completed);
					DebugHelper.Assert(state.isDisposed() == e.Cancelled);
					if (!completing) {
						return;
					}

					DebugHelper.Assert(!e.Cancelled);

					if (e.Error != null) {
						observer.OnError(e.Error);
					} else {
						if (e.Result != null) {
							observer.OnNext(e.Result.EndpointDiscoveryMetadata);
						}
						observer.OnCompleted();
					}
					dc.Close();
				};

				dc.ResolveAsync(resolveCriteria, sync);

				return Disposable.Create(() => {
					DebugHelper.Assert(!state.isDisposed());
					var disposing = state.transit(ObserverState.subscribed, ObserverState.disposed);
					if (disposing) {
						dc.CancelAsync(sync);
					}					
				});
			});
		}

	}
}
