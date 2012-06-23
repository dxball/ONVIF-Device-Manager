using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using System.Threading;
using odm.controllers;
using utils;
using odm.extensibility;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Net;


namespace odm.ui {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		public void ChangeTheme(string _theme) {

		}

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
		static Thread s_uiThread = null;
		public static Thread uiThread {
			get {
				dbg.Assert(s_uiThread != null);
				return s_uiThread;
			}
		}

		[ImportMany(typeof(IPlugin))]
		public IEnumerable<IPlugin> plugins;
		
		protected override void OnStartup(StartupEventArgs e) {
			s_uiThread = Thread.CurrentThread;
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, policyErrors) => {
				return true;
			};

			base.OnStartup(e);
			Bootstrapper bootstrapper = new Bootstrapper();
			try{
				//var pluginsPath = MapPath("~/plugins");
				//if (!Directory.Exists(pluginsPath)) {
				//    Directory.CreateDirectory(pluginsPath);
				//}
				var catalog = new DirectoryCatalog("plugins");
				var container = new CompositionContainer(catalog);
				container.SatisfyImportsOnce(this);
				foreach (var p in plugins) {
					try {
						p.Init();
					} catch (Exception err) {
						//swallow error
						dbg.Error(err);
					}
				}
			}catch(Exception err){
				//swallow error
				dbg.Error(err);
			}
			bootstrapper.Run();

		}
	}
}
