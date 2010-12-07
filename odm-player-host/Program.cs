using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Drawing.Imaging;
using System.Runtime.Remoting.Messaging;
using System.Disposables;
using System.Concurrency;
using onvifdm.utils;

namespace onvifdm.player {
	
	static class Program {
		static string pipeUri;
		static int parentPID;

		static Dictionary<String, List<String>> ParseCommandLineArgs(String[] args) {
		var	commandLineArgs = new Dictionary<String, List<String>>();
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

		static int GetParamAsInt(Dictionary<String, List<String>> commandLineArgs, string paramName) {
			List<string> val = null;
			if (!commandLineArgs.TryGetValue(paramName, out val) || val==null || val.Count == 0) {
				throw new Exception(String.Format("parameter {0} is not specified", paramName));
			}
			if(val.Count > 1){
				throw new Exception(String.Format("parameter {0} is specified more than one time", paramName));
			}
			try {
				return int.Parse(val.First());
			} catch {
				throw new Exception(String.Format("parameter {0} is not valid", paramName));
			}		
		}

		static string GetParamAsString(Dictionary<String, List<String>> commandLineArgs, string paramName) {
			List<string> val = null;
			if (!commandLineArgs.TryGetValue(paramName, out val) || val == null || val.Count == 0) {
				throw new Exception(String.Format("parameter {0} is not specified", paramName));
			}
			if (val.Count > 1) {
				throw new Exception(String.Format("parameter {0} is specified more than one time", paramName));
			}
			return val.First();
		}

		delegate uint UnhandledExceptionHandler(IntPtr ExceptionPointers);
		[DllImport("kernel32.dll")]
		static extern UnhandledExceptionHandler SetUnhandledExceptionFilter(UnhandledExceptionHandler lpTopLevelExceptionFilter);

		static UnhandledExceptionHandler handler = null;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			//Trace.Listeners.Add(new ConsoleTraceListener());
			//Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			//handler = ExceptionPointers => {
			//    Process.GetCurrentProcess().Kill();
			//    return 1;
			//};
			//SetUnhandledExceptionFilter(handler);
			
			Dictionary<string, List<string>> commandLineArgs = null;
			try {
				commandLineArgs = ParseCommandLineArgs(args);
			} catch (Exception err) {
				LogUtils.WriteError(err.Message);
			}
			
			if (commandLineArgs == null || commandLineArgs.Count == 0) {
				LogUtils.WriteError("incorrect command line syntax, should be in format: player.exe /server-pipe:<pipe-uri> /parent-pid:<parent process id>");
				return;
			}

			try {
				pipeUri = GetParamAsString(commandLineArgs, "server-pipe");
				parentPID = GetParamAsInt(commandLineArgs, "parent-pid");
			} catch (Exception err) {
				LogUtils.WriteInfo(err.Message);
				return;
			}
			
			try {
				var disp = new Dispatcher();
				var cleanupQueue = new Queue<Action>(); 
				var playerInstance = new PlayerService(() => {
					lock (cleanupQueue) {
						while (cleanupQueue.Count > 0) {
							try {
								cleanupQueue.Dequeue()();
							} catch(Exception err) {
								//TODO: log error
								LogUtils.WriteInfo("error: {0}", err.Message);
							}
						}
					}
					disp.Cancel();
				});
				
				// Create a ServiceHost for the CalculatorService type and provide the base address.
				using (ServiceHost serviceHost = new ServiceHost(playerInstance)) {
					NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
					serviceHost.AddServiceEndpoint(typeof(IPlayer), binding, pipeUri);
					lock (cleanupQueue) {
						cleanupQueue.Enqueue(() => {
							LogUtils.WriteInfo("stopping service....");
							serviceHost.Close();
						});
					}
					serviceHost.Open();
					
					disp.Invoke(() => {
						LogUtils.WriteInfo("The service is ready.");						
					});

					//start watchdog timer
					var subscr = new MutableDisposable();
					subscr.Disposable = Observable
						.Interval(TimeSpan.FromMilliseconds(200))
						.Subscribe(t => {
							var termnate = false;
							try{
								termnate = Process.GetProcessById(parentPID).HasExited;
							}catch{
								termnate = true;
							}
							if(!termnate){
								return;
							}
							subscr.Dispose();
							disp.Invoke(() => {
								serviceHost.Close();
								disp.Cancel();
							});
						}, err => {
							DebugHelper.Error(err);
							disp.Invoke(() => {
								serviceHost.Close();
								disp.Cancel();
							});
						}, () => {
							DebugHelper.Error("unexpected completion of watchdog timer");
							disp.Invoke(() => {
								serviceHost.Close();
								disp.Cancel();
							});
						});
					disp.Run();
					LogUtils.WriteInfo("shutdown....");				
				}

			} catch (Exception err) {
				LogUtils.WriteError(err.Message);
			}

		}
		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e){
			Process.GetCurrentProcess().Kill();
			//Application.EnableVisualStyles();
			//Exception exp = e.ExceptionObject as Exception;
			//if (exp == null) return;
			//new ThreadExceptionDialog(exp).ShowDialog();
			//Environment.Exit(exp.GetHashCode());
		}
	}
}


