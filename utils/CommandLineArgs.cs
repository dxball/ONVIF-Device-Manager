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
