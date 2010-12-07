using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using onvif.services.media;
using nvc.onvif;
using onvifdm.utils;
using med=onvif.services.media;

namespace nvc.models {

	public class VideoResolution {
		public int width;
		public int height;
		public HashSet<VideoEncoder> encoders = new HashSet<VideoEncoder>();
		public override string ToString() {
			return String.Concat(width, "x", height);
		}
	};

	public class VideoEncoder {
		public VideoEncoder(Encoding encoding) {
			this.encoding = encoding;
		}

		public VideoEncoder() {			
		}

		public enum Encoding {
			H264,
			JPEG,
			MPEG4
		}

		public Encoding encoding;

		public int maxFrameRate;
		public int minFrameRate;
		public int maxEncodingInterval;
		public int minEncodingInterval;
		public HashSet<VideoResolution> resolutions = new HashSet<VideoResolution>();
		public override string ToString() {
			return encoding.ToString();
		}
	}



	public class VideoStreamingModel : ModelBase<VideoStreamingModel> {
		ChannelDescription m_channel;
		public VideoStreamingModel(ChannelDescription channel) {
			m_channel = channel;
		}

		private static VideoEncoder CreateVideoEncoder(VideoEncoderConfigurationOptions options, VideoEncoding encoding) {
			VideoEncoder encoder = null;
			switch (encoding) {
				case VideoEncoding.H264:
					if (options.H264 != null) {
						encoder = new VideoEncoder(VideoEncoder.Encoding.H264);
						encoder.maxFrameRate = options.H264.FrameRateRange.Max;
						encoder.minFrameRate = options.H264.FrameRateRange.Min;
						encoder.maxEncodingInterval = options.H264.EncodingIntervalRange.Max;
						encoder.minEncodingInterval = options.H264.EncodingIntervalRange.Min;
					}
					break;

				case VideoEncoding.JPEG:
					if (options.JPEG != null) {
						encoder = new VideoEncoder(VideoEncoder.Encoding.JPEG);
						encoder.maxFrameRate = options.JPEG.FrameRateRange.Max;
						encoder.minFrameRate = options.JPEG.FrameRateRange.Min;
						encoder.maxEncodingInterval = options.JPEG.EncodingIntervalRange.Max;
						encoder.minEncodingInterval = options.JPEG.EncodingIntervalRange.Min;
					}
					break;

				case VideoEncoding.MPEG4:
					if (options.MPEG4 != null) {
						encoder = new VideoEncoder(VideoEncoder.Encoding.MPEG4);
						encoder.maxFrameRate = options.MPEG4.FrameRateRange.Max;
						encoder.minFrameRate = options.MPEG4.FrameRateRange.Min;
						encoder.maxEncodingInterval = options.MPEG4.EncodingIntervalRange.Max;
						encoder.minEncodingInterval = options.MPEG4.EncodingIntervalRange.Min;
					}
					break;
			}
			return encoder;
		}

