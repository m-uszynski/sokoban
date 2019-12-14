using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SokobanGraph
{
    class Board
    {
        public static bool isBoardRestart = false;
        public static List<Wall> walls = new List<Wall>();
        public static List<Box> boxes = new List<Box>();
        public static List<GoalLocation> glocations = new List<GoalLocation>();
        public static Player p = new Player(0, 0);

        public void drawPlayer()
        {
            p.draw();
        }

        public void drawWalls()
        {
            for(int i=0; i<walls.Count; i++)
            {
                walls[i].draw();
            }
        }

        public void drawBoxes()
        {
            for(int i=0; i<boxes.Count; i++)
            {
                boxes[i].draw();
            }
        }

        public void drawGLocation()
        {
            for(int i=0; i<glocations.Count; i++)
            {
                glocations[i].draw();
            }
        }

        public void readBoard(int level)
        {
            walls.Clear();
            boxes.Clear();
            glocations.Clear();
            string[] lines = File.ReadAllLines("level" + level + ".txt");

            int x = 0;
            int y = 0;

            foreach(string line in lines)
            {
                x = 0;
                foreach(char c in line)
                {
                    if (c == 'x') walls.Add(new Wall(x, y));
                    if (c == '@') boxes.Add(new Box(x, y));
                    if (c == 'w') glocations.Add(new GoalLocation(x, y));
                    if (c == 'g')
                    {
                        p.X = x;
                        p.Y = y;
                    }
                    x++;
                }
                y++;
            }

        }

        public void clearBoard()
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.backMoveButton.IsEnabled = false;
            for (int i=0; i<9; i++)
            {
                for(int j=0; j<9; j++)
                {
                    var field = (Image)mw.FindName("field" + i + j);
                    field.Source = null;
                }
            }
            p.moveCount = 0;
            p.moveBoxCount = 0;
            p.backMovementCount = 0;
            mw.playerMove.Content = "Ruchy: " + p.moveCount;
            mw.boxMove.Content = "Przesunięcia: " + p.moveBoxCount;
            mw.backMove.Content = "Cofnięcia: " + p.backMovementCount;
        }

        public bool isWin()
        {
            bool win = true;
            for(int i=0; i<boxes.Count; i++)
            {
                int bx = boxes[i].X;
                int by = boxes[i].Y;
                if (!glocations.Exists(a => a.X == bx && a.Y == by)) win = false;
            }
            return win;
        }

    }
}
