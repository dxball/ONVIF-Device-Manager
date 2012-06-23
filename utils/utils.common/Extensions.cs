using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.IO;

namespace utils {
	

	public static class ObservableExtensions {
		public static IObservable<object> Handle<T>(this IObservable<T> observable, Action<T> act) {
			return observable.Do(act).TakeLast(0).Select(x => new object());
		}

		public static IObservable<object> Idle<T>(this IObservable<T> observable) {
			return Observable.Create<Object>(observer => {
				return observable.Subscribe(_ => {
				}, observer.OnError, observer.OnCompleted);
			});
		}

		public static IObservable<T> IgnoreError<T>(this IObservable<T> observable) {
			return Observable.Create<T>(observer => {
				return observable.Subscribe(observer.OnNext, err => observer.OnCompleted(), observer.OnCompleted);
			});
		}
		public static IObservable<T> HandleError<T>(this IObservable<T> observable, Action<Exception> errorHandler) {
			return Observable.Create<T>(observer => {
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


	public static class Extensions {

		public static Stream ToStream(this byte[] byteArray) {
			if (byteArray == null) {
				return null;
			}
			return new MemoryStream(byteArray);
		}

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

		//public static void SetDoubleBuffered(this System.Windows.Forms.Control control, bool value){
		//    var type = control.GetType();
		//    var prop = type.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		//    prop.SetValue(control, value, null);
		//    //throw new NotImplementedException("SetDoubleBuffered");
		//}
		
		//public static void SetDoubleBufferedRecursive(this System.Windows.Forms.Control control, bool value){
		//    control.SetDoubleBuffered(value);
		//    control.Controls.ForEach(_control => {
		//        SetDoubleBufferedRecursive(_control as System.Windows.Forms.Control, value);				
		//    });				
		//}

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
			return Observable.Create<T>(observer => {
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
			return Observable.Create<T>(observer => {
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
			return Observable.Create<T>(observer => {
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

		public static Uri GetBaseUri(this Uri uri) {
			if (!uri.IsAbsoluteUri) {
				return null;
			}
			var ub = new UriBuilder(uri.Scheme, uri.Host, uri.Port);
			ub.UserName = uri.UserInfo;
			return ub.Uri;
		}
		public static Uri Relocate(this Uri uri, string host) {
			if (!uri.IsAbsoluteUri) {
				return uri;
			}
			var ub = new UriBuilder(uri);
			ub.UserName = uri.UserInfo;
			ub.Password = null;
			ub.Host = host;
			return ub.Uri;
		}
		public static Uri Relocate(this Uri uri, string host, int port) {
			if (!uri.IsAbsoluteUri) {
				return uri;
			}
			var ub = new UriBuilder(uri);
			ub.UserName = uri.UserInfo;
			ub.Password = null;
			ub.Host = host;
			ub.Port = port;
			return ub.Uri;
		}
	}	
}