		private static IEnumerable<VideoEncoder.Encoding> GetVideoEncodings(VideoEncoderConfigurationOptions options) {
			if (options.H264 != null) {
				yield return VideoEncoder.Encoding.H264;
			}
			if (options.JPEG != null) {
				yield return VideoEncoder.Encoding.JPEG;
			}
			if (options.MPEG4 != null) {
				yield return VideoEncoder.Encoding.MPEG4;
			}
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<VideoStreamingModel> observer) {
			Profile[] profiles = null;

			yield return session.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();

			if (profile == null) {
				//create default profile
				yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			}

			if (profile.VideoSourceConfiguration == null) {
				//add default video source configuration
				VideoSourceConfiguration[] vscs = null;
				yield return session.GetVideoSourceConfigurations().Handle(x=>vscs=x);
				DebugHelper.Assert(vscs != null);
				var vsc = vscs.Where(x => x.SourceToken == m_channel.Id).FirstOrDefault();
				yield return session.AddVideoSourceConfiguration(profile.token, vsc.token).Idle();
				profile.VideoSourceConfiguration = vsc;
			}
			
			var vec = profile.VideoEncoderConfiguration;
			if (vec == null) {
				//add default video encoder
				yield return session.AddDefaultVideoEncoder(profile.token).Handle(x => vec = x);
				DebugHelper.Assert(vec != null);
				profile.VideoEncoderConfiguration = vec;
			}
			
			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			VideoEncoderConfiguration[] vecs = null;
			yield return session.GetVideoEncoderConfigurations().Handle(x => vecs = x);
			DebugHelper.Assert(vecs != null);

			VideoEncoderConfigurationOptions options = null;
			yield return session.GetVideoEncoderConfigurationOptions().Handle(x => options = x);
			DebugHelper.Assert(options != null);
			
			var resolutions = new Dictionary<Tuple<int, int>, VideoResolution>();
			var encoders = new Dictionary<VideoEncoder.Encoding, VideoEncoder>();

			//GetVideoEncodings(options).ForEach(enc => {

			//});

			if (options.H264 != null) {
				VideoEncoder encoder = null;
				if (!encoders.TryGetValue(VideoEncoder.Encoding.H264, out encoder)) {
					encoder = CreateVideoEncoder(options, VideoEncoding.H264);
					encoders.Add(encoder.encoding, encoder);
				}

				foreach(var r in options.H264.ResolutionsAvailable){
					VideoResolution resolution = null;
					var resolution_key = new Tuple<int, int>(r.Width, r.Height);
					if (!resolutions.TryGetValue(resolution_key, out resolution)) {
						resolution = new VideoResolution(){
							width = r.Width,
							height = r.Height
						};
						resolutions.Add(resolution_key, resolution);
					}
					resolution.encoders.Add(encoder);
					encoder.resolutions.Add(resolution);
				}				
			}

			if (options.JPEG != null) {
				VideoEncoder encoder = null;
				if (!encoders.TryGetValue(VideoEncoder.Encoding.JPEG, out encoder)) {
					encoder = CreateVideoEncoder(options, VideoEncoding.JPEG);
					encoders.Add(encoder.encoding, encoder);
				}

				foreach(var r in options.JPEG.ResolutionsAvailable){
					VideoResolution resolution = null;
					var resolution_key = new Tuple<int, int>(r.Width, r.Height);
					if (!resolutions.TryGetValue(resolution_key, out resolution)) {
						resolution = new VideoResolution(){
							width = r.Width,
							height = r.Height
						};
						resolutions.Add(resolution_key, resolution);
					}
					resolution.encoders.Add(encoder);
					encoder.resolutions.Add(resolution);
				}				
			}

			if (options.MPEG4 != null) {
				VideoEncoder encoder = null;
				if (!encoders.TryGetValue(VideoEncoder.Encoding.MPEG4, out encoder)) {
					encoder = CreateVideoEncoder(options, VideoEncoding.MPEG4);
					encoders.Add(encoder.encoding, encoder);
				}

				foreach(var r in options.MPEG4.ResolutionsAvailable){
					VideoResolution resolution = null;
					var resolution_key = new Tuple<int, int>(r.Width, r.Height);
					if (!resolutions.TryGetValue(resolution_key, out resolution)) {
						resolution = new VideoResolution(){
							width = r.Width,
							height = r.Height
						};
						resolutions.Add(resolution_key, resolution);
					}
					resolution.encoders.Add(encoder);
					encoder.resolutions.Add(resolution);
				}
				
			}

			Func<VideoEncoding, VideoEncoder.Encoding> convert_encoding = enc=>{
				switch(enc){
					case VideoEncoding.H264:
						return VideoEncoder.Encoding.H264;
					case VideoEncoding.JPEG:
						return VideoEncoder.Encoding.JPEG;
					case VideoEncoding.MPEG4:
						return VideoEncoder.Encoding.MPEG4;
				}
				throw new Exception("unknown encoding");
			};

			this.bitrate = vec.RateControl.BitrateLimit;
			this.maxFrameRate = encoders.Values.Max(x => x.maxFrameRate);
			this.minFrameRate = encoders.Values.Min(x => x.minFrameRate);
			this.maxEncodingInterval = encoders.Values.Max(x => x.maxEncodingInterval);
			this.minEncodingInterval = encoders.Values.Min(x => x.minEncodingInterval);
			this.supportedEncoders = encoders.Values;
			this.supportedResolutions = resolutions.Values.OrderByDescending(x => new Tuple<int, int>(x.width, x.height));
			
			m_metadata.SetBoth(profile.MetadataConfiguration != null);
			m_currentResolution.SetBoth(resolutions[new Tuple<int,int>(vec.Resolution.Width, vec.Resolution.Height)]);
			m_currentEncoder.SetBoth(encoders[convert_encoding(vec.Encoding)]);
			m_channelName.SetBoth(profile.Name);
			m_encodingInterval.SetBoth(vec.RateControl.EncodingInterval);


			if (vec.RateControl.FrameRateLimit > this.maxFrameRate) {
				vec.RateControl.FrameRateLimit = this.maxFrameRate;
				yield return session.SetVideoEncoderConfiguration(vec, true).Idle();
			} else if (vec.RateControl.FrameRateLimit < this.minFrameRate) {
				vec.RateControl.FrameRateLimit = this.minFrameRate;
				yield return session.SetVideoEncoderConfiguration(vec, true).Idle();
			}
			m_frameRate.SetBoth(vec.RateControl.FrameRateLimit);
			
			
			NotifyPropertyChanged(x => x.metadata);
			NotifyPropertyChanged(x => x.currentResolution);
			NotifyPropertyChanged(x => x.currentEncoder);
			NotifyPropertyChanged(x => x.frameRate);
			NotifyPropertyChanged(x => x.bitrate);
			NotifyPropertyChanged(x => x.supportedEncoders);
			NotifyPropertyChanged(x => x.supportedResolutions);
			NotifyPropertyChanged(x => x.encodingInterval);

			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;
			string uri = null;
			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => uri = x);
			this.mediaUri = uri;

