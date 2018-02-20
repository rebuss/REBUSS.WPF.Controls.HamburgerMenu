using System;
using System.Linq;
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
        
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(default(bool), OnIsOpenChanged));

        public static readonly DependencyProperty TextOpacityProperty = DependencyProperty.Register(
            "TextOpacity", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        public static readonly RoutedEvent MenuClosedEvent =
            EventManager.RegisterRoutedEvent("MenuClosed", RoutingStrategy.Bubble,
                typeof(MenuClosedEventHandler), typeof(HamburgerMenu));

        public static readonly RoutedEvent MenuOpenedEvent =
            EventManager.RegisterRoutedEvent("MenuOpened", RoutingStrategy.Bubble,
                typeof(MenuOpenedEventHandler), typeof(HamburgerMenu));
        
        private DockPanel itemsControl;

        private readonly ItemController itemController = new ItemController();

        private Storyboard storyboard;

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

        private void OnSelectedItemChanged(HamburgerMenuItem item)
        {
            SetCurrentValue(SelectedItemProperty, item);
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

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public double TextOpacity
        {
            get { return (double)GetValue(TextOpacityProperty); }
            set { SetValue(TextOpacityProperty, value); }
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

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new HamburgerMenuItem();
        }

        private void InitializeExpandAnimation()
        {
            storyboard = new Storyboard();
            storyboard.AutoReverse = false;
            var widthAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(widthAnimation, this);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));
            var widthEasing = new EasingDoubleKeyFrame(ExpandedAreaWidth, TimeSpan.FromMilliseconds(300));
            widthAnimation.KeyFrames.Add(widthEasing);
            storyboard.Children.Add(widthAnimation);
            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(opacityAnimation, this);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(TextOpacityProperty));
            var opacityEasingA = new EasingDoubleKeyFrame(0.0, TimeSpan.FromMilliseconds(300));
            var opacityEasingB = new EasingDoubleKeyFrame(1.0, TimeSpan.FromMilliseconds(500));
            opacityAnimation.KeyFrames.Add(opacityEasingA);
            opacityAnimation.KeyFrames.Add(opacityEasingB);
            storyboard.Children.Add(opacityAnimation);
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = d as HamburgerMenu;
            if ((bool)e.NewValue)
            {
                menu?.StartExpandAnimation();
            }
        }

        private void CacheHamburgerMenuItems()
        {
            foreach (var item in Items)
            {
                HamburgerMenuItem menuItem = null;
                if (item is HamburgerMenuItem)
                {
                    menuItem = item as HamburgerMenuItem;
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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeExpandAnimation();
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

        private void RaiseMenuClosedEvent(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MenuClosedEvent, this));
        }

        private void RaiseMenuOpenedEvent(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MenuOpenedEvent, this));
        }

        private void StartExpandAnimation()
        {
            if (storyboard == null)
            {
                InitializeExpandAnimation();
            }

            if (itemsControl != null)
            {
                storyboard?.Begin(itemsControl);
            }
        }
    }
}