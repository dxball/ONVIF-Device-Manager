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
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace odm.utils.rx {

	public static class ObservableStream {
		public static IObservable<int> Read(Stream stream, byte[] buffer, int offset, int count) {
			return Observable.FromAsyncPattern<byte[], int, int, int>(stream.BeginRead, stream.EndRead)(buffer, offset, count);
		}
		public static IObservable<Unit> Write(Stream stream, byte[] buffer, int offset, int count) {
			return Observable.FromAsyncPattern<byte[], int, int>(stream.BeginWrite, stream.EndWrite)(buffer, offset, count);
		}
		private static IEnumerable<IObservable<object>> CopyImpl(Stream from, Stream to) {
			var bufferSize = 0x1000;//4KB
			byte[] buffer = new byte[bufferSize];
			while (true) {
				int readed = 0;
				yield return ObservableStream.Read(from, buffer, 0, buffer.Length).Handle(x => readed = x);
				if (readed == 0) {
					yield break;
				}
				yield return ObservableStream.Write(to, buffer, 0, readed).Idle();
				//to.Flush();
			}
		}
		public static IObservable<Unit> Copy(Stream from, Stream to) {
			return Observable.Iterate(() => CopyImpl(from, to));
		}
	}

}
