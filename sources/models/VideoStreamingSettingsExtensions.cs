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
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;

using nvc.onvif;
using dev = onvif.services.device;
using med = onvif.services.media;

namespace nvc.models {
	public partial class Channel {

		private string GetProfileToken() {
			return "http://ornesis.com/profile/" + m_videoSourceToken;
		}
			
		public IObservable<string> GetStreamUri(Session session){
			var med = session.media;
			var streamSetup = new med::StreamSetup();
			streamSetup.Stream = med::StreamType.RTPUnicast;
			streamSetup.Transport = new med::Transport();
			streamSetup.Transport.Protocol = med::TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;
			return med.GetStreamUri(streamSetup, GetProfileToken()).Select(x=>x.Uri);
		}

		private static VideoEncoder CreateVideoEncoder(med::VideoEncoderConfigurationOptions options, med::VideoEncoding encoding){
			Func<VideoEncoder> factory = () => {
				return null;
			};

			switch (encoding) {
				case med::VideoEncoding.H264:
					factory = () => {
						var encoder = new VideoEncoder(VideoEncoder.Encoding.H264);
						if (options.H264 == null) {
							return encoder;
						}
						encoder.maxBitrate = options.H264.FrameRateRange.Max;
						encoder.minBitrate = options.H264.FrameRateRange.Min;
						encoder.supportedResolutions = options.H264.ResolutionsAvailable.Select(x => new VideoEncoder.Resolution() {
							width = x.Width,
							height = x.Height
						}).ToArray();
						return encoder;
					};
					break;
					
				case med::VideoEncoding.JPEG:
					factory = () => {
						var encoder = new VideoEncoder(VideoEncoder.Encoding.JPEG);
						if (options.JPEG == null) {
							return encoder;
						}
						encoder.maxBitrate = options.JPEG.FrameRateRange.Max;
						encoder.minBitrate = options.JPEG.FrameRateRange.Min;
						encoder.supportedResolutions = options.JPEG.ResolutionsAvailable.Select(x => new VideoEncoder.Resolution() {
							width = x.Width,
							height = x.Height
						}).ToArray();
						return encoder;
					};
					break;
				case med::VideoEncoding.MPEG4:
					factory = () => {
						var encoder = new VideoEncoder(VideoEncoder.Encoding.MPEG4);
						if (options.MPEG4 == null) {
							return encoder;
						}
						encoder.maxBitrate = options.MPEG4.FrameRateRange.Max;
						encoder.minBitrate = options.MPEG4.FrameRateRange.Min;
						encoder.supportedResolutions = options.MPEG4.ResolutionsAvailable.Select(x => new VideoEncoder.Resolution() {
							width = x.Width,
							height = x.Height
						}).ToArray();
						return encoder;
					};
					break;
			}
			return factory();
		}
		private IEnumerable<VideoEncoder> GetAvailableEncodersInternal(med::Media med) {
			var encoders = new List<VideoEncoder>();
			var options = med.GetVideoEncoderConfigurationOptions(new med::GetVideoEncoderConfigurationOptionsRequest()).Options;

			EnumHelper.GetValues<med::VideoEncoding>().ForEach(x => {
				var encoder = CreateVideoEncoder(options, x);
				if (encoder != null) {
					encoders.Add(encoder);
				}
			});

			return encoders;
		}

		public IObservable<IEnumerable<VideoEncoder>> GetAvailableEncoders(Session session) {
			var dev = session.device.Services;
			var med = session.media.Services;
			return Observable.Start(() => {
				return GetAvailableEncodersInternal(med);
			});
		}

		public IObservable<IEnumerable<Channel.Resolution>> GetAvailableResolutions(Session session) {
			var observable = session.media.GetCompatibleVideoSourceConfigurations(GetProfileToken());
			return observable.Select(x => {
				return x.Select(y => {
					return new Channel.Resolution() {
						width = y.Bounds.width,
						height = y.Bounds.height
					};
				}).Distinct();
			});
		}

		private med::Profile CreateDefaultProfile(med::Media med) {
			var profile = med.CreateProfile(new med::CreateProfileRequest(m_videoSourceToken, GetProfileToken())).Profile;
			var vsc_list = med.GetCompatibleVideoSourceConfigurations(new med::GetCompatibleVideoSourceConfigurationsRequest(profile.token)).Configurations;
			var vsc = vsc_list.Where(x=>x.SourceToken == m_videoSourceToken).FirstOrDefault();
			if (vsc != null) {
				med.AddVideoSourceConfiguration(new med::AddVideoSourceConfigurationRequest(profile.token, vsc.token));
				profile.VideoSourceConfiguration = vsc;
			}
			var vec_list = med.GetCompatibleVideoEncoderConfigurations(new med::GetCompatibleVideoEncoderConfigurationsRequest(profile.token)).Configurations;
			var vec = vec_list.Where(x => x.Encoding == med::VideoEncoding.H264).FirstOrDefault();
			if (vec != null) {
				med.AddVideoEncoderConfiguration(new med::AddVideoEncoderConfigurationRequest(profile.token, vec.token));
				profile.VideoEncoderConfiguration = vec;
			}
			return profile;
		}

