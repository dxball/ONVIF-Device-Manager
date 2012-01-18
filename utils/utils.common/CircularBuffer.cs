using System;
using System.Collections;
using System.Collections.Generic;

namespace utils {
	public class CircularBuffer<T> : IEnumerable<T> {
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
				return GetElementImpl(index);
			}
		}
		public CircularBuffer(int size) {
			if (size <= 0) {
				throw new ArgumentOutOfRangeException("size");
			}
			innerBuffer = new T[size];
		}
		public void Enqueue(T value) {
			SetElementImpl(length, value);
			if (length < capacity) {
				++length;
			} else {
				head = (head + 1) % capacity;
			}
		}
		public T Dequeue() {
			if (length == 0) {
				throw new ArgumentOutOfRangeException();
			}
			length = length-1;
			var val = GetElementImpl(0);
			if (length == 0) {
				head = 0;
			} else {
				head = (head + capacity-1) % capacity;
			}
			return val;
		}
		public T DequeueOrDefault() {
			if (length == 0) {
				return default(T);
			}
			length = length - 1;
			var val = GetElementImpl(0);
			if (length == 0) {
				head = 0;
			} else {
				head = (head + capacity - 1) % capacity;
			}
			return val;
		}
		public void Clear() {
			length = 0;
			head = 0;
		}

		public int capacity {
			get {
				return innerBuffer.Length;
			}
		}
		public IEnumerator<T> GetEnumerator() {
			return GetTypedEnumeratorImpl();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetUntypedEnumeratorImpl();
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

		private T GetElementImpl(int index) {
			return innerBuffer[(head + index) % capacity];
		}
		private void SetElementImpl(int index, T value) {
			innerBuffer[(head + index) % capacity] = value;
		}
	}

}
