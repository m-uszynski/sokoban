using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SokobanGraph
{
    class GoalLocation : Pixel
    {
        public GoalLocation(int x, int y) : base(x, y)
        {

        }

        public void draw()
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var field = (Image)mw.FindName("field" + X + Y);
            field.Source = new BitmapImage(new Uri("pack://application:,,,/sprites/EndPoint_Blue.png"));
        }
    }
}
