using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Windows.Threading;
using Microsoft.FSharp.Control;

namespace utils {
	public static class ApmExtensions {
		public static FSharpAsync<T> ObserveOnCurrentDispatcher<T>(this FSharpAsync<T> comp) {
			var synCtx = SynchronizationContext.Current;
			if (synCtx == null) {
				synCtx = new TrampolineSynCtx();
			}
			var scheduler = new SynchronizationContextScheduler(synCtx);
			return comp.ObserveOn(scheduler);
		}
	}
}
