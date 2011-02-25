#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion

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
using System.Disposables;

namespace odm.ui.controls {
	/// <summary>
	/// Interaction logic for PropertyTimeZone.xaml
	/// </summary>
	public partial class DateTimeSettings : BasePropertyControl {
		public DateTimeSettings(DateTimeSettingsModel model) {
			this.model = model;
			InitializeComponent();
			Localization();
			InitControl();
			BindData();			

			Action UpdateSystemDateTime = () => {
				var tz = (TimeZoneInfo)timeZonesList.SelectedItem;

				var utc = TimeZoneInfo.ConvertTime(DateTime.Now.ToUniversalTime(), tz);
				systemDate.Text = utc.ToString("d");
				systemTime.Text = utc.ToString("T");
			};
			UpdateSystemDateTime();
			disposable.Add(Observable
				.Interval(TimeSpan.FromMilliseconds(500))
				.ObserveOnDispatcher()
				.Subscribe(x => {
					UpdateSystemDateTime();
				})
			);


		}

		public override void ReleaseAll() {
			disposable.Dispose();
			base.ReleaseAll();
		}
		CompositeDisposable disposable = new CompositeDisposable();

		PropertyTimeZoneStrings strings = new PropertyTimeZoneStrings();
		LinkButtonsStrings titles = new LinkButtonsStrings();

		DateTimeSettingsModel model;
		public Action onApply;
		ReadOnlyCollection<TimeZoneInfo> _tzInfoList;
		List<TimeZoneInfo> tzInfoListMutable;

		void Localization() {
			title.CreateBinding(ContentColumn.TitleProperty, titles, x => x.timesettings);
			currentTimeTitle.CreateBinding(TextBlock.TextProperty, strings, x => x.currentTime);
			//newTimeTitle.CreateBinding(TextBlock.TextProperty, strings, x => x.newTime);

			warningCaption.CreateBinding(TextBlock.TextProperty, strings, x => x.warning);

			enableAutoDayLightSwitch.CreateBinding(CheckBox.ContentProperty, strings, x => x.autoAdjustString);

			timeSettingsGroupBox.CreateBinding(GroupBox.HeaderProperty, strings, x => x.timeMode);
			timeZoneGroupBox.CreateBinding(GroupBox.HeaderProperty, strings, x => x.timeZone);
			
			//currentDateCaption.CreateBinding(Label.ContentProperty, strings, x => x.date);
			//currentTimeCaption.CreateBinding(Label.ContentProperty, strings, x => x.time);
			//computerDateCaption.CreateBinding(Label.ContentProperty, strings, x => x.date);
			//computerTimeCaption.CreateBinding(Label.ContentProperty, strings, x => x.time);
			//manualDateCaption.CreateBinding(Label.ContentProperty, strings, x => x.date);
			//manualTimePickerCaption.CreateBinding(Label.ContentProperty, strings, x => x.time);

			//ntpServerCaption.CreateBinding(Label.ContentProperty, strings, x => x.sinchronizeWithNTP);
			//manualCaption.CreateBinding(Label.ContentProperty, strings, x => x.manually);
			//computerCaption.CreateBinding(Label.ContentProperty, strings, x => x.sinchronizeWithComp);
			//labelNtp.CreateBinding(Label.ContentProperty, strings, x => x.NTPserver);

			//timeZoneCaption.CreateBinding(Label.ContentProperty, strings, x => x.timeZone);
			
		}
		TimeZoneInfo originTimezone;
		void InitControl() {
			
			//current timezone in windows format
			var initialPosTZ = PosixTimeZone.Convert(model.timeZone);
			
			//init timezones list
			_tzInfoList = TimeZoneInfo.GetSystemTimeZones();
			
			tzInfoListMutable = _tzInfoList.ToList();
			var ret = tzInfoListMutable.FirstOrDefault(x => {
				var posString = PosixTimeZone.GetPosixTimeZone(x);
				var tmpTZ = PosixTimeZone.Convert(posString);
				return PosixTimeZone.Compare(initialPosTZ, tmpTZ);
			});
			
			if (ret != null)
				originTimezone = ret;
			else {
				TimeZoneInfo tzi = TimeZoneInfo.CreateCustomTimeZone("0", new TimeSpan(initialPosTZ.stdOffset.hours, initialPosTZ.stdOffset.minutes, initialPosTZ.stdOffset.seconds),
					model.timeZone, model.timeZone);
				tzInfoListMutable.Add(tzi);
				originTimezone = tzi;
			}
			timeZonesList.ItemsSource = tzInfoListMutable;
			timeZonesList.DisplayMemberPath = "DisplayName";
			timeZonesList.SelectedItem = originTimezone;

			
			//Set time
			//computerDatePicker.SelectedDate = DateTime.Now;
			manualDatePicker.SelectedDate = DateTime.Now;
						
			//init events
			manualDatePicker.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(eDate_SelectedDateChanged);
			manualTimePicker.SelectedTimeChanged += new TimeSelectedChangedEventHandler(eTime_SelectedTimeChanged);
			timeZonesList.SelectionChanged += new SelectionChangedEventHandler(_cmbTimeZone_SelectionChanged);
			_saveCancelControl.Save.Click += new RoutedEventHandler(Save_Click);
			_saveCancelControl.Cancel.Click += new RoutedEventHandler(Cancel_Click);

			_saveCancelControl.Cancel.IsEnabled = false;
			_saveCancelControl.Save.IsEnabled = true;
		}

