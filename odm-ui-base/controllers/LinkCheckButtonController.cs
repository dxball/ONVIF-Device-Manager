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
using odm.onvif;
using odm.models;
using odm.utils;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class LinkCheckButtonController {
		public LinkCheckButtonController(bool isCheckable){
			LinkChannelID = LinkButtonsChannelID.NONE;
			LinkDeviceID = LinkButtonsDeviceID.NONE;
		}
		public Session ModelSession{get;set;}
		public ChannelModel Channel { get; set; }
		
		public Func<BasePropertyController> CreatePropertyAction{get;set;}
		public Action ReleasePropertyAction{get;set;}
		public Action<LinkCheckButtonController> Click { get; set; }
		public Action<LinkCheckButtonController, Action> CheckBoxSwitchedAcion { get; set; }

		public string Name { get; set; }
		public bool IsCheckable { get; set; }
		public bool IsChecked { get; set; }
		//public bool ChbOn { get; set; }
		public bool Enabled { get; set; }

		public LinkButtonsChannelID LinkChannelID { get; set; }
		public LinkButtonsDeviceID LinkDeviceID { get; set; }

		public void onClick(){
			var settings = odm.ui.properties.Settings.Default;
			//settings.LastDeviceActivity
			if(Click != null)
				Click(this);
		}

		public void ChBSwitched(bool Status, Action OnSwithError) {
			IsChecked = Status;
			if (CheckBoxSwitchedAcion != null)
				CheckBoxSwitchedAcion(this, OnSwithError);
		}

		public void SetName(string name){
		}

		public void AddLinkDeviceButton(LinkButtonsDeviceID linkID){
			LinkDeviceID = linkID;
			if (Enabled)
				UIProvider.Instance.GetMainFrameProvider().AddDeviceLinkButton(onClick, IsCheckable, Enabled, linkID);
		}

		public void AddLinkChannelButton(LinkButtonsChannelID linkID) {
			LinkChannelID = linkID;
			if(Enabled)
				UIProvider.Instance.GetMainFrameProvider().AddChannelLinkButton(onClick, Channel, IsCheckable, IsChecked, Enabled, linkID, ChBSwitched);
		}
	}
}
