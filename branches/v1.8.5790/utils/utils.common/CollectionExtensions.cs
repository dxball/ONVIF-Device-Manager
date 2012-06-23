using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public static class CollectionExtensions {
		//public static T[] ToArray<T>(this LinkedList<T> list) {
		//    var array = new T[list.Count];
		//    int i = 0;
		//    foreach (var e in list) {
		//        array[i++] = e;
		//    }
		//    return array;
		//}
		public static void AddLast<T>(this LinkedList<T> list, IEnumerable<T> elements) {
			foreach (var e in elements) {
				list.AddLast(e);
			}
		}

	}
}
