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
using odm.ui.activities;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.ComponentModel;
using utils;
using System.Diagnostics;
using System.Windows.Threading;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for DateTimeControl.xaml
	/// </summary>
	public partial class DateTimeControl : UserControl, INotifyPropertyChanged, IDisposable {
		public DateTimeControl() {
			InitializeComponent();

			dispatch = Dispatcher.CurrentDispatcher;
			time = DateTime.UtcNow;
			valueTime.CreateBinding(TextBlock.TextProperty, this, t => t.time.ToLongTimeString());
			valueDate.CreateBinding(TextBlock.TextProperty, this, t => t.time.ToShortDateString());
		}
		Dispatcher dispatch;
		DateTime _time;
		public DateTime time {
			get { return _time; }
			set {
				_time = value;
				NotifyPropertyChanged("time");
			}
		}
		TimeZoneViewModel timezone { get; set; }
		Func<DateTime> GetTime;
		CompositeDisposable disposables = new CompositeDisposable();
		public void SetTime(DateTime time, TimeZoneViewModel timezone, Func<DateTime> GetTime) {
			this.GetTime = GetTime;
			this.timezone = timezone;
			this.time = time;
			Startup();
		}
		public void TimeZoneChanged(TimeZoneViewModel timezone) {
			this.timezone = timezone;
			Startup();
		}
		public void Stop() {
			disposables.Dispose();
			disposables = new CompositeDisposable();
		}
		protected void Startup() {
			disposables.Dispose();
			disposables = new CompositeDisposable();

			TimeZoneInfo tzInfo = null;
			try {
				tzInfo = timezone.posixTz.ToSystemTimeZone(time.Year);
			} catch (Exception err) { }
			
			Stopwatch.GetTimestamp();
			var difTime = DateTime.UtcNow.Subtract(time);
			dispatch.BeginInvoke(() => {
				InitTimeAction(tzInfo);
			});
			disposables.Add(
				Observable.Interval(TimeSpan.FromMilliseconds(500))
				.Subscribe(next=>{
					if(GetTime != null){
						dispatch.BeginInvoke(() => {
							InitTimeAction(tzInfo);
						});
					}
				})
			);
		}
		private void InitTimeAction(TimeZoneInfo tzInfo) {
			var tmp = GetTime();
			if (tzInfo != null) {
				time = TimeZoneInfo.ConvertTime(tmp, tzInfo);
				valueCaption.Text = LocalTimeZone.instance.captionLocal;
				valueCaption.FontWeight = FontWeights.Normal;
				valueCaption.Foreground = Brushes.DarkGray;
			} else {
				time = tmp;
				valueCaption.Text = LocalTimeZone.instance.captionUtc;
				valueCaption.FontWeight = FontWeights.Bold;
				valueCaption.Foreground = Brushes.DarkRed;
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String info) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		public void Dispose() {
			disposables.Dispose();
		}
	}
}
