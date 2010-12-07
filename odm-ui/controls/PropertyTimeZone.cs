using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nvc.models;
using System.Collections.ObjectModel;
using nvc.controllers;

namespace nvc.controls {
	public partial class PropertyTimeZone : BasePropertyControl {
		PropertyTimeZoneStrings _strings = new PropertyTimeZoneStrings();
		DateTimeSettingsModel _devModel;

		public Action onApply { get; set; }

		public PropertyTimeZone(DateTimeSettingsModel devModel) {
			InitializeComponent();
			_devModel = devModel;

			//InitialPosixTimeZone = TimeZone;

			Load += new EventHandler(PropertyTimeZone_Load);
		}

		void PropertyTimeZone_Load(object sender, EventArgs e) {
			Localization();
			InitControl();
			BindData(_devModel);
		}

		void BindData(DateTimeSettingsModel devModel) {
			_chbAutoDaylightSave.CreateBinding(x=>x.Checked, devModel, x=>x.daylightSavings);

			_saveCancelControl._btnCancel.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
			_saveCancelControl._btnSave.CreateBinding(x => x.Enabled, devModel, x => x.isModified);
		}

		ReadOnlyCollection<TimeZoneInfo> _tzInfoList;
		List<TimeZoneInfo> tzInfoListMutable;
		void InitControl() {
			_rbComp.CheckedChanged += new EventHandler(_rbCheckedChanged);
			_rbNtp.CheckedChanged += new EventHandler(_rbCheckedChanged);
			_rbManual.CheckedChanged += new EventHandler(_rbCheckedChanged);
			_tbManualDate.ValueChanged += new EventHandler(_tbManualDate_ValueChanged);
			_tbCurrentTime.ValueChanged += new EventHandler(_tbManualDate_ValueChanged);
			
			//_tbCurrentTime;
			var initialPosTZ = PosixTimeZone.Convert(_devModel.timeZone);

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
			
			
			_cmbTimeZone.DataSource = tzInfoListMutable;
			_cmbTimeZone.DisplayMember = "DisplayName";
			_cmbTimeZone.SelectedItem = selectedItem;

			_tbCurrentDate.Value = PosixTimeZone.GetDeviceTimeWithTimeZone(_devModel.dateTime, (TimeZoneInfo)_cmbTimeZone.SelectedItem);

			_cmbTimeZone.SelectionChangeCommitted += new EventHandler(_cmbTimeZone_SelectionChangeCommitted);
			_saveCancelControl._btnSave.Click += new EventHandler(_btnSave_Click);
			_saveCancelControl._btnCancel.Click += new EventHandler(_btnCancel_Click);

			_saveCancelControl._btnCancel.Enabled = false;
			_saveCancelControl._btnSave.Enabled = true;
		}

		void _tbManualDate_ValueChanged(object sender, EventArgs e) {
			_saveCancelControl._btnSave.Enabled = true;
		}

		void _rbCheckedChanged(object sender, EventArgs e) {
			_saveCancelControl._btnSave.Enabled = true;
		}

		void _btnCancel_Click(object sender, EventArgs e) {
			
		}

		void _btnSave_Click(object sender, EventArgs e) {
			GetSelectedValues();
			if (onApply != null)
				onApply();
		}

		void GetSelectedValues() {
			DateTime UtcDateTime = DateTime.UtcNow;
			if (_rbComp.Checked) {
				UtcDateTime = DateTime.UtcNow;
			}
			if (_rbManual.Checked) {
				UtcDateTime = new DateTime(_tbManualDate.Value.Year,
					_tbManualDate.Value.Month, _tbManualDate.Value.Day,
					_tbManualTime.Value.Hour, _tbManualTime.Value.Minute,
					_tbManualTime.Value.Second, DateTimeKind.Utc);
			}
			if (_rbNtp.Checked) {
				UtcDateTime = DateTime.UtcNow;
			}
			_devModel.dateTime = UtcDateTime;
			var tzinfo = (TimeZoneInfo)_cmbTimeZone.SelectedItem;
			if(tzinfo.Id != "0")
				_devModel.timeZone = PosixTimeZone.GetPosixTimeZone(tzinfo);
		}

		void _cmbTimeZone_SelectionChangeCommitted(object sender, EventArgs e) {
			TimeZoneInfo tzInfo = (TimeZoneInfo)_cmbTimeZone.SelectedValue;
			_devModel.timeZone = PosixTimeZone.GetPosixTimeZone(tzInfo);
		}

		void Localization() {
			_titleCurrentTime.CreateBinding(x => x.Text, _strings, x => x.currentTime);
			_titleNewTime.CreateBinding(x => x.Text, _strings, x => x.newTime);

			_lblAutoDaylightSave.CreateBinding(x => x.Text, _strings, x => x.autoAdjustString);
			_lblCompDate.CreateBinding(x => x.Text, _strings, x => x.date);
			_lblCompTime.CreateBinding(x => x.Text, _strings, x => x.time);
			_lblCurrentDate.CreateBinding(x => x.Text, _strings, x => x.date);
			_lblCurrentTime.CreateBinding(x => x.Text, _strings, x => x.time);
			_lblManualDate.CreateBinding(x => x.Text, _strings, x => x.date);
			_lblManualTime.CreateBinding(x => x.Text, _strings, x => x.time);
			_lblNtpServer.CreateBinding(x => x.Text, _strings, x => x.NTPserver);
			_lblSetManual.CreateBinding(x => x.Text, _strings, x => x.manually);
			_lblSynchronizeComp.CreateBinding(x => x.Text, _strings, x => x.sinchronizeWithComp);
			_lblSynchronizeNTP.CreateBinding(x => x.Text, _strings, x => x.sinchronizeWithNTP);
			_lblTimeZone.CreateBinding(x => x.Text, _strings, x => x.timeZone);
		}
		public override void ReleaseAll() {
		}
	}
}
