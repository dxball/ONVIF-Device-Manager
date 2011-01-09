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
		public ChannelDescription Channel { get; set; }
		
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
			var settings = odm.utils.properties.Settings.Default;
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
