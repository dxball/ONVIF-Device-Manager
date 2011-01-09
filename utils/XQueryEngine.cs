using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.utils.saxon;
using Saxon.Api;
using System.IO;
using System.Xml;
//using System.Web.Hosting;
using System.Reflection;

using saxon = net.sf.saxon.om;
using System.Diagnostics;
using System.ComponentModel;

namespace odm.xquery {

	//public class XmlUriResolver : XmlResolver {
		
	//    public override System.Net.ICredentials Credentials {
	//        set {
	//            throw new NotImplementedException();
	//        }
	//    }

	//    public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn) {
	//        throw new NotImplementedException();
	//    }

	//    public override Uri ResolveUri(Uri baseUri, string relativeUri) {
	//        var uri = base.ResolveUri(baseUri, relativeUri);
	//        return uri;
	//    }
	//}
	public interface IScriptInfo {
		Stream GetStream();
		object id {
			get;
		}
		DateTime modifiedTime {
			get;
		}
	}

	public class FsScriptInfo : IScriptInfo {
		public readonly FileInfo fileInfo = null;
		public FsScriptInfo(string filePath) {
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}
			fileInfo = new FileInfo(filePath);
		}

		public Stream GetStream() {
			return fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		public object id {
			get {
				return fileInfo.FullName;
			}
		}
		public DateTime modifiedTime {
			get {
				return fileInfo.LastWriteTime;
			}
		}
	}

	public interface IXQueryEngineBuilder {
		void OnInstanceCreated(XQueryCompiler compiler);
		void OnGetEvaluator(XQueryEvaluator eval);
		IScriptInfo ResolveScriptPath(string path);
	}

	public class DefaultXQueryEngineBuilder : IXQueryEngineBuilder {

		public virtual void OnInstanceCreated(XQueryCompiler compiler) {
			//DeclareExtensions(compiler);
			//if (HostingEnvironment.IsHosted) {
				//compiler.BaseUri = new Uri(HostingEnvironment.MapPath("~/content/")).ToString();
				//compiler.BaseUri = new Uri(HostingEnvironment.ApplicationPhysicalPath).ToString();
				//return;
			//}
			//compiler.BaseUri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).ToString();
			compiler.BaseUri = new Uri(Directory.GetCurrentDirectory()).ToString();
		}

		public virtual void OnGetEvaluator(XQueryEvaluator eval) {
			//eval.SetExternalVariable(
			//    new QName(null, "current_theme"),
			//    new XdmAtomicValue("default")
			//);
			//eval.SetExternalVariable(
			//    new QName(null, "theme_server_path"),
			//    new XdmAtomicValue(HostingEnvironment.MapPath("~/themes/default/"))
			//);
			//eval.SetExternalVariable(
			//    new QName(null, "content_server_path"),
			//    new XdmAtomicValue(HostingEnvironment.MapPath("~/content/"))
			//);
			//eval.SetExternalVariable(
			//    new QName(null, "theme_path"),
			//    new XdmAtomicValue(
			//        VirtualPathUtility.ToAbsolute("~/themes/default/")
			//    )
			//);
			//eval.SetExternalVariable(
			//    new QName(null, "content_path"),
			//    new XdmAtomicValue(
			//        VirtualPathUtility.ToAbsolute("~/content/")
			//    )
			//);

		}
		public IScriptInfo ResolveScriptPath(string path) {
			//var fPath = HostingEnvironment.MapPath(path);
			var sInfo = new FsScriptInfo(path);
			return sInfo;
		}

	}

	public class XQueryEngineBuilderProxy : DefaultXQueryEngineBuilder {
		private Action<XQueryCompiler> _fOnInstanceCreated = null;
		private Action<XQueryEvaluator> _fOnGetEvaluator = null;

		public XQueryEngineBuilderProxy(Action<XQueryCompiler> fOnInstanceCreated) {
			_fOnInstanceCreated = fOnInstanceCreated;
		}

		public XQueryEngineBuilderProxy(Action<XQueryCompiler> fOnInstanceCreated, Action<XQueryEvaluator> fOnGetEvaluator) {
			_fOnGetEvaluator = fOnGetEvaluator;
		}

