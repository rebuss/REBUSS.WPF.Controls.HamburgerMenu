using System.Windows;
using System.Windows.Controls;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public class HamburgerMenuItem : RadioButton
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(HamburgerMenuItem), new PropertyMetadata(default(string)));

        static HamburgerMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}