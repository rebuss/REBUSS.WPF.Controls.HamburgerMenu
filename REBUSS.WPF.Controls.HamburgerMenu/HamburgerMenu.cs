using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public class HamburgerMenu : Selector
    {
        public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register(
            "BackgroundContent", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty BarBrushProperty = DependencyProperty.Register(
            "BarBrush", typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(default(Brush)));
        
        public static readonly DependencyProperty CompactMenuTooltipProperty = DependencyProperty.Register(
            "CompactMenuTooltip", typeof(string), typeof(HamburgerMenu), new PropertyMetadata("Compact"));

        public static readonly DependencyProperty CompactPaneWidthProperty = DependencyProperty.Register(
            "CompactPaneWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(50.0));

        public static readonly DependencyProperty FeedsProperty = DependencyProperty.Register(
            "Feeds", typeof(FeedCollection), typeof(HamburgerMenu), new PropertyMetadata(new FeedCollection()));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(default(object)));
        
        public static readonly DependencyProperty OpenMenuTooltipProperty = DependencyProperty.Register(
            "OpenMenuTooltip", typeof(string), typeof(HamburgerMenu), new PropertyMetadata("Open"));

        public static readonly DependencyProperty OpenPaneWidthProperty = DependencyProperty.Register(
            "OpenPaneWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(default(bool), OnIsOpenChanged));

        public static readonly DependencyProperty SwitchButtonContentProperty = DependencyProperty.Register(
            "SwitchButtonContent", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(default(object)));
        
        public static readonly DependencyProperty TextOpacityProperty = DependencyProperty.Register(
            "TextOpacity", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        public static readonly RoutedEvent MenuCompactedEvent =
            EventManager.RegisterRoutedEvent("MenuCompacted", RoutingStrategy.Bubble,
                typeof(MenuCompactedEventHandler), typeof(HamburgerMenu));

        public static readonly RoutedEvent MenuOpenedEvent =
            EventManager.RegisterRoutedEvent("MenuOpened", RoutingStrategy.Bubble,
                typeof(MenuOpenedEventHandler), typeof(HamburgerMenu));

        private readonly ItemController itemController = new ItemController();

        private Storyboard collapsingStoryboard;

        private Storyboard expandingStoryboard;

        private Grid itemsControl;

        private ToggleButton switchButton;

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu),
                new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
        }

        public HamburgerMenu()
        {
            Loaded += OnLoaded;
            itemController.SelectedItemChanged += OnSelectedItemChanged;
        }

        public object BackgroundContent
        {
            get { return (object)GetValue(BackgroundContentProperty); }
            set { SetValue(BackgroundContentProperty, value); }
        }

        public Brush BarBrush
        {
            get { return (Brush)GetValue(BarBrushProperty); }
            set { SetValue(BarBrushProperty, value); }
        }

        public string CompactMenuTooltip
        {
            get { return (string)GetValue(CompactMenuTooltipProperty); }
            set { SetValue(CompactMenuTooltipProperty, value); }
        }

        public double CompactPaneWidth
        {
            get { return (double)GetValue(CompactPaneWidthProperty); }
            set { SetValue(CompactPaneWidthProperty, value); }
        }

        public FeedCollection Feeds
        {
            get { return (FeedCollection)GetValue(FeedsProperty); }
            set { SetValue(FeedsProperty, value); }
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public string OpenMenuTooltip
        {
            get { return (string)GetValue(OpenMenuTooltipProperty); }
            set { SetValue(OpenMenuTooltipProperty, value); }
        }

        public double OpenPaneWidth
        {
            get { return (double)GetValue(OpenPaneWidthProperty); }
            set { SetValue(OpenPaneWidthProperty, value); }
        }

        public object SwitchButtonContent
        {
            get { return GetValue(SwitchButtonContentProperty); }
            set { SetValue(SwitchButtonContentProperty, value); }
        }

        public double TextOpacity
        {
            get { return (double)GetValue(TextOpacityProperty); }
            set { SetValue(TextOpacityProperty, value); }
        }

        public event MenuCompactedEventHandler MenuCompacted
        {
            add { AddHandler(MenuCompactedEvent, value); }
            remove { RemoveHandler(MenuCompactedEvent, value); }
        }

        public event MenuOpenedEventHandler MenuOpened
        {
            add { AddHandler(MenuOpenedEvent, value); }
            remove { RemoveHandler(MenuOpenedEvent, value); }
        }

        private void CacheHamburgerMenuItems()
        {
            foreach (var item in Items)
            {
                HamburgerMenuItem menuItem = null;
                if (item is HamburgerMenuItem)
                {
                    menuItem = item as HamburgerMenuItem;
                    if ((int)menuItem.IconWidth == 0)
                    {
                        menuItem.IconWidth = CompactPaneWidth;
                    }

                    if (ItemsSource == null || !ItemsSource.GetEnumerator().MoveNext())
                    {
                        var newFeed = new ItemFeed
                        {
                            Command = menuItem.Command,
                            IconContent = menuItem.Content,
                            Tooltip = menuItem.ToolTip,
                            Label = menuItem.Text,
                            Key = menuItem.GetHashCode()
                        };

                        Feeds.Add(newFeed);
                    }
                }
                else
                {
                    menuItem = (HamburgerMenuItem)ItemContainerGenerator.ContainerFromItem(item);
                }

                menuItem.BarBrush = BarBrush;
                itemController.AddItem(menuItem);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new HamburgerMenuItem
            {
                IconWidth = CompactPaneWidth,
                FontSize = FontSize
            };
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = d as HamburgerMenu;
            if (menu?.IsLoaded == true)
            {
                if ((bool)e.NewValue)
                {
                    menu?.StartExpandAnimation();
                }
                else
                {
                    menu?.StartCollapsingAnimation();
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            expandingStoryboard = AnimationProvider.GetExpandingAnimation(this);
            collapsingStoryboard = AnimationProvider.GetCollapsingAnimation(this);
            itemsControl = Template.FindName("itemsControl", this) as Grid;
            switchButton = Template.FindName("switchButton", this) as ToggleButton;

            if (switchButton != null)
            {
                switchButton.Checked += RaiseMenuOpenedEvent;
                switchButton.Unchecked += RaiseMenuCompactedEvent;
            }

            CacheHamburgerMenuItems();
            itemController.InjectData(Feeds);

            if (IsOpen)
            {
                StartExpandAnimation();
            }
        }

        private void OnSelectedItemChanged(HamburgerMenuItem item)
        {
            SetCurrentValue(SelectedItemProperty, item);
        }

        private void RaiseMenuCompactedEvent(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MenuCompactedEvent, this));
        }

        private void RaiseMenuOpenedEvent(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MenuOpenedEvent, this));
        }

        private void StartCollapsingAnimation()
        {
            if (collapsingStoryboard == null)
            {
                collapsingStoryboard = AnimationProvider.GetCollapsingAnimation(this);
            }

            if (itemsControl != null)
            {
                collapsingStoryboard?.Begin(itemsControl);
            }
        }

        private void StartExpandAnimation()
        {
            if (expandingStoryboard == null)
            {
                expandingStoryboard = AnimationProvider.GetExpandingAnimation(this);
            }

            if (itemsControl != null)
            {
                expandingStoryboard?.Begin(itemsControl);
            }
        }
    }
}