		public override void OnInstanceCreated(XQueryCompiler compiler) {
			if (_fOnInstanceCreated != null) {
				_fOnInstanceCreated(compiler);
				return;
			}
			base.OnInstanceCreated(compiler);
		}
		public override void OnGetEvaluator(XQueryEvaluator eval) {
			if (_fOnGetEvaluator != null) {
				_fOnGetEvaluator(eval);
				return;
			}
			base.OnGetEvaluator(eval);
		}
	}

			

	public class XQueryEngine {
		
		//private static readonly Object syncRoot = new Object();
		private IXQueryEngineBuilder builder = null;
		private XQueryCompiler compiler = null;
		
		public XQueryEngine() {
			this.builder = new DefaultXQueryEngineBuilder();
		}

		public XQueryEngine(IXQueryEngineBuilder builder) {

			if (builder == null) {
				throw new ArgumentNullException("builder");
			}

			this.builder = builder;
		}

		public XQueryEngine(Action<XQueryCompiler> fOnInstanceCreated) {
			builder = new XQueryEngineBuilderProxy(fOnInstanceCreated);
		}

		public XQueryEngine(Action<XQueryCompiler> fOnInstanceCreated, Action<XQueryEvaluator> fOnGetEvaluator) {
			builder = new XQueryEngineBuilderProxy(fOnInstanceCreated, fOnGetEvaluator);
		}
		
		private Processor processor = null;
		private class CacheItem {
			public DateTime modifiedTime;
			public XQueryExecutable executable;
		}
		private readonly Dictionary<object, CacheItem> scriptCache = new Dictionary<object, CacheItem>();

		private void CreateInstance() {
			lock (this) {
				if (compiler != null) {
					return;
				}
				var proc = new Processor();
				var comp = proc.NewXQueryCompiler();
				builder.OnInstanceCreated(comp);
				compiler = comp;
				processor = proc;
			}
		}

		public Processor GetProcessor() {
			if (processor == null) {
		        CreateInstance();
		    }
		    return processor;
		}

		private XQueryCompiler GetCompiler() {
			if (compiler == null) {
				CreateInstance();
			}
			return compiler;
		}

        public XmlDocument ExecuteScript(string scriptPath) {
            var eval = GetEvaluator(scriptPath);
            DomDestination doc = new DomDestination();
            eval.Run(doc);
            doc.Close();
            return doc.XmlDocument;
           
        }

		public XmlDocument ExecuteScript(string scriptPath, object parameters, XmlNode context = null) {
			var eval = GetEvaluator(scriptPath);
			if(context != null){
				var proc = GetProcessor();
				var doc_builder = proc.NewDocumentBuilder();
				eval.ContextItem = doc_builder.Build(context);
			}
			//var paramDic = new Dictionary<string, XdmValue>(parameters);
			if (parameters != null) {
				foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(parameters)) {
					var obj = descriptor.GetValue(parameters);
					eval.SetExternalVariable(
						new QName(null, descriptor.Name),
						obj
					);
				}
			}
			//eval.InputXmlResolver = new XmlUriResolver();
			DomDestination doc = new DomDestination();
			eval.Run(doc);
			doc.Close();
			return doc.XmlDocument;
		}

		public void ExecuteScript(string scriptPath, TextWriter textWriter) {
			var res = ExecuteScript(scriptPath);

			using (var _xw = new XmlTextWriter(Console.Out)) {
				_xw.Formatting = Formatting.Indented;
				res.WriteTo(_xw);
				_xw.Close();
			};
			textWriter.WriteLine();
		}

		public XQueryEvaluator GetEvaluator(string scriptPath) {
			XQueryExecutable exec = null;
			var sInfo = builder.ResolveScriptPath(scriptPath);
			//var fPath = sInfo.FullName;
			var modifiedTime = sInfo.modifiedTime;

			lock (scriptCache) {

				CacheItem cache = null;
				if (!scriptCache.TryGetValue(sInfo.id, out cache) || (modifiedTime > cache.modifiedTime)) {

					var stream = sInfo.GetStream();
					try {
						cache = new CacheItem() {
							modifiedTime = modifiedTime,
							executable = GetCompiler().Compile(stream)
						};
						scriptCache[sInfo.id] = cache;
					} finally {
						stream.Close();
					}
				}
				exec = cache.executable;
			}

			XQueryEvaluator eval = exec.Load();
			//SetExternalVars(eval);
			builder.OnGetEvaluator(eval);
			return eval;
		}
	}
}
