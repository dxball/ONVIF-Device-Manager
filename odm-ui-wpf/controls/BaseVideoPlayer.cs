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
using System.Windows.Controls;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Windows;
using System.Diagnostics;
using odm.utils;

namespace odm.ui.controls {
	public class BaseVideoPlayer : UserControl {
		public MemoryMappedFile memFile;
		protected Rect _resolution;
		protected System.Windows.Forms.Timer _ftimer;
		protected Timer _tmr;
		protected Action<IntPtr, Rect> pBox;
		
		protected void _ftimer_Tick(object sender, EventArgs e) {
			try {
				if (memFile != null) {
					using (var f = memFile.CreateViewStream()) {
						var scan0 = f.SafeMemoryMappedViewHandle.DangerousGetHandle();
						if (pBox != null) {
							pBox(scan0, _resolution);
						}
					}
				}
			} catch (Exception err) {
				dbg.Error(err);
				string msg = err.Message;
			}
		}

		public void _tmr_Tick(Object stateInfo) {
			//Refresh image
			//try {
			//    if (memFile != null) {
			//        using (var f = memFile.CreateViewStream()) {
			//            var scan0 = f.SafeMemoryMappedViewHandle.DangerousGetHandle();
			//            if (pBox != null) {
			//                pBox(scan0, _resolution);
			//            }
			//        }
			//    }
			//} catch (Exception err) {
			//    dbg.Error(err);
			//    string msg = err.Message;
			//}
		}
	}
}
