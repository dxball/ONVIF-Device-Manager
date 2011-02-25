﻿#region License and Terms
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

using onvif.services.media;
using onvif;

namespace odm.onvif {
	public class MediaObservable:IDisposable {
		Media m_proxy;
		public MediaObservable(Media proxy) {
			m_proxy = proxy;
		}

		public IObservable<MediaUri> GetSnapshotUri(ProfileToken profileToken) {
			var request = new GetSnapshotUriRequest() {
				ProfileToken = profileToken.value,
			};
			var asyncOp = Observable.FromAsyncPattern<GetSnapshotUriRequest, GetSnapshotUriResponse>(m_proxy.BeginGetSnapshotUri, m_proxy.EndGetSnapshotUri);
			return asyncOp(request).Select(x => x.MediaUri);
		}

		//Profile functions

		public IObservable<Profile[]> GetProfiles() {
			var request = new GetProfilesRequest();
			var asyncOp = Observable.FromAsyncPattern<GetProfilesRequest, GetProfilesResponse>(m_proxy.BeginGetProfiles, m_proxy.EndGetProfiles);
			return asyncOp(request).Select(x => x.Profiles);
		}

		public IObservable<Profile> GetProfile(ProfileToken profileToken) {
			var request = new GetProfileRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<GetProfileRequest, GetProfileResponse>(m_proxy.BeginGetProfile, m_proxy.EndGetProfile);
			return asyncOp(request).Select(x => x.Profile);
		}

