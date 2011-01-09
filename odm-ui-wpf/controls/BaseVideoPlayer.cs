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

namespace odm.controls {
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
