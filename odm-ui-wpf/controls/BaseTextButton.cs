using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace odm.controls {
	public class TextButton: Button {
		public TextButton()
			: base() {
			textBlock = new TextBlock();
		}
		public TextBlock textBlock;
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextButton));
		public string Text {
			get {
				if (base.Content == null)
					return "";
				return base.Content.ToString();
			}
			set {
				base.Content = value;
			}
		}
	}
}
