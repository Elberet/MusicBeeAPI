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
using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    /// <summary>
    /// Provides a managed interface to the MusicBee API.
    /// </summary>
    public interface IMusicBeeAPI
    {
        #region Events

        /// <summary>
        /// Event raised when the Auto-Dj is stopped.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> AutoDjStarted;

        /// <summary>
        /// Event raised when the Auto-Dj is started.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> AutoDjStopped;

        /// <summary>
        /// Event raised when any notification is received from MusicBee. Using
        /// this event is strongly discouraged.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> NotificationReceived;

        /// <summary>
        /// Event raised when the corresponding MusicBee notification arrives.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> NowPlayingArtworkReady;

        /// <summary>
        /// Event raised when the NowPlaying list is modified either
        /// programatically or by user interaction.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> NowPlayingListChanged;

        /// <summary>
        /// Event raised when the corresponding MusicBee notification arrives.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> NowPlayingLyricsReady;

        /// <summary>
        /// Event raised when the player state changes, either programatically
        /// or by user interaction. Note: an event is raised when the "Next Track"
        /// button or API call is used, but not when the current track is finished
        /// and MusicBee continues with the next track automatically.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> PlayStateChanged;

        /// <summary>
        /// Document me.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> RatingChanged;

        /// <summary>
        /// Document me.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> PlayCountersChanged;

        /// <summary>
        /// Event raised when tags of a file have been changed by the user but
        /// before they have been committed to the database or file.<para/>
        /// Only delivered if TagEvents in the plugin frontend attribute is true.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> TagsChanging;

        /// <summary>
        /// Event raised when the tags of a file have been changed and successfully
        /// committed to the database and/or file. The event arguments object may
        /// contain hints as to which tags have been modified.<para/>
        /// Only delivered if TagEvents in the plugin frontend attribute is true.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> TagsChanged;

        /// <summary>
        /// Event raised when the current track changes. It is not guaranteed that
        /// the track is being played, only that it is loaded and will be played
        /// once playback begins.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> TrackChanged;

        /// <summary>
        /// Event raised when the volume level is changed, either programatically
        /// or by user interaction.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> VolumeLevelChanged;

        /// <summary>
        /// Event raised when the volume is muted or unmuted, either programatically
        /// or by user interaction.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> VolumeMuteChanged;

        /// <summary>
        /// Event raised when the plugin has been loaded and enabled and is
        /// requested to start running. This event is sent only once during the
        /// lifetime of the plugin.
        /// </summary>
        event PluginEventHandler<NotificationEventArgs> PluginStartup;

        /// <summary>
        /// Event raised when another event handler produced an unhandled or
        /// exception.
        /// </summary>
        event UnhandledExceptionEventHandler UnhandledException;

        /// <summary>
        /// Event raised when the plugin is being closed by MusicBee. Once this event
        /// has been processed, no further events will be delivered and the plugin
        /// should make no more calls to the MusicBee API.
        /// </summary>
        event PluginEventHandler<PluginClosedEventArgs> PluginClosed;

        /// <summary>
        /// Event raised when the plugin is being uninstalled from MusicBee. The
        /// plugin should remove any configuration data, caches and temporary files.
        /// </summary>
        event PluginEventHandler<PluginEventArgs> PluginUninstalled;

        /// <summary>
        /// Event raised when modified settings are expected to be saved and committed.
        /// Typically triggered by the user clicking Save or Apply in the MusicBee
        /// preferences dialog.
        /// </summary>
        event PluginEventHandler<PluginEventArgs> SettingsSaved;

        #endregion

        // ................................................................................

        #region UI and Configuration

        /// <summary>
        /// Gets the Form representing MusicBee's main window.
        /// </summary>
        Form MainWindow { get; }

        /// <summary>
        /// Syncronously runs the given delegate on MusicBee's UI thread.
        /// </summary>
        void Invoke(Delegate callback);

        /// <summary>
        /// Syncronously runs the given delegate on MusicBee's UI thread.
        /// </summary>
        void Invoke(Delegate callback, params object[] args);

        /// <summary>
        /// Schedules the given delegate to be run on MusicBee's UI thread.
        /// </summary>
        void BeginInvoke(Delegate callback);

        /// <summary>
        /// Schedules the given delegate to be run on MusicBee's UI thread.
        /// </summary>
        void BeginInvoke(Delegate callback, params object[] args);

        /// <summary>
        /// Gets detailed information about MusicBee's current skin.
        /// </summary>
        ISkinInfo Skin { get; }

        /// <summary>
        /// Gets the local path where MusicBee's configuration files and settings
        /// are stored. Plugin configuration and settings should be stored in a new
        /// subdirectory below this path.
        /// </summary>
        String PersistentStoragePath { get; }

        #endregion

        // ................................................................................

        #region Files, Library, Playlists

        /// <summary>
        /// Returns the file information wrapper for the given local file.
        /// </summary>
        IFileInfo GetFileInfo(string localPath);

        /// <summary>
        /// Returns the file information wrapper for the given URI.
        /// </summary>
        IFileInfo GetFileInfo(Uri uri);

        /// <summary>
        /// Returns the NowPlaying list wrapper.
        /// </summary>
        INowPlayingList NowPlayingList { get; }

        /// <summary>
        /// Clears the NowPlaying list, loads the specified file and starts playback.
        /// </summary>
        void PlayNow(Uri file);

        /// <summary>
        /// Clears the NowPlaying list, loads the specified file and starts playback.
        /// </summary>
        void PlayNow(IFileInfo file);

        /// <summary>
        /// Clears the NowPlaylist list and begins playback, selecting tracks from the
        /// library at random.
        /// </summary>
        void PlayLibraryShuffled();

        /// <summary>
        /// Inserts the specified file in the NowPlaying list immediately after the
        /// currently playing file.
        /// </summary>
        void QueueNext(Uri file);

        /// <summary>
        /// Inserts the specified file in the NowPlaying list immediately after the
        /// currently playing file.
        /// </summary>
        void QueueNext(IFileInfo file);

        /// <summary>
        /// Appends the specified file to the end of the NowPlaying list.
        /// </summary>
        void QueueLast(Uri file);

        /// <summary>
        /// Appends the specified file to the end of the NowPlaying list.
        /// </summary>
        void QueueLast(IFileInfo file);

        #endregion

        // ................................................................................

        #region Player

        /// <summary>
        /// Gets the currently playing file. It is not guaranteed that playback has been
        /// started, only that the returned file would be played if playback were started
        /// at that time.
        /// </summary>
        IFileInfo CurrentlyPlaying { get; }

        /// <summary>
        /// Gets and sets the position of the currently ongoing playback in the currently
        /// playing  file in milliseconds.
        /// </summary>
        Int32 PlayerPosition { get; set; }

        /// <summary>
        /// Gets and sets the current player state. May return any member of the PlayState
        /// enum, however when writing, only Paused, Stopped and Playing are considered
        /// possible values.
        /// </summary>
        PlayState PlayerState { get; set; }

        /// <summary>
        /// Schedules playback to stop after the current file has finished playback.
        /// </summary>
        void StopAfterCurrent();

        /// <summary>
        /// Gets and sets the repeat mode state.
        /// </summary>
        RepeatMode Repeat { get; set; }

        /// <summary>
        /// Gets and sets the shuffle mode state.
        /// </summary>
        Boolean Shuffle { get; set; }

        /// <summary>
        /// Jumps to the next track in the NowPlaying list and starts playback, if it
        /// is not already started. Note: this call will cause the PlayStateChanged event
        /// to be raised twice, followed by TrackChanged.
        /// </summary>
        void PlayNextTrack();

        /// <summary>
        /// Jumps to the previous track in the NowPlaying list and starts playback, if
        /// if is not already started. Note: this call will cause the PlayStateChanged
        /// event to be raised twice, followed by TrackChanged.
        /// </summary>
        void PlayPreviousTrack();

        /// <summary>
        /// Enables or disables the Auto-Dj. It is currently not possible to get the
        /// state of the Auto-Dj apart from listening to the corresponding events
        /// AutoDjStopped and AutoDjStarted.
        /// </summary>
        Boolean AutoDj { set; }

        /// <summary>
        /// Gets and sets the current volume in percent, i.e. the valid range of the
        /// returned value is 0.0 to 100.0.
        /// </summary>
        Single Volume { get; set; }

        /// <summary>
        /// Gets and sets the mute state.
        /// </summary>
        Boolean Mute { get; set; }

        /// <summary>
        /// Gets and sets whether the equaliser is enabled.
        /// </summary>
        Boolean EqualiserEnabled { get; set; }

        /// <summary>
        /// Gets and sets whether DSP is enabled.
        /// </summary>
        Boolean DspEnabled { get; set; }

        /// <summary>
        /// Gets and sets whether scrobbling is enabled.
        /// </summary>
        Boolean ScrobbleEnabled { get; set; }

        #endregion
    }

    /// <summary>
    /// Wraps MusicBee's NowPlaying list as a standard collection. Note that not all
    /// properties and methods defined by the inherited interfaces need actually be
    /// implemented and may throw NotImplementedException exceptions.
    /// </summary>
    public interface INowPlayingList : IEnumerable<IFileInfo>, IList<IFileInfo>
    {
        /* empty, just inherits */
    }

    /// <summary>
    /// Delegate type used by callback functions receiving MusicBee notifications.
    /// </summary>
    public delegate void PluginEventHandler<T>(T eventArgs) where T : PluginEventArgs;

    public interface PluginEventArgs
    {
        /// <summary>
        /// Instance of the API that raised the event.
        /// </summary>
        IMusicBeeAPI API { get; }
    }

    /// <summary>
    /// Event argument type delivered to event handlers listening for MusicBee
    /// notifications.
    /// </summary>
    public interface NotificationEventArgs : PluginEventArgs
    {
        /// <summary>
        /// Type of the MusicBee notification that triggered the event.
        /// </summary>
        NotificationType EventType { get; }

        /// <summary>
        /// File info wrapper representing the file related to the event. This property
        /// may be null as not all events are related to files.
        /// </summary>
        IFileInfo RelatedFile { get; }

        /// <summary>
        /// The related file path as indicated by MusicBee. Aliases the Uri.OriginalFile
        /// property in IFileInfo.
        /// </summary>
        String RelatedFilePath { get; }
    }

    public interface PluginClosedEventArgs : PluginEventArgs
    {
        /// <summary>
        /// Gets the reason why the plugin is being closed.
        /// </summary>
        PluginCloseReason Reason { get; }
    }

}
