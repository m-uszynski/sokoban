using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace SokobanGraph
{
    class GameController
    {
        private Player p;
        private Level level;
        public static string currentNick;

        public GameController(Level l, string nick)
        {
            p = Board.p;
            level = l;
            currentNick = nick;
        }

        public int previousPlayerMove = -1;
        public int previousBoxMove = -1;

        public void ModifyUserLastLevel(int level)
        {
            string jsonConv = System.IO.File.ReadAllText("users.json");
            if (jsonConv == "") return;
            List<User> newUsers = JsonConvert.DeserializeObject<List<User>>(jsonConv);
            for(int i=0; i < newUsers.Count; i++)
            {
                if (newUsers[i].Nick == currentNick)
                {
                    if(newUsers[i].LastLevel<level) newUsers[i].LastLevel = level;
                }
            }
            string json = JsonConvert.SerializeObject(newUsers);
            System.IO.File.WriteAllText("users.json", json);
        }

        public void ModifyUserTimeLevel(int level, int time)
        {
            string jsonConv = System.IO.File.ReadAllText("users.json");
            if (jsonConv == "") return;
            List<User> newUsers = JsonConvert.DeserializeObject<List<User>>(jsonConv);
            for (int i = 0; i < newUsers.Count; i++)
            {
                if (newUsers[i].Nick == currentNick)
                {
                    int val;
                    if (newUsers[i].times.TryGetValue("level" + level, out val))
                    {
                        if (val < time) return;
                        newUsers[i].times["level" + level] = time;
                    }
                    else
                    {
                        newUsers[i].times.Add("level" + level, time);
                    }
                }
            }
            string json = JsonConvert.SerializeObject(newUsers);
            System.IO.File.WriteAllText("users.json", json);
        }

        public static void changeBestLevelTime(int level, string nick)
        {
            string jsonConv = System.IO.File.ReadAllText("users.json");
            if (jsonConv == "") return;
            List<User> newUsers = JsonConvert.DeserializeObject<List<User>>(jsonConv);
            for (int i = 0; i < newUsers.Count; i++)
            {
                if (newUsers[i].Nick == nick)
                {
                    int val;
                    if (newUsers[i].times.TryGetValue("level" + level, out val))
                    {
                        MainWindow mw = (MainWindow)Application.Current.MainWindow;
                        mw.bestTime.Content = "Twój najlepszy czas: " + val + " s";
                    }
                    else
                    {
                        MainWindow mw = (MainWindow)Application.Current.MainWindow;
                        mw.bestTime.Content = "Twój najlepszy czas: -";
                    }

                }
            }
        }

        public static void calculateBestUser(int level)
        {
            ArrayList rankUser = new ArrayList();
            string jsonConv = System.IO.File.ReadAllText("users.json");
            if (jsonConv == "") return;
            List<User> newUsers = JsonConvert.DeserializeObject<List<User>>(jsonConv);
            for(int i=0; i<newUsers.Count; i++)
            {
                int val;
                if (newUsers[i].times.TryGetValue("level" + level, out val))
                {
                    rankUser.Add(newUsers[i]);
                }
            }
            var sortedUser = rankUser.Cast<User>().OrderBy(r => r.times["level" +level]).ToList();

            for(int i=0; i<5; i++)
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                var userPosition = (Label)mw.FindName("bestUser" + (i + 1));
                userPosition.Content = (i + 1) + ". ---";
            }

            for(int i=0; i<sortedUser.Count; i++)
            {
                if (i < 5)
                {
                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    var userPosition = (Label)mw.FindName("bestUser" + (i + 1));
                    userPosition.Content = (i + 1) + ". " + sortedUser[i].Nick + "(" + sortedUser[i].times["level" + level] + " s)";
                }
            }
        }

        public void updatePosition(Key k)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.restartLevel.IsEnabled = true;
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
                        reDrawPlayerTo(p.X, p.Y + 1,"Down",true);
                        if (previousBoxMove == 0)
                        {
                            reDrawBoxFromTo(p.X, p.Y - 2, p.X, p.Y - 1,true);
                            if (isObject(p.X, p.Y - 2, "glocation")) drawGoalLocation(p.X, p.Y - 2);
                        }
                    }
                    if (previousPlayerMove == 1)
                    {
                        reDrawPlayerTo(p.X, p.Y - 1,"Up",true);
                        if (previousBoxMove == 1)
                        {
                            reDrawBoxFromTo(p.X, p.Y + 2, p.X, p.Y + 1,true);
                            if (isObject(p.X, p.Y + 2, "glocation")) drawGoalLocation(p.X, p.Y + 2);
                        }
                    }
                    if (previousPlayerMove == 2)
                    {
                        reDrawPlayerTo(p.X + 1, p.Y,"Right",true);
                        if (previousBoxMove == 2)
                        {
                            reDrawBoxFromTo(p.X - 2, p.Y, p.X - 1, p.Y,true);
                            if (isObject(p.X - 2, p.Y, "glocation")) drawGoalLocation(p.X - 2, p.Y);
                        }
                    }
                    if (previousPlayerMove == 3)
                    {
                        reDrawPlayerTo(p.X - 1, p.Y,"Left",true);
                        if (previousBoxMove == 3)
                        {
                            reDrawBoxFromTo(p.X + 2, p.Y, p.X + 1, p.Y,true);
                            if (isObject(p.X + 2, p.Y, "glocation")) drawGoalLocation(p.X + 2, p.Y);
                        }
                    }
                    previousPlayerMove = -1;
                    previousBoxMove = -1;
                    mw.backMoveButton.IsEnabled = false;
                    break;

                case Key.R:
                    level.generateLevel();
                    break;

                case Key.Q:
                    Menu m = new Menu();
                    MainWindow.dispatcherTimer.Stop();
                    MainWindow.dispatcherTimer = null;
                    m.Show();
                    mw.Close();
                    break;
            }
        }

        private void checkWin()
        {
            bool finishedGame = (level.numberLevel == Level.levelCount());
            if (level.isLevelWin() && !finishedGame)
            {
                MainWindow.dispatcherTimer.Stop();
                ModifyUserTimeLevel(level.numberLevel, MainWindow.second);
                calculateBestUser(level.numberLevel);
                WinLevel wl = new WinLevel();
                if (wl.ShowDialog() == true)
                {
                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    switch (wl.Choice)
                    {
                        case 1:
                            ModifyUserLastLevel(level.numberLevel+1);
                            Menu m = new Menu();
                            m.Show();
                            mw.Close();
                            break;

                        case 2:
                            ModifyUserLastLevel(level.numberLevel+1);
                            level.generateLevel();
                            break;

                        case 3:
                            level.numberLevel++;
                            ModifyUserLastLevel(level.numberLevel);
                            level.generateLevel();
                            break;
                    }
                }
                else
                {
                    level.numberLevel++;
                    level.generateLevel();
                }
            }
            if(level.isLevelWin() && finishedGame)
            {
                ModifyUserLastLevel(level.numberLevel + 1);
                MainWindow.dispatcherTimer.Stop();
                ModifyUserTimeLevel(level.numberLevel, MainWindow.second);
                calculateBestUser(level.numberLevel);
                FinishGame fg = new FinishGame();
                if (fg.ShowDialog() == true)
                {
                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    switch (fg.Choice)
                    {
                        case 1:
                            Menu m = new Menu();
                            m.Show();
                            mw.Close();
                            break;

                        case 2:
                            level.generateLevel();
                            break;
                    }
                }
                else
                {
                    ModifyUserLastLevel(level.numberLevel + 1);
                    level.generateLevel();
                }
            }
        }

        private void reDrawBoxFromTo(int bx, int by, int x, int y, bool isBackMovement = false)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            Box b = Board.boxes.Find(a => a.X == bx && a.Y == by);
            GoalLocation g = Board.glocations.Find(a => a.X == x && a.Y == y);
            if (b == null) return;
            b.ClearPrevious();
            b.X = x;
            b.Y = y;
            if (!isBackMovement) p.moveBoxCount++;
            else p.moveBoxCount--;
            mw.boxMove.Content = "Przesunięcia: " + p.moveBoxCount;
            if (g != null) b.draw(true);
            else b.draw(false);
        }

        private void reDrawPlayerTo(int x, int y,String direction="Down",bool isBackMovement = false)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            p.ClearPrevious();
            p.X = x;
            p.Y = y;
            if (!isBackMovement) p.moveCount++;
            else
            {
                p.backMovementCount++;
                p.moveCount--;
            }
            mw.playerMove.Content = "Ruchy: " + p.moveCount;
            mw.backMove.Content = "Cofnięcia: " + p.backMovementCount;
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
