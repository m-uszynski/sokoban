using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SokobanGraph
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameController gc = new GameController();
        Level level = new Level(1);
        //Board b = new Board();

        public MainWindow()
        {
            InitializeComponent();

            //level.numberLevel = 3;
            level.generateLevel();

            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }


        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            gc.updatePosition(e.Key);
        }

    }

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
            mw.playerMove.Content = "Ruchy: " + p.moveCount;
            mw.boxMove.Content = "Przesunięcia: " + p.moveBoxCount;
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

    class Level
    {
        public int numberLevel { get; set; }

        public Level(int numberLevel)
        {
            this.numberLevel = numberLevel;
        }

        Board b = new Board();

        public void generateLevel()
        {
            Board.isBoardRestart = true;
            b.clearBoard();
            b.readBoard(numberLevel);
            b.drawWalls();
            b.drawBoxes();
            b.drawGLocation();
            b.drawPlayer();
        }

        public bool isLevelWin()
        {
            if (b.isWin()) return true;
            else return false;
        }

        public static int levelCount()
        {
            int count = 1;
            bool check = true;
            while (check)
            {
                if (File.Exists("level" + count + ".txt")) count++;
                else check = false;
            }
            return count - 1;
        }

    }

    class GameController
    {
        private Player p;
        private Level level = new Level(1);
        

        public GameController()
        {
            p = Board.p;
        }

        public int previousPlayerMove = -1;
        public int previousBoxMove = -1;

        public void updatePosition(Key k)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            switch (k)
            {
                case Key.Up:
                    if (isObject(p.X, p.Y - 1, "wall")) break;
                    if (isObject(p.X, p.Y - 1, "box"))
                    {
                        if (!isObject(p.X, p.Y - 2))
                        {
                            previousBoxMove = 0;
                            reDrawBoxFromTo(p.X, p.Y - 1, p.X, p.Y - 2);
                        }
                        else break;
                    }
                    Board.isBoardRestart = false;
                    mw.backMoveButton.IsEnabled = true;
                    reDrawPlayerTo(p.X, p.Y - 1, "Up");
                    previousPlayerMove = 0;
                    checkWin();
                    break;

                case Key.Down:
                    if (isObject(p.X, p.Y + 1, "wall")) break;
                    if (isObject(p.X, p.Y + 1, "box"))
                    {
                        if (!isObject(p.X, p.Y + 2))
                        {
                            previousBoxMove = 1;
                            reDrawBoxFromTo(p.X, p.Y + 1, p.X, p.Y + 2);
                        }
                        else break;
                    }
                    Board.isBoardRestart = false;
                    mw.backMoveButton.IsEnabled = true;
                    reDrawPlayerTo(p.X, p.Y + 1, "Down");
                    previousPlayerMove = 1;
                    checkWin();
                    break;

                case Key.Left:
                    if (isObject(p.X - 1, p.Y, "wall")) break;
                    if (isObject(p.X - 1, p.Y, "box"))
                    {
                        if (!isObject(p.X - 2, p.Y))
                        {
                            previousBoxMove = 2;
                            reDrawBoxFromTo(p.X - 1, p.Y, p.X - 2, p.Y);
                        }
                        else break;
                    }
                    Board.isBoardRestart = false;
                    mw.backMoveButton.IsEnabled = true;
                    reDrawPlayerTo(p.X - 1, p.Y, "Left");
                    previousPlayerMove = 2;
                    checkWin();
                    break;

                case Key.Right:
                    if (isObject(p.X + 1, p.Y, "wall")) break;
                    if (isObject(p.X + 1, p.Y, "box"))
                    {
                        if (!isObject(p.X + 2, p.Y))
                        {
                            previousBoxMove = 3;
                            reDrawBoxFromTo(p.X + 1, p.Y, p.X + 2, p.Y);
                        }
                        else break;
                    }
                    Board.isBoardRestart = false;
                    mw.backMoveButton.IsEnabled = true;
                    reDrawPlayerTo(p.X + 1, p.Y, "Right");
                    previousPlayerMove = 3;
                    checkWin();
                    break;
                case Key.P:
                    
                    if (Board.isBoardRestart)
                    {
                        
                        previousBoxMove = -1;
                        previousBoxMove = -1;
                        mw.backMoveButton.IsEnabled = false;
                        break;
                    }
                    if (previousPlayerMove == 0)
                    {
                        reDrawPlayerTo(p.X, p.Y + 1,"Down");
                        if (previousBoxMove == 0)
                        {
                            reDrawBoxFromTo(p.X, p.Y - 2, p.X, p.Y - 1);
                            if (isObject(p.X, p.Y - 2, "glocation")) drawGoalLocation(p.X, p.Y - 2);
                        }
                    }
                    if (previousPlayerMove == 1)
                    {
                        reDrawPlayerTo(p.X, p.Y - 1,"Up");
                        if (previousBoxMove == 1)
                        {
                            reDrawBoxFromTo(p.X, p.Y + 2, p.X, p.Y + 1);
                            if (isObject(p.X, p.Y + 2, "glocation")) drawGoalLocation(p.X, p.Y + 2);
                        }
                    }
                    if (previousPlayerMove == 2)
                    {
                        reDrawPlayerTo(p.X + 1, p.Y,"Right");
                        if (previousBoxMove == 2)
                        {
                            reDrawBoxFromTo(p.X - 2, p.Y, p.X - 1, p.Y);
                            if (isObject(p.X - 2, p.Y, "glocation")) drawGoalLocation(p.X - 2, p.Y);
                        }
                    }
                    if (previousPlayerMove == 3)
                    {
                        reDrawPlayerTo(p.X - 1, p.Y,"Left");
                        if (previousBoxMove == 3)
                        {
                            reDrawBoxFromTo(p.X + 2, p.Y, p.X + 1, p.Y);
                            if (isObject(p.X + 2, p.Y, "glocation")) drawGoalLocation(p.X + 2, p.Y);
                        }
                    }
                    previousPlayerMove = -1;
                    previousBoxMove = -1;
                    mw.backMoveButton.IsEnabled = false;
                    break;
            }
        }

        private void checkWin()
        {
            if (level.isLevelWin())
            {
                MessageBox.Show("Wygrana", "Wygrana");
                level.numberLevel++;
                level.generateLevel();
            }
        }

        private void reDrawBoxFromTo(int bx, int by, int x, int y)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            Box b = Board.boxes.Find(a => a.X == bx && a.Y == by);
            GoalLocation g = Board.glocations.Find(a => a.X == x && a.Y == y);
            if (b == null) return;
            b.ClearPrevious();
            b.X = x;
            b.Y = y;
            p.moveBoxCount++;
            mw.boxMove.Content = "Przesunięcia: " + p.moveBoxCount;
            if (g != null) b.draw(true);
            else b.draw(false);
        }

        private void reDrawPlayerTo(int x, int y,String direction="Down")
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            p.ClearPrevious();
            p.X = x;
            p.Y = y;
            p.moveCount++;
            mw.playerMove.Content = "Ruchy: " + p.moveCount;
            p.draw(direction);
        }

        private bool isObject(int x, int y)
        {
            bool empty = false;
            if (Board.walls.Exists(a => a.X == x && a.Y == y)) empty = true;
            if (Board.boxes.Exists(a => a.X == x && a.Y == y)) empty = true;
            return empty;
        }

        private void drawGoalLocation(int x, int y)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var field = (Image)mw.FindName("field" + x + y);
            field.Source = new BitmapImage(new Uri("pack://application:,,,/sprites/EndPoint_Blue.png"));
        }

        private bool isObject(int x, int y,string obj)
        {
            bool empty = false;
            if (obj == "wall") if (Board.walls.Exists(a => a.X == x && a.Y == y)) empty = true;
            if (obj == "box") if (Board.boxes.Exists(a => a.X == x && a.Y == y)) empty = true;
            if (obj == "glocation") if (Board.glocations.Exists(a => a.X == x && a.Y == y)) empty = true;
            return empty;
        }
    }
}
