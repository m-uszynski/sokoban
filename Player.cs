using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SokobanGraph
{
    class Player : Pixel
    {
        public int moveCount { get; set; }
        public int moveBoxCount { get; set; }
        public int backMovementCount { get; set; }

        public Player(int x, int y) : base(x, y)
        {
            moveCount = 0;
            moveBoxCount = 0;
            backMovementCount = 0;
        }

        public void ClearPrevious()
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var field = (Image)mw.FindName("field" + X + Y);
            if (Board.glocations.Exists(a => a.X == X && a.Y == Y))
            {
                field.Source = new BitmapImage(new Uri("pack://application:,,,/sprites/EndPoint_Blue.png"));
            }
            else
            {
                field.Source = null;
            }
        }

        public void draw(String direction="Down")
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var field = (Image)mw.FindName("field" + X + Y);
            Uri dirUri = new Uri("pack://application:,,,/sprites/Character" + direction + ".png");
            if (Board.glocations.Exists(a => a.X == X && a.Y == Y))
            {
                var group = new DrawingGroup();
                
                group.Children.Add(new ImageDrawing(new BitmapImage(new Uri("pack://application:,,,/sprites/EndPoint_Blue.png")), new Rect(0, 0, 64, 64)));
                group.Children.Add(new ImageDrawing(new BitmapImage(new Uri("pack://application:,,,/sprites/Character" + direction + ".png")), new Rect(0, 0, 64, 64)));

                field.Source = new DrawingImage(group);
            }
            else
            {
                field.Source = new BitmapImage(dirUri);
            }
        }

    }
}
