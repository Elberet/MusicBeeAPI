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
    partial class Plugin
    {
        partial class MusicBeeAPI
        {
            private class SkinInfo : ISkinInfo
            {
                private MusicBeeApiInterface mb;

                internal SkinInfo(MusicBeeApiInterface mbIntf) {
                    mb = mbIntf;
                    skinName = mb.Setting_GetSkin();
                    isWindowBordersSkinned = mb.Setting_IsWindowBordersSkinned();
                    control = new Element(mb, SkinElement.SkinInputControl);
                    panel = new Element(mb, SkinElement.SkinInputPanel);
                    panelLabel = new Element(mb, SkinElement.SkinInputPanelLabel);
                }

                private struct Element : ISkinElementInfo
                {
                    private Color bg, bgm, fg, fgm, b, bm;

                    internal Element(MusicBeeApiInterface mb, SkinElement e) {
                        bg = GetColor(mb, e, ElementComponent.ComponentBackground, ElementState.ElementStateDefault);
                        fg = GetColor(mb, e, ElementComponent.ComponentForeground, ElementState.ElementStateDefault);
                        b = GetColor(mb, e, ElementComponent.ComponentBorder, ElementState.ElementStateDefault);
                        bgm = GetColor(mb, e, ElementComponent.ComponentBackground, ElementState.ElementStateModified);
                        fgm = GetColor(mb, e, ElementComponent.ComponentForeground, ElementState.ElementStateModified);
                        bm = GetColor(mb, e, ElementComponent.ComponentBorder, ElementState.ElementStateModified);
                    }

                    private static Color GetColor(MusicBeeApiInterface mb, SkinElement e, ElementComponent cmp, ElementState st) {
                        return Color.FromArgb(mb.Setting_GetSkinElementColour(e, st, cmp));
                    }

                    public Color Background { get { return bg; } }
                    public Color Foreground { get { return fg; } }
                    public Color Border { get { return b; } }
                    public Color BackgroundModified { get { return bgm; } }
                    public Color ForegroundModified { get { return fgm; } }
                    public Color BorderModified { get { return bm; } }
                }

                private String skinName;
                public String Name { get { return skinName; } }

                private bool isWindowBordersSkinned;
                public Boolean IsWindowBordersSkinned { get { return isWindowBordersSkinned; } }

                private Element control, panel, panelLabel;
                public ISkinElementInfo InputControl { get { return control; } }
                public ISkinElementInfo Panel { get { return panel; } }
                public ISkinElementInfo PanelLabel { get { return panelLabel; } }
            }
        }
    }
}
