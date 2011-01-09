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
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;

using odm.onvif;
using dev = onvif.services.device;
using med = onvif.services.media;

namespace odm.models {
	//public partial class Channel {

		

		
	//    static void SetVideoSettingsJPEG(Session session, Channel channel) {

	//        med::VideoResolution resolution = new med::VideoResolution() {
	//            Width = channel.encoder.resolution.width,
	//            Height = channel.encoder.resolution.height
	//        };

	//        //get VEC for the channel
	//        var profiles = session.media.GetProfiles().First();
	//        var profile = profiles.Where(p => p.token == channel.GetProfileToken()).SingleOrDefault();
	//        //var profile = session.media.GetProfile(channel.GetProfileToken()).SingleOrDefault();
	//        if (profile == null) {
	//            //TODO: create profile ...
	//        }
	//        //TODO: first check if we can use current profile's vec

	//        var vecs = session.media.GetCompatibleVideoEncoderConfigurations(channel.GetProfileToken()).First();
	//        var vec = vecs.Where(x => x.Encoding == med::VideoEncoding.JPEG && x.Resolution.Height == resolution.Height && x.Resolution.Width == resolution.Width).FirstOrDefault();
	//        med::VideoEncoderConfigurationOptions options = null;

	//        if (vec == null) {
	//            foreach (var v in vecs) {
	//                var _options = session.media.GetVideoEncoderConfigurationOptions(v.token, null).First();
	//                if (_options.JPEG == null) {
	//                    continue;
	//                }
	//                if (!_options.JPEG.ResolutionsAvailable.Any(x => x.Height == resolution.Height && x.Width == resolution.Width)) {
	//                    continue;
	//                }
	//                options = _options;
	//                vec = v;
	//                break;
	//            };

	//            if (options == null) {
	//                throw new Exception("configuration not supported");
	//            }
	//            vec.Resolution.Height = resolution.Height;
	//            vec.Resolution.Width = resolution.Width;
	//            vec.Encoding = med::VideoEncoding.JPEG;
	//        }

	//        session.media.SetVideoEncoderConfiguration(vec, true).First();
	//        if (vec.token != profile.VideoEncoderConfiguration.token) {
	//            session.media.RemoveVideoEncoderConfiguration(profile.token).First();
	//            session.media.AddVideoEncoderConfiguration(profile.token, vec.token).First();
	//        }
	//    }

	//    static void SetVideoSettingsMpeg4(Session session, Channel channel) {

	//        med::VideoResolution resolution = new med::VideoResolution() {
	//            Width = channel.encoder.resolution.width,
	//            Height = channel.encoder.resolution.height
	//        };

	//        //get VEC for the channel
	//        var profiles = session.media.GetProfiles().First();
	//        var profile = profiles.Where(p => p.token == channel.GetProfileToken()).SingleOrDefault();
	//        //var profile = session.media.GetProfile(channel.GetProfileToken()).SingleOrDefault();
	//        if (profile == null) {
	//            //TODO: create profile ...
	//        }
	//        //TODO: first check if we can use current profile's vec

	//        var vecs = session.media.GetCompatibleVideoEncoderConfigurations(channel.GetProfileToken()).First();
	//        var vec = vecs.Where(x => x.Encoding == med::VideoEncoding.MPEG4 && x.Resolution.Height == resolution.Height && x.Resolution.Width == resolution.Width).FirstOrDefault();
	//        med::VideoEncoderConfigurationOptions options = null;

	//        if (vec == null) {
	//            foreach (var v in vecs) {
	//                var _options = session.media.GetVideoEncoderConfigurationOptions(v.token, null).First();
	//                if (_options.MPEG4 == null) {
	//                    continue;
	//                }
	//                if (!_options.MPEG4.ResolutionsAvailable.Any(x => x.Height == resolution.Height && x.Width == resolution.Width)) {
	//                    continue;
	//                }
	//                options = _options;
	//                vec = v;
	//                break;
	//            };

