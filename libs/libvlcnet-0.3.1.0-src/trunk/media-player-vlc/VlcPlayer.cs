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

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
    /// Represents <see cref="Player"/> abstract class implementation using libvlc.
    /// </summary>
    public sealed class VlcPlayer : Player, IAdjustable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(VlcPlayer));

        #region Constants

        private const string ADJUST_FILTER_ALIAS = "elwood_adjust";

        /// <summary>
        /// Default brightness level.
        /// </summary>
        public const float DEFAULT_BRIGHTNESS = 1.0f;
        /// <summary>
        /// Default constrast level.
        /// </summary>
        public const float DEFAULT_CONTRAST = 1.0f;
        /// <summary>
        /// Default gamma level.
        /// </summary>
        public const float DEFAULT_GAMMA = 1.0f;
        /// <summary>
        /// Default hue level.
        /// </summary>
        public const int DEFAULT_HUE = 0;
        /// <summary>
        /// Default saturation level.
        /// </summary>
        public const float DEFAULT_SATURATION = 1.0f;

        #endregion

        // Necessary to create internal objects
        private readonly IInternalObjectsFactory internalObjectsFactory;
        private readonly IVolumeManager volumeManager;

        // For waiting required player state after Play(), Pause() and other calls
        private readonly Stopwatch timeoutWatch = new Stopwatch();
        private TimeSpan waitingRequiredStateTimeout = TimeSpan.FromSeconds(5);

        // Current player state
        private readonly VlcMediaPlayerInternal firstMediaPlayerInternal;
        private readonly VlcMediaPlayerInternal secondMediaPlayerInternal;

