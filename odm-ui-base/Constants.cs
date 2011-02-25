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
using System.Drawing;
using System.Xml.XPath;
using System.Reflection;
using System.ComponentModel;

namespace odm {
	public enum LinkButtonsDeviceID {
		NONE,
		Identification,
		TimeSettings,
		Network,
		DigitalIO,
		Maintenance,
		SystemLog,
		OnvifExplorer,
		CommonEvents
	};
	public enum LinkButtonsChannelID {
		NONE,
		ProfileEditor,
		LiveVideo,
		ImagingSettings,
		Events,
		Metadata,
		Depth,
		VideoStreaming,
		Rotation,
		AnalogueOut,
		Annotation,
		TamperingDetectors,
		Tracker,
		ApproMotionDetector,
		Rule,
		Antishaker
	};

	public class MatrixValue {
		public MatrixValue(string name, float val) {
			Name = name;
			Value = val;
		}

		public string Name { get; set; }
		public float Value { get; set; }
	}
	public class MatrixTable {
		public static MatrixValue[] MatrixTbl = new MatrixValue[] { 
			new MatrixValue("Default", 4.8f),
			new MatrixValue("1''", 12.8f),
			new MatrixValue("2/3''", 8.8f),
			new MatrixValue("1/1,6''", 8.0f),
			new MatrixValue("1/1,8''", 7.2f),
			new MatrixValue("1/2''", 6.4f),
			new MatrixValue("1/2,3''", 6.1f),
			new MatrixValue("1/2,5''", 5.8f),
			new MatrixValue("1/2,7''", 5.3f),
			new MatrixValue("1/3''", 4.8f),
			new MatrixValue("1/3,2''", 4.5f),
			new MatrixValue("1/3,6''", 4.0f),
			new MatrixValue("1/4''", 3.2f),
			new MatrixValue("1/5''", 2.8f),
			new MatrixValue("1/6''", 2.4f),
			new MatrixValue("1/8''", 1.6f)
		};
	}


	public class ColorDefinition {
		public static readonly Color colLinkButtonsIitial = Color.FromArgb(0, 0, 255);
		public static readonly Color colLinkButtonsClicked = Color.FromArgb(150, 0, 200);
		public static readonly Color colLinkButtonsHovered = Color.FromArgb(150, 150, 255);
		public static readonly Color colControlBackground = Color.FromArgb(240, 240, 240);
		public static readonly Color colActiveControlBackground = Color.FromArgb(240, 250, 255);
		public static readonly Color colTitleBackground = Color.FromArgb(210, 210, 210);
		public static readonly Color colActiveTitleBackground = Color.FromArgb(240, 240, 250);
		public static readonly Color colMainWindowBackkground = Color.FromArgb(235, 235, 235);
		public static readonly Color colHighlightedImage = Color.FromArgb(0, 255, 0);
	}
	public class Defaults {
		public static string sNotifierImg =@"Resources\Images\BigLogo.png";
		#region Informatin Form
		public const int iInformationFormInitialHeight = 131;
		public const int iInformationFormXMLViewHeight = 556;
		#endregion
		#region DevicesListControl
		public const int iDevicesListControlHeaderNameWidth = 80;
		public const int iDevicesListControlHeaderIPWidth = 75;
		public const int iDevicesListControlHeaderTypeWidth = 110;
		public const int iDevicesListControlWidth = 270;
		#endregion
		#region PropertyEvents
		public const int iPropertyEventsHeaderIDWidth = 100;
		public const int iPropertyEventsHeaderDateWidth = 100;
		public const int iPropertyEventsHeaderTypeWidth = 100;
		public const int iPropertyEventsHeaderDetailsWidth = 100;
		public const int iEventsMaxCount = 15;
		#endregion
		#region RuleEngine
		public const int iRuleEngineHeaderName = 73;
		public const int iRuleEngineHeaderNameWithScroll = 61;
		public const int iRuleEngineHeaderIsChecked = 20;
		#endregion
		#region PropertyVideoStreaming
		//public const int iPropertyVideoStreamingFrameRateMin = 1;
		public const int iPropertyVideoStreamingFrameRateMin = 0;
		public const int iPropertyVideoStreamingFrameRateDef = 25;
		public const int iPropertyVideoStreamingFrameRateMax = 30;
		//public const int iPropertyVideoStreamingBitrateMin = 500;
		public const int iPropertyVideoStreamingBitrateMin = 0;
		public const int iPropertyVideoStreamingBitrateDef = 15000;
		public const int iPropertyVideoStreamingBitrateMax = 400000;
		public const string sDefaultVideoProfileName = "default_video_profile_channel_";
		public static string GetDefaultVideoProfileName(string channelID) {
			return sDefaultVideoProfileName + channelID;
		}

		#endregion
		#region LinkButtonsID
		public const int constLinkButtonIdentificationAndStatusID = 101;
		public const int constLinkButtonNetworkSettingsID = 102;
		public const int constLinkButtonDigitalIOID = 103;
		public const int constLinkButtonMaintenanceID = 104;
		public const int constLinkButtonLiveVideoID = 201;
		public const int constLinkButtonEventsID = 202;
		public const int constLinkButtonDepthCalibrationID = 203;
		public const int constLinkButtonVideoStreamingID = 204;
		public const int constLinkButtonDisplayAnnotationID = 205;
		public const int constLinkButtonTamperingDetectorsID = 206;
		public const int constLinkButtonObjectTrackerID = 207;
		public const int constLinkButtonRuleEngineID = 208;
		public const int constLinkButtonAntishakerID = 209;
		public const int constLinkButtonRotationID = 210;

		//Upper border for links ID
		public const int constDeviceControlLinksIDBorder = 200;
		public const int constDeviceChannelLinksIDBorder = 300;

		public const int iDeviceControlWidth = 350;
		#endregion LinkButtonsID


	}
}

