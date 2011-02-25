using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using odm.onvif;
using odm.utils;

using dev = global::onvif.services.device;
using med = global::onvif.services.media;
using tt = global::onvif.types;
using onvif.services.device;
using onvif.services.media;
using System.IO;
using System.Net;
using System.Net.Mime;
using odm.utils.rx;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace odm.models {

	public class MaintenanceModel : ModelBase<MaintenanceModel> {
		public MaintenanceModel() {
			firmwareUpgradeSupported = false;
		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<MaintenanceModel> observer) {
			GetDeviceInformationResponse info = null;
			yield return session.GetDeviceInformation().Handle(x => info = x);
			dbg.Assert(info != null);

			DeviceObservable device = null;
			yield return session.GetDeviceClient().Handle(x => device = x);
			dbg.Assert(device != null);

			//StartFirmwareUpgradeResponse upgradeInfo = null;
			//yield return device.StartFirmwareUpgrade().Handle(x => upgradeInfo = x).IgnoreError();

			//if (upgradeInfo != null) {
				firmwareUpgradeSupported = true;
				//firmwareUploadUri = upgradeInfo.UploadUri;
			//}
			currentFirmwareVersion = info.FirmwareVersion;

			NotifyPropertyChanged(x => x.firmwareUpgradeSupported);
			NotifyPropertyChanged(x => x.firmwareUploadUri);
			NotifyPropertyChanged(x => x.currentFirmwareVersion);
			
			if (observer != null) {
				observer.OnNext(this);
			}
		}

		[Serializable]
		public class BackupFiles {
			[XmlElement("BackupFile")]
			//[XmlArrayItem("BackupFile")]
			public tt::BackupFile[] files;
		}

		protected IEnumerable<IObservable<object>> BackupImpl(Stream stream, IObserver<Unit> observer) {
			BackupFiles backup = new BackupFiles();
			yield return session.GetSystemBackup().Handle(x => backup.files = x);
			dbg.Assert(backup.files != null);

			using (var _w = XmlDictionaryWriter.CreateMtomWriter(stream, Encoding.UTF8, int.MaxValue, "text/xml")) {
				_w.WriteNode(backup.Serialize().CreateNavigator(), false);
			}

			if (observer != null) {
				observer.OnNext(new Unit());
			}
		}

		public IObservable<Unit> Backup(Stream stream) {
			return Observable.Iterate<Unit>(observer => BackupImpl(stream, observer)).ObserveOn(SynchronizationContext.Current);
		}


		protected IEnumerable<IObservable<object>> RestoreImpl(Stream stream, IObserver<Unit> observer) {
			BackupFiles backup = null;

			using (var _r = XmlDictionaryReader.CreateMtomReader(stream, Encoding.UTF8, new XmlDictionaryReaderQuotas())) {
				backup = (_r as XmlReader).Deserialize<BackupFiles>();
			}

			yield return session.RestoreSystem(backup.files).Idle();
			
			
			if (observer != null) {
				observer.OnNext(new Unit());
			}
		}

		public IObservable<Unit> Restore(Stream stream) {
			return Observable.Iterate<Unit>(observer => RestoreImpl(stream, observer)).ObserveOn(SynchronizationContext.Current);
		}

		public IObservable<string> Reboot() {
			return session.SystemReboot().ObserveOn(SynchronizationContext.Current);
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<MaintenanceModel> observer) {
			DeviceObservable device = null;
			yield return session.GetDeviceClient().Handle(x => device = x);
			dbg.Assert(device != null);
			StartFirmwareUpgradeResponse upgradeInfo = null;
			yield return device.StartFirmwareUpgrade().Handle(x => upgradeInfo = x);
			dbg.Assert(upgradeInfo != null);

			//if (upgradeInfo.UploadDelay > 0) {
			//    yield return Observable.Delay(upgradeInfo.UploadDelay);
			//}

			using (var fs = new FileStream(firmwarePath, FileMode.Open)) {
				
				var requestUri = new Uri(upgradeInfo.UploadUri);
				if (requestUri.Scheme != Uri.UriSchemeHttp) {
					throw new NotSupportedException(String.Format("specified protocol ({0}) not suppoted", requestUri.Scheme));
				}
				var request = (HttpWebRequest)HttpWebRequest.Create(requestUri);

				request.Method = WebRequestMethods.Http.Post;
				//request.Method = WebRequestMethods.File.UploadFile;
				//request.Method = WebRequestMethods.Ftp.UploadFile;
				request.MaximumResponseHeadersLength = 10 * 1024;
				//request.UserAgent = "Mozilla/4.0 (compatible; VAMS)";
				request.ContentType = MediaTypeNames.Application.Octet; //"application/octet-stream"
				//request.Headers.Add("Content-Transfer-Encoding: binary");
				request.ContentLength = fs.Length;
				//request.SendChunked = true;
				request.Timeout = 60*60*1000;//60min
				request.ReadWriteTimeout = 60*60*1000;//60min
				request.ProtocolVersion = HttpVersion.Version10;

				Stream uploadStream = null;
				yield return Observable.FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)().Handle(x=>uploadStream =x);
				Exception sendError = null;
				
				try {
					yield return ObservableStream.Copy(fs, uploadStream).Idle().HandleError(err=>sendError = err);			
				} finally {
					uploadStream.Close();
				}
				
				if (sendError !=null) {
					throw sendError;
				}
				HttpWebResponse response = null;
				yield return Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)().Handle(x => response = (HttpWebResponse)x);
				if (response.StatusCode != HttpStatusCode.OK) {
					response.Close();
					throw new Exception("upload failed");
				}
				dbg.Assert(response != null);
				response.Close();

			};
						
			if (observer != null) {
				observer.OnNext(this);
			}	
		}
		public bool firmwareUpgradeSupported {get; private set;}
		public string firmwareUploadUri	{get; private set;}
		public string currentFirmwareVersion {get; private set;}
		public string firmwarePath {get;set;}
	}
}
