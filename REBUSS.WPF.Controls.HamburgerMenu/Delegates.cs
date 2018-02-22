using System;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public delegate void MenuCompactedEventHandler(object sender, EventArgs args);

    public delegate void MenuOpenedEventHandler(object sender, EventArgs args);

    public delegate void PreviewMenuCompactedEventHandler(object sender, EventArgs args);

    public delegate void PreviewMenuOpenedEventHandler(object sender, EventArgs args);
}