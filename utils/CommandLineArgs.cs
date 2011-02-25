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
using System.Text.RegularExpressions;

namespace odm.utils {

	public class CommandLineArgs : Dictionary<String, List<String>> {

		public int GetParamAsInt(string paramName) {
			List<string> val = null;
			if (!TryGetValue(paramName, out val) || val == null || val.Count == 0) {
				throw new Exception(String.Format("parameter {0} is not specified", paramName));
			}
			if (val.Count > 1) {
				throw new Exception(String.Format("parameter {0} is specified more than one time", paramName));
			}
			try {
				return int.Parse(val.First());
			} catch {
				throw new Exception(String.Format("parameter {0} is not valid", paramName));
			}
		}

		public string GetParamAsString(string paramName) {
			List<string> val = null;
			if (!TryGetValue(paramName, out val) || val == null || val.Count == 0) {
				throw new Exception(String.Format("parameter {0} is not specified", paramName));
			}
			if (val.Count > 1) {
				throw new Exception(String.Format("parameter {0} is specified more than one time", paramName));
			}
			return val.First();
		}

		public static CommandLineArgs Parse(String[] args) {
			var commandLineArgs = new CommandLineArgs();
			if (args.Length == 0) {
				return commandLineArgs;
			}

			String pattern = @"^/(?<argname>[A-Za-z0-9_-]+):(?<argvalue>.+)$";
			foreach (string x in args) {
				Match match = Regex.Match(x, pattern);

				if (!match.Success) {
					throw new Exception("failed to parse command line");
				}
				String argname = match.Groups["argname"].Value.ToLower();
				List<String> values = null;
				if (!commandLineArgs.TryGetValue(argname, out values)) {
					values = new List<String>();
					commandLineArgs.Add(argname, values);
				}
				var s = match.Groups["argvalue"].Value;
				values.Add(match.Groups["argvalue"].Value);
			}
			return commandLineArgs;
		}
	}
}
