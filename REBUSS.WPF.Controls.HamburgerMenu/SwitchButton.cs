using System.Windows;
using System.Windows.Controls.Primitives;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    internal class SwitchButton : ToggleButton
    {
        public static readonly DependencyProperty CompactMenuTooltipProperty = DependencyProperty.Register(
            "CompactMenuTooltip", typeof(string), typeof(SwitchButton), new PropertyMetadata("Collapse"));

        public static readonly DependencyProperty OpenMenuTooltipProperty = DependencyProperty.Register(
            "OpenMenuTooltip", typeof(string), typeof(SwitchButton), new PropertyMetadata("Expand"));

        static SwitchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchButton), new FrameworkPropertyMetadata(typeof(SwitchButton)));
        }
        
        public string CompactMenuTooltip
        {
            get { return (string) GetValue(CompactMenuTooltipProperty); }
            set { SetValue(CompactMenuTooltipProperty, value); }
        }

        public string OpenMenuTooltip
        {
            get { return (string) GetValue(OpenMenuTooltipProperty); }
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