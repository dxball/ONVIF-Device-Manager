using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Frms = System.Drawing;
using System.Globalization;
//using WPFMediaKit;

namespace odm.utils.controls.wpfControls {
	/// <summary>
	/// Interaction logic for wpfViewer.xaml
	/// </summary>
	public partial class wpfViewer : UserControl {
		public wpfViewer() {
			InitializeComponent();

			Loaded += new RoutedEventHandler(wpfViewer_Loaded);
			Unloaded += new RoutedEventHandler(wpfViewer_Unloaded);
		}
		 
		void wpfViewer_Unloaded(object sender, RoutedEventArgs e) {
		}
		void wpfViewer_Loaded(object sender, RoutedEventArgs e) {
		}
		bool isInit = true;
		RenderTargetBitmap rtBmp;
		WriteableBitmap wrBmp;
		DrawingVisual dv = new DrawingVisual();
		void InitBitmap(Frms.Bitmap img) {
			try {
				wrBmp = new WriteableBitmap((BitmapSource)ToImageSource(img));
				rtBmp = new RenderTargetBitmap(img.Width, img.Height, wrBmp.DpiX, wrBmp.DpiY, PixelFormats.Pbgra32);
				RenderOptions.SetBitmapScalingMode(_imageVisual, BitmapScalingMode.LowQuality);
				_imageVisual.Source = wrBmp;
				
				//_imageVisual.Source = rtBmp;
			} catch (Exception err) {
				dbg.Error(err);
			}
		}
		public void SetImage(Frms.Bitmap img, Rect size) {
			if (isInit) {
				InitBitmap(img);
				isInit = false;
			} else {
				try {
					var imgData = img.LockBits(new Frms.Rectangle(0, 0, (int)size.Width, (int)size.Height), Frms.Imaging.ImageLockMode.ReadOnly, Frms.Imaging.PixelFormat.Format32bppArgb);
					//wrBmp.WritePixels(new Int32Rect(0, 0, (int)size.Width, (int)size.Height), imgData.Scan0, imgData.Width * imgData.Height * 4, imgData.Stride);
					wrBmp.WritePixels(new Int32Rect(0, 0, (int)size.Width, (int)size.Height), imgData.Scan0, imgData.Width * imgData.Height * 4, imgData.Stride, 0, 0);
					//rtBmp.CopyPixels(Int32Rect.Empty, imgData.Scan0, imgData.Width * imgData.Height * 4, imgData.Stride);
					img.UnlockBits(imgData);
					//_imageVisual.InvalidateVisual();
					
					//DrawingContext ctx = dv.RenderOpen();
					////VisualBrush vb = new VisualBrush(_imageVisual);
					//ctx.DrawImage(ToImageSource(img), size);
					//ctx.Close();

					//rtBmp.Render(dv);

					//_imageVisual.InvalidateVisual();

				} catch (Exception ex) {
					//MessageBox.Show("Failed to manipulate image:\n" + ex.Message);
				}
			}
		}

		[System.Runtime.InteropServices.DllImport("gdi32")]
	    public static extern int DeleteObject(IntPtr hObject);
	    public static ImageSource ToImageSource(System.Drawing.Bitmap bitmap){
	        var hbitmap = bitmap.GetHbitmap();
	        try
	        {
	            var imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
	 
            return imageSource;
	        }
	        finally
	        {
	            DeleteObject(hbitmap);
	        }
	    }
	}
}