			if (observer != null) {
				observer.OnNext(this);
			}

		}
		IEnumerable<IObservable<Object>> ConfigVec(VideoEncoderConfiguration vec, VideoEncoderConfigurationOptions opt, Profile profile) {
			Exception _error = null;
			vec.Resolution = new med::VideoResolution() {
					Width = currentResolution.width,
					Height = currentResolution.height
			};

			yield return session.SetVideoEncoderConfiguration(vec, true).Idle().HandleError(x => _error = x);
			if (_error != null) {
				yield break;
			}

			yield return session.AddVideoEncoderConfiguration(profile.token, vec.token).Idle().HandleError(x => _error = x);
			if (_error == null) {
				profile.VideoEncoderConfiguration = vec;
			}
		}

		protected override IEnumerable<IObservable<Object>> ApplyChangesImpl(Session session, IObserver<VideoStreamingModel> observer) {
			if (!isModified) {
				yield break;
			}
			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			yield return media.DeleteProfile(NvcHelper.GetChannelProfileToken(m_channel.Id)).Idle().IgnoreError();

			Profile profile = null;
			yield return media.CreateProfile(m_channelName.current, NvcHelper.GetChannelProfileToken(m_channel.Id)).Handle(x => profile = x);

			
			//Profile[] profiles = null;
			//yield return session.GetProfiles().Handle(x => profiles = x);
			//DebugHelper.Assert(profiles != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			//DebugHelper.Assert(profile != null);

			

			
			//if (profile.VideoEncoderConfiguration != null) {
			//    yield return media.RemoveVideoEncoderConfiguration(profile.token).Idle();
			//    profile.VideoEncoderConfiguration = null;
			//}

			//if (profile.VideoSourceConfiguration != null) {
			//    yield return media.RemoveVideoSourceConfiguration(profile.token).Idle();
			//    profile.VideoSourceConfiguration = null;
			//}

			//VideoEncoderConfigurationOptions options = null;
			//yield return media.GetVideoEncoderConfigurationOptions().Handle(x => options = x);
			//DebugHelper.Assert(options != null);
			

			VideoSourceConfiguration[] vscs = null;
			yield return session.GetCompatibleVideoSourceConfigurations(profile.token).Handle(x => vscs = x);
			//var vsc = vscs.Where(x=>x.SourceToken == m_channel.Id).OrderByDescending(x => new Tuple<int, int>(x.Bounds.width, x.Bounds.height)).FirstOrDefault();
			foreach (var _vsc in vscs.Where(x => x.SourceToken == m_channel.Id).OrderBy(x => x.UseCount)) {
				yield return session.AddVideoSourceConfiguration(profile.token, _vsc.token);
				profile.VideoSourceConfiguration = _vsc;
				VideoEncoderConfiguration[] _vecs = null;
				yield return session.GetCompatibleVideoEncoderConfigurations(profile.token).Handle(x => _vecs = x);
				foreach(var _vec in _vecs.OrderBy(x=>x.UseCount)){
					VideoEncoderConfigurationOptions _vec_opt = null;
					yield return session.GetVideoEncoderConfigurationOptions(_vec.token, null).Handle(x => _vec_opt = x);
					DebugHelper.Assert(_vec_opt != null);

					switch(currentEncoder.encoding){
						case VideoEncoder.Encoding.H264:
							if(_vec_opt.H264!=null){
								if(_vec_opt.H264.ResolutionsAvailable.Any(x=>x.Height==currentResolution.height && x.Width==currentResolution.width)){
									Exception _error = null;
									_vec.Encoding = VideoEncoding.H264;
									var frameRateRange = _vec_opt.H264.FrameRateRange;
									var _frameRate = frameRate;
									if (_frameRate < frameRateRange.Min) {
										_frameRate = frameRateRange.Min;
									} else if (_frameRate > frameRateRange.Max) {
										_frameRate = frameRateRange.Max;
									}
									_vec.RateControl.FrameRateLimit = _frameRate;
									
									var encodingIntervalRange = _vec_opt.H264.EncodingIntervalRange;
									var _encodingInterval = encodingInterval;
									if (_encodingInterval < encodingIntervalRange.Min) {
										_encodingInterval = encodingIntervalRange.Min;
									} else if (_encodingInterval > encodingIntervalRange.Max) {
										_encodingInterval = encodingIntervalRange.Max;
									}
									_vec.RateControl.EncodingInterval = _encodingInterval;

									_vec.Resolution = new med::VideoResolution() {
										Width = currentResolution.width,
										Height = currentResolution.height
									};
									yield return media.SetVideoEncoderConfiguration(_vec, true).Idle().HandleError(x => _error = x);
									if (_error != null) {
										break;
									}
									yield return session.AddVideoEncoderConfiguration(profile.token, _vec.token).Idle().HandleError(x=>_error = x);
									if (_error == null) {
										profile.VideoEncoderConfiguration = _vec;
									}
								}
							}
							break;
						case VideoEncoder.Encoding.JPEG:
							if (_vec_opt.JPEG != null) {
								if (_vec_opt.JPEG.ResolutionsAvailable.Any(x => x.Height == currentResolution.height && x.Width == currentResolution.width)) {
									Exception _error = null;
									_vec.Encoding = VideoEncoding.JPEG;
									var frameRateRange = _vec_opt.JPEG.FrameRateRange;
									var _frameRate = frameRate;
									if (_frameRate < frameRateRange.Min) {
										_frameRate = frameRateRange.Min;
									} else if (_frameRate > frameRateRange.Max) {
										_frameRate = frameRateRange.Max;
									}
									_vec.RateControl.FrameRateLimit = _frameRate;

									var encodingIntervalRange = _vec_opt.JPEG.EncodingIntervalRange;
									var _encodingInterval = encodingInterval;
									if (_encodingInterval < encodingIntervalRange.Min) {
										_encodingInterval = encodingIntervalRange.Min;
									} else if (_encodingInterval > encodingIntervalRange.Max) {
										_encodingInterval = encodingIntervalRange.Max;
									}
									_vec.RateControl.EncodingInterval = _encodingInterval;
									
									_vec.Resolution = new med::VideoResolution() {
										Width = currentResolution.width,
										Height = currentResolution.height
									};
									yield return media.SetVideoEncoderConfiguration(_vec, true).Idle().HandleError(x => _error = x);
									if (_error != null) {
										break;
									}
									yield return session.AddVideoEncoderConfiguration(profile.token, _vec.token).Idle().HandleError(x=>_error = x);
									if (_error == null) {
										profile.VideoEncoderConfiguration = _vec;
									}
								}
							}
							break;
						case VideoEncoder.Encoding.MPEG4:
							if (_vec_opt.MPEG4 != null) {
								if (_vec_opt.MPEG4.ResolutionsAvailable.Any(x => x.Height == currentResolution.height && x.Width == currentResolution.width)) {
									Exception _error = null;
									_vec.Encoding = VideoEncoding.MPEG4;
									var frameRateRange = _vec_opt.MPEG4.FrameRateRange;
									var _frameRate = frameRate;
									if (_frameRate < frameRateRange.Min) {
										_frameRate = frameRateRange.Min;
									} else if (_frameRate > frameRateRange.Max) {
										_frameRate = frameRateRange.Max;
									}
									_vec.RateControl.FrameRateLimit = _frameRate;

									var encodingIntervalRange = _vec_opt.MPEG4.EncodingIntervalRange;
									var _encodingInterval = encodingInterval;
									if (_encodingInterval < encodingIntervalRange.Min) {
										_encodingInterval = encodingIntervalRange.Min;
									} else if (_encodingInterval > encodingIntervalRange.Max) {
										_encodingInterval = encodingIntervalRange.Max;
									}
									_vec.RateControl.EncodingInterval = _encodingInterval;

									_vec.Resolution = new med::VideoResolution() {
										Width = currentResolution.width,
										Height = currentResolution.height
									};
									yield return media.SetVideoEncoderConfiguration(_vec, true).Idle().HandleError(x => _error = x);
									if (_error != null) {
										break;
									}
									yield return session.AddVideoEncoderConfiguration(profile.token, _vec.token).Idle().HandleError(x=>_error = x);
									if (_error == null) {
										profile.VideoEncoderConfiguration = _vec;
									}
								}
							}
							break;				
					}
					if (profile.VideoEncoderConfiguration != null) {
						break;
					}
				}
				if (profile.VideoEncoderConfiguration != null) {
					break;
				} else {
					yield return media.RemoveVideoSourceConfiguration(profile.token).Idle();
					profile.VideoSourceConfiguration = null;
				}
			}


			if (m_metadata.current) {
				yield return session.AddDefaultMetadata(profile).Idle();
			}

			yield return Observable.Concat(LoadImpl(session, null));

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public override void RevertChanges() {
			m_currentResolution.Revert();
			m_currentEncoder.Revert();
		}

		private ChangeTrackingProperty<String> m_channelName = new ChangeTrackingProperty<String>();
		private ChangeTrackingProperty<VideoEncoder> m_currentEncoder = new ChangeTrackingProperty<VideoEncoder>();
		private ChangeTrackingProperty<VideoResolution> m_currentResolution = new ChangeTrackingProperty<VideoResolution>();
		private ChangeTrackingProperty<int> m_frameRate = new ChangeTrackingProperty<int>();
		private ChangeTrackingProperty<int> m_encodingInterval = new ChangeTrackingProperty<int>();
		

		private ChangeTrackingProperty<bool> m_metadata = new ChangeTrackingProperty<bool>();
		public bool metadata {
			get {
				return m_metadata.current;
			}
			set {
				if (m_metadata.current != value) {
					m_metadata.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.metadata);
				}
			}
		}
		public string mediaUri { get; private set; }
		public IEnumerable<VideoResolution> supportedResolutions { get; private set; }
		public VideoResolution currentResolution {
			get {
				return m_currentResolution.current;
			}
			set{
				if(m_currentResolution.current != value){
					m_currentResolution.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.currentResolution);
				}
			}
		}

		public IEnumerable<VideoEncoder> supportedEncoders { get; private set; }
		public VideoEncoder currentEncoder {
			get {
				return m_currentEncoder.current;
			}
			set {
				if (m_currentEncoder.current != value) {
					m_currentEncoder.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.currentEncoder);
				}
			}
		}
		public int frameRate {
			get {
				return m_frameRate.current;
			}
			set {
				if (m_frameRate.current != value) {
					m_frameRate.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.frameRate);
				}
			}
		}
		
		public int encodingInterval {
			get {
				return m_encodingInterval.current;
			}
			set {
				if (m_encodingInterval.current != value) {
					m_encodingInterval.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.encodingInterval);
				}
			}
		}

		public string channelName {
			get {
				return m_channelName.current;
			}
			set {
				if (m_channelName.current != value) {
					m_channelName.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.channelName);
				}
			}
		}
		
		public int maxFrameRate { get; private set; }
		public int minFrameRate { get; private set; }
		public int maxEncodingInterval { get; private set; }
		public int minEncodingInterval { get; private set; }
		public int bitrate { get; private set; }
		//public int maxBitrate { get; private set; }
		//public int minBitrate { get; private set; }
	}
}
