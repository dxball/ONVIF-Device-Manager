using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.FSharp.Control;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using odm.controllers;
using odm.infra;
using odm.ui.controls;
using utils;

namespace odm.ui.activities {
	public partial class TimeSettingsView : BasePropertyControl {
		public enum SetDateTimeMode {
			SetManually,
			SyncWithNtp,
			SyncWithComp
		}

		#region Activity definition
		public static FSharpAsync<Result> Show(IUnityContainer container, Model model) {
			return container.StartViewActivity<Result>(context => {
				var view = new TimeSettingsView(model, context);
				var presenter = container.Resolve<IViewPresenter>();
				presenter.ShowView(view);
			});
		}
		#endregion

		CompositeDisposable disposables = new CompositeDisposable();
		public ICommand CancelCommand { get; private set; }
		public PropertyTimeZoneStrings Strings { get { return PropertyTimeZoneStrings.instance; } }
		public Model model;

		public string DeviceTimeZoneOriginal = "";
		//public List<TimeZoneInfo> timeZones = new List<TimeZoneInfo>();
		public List<TZInfoDescriptor> timeZones = new List<TZInfoDescriptor>();
		public delegate void PropertyChanged();
		public event PropertyChanged OnDeviceTimeZoneChanged;
		
		//public TimeZoneInfo m_deviceTimeZone;
		//public TimeZoneInfo deviceTimeZone { 
		//    get { 
		//        return m_deviceTimeZone;
		//    }
		//    private set {
		//        m_deviceTimeZone = value; 
		//        if (OnDeviceTimeZoneChanged != null) {
		//            OnDeviceTimeZoneChanged();
		//        }
		//    }
		//}
		public TZInfoDescriptor m_deviceTimeZone;
		public TZInfoDescriptor deviceTimeZone {
			get {
				return m_deviceTimeZone;
			}
			private set {
				m_deviceTimeZone = value;
				if (OnDeviceTimeZoneChanged != null) {
					OnDeviceTimeZoneChanged();
				}
			}
		}

		public SetDateTimeMode setDateTimeMode { get; set; }
		public string ComputerTime {get; set;}
		public string ComputerDate {get; set;}
		public string CurrentDate {get; set;}
		public string CurrentTime {get; set;}
		public DateTime deviceDateTime {get; set;}
		public DateTime SelectedDate {get; set;}
		public System.TimeSpan SelectedTime {get; set;}
		public event PropertyChanged OnAutoAdjustDaylightChanged;
		public bool m_autoAdjustDaylight;
		public bool autoAdjustDaylight { 
			get{
				return m_autoAdjustDaylight;
			} 
			set{
				m_autoAdjustDaylight = value;
				if (OnAutoAdjustDaylightChanged != null) {
					OnAutoAdjustDaylightChanged();
				}
			} 
		}
		public bool IsNTPFromDHCP { get; set; }
		public string NtpServerPath { get; set; }

        public LinkButtonsStrings Title { get { return LinkButtonsStrings.instance; } }

		private void Init(Model model) {
			OnCompleted += () => {
				disposables.Dispose();
			};
			this.model = model;

			var applyCommand = new DelegateCommand(
				() => OnApplyChanges(),
				() => true
			);
			ApplyCommand = applyCommand;

			var closeCommand = new DelegateCommand(
				() => Success(new Result.Close()),
				() => true
			);
			CloseCommand = closeCommand;

			var cancelCommand = new DelegateCommand(
				() => {
                    OnRevertChanges();
				},
				() => true
			);
			CancelCommand = cancelCommand;

			if (model != null) {
			}

			InitializeComponent();
			
			DeviceTimeZoneOriginal = model.timeZone;
			InitTimeZones();
			Init();
			BindModel(model);
            Localization();
		}

