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
using System.Disposables;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Threading;
using System.Xml.XPath;
using System.Xml;

using onvifdm.utils;
using System.Xml.Serialization;
using com=System.ComponentModel;
using System.Globalization;


namespace nvc {
	public static class BoolHelper {
		public static bool parse(string str) {
			if (str == null) {
				throw new ArgumentNullException("str");
			}
			switch (str.ToLower().Trim()) {
				case "0":
					return false;
				case "1":
					return true;
				case "true":
					return true;
				case "false":
					return false;
			}

			throw new Exception("failed to parse string to boolean");
		}
		
	}

	public static class ObjectExtensions {
		public static object Convert(this object val, Type type) {
			DebugHelper.Assert(type != null);

			Type valType = val.GetType();
			if (valType == type) {
				return val;
			}

			com::TypeConverter conv = com::TypeDescriptor.GetConverter(type);
			if (conv.CanConvertFrom(valType)) {
				//return conv.ConvertFrom(val);
				return conv.ConvertFrom(null, CultureInfo.InvariantCulture, val);
			}

			conv = com::TypeDescriptor.GetConverter(valType);

			if (!conv.CanConvertTo(type)) {
				throw new InvalidOperationException("failed to convert");
			}

			//return conv.ConvertTo(val, type);
			return conv.ConvertTo(null, CultureInfo.InvariantCulture, val, type);
		}

		public static TType Convert<TType>(this object val) {
			if (val == null) {
				return default(TType);
			}
			return (TType)val.Convert(typeof(TType));
		}

		//public static string ToHtmlText(this object val) {
		//    if (val == null) {
		//        return null;
		//    }
		//    return HttpUtility.HtmlEncode(val.ToString());
		//}

		//public static string ToJson(this object val) {
		//    var ser = new JavaScriptSerializer();
		//    return ser.Serialize(val);
		//}

		//public static void ToJson(this object val, StringBuilder builder) {
		//    var ser = new JavaScriptSerializer();
		//    ser.Serialize(val, builder);
		//}
	}

	public static class XmlExtensions {
		
		public static T Deserialize<T>(this XmlNode xmlNode) {
			var root = new XmlRootAttribute(xmlNode.LocalName){
				Namespace = xmlNode.NamespaceURI
			};
			//var serializer = new XmlSerializer(typeof(T), root);
			var serializer = new XmlSerializer(typeof(T));
			using (var reader = new XmlNodeReader(xmlNode)) {
				return (T)serializer.Deserialize(reader);
			}
		}
		public static XmlElement Serialize<T>(this T obj) {
			var serializer = new XmlSerializer(typeof(T));
			var  xmlDoc = new XmlDocument();
			
			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj);
			}

