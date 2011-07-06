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
using System.Drawing;
namespace MusicBeePlugin
{
    /// <summary>
    /// Represents a media file and provides access to the represented file's
    /// properties and tags in the MusicBee library.
    /// </summary>
    public interface IFileInfo
    {
        #region URI and Miscellaneous

        /// <summary>
        /// The represented file's URI.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Store any changed values in the MusicBee library and/or the file's embedded tags.
        /// </summary>
        void CommitTags();

        /// <summary>
        /// Retrieves the main album cover artwork associated with the file.
        /// </summary>
        Image GetArtwork();

        /// <summary>
        /// Retrieves album cover artwork associated with the file.
        /// </summary>
        Image GetArtwork(int index);

        /// <summary>
        /// Retrieves song lyrics associated with the file.
        /// </summary>
        string GetLyrics(LyricsType lyricsType);

        #endregion

        // ................................................................................

        #region Properties

        /// <summary>
        /// Provides access to the file's properties via an indexer. Properties are
        /// read-only in MusicBee.
        /// </summary>
        string this[FilePropertyType property] { get; }
        string Kind { get; }
        string Format { get; }
        string Size { get; }
        string Channels { get; }
        string SampleRate { get; }
        string DateAdded { get; }
        string DateModified { get; }
        string Bitrate { get; }
        string LastPlayed { get; }
        string PlayCount { get; }
        string SkipCount { get; }
        string Duration { get; }

        #endregion

        // ................................................................................

        #region Tags

        /// <summary>
        /// Provides access to the file's tags via an indexer. Tags can be read and
        /// modified. After modifying a file's tags, <see cref="CommitTags"/> should be
        /// called.
        /// </summary>
        string this[MetaDataType tag] { get; set; }
        string Album { get; set; }
        string AlbumArtist { get; set; }
        string Artist { get; set; } // TODO: multi artist
        string Artwork { get; set; }
        string Comment { get; set; }
        string Composer { get; set; } // TODO: multi composer
        string Conductor { get; set; }
        string Custom1 { get; set; }
        string Custom2 { get; set; }
        string Custom3 { get; set; }
        string Custom4 { get; set; }
        string Custom5 { get; set; }
        string Custom6 { get; set; }
        string Custom7 { get; set; }
        string Custom8 { get; set; }
        string Custom9 { get; set; }
        string DiscCount { get; set; }
        string DiscNo { get; set; }
        string Genre { get; set; }
        string GenreCategory { get; set; }
        string Grouping { get; set; }
        string Keywords { get; set; }
        string Lyricist { get; set; }
        string Mood { get; set; }
        string Occasion { get; set; }
        string Origin { get; set; }
        string Publisher { get; set; }
        string Quality { get; set; }
        string Rating { get; set; }
        string RatingAlbum { get; set; }
        string RatingLove { get; set; }
        string TrackCount { get; set; }
        string TrackNo { get; set; }
        string TrackTitle { get; set; }
        string Year { get; set; }

        #endregion
    }
}
