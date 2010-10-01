/* This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.
*/

#region Usings

using System;
using System.Threading;
using Common.Logging;
using DZ.MediaPlayer.Filters;
using DZ.MediaPlayer.Io;
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal;
using DZ.MediaPlayer.Vlc.Internal.Interfaces;
using DZ.MediaPlayer.Vlc.Internal.InternalObjects;
using DZ.MediaPlayer.Vlc.Internal.Interop;

#endregion

namespace DZ.MediaPlayer.Vlc
{
    /// <summary>
    /// Implements <see cref="MediaLibraryFactory"/> and manages vlc specific objects internally.
    /// </summary>
    public sealed class VlcMediaLibraryFactory : MediaLibraryFactory, IVolumeManager, ILogVerbosityManager
    {
        /// <summary>
        /// Associated logger. Used primarily to log vlclib logging messages.
        /// </summary>
        private readonly ILog logger;

        /// <summary>
        /// Default name of vlc logger.
        /// </summary>
        public const string DEFAULT_LOGGER_CLASS_NAME = "VlcManagerLogger";

        /// <summary>
        /// Descriptor of libvlc instance retrieved from libvlc module.
        /// </summary>
        private readonly IntPtr descriptor;

        /// <summary>
        /// Instantiates vlc library using specific parameters.
        /// </summary>
        /// <param name="parameters">Parameters for vlc core.</param>
        public VlcMediaLibraryFactory(string[] parameters)
            : this(parameters, DEFAULT_LOGGER_CLASS_NAME) {
        }

