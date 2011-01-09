using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using odm.utils;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace odm.controls {

	public class CustomDialogWindow : Window {

		public static readonly DependencyProperty IsNonClientProperty;

		static CustomDialogWindow() {
			var IsNonClientMetadata = new FrameworkPropertyMetadata() {
				DefaultValue = false,
				Inherits = true
			};
			IsNonClientProperty = DependencyProperty.RegisterAttached("IsNonClient", typeof(bool), typeof(CustomDialogWindow), IsNonClientMetadata);
		}

		public CustomDialogWindow() {
			MouseLeftButtonDown += MouseLeftButtonDown_Handler;
			VisualTextRenderingMode = TextRenderingMode.ClearType;
		}

		private void MouseLeftButtonDown_Handler(object sender, MouseButtonEventArgs e) {
			dbg.Assert(e != null);
			dbg.Assert(e.OriginalSource != null);
			UIElement d = e.OriginalSource as UIElement;
			dbg.Assert(d != null);

			bool nc = GetIsNonClient(d);
			if (nc) {
				DragMove();
				e.Handled = true;
			}

		}


		public static bool GetIsNonClient(DependencyObject element) {
			if (element == null) {
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(IsNonClientProperty);
		}

		public static void SetIsNonClient(DependencyObject element, Boolean value) {
			if (element == null) {
				throw new ArgumentNullException("element");
			}
			element.SetValue(IsNonClientProperty, value);
		}
	}
}
