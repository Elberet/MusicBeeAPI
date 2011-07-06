using System;
using System.Windows.Forms;
using MusicBeePlugin;

namespace PluginTemplate
{
    [PluginFrontend]
    class PluginTemplate : IPluginFrontend
    {
        void IPluginFrontend.Initialize(IMusicBeeAPI api, out int configPanelHeight) {
            configPanelHeight = 0;
        }

        bool IPluginFrontend.PluginConfigure(Panel configPanel) {
            return false;
        }

        void IDisposable.Dispose() { }
    }
}
