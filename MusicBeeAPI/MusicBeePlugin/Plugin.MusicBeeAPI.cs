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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

namespace MusicBeePlugin
{
    partial class Plugin
    {
        private partial class MusicBeeAPI : IMusicBeeAPI
        {
            private Form mbMainForm;
            private MusicBeeApiInterface mbApiInterface;

            private readonly object syncRoot = new object();

            public MusicBeeAPI(IntPtr apiInterfacePtr) {
                mbMainForm = Application.OpenForms[0];
                mbApiInterface = (MusicBeeApiInterface) Marshal.PtrToStructure(apiInterfacePtr,
                    typeof(MusicBeeApiInterface));
            }

            // ..................................................................

            #region Events and Event Handling

            public event UnhandledExceptionEventHandler UnhandledException;
            public event PluginEventHandler<NotificationEventArgs> NotificationReceived;
            public event PluginEventHandler<NotificationEventArgs> PlayStateChanged;
            public event PluginEventHandler<NotificationEventArgs> TrackChanged;
            public event PluginEventHandler<NotificationEventArgs> AutoDjStarted;
            public event PluginEventHandler<NotificationEventArgs> AutoDjStopped;
            public event PluginEventHandler<NotificationEventArgs> NowPlayingArtworkReady;
            public event PluginEventHandler<NotificationEventArgs> NowPlayingListChanging;
            public event PluginEventHandler<NotificationEventArgs> NowPlayingListChanged;
            public event PluginEventHandler<NotificationEventArgs> NowPlayingLyricsReady;
            public event PluginEventHandler<NotificationEventArgs> RatingChanged;
            public event PluginEventHandler<NotificationEventArgs> PlayCountersChanged;
            public event PluginEventHandler<NotificationEventArgs> TagsChanged;
            public event PluginEventHandler<NotificationEventArgs> TagsChanging;
            public event PluginEventHandler<NotificationEventArgs> VolumeLevelChanged;
            public event PluginEventHandler<NotificationEventArgs> VolumeMuteChanged;
            public event PluginEventHandler<NotificationEventArgs> PluginStartup;
            public event PluginEventHandler<PluginEventArgs> SettingsSaved;
            public event PluginEventHandler<PluginEventArgs> PluginUninstalled;
            public event PluginEventHandler<PluginClosedEventArgs> PluginClosed;

            public void RaiseSettingsSaved() {
                if (SettingsSaved != null) SettingsSaved(new PluginEventArgsImpl(this));
            }

            public void RaisePluginClosed(PluginCloseReason reason) {
                if (PluginClosed != null) PluginClosed(new PluginClosedEventArgsImpl(this) { Reason = reason });
            }

            public void RaisePluginUninstalled() {
                if (PluginUninstalled != null) PluginUninstalled(new PluginEventArgsImpl(this));
            }

