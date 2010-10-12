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
using nvc.utils;

namespace nvc {
	
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
	}
}
