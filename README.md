# REBUSS.WPF.Controls.HamburgerMenu
Hamburger menu control for WPF desktop apps. The project is licensed under the terms of the MIT license. It is fully customizable and supports keyboard and MVVM pattern.
<p><img src="http://moj.hajs.hostingasp.pl/REBUSS_HamburgerMenu.gif"></p>

To add the menu to your solution you need 2 things:
1. Add REBUSS.WPF.Controls.HamburgerMenu reference
2. Add Brushes.xaml to you Application.Resources like below:

```xaml
<Application.Resources>
        <ResourceDictionary Source="pack://application:,,,/REBUSS.WPF.Controls.HamburgerMenu;component/Resources/Brushes.xaml"/>
</Application.Resources>
```
If you need to customize colors of the menu, then modify Brushes.xaml or add to your app different ResourceDictionary that contains brushes with the same names as the Brushes.xaml dictionary in HamburgerMenu project.

There are 2 ways to define the menu in XAML. First approach is to bind ItemsSource collection that could be any IEnumerable object. To construct a correct menu you need to define also Feeds. It's a property in HamburgerMenu class. It tells the control what's a label, tooltip, command and icon for the particular object on the list. As a key please use the same value as the value from the list in ViewModel.
```xaml
<hamburgerMenu:HamburgerMenu HorizontalAlignment="Left" IsOpen="True"
                                     OpenPaneWidth="200"
                                     ItemsSource="{Binding MenuItems}"
                                     BackgroundContent="{StaticResource GlassEffect}">
            <hamburgerMenu:HamburgerMenu.Feeds>
                <hamburgerMenu:FeedCollection>
                    <hamburgerMenu:ItemFeed IconContent="&#xE14C;" 
                                        Label="List" 
                                        Tooltip="List view" 
                                        Key="{x:Static demoHamburgerMenu:Consts.List}"/>
                    <hamburgerMenu:ItemFeed IconContent="&#xE104;" 
                                        Label="Edit" 
                                        Tooltip="Edit document" 
                                        Key="{x:Static demoHamburgerMenu:Consts.Edit}"/>
                </hamburgerMenu:FeedCollection>
            </hamburgerMenu:HamburgerMenu.Feeds>
        </hamburgerMenu:HamburgerMenu>
```

The second approach is to hardcode all HamburgerMenuItems in XAML. It supports MVVM as well because you can bind any Command to the HamburgerMenuItem or bind anything to its properties.
```xaml
<hamburgerMenu:HamburgerMenu HorizontalAlignment="Left" 
                                        IsOpen="True"
                                        OpenPaneWidth="200"
                                        CompactPaneWidth="40"
                                        Header="Header"
                                        BarBrush="Red"
                                        CompactMenuTooltip="Collapse"
                                        OpenMenuTooltip="Open">
            <hamburgerMenu:HamburgerMenuItem Text="List"
                                        Content="&#xE14C;"
                                        ToolTip="List View"/>
            <hamburgerMenu:HamburgerMenuItem Content="&#xE104;"
                                        Text="Edit"
                                        ToolTip="Edit Document"/>
        </hamburgerMenu:HamburgerMenu>
```

There are several properties that you can modify:

        BackgroundContent, typeof(object) - it allows you to add some fancy effect/path/image over the background

        BarBrush, typeof(Brush) - it's a color of a small bar indicating if an item is selected
        
        CompactMenuTooltip, typeof(string)

        CompactPaneWidth, typeof(double) - width of minimized menu

        Feeds, typeof(FeedCollection)

        Header, typeof(object) - an object that will be displayed as a header
        
        OpenMenuTooltip, typeof(string)

        OpenPaneWidth, typeof(double) - width of maximized menu

        IsOpen, typeof(bool)

        SwitchButtonContent, typeof(object) - an object that is shown as an icon of the open/close button
