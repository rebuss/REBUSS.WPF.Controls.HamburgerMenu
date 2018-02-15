using System.Windows;
using System.Windows.Controls;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public class HamburgerMenu : ItemsControl
    {
        // TODO : Implement 2 routed events: Opened and Closed
        public static readonly DependencyProperty CollapseMenuTooltipProperty = DependencyProperty.Register(
            "CollapseMenuTooltip", typeof(string), typeof(HamburgerMenu), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ExpandedAreaWidthProperty = DependencyProperty.Register(
            "ExpandedAreaWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(250.0));

        public static readonly DependencyProperty ExpandMenuTooltipProperty = DependencyProperty.Register(
            "ExpandMenuTooltip", typeof(string), typeof(HamburgerMenu), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty TextOpacityProperty = DependencyProperty.Register(
            "TextOpacity", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu),
                new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
        }

        public string CollapseMenuTooltip
        {
            get { return (string) GetValue(CollapseMenuTooltipProperty); }
            set { SetValue(CollapseMenuTooltipProperty, value); }
        }

        // TODO change storyboard implementation to work with that property
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
    }
}