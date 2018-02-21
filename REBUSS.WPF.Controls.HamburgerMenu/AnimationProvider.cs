using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace REBUSS.WPF.Controls.HamburgerMenu
{
    public class AnimationProvider
    {
        public static Storyboard GetExpandingAnimation(HamburgerMenu target)
        {
            var storyboard = new Storyboard();
            storyboard.AutoReverse = false;
            var widthAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(widthAnimation, target);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));
            var widthEasing = new EasingDoubleKeyFrame(target.ExpandedAreaWidth, TimeSpan.FromMilliseconds(300));
            widthAnimation.KeyFrames.Add(widthEasing);
            storyboard.Children.Add(widthAnimation);
            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(opacityAnimation, target);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(HamburgerMenu.TextOpacityProperty));
            var opacityEasingA = new EasingDoubleKeyFrame(0.0, TimeSpan.FromMilliseconds(300));
            var opacityEasingB = new EasingDoubleKeyFrame(1.0, TimeSpan.FromMilliseconds(500));
            opacityAnimation.KeyFrames.Add(opacityEasingA);
            opacityAnimation.KeyFrames.Add(opacityEasingB);
            storyboard.Children.Add(opacityAnimation);
            return storyboard;
        }

        public static Storyboard GetCollapsingAnimation(HamburgerMenu target)
        {
            var storyboard = new Storyboard();
            storyboard.AutoReverse = false;
            var widthAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(widthAnimation, target);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));
            var widthEasing = new EasingDoubleKeyFrame(target.IconPanelWidth, TimeSpan.FromMilliseconds(300));
            widthAnimation.KeyFrames.Add(widthEasing);
            storyboard.Children.Add(widthAnimation);
            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(opacityAnimation, target);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(HamburgerMenu.TextOpacityProperty));
            var opacityEasingA = new EasingDoubleKeyFrame(1.0, TimeSpan.FromMilliseconds(0));
            var opacityEasingB = new EasingDoubleKeyFrame(0.0, TimeSpan.FromMilliseconds(200));
            opacityAnimation.KeyFrames.Add(opacityEasingA);
            opacityAnimation.KeyFrames.Add(opacityEasingB);
            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }
    }
}
