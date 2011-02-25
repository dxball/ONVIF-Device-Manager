using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

using odm.onvif;
using odm.utils;

using onvif.services.device;
using onvif.services.media;
using onvif.types;

using device = global::onvif.services.device;
using media = global::onvif.services.media;
using tt = global::onvif.types;
using System.Xml;


namespace odm.models {


	public class SystemLogModel : ModelBase<SystemLogModel> {

		protected override IEnumerable<IObservable<object>> LoadImpl(onvif.Session session, IObserver<SystemLogModel> observer) {
			SystemLog sysLog = null;
			
			DeviceObservable device = null;
			yield return session.GetDeviceMtomClient().Handle(x => device = x);
			dbg.Assert(device != null);

			yield return device.GetSystemLog().Handle(x => sysLog = x);
			dbg.Assert(sysLog != null);

			if (!String.IsNullOrWhiteSpace(sysLog.String)) {
				log = sysLog.String;
			} else if (sysLog.Binary != null && sysLog.Binary.Length > 0) {
				log = Encoding.UTF8.GetString(sysLog.Binary);
			} else {
				log = null;
			}

			//text = sysLog.String;
			//data = sysLog.Binary;

			//NotifyPropertyChanged(x => x.text);
			//NotifyPropertyChanged(x => x.data);
			NotifyPropertyChanged(x => x.log);
			
			if (observer != null) {
				observer.OnNext(this);
			}
		}
		public string log {get; private set;}		
	
		//public string text {get; private set;}		
		//public byte[] data {get; private set;}		
	}
}
