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
using odm.utils.controlsUIProvider;
using odm.views;
using onvif.services.media;
using onvif;

namespace odm.controlsUIProvider {
	public class ProfileEditorProvider : BaseUIProvider, IProfileEditorProvider {
		PropertyProfileEditor profEditor;
		public void InitView(List<Profile> plst, ProfileToken profileToken, Action createNew, Action<Profile> renameProfile, Action<Profile> selectProfile, Action<Profile> deleteProfile) {
			profEditor = new PropertyProfileEditor(plst, profileToken, createNew, renameProfile, selectProfile, deleteProfile);
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(profEditor);
		}
		public void AddProfile(Profile prof) {
			profEditor.AddProfile(prof);
		}
		public void DeleteProfile(Profile prof) {
			profEditor.DeleteProfile(prof);
		}
		public override void ReleaseUI() {
			if (profEditor != null) {
				profEditor.ReleaseAll();
				profEditor = null;
			}
		}
	}
}
