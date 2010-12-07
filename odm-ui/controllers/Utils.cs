using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nvc.controllers {
	public class offset {
		public int hours { get; set; }
		public int minutes { get; set; }
		public int seconds { get; set; }
	}
	public class rules {
		public int JulianDayNumber = -1;
		public int ZerroBasedJulianDayNumber = -1;
		public int MonthNum = -1;
		public int DayNum = -1;
		public int WeekNum = -1;

		offset _offset;
		public offset Offset {
			get {
				if (_offset == null)
					_offset = new offset();
				return _offset;
			}
			set {
				_offset = value;
			}
		}
	}
	public class PosixTimeZone {
		public static PosixTimeZone Convert(string rawData) {
			PosixTimeZone tz = new PosixTimeZone();
			char sep_char = ',';
			var separatedParts = rawData.Split(sep_char);
			//First parameter are required
			ParseTimeZone(separatedParts[0], tz);

			if (separatedParts.Length > 1)
				ParseRules(separatedParts[1], tz.startRule);
			if (separatedParts.Length > 2)
				ParseRules(separatedParts[2], tz.startRule);
			return tz;
		}
		static void ParseRules(string rawData, rules rule) {
			//Check rule type
			switch (rawData.ElementAt(0)) {
				case 'J':
					rawData = rawData.Remove(0, 1);
					var retval = rawData.Split('/');
					int tmpVal;
					Int32.TryParse(retval[0], out tmpVal);
					rule.JulianDayNumber = tmpVal;
					if (retval.Length == 1)
						ParseOffset("02:00:00", rule.Offset);
					else
						ParseOffset(retval[1], rule.Offset);
					break;
				case 'M'://M10.1.0
					rawData = rawData.Remove(0, 1);
					var mretval = rawData.Split('/');
					string rawDate = mretval[0];
					string rawOffset = "02:00:00";
					if (mretval.Length == 2) {
						rawOffset = mretval[1];
					}
					int mtmpVal;

					var mrawDate = rawDate.Split('.');
					Int32.TryParse(mrawDate[0], out mtmpVal);
					rule.MonthNum = mtmpVal;
					Int32.TryParse(mrawDate[1], out mtmpVal);
					rule.WeekNum = mtmpVal;
					Int32.TryParse(mrawDate[2], out mtmpVal);
					rule.DayNum = mtmpVal;

					ParseOffset(rawOffset, rule.Offset);

					break;
				default:
					var defretval = rawData.Split('/');
					int deftmpVal;
					Int32.TryParse(defretval[0], out deftmpVal);
					rule.ZerroBasedJulianDayNumber = deftmpVal;
					if (defretval.Length == 1)
						ParseOffset("02:00:00", rule.Offset);
					else
						ParseOffset(defretval[1], rule.Offset);
					break;
			}
		}
		static void ParseOffset(string Offset, offset ost) {
			int tmpVal;
			var retval = Offset.Split(':');
			if (retval.Length > 0) {
				Int32.TryParse(retval[0], out tmpVal);
				ost.hours = tmpVal;
			}
			if (retval.Length > 1) {
				Int32.TryParse(retval[1], out tmpVal);
				ost.minutes = tmpVal;
			}
			if (retval.Length == 3) {
				Int32.TryParse(retval[2], out tmpVal);
				ost.seconds = tmpVal;
			}
		}
		static void ParseTimeZone(string rawData, PosixTimeZone tz) {
			string stdOffset = "";
			string dstOffset = "";

			if (rawData.ElementAt(0) == '<') {
				//quoted issue
				string[] separators = { "<", ">" };
				var retres = rawData.Split(separators, StringSplitOptions.RemoveEmptyEntries);

				if (retres.Length > 1) {
					tz.std = retres[0];
					stdOffset = retres[1];
					ParseOffset(stdOffset, tz.stdOffset);
				}
				if (retres.Length > 2) {
					tz.dst = retres[2];
				}
				if (retres.Length == 3) {
					dstOffset = "1";
					ParseOffset(dstOffset, tz.dstOffset);
				}
				if (retres.Length > 3) {
					dstOffset = retres[3];
					ParseOffset(dstOffset, tz.dstOffset);
				}
			} else {//WST-8WDT-9
				tz.std = "";
				tz.dst = "";

				rawData.TakeWhile(x => char.IsLetter(x)).ToList().ForEach(x => tz.std += x);
				rawData = rawData.Remove(0, tz.std.Length);
				rawData.TakeWhile(x => !char.IsLetter(x)).ToList().ForEach(x => stdOffset += x);
				rawData = rawData.Remove(0, stdOffset.Length);

				rawData.TakeWhile(x => char.IsLetter(x)).ToList().ForEach(x => tz.dst += x);
				if (tz.dst.Length != 0) {
					rawData = rawData.Remove(0, tz.dst.Length);
					rawData.TakeWhile(x => !char.IsLetter(x)).ToList().ForEach(x => dstOffset += x);
					if (dstOffset == "")
						dstOffset = "1";
				}

				ParseOffset(stdOffset, tz.stdOffset);
				ParseOffset(dstOffset, tz.dstOffset);
			}
		}
		public static string GetPosixTimeZone(TimeZoneInfo wndTimeZone) {
			string rawPosixData = "";

			rawPosixData = ParseTimeZoneToRaw(wndTimeZone);
			var rules = wndTimeZone.GetAdjustmentRules();
			if (rules.Length > 0)
				rawPosixData = rawPosixData + ParseRulesToRaw(rules[0]);
			return rawPosixData;
		}
		static string ParseTimeZoneToRaw(TimeZoneInfo tz) {
			string rawtz = "std" + tz.BaseUtcOffset.Hours + ":" + tz.BaseUtcOffset.Minutes + ":" + tz.BaseUtcOffset.Seconds;
			return rawtz;
		}
		static int GetDaOfWeekPosixNumber(DayOfWeek dw) {
			int retval = 0;
			switch (dw) {
				case DayOfWeek.Sunday:
					retval = 0;
					break;
				case DayOfWeek.Monday:
					retval = 1;
					break;
				case DayOfWeek.Tuesday:
					retval = 2;
					break;
				case DayOfWeek.Thursday:
					retval = 3;
					break;
				case DayOfWeek.Wednesday:
					retval = 4;
					break;
				case DayOfWeek.Friday:
					retval = 5;
					break;
				case DayOfWeek.Saturday:
					retval = 6;
					break;
				default:
					retval = 8;
					break;
			}
			return retval;
		}
		static string ParseRulesToRaw(TimeZoneInfo.AdjustmentRule rule) {
			string RawDataStart = "";
			string RawDataStop = "";
			string DaylightDelta = ""; 
			string RawData ="";
			DateTime time;

			if (rule.DaylightTransitionStart.IsFixedDateRule) {
				RawDataStart = "J" + rule.DaylightTransitionStart.Day.ToString();

				time = rule.DaylightTransitionStart.TimeOfDay;
				RawDataStart = RawDataStart + "/" + time.Hour + ":" + time.Minute + ":" + time.Second;
			} else {
				RawDataStart = "M" + rule.DaylightTransitionStart.Month + "." + rule.DaylightTransitionStart.Week
					+ "." + GetDaOfWeekPosixNumber(rule.DaylightTransitionStart.DayOfWeek);

				time = rule.DaylightTransitionStart.TimeOfDay;
				RawDataStart = RawDataStart + "/" + time.Hour + ":" + time.Minute + ":" + time.Second;
			}
			if (rule.DaylightTransitionEnd.IsFixedDateRule) {
				RawDataStop = "J" + rule.DaylightTransitionEnd.Day.ToString();

				time = rule.DaylightTransitionEnd.TimeOfDay;
				RawDataStop = RawDataStop + "/" + time.Hour + ":" + time.Minute + ":" + time.Second;
			} else {
				RawDataStop = "M" + rule.DaylightTransitionEnd.Month + "." + rule.DaylightTransitionEnd.Week
					+ "." + GetDaOfWeekPosixNumber(rule.DaylightTransitionEnd.DayOfWeek);

				time = rule.DaylightTransitionEnd.TimeOfDay;
				RawDataStop = RawDataStop + "/" + time.Hour + ":" + time.Minute + ":" + time.Second;
			}

			DaylightDelta = "dst" + rule.DaylightDelta.Hours + ":" + rule.DaylightDelta.Minutes + ":" + rule.DaylightDelta.Seconds;

			RawData = DaylightDelta + "," + RawDataStart + "," + RawDataStop;
			return RawData;
		}
		public static bool Compare(PosixTimeZone tz1, PosixTimeZone tz2) {
			bool isEqual = false;

			if (IsOffsetEqual(tz1.stdOffset, tz2.stdOffset))
				if (IsOffsetEqual(tz1.dstOffset, tz2.dstOffset))
					if (IsRulesEquals(tz1.startRule, tz2.startRule))
						if (IsRulesEquals(tz1.stopRule, tz2.stopRule))
							isEqual = true;
			return isEqual;
		}
		static bool IsRulesEquals(rules rul1, rules rul2) {
			if (rul1.DayNum == rul2.DayNum &&
				rul1.JulianDayNumber == rul2.JulianDayNumber &&
				rul1.MonthNum == rul2.MonthNum &&
				rul1.WeekNum == rul2.WeekNum &&
				rul1.ZerroBasedJulianDayNumber == rul2.ZerroBasedJulianDayNumber &&
				IsOffsetEqual(rul1.Offset, rul2.Offset))
				return true;
			return false;
		}
		static bool IsOffsetEqual(offset off1, offset off2) {
			if (off1.hours == off2.hours && off1.minutes == off2.minutes && off1.seconds == off2.seconds)
				return true;
			return false;
		}

		public static DateTime GetDeviceTimeWithTimeZone(DateTime td, TimeZoneInfo tz) {
			return TimeZoneInfo.ConvertTime(td, tz);
		}
		
		public DateTime GetUTCDeviceTime(DateTime localTime){
			return localTime;
		}

		offset _stdOffset;
		public offset stdOffset {
			get {
				if (_stdOffset == null)
					_stdOffset = new offset();
				return _stdOffset;
			}
			set {
				_stdOffset = value;
			}
		}
		offset _dstOffset;
		public offset dstOffset {
			get {
				if (_dstOffset == null)
					_dstOffset = new offset();
				return _dstOffset;
			}
			set {
				_dstOffset = value;
			}
		}
		string _std = "";
		public string std { get { return _std; } set { _std = value; } }
		string _dst = "";
		public string dst { get { return _dst; } set { _dst = value; } }
		rules _startRule;
		public rules startRule {
			get {
				if (_startRule == null)
					_startRule = new rules();
				return _startRule;
			}
			set {
				_startRule = value;
			}
		}
		rules _stopRule;
		public rules stopRule {
			get {
				if (_stopRule == null)
					_stopRule = new rules();
				return _stopRule;
			}
			set {
				_stopRule = value;
			}
		}
	}
}
