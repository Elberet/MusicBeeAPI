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
    /// Skinning-related settings and properties of MusicBee.
    /// </summary>
    public interface ISkinInfo
    {
        /// <summary>
        /// Gets the name of the currently selected MusicBee skin.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets whether the MusicBee main window's window borders are skinned
        /// or drawn in the Windows default style.
        /// </summary>
        Boolean IsWindowBordersSkinned { get; }

        /// <summary>
        /// Gets detailed skinning information for input controls.
        /// </summary>
        ISkinElementInfo InputControl { get; }

        /// <summary>
        /// Gets detailed skinning information for panels.
        /// </summary>
        ISkinElementInfo Panel { get; }

        /// <summary>
        /// Gets detailed skinning information for panel labels.
        /// </summary>
        ISkinElementInfo PanelLabel { get; }
    }

    public interface ISkinElementInfo
    {
        /// <summary>
        /// Gets the Color used for the element's background.
        /// </summary>
        Color Background { get; }

        /// <summary>
        /// Gets the Color used for the element's background when the element
        /// has been modified.
        /// </summary>
        Color BackgroundModified { get; }

        /// <summary>
        /// Gets the Color used for the element's foreground.
        /// </summary>
        Color Foreground { get; }

        /// <summary>
        /// Gets the Color used for the element's foreground when the element
        /// has been modified.
        /// </summary>
        Color ForegroundModified { get; }

        /// <summary>
        /// Gets the Color used for the element's border.
        /// </summary>
        Color Border { get; }

        /// <summary>
        /// Gets the Color used for the element's border when the element has
        /// been modified.
        /// </summary>
        Color BorderModified { get; }
    }
}