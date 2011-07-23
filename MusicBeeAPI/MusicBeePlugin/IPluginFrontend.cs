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

namespace MusicBeePlugin
{
    /// <summary>
    /// Defines the interface implemented by clients to the extended MusicBee Plugin API.
    /// </summary>
    public interface IPluginFrontend
    {
        /// <summary>
        /// Initializes the plugin frontend. The frontend must return the intended height of its
        /// configuration user interface panel and return it via configPanelHeight.
        /// </summary>
        void Initialize(IMusicBeeAPI api, out int configPanelHeight);

        /// <summary>
        /// Provides the plugin's configuration UI. If <c>ConfigPanelHeight</c> is nonzero,
        /// <paramref name="configPanel"/> is a blank instance of <c>Panel</c> and should be
        /// popuplated with any UI elements required.
        /// </summary>
        /// <param name="configPanel"></param>
        /// <returns>If the plugin handles configuration, return <c>true</c>, else return
        /// <c>false</c> (MusicBee will display a default About dialog).</returns>
        bool PluginConfigure(Panel configPanel);
    }

    public interface ILyricProvider : IPluginFrontend
    {
        /// <summary>
        /// Gets a list of lyrics providers implemented by the plugin.
        /// </summary>
        string[] GetProviders();

        /// <summary>
        /// Retrieves lyrics for the specified song from a provider.
        /// </summary>
        string RetrieveLyrics(string sourceFileUrl, string artist, string trackTitle,
            string album, bool synchronisedPreferred, string provider);
    }

    public interface IArtworkProvider : IPluginFrontend
    {
        /// <summary>
        /// Gets a list of artwork providers implemented by the plugin.
        /// </summary>
        string[] GetProviders();

        /// <summary>
        /// Retrieves artwork for the specified album from a provider.
        /// </summary>
        /// <returns>A base-64 encoded string representation of the binary image data.</returns>
        string RetrieveArtwork(string sourceFileUrl, string albumArtist, string album,
            string provider);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PluginFrontendAttribute : Attribute {
        public string TargetApplication { get; set; }
        public PluginType PluginType { get; set; }
        public bool TagEvents { get; set; }
        public PluginFrontendAttribute() {
            TargetApplication = "";
            PluginType = PluginType.General;
            TagEvents = false;
        }
    }
}
