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

namespace odm.controls {
	/// <summary>
	/// Interaction logic for DirectionRose.xaml
	/// </summary>
	public partial class DirectionRose : UserControl {
		public DirectionRose() {
			InitializeComponent();
			InitControls();
		}
		public Action SelectionChanged;

		void InitControls() {
			bE.Click += Click;
			bN.Click += Click;
			bNE.Click += Click;
			bNW.Click += Click;
			bS.Click += Click;
			bSE.Click += Click;
			bSW.Click += Click;
			bW.Click += Click;

			btnAll.Click += new RoutedEventHandler(btnAll_Click);
			btnNone.Click += new RoutedEventHandler(btnNone_Click);
		}

		void btnNone_Click(object sender, RoutedEventArgs e) {
			bE.Background = Brushes.White;
			bN.Background = Brushes.White;
			bNE.Background = Brushes.White;
			bNW.Background = Brushes.White;
			bS.Background = Brushes.White;
			bSE.Background = Brushes.White;
			bSW.Background = Brushes.White;
			bW.Background = Brushes.White;
		}

		void btnAll_Click(object sender, RoutedEventArgs e) {
			bE.Background = Brushes.DarkGray;
			bN.Background = Brushes.DarkGray;
			bNE.Background = Brushes.DarkGray;
			bNW.Background = Brushes.DarkGray;
			bS.Background = Brushes.DarkGray;
			bSE.Background = Brushes.DarkGray;
			bSW.Background = Brushes.DarkGray;
			bW.Background = Brushes.DarkGray;			
		}

		void Click(object sender, RoutedEventArgs e) {
			if (((Button)sender).Background == Brushes.White)
				((Button)sender).Background = Brushes.DarkGray;
			else
				((Button)sender).Background = Brushes.White;
			
			if (SelectionChanged != null)
				SelectionChanged();
		}
		public bool IfbE { get {return bE.Background == Brushes.DarkGray;} set { if (value)bE.Background = Brushes.DarkGray; }}
		public bool IfbN { get { return bN.Background == Brushes.DarkGray; } set { if (value)bN.Background = Brushes.DarkGray; }}
		public bool IfbNE { get { return bNE.Background == Brushes.DarkGray; } set { if (value)bNE.Background = Brushes.DarkGray; }}
		public bool IfbNW { get { return bNW.Background == Brushes.DarkGray; } set { if (value)bNW.Background = Brushes.DarkGray; }}
		public bool IfbS { get { return bS.Background == Brushes.DarkGray; } set { if (value)bS.Background = Brushes.DarkGray; }}
		public bool IfbSE { get { return bSE.Background == Brushes.DarkGray; } set { if (value)bSE.Background = Brushes.DarkGray; } }
		public bool IfbSW { get { return bSW.Background == Brushes.DarkGray; } set { if (value)bSW.Background = Brushes.DarkGray; } }
		public bool IfbW { get { return bW.Background == Brushes.DarkGray; } set { if (value)bW.Background = Brushes.DarkGray; } }
	}
}
