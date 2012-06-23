using System;
using System.Reactive.Concurrency;
using System.Windows.Threading;
using Microsoft.FSharp.Control;

namespace utils {
	public static class ApmExtensions {
		public static FSharpAsync<T> ObserveOnCurrentDispatcher<T>(this FSharpAsync<T> comp) {
			var dispatcher = Dispatcher.CurrentDispatcher;
			if (dispatcher == null) {
				var err = new Exception("failed to determine current dispatcher");
				dbg.Error(err);
				throw err;
			}
			var scheduler = new DispatcherScheduler(Dispatcher.CurrentDispatcher);
			return comp.ObserveOn(scheduler);
		}
	}
}