			return xmlDoc.DocumentElement;
		}
		public static XmlElement Serialize<T>(this T obj, XmlSerializerNamespaces ns) {
			
			var serializer = new XmlSerializer(typeof(T));
			var xmlDoc = new XmlDocument();

			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj, ns);
			}

			return xmlDoc.DocumentElement;
		}
		public static XmlElement Serialize<T>(this T obj, XmlQualifiedName root) {
			var rootAttr = new XmlRootAttribute(root.Name);
			rootAttr.Namespace = root.Namespace;
			var serializer = new XmlSerializer(typeof(T), rootAttr);
			var xmlDoc = new XmlDocument();

			using (var writer = xmlDoc.CreateNavigator().AppendChild()) {
				serializer.Serialize(writer, obj);
			}

			return xmlDoc.DocumentElement;
		}

	}
	public static class ObservableExtensions {
		public static IObservable<object> Handle<T>(this IObservable<T> observable, Action<T> act) {
			return observable.Do(act).TakeLast(0).Select(x => new object());
		}

		public static IObservable<object> Idle<T>(this IObservable<T> observable) {
			return Observable.CreateWithDisposable<Object>(observer => {
				return observable.Subscribe(_ => {
				}, observer.OnError, observer.OnCompleted);
			});
		}

		public static IObservable<T> IgnoreError<T>(this IObservable<T> observable) {
			return Observable.CreateWithDisposable<T>(observer => {
				return observable.Subscribe(observer.OnNext, err => observer.OnCompleted(), observer.OnCompleted);
			});
		}
		public static IObservable<T> HandleError<T>(this IObservable<T> observable, Action<Exception> errorHandler) {
			return Observable.CreateWithDisposable<T>(observer => {
				return observable.Subscribe(observer.OnNext, 
					err => {
						try {
							errorHandler(err);
						} finally {
							observer.OnCompleted();
						}
					}, 
					observer.OnCompleted);
			});
		}
	}


	public static class XPathNavigable {
		private class AnonymousXPathNavigable:IXPathNavigable {
			private Func<XPathNavigator> m_factory;
			public AnonymousXPathNavigable(Func<XPathNavigator> factory) {
				if (factory == null) {
					throw new ArgumentNullException("factory");
				}
				m_factory = factory;
			}

			public XPathNavigator CreateNavigator() {
				return m_factory();
			}
		}
		public static IXPathNavigable Create(Func<XPathNavigator> factory) {
			return new AnonymousXPathNavigable(factory);
		}
	}
	public static class XPathExtensions {
		//private static Dictionary<string, XPathExpression> m_ExpressionCache = new Dictionary<string, XPathExpression>();
		
		public static Func<string, string> CreateEvaluator(this IXPathNavigable navigable) {
			return navigable.CreateNavigator().GetEvaluator();
		}
		public static Func<XPathExpression, string> CreateExprEvaluator(this IXPathNavigable navigable) {
			return navigable.CreateNavigator().GetExprEvaluator();
		}

		public static Func<string, string> GetEvaluator(this XPathNavigator navigator) {
			var xeval = GetExprEvaluator(navigator);
			return xpath => {
				XPathExpression expr = null;
				//lock (m_ExpressionCache) {
				//    if (!m_ExpressionCache.TryGetValue(xpath, out expr)) {
						expr = XPathExpression.Compile(xpath);
				//        m_ExpressionCache[xpath] = expr;
				//    }
				//}
				return xeval(expr);
			};			
		}

		public static Func<XPathExpression, string> GetExprEvaluator(this XPathNavigator navigator) {
			return xpath => {
				if (navigator == null) {
					return null;
				}
				var t = navigator.Select(xpath);
				var sb = new StringBuilder();
				while (t.MoveNext()) {
					sb.Append(t.Current);
				}
				var result = sb.ToString();
				if (String.IsNullOrWhiteSpace(result)) {
					return null;
				}
				return result;
			};
		}
	}
	
	public static class Extensions {

		public static Func<TArg, TResult> Wrap<TArg, TResult>(this Func<TArg, TResult> fun, Func<Func<TArg, TResult>, Func<TArg, TResult>> wrapper) {
			return wrapper(fun);
		}

		public static void Wrap<TArg>(this Action<TArg> act, Action<Action<TArg>> wrapper) {
			wrapper(act);
		}

		public static Func<TArg, TResult> Catch<TArg, TResult>(this Func<TArg, TResult> fun, Func<TResult> handler) {
			return arg => {
				try{
					return fun(arg);
				}catch{
					return handler();
				}
			};
		}

		public static Func<TArg, TResult> Catch<TArg, TResult>(this Func<TArg, TResult> fun, Func<Exception, TResult> handler) {
			return arg => {
				try {
					return fun(arg);
				} catch(Exception err) {
					return handler(err);
				}
			};
		}

		public static T GetCustomAttribute<T>(this Type type) where T : Attribute {
			return Attribute.GetCustomAttribute(type, typeof(T)) as T;
		}

		public static void SetDoubleBuffered(this System.Windows.Forms.Control control, bool value){
			var type = control.GetType();
			var prop = type.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			prop.SetValue(control, value, null);
		    //throw new NotImplementedException("SetDoubleBuffered");
		}
		
		public static void SetDoubleBufferedRecursive(this System.Windows.Forms.Control control, bool value){
			control.SetDoubleBuffered(value);
			control.Controls.ForEach(_control => {
				SetDoubleBufferedRecursive(_control as System.Windows.Forms.Control, value);				
			});				
		}

		public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action) {
			return dispatcher.BeginInvoke(action);
		}

		public static DispatcherOperation BeginInvoke(this DispatcherFrame dispatcherFrame, Action action) {
			return dispatcherFrame.Dispatcher.BeginInvoke(action);
		}




		//private static IObservable<T> OnError<T>(this IObservable<T> observable, Action<Exception> errorHandler) {
		//    return Observable.CreateWithDisposable<T>(observer => {
		//        var subscription = new MutableDisposable();
		//        var node = slot.Add(subscription);
		//        subscription.Disposable = observable
		//            .Subscribe(
		//                (t) => observer.OnNext(t),
		//                (err) => observer.OnError(err),
		//                () => {
		//                    slot.Complete();
		//                    observer.OnCompleted();
		//                });
		//        return slot;
		//    });
		//}

		public static IObservable<T> OnCompleted<T>(this IObservable<T> observable, Action completeHandler) {
			if (completeHandler == null) {
				throw new ArgumentNullException("completeHandler");
			}
			return Observable.CreateWithDisposable<T>(observer => {
				var subscription = observable
					.Subscribe(
						(t) => {
							observer.OnNext(t);
						},
						(err) => {
							observer.OnError(err);
						},
						() => {
							completeHandler();
							observer.OnCompleted();
						});
				return subscription;
			});
		}
		public static IObservable<T> OnError<T>(this IObservable<T> observable, Action<Exception> errorHandler) {
			if (errorHandler == null) {
				throw new ArgumentNullException("errorHandler");
			}
			return Observable.CreateWithDisposable<T>(observer => {
				var subscription = observable
					.Subscribe(
						(t) => observer.OnNext(t),
						(err) => {
							errorHandler(err);
							observer.OnError(err);
						},
						() => observer.OnCompleted()
					);
				return subscription;
			});
		}

		public static IObservable<T> OnDispose<T>(this IObservable<T> observable, Action disposeHandler) {
			if (disposeHandler == null) {
				throw new ArgumentNullException("disposeHandler");
			}
			return Observable.CreateWithDisposable<T>(observer => {
				var subscription = observable.Subscribe(observer);
				return Disposable.Create(() => {
					disposeHandler();
					subscription.Dispose();								
				});

			});
		}

		public static void Add(this CompositeDisposable compositeDisposable, Action disposeHandler) {
			compositeDisposable.Add(Disposable.Create(disposeHandler));
		}

		public static void ForEach<T>(this IEnumerable<T> src, Action<T> action) {
			foreach (var element in src) {
				action(element);
			}
		}
		public static void ForEach<T>(this IEnumerable<T> src, Action<T, int> action) {
			int index = 0;
			foreach (var element in src) {
				action(element, index++);
			}
		}

		public static T FirstOr<T>(this IEnumerable<T> src, Func<T> factory) {
			if (src == null) {
				throw new ArgumentNullException("src");
			}

			using(var itor = src.GetEnumerator()) {
				if (itor.MoveNext()) {
					return itor.Current;
				}
			}
			return factory();
		}

		public static void ForEach(this IEnumerable src, Action<object> action) {
			foreach (var element in src) {
				action(element);
			}
		}
		public static void ForEach(this IEnumerable src, Action<object, int> action) {
			int index = 0;
			foreach (var element in src) {
				action(element, index);
			}
		}
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T head) {
			return Enumerable.Repeat(head, 1).Concat(source);
		}
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T tail) {
			return source.Concat(Enumerable.Repeat(tail, 1));
		}
	}

	public class EnumHelper {
		public static IEnumerable<T> GetValues<T>() where T: struct {
			foreach (var val in Enum.GetValues(typeof(T))) {
				yield return (T)val;
			}
		}

		public static T Parse<T>(string value) where T : struct {
			return (T)Enum.Parse(typeof(T), value);
		}
	}
}
