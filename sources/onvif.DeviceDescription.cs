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
using System.ServiceModel;

using onvif.services.media;
using onvif.services.device;
using System.ServiceModel.Channels;
using System.Threading;
using nvc.utils;
using nvc.models;

namespace nvc.onvif {
	public class DeviceDescription {

		public static IObservable<DeviceDescription> Load(Uri deviceUri) {
			return Observable.Start(() => {
				var _epMetadata = new EndpointDiscoveryMetadata();
				_epMetadata.ListenUris.Add(deviceUri);
				var devDescr = new DeviceDescription(_epMetadata);
				var proxy = DeviceDescription.CreateDeviceClient(deviceUri);
				devDescr.devInfo = proxy.GetDeviceInformation().Select(x=>new DeviceInfo(){Model=x.Model, FirmwareVersion = x.FirmwareVersion, HardwareId = x.HardwareId, Manufacturer = x.Manufacturer, SerialNumber = x.SerialNumber}).First();
				devDescr.capabilities = proxy.GetCapabilities().First();
				devDescr.deviceUri = deviceUri;
				return devDescr;
			});
		}

		public DeviceDescription(EndpointDiscoveryMetadata epMetadata) {
			this.epMetadata = epMetadata;
		}

		public EndpointDiscoveryMetadata epMetadata;
		public DeviceInfo devInfo = null;
		public Capabilities capabilities = null;
		public Uri deviceUri = null;

		public string IPAddress{
			get{
				return epMetadata.ListenUris[0].Host;
			}
		}

		public string Id {
			get {
				return epMetadata.Address.Uri.OriginalString;
			}
		}

		public Session CreateSession() {
			var session = new Session(this);
			return session;
		}

		private static ChannelFactory<Device> m_deviceFactory =
			new ChannelFactory<Device>(new WSHttpBinding(SecurityMode.None) {
				TextEncoding = Encoding.UTF8		
			});
		private static ChannelFactory<Media> m_mediaFactory = new ChannelFactory<Media>(new WSHttpBinding(SecurityMode.None));

		public static DeviceObservable CreateDeviceClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = m_deviceFactory.CreateChannel(endpointAddr);
			return new DeviceObservable(proxy);				
		}

		public static MediaObservable CreateMediaClient(Uri uri) {
			var endpointAddr = new EndpointAddress(uri);
			var proxy = m_mediaFactory.CreateChannel(endpointAddr);
			return new MediaObservable(proxy);
		}

		public DeviceObservable CreateDeviceClient() {
			return CreateDeviceClient(deviceUri);
		}
		
		public MediaObservable CreateMediaClient() {
			return CreateMediaClient(new Uri(capabilities.Media.XAddr));
		}
	}
}
