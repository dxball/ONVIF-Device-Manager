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
using System.Collections.ObjectModel;

using odm.models;
using odm.controllers;
using odm.utils.extensions;

namespace odm.controls {
	/// <summary>
	/// Interaction logic for PropertyTimeZone.xaml
	/// </summary>
	public partial class PropertyTimeZone : BasePropertyControl {
		public PropertyTimeZone(DateTimeSettingsModel devModel) {
			InitializeComponent();
			_devModel = devModel;

			Localization();
			InitControl();
			BindData(_devModel);
		}
		PropertyTimeZoneStrings _strings = new PropertyTimeZoneStrings();
		DateTimeSettingsModel _devModel;
		public Action onApply;
		ReadOnlyCollection<TimeZoneInfo> _tzInfoList;
		List<TimeZoneInfo> tzInfoListMutable;

		void Localization() {
			currentTimeTitle.CreateBinding(Title.ContentProperty, _strings, x => x.currentTime);
			newTimeTitle.CreateBinding(Title.ContentProperty, _strings, x => x.newTime);

			enableAutoDayLightCaption.CreateBinding(Label.ContentProperty, _strings, x => x.autoAdjustString);

			timeSettingsGroupBox.CreateBinding(GroupBox.HeaderProperty, _strings, x => x.timeMode);
			timeZoneGroupBox.CreateBinding(GroupBox.HeaderProperty, _strings, x => x.timeZone);

			currentDateCaption.CreateBinding(Label.ContentProperty, _strings, x => x.date);
			currentTimeCaption.CreateBinding(Label.ContentProperty, _strings, x => x.time);
			computerDateCaption.CreateBinding(Label.ContentProperty, _strings, x => x.date);
			computerTimeCaption.CreateBinding(Label.ContentProperty, _strings, x => x.time);
			manualDateCaption.CreateBinding(Label.ContentProperty, _strings, x => x.date);
			manualTimePickerCaption.CreateBinding(Label.ContentProperty, _strings, x => x.time);

			ntpServerCaption.CreateBinding(Label.ContentProperty, _strings, x => x.sinchronizeWithNTP);
			manualCaption.CreateBinding(Label.ContentProperty, _strings, x => x.manually);
			computerCaption.CreateBinding(Label.ContentProperty, _strings, x => x.sinchronizeWithComp);
			ntpServerButton.CreateBinding(Button.ContentProperty, _strings, x => x.NTPserver);
			
			//_lblTimeZone.label.CreateBinding(TextBlock.TextProperty, _strings, x => x.timeZone);
		}

		System.Windows.Forms.Timer clockTmr;

		void InitControl() {
			clockTmr = new System.Windows.Forms.Timer();
			clockTmr.Tick += new EventHandler(clockTmr_Tick);
			clockTmr.Interval = 1000;
			clockTmr.Start();

			//current timezone in windows format
			var initialPosTZ = PosixTimeZone.Convert(_devModel.timeZone);
			//init timezones list
			_tzInfoList = TimeZoneInfo.GetSystemTimeZones();
			tzInfoListMutable = _tzInfoList.ToList();
			var ret = tzInfoListMutable.FirstOrDefault(x => {
				var posString = PosixTimeZone.GetPosixTimeZone(x);
				var tmpTZ = PosixTimeZone.Convert(posString);
				return PosixTimeZone.Compare(initialPosTZ, tmpTZ);
			});
			TimeZoneInfo selectedItem;
			if (ret != null)
				selectedItem = ret;
			else {
				TimeZoneInfo tzi = TimeZoneInfo.CreateCustomTimeZone("0", new TimeSpan(initialPosTZ.stdOffset.hours, initialPosTZ.stdOffset.minutes, initialPosTZ.stdOffset.seconds),
					_devModel.timeZone, _devModel.timeZone);
				tzInfoListMutable.Add(tzi);
				selectedItem = tzi;
			}
			timeZonesList.ItemsSource = tzInfoListMutable;
			timeZonesList.DisplayMemberPath = "DisplayName";
			timeZonesList.SelectedItem = selectedItem;

			//Initial radio button
			computerTimeRadio.IsChecked = true;

			//Set time
			computerDatePicker.SelectedDate = DateTime.Now;
			manualDatePicker.SelectedDate = DateTime.Now;
			currentDatePicker.SelectedDate = PosixTimeZone.GetDeviceTimeWithTimeZone(_devModel.dateTime, (TimeZoneInfo)timeZonesList.SelectedItem);
			currentTimePicker.SelectedTime = PosixTimeZone.GetDeviceTimeWithTimeZone(_devModel.dateTime, (TimeZoneInfo)timeZonesList.SelectedItem).TimeOfDay;

			//init events
			manualDatePicker.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(eDate_SelectedDateChanged);
			manualTimePicker.SelectedTimeChanged += new TimeSelectedChangedEventHandler(eTime_SelectedTimeChanged);
			computerTimeRadio.Click += new RoutedEventHandler(computerTimeRadio_Click);
			manualTimeRadio.Click += new RoutedEventHandler(manualTimeRadio_Click);
			ntpServerRadio.Click += new RoutedEventHandler(ntpServerRadio_Click);
			timeZonesList.SelectionChanged += new SelectionChangedEventHandler(_cmbTimeZone_SelectionChanged);
			_saveCancelControl.Save.Click += new RoutedEventHandler(Save_Click);
			_saveCancelControl.Cancel.Click += new RoutedEventHandler(Cancel_Click);

			ntpServerButton.Click += new RoutedEventHandler(ntpServerButton_Click);

			_saveCancelControl.Cancel.IsEnabled = false;
			_saveCancelControl.Save.IsEnabled = true;
		}

