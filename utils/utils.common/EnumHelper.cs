using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils {
	public static class EnumHelper {
		public static IEnumerable<T> GetValues<T>() where T : struct {
			foreach (var val in Enum.GetValues(typeof(T))) {
				yield return (T)val;
			}
		}

		public static T Parse<T>(string value) where T : struct {
			return (T)Enum.Parse(typeof(T), value);
		}
	}
}