		public static IObservable<IEnumerable<Channel>> GetChannels(Session session) {
			var dev = session.device.Services;
			var med = session.media.Services;
			return Observable.Start(()=>{
				var video_sources = med.GetVideoSources(new med::GetVideoSourcesRequest()).VideoSources;
				var profiles = med.GetProfiles(new med::GetProfilesRequest()).Profiles;
				var pending_queue = new Queue<Action>();
				var channels = video_sources.Select(x =>{
					var channel =  new Channel() {
						m_videoSourceToken = x.token
					};
					
					var profile_token = channel.GetProfileToken();
					var profile = profiles.Where(p => p.token == profile_token).SingleOrDefault();

					Action<med::Profile> fulfillChannel = _profile => {
						channel.bitrate = _profile.VideoEncoderConfiguration.RateControl.BitrateLimit;
						channel.frameRate = _profile.VideoEncoderConfiguration.RateControl.FrameRateLimit;
						channel.resolution.width = _profile.VideoSourceConfiguration.Bounds.width;
						channel.resolution.height = _profile.VideoSourceConfiguration.Bounds.height;
						channel.name = _profile.Name;
					};

					if (profile != null) {
						fulfillChannel(profile);
						pending_queue.Enqueue(() => {
							var vec = profile.VideoEncoderConfiguration;
							var options = med.GetVideoEncoderConfigurationOptions(new med::GetVideoEncoderConfigurationOptionsRequest(vec.token, profile.token)).Options;
							channel.encoder = CreateVideoEncoder(options, vec.Encoding);
						});
					} else {
						pending_queue.Enqueue(()=>{
							var _profile = channel.CreateDefaultProfile(med);
							fulfillChannel(_profile);
							var vec = _profile.VideoEncoderConfiguration;
							var options = med.GetVideoEncoderConfigurationOptions(new med::GetVideoEncoderConfigurationOptionsRequest(vec.token, _profile.token)).Options;
							channel.encoder = CreateVideoEncoder(options, vec.Encoding);
						});
					}					

					return channel;
				}).ToArray();
				
				pending_queue.AsParallel().ForAll(act=>act());

				return channels;
			});
		}
	}

	public static class VideoStreamingSettingsExtensions {
		

		public static IObservable<NetworkStatus> GetVideoStreamingSettings(this Session session) {

			var dev_proxy = session.device;
			var med_proxy = session.media;

			var video_sources = med_proxy.GetVideoSources().FirstOrDefault();
			var profiles = med_proxy.GetProfiles().FirstOrDefault();

			
			var asyncOp = BatchOperation.Create<NetworkStatus>(batch => {
				var netStat = new NetworkStatus();

				//batch.PutInSlot(proxy.GetNetworkDefaultGateway())
				//    .Subscribe(t => {
				//        if (t.IPv4Address != null && t.IPv4Address.Count() > 0) {
				//            netStat.m_defaultGateway = IPAddress.Parse(t.IPv4Address[0]);
				//        }
				//    });

				batch.Join(dev_proxy.GetDNS())
					.Subscribe(t => {
						if (t.FromDHCP && t.DNSFromDHCP.Count() > 0) {
							netStat.dns = IPAddress.Parse(t.DNSFromDHCP[0].IPv4Address);
						}
					});

				batch.Join(dev_proxy.GetNetworkInterfaces())
					.Subscribe(t => {
						var nic = t.Where(x => x.Enabled).FirstOrDefault();
						var nic_cfg = nic.IPv4.Config;
						//networkSettngs.m_token = t[0].token;
						netStat.mac = PhysicalAddress.Parse(nic.Info.HwAddress.Replace(':', '-'));
						//netStat.m_dhcp = nic.IPv4.Config.DHCP;

						if (nic_cfg.DHCP) {
							netStat.ip = IPAddress.Parse(nic_cfg.FromDHCP.Address);
							netStat.subnetPrefix = nic_cfg.FromDHCP.PrefixLength;
						} else if (nic_cfg.Manual.Count() > 0) {
							netStat.ip = IPAddress.Parse(nic_cfg.Manual[0].Address);
							netStat.subnetPrefix = nic_cfg.Manual[0].PrefixLength;
						}
					}
				);

				return netStat;
			});

			return asyncOp;
		}
	}	
}
