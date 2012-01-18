using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;


namespace utils {
	public class FuncBindableSource<TSrc, TProp> : INotifyPropertyChanged
		where TSrc : INotifyPropertyChanged {

		protected Action<TProp> m_setter = null;
		protected Func<TProp> m_getter = null;

		public FuncBindableSource(TSrc src, Func<TSrc, TProp> getter, Action<TSrc, TProp> setter = null) {
			if (src == null) {
				throw new ArgumentNullException("src");
			}
			if (getter == null) {
				throw new ArgumentNullException("getter");
			}
			m_getter = () => {
				return getter(src);
			};
			if (setter != null) {
				m_setter = (val) => setter(src, val);
			}
			src.PropertyChanged += (sender, args) => {
				m_isCached = false;
				if (PropertyChanged != null) {
					PropertyChanged(this, new PropertyChangedEventArgs("value"));
				}
			};
		}

		bool m_isCached = false;
		TProp m_cachedValue;
		public TProp value {
			get {
				if (!m_isCached) {
					m_cachedValue = m_getter();
					m_isCached = true;
				}
				return m_cachedValue;
			}
			set {
				if (m_setter != null) {
					m_setter(value);
				}
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}

	public static class WpfExtensions {

		[DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hObject);

		/// <summary> 
		/// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>. 
		/// </summary> 
		/// <param name="source">Bitmap to convert</param> 
		/// <returns>Converted BitmapSource</returns> 
		public static BitmapSource ToBitmapSource(this Bitmap source) {
			var hBitmap = source.GetHbitmap();
			try {
				return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
			} finally {
				DeleteObject(hBitmap);
			}
		}


		public static TControl CreateBinding<TControl, TSource, TSourceProperty>(this TControl control, DependencyProperty dp, TSource source, Func<TSource, TSourceProperty> getter)
			where TControl : DependencyObject
			where TSource : INotifyPropertyChanged {

			var funSrc = new FuncBindableSource<TSource, TSourceProperty>(source, getter);
			Binding binding = new Binding("value");
			binding.Mode = BindingMode.OneWay;
			binding.Source = funSrc;

			var retval = BindingOperations.SetBinding(control, dp, binding);
			return control;
		}
		public static TControl CreateBinding<TControl, TSource, TSourceProperty>(this TControl control, DependencyProperty dp, TSource source, Func<TSource, TSourceProperty> getter, Action<TSource, TSourceProperty> setter)
			where TControl : DependencyObject
			where TSource : INotifyPropertyChanged {

			var funSrc = new FuncBindableSource<TSource, TSourceProperty>(source, getter, setter);
			Binding binding = new Binding("value");
			binding.Mode = BindingMode.TwoWay;
			binding.Source = funSrc;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			var retval = BindingOperations.SetBinding(control, dp, binding);
			return control;
		}

		public static void ClearBinding<TControl>(this TControl ctrl, DependencyProperty dp) where TControl : DependencyObject {
			BindingOperations.ClearBinding(ctrl, dp);
		}

		public static void ClearAll<TControl>(this TControl ctrl) where TControl : DependencyObject {
			BindingOperations.ClearAllBindings(ctrl);
		}

		public static IObservable<object> GetPropertyChangedEvents<TControl>(this TControl ctrl, DependencyProperty dp) where TControl : DependencyObject {
			var dpd = DependencyPropertyDescriptor.FromProperty(dp, typeof(TControl));
			
			return Observable.Create<object>(observer=>{
				EventHandler eh = (s, e) => {
					var val = ctrl.GetValue(dp);
					observer.OnNext(val);
				};
				dpd.AddValueChanged(ctrl, eh);
				return Disposable.Create(() => {
					dpd.RemoveValueChanged(ctrl, eh);
				});
			});
		}
	}
}
