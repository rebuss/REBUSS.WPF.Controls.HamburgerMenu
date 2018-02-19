using System;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public delegate void MenuOpenedEventHandler(object sender, EventArgs args);

    public delegate void PreviewMenuOpenedEventHandler(object sender, EventArgs args);

    public delegate void MenuClosedEventHandler(object sender, EventArgs args);

    public delegate void PreviewMenuClosedEventHandler(object sender, EventArgs args);
}