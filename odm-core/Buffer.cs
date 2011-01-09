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
using System.Collections;
using System.Collections.Generic;

namespace odm.utils {

	public class Buffer<T> : IEnumerable<T> {
		protected T[] innerBuffer = null;
		protected int head = 0;
		public int length {
			get;
			private set;
		}
		public T first {
			get {
				return this[0];
			}
		}
		public T last {
			get {
				return this[length - 1];
			}
		}
		public T this[int index] {
			get {
				if (index >= length || index < 0) {
					throw new ArgumentOutOfRangeException("index");
				}
				return innerBuffer[(head + index) % capacity];
			}
		}
		public void Push(T value) {
			innerBuffer[(head + length) % capacity] = value;
			if (length < capacity) {
				++length;
			} else {
				head = (head + 1) % capacity;
			}
		}
		public T Pop() {
			if (length == 0) {
				throw new ArgumentOutOfRangeException();
			}
			--length;
			return innerBuffer[(head + length) % capacity];
		}
		public void Clear() {
			length = 0;
		}

		public int capacity {
			get {
				return innerBuffer.Length;
			}
		}
		public Buffer(int size) {
			if (size <= 0) {
				throw new ArgumentOutOfRangeException("size");
			}
			innerBuffer = new T[size];
		}
		protected IEnumerator<T> GetTypedEnumeratorImpl() {
			for (int i = 0; i < length; ++i) {
				yield return this[i];
			}
		}
		protected IEnumerator GetUntypedEnumeratorImpl() {
			for (int i = 0; i < length; ++i) {
				yield return this[i];
			}
		}

		public IEnumerator<T> GetEnumerator() {
			return GetTypedEnumeratorImpl();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetUntypedEnumeratorImpl();
		}
	}

}