/**/    public readonly PlayerOutput playerOutput;

        private MediaInput currentMedia;
        private MediaInput nextMedia;
        private VlcMediaInternal currentMediaInternal;
        private VlcMediaInternal nextMediaInternal;
        private bool firstMediaPlayerInternalIsCurrent = true;

        private readonly List<PlayerEventsReceiver> eventsReceivers = new List<PlayerEventsReceiver>();

        /// <summary>
        /// List of subscribers to the player events.
        /// </summary>
        public override IList<PlayerEventsReceiver> EventsReceivers {
            get {
                VerifyObjectIsNotDisposed();
                //
                return (eventsReceivers);
            }
        }

        private VlcMediaPlayerInternal getCurrentMediaPlayerInternal() {
            return (firstMediaPlayerInternalIsCurrent ? firstMediaPlayerInternal : secondMediaPlayerInternal);
        }

        private VlcMediaPlayerInternal getNextMediaPlayerInternal() {
            return (firstMediaPlayerInternalIsCurrent ? secondMediaPlayerInternal : firstMediaPlayerInternal);
        }

        private void switchCurrentAndNextMediaPlayersInternal() {
            firstMediaPlayerInternalIsCurrent = !firstMediaPlayerInternalIsCurrent;
        }

        #region Constructors & Destructors

        internal VlcPlayer(PlayerOutput playerOutput, IInternalObjectsFactory internalObjectsFactory, IVolumeManager volumeManager) {
            if (playerOutput == null) {
                throw new ArgumentNullException("playerOutput");
            }
            if (volumeManager == null) {
                throw new ArgumentNullException("volumeManager");
            }
            if (internalObjectsFactory == null) {
                throw new ArgumentNullException("internalObjectsFactory");
            }
            //
            this.playerOutput = playerOutput;
            this.volumeManager = volumeManager;
            this.internalObjectsFactory = internalObjectsFactory;
            //
            firstMediaPlayerInternal = this.internalObjectsFactory.CreateVlcMediaPlayerInternal();
            secondMediaPlayerInternal = this.internalObjectsFactory.CreateVlcMediaPlayerInternal();
            //
            initializeEventsEngine();
        }

        /// <summary>
        /// Attaching events.
        /// </summary>
        private void initializeEventsEngine() {
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            IntPtr pFirstMediaPlayerInternalEventManager = LibVlcInterop.libvlc_media_player_event_manager(firstMediaPlayerInternal.Descriptor, ref exc);
            if (0 != exc.b_raised) {
                throw new VlcInternalException(exc.Message);
            }
            IntPtr pSecondMediaPlayerInternalEventManager = LibVlcInterop.libvlc_media_player_event_manager(secondMediaPlayerInternal.Descriptor, ref exc);
            if (0 != exc.b_raised) {
                throw new VlcInternalException(exc.Message);
            }

            // Attaching to first player
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_TimeChanged), libvlc_event_type_t.libvlc_MediaPlayerTimeChanged);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_Stopped), libvlc_event_type_t.libvlc_MediaPlayerStopped);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_PositionChanged), libvlc_event_type_t.libvlc_MediaPlayerPositionChanged);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_EncounteredError), libvlc_event_type_t.libvlc_MediaPlayerEncounteredError);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_EndReached), libvlc_event_type_t.libvlc_MediaPlayerEndReached);

            // StateChanged events
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerOpening);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerBuffering);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerPlaying);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerPaused);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerSeekableChanged);
            attachToEvent(pFirstMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(firstMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerPausableChanged);

            // Attaching to second player
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_TimeChanged), libvlc_event_type_t.libvlc_MediaPlayerTimeChanged);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_Stopped), libvlc_event_type_t.libvlc_MediaPlayerStopped);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_PositionChanged), libvlc_event_type_t.libvlc_MediaPlayerPositionChanged);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_EncounteredError), libvlc_event_type_t.libvlc_MediaPlayerEncounteredError);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_EndReached), libvlc_event_type_t.libvlc_MediaPlayerEndReached);

            // StateChanged events
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerOpening);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerBuffering);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerPlaying);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerPaused);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerSeekableChanged);
            attachToEvent(pSecondMediaPlayerInternalEventManager, new LibVlcInterop.VlcEventHandlerDelegate(secondMediaPlayerInternal_StateChanged), libvlc_event_type_t.libvlc_MediaPlayerPausableChanged);
        }

        /// <summary>
        /// Stops player and cleans up resources.
        /// </summary>
        protected override void Dispose(bool isDisposing) {
            try {
                if (isDisposing) {
                    // Suppress exceptions in Dispose()
                    try {
                        stop();
                    } catch (VlcTimeoutException exc) {
                        if (logger.IsErrorEnabled) {
                            logger.Error("Timeout while stopping player in Dispose() method.", exc);
                        }
                    }
                    //
                    if (firstMediaPlayerInternal != null) {
                        firstMediaPlayerInternal.Dispose();
                    }
                    if (secondMediaPlayerInternal != null) {
                        secondMediaPlayerInternal.Dispose();
                    }
                    if (currentMediaInternal != null) {
                        currentMediaInternal.Dispose();
                        currentMediaInternal = null;
                    }
                    if (nextMediaInternal != null) {
                        nextMediaInternal.Dispose();
                        nextMediaInternal = null;
                    }
                }
            } finally {
                base.Dispose(isDisposing);
            }
        }

        #endregion

        #region Events routine

        /// <summary>
        /// List for store used delegates. This delegates should not be disposed before disposing VlcPlayer.
        /// </summary>
        private readonly List<Delegate> eventDelegates = new List<Delegate>();

        /// <summary>
        /// Attaches an event to player with selected eventManager.
        /// And stores used delegate in internal list.
        /// </summary>
        private void attachToEvent(IntPtr eventManager, Delegate eventHandlerDelegate, libvlc_event_type_t eventType) {
            if (eventManager == IntPtr.Zero) {
                throw new ArgumentException("IntPtr.Zero is invalid value.", "eventManager");
            }
            if (eventHandlerDelegate == null) {
                throw new ArgumentNullException("eventHandlerDelegate");
            }
            //
            libvlc_exception_t exc = new libvlc_exception_t();
            LibVlcInterop.libvlc_exception_init(ref exc);
            LibVlcInterop.libvlc_event_attach(eventManager, eventType,
                Marshal.GetFunctionPointerForDelegate(eventHandlerDelegate),
                IntPtr.Zero, ref exc);
            if (0 != exc.b_raised) {
                throw new VlcInternalException(exc.Message);
            }
            // Save delegate to a private list (to suppress finalizing it)
            eventDelegates.Add(eventHandlerDelegate);
        }

        // First player events handlers

        private void firstMediaPlayerInternal_TimeChanged(IntPtr libvlc_event, IntPtr data) {
            if (firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnTimeChanged();
                }
            }
        }

        private void firstMediaPlayerInternal_Stopped(IntPtr libvlc_event, IntPtr data) {
            if (firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnStopped();
                }
            }
        }

        private void firstMediaPlayerInternal_PositionChanged(IntPtr libvlc_event, IntPtr data) {
            if (firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnPositionChanged();
                }
            }
        }

        private void firstMediaPlayerInternal_EncounteredError(IntPtr libvlc_event, IntPtr data) {
            if (firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnEncounteredError();
                }
            }
        }

        private void firstMediaPlayerInternal_StateChanged(IntPtr libvlc_event, IntPtr data) {
            if (firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnStateChanged();
                }
            }
        }

        private void firstMediaPlayerInternal_EndReached(IntPtr libvlc_event, IntPtr data) {
            if (firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnEndReached();
                }
            }
        }

        // Second player events handlers

        private void secondMediaPlayerInternal_TimeChanged(IntPtr libvlc_event, IntPtr data) {
            if (!firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnTimeChanged();
                }
            }
        }

        private void secondMediaPlayerInternal_Stopped(IntPtr libvlc_event, IntPtr data) {
            if (!firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnStopped();
                }
            }
        }

        private void secondMediaPlayerInternal_PositionChanged(IntPtr libvlc_event, IntPtr data) {
            if (!firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnPositionChanged();
                }
            }
        }

        private void secondMediaPlayerInternal_EncounteredError(IntPtr libvlc_event, IntPtr data) {
            if (!firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnEncounteredError();
                }
            }
        }

        private void secondMediaPlayerInternal_StateChanged(IntPtr libvlc_event, IntPtr data) {
            if (!firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnStateChanged();
                }
            }
        }

        private void secondMediaPlayerInternal_EndReached(IntPtr libvlc_event, IntPtr data) {
            if (!firstMediaPlayerInternalIsCurrent) {
                foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                    receiver.OnEndReached();
                }
            }
        }

        #endregion

        /// <summary>
        /// Timeout to wait a required state.
        /// </summary>
        public TimeSpan WaitingRequiredStateTimeout {
            get {
                VerifyObjectIsNotDisposed();
                //
                lock (filtersWaitingThreadLock) {
                    return (waitingRequiredStateTimeout);
                }
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                lock (filtersWaitingThreadLock) {
                    waitingRequiredStateTimeout = value;
                }
            }
        }

        /// <summary>
        /// Is next media prepared for playing.
        /// </summary>
        public bool IsNextMediaPrepared {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (nextMediaInternal == null) {
                    return (false);
                }
                //
                return (nextMediaInternal.State == VlcMediaState.PAUSED);
            }
        }

        /// <summary>
        /// Position of current media (0.0f - 1.0f).
        /// </summary>
        public override float Position {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (currentMediaInternal == null) {
                    throw new MediaPlayerException("Current media is not loaded.");
                }
                //
                return (getCurrentMediaPlayerInternal().Position);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value < 0f) || (value > 1.0f)) {
                    throw new ArgumentOutOfRangeException("value", "must be (0.0 - 1.0)");
                }
                if (currentMediaInternal == null) {
                    throw new MediaPlayerException("Current media is not loaded.");
                }
                //
                getCurrentMediaPlayerInternal().Position = value;
            }
        }

        /// <summary>
        /// Current media playing time.
        /// </summary>
        public override TimeSpan Time {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (currentMediaInternal == null) {
                    throw new MediaPlayerException("Current media is not loaded.");
                }
                //
                return (getCurrentMediaPlayerInternal().Time);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if (currentMediaInternal == null) {
                    throw new MediaPlayerException("Current media is not loaded.");
                }
                //
                getCurrentMediaPlayerInternal().Time = value;
            }
        }

        /// <summary>
        /// Gets the actual state of player.
        /// </summary>
        public override PlayerState State {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (currentMediaInternal == null) {
                    return PlayerState.Idle;
                }
                //
                return (PlayerState) currentMediaInternal.State;
            }
        }

        #region Filters routine

        private float brightness = DEFAULT_BRIGHTNESS;
        private float contrast = DEFAULT_CONTRAST;
        private float gamma = DEFAULT_GAMMA;
        private int hue = DEFAULT_HUE;
        private float saturation = DEFAULT_SATURATION;

        private readonly Object filtersWaitingThreadLock = new Object();
        private bool _currentMediaFilterAvailable;

        private bool currentMediaFilterAvailable {
            get {
                lock (filtersWaitingThreadLock) {
                    return _currentMediaFilterAvailable;
                }
            }
            set {
                lock (filtersWaitingThreadLock) {
                    _currentMediaFilterAvailable = value;
                }
            }
        }

        /// <summary>
        /// Is parameters can be adjusted.
        /// </summary>
        public bool IsFilterAvailable {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (!currentMediaFilterAvailable) {
                    return (false);
                }
                //
                const int FIND_CHILD = 2;
                libvlc_instance_t instance = internalObjectsFactory.GetInteropStructure();
                IntPtr pfilter = LibVlcInterop.__vlc_object_find_name(instance.p_libvlc_int, ADJUST_FILTER_ALIAS, FIND_CHILD);
                if (pfilter != IntPtr.Zero) {
                    LibVlcInterop.__vlc_object_release(pfilter);
                    return (true);
                }
                //
                return (false);
            }
        }

        /// <summary>
        /// Brightness (0.0 - 2.0).
        /// </summary>
        public float Brightness {
            get {
                VerifyObjectIsNotDisposed();
                //
                lock (filtersWaitingThreadLock) {
                    return (brightness);
                }
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value > 2.0f) || (value < 0f)) {
                    throw new ArgumentOutOfRangeException("value", "must be (0.0 - 2.0)");
                }
                if (!IsFilterAvailable) {
                    throw new MediaPlayerException("Adjust filter is not enabled.");
                }
                //
                lock (filtersWaitingThreadLock) {
                    brightness = value;
                }
                vlc_value_t vlc_value = new vlc_value_t();
                vlc_value.f_float = brightness;
                setAdjustValue("brightness", vlc_value);
            }
        }

        /// <summary>
        /// Contrast (0.0 - 2.0).
        /// </summary>
        public float Contrast {
            get {
                VerifyObjectIsNotDisposed();
                //
                lock (filtersWaitingThreadLock) {
                    return (contrast);
                }
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value > 2.0f) || (value < 0f)) {
                    throw new ArgumentOutOfRangeException("value", "must be (0.0 - 2.0)");
                }
                if (!IsFilterAvailable) {
                    throw new MediaPlayerException("Adjust filter is not enabled.");
                }
                //
                lock (filtersWaitingThreadLock) {
                    contrast = value;
                }
                vlc_value_t vlc_value = new vlc_value_t();
                vlc_value.f_float = contrast;
                setAdjustValue("contrast", vlc_value);
            }
        }

        /// <summary>
        /// Gamma (0.01 - 10.0).
        /// </summary>
        public float Gamma {
            get {
                VerifyObjectIsNotDisposed();
                //
                lock (filtersWaitingThreadLock) {
                    return (gamma);
                }
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value > 10.0f) || (value < 0.01f)) {
                    throw new ArgumentOutOfRangeException("value", "must be (0.01 - 10.0)");
                }
                if (!IsFilterAvailable) {
                    throw new MediaPlayerException("Adjust filter is not enabled.");
                }
                //
                lock (filtersWaitingThreadLock) {
                    gamma = value;
                }
                vlc_value_t vlc_value = new vlc_value_t();
                vlc_value.f_float = gamma;
                setAdjustValue("gamma", vlc_value);
            }
        }

        /// <summary>
        /// Saturation (0.0 - 3.0).
        /// </summary>
        public float Saturation {
            get {
                VerifyObjectIsNotDisposed();
                //
                lock (filtersWaitingThreadLock) {
                    return (saturation);
                }
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value > 3.0f) || (value < 0f)) {
                    throw new ArgumentOutOfRangeException("value", "must be (0.0 - 3.0)");
                }
                if (!IsFilterAvailable) {
                    throw new MediaPlayerException("Adjust filter is not enabled.");
                }
                //
                lock (filtersWaitingThreadLock) {
                    saturation = value;
                }
                vlc_value_t vlc_value = new vlc_value_t();
                vlc_value.f_float = saturation;
                setAdjustValue("saturation", vlc_value);
            }
        }

        /// <summary>
        /// Hue (0 - 360).
        /// </summary>
        public int Hue {
            get {
                VerifyObjectIsNotDisposed();
                //
                lock (filtersWaitingThreadLock) {
                    return (hue);
                }
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value < 0) || (value > 360)) {
                    throw new ArgumentOutOfRangeException("value", "must be (0 - 360)");
                }
                if (!IsFilterAvailable) {
                    throw new MediaPlayerException("Adjust filter is not enabled.");
                }
                //
                lock (filtersWaitingThreadLock) {
                    hue = value;
                }
                vlc_value_t vlc_value = new vlc_value_t();
                vlc_value.i_int = hue;
                setAdjustValue("hue", vlc_value);
            }
        }

        /// <summary>
        /// Apply adjust filter settings to current playing videos.
        /// </summary>
        private void applyAdjustFilterSettings() {
            if (!IsFilterAvailable) {
                return;
            }
            //
            vlc_value_t vlc_value = new vlc_value_t();

            vlc_value.f_float = Brightness;
            setAdjustValue("brightness", vlc_value);

            vlc_value.f_float = Contrast;
            setAdjustValue("contrast", vlc_value);

            vlc_value.f_float = Gamma;
            setAdjustValue("gamma", vlc_value);

            vlc_value.f_float = Saturation;
            setAdjustValue("saturation", vlc_value);

            vlc_value.i_int = Hue;
            setAdjustValue("hue", vlc_value);
        }

        /// <summary>
        /// Processes the vlc core objects tree recoursively and sets <paramref name="valueName"/> variable
        /// to <paramref name="value"/>. Objects with name equal to <paramref name="objectNameToSearch"/> will be processed.
        /// </summary>
        /// <param name="pObject">Parent in VLC tree (for example, libvlc_instance_t -> p_libvlc_int)</param>
        /// <param name="objectNameToSearch">Object name to search.</param>
        /// <param name="valueName">Name of variable to set.</param>
        /// <param name="value">New value of <paramref name="valueName"/> variable.</param>
        private static void processChangeVlcObjectsVariable(IntPtr pObject, string objectNameToSearch, string valueName,
                                                            vlc_value_t value) {
            vlc_common_members common_members =
                (vlc_common_members) Marshal.PtrToStructure(pObject, typeof(vlc_common_members));

            if (common_members.ObjectName == objectNameToSearch) {
                LibVlcInterop.__var_Set(
                    pObject,
                    valueName,
                    value);
            }

            libvlc_list_t childs = LibVlcInterop.__vlc_list_children(pObject);
            foreach (libvlc_value_t val in childs.values) {
                processChangeVlcObjectsVariable(val.p_object, objectNameToSearch, valueName, value);
                LibVlcInterop.__vlc_object_release(val.p_object);
            }
        }

        private void setAdjustValue(string valueName, vlc_value_t value) {
            libvlc_instance_t instance = internalObjectsFactory.GetInteropStructure();
            processChangeVlcObjectsVariable(instance.p_libvlc_int, ADJUST_FILTER_ALIAS, valueName, value);
        }

        #endregion

        /// <summary>
        /// Event signals of <see cref="IAdjustable"/> filter has been loaded.
        /// </summary>
        public event AdjustFilterLoadedHandler FilterLoaded;

        #region Private methods

        /// <summary>
        /// Waiting for change the state of <paramref name="mediaInternal"/> to the requested <paramref name="stateRequired"/>.
        /// <see cref="MediaPlayerException"/> exception will be thrown if operation will be timeouted.
        /// </summary>
        private void waitForMediaState(VlcMediaInternal mediaInternal, VlcMediaState stateRequired) {
            if (mediaInternal == null) {
                throw new ArgumentNullException("mediaInternal");
            }
            //
            Stopwatch watch = timeoutWatch;
            try {
                TimeSpan timeout;
                lock (filtersWaitingThreadLock) {
                    timeout = waitingRequiredStateTimeout;
                }
                //
                while (mediaInternal.State != stateRequired) {
                    if (!watch.IsRunning) {
                        watch.Start();
                    }
                    if (watch.Elapsed > timeout) {
                        throw new VlcTimeoutException("Timeout waiting required state.");
                    }
                }
            } finally {
                watch.Stop();
                watch.Reset();
            }
        }

        private static void getVlcObjectsByName(IntPtr pObject, string objectNameToSearch, ICollection<int> idList) {
            vlc_common_members common_members = (vlc_common_members) Marshal.PtrToStructure(pObject, typeof(vlc_common_members));
            //
            if (common_members.ObjectName == objectNameToSearch) {
                idList.Add(common_members.i_object_id);
            }
            //
            libvlc_list_t childs = LibVlcInterop.__vlc_list_children(pObject);
            foreach (libvlc_value_t val in childs.values) {
                getVlcObjectsByName(val.p_object, objectNameToSearch, idList);
                LibVlcInterop.__vlc_object_release(val.p_object);
            }
        }

        /// <summary>
        /// Begining playing media by mediaplayer, using current filter settings.
        /// </summary>
        private void startPlaying(VlcMediaPlayerInternal mediaplayer, VlcMediaInternal media) {
            if (logger.IsTraceEnabled) {
                logger.Trace("startPlaying method called.");
            }
            // Dump VLC Objects before start playing
            if (logger.IsTraceEnabled) {
                logger.Trace("DUMP before start playing :");
                logger.Trace(getVlcObjectsTreeDump());
            }
            List<int> loadedFiltersIdList = new List<int>();
            //
            libvlc_instance_t libvlc_instance = internalObjectsFactory.GetInteropStructure();
            getVlcObjectsByName(libvlc_instance.p_libvlc_int, ADJUST_FILTER_ALIAS, loadedFiltersIdList);
            //
            mediaplayer.Play();
            if (logger.IsTraceEnabled) {
                logger.Trace("mediaPlayer.Play() called.");
            }
            // Wait for PLAYING state. Otherwise the next call Pause() will take no effect
            waitForMediaState(media, VlcMediaState.PLAYING);
            if (logger.IsTraceEnabled) {
                logger.Trace("VlcMediaState is PLAYING now.");
            }
            // Dump VLC Objects after start playing
            if (logger.IsTraceEnabled) {
                logger.Trace("DUMP after star playing :");
                logger.Trace(getVlcObjectsTreeDump());
            }
            // If other thread is processing filters now, stop them and wait until it stopped
            stopWaitingFiltersThread();
            //
            currentMediaFilterAvailable = false;
            // Starting new thread for waiting filters
            if (logger.IsTraceEnabled) {
                logger.Trace("Starting new thread for waiting IAdjustable filter.");
            }
            //
            waitingFiltersThread = new Thread(waitingFiltersThreadHandler);
            waitingFiltersThread.Start(new VlcObjectsSnapshot(loadedFiltersIdList, libvlc_instance));
        }

        /// <summary>
        /// Snapshot of vlc internal structures retrieved before playing will started.
        /// </summary>
        private class VlcObjectsSnapshot
        {
            private readonly List<int> loadedFiltersIdList;
            private readonly libvlc_instance_t libvlc_instance;

            /// <summary>
            /// List if filters identifiers.
            /// </summary>
            public List<int> LoadedFiltersIdList {
                get {
                    return (loadedFiltersIdList);
                }
            }

            /// <summary>
            /// Marshalled native libvlc_instance_t struct.
            /// </summary>
            public libvlc_instance_t Libvlc_Instance {
                get {
                    return (libvlc_instance);
                }
            }

            public VlcObjectsSnapshot(List<int> loadedFiltersIdList, libvlc_instance_t libvlc_instance) {
                if (loadedFiltersIdList == null) {
                    throw new ArgumentNullException("loadedFiltersIdList");
                }
                //
                this.loadedFiltersIdList = loadedFiltersIdList;
                this.libvlc_instance = libvlc_instance;
            }
        }

        private void stopWaitingFiltersThread() {
            if ((waitingFiltersThread != null) && (waitingFiltersThread.IsAlive)) {
                if (logger.IsTraceEnabled) {
                    logger.Trace("Thread waiting filters is alive. Stopping them.");
                }
                waitingFiltersThreadStopSignal.Set();
                waitingFiltersThread.Join();
                if (logger.IsTraceEnabled) {
                    logger.Trace("Stopped OK.");
                }
            }
        }

        /// <summary>
        /// Thread to wait required filter to be loaded.
        /// </summary>
        private Thread waitingFiltersThread;

        /// <summary>
        /// Signals to waiting thread to be stopped.
        /// </summary>
        private readonly EventWaitHandle waitingFiltersThreadStopSignal = new EventWaitHandle(false, EventResetMode.AutoReset);

        private void waitingFiltersThreadHandler(object vlcObjectsSnapshot) {
            if (vlcObjectsSnapshot == null) {
                throw new ArgumentNullException("vlcObjectsSnapshot");
            }
            if (!(vlcObjectsSnapshot is VlcObjectsSnapshot)) {
                throw new ArgumentException("Invalid argument type.", "vlcObjectsSnapshot");
            }
            //
            try {
                VlcObjectsSnapshot objectsSnapshot = (VlcObjectsSnapshot) vlcObjectsSnapshot;
                //
                Stopwatch watch = timeoutWatch;
                try {
                    TimeSpan timeout;
                    lock (filtersWaitingThreadLock) {
                        timeout = waitingRequiredStateTimeout;
                    }
                    //
                    List<int> currentFiltersIdList = new List<int>();
                    //
                    currentMediaFilterAvailable = true;
                    while (currentFiltersIdList.Count <= objectsSnapshot.LoadedFiltersIdList.Count) {
                        //
                        currentFiltersIdList = new List<int>();
                        getVlcObjectsByName(objectsSnapshot.Libvlc_Instance.p_libvlc_int, ADJUST_FILTER_ALIAS, currentFiltersIdList);
                        //
                        if (!watch.IsRunning) {
                            watch.Start();
                        }
                        if ((waitingFiltersThreadStopSignal.WaitOne(50)) || (watch.Elapsed > timeout)) {
                            currentMediaFilterAvailable = false;
                            break;
                        }
                    }
                } finally {
                    watch.Stop();
                    watch.Reset();
                }
                //
                if (logger.IsTraceEnabled) {
                    logger.Trace(String.Format("Thread waiting filters is stopping now. FiltersFound = {0}.", currentMediaFilterAvailable));
                }
                // Dump VLC Objects after waiting a filter
                if (logger.IsTraceEnabled) {
                    logger.Trace("DUMP after waiting thread :");
                    logger.Trace(getVlcObjectsTreeDump());
                }
                // Apply current filter settings if filter presents
                if (currentMediaFilterAvailable) {
                    if (logger.IsTraceEnabled) {
                        logger.Trace("Applying filter settings to current media.");
                    }
                    applyAdjustFilterSettings();
                }
                // Notify all subscribers
                AdjustFilterLoadedHandler handler = FilterLoaded;
                if (handler != null) {
                    try {
                        handler.Invoke(this, new AdjustFilterLoadedHandlerArgs());
                    } catch (Exception exc) {
                        if (logger.IsErrorEnabled) {
                            logger.Error(String.Format("Unhandled exception has been occured in event dispatchers code : {0}", exc));
                        }
                    }
                }
                //
            } catch (Exception exc) {
                if (logger.IsFatalEnabled) {
                    logger.Fatal(String.Format("Unhandled exception has been occured in filter waiting thread. Exception : {0}", exc));
                }
                //
                currentMediaFilterAvailable = false;
            }
        }

        private void prepareNextMedia() {
            // If nextMedia was already loaded we need to release it before reinitialization.
            if (nextMediaInternal != null) {
                getNextMediaPlayerInternal().Stop();
                //
                nextMediaInternal.Dispose();
                nextMediaInternal = null;
            }
            //
            nextMediaInternal = internalObjectsFactory.CreateVlcMediaInternal(nextMedia);
            nextMediaInternal.SetOutput(playerOutput);
            //
            getNextMediaPlayerInternal().SetMedia(nextMediaInternal);
            getNextMediaPlayerInternal().SetDisplayOutput(
                (int) ((DoubleWindowBase) playerOutput.Window).GetInactiveWindowHandleInternal());
            //
            startPlaying(getNextMediaPlayerInternal(), nextMediaInternal);
            getNextMediaPlayerInternal().Pause();
            //
            waitForMediaState(nextMediaInternal, VlcMediaState.PAUSED);
            //
            if (nextMediaInternal.State != VlcMediaState.PAUSED) {
                throw new VlcTimeoutException("Timeout waiting required state.");
            }
        }

        #endregion

        /// <summary>
        /// Specifies current media input.
        /// </summary>
        public override void SetMediaInput(MediaInput mediaInput) {
            VerifyObjectIsNotDisposed();
            //
            if (mediaInput == null) {
                throw new ArgumentNullException("mediaInput");
            }
            //
            currentMedia = mediaInput;
        }

        /// <summary>
        /// Specifies next media input.
        /// </summary>
        public override void SetNextMediaInput(MediaInput mediaInput) {
            VerifyObjectIsNotDisposed();
            //
            if (mediaInput == null) {
                throw new ArgumentNullException("mediaInput");
            }
            //
            nextMedia = mediaInput;
            // If nextMediaInternal was prepared to playing already we have to release it
            if (nextMediaInternal != null) {
                getNextMediaPlayerInternal().Stop();
                nextMediaInternal.Dispose();
                nextMediaInternal = null;
            }
            // If current player state is PLAYING we can prepare the nextMedia
            if (currentMediaInternal != null) {
                if ((currentMediaInternal.State == VlcMediaState.PLAYING) ||
                    (currentMediaInternal.State == VlcMediaState.PAUSED)) {
                    prepareNextMedia();
                }
            }
        }

        /// <summary>
        /// Starts playing of media which was initialized using <see cref="SetMediaInput"/> method.
        /// If some media is playing now it will be simply restarted.
        /// </summary>
        public override void Play() {
            VerifyObjectIsNotDisposed();
            //
            if (logger.IsTraceEnabled) {
                logger.Trace("Play method called.");
            }
            if (currentMedia == null) {
                throw new MediaPlayerException("Current media is null.");
            }
            if (playerOutput == null) {
                throw new MediaPlayerException("Player output is null.");
            }
            //
            if (currentMediaInternal != null) {
                if (currentMediaInternal.State == VlcMediaState.PAUSED) {
                    Resume();
                    return;
                }
                if (currentMediaInternal.State == VlcMediaState.PLAYING) {
                    Position = 0f;
                    return;
                }
                Stop();
            }
            // Verify currentMedia
            if (currentMedia.Type == MediaInputType.File) {
                if (!File.Exists(currentMedia.Source)) {
                    throw new FileNotFoundException("File of media specified was not found.", currentMedia.Source);
                }
            }
            // Create the internal medias and start playing
            currentMediaInternal = internalObjectsFactory.CreateVlcMediaInternal(currentMedia);
            currentMediaInternal.SetOutput(playerOutput);
            getCurrentMediaPlayerInternal().SetMedia(currentMediaInternal);
            getCurrentMediaPlayerInternal().SetDisplayOutput((int) ((DoubleWindowBase) playerOutput.Window).GetActiveWindowHandleInternal());
            //
            (new Thread(new ThreadStart(() =>
            {
                getCurrentMediaPlayerInternal().Play();
            }))).Start();
            //
            startPlaying(getCurrentMediaPlayerInternal(), currentMediaInternal);
            // If nextMedia was specified, preparing it too
            if (nextMedia != null) {
                prepareNextMedia();
            }
            //
            ((DoubleWindowBase) playerOutput.Window).PlayerVisibleInternal = true;
        }

        private string getVlcObjectsTreeDump() {
            StringBuilder str = new StringBuilder();
            //
            libvlc_instance_t instance = internalObjectsFactory.GetInteropStructure();
            dumpChildrenObjects(str, instance.p_libvlc_int, 0);
            //
            return (str.ToString());
        }

        private static void dumpChildrenObjects(StringBuilder str, IntPtr pObject, int enclosureLevel) {
            vlc_common_members common_members = (vlc_common_members) Marshal.PtrToStructure(pObject, typeof(vlc_common_members));
            for (int i = 0; i < enclosureLevel; i++)
                str.Append("- ");
            str.AppendLine(common_members.ToString());
            libvlc_list_t childs = LibVlcInterop.__vlc_list_children(pObject);
            foreach (libvlc_value_t value in childs.values) {
                dumpChildrenObjects(str, value.p_object, enclosureLevel + 1);
                LibVlcInterop.__vlc_object_release(value.p_object);
            }
        }

        /// <summary>
        /// Synchronized pause current player.
        /// </summary>
        public override void Pause() {
            VerifyObjectIsNotDisposed();
            //
            Pause(true);
        }

        /// <summary>
        /// Pauses the current player.
        /// </summary>
        /// <param name="waitForPlayingState">If true, operation is synchronized. Else - asynchronous.</param>
        public void Pause(bool waitForPlayingState) {
            VerifyObjectIsNotDisposed();
            //
            if (currentMediaInternal == null) {
                throw new MediaPlayerException("Current media is not loaded.");
            }
            // If already paused, return
            if (currentMediaInternal.State == VlcMediaState.PAUSED) {
                return;
            }
            if (currentMediaInternal.State != VlcMediaState.PLAYING) {
                throw new MediaPlayerException("Unexpected media state, must be playing.");
            }
            //
            getCurrentMediaPlayerInternal().Pause();
            if (waitForPlayingState) {
                waitForMediaState(currentMediaInternal, VlcMediaState.PAUSED);
            }
        }

        /// <summary>
        /// Synchronized resume current player.
        /// </summary>
        public override void Resume() {
            VerifyObjectIsNotDisposed();
            //
            Resume(true);
        }

        /// <summary>
        /// Resumes the current player.
        /// </summary>
        /// <param name="waitForPausedState">If true, operation is synchronized. Else - asynchronous.</param>
        public void Resume(bool waitForPausedState) {
            VerifyObjectIsNotDisposed();
            //
            if (currentMediaInternal == null) {
                throw new MediaPlayerException("Current media is not loaded.");
            }
            // If already playing, return
            if (currentMediaInternal.State == VlcMediaState.PLAYING) {
                return;
            }
            if (currentMediaInternal.State != VlcMediaState.PAUSED) {
                throw new MediaPlayerException("Unexpected media state, must be paused.");
            }
            //
            getCurrentMediaPlayerInternal().Pause();
            if (waitForPausedState) {
                waitForMediaState(currentMediaInternal, VlcMediaState.PAUSED);
            }
        }

        /// <summary>
        /// Stops playing.
        /// </summary>
        public override void Stop() {
            VerifyObjectIsNotDisposed();
            //
            stop();
        }

        private void stop() {
            ((DoubleWindowBase) playerOutput.Window).PlayerVisibleInternal = false;
            //
            stopWaitingFiltersThread();

            if (currentMediaInternal != null) {
                if ((currentMediaInternal.State != VlcMediaState.ENDED) &&
                    (currentMediaInternal.State != VlcMediaState.IDLE_CLOSE)) {
                    //
                    getCurrentMediaPlayerInternal().Stop();
                    waitForMediaState(currentMediaInternal, VlcMediaState.ENDED);
                    //
                    if (currentMediaInternal.State != VlcMediaState.ENDED) {
                        throw new VlcTimeoutException("Timeout waiting required state.");
                    }
                }
                currentMediaInternal.Dispose();
                currentMediaInternal = null;
            }
            //
            if (nextMediaInternal != null) {
                if ((nextMediaInternal.State != VlcMediaState.ENDED) &&
                    (nextMediaInternal.State != VlcMediaState.IDLE_CLOSE)) {
                    //
                    getNextMediaPlayerInternal().Stop();
                    waitForMediaState(nextMediaInternal, VlcMediaState.ENDED);
                    //
                    if (nextMediaInternal.State != VlcMediaState.ENDED) {
                        throw new VlcTimeoutException("Timeout waiting required state.");
                    }
                }
                nextMediaInternal.Dispose();
                nextMediaInternal = null;
            }
        }

        /// <summary>
        /// Stop the current media playing and begin playing next.
        /// </summary>
        public override void PlayNext() {
            VerifyObjectIsNotDisposed();
            //
            if (nextMedia == null) {
                throw new MediaPlayerException("Next media is not selected.");
            }
            // Verify nexyMedia
            if (nextMedia.Type == MediaInputType.File) {
                if (!File.Exists(nextMedia.Source)) {
                    throw new FileNotFoundException("File of media specified was not found.", nextMedia.Source);
                }
            }
            //
            if (nextMediaInternal == null) {
                // Create nextMediaInternal and start playing in the inactiveWindow
                nextMediaInternal = internalObjectsFactory.CreateVlcMediaInternal(nextMedia);
                nextMediaInternal.SetOutput(playerOutput);
                getNextMediaPlayerInternal().SetMedia(nextMediaInternal);
                getNextMediaPlayerInternal().SetDisplayOutput((int) ((DoubleWindowBase) playerOutput.Window).GetInactiveWindowHandleInternal());
                //
                startPlaying(getNextMediaPlayerInternal(), nextMediaInternal);
            } else {
                if (nextMediaInternal.State != VlcMediaState.PAUSED) {
                    throw new MediaPlayerException("Next media has an unexpected state now.");
                }
                //
                getNextMediaPlayerInternal().Pause();
            }
            //
            waitForMediaState(nextMediaInternal, VlcMediaState.PLAYING);
            if (nextMediaInternal.State != VlcMediaState.PLAYING) {
                throw new VlcTimeoutException("Timeout waiting required state.");
            }
            //
            ((DoubleWindowBase) playerOutput.Window).SwitchWindowsInternal();
            // It is important to call this BEFORE stopping the player
            // if we want to EndReached event will not be raised incorrectly
            switchCurrentAndNextMediaPlayersInternal();
            //
            if (currentMediaInternal != null) {
                if ((currentMediaInternal.State != VlcMediaState.IDLE_CLOSE) &&
                    (currentMediaInternal.State != VlcMediaState.ENDED)) {
                    // It was _current_ player before the switchCurrentAndNextMediaPlayersInternal() call
                    getNextMediaPlayerInternal().Stop();
                    //
                    waitForMediaState(currentMediaInternal, VlcMediaState.ENDED);
                    if (currentMediaInternal.State != VlcMediaState.ENDED) {
                        throw new VlcTimeoutException("Timeout waiting required state.");
                    }
                }
                //
                currentMediaInternal.Dispose();
                currentMediaInternal = null;
            }
            // current := next
            currentMediaInternal = nextMediaInternal;
            nextMediaInternal = null;
            currentMedia = nextMedia;
            nextMedia = null;
            //
            ((DoubleWindowBase) playerOutput.Window).PlayerVisibleInternal = true;
        }

        /// <summary>
        /// Takes the snapshot of currently playing media. Snapshot will be stored in specified file.
        /// </summary>
        /// <param name="filePath">Path where to save snapshot.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        public override void TakeSnapshot(string filePath, int width, int height) {
            VerifyObjectIsNotDisposed();
            //
            if (currentMediaInternal == null) {
                throw new MediaPlayerException("Player is empty.");
            }
            //
            if ((currentMediaInternal.State != VlcMediaState.PLAYING) &&
                (currentMediaInternal.State != VlcMediaState.PAUSED)) {
                throw new MediaPlayerException("Unexpected player state.");
            }
            //
            getCurrentMediaPlayerInternal().TakeSnapshot(filePath, width, height);
        }

        /// <summary>
        /// Audio volume (0 - 100).
        /// </summary>
        public override int Volume {
            get {
                VerifyObjectIsNotDisposed();
                //
                return (volumeManager.Volume);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value > 100) || (value < 0)) {
                    throw new ArgumentException("Must be between 0 and 100.", "value");
                }
                //
                volumeManager.Volume = value;
            }
        }
    }
}