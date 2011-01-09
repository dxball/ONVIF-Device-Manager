using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Linq.Expressions;
using odm.utils;
using System.Windows.Data;
using System.Windows.Media;

namespace odm {
	//public static class WPFBindingExtensions {
	//    public static TControl CreateBinding<TControl, TSource, TSourceProperty>(this TControl control, System.Windows.DependencyProperty dp, TSource dataSource, Expression<Func<TSource, TSourceProperty>> dataExpr, BindingMode mode)
	//        where TControl : FrameworkElement
	//        where TSource : INotifyPropertyChanged
	//    {
	//        var data_member = dataExpr.Body as MemberExpression;
	//        dbg.Assert(data_member != null);

	//        Binding binding = new Binding();

	//        binding.Source = dataSource;

	//        PropertyPath pp = new PropertyPath(data_member.Member.Name);
	//        binding.Path = pp;
	//        binding.Mode = mode;

	//        var retval = BindingOperations.SetBinding(control, dp, binding);
	//        return control;
	//    }
	//    public static TControl CreateBinding<TControl, TSource, TSourceProperty>(this TControl control, System.Windows.DependencyProperty dp, TSource dataSource, Expression<Func<TSource, TSourceProperty>> dataExpr)
	//        where TControl : FrameworkElement
	//        where TSource : INotifyPropertyChanged
	//    {
	//        var data_member = dataExpr.Body as MemberExpression;
	//        dbg.Assert(data_member != null);

	//        Binding binding = new Binding();
			
	//        binding.Source = dataSource;
			 
	//        PropertyPath pp = new PropertyPath(data_member.Member.Name);
	//        binding.Path = pp;

	//        var retval = BindingOperations.SetBinding(control, dp, binding);
	//        return control;
	//    }
	//}
	public static class ImageConversion {
		[System.Runtime.InteropServices.DllImport("gdi32")]
		public static extern int DeleteObject(IntPtr hObject);
		public static ImageSource ToImageSource(System.Drawing.Image img) {
			return ToImageSource(new System.Drawing.Bitmap(img));
		}
		public static ImageSource ToImageSource(System.Drawing.Bitmap bitmap) {
			var hbitmap = bitmap.GetHbitmap();
			try {
				var imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

				return imageSource;
			} finally {
				DeleteObject(hbitmap);
			}
		}
	}
}
