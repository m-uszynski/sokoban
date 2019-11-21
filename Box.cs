using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SokobanGraph
{
    class Box : Pixel
    {
        public Box(int x, int y) : base(x, y)
        {

        }

        public void ClearPrevious()
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var field = (Image)mw.FindName("field" + X + Y);
            field.Source = null;
        }

        public void draw(bool isGoalLocation = false)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var field = (Image)mw.FindName("field" + X + Y);
            field.Source = new BitmapImage(new Uri("pack://application:,,,/sprites/Crate_Yellow.png"));
            
            if (!isGoalLocation)
            {
                field.Source = new BitmapImage(new Uri("pack://application:,,,/sprites/Crate_Yellow.png"));
            }
            else
            {
                field.Source = new BitmapImage(new Uri("pack://application:,,,/sprites/CrateDark_Blue.png"));
            }
        }
    }
}
