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
using System.IO;
using System.Drawing;

namespace MusicBeePlugin
{
    partial class Plugin
    {
        partial class MusicBeeAPI
        {
            private class FileInfo : IFileInfo
            {
                private MusicBeeApiInterface mb;
                private Uri uri;

                public FileInfo(MusicBeeApiInterface api, string uri) {
                    this.mb = api;
                    this.uri = new Uri(uri);
                }

                #region URI

                public Uri Uri { get { return uri; } }

                public override string ToString() {
                    return uri.IsFile ? uri.LocalPath : base.ToString();
                }

                public override bool Equals(object comparand) {
                    if (!(comparand is IFileInfo)) return false;
                    return uri.Equals((comparand as IFileInfo).Uri);
                }

                public override int GetHashCode() {
                    return uri.GetHashCode();
                }

                #endregion

                // ........................................................................

                #region Properties

                public string Bitrate { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Bitrate); } }
                public string Channels { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Channels); } }
                public string DateAdded { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.DateAdded); } }
                public string DateModified { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.DateModified); } }
                public string Duration { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Duration); } }
                public string Format { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Format); } }
                public string Kind { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Kind); } }
                public string LastPlayed { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.LastPlayed); } }
                public string SampleRate { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.SampleRate); } }
                public string Size { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Size); } }
                public string SkipCount { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.SkipCount); } }
                public string PlayCount { get { return mb.Library_GetFileProperty(uri.OriginalString, FilePropertyType.PlayCount); } }

                public string this[FilePropertyType property] {
                    get { return mb.Library_GetFileProperty(uri.OriginalString, property); }
                }

                #endregion

                // ........................................................................

                #region Tags

                // Tags
                public string Album {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Album); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Album, value); }
                }
                public string AlbumArtist {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.AlbumArtist); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.AlbumArtist, value); }
                }
                public string AlbumArtistRaw {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.AlbumArtistRaw); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.AlbumArtistRaw, value); }
                }
                public string Artist {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Artist); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Artist, value); }
                }
                public string Artwork {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Artwork); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Artwork, value); }
                }
                public string BeatsPerMin {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.BeatsPerMin); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.BeatsPerMin, value); }
                }
                public string Comment {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Comment); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Comment, value); }
                }
                public string Composer {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Composer); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Composer, value); }
                }
                public string Conductor {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Conductor); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Conductor, value); }
                }
                public string Custom1 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom1); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom1, value); }
                }
                public string Custom2 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom2); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom2, value); }
                }
                public string Custom3 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom3); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom3, value); }
                }
                public string Custom4 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom4); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom4, value); }
                }
                public string Custom5 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom5); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom5, value); }
                }
                public string Custom6 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom6); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom6, value); }
                }
                public string Custom7 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom7); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom7, value); }
                }
                public string Custom8 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom8); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom8, value); }
                }
                public string Custom9 {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom9); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom9, value); }
                }
                public string DiscCount {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.DiscCount); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.DiscCount, value); }
                }
                public string DiscNo {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.DiscNo); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.DiscNo, value); }
                }
                public string Encoder {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Encoder); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Encoder, value); }
                }
                public string Genre {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Genre); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Genre, value); }
                }
                public string GenreCategory {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.GenreCategory); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.GenreCategory, value); }
                }
                public string Grouping {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Grouping); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Grouping, value); }
                }
                public string Keywords {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Keywords); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Keywords, value); }
                }
                public string Lyricist {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Lyricist); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Lyricist, value); }
                }
                public string Mood {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Mood); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Mood, value); }
                }
                public string Occasion {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Occasion); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Occasion, value); }
                }
                public string Origin {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Origin); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Origin, value); }
                }
                public string Publisher {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Publisher); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Publisher, value); }
                }
                public string Quality {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Quality); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Quality, value); }
                }
                public string Rating {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Rating); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Rating, value); }
                }
                public string RatingAlbum {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.RatingAlbum); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.RatingAlbum, value); }
                }
                public string RatingLove {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.RatingLove); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.RatingLove, value); }
                }
                public string Tempo {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Tempo); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Tempo, value); }
                }
                public string TrackCount {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.TrackCount); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.TrackCount, value); }
                }
                public string TrackNo {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.TrackNo); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.TrackNo, value); }
                }
                public string TrackTitle {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.TrackTitle); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.TrackTitle, value); }
                }
                public string Year {
                    get { return mb.Library_GetFileTag(uri.OriginalString, MetaDataType.Year); }
                    set { mb.Library_SetFileTag(uri.OriginalString, MetaDataType.Year, value); }
                }

                public string this[MetaDataType tag] {
                    get { return mb.Library_GetFileTag(uri.OriginalString, tag); }
                    set { mb.Library_SetFileTag(uri.OriginalString, tag, value); }
                }

                public void CommitTags() {
                    mb.Library_CommitTagsToFile(uri.OriginalString);
                }

                public Image GetArtwork() { return GetArtwork(0); }

                public Image GetArtwork(int index) {
                    string raw = mb.Library_GetArtwork(uri.OriginalString, index);
                    byte[] byteData = Convert.FromBase64String(raw);
                    MemoryStream s = new MemoryStream(byteData, false);
                    return Image.FromStream(s);
                    // stream stays open on purpose, required by Image
                }

                public string GetLyrics(LyricsType lyricsType) {
                    return mb.Library_GetLyrics(uri.OriginalString, (int)lyricsType);
                }

                #endregion
            }
        }
    }
}
