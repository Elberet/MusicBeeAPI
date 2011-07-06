using System;
using System.Windows.Forms;
using MusicBeePlugin;

/*
 * This is an extensively commented minimal example plugin that only works
 * inside the debugger (because it uses the debugger for its output).
 * 
 * It should give you a general idea how things work; from there, look at the
 * documentation in the API interfaces (MusicBeePlugin.IMusicBeeAPI and so on)
 * or just "Use the source, Luke".
 */

namespace PluginTemplate
{
    /*
     * This attribute is used to tell the MusicBeeAPI wrapper what type of
     * plugin this class implements and which application it targets.
     * 
     * Examples:
     *   [PluginFrontend(PluginType=PluginType.ArtworkRetrieval, TargetApplication="Foo")]
     */
    [PluginFrontend]
    /*
     * IPluginFrontend is required for all plugins. Artwork and lyrics plugins
     * must also implement IArtworkProvider and ILyricProvider, respectively.
     * 
     * If you don't implement a required interface, the Plugin.Initialize code
     * will throw an exception before even instantiating this frontend class.
     */
    class PluginTemplate : IPluginFrontend
    {
        /*
         * Called during the startup sequence. api is your ticket to MusicBee,
         * so you may want to store it in an instance variable for later use.
         * 
         * You set configPanelHeight to the height in pixels your configuration
         * UI will consume in the MusicBee preferences dialog.
         */
        void IPluginFrontend.Initialize(IMusicBeeAPI api, out int configPanelHeight) {
            configPanelHeight = 0;

            /*
             * This attaches an event handler to a MusicBee event. What the
             * events mean and when they are triggered should be documented in
             * the IMusicBeeAPI interface. (Warning: work in progress!)
             */
            api.PluginStartup += Start;

            /*
             * Note that the plugin is technically not running at this point,
             * so you should not yet interact with MusicBee. Registering event
             * handlers, especially the PluginStartup handler, is done inside
             * the wrapper, so that's safe.
             * 
             * Typically, you will want to use Initialize to check your
             * environment, load libraries you depend on, set up the .NET
             * environment, etc. If you encounter an error, just skip
             * registering your PluginStartup handler; throwing exceptions from
             * this method is not a good idea. MusicBee will handle plugin
             * exceptions rather then die, but the messages it shows aren't the
             * most helpful...
             */
        }

        private void Start(NotificationEventArgs args) {
            /*
             * PluginStartup will only be called once per plugin lifecycle and
             * unlike Initialize, it will be called when MusicBee is all done
             * initializing the plugin, not somewhere in the middle.
             * 
             * This is the ideal place to actually start your plugin magic.
             */
            args.API.TrackChanged += TrackChanged;
        }

        private void TrackChanged(NotificationEventArgs args) {
            /*
             * args contains three interesting fields:
             * - API, a reference to the API wrapper. If you do all your work
             *   within event handlers, you won't need to store the api
             *   reference in Initialize.
             * - EventType, in case you want one handler to handle multiple
             *   difference events.
             * - RelatedFile, basically a wrapper around a string that gives
             *   you a nice way of fetching properties and tags for a file from
             *   MusicBee. This might be null for some event types, tho!
             */
            System.Diagnostics.Debug.WriteLine(String.Format("now playing: {0} - {1}",
                args.RelatedFile.Artist, args.RelatedFile.TrackTitle));
        }

        /*
         * If the configPanelHeight set in Initialize is non-zero, this gets
         * an actual Panel which it needs to populate with a user interface. If
         * you set the height to zero, MusicBee shows a default button and
         * calls PluginConfigure when it's clicked, tho configPanel will be
         * null. If this then returns false, MusicBee shows a default
         * about-type dialog; if you returned true, it assumes that you handled
         * configuration yourself in some way, such as showing a custom modal
         * dialog.
         */
        bool IPluginFrontend.PluginConfigure(Panel configPanel) {
            return false;
        }

        /*
         * The last method ever called during your plugin's lifecycle. Free
         * all non-managed resources, close all files, handles, sockets,
         * streams, forms, whatever.
         */
        void IDisposable.Dispose() { }
    }
}