        #region Binding
        void Localization() {
            daylightCheckBox.CreateBinding(CheckBox.ContentProperty, Strings, s => s.autoAdjustString);
            ntpSettingsToolTip.CreateBinding(
                TextBlock.TextProperty, Strings,
                s => s.ntpSetupInfo
            );
            deviceDateTimeCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.currentTime);
            syncWithNtp.CreateBinding(ComboBoxItem.ContentProperty, Strings, s => s.synchronizeWithNtp);
            setManual.CreateBinding(ComboBoxItem.ContentProperty, Strings, s => s.manually);
            timeZoneGroupBox.CreateBinding(GroupBox.HeaderProperty, Strings, s => s.timeZone);
            newCompTimeCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.newDateTime);
            applyButton.CreateBinding(Button.ContentProperty, SaveCancelStrings.instance, s => s.save);
            cancelButton.CreateBinding(Button.ContentProperty, SaveCancelStrings.instance, s => s.cancel);
            newManualTimeCaption.CreateBinding(TextBlock.TextProperty, Strings, s => s.newDateTime);
            syncWithComp.CreateBinding(ComboBoxItem.ContentProperty, Strings, s => s.synchronizeWithComp);
        }

		void Refresh() {
			autoAdjustDaylight = model.daylightSavings;
			daylightCheckBox.IsChecked = autoAdjustDaylight;

			if (deviceTimeZone != null) {
				var tz = TimeZoneInfo.CreateCustomTimeZone(
					deviceTimeZone.Id,
					deviceTimeZone.BaseUtcOffset,
					deviceTimeZone.DisplayName,
					deviceTimeZone.StandardName,
					deviceTimeZone.DaylightName,
					deviceTimeZone.GetAdjustmentRules(),
					!autoAdjustDaylight
				);
				deviceDateTime = TimeZoneInfo.ConvertTime(model.utcDateTime, tz);
			} else {
				deviceDateTime = model.utcDateTime;
			}

			setDateTimeMode = model.useDateTimeFromNtp ? SetDateTimeMode.SyncWithNtp : SetDateTimeMode.SyncWithComp;
			PanelNtpMode.Visibility =
				setDateTimeMode == SetDateTimeMode.SyncWithNtp ?
				Visibility.Visible : Visibility.Collapsed;

			syncWithNtp.IsSelected = setDateTimeMode == SetDateTimeMode.SyncWithNtp;
			PanelSystemMode.Visibility =
			   setDateTimeMode == SetDateTimeMode.SyncWithComp ?
			   Visibility.Visible : Visibility.Collapsed;
			syncWithComp.IsSelected = setDateTimeMode == SetDateTimeMode.SyncWithComp;

			PanelManualMode.Visibility = System.Windows.Visibility.Collapsed;
			setManual.IsSelected = false;

			timeZonesComboBox.ItemsSource = timeZones;
			timeZonesComboBox.SelectedItem = deviceTimeZone;

			SelectedTime = new TimeSpan(
				deviceDateTime.Hour,
				deviceDateTime.Minute,
				deviceDateTime.Second
			);
			SelectedDate = deviceDateTime;

			newManualTimeValue.SelectedTime = SelectedTime;
			newManualDateValue.SelectedDate = SelectedDate;
		}

        void BindModel(TimeSettingsView.Model model) {
            Refresh();

            applyButton.Command = ApplyCommand;
            cancelButton.Command = CancelCommand;

            daylightCheckBox.Checked += (s, a) => { autoAdjustDaylight = true; };
            daylightCheckBox.Unchecked += (s, a) => { autoAdjustDaylight = false; };

            disposables.Add(
                syncWithNtp
                    .GetPropertyChangedEvents(ComboBoxItem.IsSelectedProperty)
                    .OfType<bool>()
                    .Subscribe(value => {
                        PanelNtpMode.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                        setDateTimeMode = SetDateTimeMode.SyncWithNtp;
                    })
            );

            disposables.Add(
                syncWithComp
                    .GetPropertyChangedEvents(ComboBoxItem.IsSelectedProperty)
                    .OfType<bool>()
                    .Subscribe(value => {
                        PanelSystemMode.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                        setDateTimeMode = SetDateTimeMode.SyncWithComp;
                    })
            );

            disposables.Add(
                setManual
                    .GetPropertyChangedEvents(ComboBoxItem.IsSelectedProperty)
                    .OfType<bool>()
                    .Subscribe(value => {
                        PanelManualMode.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                        setDateTimeMode = SetDateTimeMode.SetManually;
                    })
            );

            timeZonesComboBox.SelectionChanged += (s, a) => {
               // deviceTimeZone = (TimeZoneInfo)timeZonesComboBox.SelectedItem;
				deviceTimeZone = (TZInfoDescriptor)timeZonesComboBox.SelectedItem;
            };

            Action UpdateCompDateTime = () => {
                var tz = TimeZoneInfo.CreateCustomTimeZone(
                    deviceTimeZone.Id,
                    deviceTimeZone.BaseUtcOffset,
                    deviceTimeZone.DisplayName,
                    deviceTimeZone.StandardName,
                    deviceTimeZone.DaylightName,
                    deviceTimeZone.GetAdjustmentRules(),
                    !autoAdjustDaylight
                );
                var compDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
                compTimeValue.Text = compDateTime.ToString("T");
                compDateValue.Text = compDateTime.ToString("d");
            };
            UpdateCompDateTime();
            OnDeviceTimeZoneChanged += () => {
                UpdateCompDateTime();
            };
            OnAutoAdjustDaylightChanged += () => {
                UpdateCompDateTime();
            };

            Action UpdateDeviceDateTime = () => {
                var elapsedTicks = Stopwatch.GetTimestamp() - model.timestamp;
                var elapsedMilliseconds = (double)elapsedTicks * 1000.0 / (double)Stopwatch.Frequency;
                var devDateTime = this.deviceDateTime.Add(TimeSpan.FromMilliseconds(elapsedMilliseconds));
                deviceTimeValue.Text = devDateTime.ToString("T");
                deviceDateValue.Text = devDateTime.ToString("d");
            };
            UpdateDeviceDateTime();

            disposables.Add(
                Observable
                    .Interval(TimeSpan.FromMilliseconds(500))
                    .ObserveOnDispatcher()
                    .Subscribe(ticks => {
                        try {
                            UpdateDeviceDateTime();
                            UpdateCompDateTime();
                        } catch (Exception err) {
                            dbg.Error(err);
                        }
                    })
            );

            
            newManualTimeValue.SelectedTimeChanged += (s, a) => {
                SelectedTime = newManualTimeValue.SelectedTime;
            };
            
            newManualDateValue.SelectedDateChanged += (s, a) => {
                SelectedDate = newManualDateValue.SelectedDate.Value;
            };

        }
        #endregion Binding

		void Init() {
			Action UpdateDateTime = () => {
				ComputerTime = System.DateTime.Now.ToString("T");
				ComputerDate = System.DateTime.Now.ToString("d");
				//DeviceTime = 
				//DeviceDate = 
			};
			UpdateDateTime();
			disposables.Add(
				Observable
					.Interval(TimeSpan.FromMilliseconds(500))
					.ObserveOnDispatcher()
					.Subscribe(x => {
						UpdateDateTime();
					})
			);
			SelectedDate = System.DateTime.Now;
			SelectedTime = System.DateTime.Now.TimeOfDay;
		}
		public class TZInfoDescriptor {
			public TZInfoDescriptor() {
			}
			public override string ToString() {
				return Name;
			}
			List<TimeZoneInfo> tzinfoList = new List<TimeZoneInfo>();
			public List<TimeZoneInfo> TzinfoList {
				get { return tzinfoList; }
			}
			public void AddTimeZineInfo(TimeZoneInfo tzinfo) {
				tzinfoList.Add(tzinfo);
			}
			public TimeZoneInfo GetLastTimeZone() {
				if (tzinfoList.Count == 0)
					return null;
				return tzinfoList.Last();
			}
			public string Name {
				get {
					string displName = "";
					if(tzinfoList.Count == 0)
						return displName;

					//displName += "(" + tzinfoList[0].BaseUtcOffset.Hours + ":" + tzinfoList[0].BaseUtcOffset.Minutes + ":" + tzinfoList[0].BaseUtcOffset.Seconds + ") ";
					tzinfoList.ForEach(tz => {
						displName += tz.DisplayName + ",";
					});
					displName.TrimEnd(',');

					return displName;
				}
			}
			public string RawPosizString {
				get {
					string res = PosixTimeZone.GetPosixTimeZone(tzinfoList[0]);
					return res;
				}
				set { 
				}
			}
			public string Id{
				get{
					if(tzinfoList.Count != 0)
						return tzinfoList[0].Id;
					return "0";
				}
			}
			public string DisplayName{
				get{
					return Name;
				}
			}
			public string StandardName{
				get{
					return tzinfoList[0].StandardName;
				}
			}
			public string DaylightName{
				get{
					return tzinfoList[0].DaylightName;
				}
			}
			public TimeSpan BaseUtcOffset{
				get{
					return tzinfoList[0].BaseUtcOffset;
				}
			}
			public TimeZoneInfo.AdjustmentRule[] GetAdjustmentRules() {
				return tzinfoList[0].GetAdjustmentRules();
			}
		}
		class TZInfoDescriptorManager {
			public static Dictionary<string, TZInfoDescriptor> Resolve(ReadOnlyCollection<TimeZoneInfo> tzInfoList) {
				Dictionary<string, TZInfoDescriptor> dictTZInfo = new Dictionary<string, TZInfoDescriptor>();

				tzInfoList.ForEach(tzi => {
					string key = PosixTimeZone.GetPosixTimeZone(tzi);
					if (dictTZInfo.ContainsKey(key)) {
						dictTZInfo[key].AddTimeZineInfo(tzi);
					} else {
						dictTZInfo.Add(key, new TZInfoDescriptor());
						dictTZInfo[key].AddTimeZineInfo(tzi);
					}
				});

				return dictTZInfo;
			}
		}
		void InitTimeZones() {
			//current timezone in windows format
			var initialPosTZ = PosixTimeZone.Convert(DeviceTimeZoneOriginal);

			//init timezones list
			var tzInfoList = TimeZoneInfo.GetSystemTimeZones();

			var tzinfoDict = TZInfoDescriptorManager.Resolve(tzInfoList);

			var normTZ = PosixTimeZone.GetNormalizeString(DeviceTimeZoneOriginal);

			if (tzinfoDict.ContainsKey(normTZ)) {
				deviceTimeZone = tzinfoDict[normTZ];
			} else { 
				TimeZoneInfo tzi = TimeZoneInfo.CreateCustomTimeZone(
					Guid.NewGuid().ToString(),
					new TimeSpan(
						initialPosTZ.stdOffset.hours,
						initialPosTZ.stdOffset.minutes,
						initialPosTZ.stdOffset.seconds
					),
					DeviceTimeZoneOriginal,
					DeviceTimeZoneOriginal
				);
				var tzdes = new TZInfoDescriptor();
				tzdes.AddTimeZineInfo(tzi);
				tzinfoDict.Add(normTZ, tzdes);
				deviceTimeZone = tzdes;
			}

			//List<TimeZoneInfo> tzInfoListMutable = tzInfoList.ToList();
			//var ret = tzInfoListMutable.FirstOrDefault(x => {
			//    var posString = PosixTimeZone.GetPosixTimeZone(x);
			//    var tmpTZ = PosixTimeZone.Convert(posString);
			//    return PosixTimeZone.Compare(initialPosTZ, tmpTZ);
			//});

			//if (ret != null) {
			//    deviceTimeZone = ret;
			//} else {
			//    TimeZoneInfo tzi = TimeZoneInfo.CreateCustomTimeZone(
			//        Guid.NewGuid().ToString(), 
			//        new TimeSpan(
			//            initialPosTZ.stdOffset.hours, 
			//            initialPosTZ.stdOffset.minutes, 
			//            initialPosTZ.stdOffset.seconds
			//        ),
			//        DeviceTimeZoneOriginal, 
			//        DeviceTimeZoneOriginal
			//    );
			//    tzInfoListMutable.Add(tzi);
			//    deviceTimeZone = tzi;
			//}

			timeZones.Clear();
			tzinfoDict.Values.ForEach(x => {
				timeZones.Add(x);
			});

		}

        void OnRevertChanges() {
            model.RevertChanges();
            disposables.Dispose();
            disposables = new CompositeDisposable();
            InitTimeZones();
            Init();
            BindModel(model);
        }

		void OnApplyChanges() {
			//var tzinfo = DeviceTimeZone;
			model.timeZone = deviceTimeZone.RawPosizString;
			model.daylightSavings = autoAdjustDaylight;
			switch (setDateTimeMode) {
				case SetDateTimeMode.SyncWithComp:
					model.useDateTimeFromNtp = false;
					model.utcDateTime = DateTime.UtcNow;
					break;
				case SetDateTimeMode.SetManually:
					model.useDateTimeFromNtp = false;
					var dateTime = new DateTime(
						SelectedDate.Year, 
						SelectedDate.Month, 
						SelectedDate.Day, 
						SelectedTime.Hours,
						SelectedTime.Minutes,
						SelectedTime.Seconds,
						DateTimeKind.Unspecified
					);
					var tz = TimeZoneInfo.CreateCustomTimeZone(
						deviceTimeZone.Id,
						deviceTimeZone.BaseUtcOffset,
						deviceTimeZone.DisplayName,
						deviceTimeZone.StandardName,
						deviceTimeZone.DaylightName,
						deviceTimeZone.GetAdjustmentRules(),
						!autoAdjustDaylight
					);
					model.utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, tz);
					break;
				case SetDateTimeMode.SyncWithNtp:
					model.useDateTimeFromNtp = true;
					break;
			}
			Success(new Result.Apply(model));
		} 

		public void Dispose() {
			Cancel();
		}
	}
}
