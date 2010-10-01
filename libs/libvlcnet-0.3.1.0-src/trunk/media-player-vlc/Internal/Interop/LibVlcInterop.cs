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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

#endregion

namespace DZ.MediaPlayer.Vlc.Internal.Interop
{
    /// <summary>
    /// Thunks to original library.
    /// </summary>
    internal static class LibVlcInterop
    {
        #region Volume

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int libvlc_audio_get_volume(IntPtr libvlc_instance, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_audio_set_volume(IntPtr libvlc_instance, int volume, ref libvlc_exception_t exception);

        #endregion

        #region Core objects

        /// <summary>
        /// Find a named object and increment its refcount. Don't forget to call __vlc_object_release()
        /// after using found object.
        /// </summary>
        [DllImport("libvlccore", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr __vlc_object_find_name(IntPtr vlc_object_t, IntPtr name, int flags);

        public static IntPtr __vlc_object_find_name(IntPtr vlc_object_t, string name, int flags) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            IntPtr namePtr = Marshal.StringToHGlobalAnsi(name);
            try {
                return __vlc_object_find_name(vlc_object_t, namePtr, flags);
            } finally {
                Marshal.FreeHGlobal(namePtr);
            }
        }

        /// <summary>
        /// Find a typed object and increment its refcount. Don't forget to call __vlc_object_release()
        /// after using found object.
        /// </summary>
        [DllImport("libvlccore", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, EntryPoint = "__vlc_list_children")]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr __vlc_object_find(IntPtr vlc_object_t, int i_type, int i_mode);

        /// <summary>
        /// Decrement an object refcount
        /// * And destroy the object if its refcount reach zero.
        /// </summary>
        [DllImport("libvlccore", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void __vlc_object_release(IntPtr vlc_object_t);

        /// <summary>
        /// Gets the list of children of an objects, and increment their reference count.
        /// </summary>
        /// <param name="p_object">Parent object</param>
        /// <returns>A list (possibly empty) or NULL in case of error.</returns>
        [DllImport("libvlccore", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, EntryPoint = "__vlc_list_children")]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr __vlc_list_children_internal(IntPtr p_object);

        public static libvlc_list_t __vlc_list_children(IntPtr p_object) {
            IntPtr ptrRet = __vlc_list_children_internal(p_object);
            return (libvlc_list_t) (Marshal.PtrToStructure(ptrRet, typeof (libvlc_list_t)));
        }

        [DllImport("libvlccore", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void __var_Set(IntPtr p_object, IntPtr psz_name, vlc_value_t value);

        public static void __var_Set(IntPtr p_object, string name, vlc_value_t value) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            IntPtr namePtr = Marshal.StringToHGlobalAnsi(name);
            try {
                __var_Set(p_object, namePtr, value);
            } finally {
                Marshal.FreeHGlobal(namePtr);
            }
        }

        [DllImport("libvlccore", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void __var_Get(IntPtr p_object, IntPtr psz_name, ref vlc_value_t value);

        public static void __var_Get(IntPtr p_object, string name, ref vlc_value_t value) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            IntPtr namePtr = Marshal.StringToHGlobalAnsi(name);
            try {
                __var_Get(p_object, namePtr, ref value);
            } finally {
                Marshal.FreeHGlobal(namePtr);
            }
        }

        #endregion

        #region Exceptions

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_exception_init(ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        [Obsolete("builder's content must to be NOT formatted in C-style (like %s)")]
        public static extern void libvlc_exception_raise(ref libvlc_exception_t exception,
                                                         StringBuilder builder);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int32 libvlc_exception_raised(ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_exception_clear(ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true,
            EntryPoint = "libvlc_exception_get_message")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_exception_get_message_internal(ref libvlc_exception_t p_exception);

        public static string libvlc_exception_get_message(ref libvlc_exception_t exception) {
            IntPtr msgPtr = libvlc_exception_get_message_internal(ref exception);
            return msgPtr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(msgPtr);
        }

        #endregion

        #region Core

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_new(int argc, IntPtr argv, ref libvlc_exception_t exception);

        public static IntPtr libvlc_new(string[] parameters, ref libvlc_exception_t exception) {
            if (parameters.Length > 10) {
                throw new ArgumentException("Too many items in array, maximum 10 allowed", "parameters");
            }
            //
            PointerToArrayOfPointerHelper argv = new PointerToArrayOfPointerHelper();
            argv.pointers = new IntPtr[11];
            //
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly != null) {
                argv.pointers[0] = Marshal.StringToHGlobalAnsi(assembly.Location);
            } else {
                assembly = Assembly.GetExecutingAssembly();
                argv.pointers[0] = Marshal.StringToHGlobalAnsi(assembly.Location);
            }

            for (int i = 0; i < parameters.Length; i++) {
                argv.pointers[i + 1] = Marshal.StringToHGlobalAnsi(parameters[i]);
            }

            IntPtr argvPtr = IntPtr.Zero;
            try {
                int size = Marshal.SizeOf(typeof (PointerToArrayOfPointerHelper));
                argvPtr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(argv, argvPtr, false);

                return libvlc_new(parameters.Length + 1, argvPtr, ref exception);
            } finally {
                for (int i = 0; i < parameters.Length + 1; i++) {
                    if (argv.pointers[i] != IntPtr.Zero) {
                        Marshal.FreeHGlobal(argv.pointers[i]);
                    }
                }
                if (argvPtr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(argvPtr);
                }
            }
        }

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern String libvlc_get_version();

        /// <summary>
        /// Decrement the reference count of a libvlc instance, and destroy it if it reaches zero
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_release(IntPtr libvlc_instance_t);

        #endregion

        #region Events

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VlcEventHandlerDelegate(IntPtr libvlc_event, IntPtr userData);

        #endregion

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_event_attach(IntPtr p_event_manager, libvlc_event_type_t i_event_type, IntPtr f_callback, IntPtr user_data, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, EntryPoint = "libvlc_event_type_name")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_event_type_name_internal(libvlc_event_type_t event_type);

        public static string libvlc_event_type_name(libvlc_event_type_t event_type) {
            IntPtr strPtr = libvlc_event_type_name_internal(event_type);
            return Marshal.PtrToStringAnsi(strPtr);
        }

        #endregion

        #region Media

        /// <summary>
        /// Create a media with the given MRL
        /// </summary>
        /// <param name="p_instance">libvlc instance </param>
        /// <param name="psz_mrl">MRL</param>
        /// <param name="exception">An initialized exception pointer</param>
        /// <returns>Media descriptor</returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_new(IntPtr p_instance, IntPtr psz_mrl,
                                                      ref libvlc_exception_t exception);

        /// <summary>
        /// Helper to the original function, <paramref name="mrl"/> string will be
        /// converted to the UTF-8 encoding automatically and marshalled to the libvlc.
        /// </summary>
        public static IntPtr libvlc_media_new(IntPtr p_instance, string mrl, ref libvlc_exception_t exception) {
            IntPtr pMrl = IntPtr.Zero;
            try {
                byte[] bytes = Encoding.UTF8.GetBytes(mrl);
                //
                pMrl = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, pMrl, bytes.Length);
                Marshal.WriteByte(pMrl, bytes.Length, 0);
                //
                return (libvlc_media_new(p_instance, pMrl, ref exception));
            } finally {
                if (pMrl != IntPtr.Zero) {
                    Marshal.FreeHGlobal(pMrl);
                }
            }
        }

        /// <summary>
        /// Add an option to the media
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_add_option(IntPtr libvlc_media_inst, IntPtr ppsz_options,
                                                           ref libvlc_exception_t exc);

        /// <summary>
        /// Adds an options in MRL's option format
        /// </summary>
        /// <param name="libvlc_media_inst">Media instance</param>
        /// <param name="options">Options string ("::sout=#transcode{vcodec=DIV3,vb=1024,scale=1}:duplicate{dst=display,dst=std{access=file,mux=ps,dst=\"C:\\temp.mpeg\"}}" for example)</param>
        /// <param name="exc">Initialized exemplar of libvlc_exception_t structure</param>
        public static void libvlc_media_add_option(IntPtr libvlc_media_inst, string options, ref libvlc_exception_t exc) {
            IntPtr pOptions = IntPtr.Zero;
            try {
                pOptions = Marshal.StringToHGlobalAnsi(options);

                libvlc_media_add_option(libvlc_media_inst, pOptions, ref exc);
            } finally {
                if (pOptions != IntPtr.Zero) {
                    Marshal.FreeHGlobal(pOptions);
                }
            }
        }

        /// <summary>
        /// Decrement the reference count of a media descriptor object
        /// </summary>
        /// <param name="libvlc_media_inst">Media descriptor</param>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_release(IntPtr libvlc_media_inst);

        /// <summary>
        /// Get current state of media descriptor object
        /// </summary>
        /// <param name="libvlc_media_inst">Media descriptor</param>
        /// <param name="exc">An initialized exception pointer</param>
        /// <returns>libvlc_state_t enum</returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern libvlc_state_t libvlc_media_get_state(IntPtr libvlc_media_inst, ref libvlc_exception_t exc);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_event_manager(IntPtr libvlc_media_t, ref libvlc_exception_t exception);

        #endregion

        #region Mediaplayer

        /// <summary>
        /// Create an empty Media Player object
        /// </summary>
        /// <param name="libvlc_instance">Pointer to instance</param>
        /// <param name="exception">An initialized exception pointer</param>
        /// <returns>Media player descriptor</returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_player_new(IntPtr libvlc_instance, ref libvlc_exception_t exception);

        /// <summary>
        /// Create a Media Player object from a Media
        /// </summary>
        /// <param name="libvlc_media">Media descriptor</param>
        /// <param name="exception">An initialized exception pointer</param>
        /// <returns>Media player descriptor</returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_player_new_from_media(IntPtr libvlc_media,
                                                                       ref libvlc_exception_t exception);

        /// <summary>
        /// Get the media used by the media_player
        /// </summary>
        /// <param name="libvlc_mediaplayer">Mediaplayer descriptor</param>
        /// <param name="exception">An initialized exception pointer</param>
        /// <returns>Media descriptor</returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_player_get_media(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_media(IntPtr libvlc_media_player_t, IntPtr libvlc_media_t, ref libvlc_exception_t exception);

        /// <summary>
        /// Play
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_play(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        /// <summary>
        /// Pause
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_pause(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        /// <summary>
        /// Stop
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_stop(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        /// <summary>
        /// Set the drawable where the media player should render its video output
        /// </summary>
        /// <param name="libvlc_mediaplayer">The Media Player</param>
        /// <param name="libvlc_drawable">The libvlc_drawable_t where the media player should render its video</param>
        /// <param name="exception">An initialized exception pointer</param>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_drawable(IntPtr libvlc_mediaplayer, Int32 libvlc_drawable,
                                                                   ref libvlc_exception_t exception);

        /// <summary>
        /// Get the current movie length (in ms)
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_player_get_length(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        /// <summary>
        /// Get the current movie time (in ms)
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_player_get_time(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        /// <summary>
        /// Set the movie time (in ms)
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_time(IntPtr libvlc_mediaplayer, Int64 time, ref libvlc_exception_t exception);

        /// <summary>
        /// Get current position (float part)
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern float libvlc_media_player_get_position(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        /// <summary>
        /// Set current position (float part)
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_position(IntPtr libvlc_mediaplayer, float position, ref libvlc_exception_t exception);

        /// <summary>
        /// Release a media_player after use Decrement the reference count of a media player object
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_release(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Get the Event MediaLibraryFactory from which the media player send event
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_player_event_manager(IntPtr libvlc_media_player_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern float libvlc_media_player_get_fps(IntPtr libvlc_media_player_t, ref libvlc_exception_t exception);

        #endregion

        #region Core->MediaPlayer->Video

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true,
            EntryPoint = "libvlc_video_reparent")]
        [SuppressUnmanagedCodeSecurity]
        private static extern Int32 libvlc_video_reparent_internal(IntPtr libvlc_mediaplayer, Int32 libvlc_drawable_t,
                                                                   ref libvlc_exception_t exc);

        public static bool libvlc_video_reparent(IntPtr libvlc_mediaplayer, Int32 libvlc_drawable_t,
                                                 ref libvlc_exception_t exc) {
            return libvlc_video_reparent_internal(libvlc_mediaplayer, libvlc_drawable_t, ref exc) != 0;
        }

        /// <summary>
        /// Get current video subtitle
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int libvlc_video_get_spu(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        /// <summary>
        /// Set new video subtitle
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_video_set_spu(IntPtr libvlc_mediaplayer, int spu, ref libvlc_exception_t exception);

        /// <summary>
        /// Take a snapshot of the current video window
        /// If i_width AND i_height is 0, original size is used. If i_width XOR i_height is 0, original aspect-ratio is preserved
        /// </summary>
        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_video_take_snapshot(IntPtr libvlc_media_player_t,
                                                             IntPtr filePath, UInt32 i_width, UInt32 i_height, ref libvlc_exception_t exc);

        #endregion

        #region Logging

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern UInt32 libvlc_get_log_verbosity(IntPtr libvlc_instance_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_set_log_verbosity(IntPtr libvlc_instance_t, UInt32 level, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_log_open(IntPtr libvlc_instance_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_log_close(IntPtr libvlc_log_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern UInt32 libvlc_log_count(IntPtr libvlc_log_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_log_clear(IntPtr libvlc_log_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_log_get_iterator(IntPtr libvlc_log_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_log_iterator_free(IntPtr libvlc_log_iterator_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int32 libvlc_log_iterator_has_next(IntPtr libvlc_log_iterator_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_log_iterator_next(IntPtr libvlc_log_iterator_t, ref libvlc_log_message_t p_buffer, ref libvlc_exception_t exception);

        #endregion
    }
}