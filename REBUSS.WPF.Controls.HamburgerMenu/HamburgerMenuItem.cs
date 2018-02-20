using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public class HamburgerMenuItem : RadioButton
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(HamburgerMenuItem), new PropertyMetadata(default(string)));
        
        static HamburgerMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem),
                new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));
        }
        
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        
        internal void UpdateWith(ItemFeed feed)
        {
            if (feed != null)
            {
                SetCurrentValue(ContentProperty, feed.IconContent);
                SetCurrentValue(TextProperty, feed.Label);
                SetCurrentValue(CommandProperty, feed.Command);
                SetCurrentValue(ToolTipProperty, feed.Tooltip);
            }
        }

        // TODO implement GetHashCode
    }
}