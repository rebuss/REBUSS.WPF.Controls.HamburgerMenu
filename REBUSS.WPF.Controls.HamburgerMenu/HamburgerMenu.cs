using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public class HamburgerMenu : ItemsControl
    {
        // TODO : Implement 2 routed events: Opened and Closed
        public static readonly DependencyProperty CollapseMenuTooltipProperty = DependencyProperty.Register(
            "CollapseMenuTooltip", typeof(string), typeof(HamburgerMenu), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ExpandedAreaWidthProperty = DependencyProperty.Register(
            "ExpandedAreaWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ExpandMenuTooltipProperty = DependencyProperty.Register(
            "ExpandMenuTooltip", typeof(string), typeof(HamburgerMenu), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(default(bool), OnIsOpenChanged));

        public static readonly DependencyProperty TextOpacityProperty = DependencyProperty.Register(
            "TextOpacity", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        private DockPanel itemsControl;

        private Storyboard storyboard;

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu),
                new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
        }

        public HamburgerMenu()
        {
            Loaded += OnLoaded;
        }

        public string CollapseMenuTooltip
        {
            get { return (string) GetValue(CollapseMenuTooltipProperty); }
            set { SetValue(CollapseMenuTooltipProperty, value); }
        }
        
        public double ExpandedAreaWidth
        {
            get { return (double) GetValue(ExpandedAreaWidthProperty); }
            set { SetValue(ExpandedAreaWidthProperty, value); }
        }

        public string ExpandMenuTooltip
        {
            get { return (string) GetValue(ExpandMenuTooltipProperty); }
            set { SetValue(ExpandMenuTooltipProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool) GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public double TextOpacity
        {
            get { return (double) GetValue(TextOpacityProperty); }
            set { SetValue(TextOpacityProperty, value); }
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
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(HamburgerMenu.TextOpacityProperty));
            var opacityEasingA = new EasingDoubleKeyFrame(0.0, TimeSpan.FromMilliseconds(300));
            var opacityEasingB = new EasingDoubleKeyFrame(1.0, TimeSpan.FromMilliseconds(500));
            opacityAnimation.KeyFrames.Add(opacityEasingA);
            opacityAnimation.KeyFrames.Add(opacityEasingB);
            storyboard.Children.Add(opacityAnimation);
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = d as HamburgerMenu;
            if ((bool) e.NewValue)
            {
                menu?.StartExpandAnimation();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeExpandAnimation();
            itemsControl = Template.FindName("itemsControl", this) as DockPanel;
            if (IsOpen)
            {
                StartExpandAnimation();
            }
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