// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.

using System;
using System.Threading;
using System.Windows.Forms;
using Common.Logging;

namespace SimplePlayer
{
    internal static class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            try {
                Main_();
            } catch (Exception exc) {
                if (logger.IsFatalEnabled) {
                    logger.Fatal("An unhandled exception has been intercepted in Main() method.");
                }
                //
                MessageBox.Show("An unhandled exception occured. Program is terminating.\r\nView log for additional information.",
                    "Abnormal program termination", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //
                handleExceptionObject(exc);
            }
        }

        private static void Main_() {
            Application.ThreadException += ApplicationOnThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            //
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs args) {
            if (logger.IsFatalEnabled) {
                logger.Fatal("ApplicationOnThreadException handler called.");
            }
            //
            handleExceptionObject(args.Exception);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args) {
            if (logger.IsFatalEnabled) {
                logger.Fatal("AppDomainOnUnhandledException handler called.");
            }
            //
            handleExceptionObject(args.ExceptionObject);
        }

        private static void handleExceptionObject(object exceptionObject) {
            if (logger.IsFatalEnabled) {
                if (exceptionObject != null) {
                    if (exceptionObject is Exception) {
                        logger.Fatal("An unhandled exception occured. Program is terminating.", (Exception) exceptionObject);
                    } else {
                        logger.Fatal(String.Format("An unhandled exception occured. Program is terminating. Exception object : {0}", exceptionObject));
                    }
                }
            }
            //
            Application.Exit();
            Environment.Exit(-1);
        }
    }
}
