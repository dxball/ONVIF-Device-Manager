using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using utils;

namespace odm.controllers {

	/// <summary>
	/// Class represents a time shift. Used when converting time zones from the Posix format.
	/// </summary>
	public class offset {
		public int hours { get; set; }
		public int minutes { get; set; }
		public int seconds { get; set; }
	}
	/// <summary>
	/// Class represents a rule for start-stop daylighting time. Used when converting time zones from the Posix format.
	/// </summary>
	public class rules {
		//settings for different (3) types of rule settings.
		//value -1 sets if rule did not used
		public int JulianDayNumber = -1;
		public int ZerroBasedJulianDayNumber = -1;
		public int MonthNum = -1;
		public int DayNum = -1;
		public int WeekNum = -1;

		//offset for current rule
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
	/// <summary>
	/// This class used to create timezone from Posix raw format.
	/// It provide all methods to operate with TZ on device
	/// </summary>
	public class PosixTimeZone {
		public static string GetNormalizeString(string rawData) {
			string outstr = "";
			var ptz = Convert(rawData);

			if (ptz.dst == "") {
 				//dst not used
				outstr = ptz.std +
					ptz.stdOffset.hours * (-1) + ":" +
					ptz.stdOffset.minutes + ":" +
					ptz.stdOffset.seconds;
			} else if (ptz.startRule.MonthNum == -1) {
				//no rules available
				outstr = ptz.std +
					ptz.stdOffset.hours * (-1) + ":" +
					ptz.stdOffset.minutes + ":" +
					ptz.stdOffset.seconds +
					ptz.dst +
					ptz.dstOffset.hours * (-1) + ":" +
					ptz.dstOffset.minutes + ":" +
					ptz.dstOffset.seconds;
			} else {
				outstr = ptz.std +
					ptz.stdOffset.hours * (-1) + ":" +
					ptz.stdOffset.minutes + ":" +
					ptz.stdOffset.seconds +
					ptz.dst +
					ptz.dstOffset.hours * (-1) + ":" +
					ptz.dstOffset.minutes + ":" +
					ptz.dstOffset.seconds + ",M" +
					ptz.startRule.MonthNum + "." +
					ptz.startRule.WeekNum + "." +
					ptz.startRule.DayNum + "/" +
					ptz.startRule.Offset.hours * (-1) + ":" +
					ptz.startRule.Offset.minutes + ":" +
					ptz.startRule.Offset.seconds + ",M" +
					ptz.stopRule.MonthNum + "." +
					ptz.stopRule.WeekNum + "." +
					ptz.stopRule.DayNum + "/" +
					ptz.stopRule.Offset.hours * (-1) + ":" +
					ptz.stopRule.Offset.minutes + ":" +
					ptz.stopRule.Offset.seconds;
			}


			return outstr;
		}
		/// <summary>
		/// Create PosixTimeZone class from posix raw data
		/// </summary>
		/// <param name="rawData">string in Posix format (for example): "EST2"</param>
		/// <returns></returns>
		public static PosixTimeZone Convert(string rawData) {
			PosixTimeZone tz = new PosixTimeZone();
			char sep_char = ',';
			var separatedParts = rawData.Split(sep_char);
			//First parameter are required
            try {
                ParseTimeZone(separatedParts[0], tz);

                if (separatedParts.Length > 1)
                    ParseRules(separatedParts[1], tz.startRule);
                if (separatedParts.Length > 2)
                    ParseRules(separatedParts[2], tz.stopRule);
            } catch (Exception err) {
                dbg.Error(err);
            }
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
				ost.hours = tmpVal * (-1);
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
			if (rawData == "") {
				return;
			}

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

				ParseOffset(stdOffset, tz.stdOffset);

				if (tz.dst.Length != 0) {
					rawData = rawData.Remove(0, tz.dst.Length);
					rawData.TakeWhile(x => !char.IsLetter(x)).ToList().ForEach(x => dstOffset += x);
					if (rawData == "") {
						tz.dstOffset.hours = tz.stdOffset.hours +1;
						tz.dstOffset.minutes = tz.stdOffset.minutes;
						tz.dstOffset.seconds = tz.stdOffset.seconds;
					} else {
						ParseOffset(dstOffset, tz.dstOffset);
					}
				}
			}
		}
		/// <summary>
		/// Returns timezone in Posix format from Windows structure.
		/// </summary>
		/// <param name="wndTimeZone">Windows based timezone class</param>
		/// <returns></returns>
		public static string GetPosixTimeZone(TimeZoneInfo wndTimeZone) {
			string rawPosixData = "";

			rawPosixData = ParseTimeZoneToRaw(wndTimeZone);
			var rules = wndTimeZone.GetAdjustmentRules();
			if (rules.Length > 0)
				rawPosixData = rawPosixData + ParseRulesToRaw(rules[0], wndTimeZone);
			return rawPosixData;
		}
        /// <summary>
        /// Convert to posix
        /// </summary>
        /// <param name="tz"></param>
        /// <returns></returns>
		static string ParseTimeZoneToRaw(TimeZoneInfo tz) {
			//posix invertion
            string rawtz = "std" + tz.BaseUtcOffset.Hours * (-1) + ":" + Math.Abs(tz.BaseUtcOffset.Minutes) + ":" + Math.Abs(tz.BaseUtcOffset.Seconds);
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
		static string ParseRulesToRaw(TimeZoneInfo.AdjustmentRule rule, TimeZoneInfo tz) {
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

			int deltaH = tz.BaseUtcOffset.Hours + rule.DaylightDelta.Hours;
			int deltaM = tz.BaseUtcOffset.Minutes + rule.DaylightDelta.Minutes;
			int deltaS = tz.BaseUtcOffset.Seconds + rule.DaylightDelta.Seconds;

			DaylightDelta = "dst" + deltaH * (-1) + ":" + Math.Abs(deltaM) + ":" + Math.Abs(deltaS);

			RawData = DaylightDelta + "," + RawDataStart + "," + RawDataStop;
			return RawData;
		}
		/// <summary>
		/// Compares two Posix TimeZone classes.
		/// Comparation provided for every parameter of the class
		/// </summary>
		/// <param name="tz1">First comparable value</param>
		/// <param name="tz2">Second comparable value</param>
		/// <returns></returns>
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
		/// <summary>
		/// Returns DateTime value for given timezone
		/// </summary>
		/// <param name="td">datetime in local TZ</param>
		/// <param name="tz">desired time zone</param>
		/// <returns></returns>
		public static DateTime GetDeviceTimeWithTimeZone(DateTime td, TimeZoneInfo tz) {
			return TimeZoneInfo.ConvertTime(td, tz);
		}
		/// <summary>
		/// Return UTC time from given local time
		/// </summary>
		/// <param name="localTime">local time</param>
		/// <returns></returns>
		public DateTime GetUTCDeviceTime(DateTime localTime){
			return localTime.ToUniversalTime();
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
