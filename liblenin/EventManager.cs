using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace liblenin
{
    public class EventManager
    {
        private IntPtr m_handle = IntPtr.Zero;

        public enum EventType : int
        {
            libvlc_MediaMetaChanged = 0,
            libvlc_MediaSubItemAdded,
            libvlc_MediaDurationChanged,
            libvlc_MediaParsedChanged,
            libvlc_MediaFreed,
            libvlc_MediaStateChanged,

            libvlc_MediaPlayerMediaChanged = 0x100,
            libvlc_MediaPlayerNothingSpecial,
            libvlc_MediaPlayerOpening,
            libvlc_MediaPlayerBuffering,
            libvlc_MediaPlayerPlaying,
            libvlc_MediaPlayerPaused,
            libvlc_MediaPlayerStopped,
            libvlc_MediaPlayerForward,
            libvlc_MediaPlayerBackward,
            libvlc_MediaPlayerEndReached,
            libvlc_MediaPlayerEncounteredError,
            libvlc_MediaPlayerTimeChanged,
            libvlc_MediaPlayerPositionChanged,
            libvlc_MediaPlayerSeekableChanged,
            libvlc_MediaPlayerPausableChanged,
            libvlc_MediaPlayerTitleChanged,
            libvlc_MediaPlayerSnapshotTaken,
            libvlc_MediaPlayerLengthChanged,

            libvlc_MediaListItemAdded = 0x200,
            libvlc_MediaListWillAddItem,
            libvlc_MediaListItemDeleted,
            libvlc_MediaListWillDeleteItem,

            libvlc_MediaListViewItemAdded = 0x300,
            libvlc_MediaListViewWillAddItem,
            libvlc_MediaListViewItemDeleted,
            libvlc_MediaListViewWillDeleteItem,

            libvlc_MediaListPlayerPlayed = 0x400,
            libvlc_MediaListPlayerNextItemSet,
            libvlc_MediaListPlayerStopped,

            libvlc_MediaDiscovererStarted = 0x500,
            libvlc_MediaDiscovererEnded,

            libvlc_VlmMediaAdded = 0x600,
            libvlc_VlmMediaRemoved,
            libvlc_VlmMediaChanged,
            libvlc_VlmMediaInstanceStarted,
            libvlc_VlmMediaInstanceStopped,
            libvlc_VlmMediaInstanceStatusInit,
            libvlc_VlmMediaInstanceStatusOpening,
            libvlc_VlmMediaInstanceStatusPlaying,
            libvlc_VlmMediaInstanceStatusPause,
            libvlc_VlmMediaInstanceStatusEnd,
            libvlc_VlmMediaInstanceStatusError,
        };

        public EventManager(IntPtr hEventManager)
        {
            m_handle = hEventManager;
        }

        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern string libvlc_event_type_name(Int32 type);
        public static string EventName(EventType type)
        {
            return libvlc_event_type_name((Int32)type);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void NativeEventCallback(IntPtr pEventStruct, IntPtr pUserData);
        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int libvlc_event_attach(
            IntPtr hEventManager,
            [param: MarshalAs(UnmanagedType.I4)] EventType type,
            [param: MarshalAs(UnmanagedType.FunctionPtr)] NativeEventCallback callback,
            IntPtr pUserData);
        [DllImport("libvlc.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libvlc_event_detach(
            IntPtr hEventManager,
            [param: MarshalAs(UnmanagedType.I4)] EventType type,
            [param: MarshalAs(UnmanagedType.FunctionPtr)] NativeEventCallback callback,
            IntPtr pUserData);

        public bool Attach(EventType aType, NativeEventCallback callback)
        {
            return 0 == libvlc_event_attach(m_handle, aType, callback, IntPtr.Zero);
        }


        [StructLayout(LayoutKind.Explicit)]
        private struct native_event
        {
            //    int   type; /**< Event type (see @ref libvlc_event_e) */
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.I4)]
            EventType mType;

            //    void *p_obj; /**< Object emitting the event */
            [FieldOffset(4)]
            [MarshalAs(UnmanagedType.I4)]
            IntPtr mObject;
            /************************************************************************/
            /* UNIT BEGIN                                                           */
            /************************************************************************/

//        struct
//        {
//            libvlc_meta_t meta_type;
//        } media_meta_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mMetaType;

//        struct
//        {
//            libvlc_media_t * new_child;
//        } media_subitem_added;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I4)]
            IntPtr mNewChild;

//        struct
//        {
//            int64_t new_duration;
//        } media_duration_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I8)]
            Int64 mNewDuration;

//        struct
//        {
//            int new_status;
//        } media_parsed_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mNewStatus;

//        struct
//        {
//            libvlc_media_t * md;
//        } media_freed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I4)]
            IntPtr mD;

//        struct
//        {
//            libvlc_state_t new_state;
//        } media_state_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mNewState;

//        struct
//        {
//            float new_position;
//        } media_player_position_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.R4)]
            float mNewPosition;

//        struct
//        {
//            libvlc_time_t new_time;
            //        } media_player_time_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I8)]
            Int64 mNewTime;

//        struct
//        {
//            int new_title;
//        } media_player_title_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mNewTitle;

//        struct
//        {
//            int new_seekable;
//        } media_player_seekable_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mNewSeekable;

//        struct
//        {
//            int new_pausable;
//        } media_player_pausable_changed;
            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mNewPausable;

//        struct
//        {
//            libvlc_media_t * item;
//            int index;
//        } media_list_item_added;
            [FieldOffset(8)]
            IntPtr mItem;

            [FieldOffset(12)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mIndex;

//        struct
//        {
//            libvlc_media_t * item;
//            int index;
//        } media_list_will_add_item;
            [FieldOffset(8)]
            IntPtr mWillItem;

            [FieldOffset(12)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mWillIndex;

//        struct
//        {
//            libvlc_media_t * item;
//            int index;
//        } media_list_item_deleted;
            [FieldOffset(8)]
            IntPtr mItemDeleted;

            [FieldOffset(12)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mIndexDeleted;

//        struct
//        {
//            libvlc_media_t * item;
//            int index;
//        } media_list_will_delete_item;
            [FieldOffset(8)]
            IntPtr mWillItemDeleted;

            [FieldOffset(12)]
            [MarshalAs(UnmanagedType.I4)]
            Int32 mWillIndexDeleted;

//        /* media list player */
        }
//        struct
//        {
//            libvlc_media_t * item;
//        } media_list_player_next_item_set;

//        /* snapshot taken */
//        struct
//        {
//             char* psz_filename ;
//        } media_player_snapshot_taken ;

//        /* Length changed */
//        struct
//        {
//            libvlc_time_t   new_length;
//        } media_player_length_changed;

//        /* VLM media */
//        struct
//        {
//            const char * psz_media_name;
//            const char * psz_instance_name;
//        } vlm_media_event;

//        /* Extra MediaPlayer */
//        struct
//        {
//            libvlc_media_t * new_media;
//        } media_player_media_changed;
//    } u; /**< Type-dependent event description */
//} libvlc_event_t;
    }
}