		public IObservable<Profile> CreateProfile(string profileName, ProfileToken profileToken) {
			var request = new CreateProfileRequest(profileName, profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<CreateProfileRequest, CreateProfileResponse>(m_proxy.BeginCreateProfile, m_proxy.EndCreateProfile);
			return asyncOp(request).Select(x => x.Profile);
		}

		public IObservable<Unit> DeleteProfile(ProfileToken profileToken) {
			var request = new DeleteProfileRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<DeleteProfileRequest, DeleteProfileResponse>(m_proxy.BeginDeleteProfile, m_proxy.EndDeleteProfile);
			return asyncOp(request).Select(x => new Unit());			
		}

		//Video source functions

		public IObservable<VideoSource[]> GetVideoSources() {
			var request = new GetVideoSourcesRequest();
			var asyncOp = Observable.FromAsyncPattern<GetVideoSourcesRequest, GetVideoSourcesResponse>(m_proxy.BeginGetVideoSources, m_proxy.EndGetVideoSources);
			return asyncOp(request).Select(x => x.VideoSources);
		}

		public IObservable<VideoSourceConfiguration[]> GetVideoSourceConfigurations() {
			var request = new GetVideoSourceConfigurationsRequest();
			var asyncOp = Observable.FromAsyncPattern<GetVideoSourceConfigurationsRequest, GetVideoSourceConfigurationsResponse>(m_proxy.BeginGetVideoSourceConfigurations, m_proxy.EndGetVideoSourceConfigurations);
			return asyncOp(request).Select(x => x.Configurations);
		}

		public IObservable<VideoSourceConfiguration[]> GetCompatibleVideoSourceConfigurations(ProfileToken profileToken) {
			var request = new GetCompatibleVideoSourceConfigurationsRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<GetCompatibleVideoSourceConfigurationsRequest, GetCompatibleVideoSourceConfigurationsResponse>(m_proxy.BeginGetCompatibleVideoSourceConfigurations, m_proxy.EndGetCompatibleVideoSourceConfigurations);
			return asyncOp(request).Select(x => x.Configurations);
		}

		public IObservable<Unit> AddVideoSourceConfiguration(ProfileToken profileToken, VideoSourceConfigurationToken vscToken) {
			var request = new AddVideoSourceConfigurationRequest(profileToken.value, vscToken.value);
			var asyncOp = Observable.FromAsyncPattern<AddVideoSourceConfigurationRequest,AddVideoSourceConfigurationResponse>(m_proxy.BeginAddVideoSourceConfiguration, m_proxy.EndAddVideoSourceConfiguration);
			return asyncOp(request).Select(x => new Unit());				
		}

		public IObservable<Unit> RemoveVideoSourceConfiguration(ProfileToken profileToken) {
			var request = new RemoveVideoSourceConfigurationRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<RemoveVideoSourceConfigurationRequest, RemoveVideoSourceConfigurationResponse>(m_proxy.BeginRemoveVideoSourceConfiguration, m_proxy.EndRemoveVideoSourceConfiguration);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<Unit> SetVideoSourceConfiguration(VideoSourceConfiguration configuration, bool forcePersistance) {
			var request = new SetVideoSourceConfigurationRequest(configuration, forcePersistance);
			var asyncOp = Observable.FromAsyncPattern<SetVideoSourceConfigurationRequest,SetVideoSourceConfigurationResponse>(m_proxy.BeginSetVideoSourceConfiguration, m_proxy.EndSetVideoSourceConfiguration);
			return asyncOp(request).Select(x => new Unit());				
		}

		public IObservable<VideoSourceConfigurationOptions> GetVideoSourceConfigurationOptions(VideoSourceConfigurationToken vscToken, ProfileToken profileToken) {
			var request = new GetVideoSourceConfigurationOptionsRequest(vscToken.value, profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<GetVideoSourceConfigurationOptionsRequest, GetVideoSourceConfigurationOptionsResponse>(m_proxy.BeginGetVideoSourceConfigurationOptions, m_proxy.EndGetVideoSourceConfigurationOptions);
			return asyncOp(request).Select(x => x.Options);
		}

		//Video Encoder functions

		public IObservable<Unit> AddVideoEncoderConfiguration(ProfileToken profileToken, VideoEncoderConfigurationToken vecToken) {
			var request = new AddVideoEncoderConfigurationRequest(profileToken.value, vecToken.value);
			var asyncOp = Observable.FromAsyncPattern<AddVideoEncoderConfigurationRequest,AddVideoEncoderConfigurationResponse>(m_proxy.BeginAddVideoEncoderConfiguration, m_proxy.EndAddVideoEncoderConfiguration);
			return asyncOp(request).Select(x => new Unit());				
		}

		public IObservable<Unit> RemoveVideoEncoderConfiguration(ProfileToken profileToken) {
			var request = new RemoveVideoEncoderConfigurationRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<RemoveVideoEncoderConfigurationRequest, RemoveVideoEncoderConfigurationResponse>(m_proxy.BeginRemoveVideoEncoderConfiguration, m_proxy.EndRemoveVideoEncoderConfiguration);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<VideoEncoderConfiguration[]> GetCompatibleVideoEncoderConfigurations(ProfileToken profileToken) {
			var request = new GetCompatibleVideoEncoderConfigurationsRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<GetCompatibleVideoEncoderConfigurationsRequest, GetCompatibleVideoEncoderConfigurationsResponse>(m_proxy.BeginGetCompatibleVideoEncoderConfigurations, m_proxy.EndGetCompatibleVideoEncoderConfigurations);
			return asyncOp(request).Select(x => x.Configurations);
		}

		public IObservable<VideoEncoderConfiguration[]> GetVideoEncoderConfigurations() {
			var request = new GetVideoEncoderConfigurationsRequest();
			var asyncOp = Observable.FromAsyncPattern<GetVideoEncoderConfigurationsRequest, GetVideoEncoderConfigurationsResponse>(m_proxy.BeginGetVideoEncoderConfigurations, m_proxy.EndGetVideoEncoderConfigurations);
			return asyncOp(request).Select(x => x.Configurations);
		}

		public IObservable<Unit> SetVideoEncoderConfiguration(VideoEncoderConfiguration configuration, bool forcePersistance) {
			var request = new SetVideoEncoderConfigurationRequest(configuration, forcePersistance);
			var asyncOp = Observable.FromAsyncPattern<SetVideoEncoderConfigurationRequest,SetVideoEncoderConfigurationResponse>(m_proxy.BeginSetVideoEncoderConfiguration, m_proxy.EndSetVideoEncoderConfiguration);
			return asyncOp(request).Select(x => new Unit());				
		}

		public IObservable<VideoEncoderConfigurationOptions> GetVideoEncoderConfigurationOptions(VideoEncoderConfigurationToken vecToken, ProfileToken profileToken) {
			var request = new GetVideoEncoderConfigurationOptionsRequest(){
				ConfigurationToken = (vecToken==null ? null : vecToken.value),
				ProfileToken = (profileToken==null ? null : profileToken.value)
			};
			var asyncOp = Observable.FromAsyncPattern<GetVideoEncoderConfigurationOptionsRequest,GetVideoEncoderConfigurationOptionsResponse>(m_proxy.BeginGetVideoEncoderConfigurationOptions, m_proxy.EndGetVideoEncoderConfigurationOptions);
			return asyncOp(request).Select(x => x.Options);				
		}

		public IObservable<VideoEncoderConfigurationOptions> GetVideoEncoderConfigurationOptions() {
			return GetVideoEncoderConfigurationOptions(null, null);
		}


		public IObservable<MetadataConfiguration[]> GetCompatibleMetadataConfigurations(ProfileToken profileToken) {
			var request = new GetCompatibleMetadataConfigurationsRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<GetCompatibleMetadataConfigurationsRequest, GetCompatibleMetadataConfigurationsResponse>(m_proxy.BeginGetCompatibleMetadataConfigurations, m_proxy.EndGetCompatibleMetadataConfigurations);
			return asyncOp(request).Select(x => x.Configurations);
		}

		public IObservable<Unit> AddMetadataConfiguration(ProfileToken profileToken, MetadataConfigurationToken mdcToken) {
			var request = new AddMetadataConfigurationRequest(profileToken.value, mdcToken.value);
			var asyncOp = Observable.FromAsyncPattern<AddMetadataConfigurationRequest,AddMetadataConfigurationResponse>(m_proxy.BeginAddMetadataConfiguration, m_proxy.EndAddMetadataConfiguration);
			return asyncOp(request).Select(x => new Unit());				
		}

		public IObservable<MetadataConfigurationOptions> GetMetadataConfigurationOptions(MetadataConfigurationToken mdcToken, ProfileToken profileToken) {
			var request = new GetMetadataConfigurationOptionsRequest(mdcToken.value, profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<GetMetadataConfigurationOptionsRequest, GetMetadataConfigurationOptionsResponse>(m_proxy.BeginGetMetadataConfigurationOptions, m_proxy.EndGetMetadataConfigurationOptions);
			return asyncOp(request).Select(x => x.Options);
		}

		public IObservable<Unit> RemoveMetadataConfiguration(ProfileToken profileToken) {
			var request = new RemoveMetadataConfigurationRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<RemoveMetadataConfigurationRequest, RemoveMetadataConfigurationResponse>(m_proxy.BeginRemoveMetadataConfiguration, m_proxy.EndRemoveMetadataConfiguration);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<Unit> SetMetadataConfiguration(MetadataConfiguration configuration, bool forcePersistance) {
			var request = new SetMetadataConfigurationRequest(configuration, forcePersistance);
			var asyncOp = Observable.FromAsyncPattern<SetMetadataConfigurationRequest,SetMetadataConfigurationResponse>(m_proxy.BeginSetMetadataConfiguration, m_proxy.EndSetMetadataConfiguration);
			return asyncOp(request).Select(x => new Unit());				
		}

		public IObservable<MediaUri> GetStreamUri(StreamSetup streamSetup, ProfileToken profileToken) {
			var request = new GetStreamUriRequest() {
				ProfileToken = profileToken.value,
				StreamSetup = streamSetup
			};
			var asyncOp = Observable.FromAsyncPattern<GetStreamUriRequest, GetStreamUriResponse>(m_proxy.BeginGetStreamUri, m_proxy.EndGetStreamUri);
			return asyncOp(request).Select(x => x.MediaUri);				
		}

		//Video Analytics functions

		public IObservable<Unit> AddVideoAnalyticsConfiguration(ProfileToken profileToken, VideoAnalyticsConfigurationToken vacToken) {
			var request = new AddVideoAnalyticsConfigurationRequest(profileToken.value, vacToken.value);
			var asyncOp = Observable.FromAsyncPattern<AddVideoAnalyticsConfigurationRequest, AddVideoAnalyticsConfigurationResponse>(m_proxy.BeginAddVideoAnalyticsConfiguration, m_proxy.EndAddVideoAnalyticsConfiguration);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<Unit> RemoveVideoAnalyticsConfiguration(ProfileToken profileToken) {
			var request = new RemoveVideoAnalyticsConfigurationRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<RemoveVideoAnalyticsConfigurationRequest, RemoveVideoAnalyticsConfigurationResponse>(m_proxy.BeginRemoveVideoAnalyticsConfiguration, m_proxy.EndRemoveVideoAnalyticsConfiguration);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<VideoAnalyticsConfiguration[]> GetCompatibleVideoAnalyticsConfigurations(ProfileToken profileToken) {
			var request = new GetCompatibleVideoAnalyticsConfigurationsRequest(profileToken.value);
			var asyncOp = Observable.FromAsyncPattern<GetCompatibleVideoAnalyticsConfigurationsRequest, GetCompatibleVideoAnalyticsConfigurationsResponse>(m_proxy.BeginGetCompatibleVideoAnalyticsConfigurations, m_proxy.EndGetCompatibleVideoAnalyticsConfigurations);
			return asyncOp(request).Select(x => x.Configurations);
		}

		public IObservable<VideoAnalyticsConfiguration[]> GetVideoAnalyticsConfigurations() {
			var request = new GetVideoAnalyticsConfigurationsRequest();
			var asyncOp = Observable.FromAsyncPattern<GetVideoAnalyticsConfigurationsRequest, GetVideoAnalyticsConfigurationsResponse>(m_proxy.BeginGetVideoAnalyticsConfigurations, m_proxy.EndGetVideoAnalyticsConfigurations);
			return asyncOp(request).Select(x => x.Configurations);
		}

		public IObservable<Unit> SetVideoAnalyticsConfiguration(VideoAnalyticsConfiguration configuration, bool forcePersistance) {
			var request = new SetVideoAnalyticsConfigurationRequest(configuration, forcePersistance);
			var asyncOp = Observable.FromAsyncPattern<SetVideoAnalyticsConfigurationRequest, SetVideoAnalyticsConfigurationResponse>(m_proxy.BeginSetVideoAnalyticsConfiguration, m_proxy.EndSetVideoAnalyticsConfiguration);
			return asyncOp(request).Select(x => new Unit());
		}
		
		public void Dispose() {
			//m_proxy.Close();
		}
		public Media Services {
			get {
				return m_proxy;
			}
		}
	}
}