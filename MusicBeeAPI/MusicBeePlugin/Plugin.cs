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
using System.Reflection;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeAPI apiWrapper;
        private PluginInfo pluginInfo;
        private IPluginFrontend frontend;

        private volatile bool pluginClosed = false;
        private readonly object syncRoot = new object();

        private const ReceiveNotificationFlags AllNotifications =
            ReceiveNotificationFlags.DataStreamEvents |
            ReceiveNotificationFlags.PlayerEvents |
            ReceiveNotificationFlags.StartupOnly;

        [ThreadStatic]
        private static Plugin currentInstance;

        public static IMusicBeeAPI API { get { return currentInstance.apiWrapper; } }

        /*
         * MusicBee entrypoint -- initialise the plugin
         */
        public PluginInfo Initialise(IntPtr apiInterfacePtr) {
            currentInstance = this;
            pluginClosed = false;
            apiWrapper = new MusicBeeAPI(apiInterfacePtr);
            pluginInfo = new PluginInfo();

            frontend = FindFrontend(out pluginInfo.ConfigurationPanelHeight);

            pluginInfo.PluginInfoVersion = PluginInfoVersion;
            pluginInfo.Name = GetAssemblyAttribute<AssemblyTitleAttribute>().Title;
            pluginInfo.Description = GetAssemblyAttribute<AssemblyDescriptionAttribute>().Description;
            pluginInfo.Author = GetAssemblyAttribute<AssemblyCompanyAttribute>().Company;
            string[] numbers = GetAssemblyAttribute<AssemblyFileVersionAttribute>().Version.Split('.');
            pluginInfo.VersionMajor = ExtractVersion(numbers, 0);
            pluginInfo.VersionMinor = ExtractVersion(numbers, 1);
            pluginInfo.Revision = ExtractVersion(numbers, 2);
            pluginInfo.MinInterfaceVersion = MinInterfaceVersion;
            pluginInfo.MinApiRevision = MinApiRevision;

            pluginInfo.ReceiveNotifications = AllNotifications;
            pluginInfo.TargetApplication = GetFrontendAttribute().TargetApplication;
            pluginInfo.Type = GetFrontendAttribute().PluginType;
            VerifyFrontend(pluginInfo);

            return pluginInfo;
        }

        private short ExtractVersion(string[] numbers, int pos) {
            short v;
            if (numbers.Length <= pos) return 0;
            if (!short.TryParse(numbers[pos], out v)) return 0;
            return v;
        }

        private void VerifyFrontend(PluginInfo info) {
            switch (info.Type) {
                case PluginType.ArtworkRetrieval:
                    if (!(frontend is IArtworkProvider)) throw new ApplicationException("Artwork retrieval plugins must provide a IArtworkProvider frontend.");
                    break;
                case PluginType.LyricsRetrieval:
                    if (!(frontend is ILyricProvider)) throw new ApplicationException("Lyrics retrieval plugins must provide a ILyricsProvider frontend.");
                    break;
                case PluginType.InstantMessenger:
                    if (info.TargetApplication == null || info.TargetApplication.Equals(""))
                        throw new ApplicationException("Instant messenger plugin must specify target application.");
                    break;
                case PluginType.General:
                    return; // always ok.
                default:
                    throw new ApplicationException(String.Format("Plugin type {0} is not supported.", info.Type.ToString()));
            }
        }

        private IPluginFrontend FindFrontend(out int configPanelHeight) {
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type type in asm.GetTypes()) {
                object[] attrs = type.GetCustomAttributes(typeof(PluginFrontendAttribute), false);
                if (attrs.Length == 1 && typeof(IPluginFrontend).IsAssignableFrom(type)) {
                    ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
                    if (ctor != null) {
                        IPluginFrontend f = ctor.Invoke(null) as IPluginFrontend;
                        f.Initialize(apiWrapper, out configPanelHeight);
                        return f;
                    }
                }
            }
            throw new ApplicationException("No plugin frontend found or missing default constructor.");
        }

        private T GetAssemblyAttribute<T>() where T : class {
            T[] attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(T), false) as T[];
            if (attrs.Length != 1) throw new ApplicationException("Missing attribute " + typeof(T).Name);
            return attrs[0];
        }

        private PluginFrontendAttribute GetFrontendAttribute() {
            return frontend.GetType().GetCustomAttributes(typeof(PluginFrontendAttribute), false)[0] as PluginFrontendAttribute;
        }

        /*
         * MusicBee entrypoint -- get configuration UI
         */
        public bool Configure(IntPtr panelHandle) {
            if (pluginClosed) return false;
            currentInstance = this;
            return frontend.PluginConfigure(panelHandle == IntPtr.Zero ? null :
                System.Windows.Forms.Control.FromHandle(panelHandle) as System.Windows.Forms.Panel);
        }

        /*
         * MusicBee entrypoint -- save configuration data
         */
        public void SaveSettings() {
            lock (syncRoot) {
                if (!pluginClosed) {
                    currentInstance = this;
                    apiWrapper.RaiseSettingsSaved();
                }
            }
        }

        /*
         * MusicBee entrypoint -- signal plugin shutdown
         */
        public void Close(PluginCloseReason reason) {
            lock (syncRoot) {
                if (!pluginClosed) {
                    currentInstance = this;
                    pluginClosed = true;
                    apiWrapper.RaisePluginClosed(reason);
                    frontend.Dispose();
                }
            }
        }

        /*
         * MusicBee entrypoint -- signal plugin uninstall
         */
        public void Uninstall() {
            lock (syncRoot) {
                if (!pluginClosed) {
                    currentInstance = this;
                    apiWrapper.RaisePluginUninstalled();
                }
            }
        }

        /*
         * MusicBee entrypoint -- process notifications
         */
        public void ReceiveNotification(string sourceFileUrl, NotificationType type) {
            lock (syncRoot) {
                if (!pluginClosed) {
                    currentInstance = this;
                    apiWrapper.ProcessNotification(sourceFileUrl, type);
                }
            }
        }

        /*
         * MusicBee entrypoint -- get provider names
         */
        public string[] GetProviders() {
            lock (syncRoot) {
                if (!pluginClosed) {
                    currentInstance = this;
                    if (frontend is ILyricProvider) {
                        return ((ILyricProvider) frontend).GetProviders();
                    } else if (frontend is IArtworkProvider) {
                        return ((IArtworkProvider) frontend).GetProviders();
                    }
                }
                return null;
            }
        }

        /*
         * MusicBee entrypoint -- retrieve lyrics for song
         */
        public string RetrieveLyrics(string sourceFileUrl, string artist, string trackTitle, string album, bool synchronisedPreferred, string provider) {
            lock (syncRoot) {
                if (!pluginClosed) {
                    currentInstance = this;
                    return ((ILyricProvider) frontend).RetrieveLyrics(sourceFileUrl, artist, trackTitle, album, synchronisedPreferred, provider);
                }
                return null;
            }
        }

        /*
         * MusicBee entrypoint -- retrieve album artwork
         */
        public string RetrieveArtwork(string sourceFileUrl, string albumArtist, string album, string provider) {
            lock (syncRoot) {
                if (!pluginClosed) {
                    currentInstance = this;
                    return ((IArtworkProvider) frontend).RetrieveArtwork(sourceFileUrl, albumArtist, album, provider);
                }
                return null;
            }
        }

        // TODO: support storage plugins
    }
}