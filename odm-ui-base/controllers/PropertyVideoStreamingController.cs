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
using odm.utils.entities;
using odm.models;
using odm.onvif;
using System.Threading;
using odm.utils;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class PropertyVideoStreamingController : BasePropertyController {
		VideoStreamingModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new VideoStreamingModel(CurrentChannel);
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
				UIProvider.Instance.GetVideoStreamingProvider().InitView(_devModel, dprocinfo, ApplyChanges, CancelChanges);
			}, err => {
				OnCriticalError(err);
			});
		}

		protected override void CancelChanges() {
			_devModel.RevertChanges();
		}
		protected override void ApplyChanges() {
			UIProvider.Instance.ReleaseVideoStreamingProvider();
			WorkflowController.Instance.GetMainFrameController().StopVideoStreaming();
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
				}, err => {
					ApplyError(err);
				}, () => {
					ApplyCompleate();
				});
			OnApply(InfoFormStrings.Instance.applyChanges);
		}
		protected override void ApplyError(Exception err) {
			base.ApplyError(err);

			WorkflowController.Instance.GetMainFrameController().ReloadModel();
		}
		protected override void ApplyCompleate() {
			base.ApplyCompleate();

			WorkflowController.Instance.GetMainFrameController().ReloadModel();

			//WorkflowController.Instance.GetMainFrameController().RefershLikButtonsEnabledState(CurrentChannel);

			////WorkflowController.Instance.GetMainFrameController().StartVideoStreaming(CurrentChannel);

			//var dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
			//UIProvider.Instance.VideoStreamingProvider.InitView(_devModel, dprocinfo, ApplyChanges, CancelChanges);

			//UIProvider.Instance.MainFrameProvider.SetChannelName(CurrentChannel, _devModel.channelName);
			//UIProvider.Instance.VideoStreamingProvider.RefreshStream();
		}
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseVideoStreamingProvider();
			if (_subscription != null) _subscription.Dispose();
		}
	}
}