            public void ProcessNotification(string sourceFileUrl, NotificationType type) {
                if (NotificationReceived != null) {
                    NotificationReceived(new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                }
                switch (type) {
                    case NotificationType.PlayStateChanged:
                        if (PlayerState == PlayState.Stopped) {
                            internalStopAfterCurrent = false;
                        }
                        if (PlayStateChanged != null) PlayStateChanged(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.TrackChanged:
                        internalStopAfterCurrent = false;
                        if (TrackChanged != null) TrackChanged(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.AutoDjStarted:
                        if (AutoDjStarted != null) AutoDjStarted(
                            new NotificationEventArgsImpl(this) { EventType = type });
                        break;

                    case NotificationType.AutoDjStopped:
                        if (AutoDjStopped != null) AutoDjStopped(
                            new NotificationEventArgsImpl(this) { EventType = type });
                        break;

                    case NotificationType.NowPlayingArtworkReady:
                        if (NowPlayingArtworkReady != null) NowPlayingArtworkReady(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.NowPlayingListChanged:
                        if (NowPlayingListChanging != null) NowPlayingListChanging(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        if (NowPlayingListChanged != null) NowPlayingListChanged(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.NowPlayingLyricsReady:
                        if (NowPlayingLyricsReady != null) NowPlayingLyricsReady(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.PluginStartup:
                        internalStopAfterCurrent = false;
                        if (PluginStartup != null) PluginStartup(
                            new NotificationEventArgsImpl(this) { EventType = type });
                        break;

                    case NotificationType.TagsChanging:
                        if (TagsChanging != null) TagsChanging(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.RatingChanged:
                        if (RatingChanged != null) RatingChanged(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.PlayCountersChanged:
                        if (PlayCountersChanged != null) PlayCountersChanged(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.TagsChanged:
                        if (TagsChanged != null) TagsChanged(
                            new NotificationEventArgsImpl(this) { EventType = type, RelatedFilePath = sourceFileUrl });
                        break;

                    case NotificationType.VolumeLevelChanged:
                        if (VolumeLevelChanged != null) VolumeLevelChanged(
                            new NotificationEventArgsImpl(this) { EventType = type });
                        break;

                    case NotificationType.VolumeMuteChanged:
                        if (VolumeMuteChanged != null) VolumeMuteChanged(
                            new NotificationEventArgsImpl(this) { EventType = type });
                        break;
                }
            }

            public void HandleException(Exception e) {
                try {
                    if (UnhandledException != null) UnhandledException(this,
                        new UnhandledExceptionEventArgs(e, false));
                } catch (Exception e2) {
                    System.Diagnostics.Debug.Print("\r\nException handler failed while processing an unhandled exception: {0}\r\n{1}\r\n", e2.Message, e2.StackTrace);
                    System.Diagnostics.Debug.Print("The original exception was: {0}\r\n{1}\r\n", e.Message, e.StackTrace);
                }
            }

            #endregion

            // ..................................................................

            #region UI and Miscellaneous

            public Form MainWindow { get { return mbMainForm; } }

            public void Invoke(Delegate callback) { mbMainForm.Invoke(callback); }
            public void Invoke(Delegate callback, params object[] args) { mbMainForm.Invoke(callback, args); }
            public void BeginInvoke(Delegate callback) { mbMainForm.BeginInvoke(callback); }
            public void BeginInvoke(Delegate callback, params object[] args) { mbMainForm.BeginInvoke(callback, args); }

            public String PersistentStoragePath {
                get { return mbApiInterface.Setting_GetPersistentStoragePath(); }
            }

            private SkinInfo skin;
            public ISkinInfo Skin {
                get {
                    if (skin == null || !skin.Name.Equals(mbApiInterface.Setting_GetSkin())) {
                        skin = new SkinInfo(mbApiInterface);
                    }
                    return skin;
                }
            }

            #endregion

            // ..................................................................

            #region Player Commands

            public PlayState PlayerState {
                get { return mbApiInterface.Player_GetPlayState(); }
                set {
                    switch (value) {
                        case PlayState.Paused:
                            if (PlayerState == PlayState.Playing) mbApiInterface.Player_PlayPause();
                            break;
                        case PlayState.Stopped:
                            if (PlayerState != PlayState.Stopped) mbApiInterface.Player_Stop();
                            break;
                        case PlayState.Playing:
                            if (PlayerState != PlayState.Playing) mbApiInterface.Player_PlayPause();
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
            }

            public int PlayerPosition {
                get { return mbApiInterface.Player_GetPosition(); }
                set { mbApiInterface.Player_SetPosition(value); }
            }

            public void PlayNextTrack() { mbApiInterface.Player_PlayNextTrack(); }
            public void PlayPreviousTrack() { mbApiInterface.Player_PlayPreviousTrack(); }

            //public void StopAfterCurrent() { mbApiInterface.Player_StopAfterCurrent(); }
            private bool internalStopAfterCurrent = false;
            public Boolean StopAfterCurrent {
                get { return internalStopAfterCurrent; }
                set {
                    if (internalStopAfterCurrent != value) {
                        mbApiInterface.Player_StopAfterCurrent();
                        internalStopAfterCurrent = value;
                    }
                }
            }

            public bool AutoDj {
                set {
                    if (value) {
                        mbApiInterface.Player_StartAutoDj();
                    } else {
                        mbApiInterface.Player_EndAutoDj();
                    }
                }
            }

            public float Volume {
                get { return mbApiInterface.Player_GetVolume(); }
                set { mbApiInterface.Player_SetVolume(value); }
            }

            public bool Mute {
                get { return mbApiInterface.Player_GetMute(); }
                set { mbApiInterface.Player_SetMute(value); }
            }

            public bool Shuffle {
                get { return mbApiInterface.Player_GetShuffle(); }
                set { mbApiInterface.Player_SetShuffle(value); }
            }

            public RepeatMode Repeat {
                get { return mbApiInterface.Player_GetRepeat(); }
                set { mbApiInterface.Player_SetRepeat(value); }
            }

            public Boolean EqualiserEnabled {
                get { return mbApiInterface.Player_GetEqualiserEnabled(); }
                set { mbApiInterface.Player_SetEqualiserEnabled(value); }
            }

            public Boolean DspEnabled {
                get { return mbApiInterface.Player_GetDspEnabled(); }
                set { mbApiInterface.Player_SetDspEnabled(value); }
            }

            public Boolean ScrobbleEnabled {
                get { return mbApiInterface.Player_GetScrobbleEnabled(); }
                set { mbApiInterface.Player_SetScrobbleEnabled(value); }
            }

            #endregion

            // ..................................................................

            #region Library and NowPlayingList Commands

            public IFileInfo CurrentlyPlaying {
                get { return new FileInfo(this, mbApiInterface.NowPlaying_GetFileUrl()); }
            }

            public IFileInfo GetFileInfo(string localPath) {
                return new FileInfo(this, localPath);
            }

            public IFileInfo GetFileInfo(Uri uri) {
                return new FileInfo(this, uri.OriginalString);
            }

            public void PlayNow(Uri file) { mbApiInterface.NowPlayingList_PlayNow(file.LocalPath); }
            public void PlayNow(IFileInfo file) { mbApiInterface.NowPlayingList_PlayNow(file.Uri.OriginalString); }

            public void PlayLibraryShuffled() { mbApiInterface.NowPlayingList_PlayLibraryShuffled(); }

            public void QueueNext(Uri file) { mbApiInterface.NowPlayingList_QueueNext(file.LocalPath); }
            public void QueueNext(IFileInfo file) { mbApiInterface.NowPlayingList_QueueNext(file.Uri.OriginalString); }

            public void QueueLast(Uri file) { mbApiInterface.NowPlayingList_QueueLast(file.LocalPath); }
            public void QueueLast(IFileInfo file) { mbApiInterface.NowPlayingList_QueueLast(file.Uri.OriginalString); }

            private void ClearNowPlayingList() { mbApiInterface.NowPlayingList_Clear(); } // publicly accessed via NowPlaying List

            private NowPlayingListImpl nowPlayingList;
            public INowPlayingList NowPlayingList {
                get {
                    if (nowPlayingList == null) {
                        lock (syncRoot) {
                            if (nowPlayingList == null) nowPlayingList = new NowPlayingListImpl(this);
                        }
                    }
                    return nowPlayingList;
                }
            }

            private List<IFileInfo> FetchNowPlaying() {
                List<IFileInfo> res = new List<IFileInfo>();
                if (mbApiInterface.NowPlayingList_QueryFiles(null)) {
                    string file;
                    while ((file = mbApiInterface.NowPlayingList_QueryGetNextFile()) != null) {
                        res.Add(new FileInfo(this, file));
                    }
                }
                return res;
            }

            #endregion

            // ..................................................................

            private class PluginEventArgsImpl : PluginEventArgs
            {
                public PluginEventArgsImpl(MusicBeeAPI api) { Owner = api; }
                protected MusicBeeAPI Owner { get; private set; }
                IMusicBeeAPI PluginEventArgs.API { get { return Owner; } }
            }

            private class PluginClosedEventArgsImpl : PluginEventArgsImpl, PluginClosedEventArgs
            {
                public PluginClosedEventArgsImpl(MusicBeeAPI api) : base(api) { }
                public PluginCloseReason Reason { get; set; }
            }

            private class NotificationEventArgsImpl : PluginEventArgsImpl, NotificationEventArgs
            {
                public NotificationEventArgsImpl(MusicBeeAPI api) : base(api) { }
                public NotificationType EventType { get; set; }
                public IFileInfo RelatedFile { get; set; }
                public string RelatedFilePath {
                    get { return RelatedFile.ToString(); }
                    set { RelatedFile = value == null ? null : new FileInfo(Owner, value); }
                }
            }

            private class NowPlayingListImpl : INowPlayingList
            {
                private MusicBeeAPI API;
                private bool cacheValid;

                public NowPlayingListImpl(MusicBeeAPI api) {
                    API = api;
                    cacheValid = false;
                    API.NowPlayingListChanging += ClearCache;
                }

                private List<IFileInfo> _cache;
                private List<IFileInfo> Cache {
                    get {
                        if (!cacheValid) {
                            _cache = API.FetchNowPlaying();
                            cacheValid = true;
                        }
                        return _cache;
                    }
                }

                private void ClearCache(NotificationEventArgs args) {
                    cacheValid = false;
                    _cache = null;
                }

                public int IndexOf(IFileInfo item) {
                    return Cache.IndexOf(item);
                }

                public void Insert(int index, IFileInfo item) {
                    throw new NotImplementedException(); // TODO: complete now playing list
                }

                public void RemoveAt(int index) {
                    throw new NotImplementedException(); // ...
                }

                public IFileInfo this[int index] {
                    get {
                        return Cache[index];
                    }
                    set {
                        throw new NotImplementedException(); // ...
                    }
                }

                public void Add(IFileInfo item) {
                    API.QueueLast(item.Uri);
                }

                public void Clear() {
                    API.ClearNowPlayingList();
                }

                public bool Contains(IFileInfo item) {
                    return Cache.Contains(item);
                }

                public void CopyTo(IFileInfo[] array, int arrayIndex) {
                    Cache.CopyTo(array, arrayIndex);
                }

                public int Count {
                    get { return Cache.Count; }
                }

                bool ICollection<IFileInfo>.IsReadOnly {
                    get { return ((IList) Cache).IsReadOnly; }
                }

                public bool Remove(IFileInfo item) {
                    throw new NotImplementedException(); // ... end TODO
                }

                public IEnumerator<IFileInfo> GetEnumerator() {
                    return Cache.GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator() {
                    return Cache.GetEnumerator();
                }
            }
        }
    }
}
