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
using System.ServiceModel;

using onvif.services.imaging;

namespace odm.onvif {
	public class ImagingObservable:IDisposable {
		ImagingPort m_proxy;

		public ImagingObservable(ImagingPort proxy) {
			m_proxy = proxy;
		}
		
		public IObservable<ImagingSettings> GetImagingSettings(string videoSourceToken) {
			var request = new GetImagingSettingsRequest() {
				VideoSourceToken = videoSourceToken
			};
			var asyncOp = Observable.FromAsyncPattern<GetImagingSettingsRequest, GetImagingSettingsResponse>(m_proxy.BeginGetImagingSettings, m_proxy.EndGetImagingSettings);
			return asyncOp(request).Select(x => x.ImagingSettings);
		}

		public IObservable<Unit> SetImagingSettings(string videoSourceToken, ImagingSettings imagingSettings, bool forcePersistence = true) {
			var request = new SetImagingSettingsRequest() {
				ImagingSettings = imagingSettings,
				VideoSourceToken = videoSourceToken,
				ForcePersistence = forcePersistence
			};
			var asyncOp = Observable.FromAsyncPattern<SetImagingSettingsRequest, SetImagingSettingsResponse>(m_proxy.BeginSetImagingSettings, m_proxy.EndSetImagingSettings);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<ImagingOptions> GetOptions(string videoSourceToken) {
			var request = new GetOptionsRequest() {
				VideoSourceToken = videoSourceToken
			};
			var asyncOp = Observable.FromAsyncPattern<GetOptionsRequest, GetOptionsResponse>(m_proxy.BeginGetOptions, m_proxy.EndGetOptions);
			return asyncOp(request).Select(x => x.ImagingOptions);
		}

		public IObservable<ImagingStatus> GetStatus(string videoSourceToken) {
			var request = new GetStatusRequest() {
				VideoSourceToken = videoSourceToken
			};
			var asyncOp = Observable.FromAsyncPattern<GetStatusRequest, GetStatusResponse>(m_proxy.BeginGetStatus, m_proxy.EndGetStatus);
			return asyncOp(request).Select(x => x.Status);
		}

		public IObservable<MoveOptions> GetMoveOptions(string videoSourceToken) {
			var request = new GetMoveOptionsRequest() {
				VideoSourceToken = videoSourceToken
			};
			var asyncOp = Observable.FromAsyncPattern<GetMoveOptionsRequest, GetMoveOptionsResponse>(m_proxy.BeginGetMoveOptions, m_proxy.EndGetMoveOptions);
			return asyncOp(request).Select(x => x.MoveOptions);
		}
		
		public IObservable<Unit> Move(string videoSourceToken) {
			var request = new MoveRequest() {
				VideoSourceToken = videoSourceToken
			};
			var asyncOp = Observable.FromAsyncPattern<MoveRequest, MoveResponse>(m_proxy.BeginMove, m_proxy.EndMove);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<Unit> Stop(string videoSourceToken) {
			var request = new StopRequest() {
				VideoSourceToken = videoSourceToken
			};
			var asyncOp = Observable.FromAsyncPattern<StopRequest, StopResponse>(m_proxy.BeginStop, m_proxy.EndStop);
			return asyncOp(request).Select(x => new Unit());
		}

		public void Dispose() {
		    //m_proxy.Close();
		}
		public ImagingPort Services {
			get {
				return m_proxy;
			}
		}
	}
}
