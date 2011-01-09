using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
//using System.Web.Hosting;
//using System.Web.Routing;

using odm.utils.saxon;
using Saxon.Api;
using odm.xquery;
using odm.cms;
using odm.utils;

namespace odm.cms.xquery.extensions {

	/// <summary>
	/// This class contains theme-related methods that can be invoked from XSLT as
	/// extension functions or from XQuery as external functions.
	/// </summary>

	[XQueryExt(prefix = "theme")]
	public static class theme {
		//public static string current_theme {
		//    get {
		//        return "default";
		//    }
		//}
		
		//public static string app_path{
		//    get {
		//        return "~/themes/" + current_theme + "/";
		//    }
		//}
		
		//public static string server_path() {
		//    return HostingEnvironment.MapPath(app_path);
		//}
		//public static string url() {
		//    return VirtualPathUtility.ToAbsolute(app_path);
		//}
		//public static string url(string rel_path) {
		//    return VirtualPathUtility.Combine(url(), rel_path);
		//}

	}

	[XQueryExt(prefix = "file")]
	public static class file {
		public static DateTime modified_time(string file_path) {
			var fi = new FileInfo(file_path);
			return fi.LastAccessTime;
		}		
	}

	[XQueryExt(prefix = "content")]
	public static class content {
		
		public static string app_path {
			get {
				return "~/content/";
			}
		}

		//public static string server_path() {
		//    return HostingEnvironment.MapPath(app_path);
		//}

		//public static string server_path(string rel_path) {
		//    return HostingEnvironment.MapPath(VirtualPathUtility.Combine(app_path, rel_path));
		//}

		//public static string url() {
		//    return VirtualPathUtility.ToAbsolute(app_path);
		//}

		//public static string url(string rel_path) {
		//    return VirtualPathUtility.Combine(url(), rel_path);
		//}

	}

	[XQueryExt(prefix = "context")]
	public static class context {

		//public static string app_path {
		//    get {
		//        return "~/content/";
		//    }
		//}

		public static string get_param(string name) {
			//var ret = HttpContext.Current.Request.Params[name];
			//if (ret != null) {
			//    return ret;
			//}
			//var ctx = new HttpContextWrapper(HttpContext.Current);
			//var rd = RouteTable.Routes.GetRouteData(ctx);
			//var val = rd.Values[name];
			//if (val != null) {
			//    return val.ToString();
			//}
			//return ret;
			return "ups!!!!";
		}

		//public static XdmAtomicValue get_param(string name, XdmAtomicValue default_value) {
		//    var ret = get_param(name);
		//    if (!String.IsNullOrEmpty(ret)) {
		//        return new XdmAtomicValue(ret);
		//    }
		//    return default_value;
		//}

		//public static string url() {
		//    var ret = HttpContext.Current.Request.Url.AbsolutePath;
		//    return ret;
		//}
	}

	[XQueryExt(prefix = "ctrl")]
	public static class controller {
		//public static XdmValue exec(string script_path) {
		//    XdmValue res = null;
		//    try {

		//        var scriptPath = HostingEnvironment.MapPath(VirtualPathUtility.Combine("~/controllers/", script_path));
		//        XQueryEvaluator eval = XQueryController.xqEngine.GetEvaluator(scriptPath);
		//        res = eval.Evaluate();
		//    } catch (Exception e) {
		//        log.WriteError(e.Message);
		//        dbg.Break();
		//        throw;
		//    }

		//    return res;

		//}

		//public static XdmValue exec(string script_path, XdmNode context) {
		//    XdmValue res = null;
		//    try {
		//        var scriptPath = HostingEnvironment.MapPath(VirtualPathUtility.Combine("~/controllers/", script_path));
		//        XQueryEvaluator eval = XQueryController.xqEngine.GetEvaluator(scriptPath);


		//        XmlDocument doc = new XmlDocument();
		//        using (XmlWriter docWriter = doc.CreateNavigator().AppendChild()) {
		//            context.WriteTo(docWriter);
		//            docWriter.Close();
		//        }
		//        eval.ContextItem = XQueryController.xqEngine.GetProcessor().NewDocumentBuilder().Build(doc);
		//        res = eval.Evaluate();
		//    } catch (Exception e) {
		//        log.WriteError(e.Message);
		//        dbg.Break();
		//        throw;
		//    }

		//    return res;

		//}

		//public static string url(string action) {
		//    string res = null;
		//    try {
		//        res = XQueryController.GetVirtualPath(action);
		//    } catch (Exception e) {
		//        log.WriteError(e.Message);
		//        dbg.Break();
		//        throw;
		//    }

		//    return res;

		//}
	}

	[XQueryExt(prefix = "cms")]
	public static class cms {
		//public static string url(XdmNode routeValues) {
		//    string res = null;
		//    try {
		//        //var cms_ns = XQueryCompilerExtensions.GetNameSpace(typeof(cms));
		//        var d = new XDocument();
		//        using (var xwriter = d.CreateWriter()) {
		//            routeValues.WriteTo(xwriter);
		//            //var xvals = d.Root.Elements().Where(x => x.Name.NamespaceName.Equals(cms_ns));
		//        }
				
		//        var xvals = d.Root.Elements();
				
		//        var rvd = new RouteValueDictionary();
		//        foreach (var i in xvals) {
		//            var val_at = i.Attribute("val");
		//            dbg.BreakIf(val_at == null);
		//            if (val_at != null) {
		//                //rvd.Add(i.Name.LocalName, val_at.Value);
		//                rvd[i.Name.LocalName]=val_at.Value;
		//            }
		//        }
		//        //var routeValues = new RouteValueDictionary(new { action = action });
		//        var vpd = RouteTable.Routes.GetVirtualPath(null, rvd);
		//        dbg.BreakIf(vpd == null);
		//        res = vpd.VirtualPath;
		//    } catch (Exception e) {
		//        log.WriteError(e.Message);
		//        dbg.Break();
		//        throw;
		//    }

		//    return res;
		//}

		//public static XdmNode transform(string xslt_file_name, XdmNode node_to_transform) {

		//    var xsltFilePath = HostingEnvironment.MapPath(VirtualPathUtility.Combine("~/controllers/", xslt_file_name));
            
		//    XmlDocument inXml = new XmlDocument();
		//    using (XmlWriter _xw = inXml.CreateNavigator().AppendChild()) {
		//        node_to_transform.WriteTo(_xw);
		//        _xw.Close();
		//    }


		//    var xslt = new XslCompiledTransform();
		//    XsltSettings settings = new XsltSettings();
		//    settings.EnableScript = true;
		//    xslt.Load(xsltFilePath, settings, null);

		//    //prepare arguments
		//    XsltArgumentList arguments = new XsltArgumentList();
		//    //arguments.AddParam("link", "", "#");
		//    //arguments.AddExtensionObject(@"urn:myscripts", new date());
            
		//    XmlDocument outXml = new XmlDocument();
		//    using (XmlWriter _ow = outXml.CreateNavigator().AppendChild()) {
		//        xslt.Transform(inXml, arguments, _ow);
		//        _ow.Close();
		//    }
		//    var res = XQueryController.xqEngine.GetProcessor().NewDocumentBuilder().Build(outXml);
			
			
		//    return res;
		//}

	}
}