	//            if (options == null) {
	//                throw new Exception("configuration not supported");
	//            }
	//            vec.Resolution.Height = resolution.Height;
	//            vec.Resolution.Width = resolution.Width;
	//            vec.Encoding = med::VideoEncoding.MPEG4;
	//        }

	//        if (vec.MPEG4 == null) {
	//            vec.MPEG4 = new med::Mpeg4Configuration();
	//            vec.MPEG4.Mpeg4Profile = options.MPEG4.Mpeg4ProfilesSupported[0];
	//            vec.MPEG4.GovLength = 10;
	//        }
	//        session.media.SetVideoEncoderConfiguration(vec, true).First();
	//        if (vec.token != profile.VideoEncoderConfiguration.token) {
	//            session.media.RemoveVideoEncoderConfiguration(profile.token).First();
	//            session.media.AddVideoEncoderConfiguration(profile.token, vec.token).First();
	//        }
	//    }

	//    static void SetVideoSettingsH264(Session session, Channel channel) {

	//        med::VideoResolution resolution = new med::VideoResolution() {
	//            Width = channel.encoder.resolution.width,
	//            Height = channel.encoder.resolution.height
	//        };

	//        //get VEC for the channel
	//        var profiles = session.media.GetProfiles().First();
	//        var profile = profiles.Where(p => p.token == channel.GetProfileToken()).SingleOrDefault();
	//        //var profile = session.media.GetProfile(channel.GetProfileToken()).SingleOrDefault();
	//        if (profile == null) {
	//            //TODO: create profile ...
	//        }
	//        //TODO: first check if we can use current profile's vec

	//        var vecs = session.media.GetCompatibleVideoEncoderConfigurations(channel.GetProfileToken()).First();
	//        var vec = vecs.Where(x => x.Encoding == med::VideoEncoding.H264 && x.Resolution.Height == resolution.Height && x.Resolution.Width == resolution.Width).FirstOrDefault();
	//        med::VideoEncoderConfigurationOptions options = null;

	//        if (vec == null) {
	//            foreach (var v in vecs) {
	//                var _options = session.media.GetVideoEncoderConfigurationOptions(v.token, null).First();
	//                if (_options.H264 == null) {
	//                    continue;
	//                }
	//                if (!_options.H264.ResolutionsAvailable.Any(x => x.Height == resolution.Height && x.Width == resolution.Width)) {
	//                    continue;
	//                }
	//                options = _options;
	//                vec = v;
	//                break;
	//            };

	//            if (options == null) {
	//                throw new Exception("configuration not supported");
	//            }
	//            vec.Resolution.Height = resolution.Height;
	//            vec.Resolution.Width = resolution.Width;
	//            vec.Encoding = med::VideoEncoding.H264;
	//        }

	//        if (vec.H264 == null) {
	//            vec.H264 = new med::H264Configuration();
	//            vec.H264.H264Profile = options.H264.H264ProfilesSupported[0];
	//            vec.H264.GovLength = 10;
	//        }
	//        session.media.SetVideoEncoderConfiguration(vec, true).First();
	//        if (vec.token != profile.VideoEncoderConfiguration.token) {
	//            session.media.RemoveVideoEncoderConfiguration(profile.token).First();
	//            session.media.AddVideoEncoderConfiguration(profile.token, vec.token).First();
	//        }
	//    }

	//    static void SetVideoSettings(Session session, Channel channel) {

	//        //var options = session.media.GetVideoEncoderConfigurationOptions().First();

	//        switch (channel.encoder.encoding) {
	//            case VideoEncoder.Encoding.H264:
	//                SetVideoSettingsH264(session, channel);
	//                break;

	//            case VideoEncoder.Encoding.JPEG:
	//                SetVideoSettingsJPEG(session, channel);
	//                break;

	//            case VideoEncoder.Encoding.MPEG4:
	//                SetVideoSettingsMpeg4(session, channel);
	//                break;
	//        }
	//    }


	//    public IObservable<Unit> SetVideoSettings(Session session) {
	//        return Observable.Start(() => {
	//            SetVideoSettings(session, this);
	//        });
	//    }

	
		
	//}

	
}
