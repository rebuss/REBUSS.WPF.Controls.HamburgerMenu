using System.Windows;
using System.Windows.Controls.Primitives;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    internal class SwitchButton : ToggleButton
    {
        public static readonly DependencyProperty CollapseMenuTooltipProperty = DependencyProperty.Register(
            "CollapseMenuTooltip", typeof(string), typeof(SwitchButton), new PropertyMetadata("Collapse"));

        public static readonly DependencyProperty ExpandMenuTooltipProperty = DependencyProperty.Register(
            "ExpandMenuTooltip", typeof(string), typeof(SwitchButton), new PropertyMetadata("Expand"));

        static SwitchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchButton), new FrameworkPropertyMetadata(typeof(SwitchButton)));
        }
        
        public string CollapseMenuTooltip
        {
            get { return (string) GetValue(CollapseMenuTooltipProperty); }
            set { SetValue(CollapseMenuTooltipProperty, value); }
        }

        public string ExpandMenuTooltip
        {
            get { return (string) GetValue(ExpandMenuTooltipProperty); }
            set { SetValue(ExpandMenuTooltipProperty, value); }
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            ToolTip = CollapseMenuTooltip;
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            ToolTip = ExpandMenuTooltip;
        }
    }
}