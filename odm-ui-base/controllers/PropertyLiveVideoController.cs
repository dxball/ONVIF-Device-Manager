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
using odm.onvif;
using odm.models;
using odm.utils;
using odm.utils.controlsUIProvider;
using System.Threading;
using System.IO;

namespace odm.controllers {
	public class PropertyLiveVideoController : BasePropertyController{
		LiveVideoModel _devModel;
		IDisposable _subscription;
		DataProcessInfo dprocinfo;
		bool isRecording;
		protected override void LoadControl() {
			_devModel = new LiveVideoModel(CurrentChannel.profileToken);
			_subscription = _devModel.Load(CurrentSession).Subscribe(arg => {
				dprocinfo = WorkflowController.Instance.GetMainFrameController().GetProcessByChannel(CurrentChannel);
				UIProvider.Instance.GetLiveVideoProvider().InitView(_devModel, dprocinfo, SetRecordingFolder, StartRecording, StopRecording);
			}, err => {
				OnCriticalError(err);
			});
		}
		public void SetRecordingFolder(string path) {
			savingPath = path;
		}
		string savingPath = "";
		public void StartRecording() {
			//[CHANNEL_NAME]_YY-MM-DD_HH'MM'SS.TS
			string path = savingPath;
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (dprocinfo == null || dprocinfo.iPlayer == null) {
				dbg.Error("iPlayer == null");
				return;
			}
			var time = DateTime.Now;
			path += @"\" + CurrentChannel.name + "_" + time.Year + "-" + time.Month + "-" + time.Day + "_" + time.Hour + "'" + time.Minute + "'" + time.Second + ".TS";
			try {
				
				dprocinfo.iPlayer.StartRecord(path, 30).ObserveOn(SynchronizationContext.Current).Subscribe(x=>{
					isRecording = true;
				});
			} catch (Exception err) {
				dbg.Error(err);

			}
		}
		
		public void StopRecording() {
			if (isRecording) {
				try {
					dprocinfo.iPlayer.StopRecord().ObserveOn(SynchronizationContext.Current).Subscribe(x => {
						isRecording = false;
					});
				} catch (Exception err) {
					dbg.Error(err);
				}
			}
		}

		protected override void ApplyChanges() {}
		protected override void CancelChanges() {}
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseLiveVideoProvider();
			if (_subscription != null) _subscription.Dispose();
		}
	}
}