		void ntpServerButton_Click(object sender, RoutedEventArgs e) {
			SetNtpServer ntpSetup = new SetNtpServer();
			ntpSetup.setServer = SetNtpServer;
			ntpSetup.Show();
		}

		void SetNtpServer(string ntpString) {
			SetNtpServerRadio();
		}

		void SetNtpServerRadio() {
			computerTimeRadio.IsChecked = false;
			manualTimeRadio.IsChecked = false;
			_saveCancelControl.Save.IsEnabled = true;
		}
		void ntpServerRadio_Click(object sender, RoutedEventArgs e) {
			SetNtpServerRadio();
		}

		void manualTimeRadio_Click(object sender, RoutedEventArgs e) {
			computerTimeRadio.IsChecked = false;
			ntpServerRadio.IsChecked = false;
			_saveCancelControl.Save.IsEnabled = true;
		}

		void computerTimeRadio_Click(object sender, RoutedEventArgs e) {
			manualTimeRadio.IsChecked = false;
			ntpServerRadio.IsChecked = false;
			_saveCancelControl.Save.IsEnabled = true;
		}

		void clockTmr_Tick(object sender, EventArgs e) {
			computerTimePicker.SelectedTime = DateTime.Now.TimeOfDay;
		}

		void _cmbTimeZone_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			TimeZoneInfo tzInfo = (TimeZoneInfo)timeZonesList.SelectedValue;
			_devModel.timeZone = PosixTimeZone.GetPosixTimeZone(tzInfo);
		}

		void eTime_SelectedTimeChanged(object sender, TimeSelectedChangedRoutedEventArgs e) {
			_saveCancelControl.Save.IsEnabled = true;
		}

		void eDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
			_saveCancelControl.Save.IsEnabled = true;
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			GetSelectedValues();
			if (onApply != null)
				onApply();
		}

		void BindData(DateTimeSettingsModel devModel) {
			enableAutoDayLightSwitch.CreateBinding(CheckBox.IsCheckedProperty, devModel, x => x.daylightSavings,
				(m, v) => { m.daylightSavings = v; });

			_saveCancelControl.Cancel.CreateBinding(Button.IsEnabledProperty, devModel, x => x.isModified);
			_saveCancelControl.Save.CreateBinding(Button.IsEnabledProperty, devModel, x => x.isModified);
		}

		void GetSelectedValues() {
			DateTime UtcDateTime = DateTime.UtcNow;

			var tzinfo = (TimeZoneInfo)timeZonesList.SelectedItem;
			if (tzinfo.Id != "0")
				_devModel.timeZone = PosixTimeZone.GetPosixTimeZone(tzinfo);

			if (computerTimeRadio.IsChecked.Value) {
				UtcDateTime = DateTime.UtcNow;

			}
			if (manualTimeRadio.IsChecked.Value) {
				UtcDateTime = manualDatePicker.SelectedDate.Value;
				//Set to UTC
				UtcDateTime = UtcDateTime.Add(manualTimePicker.SelectedTime);

				UtcDateTime = TimeZoneInfo.ConvertTimeToUtc(UtcDateTime, tzinfo);
			}
			if (ntpServerRadio.IsChecked.Value) {
				//UtcDateTime = DateTime.UtcNow;
			}
			_devModel.dateTime = UtcDateTime;
		}
	}
}
