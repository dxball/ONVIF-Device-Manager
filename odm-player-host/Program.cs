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
using odm.utils;

namespace odm.player {
	
	static class Program {
		static string pipeUri;
		static int parentPID;

		delegate uint UnhandledExceptionHandler(IntPtr ExceptionPointers);
		[DllImport("kernel32.dll")]
		static extern UnhandledExceptionHandler SetUnhandledExceptionFilter(UnhandledExceptionHandler lpTopLevelExceptionFilter);

		//static UnhandledExceptionHandler handler = null;
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

			CommandLineArgs commandLineArgs = null;
			try {
				commandLineArgs = CommandLineArgs.Parse(args);
			} catch (Exception err) {
				log.WriteError(err.Message);
			}
			
			if (commandLineArgs == null || commandLineArgs.Count == 0) {
				log.WriteError("incorrect command line syntax, should be in format: odm-player-host.exe /server-pipe:<pipe-uri> /parent-pid:<parent process id>");
				return;
			}

			try {
				pipeUri = commandLineArgs.GetParamAsString("server-pipe");
				parentPID = commandLineArgs.GetParamAsInt("parent-pid");
			} catch (Exception err) {
				log.WriteInfo(err.Message);
				return;
			}
			
			try {
				var actFlow = new ActionFlow();
				var cleanupQueue = new Queue<Action>();
				var playerInstance = new PlayerService(new ActionFlowScheduler(actFlow), () => {
					lock (cleanupQueue) {
						while (cleanupQueue.Count > 0) {
							try {
								cleanupQueue.Dequeue()();
							} catch(Exception err) {
								//TODO: log error
								log.WriteInfo("error: {0}", err.Message);
							}
						}
					}
					actFlow.Exit();
				});
				
				// Create a ServiceHost for the CalculatorService type and provide the base address.
				using (ServiceHost serviceHost = new ServiceHost(playerInstance)) {
					NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
					serviceHost.AddServiceEndpoint(typeof(IPlayer), binding, pipeUri);
					lock (cleanupQueue) {
						cleanupQueue.Enqueue(() => {
							log.WriteInfo("stopping service....");
							serviceHost.Close();
						});
					}
					serviceHost.Open();

					actFlow.Invoke(() => {
						log.WriteInfo("The service is ready.");						
					});

					//Trace.Listeners.Add(new ObservableTraceListener());

					var loggerSwitch = new TraceSwitch("logger", null);
					ObservableTraceListener
						.GetLogMessages()
						.Subscribe(logMsg => {
							actFlow.Invoke(() => {
								playerInstance.NotifyLogMessage(logMsg);								
							});
						});

					//start watchdog timer
					var subscr = new MutableDisposable();
					subscr.Disposable = Observable
						.Interval(TimeSpan.FromMilliseconds(200))
						.Subscribe(t => {
							//LogUtils.WriteEvent("watchdog round", null, TraceEventType.Verbose);
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
							actFlow.Invoke(() => {
								log.WriteEvent("stopping player service....", null, TraceEventType.Verbose);
								serviceHost.Close();
								actFlow.Exit();
							});
						}, err => {
							dbg.Error(err);
							actFlow.Invoke(() => {
								serviceHost.Close();
								actFlow.Exit();
							});
						}, () => {
							dbg.Error("unexpected completion of watchdog timer");
							actFlow.Invoke(() => {
								serviceHost.Close();
								actFlow.Exit();
							});
						});
					actFlow.Run();
					log.WriteEvent("shutdown....", null, TraceEventType.Verbose);							
				}

			} catch (Exception err) {
				log.WriteError(err.Message);
			}

		}
		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e){
			log.WriteError("unhandled exception was caught");
			Process.GetCurrentProcess().Kill();
			//Application.EnableVisualStyles();
			//Exception exp = e.ExceptionObject as Exception;
			//if (exp == null) return;
			//new ThreadExceptionDialog(exp).ShowDialog();
			//Environment.Exit(exp.GetHashCode());
		}
	}
}


