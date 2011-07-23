// MusicBeeAPI - an abstraction layer bringing the MusicBee plugin API
// closer to the Microsoft(R) .NET(tm) design principles.
//
// Copyright (c) Jens Maier 2011
//
// Distributed under the Creative Commons BY 3.0 license.
// http://creativecommons.org/licenses/by/3.0/
//
// Based on the MusicBee plugin API (c) Steven Mayall 2008-2011.
// http://getmusicbee.com/
//
// This program code is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE.

using System;
using System.Runtime.InteropServices;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        public const short PluginInfoVersion = 1;
        public const short MinInterfaceVersion = 4;
        public const short MinApiRevision = 6;

        [StructLayout(LayoutKind.Sequential)]
        private struct MusicBeeApiInterface
        {
            public short InterfaceVersion;
            public short ApiRevision;
            public MB_ReleaseStringDelegate MB_ReleaseString;                                   // ?
            public MB_TraceDelegate MB_Trace;                                                   // ?
            public Setting_GetPersistentStoragePathDelegate Setting_GetPersistentStoragePath;   //done
            public Setting_GetSkinDelegate Setting_GetSkin;                                     //done
            public Setting_GetSkinElementColourDelegate Setting_GetSkinElementColour;           //done
            public Setting_IsWindowBordersSkinnedDelegate Setting_IsWindowBordersSkinned;       //done
            public Library_GetFilePropertyDelegate Library_GetFileProperty;                     //done
            public Library_GetFileTagDelegate Library_GetFileTag;                               //done
            public Library_SetFileTagDelegate Library_SetFileTag;                               //done
            public Library_CommitTagsToFileDelegate Library_CommitTagsToFile;                   //done
            public Library_GetLyricsDelegate Library_GetLyrics;                                 //done
            public Library_GetArtworkDelegate Library_GetArtwork;                               //done
            public Library_QueryFilesDelegate Library_QueryFiles;                               // TODO: library files
            public Library_QueryGetNextFileDelegate Library_QueryGetNextFile;
            public Player_GetPositionDelegate Player_GetPosition;                               //done
            public Player_SetPositionDelegate Player_SetPosition;                               //done
            public Player_GetPlayStateDelegate Player_GetPlayState;                             //done
            public Player_ActionDelegate Player_PlayPause;                                      //done
            public Player_ActionDelegate Player_Stop;                                           //done
            public Player_ActionDelegate Player_StopAfterCurrent;                               //done
            public Player_ActionDelegate Player_PlayPreviousTrack;                              //done
            public Player_ActionDelegate Player_PlayNextTrack;                                  //done
            public Player_ActionDelegate Player_StartAutoDj;                                    //done
            public Player_ActionDelegate Player_EndAutoDj;                                      //done
            public Player_GetVolumeDelegate Player_GetVolume;                                   //done
            public Player_SetVolumeDelegate Player_SetVolume;                                   //done
            public Player_GetMuteDelegate Player_GetMute;                                       //done
            public Player_SetMuteDelegate Player_SetMute;                                       //done
            public Player_GetShuffleDelegate Player_GetShuffle;                                 //done
            public Player_SetShuffleDelegate Player_SetShuffle;                                 //done
            public Player_GetRepeatDelegate Player_GetRepeat;                                   //done
            public Player_SetRepeatDelegate Player_SetRepeat;                                   //done
            public Player_GetEqualiserEnabledDelegate Player_GetEqualiserEnabled;               //done
            public Player_SetEqualiserEnabledDelegate Player_SetEqualiserEnabled;               //done
            public Player_GetDspEnabledDelegate Player_GetDspEnabled;                           //done
            public Player_SetDspEnabledDelegate Player_SetDspEnabled;                           //done
            public Player_GetScrobbleEnabledDelegate Player_GetScrobbleEnabled;                 //done
            public Player_SetScrobbleEnabledDelegate Player_SetScrobbleEnabled;                 //done
            public NowPlaying_GetFileUrlDelegate NowPlaying_GetFileUrl;                         //done
            public NowPlaying_GetDurationDelegate NowPlaying_GetDuration;                       //ignored, see Library_GetFileProperty(Duration)
            public NowPlaying_GetFilePropertyDelegate NowPlaying_GetFileProperty;               //ignored, see Library_GetFileProperty(...)
            public NowPlaying_GetFileTagDelegate NowPlaying_GetFileTag;                         //ignored, see Library_GetFileTag(...)
            public NowPlaying_GetLyricsDelegate NowPlaying_GetLyrics;                           //ignored, see Library_GetLyrics(...)
            public NowPlaying_GetArtworkDelegate NowPlaying_GetArtwork;                         //ignored, see Library_GetArtwork(...)
            public NowPlayingList_ActionDelegate NowPlayingList_Clear;                          //done
            public Library_QueryFilesDelegate NowPlayingList_QueryFiles;                        //done
            public Library_QueryGetNextFileDelegate NowPlayingList_QueryGetNextFile;            //done
            public NowPlayingList_FileActionDelegate NowPlayingList_PlayNow;                    //done
            public NowPlayingList_FileActionDelegate NowPlayingList_QueueNext;                  //done
            public NowPlayingList_FileActionDelegate NowPlayingList_QueueLast;                  //done
            public NowPlayingList_ActionDelegate NowPlayingList_PlayLibraryShuffled;            //done
            public Playlist_QueryPlaylistsDelegate Playlist_QueryPlaylists;                     // TODO: playlists
            public Playlist_QueryGetNextPlaylistDelegate Playlist_QueryGetNextPlaylist;
            public Playlist_GetTypeDelegate Playlist_GetType;
            public Library_QueryFilesDelegate Playlist_QueryFiles;
            public Library_QueryGetNextFileDelegate Playlist_QueryGetNextFile;
            public MB_WindowHandleDelegate MB_GetWindowHandle;                                  //done
            public MB_RefreshPanelsDelegate MB_RefreshPanels;                                   // ?
            public MB_SendNotificationDelegate MB_SendNotification;                             // ?
            public MB_AddMenuItemDelegate MB_AddMenuItem;
            public Setting_GetFieldNameDelegate Setting_GetFieldName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class PluginInfo
        {
            public short PluginInfoVersion;
            public PluginType Type;
            public string Name;
            public string Description;
            public string Author;
            public string TargetApplication;
            public short VersionMajor;
            public short VersionMinor;
            public short Revision;
            public short MinInterfaceVersion;
            public short MinApiRevision;
            public ReceiveNotificationFlags ReceiveNotifications;
            public int ConfigurationPanelHeight;
        }

        [Flags()]
        public enum ReceiveNotificationFlags
        {
            StartupOnly = 0x0,
            PlayerEvents = 0x1,
            DataStreamEvents = 0x2,
            TagEvents = 0x4
        }

        private enum SkinElement
        {
            SkinInputControl = 7,
            SkinInputPanel = 10,
            SkinInputPanelLabel = 14
        }

        private enum ElementState
        {
            ElementStateDefault = 0,
            ElementStateModified = 6
        }

        private enum ElementComponent
        {
            ComponentBorder = 0,
            ComponentBackground = 1,
            ComponentForeground = 3
        }

        private delegate void MB_ReleaseStringDelegate(string p1);
        private delegate void MB_TraceDelegate(string p1);
        private delegate IntPtr MB_WindowHandleDelegate();
        private delegate void MB_RefreshPanelsDelegate();
        private delegate void MB_SendNotificationDelegate(CallbackType type);
        private delegate void MB_AddMenuItemDelegate(string menuPath, string hotkeyDescription, EventHandler handler);
        private delegate string Setting_GetFieldNameDelegate(MetaDataType type);
        private delegate string Setting_GetPersistentStoragePathDelegate();
        private delegate string Setting_GetSkinDelegate();
        private delegate int Setting_GetSkinElementColourDelegate(SkinElement element, ElementState state, ElementComponent component);
        private delegate bool Setting_IsWindowBordersSkinnedDelegate();
        private delegate string Library_GetFilePropertyDelegate(string sourceFileUrl, FilePropertyType type);
        private delegate string Library_GetFileTagDelegate(string sourceFileUrl, MetaDataType type);
        private delegate bool Library_SetFileTagDelegate(string sourceFileUrl, MetaDataType type, string value);
        private delegate bool Library_CommitTagsToFileDelegate(string sourceFileUrl);
        private delegate string Library_GetLyricsDelegate(string sourceFileUrl, int type);
        private delegate string Library_GetArtworkDelegate(string sourceFileUrl, int index);
        private delegate bool Library_QueryFilesDelegate(string query);
        private delegate string Library_QueryGetNextFileDelegate();
        private delegate int Player_GetPositionDelegate();
        private delegate bool Player_SetPositionDelegate(int position);
        private delegate PlayState Player_GetPlayStateDelegate();
        private delegate bool Player_ActionDelegate();
        private delegate float Player_GetVolumeDelegate();
        private delegate bool Player_SetVolumeDelegate(float volume);
        private delegate bool Player_GetMuteDelegate();
        private delegate bool Player_SetMuteDelegate(bool mute);
        private delegate bool Player_GetShuffleDelegate();
        private delegate bool Player_SetShuffleDelegate(bool shuffle);
        private delegate RepeatMode Player_GetRepeatDelegate();
        private delegate bool Player_SetRepeatDelegate(RepeatMode repeat);
        private delegate bool Player_GetEqualiserEnabledDelegate();
        private delegate bool Player_SetEqualiserEnabledDelegate(bool shuffle);
        private delegate bool Player_GetDspEnabledDelegate();
        private delegate bool Player_SetDspEnabledDelegate(bool shuffle);
        private delegate bool Player_GetScrobbleEnabledDelegate();
        private delegate bool Player_SetScrobbleEnabledDelegate(bool shuffle);
        private delegate string NowPlaying_GetFileUrlDelegate();
        private delegate int NowPlaying_GetDurationDelegate();
        private delegate string NowPlaying_GetFilePropertyDelegate(FilePropertyType type);
        private delegate string NowPlaying_GetFileTagDelegate(MetaDataType type);
        private delegate string NowPlaying_GetLyricsDelegate();
        private delegate string NowPlaying_GetArtworkDelegate();
        private delegate bool NowPlayingList_ActionDelegate();
        private delegate bool NowPlayingList_FileActionDelegate(string sourceFileUrl);
        private delegate bool Playlist_QueryPlaylistsDelegate();
        private delegate string Playlist_QueryGetNextPlaylistDelegate();
        private delegate PlaylistFormat Playlist_GetTypeDelegate(string playlistUrl);
    }

    public enum PluginType
    {
        Unknown = 0,
        General = 1,
        LyricsRetrieval = 2,
        ArtworkRetrieval = 3,
        PanelView = 4,
        DataStream = 5,
        InstantMessenger = 6,
        Storage = 7
    }

    public enum NotificationType
    {
        PluginStartup = 0,          // notification sent after successful initialisation for an enabled plugin
        TrackChanged = 1,
        PlayStateChanged = 2,
        AutoDjStarted = 3,
        AutoDjStopped = 4,
        VolumeMuteChanged = 5,
        VolumeLevelChanged = 6,
        NowPlayingListChanged = 7,
        NowPlayingArtworkReady = 8,
        NowPlayingLyricsReady = 9,
        TagsChanging = 10,
        TagsChanged = 11,
        RatingChanged = 12,
        PlayCountersChanged = 13
    }

    public enum PluginCloseReason
    {
        MusicBeeClosing = 1,
        UserDisabled = 2
    }

    public enum CallbackType
    {
        SettingsUpdated = 1,
        StorageReady = 2,
        StorageFailed = 3,
        FilesRetrievedChanged = 4,
        FilesRetrievedNoChange = 5,
        FilesRetrievedFail = 6
    }

    public enum FilePropertyType
    {
        Url = 2,
        Kind = 4,
        Format = 5,
        Size = 7,
        Channels = 8,
        SampleRate = 9,
        Bitrate = 10,
        DateModified = 11,
        DateAdded = 12,
        LastPlayed = 13,
        PlayCount = 14,
        SkipCount = 15,
        Duration = 16,
        ReplayGainTrack = 94,
        ReplayGainAlbum = 95
    }

    public enum MetaDataType
    {
        TrackTitle = 65,
        Album = 30,
        AlbumArtist = 31,        // displayed album artist
        AlbumArtistRaw = 34,     // stored album artist
        Artist = 32,             // displayed artist
        MultiArtist = 33,        // individual artists, separated by a null char        // TODO
        Artwork = 40,
        BeatsPerMin = 41,
        Composer = 43,           // displayed composer
        MultiComposer = 89,      // individual composers, separated by a null char      // TODO
        Comment = 44,
        Conductor = 45,
        Custom1 = 46,
        Custom2 = 47,
        Custom3 = 48,
        Custom4 = 49,
        Custom5 = 50,
        Custom6 = 96,
        Custom7 = 97,
        Custom8 = 98,
        Custom9 = 99,
        DiscNo = 52,
        DiscCount = 54,
        Encoder = 55,
        Genre = 59,
        GenreCategory = 60,
        Grouping = 61,
        Keywords = 84,
        Lyricist = 62,
        Mood = 64,
        Occasion = 66,
        Origin = 67,
        Publisher = 73,
        Quality = 74,
        Rating = 75,
        RatingLove = 76,
        RatingAlbum = 104,
        Tempo = 85,
        TrackNo = 86,
        TrackCount = 87,
        Year = 88
    }

    public enum LyricsType
    {
        NotSpecified = 0,
        Synchronised = 1,
        UnSynchronised = 2
    }

    public enum PlayState
    {
        Undefined = 0,
        Loading = 1,
        Playing = 3,
        Paused = 6,
        Stopped = 7
    }

    public enum RepeatMode
    {
        None = 0,
        All = 1,
        One = 2
    }

    public enum PlaylistFormat
    {
        Unknown = 0,
        M3u = 1,
        Xspf = 2,
        Asx = 3,
        Wpl = 4,
        Pls = 5,
        Auto = 7,
        M3uAscii = 8
    }
}