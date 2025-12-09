using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PCMover.Models
{
    public class AnimationClass
    {
        Storyboard storyboardTab1 = new Storyboard();
        Storyboard storyboardTab2 = new Storyboard();

        public AnimationClass() { }

        public void Clear()
        {
            storyboardTab1.Remove();
            storyboardTab2.Remove();
        }

        public void AnimationLineActive(Line line, Grid headerGrid)
        {
            Clear();
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,
                To = headerGrid.ActualWidth,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            Storyboard.SetTarget(da, line);
            Storyboard.SetTargetProperty(da, new PropertyPath(Line.X1Property));

            storyboardTab1.Children.Add(da);
            storyboardTab1.Begin();
        }

        public void AnimationLineInactive(Line line, Grid headerGrid)
        {
            Clear();
            DoubleAnimation da = new DoubleAnimation
            {
                From = headerGrid.ActualWidth,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            Storyboard.SetTarget(da, line);
            Storyboard.SetTargetProperty(da, new PropertyPath(Line.X1Property));

            storyboardTab2.Children.Add(da);
            storyboardTab2.Begin();
        }
    }
}
