using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace utils {
	public static class Utils {

		public static string MapPath(string path) {
			var assembly = Assembly.GetExecutingAssembly();
			var baseDir = Path.GetDirectoryName(assembly.Location);
			string fullPath = null;
			if (path.StartsWith("~/")) {
				fullPath = Path.Combine(baseDir, path.Substring(2, path.Length - 2).Replace('/', '\\'));
			} else {
				fullPath = path.Replace('/', '\\');
			}
			return fullPath;
		}
		
		public static void AddEnvironmentPath(IEnumerable<string> searchPath) {
			string pathVar = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
			pathVar += ";" + String.Join(";",
				from s in searchPath
				where !String.IsNullOrEmpty(s)
				select MapPath(s)
			);
			Environment.SetEnvironmentVariable("PATH", pathVar);
		}
		
		public static void AddEnvironmentPath(string searchPath) {
			string pathVar = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
			pathVar += ";" + MapPath(searchPath);
			Environment.SetEnvironmentVariable("PATH", pathVar);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetDllDirectory(string lpPathName);
	}
}
