using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public class HamburgerMenuItem : RadioButton
    {
        public static readonly DependencyProperty BarBrushProperty = DependencyProperty.Register(
            "BarBrush", typeof(Brush), typeof(HamburgerMenuItem), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(
            "IconWidth", typeof(double), typeof(HamburgerMenuItem), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(HamburgerMenuItem), new PropertyMetadata(default(string)));

        static HamburgerMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem),
                new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));
        }

        public HamburgerMenuItem()
        {
            KeyDown += OnKeyDown;
        }
        
        public Brush BarBrush
        {
            get { return (Brush)GetValue(BarBrushProperty); }
            set { SetValue(BarBrushProperty, value); }
        }

        public double IconWidth
        {
            get { return (double) GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
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

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                SetCurrentValue(IsCheckedProperty, true);
            }
        }
    }
}