        /// <summary>
        /// Instantiates vlc library using specific parameters and logger class name.
        /// </summary>
        /// <param name="parameters">Parameters for vlc core.</param>
        /// <param name="loggerClassName">Name of logger to use for vlc logs.</param>
        public VlcMediaLibraryFactory(string[] parameters, string loggerClassName) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }
            if (loggerClassName == null) {
                throw new ArgumentNullException("loggerClassName");
            }
            if (loggerClassName.Length == 0) {
                throw new ArgumentException("String is empty.", "loggerClassName");
            }
            try {
                descriptor = createInstance(addFilterRequestToParameters(parameters));
                //
                logger = LogManager.GetLogger(loggerClassName);
                vlcLog = new InternalObjectsFactory(descriptor).CreateVlcLog(logger, this);
                initializeAndStartLoggingThread();
            } catch (DllNotFoundException exc) {
                throw new VlcDeploymentException("Vlc library is not deployed. Use VlcDeployment class to deploy vlc libraries.", exc);
            }
        }

        private static string[] addFilterRequestToParameters(string[] parameters) {
            string[] parametersWithAddedFilter = new string[parameters.Length + 2];
            parameters.CopyTo(parametersWithAddedFilter, 0);
            parametersWithAddedFilter[parametersWithAddedFilter.Length - 2] = "--video-filter";
            parametersWithAddedFilter[parametersWithAddedFilter.Length - 1] = "adjust@elwood_adjust";
            //
            return (parametersWithAddedFilter);
        }

        private static IntPtr createInstance(string[] parameters) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            IntPtr res = LibVlcInterop.libvlc_new(parameters, ref exc);
            if (exc.b_raised != 0) {
                throw new VlcInternalException(exc.Message);
            }
            //
            return (res);
        }

        private VlcDoubleWindowFactory doubleWindowFactory;

        /// <summary>
        /// Factory to create vlc specific windows.
        /// </summary>
        public VlcDoubleWindowFactory DoubleWindowFactory {
            get {
                VerifyObjectIsNotDisposed();
                //
                return (doubleWindowFactory);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                doubleWindowFactory = value;
            }
        }

        private string version;

        /// <summary>
        /// Version of vlc library.
        /// </summary>
        public string Version {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (version == null) {
                    version = LibVlcInterop.libvlc_get_version();
                }
                //
                return (version);
            }
        }

        #region ILogVerbosityManager members

        /// <summary>
        /// Global vlclib log verbosity level.
        /// </summary>
        public int Verbosity {
            get {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                uint res = LibVlcInterop.libvlc_get_log_verbosity(descriptor, ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
                //
                return Convert.ToInt32(res);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exc = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exc);
                LibVlcInterop.libvlc_set_log_verbosity(descriptor, Convert.ToUInt32(value), ref exc);
                if (exc.b_raised != 0) {
                    throw new VlcInternalException(exc.Message);
                }
            }
        }

        #endregion

        /// <summary>
        /// Disposes all unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">If this parameters is <code>True</code> then Dispose is called explicitly, else Dispose called from finalizer.</param>
        protected override void Dispose(bool isDisposing) {
            try {
                if (isDisposing) {
                    if (thread != null) {
                        try {
                            if (thread.IsAlive) {
                                stopLoggingThread();
                            }
                        } catch (Exception exc) {
                            if (logger.IsErrorEnabled) {
                                logger.Error("Error while trying to interrupt a logging thread.", exc);
                            }
                            throw;
                        }
                    }
                    if (vlcLog != null) {
                        vlcLog.Dispose();
                    }
                }
                //
                if (descriptor != IntPtr.Zero) {
                    LibVlcInterop.libvlc_release(descriptor);
                }
            } finally {
                base.Dispose(isDisposing);
            }
        }

        /// <summary>
        /// Creates player which can be used to control media playing.
        /// </summary>
        /// <returns>Player instance.</returns>
        public override Player CreatePlayer(PlayerOutput playerOutput) {
            VerifyObjectIsNotDisposed();
            //
            if (playerOutput == null) {
                throw new ArgumentNullException("playerOutput");
            }
            //
            return (new VlcPlayer(playerOutput, new InternalObjectsFactory(descriptor), this));
        }

        /// <summary>
        /// Creates player with set of necessary filters support.
        /// </summary>
        public override Player CreatePlayer(PlayerOutput playerOutput, IFilterBase filtersRequired) {
            VerifyObjectIsNotDisposed();
            //
            if (playerOutput == null) {
                throw new ArgumentNullException("playerOutput");
            }
            //
            if (filtersRequired is IAdjustable) {
                
            }
            return (new VlcPlayer(playerOutput, new InternalObjectsFactory(descriptor), this));
        }

        /// <summary>
        /// Creates window where player renders video. User can control position of window and it's size.
        /// </summary>
        /// <returns>Window for rendering video.</returns>
        public override MediaWindow CreateWindow() {
            VerifyObjectIsNotDisposed();
            //
            if (doubleWindowFactory == null) {
                throw new InvalidOperationException("Factory for windows creation is not initialized.");
            }
            //
            return (DoubleWindowFactory.CreateWindow());
        }

        #region IVolumeManager members

        /// <summary>
        /// Global volume value for all vlclib-players.
        /// </summary>
        public int Volume {
            get {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exception = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exception);
                int res = LibVlcInterop.libvlc_audio_get_volume(descriptor, ref exception);
                if (exception.b_raised != 0) {
                    throw new VlcInternalException(exception.Message);
                }
                //
                return (res);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                libvlc_exception_t exception = new libvlc_exception_t();
                LibVlcInterop.libvlc_exception_init(ref exception);
                LibVlcInterop.libvlc_audio_set_volume(descriptor, value, ref exception);
                if (exception.b_raised != 0) {
                    throw new VlcInternalException(exception.Message);
                }
            }
        }

        #endregion

        #region Logging routine

        private readonly VlcLog vlcLog;
        private Thread thread;
        private readonly EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        private bool threadStopSignalSent;

        private void initializeAndStartLoggingThread() {
            thread = new Thread(loggingThreadFunc);
            thread.IsBackground = true;
            //
            thread.Start();
        }

        private void stopLoggingThread() {
            threadStopSignalSent = true;
            waitHandle.Set();
            //
            thread.Join();
        }

        private void loggingThreadFunc() {
            while (!threadStopSignalSent) {
                try {
                    vlcLog.UpdateMessages();
                    vlcLog.Clear();
                } catch (ObjectDisposedException) {
                    if (logger.IsWarnEnabled) {
                        logger.Warn("Logging thread : vlc logger instance has been disposed.");
                    }
                }
                //
                waitHandle.WaitOne(new TimeSpan(0, 0, 5), true);
            }
        }

        #endregion
    }
}