using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.IO;

namespace odm {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
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
	}
}
