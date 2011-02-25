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
using odm.controllers;
using odm.models;
using odm.utils.controlsUIProvider;
using System.Threading;
using onvif.services.media;
using odm.onvif;
using onvif;

namespace odm.controllers {
	public class PropertyProfileEditorController : BasePropertyController {
		IDisposable _subscription;
		IDisposable _createProfileSubscription;

		protected override void LoadControl() {
			_subscription = CurrentSession.GetProfiles().ObserveOn(SynchronizationContext.Current).Subscribe(arg => {
				List<Profile> plst = arg.Where(res => { 
					if(res.VideoSourceConfiguration != null)
						return res.VideoSourceConfiguration.SourceToken == CurrentChannel.sourceToken;
					return true;
				}).ToList();
				
				UIProvider.Instance.GetProfileEditorProvider().InitView(plst, NvcHelper.GetChannelProfileToken(CurrentChannel.sourceToken), CreateNewProfile, RenameProfile, SelectProfile, DeleteProfile);
			}, err => {
				OnCriticalError(err);
			});
			
		}
		
		public void CreateNewProfile() {
			var profToken =  new ProfileToken(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
			var profName = String.Concat(CurrentChannel.sourceToken.value,"-",DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));	
			_createProfileSubscription = CurrentSession.CreateDefaultProfile(profName,profToken, CurrentChannel.sourceToken)
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(argprof => {
					UIProvider.Instance.GetProfileEditorProvider().AddProfile(argprof);
				}, err => {
					ApplyError(err);
				}, () => {
					ApplyCompleate();
				});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
		public void RenameProfile(Profile pfl) {
			//_createProfileSubscription = CurrentSession.r (uniqueString, new ProfileToken(uniqueString), CurrentChannel.sourceToken)
			//    .ObserveOn(SynchronizationContext.Current)
			//    .Subscribe(argprof => {
			//        UIProvider.Instance.GetProfileEditorProvider().AddProfile(argprof);
			//    }, err => {
			//        ApplyError(err);
			//    }, () => {
			//        ApplyCompleate();
			//    });
			//OnApply(InfoFormStrings.Instance.applyChanges);
		}
		public void SelectProfile(Profile pfl) {
			WorkflowController.Instance.GetMainFrameController().ReloadModel(pfl, CurrentChannel.sourceToken);
		}

		public void DeleteProfile(Profile pfl) {
			UIProvider.Instance.GetProfileEditorProvider().DeleteProfile(pfl);
			_createProfileSubscription = CurrentSession.DeleteProfile(pfl.token)
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe( res => {
				}, err => {
					ApplyError(err);
				}, () => {
					ApplyCompleate();
				});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
		
		protected override void ApplyChanges() { }
		protected override void CancelChanges() { }
		public override void ReleaseAll() {
			if (_subscription != null)
				_subscription.Dispose();
			if (_createProfileSubscription != null)
				_createProfileSubscription.Dispose();
			UIProvider.Instance.ReleaseProfileEditorProvider();
		}
	}
}
