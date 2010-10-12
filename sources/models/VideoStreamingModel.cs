using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using onvif.services.media;
using nvc.onvif;
using nvc.utils;

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

		public int maxBitrate;
		public int minBitrate;
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
						encoder.maxBitrate = options.H264.FrameRateRange.Max;
						encoder.minBitrate = options.H264.FrameRateRange.Min;						
					}
					break;

				case VideoEncoding.JPEG:
					if (options.JPEG != null) {
						encoder = new VideoEncoder(VideoEncoder.Encoding.JPEG);
						encoder.maxBitrate = options.JPEG.FrameRateRange.Max;
						encoder.minBitrate = options.JPEG.FrameRateRange.Min;						
					}
					break;
				case VideoEncoding.MPEG4:
					if (options.MPEG4 != null) {
						encoder = new VideoEncoder(VideoEncoder.Encoding.MPEG4);
						encoder.maxBitrate = options.MPEG4.FrameRateRange.Max;
						encoder.minBitrate = options.MPEG4.FrameRateRange.Min;						
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
			
			var vec = profile.VideoEncoderConfiguration;
			if (vec == null) {
				//add default video encoder
				yield return session.AddDefaultVideoEncoder(profile.token).Handle(x => vec = x);
				profile.VideoEncoderConfiguration = vec;
			}

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			VideoEncoderConfigurationOptions options = null;
			yield return media.GetVideoEncoderConfigurationOptions().Handle(x => options = x);
			DebugHelper.Assert(options != null);

			VideoEncoderConfiguration[] vecs = null;
			yield return media.GetVideoEncoderConfigurations().Handle(x => vecs = x);
			DebugHelper.Assert(vecs != null);
			
			
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

			m_currentResolution.SetBoth(resolutions[new Tuple<int,int>(vec.Resolution.Width, vec.Resolution.Height)]);
			m_currentEncoder.SetBoth(encoders[convert_encoding(vec.Encoding)]);
			this.frameRate = vec.RateControl.FrameRateLimit;
			this.bitrate = vec.RateControl.BitrateLimit;
			this.supportedEncoders = encoders.Values;
			this.supportedResolutions = resolutions.Values;
			
			NotifyPropertyChanged(x => x.currentResolution);
			NotifyPropertyChanged(x => x.currentEncoder);
			NotifyPropertyChanged(x => x.frameRate);
			NotifyPropertyChanged(x => x.bitrate);
			NotifyPropertyChanged(x => x.supportedEncoders);
			NotifyPropertyChanged(x => x.supportedResolutions);

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


		protected override IEnumerable<IObservable<Object>> ApplyChangesImpl(Session session, IObserver<VideoStreamingModel> observer) {
			if (!isModified) {
				yield break;
			}
			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			DebugHelper.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			DebugHelper.Assert(profile != null);

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			DebugHelper.Assert(media != null);

			VideoEncoderConfigurationOptions options = null;
			yield return media.GetVideoEncoderConfigurationOptions().Handle(x => options = x);
			DebugHelper.Assert(options != null);

			VideoEncoderConfiguration[] vecs = null;
			yield return session.GetVideoEncoderConfigurations().Handle(x => vecs = x);
			DebugHelper.Assert(vecs != null);

			var new_vec = new VideoEncoderConfiguration();
			switch(currentEncoder.encoding){
				case VideoEncoder.Encoding.H264:
					new_vec.Encoding = VideoEncoding.H264;
					break;
				case VideoEncoder.Encoding.JPEG:
					new_vec.Encoding = VideoEncoding.JPEG;
					break;
				case VideoEncoder.Encoding.MPEG4:
					new_vec.Encoding = VideoEncoding.MPEG4;
					break;
			}

			var vec = vecs.Where(x => x.Encoding == new_vec.Encoding).FirstOrDefault();
			vec.Resolution.Width = currentResolution.width;
			vec.Resolution.Height = currentResolution.height;

			yield return media.SetVideoEncoderConfiguration(vec, true).Idle();

			if(profile.VideoEncoderConfiguration == null || profile.VideoEncoderConfiguration.token != vec.token){
				yield return media.AddVideoEncoderConfiguration(profile.token, vec.token).Idle();
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

		private ChangeTrackingProperty<VideoEncoder> m_currentEncoder = new ChangeTrackingProperty<VideoEncoder>();
		private ChangeTrackingProperty<VideoResolution> m_currentResolution = new ChangeTrackingProperty<VideoResolution>();
		
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
		public int frameRate { get; private set; }
		public int maxFrameRate { get; private set; }
		public int minFrameRate { get; private set; }
		public int bitrate { get; private set; }
		public int maxBitrate { get; private set; }
		public int minBitrate { get; private set; }
	}
}
