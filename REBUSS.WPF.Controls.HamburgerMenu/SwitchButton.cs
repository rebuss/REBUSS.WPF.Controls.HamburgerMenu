﻿using System.Windows;
using System.Windows.Controls.Primitives;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    internal class SwitchButton : ToggleButton
    {
        public static readonly DependencyProperty CompactMenuTooltipProperty = DependencyProperty.Register(
            "CompactMenuTooltip", typeof(object), typeof(SwitchButton), new PropertyMetadata("Collapse"));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(SwitchButton), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty OpenMenuTooltipProperty = DependencyProperty.Register(
            "OpenMenuTooltip", typeof(object), typeof(SwitchButton), new PropertyMetadata("Expand"));

        static SwitchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchButton),
                new FrameworkPropertyMetadata(typeof(SwitchButton)));
        }

        public object CompactMenuTooltip
        {
            get { return GetValue(CompactMenuTooltipProperty); }
            set { SetValue(CompactMenuTooltipProperty, value); }
        }

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public object OpenMenuTooltip
        {
            get { return GetValue(OpenMenuTooltipProperty); }
            set { SetValue(OpenMenuTooltipProperty, value); }
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            ToolTip = CompactMenuTooltip;
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            ToolTip = OpenMenuTooltip;
        }
    }
}