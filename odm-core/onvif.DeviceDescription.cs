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
//using System.Linq;
//using System.Text;
//using System.ServiceModel.Discovery;
//using System.ServiceModel;

//using onvif.services.media;
//using onvif.services.device;
//using System.ServiceModel.Channels;
//using System.Threading;

//using onvifdm.utils;
//using nvc.models;

namespace odm.onvif {
	public interface IDeviceDescription {
		string name {get;}
		string id {get;}
		string deviceConfigId {get;}
		string location {get;}
		IEnumerable<Uri> uris {get;}
		IEnumerable<string> scopes {get;}
		IObservable<Unit> removal {get;}
	}


		//public static IObservable<DeviceDescription> Load(Uri deviceUri) {
		//    return Observable.Start(() => {
		//        var _epMetadata = new EndpointDiscoveryMetadata();
		//        _epMetadata.ListenUris.Add(deviceUri);
		//        var devDescr = new DeviceDescription(_epMetadata);
		//        devDescr.deviceUri = deviceUri;
		//        var proxy = DeviceDescription.CreateDeviceClient(deviceUri);
		//        var session = devDescr.CreateSession();
		//        devDescr.devInfo = session.GetDeviceInfo().First();
		//        devDescr.capabilities = proxy.GetCapabilities().First();
		//        devDescr.deviceUri = deviceUri;
		//        return devDescr;
		//    });
		//}

		//public DeviceDescription(EndpointDiscoveryMetadata epMetadata) {
		//    this.epMetadata = epMetadata;
		//}

		
		//public DeviceInfo devInfo = null;
		//public Capabilities capabilities = null;
		//public Uri deviceUri = null;

		//public string IPAddress{
		//    get{
		//        if (deviceUri != null) {
		//            return deviceUri.Host;
		//        }
		//        return String.Join(", ", epMetadata.ListenUris.Select(x => x.Host));			
		//    }
		//}

		//public string Id {
		//    get {
		//        return epMetadata.Address.Uri.OriginalString;
		//    }
		//}

		//public Session CreateSession() {
		//    var session = new Session(this);
		//    return session;
		//}

		
}
