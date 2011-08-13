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
using System.Drawing;
using System.Globalization;
using System.IO;

namespace MusicBeePlugin
{
    partial class Plugin
    {
        partial class MusicBeeAPI
        {
            private partial class FileInfo : IFileInfo
            {
                private MusicBeeAPI api;
                private MusicBeeApiInterface MB { get { return api.mbApiInterface; } }
                private Uri uri;

                public FileInfo(MusicBeeAPI api, string uri) {
                    this.api = api;
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

                public string Bitrate { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Bitrate); } }
                public string Channels { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Channels); } }
                public string DateAdded { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.DateAdded); } }
                public string DateModified { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.DateModified); } }
                public string Duration { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Duration); } }
                public string Format { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Format); } }
                public string Kind { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Kind); } }
                public string LastPlayed { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.LastPlayed); } }
                public string SampleRate { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.SampleRate); } }
                public string Size { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.Size); } }
                public string SkipCount { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.SkipCount); } }
                public string PlayCount { get { return MB.Library_GetFileProperty(uri.OriginalString, FilePropertyType.PlayCount); } }

                public string this[FilePropertyType property] {
                    get { return MB.Library_GetFileProperty(uri.OriginalString, property); }
                }

                #endregion

                // ........................................................................

                #region Tags

                // Multi Tags
                private readonly object syncRoot = new object();
                private MultiTagList multiArtists;
                private MultiTagList multiComposers;

                public IList<string> Artists {
                    get { return getSingleton(MetaDataType.MultiArtist, ref multiArtists); }
                }

                public IList<string> Composers {
                    get { return getSingleton(MetaDataType.MultiComposer, ref multiComposers); }
                }

                private MultiTagList getSingleton(MetaDataType type, ref MultiTagList list) {
                    if (list == null) {
                        lock (syncRoot) {
                            if (list == null) {
                                list = new MultiTagList(this, type);
                            }
                        }
                    }
                    return list;
                }

                // Tags
                public string Album {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Album); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Album, value); }
                }
                public string AlbumArtist {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.AlbumArtist); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.AlbumArtist, value); }
                }
                public string AlbumArtistRaw {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.AlbumArtistRaw); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.AlbumArtistRaw, value); }
                }
                public string Artist {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Artist); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Artist, value); }
                }
                public string Artwork {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Artwork); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Artwork, value); }
                }
                public string BeatsPerMin {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.BeatsPerMin); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.BeatsPerMin, value); }
                }
                public string Comment {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Comment); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Comment, value); }
                }
                public string Composer {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Composer); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Composer, value); }
                }
                public string Conductor {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Conductor); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Conductor, value); }
                }
                public string Custom1 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom1); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom1, value); }
                }
                public string Custom2 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom2); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom2, value); }
                }
                public string Custom3 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom3); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom3, value); }
                }
                public string Custom4 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom4); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom4, value); }
                }
                public string Custom5 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom5); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom5, value); }
                }
                public string Custom6 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom6); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom6, value); }
                }
                public string Custom7 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom7); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom7, value); }
                }
                public string Custom8 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom8); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom8, value); }
                }
                public string Custom9 {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Custom9); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Custom9, value); }
                }
                public string DiscCount {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.DiscCount); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.DiscCount, value); }
                }
                public string DiscNo {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.DiscNo); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.DiscNo, value); }
                }
                public string Encoder {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Encoder); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Encoder, value); }
                }
                public string Genre {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Genre); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Genre, value); }
                }
                public string GenreCategory {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.GenreCategory); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.GenreCategory, value); }
                }
                public string Grouping {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Grouping); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Grouping, value); }
                }
                public string Keywords {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Keywords); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Keywords, value); }
                }
                public string Lyricist {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Lyricist); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Lyricist, value); }
                }
                public string Mood {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Mood); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Mood, value); }
                }
                public string Occasion {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Occasion); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Occasion, value); }
                }
                public string Origin {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Origin); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Origin, value); }
                }
                public string Publisher {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Publisher); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Publisher, value); }
                }
                public string Quality {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Quality); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Quality, value); }
                }
                public float? Rating {
                    get { return FromRating(MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Rating)); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Rating, ToRating(value)); }
                }
                public float? RatingAlbum {
                    get { return FromRating(MB.Library_GetFileTag(uri.OriginalString, MetaDataType.RatingAlbum)); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.RatingAlbum, ToRating(value)); }
                }
                public string RatingLove {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.RatingLove); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.RatingLove, value); }
                }
                public string Tempo {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Tempo); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Tempo, value); }
                }
                public string TrackCount {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.TrackCount); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.TrackCount, value); }
                }
                public string TrackNo {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.TrackNo); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.TrackNo, value); }
                }
                public string TrackTitle {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.TrackTitle); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.TrackTitle, value); }
                }
                public string Year {
                    get { return MB.Library_GetFileTag(uri.OriginalString, MetaDataType.Year); }
                    set { MB.Library_SetFileTag(uri.OriginalString, MetaDataType.Year, value); }
                }

                public string this[MetaDataType tag] {
                    get { return MB.Library_GetFileTag(uri.OriginalString, tag); }
                    set { MB.Library_SetFileTag(uri.OriginalString, tag, value); }
                }

                public bool CommitTags() {
                    try {
                        if (multiArtists != null) multiArtists.Commit();
                        if (multiComposers != null) multiComposers.Commit();
                    } catch (InvalidOperationException) {
                        return false;
                    }
                    bool res = MB.Library_CommitTagsToFile(uri.OriginalString);
                    MB.MB_RefreshPanels();
                    return res;
                }

                public Image GetArtwork() { return GetArtwork(0); }

                public Image GetArtwork(int index) {
                    string raw = MB.Library_GetArtwork(uri.OriginalString, index);
                    byte[] byteData = Convert.FromBase64String(raw);
                    MemoryStream s = new MemoryStream(byteData, false);
                    return Image.FromStream(s);
                    // stream stays open on purpose, required by Image
                }

                public string GetLyrics(LyricsType lyricsType) {
                    return MB.Library_GetLyrics(uri.OriginalString, (int)lyricsType);
                }

                // Helpers

                private float? FromRating(string rating) {
                    return rating.Equals("") ? (float?) null : float.Parse(rating);
                }

                private string ToRating(float? rating) {
                    return rating.HasValue ? Math.Round((double) rating.Value, 1).ToString(CultureInfo.InvariantCulture.NumberFormat) : "";
                }

                #endregion
            }
        }
    }
}