		System.Windows.Visibility CheckWarningVisibility(TimeZoneInfo selectedTZ) {
			if(TimeZoneInfo.Local.Id == selectedTZ.Id)
				return System.Windows.Visibility.Hidden;
			return System.Windows.Visibility.Visible;
		}

		void _cmbTimeZone_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			TimeZoneInfo tzInfo = (TimeZoneInfo)timeZonesList.SelectedValue;
			warningCaption.Visibility = CheckWarningVisibility(tzInfo);
			model.timeZone = PosixTimeZone.GetPosixTimeZone(tzInfo);
		}

		void eTime_SelectedTimeChanged(object sender, TimeSelectedChangedRoutedEventArgs e) {
			_saveCancelControl.Save.IsEnabled = true;
		}

		void eDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
			_saveCancelControl.Save.IsEnabled = true;
		}

		void Cancel_Click(object sender, RoutedEventArgs e) {
			model.RevertChanges();
			timeZonesList.SelectedItem = originTimezone;
		}

		void Save_Click(object sender, RoutedEventArgs e) {
			GetSelectedValues();
			if (onApply != null)
				onApply();
		}

		void BindData() {
			enableAutoDayLightSwitch.CreateBinding(CheckBox.IsCheckedProperty, model, m => m.daylightSavings,
				(m, v) => { m.daylightSavings = v; });

			ntpServerValue.CreateBinding(TextBox.TextProperty, model, m => {
				if (m.useNtpFromDhcp) {
					return m.ntpServerFromDhcp;
				}
				return m.ntpServerManual;

			}, (m, v) => {
				if (!m.useNtpFromDhcp) {
					m.ntpServerManual = v;
				}
			});

			useNtpFormDhcp.CreateBinding(CheckBox.IsCheckedProperty, model, x => x.useNtpFromDhcp, (m, v) => {
				m.useNtpFromDhcp = v;
			});

			_saveCancelControl.Cancel.CreateBinding(Button.IsEnabledProperty, model, x => x.isModified);
			//_saveCancelControl.Save.CreateBinding(Button.IsEnabledProperty, devModel, x => x.isModified);
		}

		void GetSelectedValues() {
			DateTime UtcDateTime = DateTime.UtcNow;

			var tzinfo = (TimeZoneInfo)timeZonesList.SelectedItem;
			if (tzinfo.Id != "0")
				model.timeZone = PosixTimeZone.GetPosixTimeZone(tzinfo);

			if (sys.IsSealed) {
				UtcDateTime = DateTime.UtcNow;
			}
			if (man.IsSelected) {
				UtcDateTime = manualDatePicker.SelectedDate.Value;
				//Set to UTC
				UtcDateTime = UtcDateTime.Add(manualTimePicker.SelectedTime);

				UtcDateTime = TimeZoneInfo.ConvertTimeToUtc(UtcDateTime, tzinfo);
			}
			//if (ntpServerRadio.IsChecked.Value) {
			//    //UtcDateTime = DateTime.UtcNow;
			//}
			model.dateTime = UtcDateTime;
		}
	}
}
