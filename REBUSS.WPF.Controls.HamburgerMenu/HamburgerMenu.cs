using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public class HamburgerMenu : Selector
    {
        public static readonly DependencyProperty CollapseMenuTooltipProperty = DependencyProperty.Register(
            "CollapseMenuTooltip", typeof(string), typeof(HamburgerMenu), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ExpandedAreaWidthProperty = DependencyProperty.Register(
            "ExpandedAreaWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ExpandMenuTooltipProperty = DependencyProperty.Register(
            "ExpandMenuTooltip", typeof(string), typeof(HamburgerMenu), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty FeedsProperty = DependencyProperty.Register(
            "Feeds", typeof(FeedCollection), typeof(HamburgerMenu), new PropertyMetadata(new FeedCollection()));
        
        public static readonly DependencyProperty IconPanelWidthProperty = DependencyProperty.Register(
            "IconPanelWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(50.0));
        
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(default(bool), OnIsOpenChanged));

        public static readonly DependencyProperty SwitchButtonContentProperty = DependencyProperty.Register(
            "SwitchButtonContent", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty SwitchButtonHeightProperty = DependencyProperty.Register(
            "SwitchButtonHeight", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(46.0));
        
        public static readonly DependencyProperty TextOpacityProperty = DependencyProperty.Register(
            "TextOpacity", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        public static readonly RoutedEvent MenuClosedEvent =
            EventManager.RegisterRoutedEvent("MenuClosed", RoutingStrategy.Bubble,
        typeof(MenuClosedEventHandler), typeof(HamburgerMenu));

        public static readonly RoutedEvent MenuOpenedEvent =
            EventManager.RegisterRoutedEvent("MenuOpened", RoutingStrategy.Bubble,
                typeof(MenuOpenedEventHandler), typeof(HamburgerMenu));
        
        private readonly ItemController itemController = new ItemController();

        private Storyboard collapsingStoryboard;

        private Storyboard expandingStoryboard;

        private DockPanel itemsControl;
        
        private ToggleButton switchButton;

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu),
                new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
        }

        public HamburgerMenu()
        {
            Loaded += OnLoaded;
            // TODO remove handler
            itemController.SelectedItemChanged += OnSelectedItemChanged;
        }

        public event MenuClosedEventHandler MenuClosed
        {
            add { AddHandler(MenuClosedEvent, value); }
            remove { RemoveHandler(MenuClosedEvent, value); }
        }

        public event MenuOpenedEventHandler MenuOpened
        {
            add { AddHandler(MenuOpenedEvent, value); }
            remove { RemoveHandler(MenuOpenedEvent, value); }
        }

        public string CollapseMenuTooltip
        {
            get { return (string)GetValue(CollapseMenuTooltipProperty); }
            set { SetValue(CollapseMenuTooltipProperty, value); }
        }

        // TODO rename
        public double ExpandedAreaWidth
        {
            get { return (double)GetValue(ExpandedAreaWidthProperty); }
            set { SetValue(ExpandedAreaWidthProperty, value); }
        }

        public string ExpandMenuTooltip
        {
            get { return (string)GetValue(ExpandMenuTooltipProperty); }
            set { SetValue(ExpandMenuTooltipProperty, value); }
        }

        public FeedCollection Feeds
        {
            get { return (FeedCollection)GetValue(FeedsProperty); }
            set { SetValue(FeedsProperty, value); }
        }
        
        public double IconPanelWidth
        {
            get { return (double)GetValue(IconPanelWidthProperty); }
            set { SetValue(IconPanelWidthProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public object SwitchButtonContent
        {
            get { return GetValue(SwitchButtonContentProperty); }
            set { SetValue(SwitchButtonContentProperty, value); }
        }

        public double SwitchButtonHeight
        {
            get { return (double)GetValue(SwitchButtonHeightProperty); }
            set { SetValue(SwitchButtonHeightProperty, value); }
        }

        public double TextOpacity
        {
            get { return (double)GetValue(TextOpacityProperty); }
            set { SetValue(TextOpacityProperty, value); }
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
                        menuItem.IconWidth = IconPanelWidth;
                    }

                    if (ItemsSource == null || !ItemsSource.GetEnumerator().MoveNext())
                    {
                        var newFeed = new ItemFeed
                        {
                            Command = menuItem.Command,
                            IconContent = menuItem.Content,
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

                itemController.AddItem(menuItem);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new HamburgerMenuItem
            {
                IconWidth = IconPanelWidth,
                FontSize = FontSize,
            };
        }
        
        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = d as HamburgerMenu;
            if ((bool)e.NewValue)
            {
                menu?.StartExpandAnimation();
            }
            else
            {
                menu?.StartCollapsingAnimation();
            }
        }
        
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            expandingStoryboard = AnimationProvider.GetExpandingAnimation(this);
            collapsingStoryboard = AnimationProvider.GetCollapsingAnimation(this);
            itemsControl = Template.FindName("itemsControl", this) as DockPanel;
            switchButton = Template.FindName("switchButton", this) as ToggleButton;

            if (switchButton != null)
            {
                switchButton.Checked += RaiseMenuOpenedEvent;
                switchButton.Unchecked += RaiseMenuClosedEvent;
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

        private void RaiseMenuClosedEvent(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MenuClosedEvent, this));
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