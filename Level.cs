using System;
using System.IO;
using System.Windows;

namespace SokobanGraph
{
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
            MainWindow.second = 0;
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            GameController.changeBestLevelTime(numberLevel, GameController.currentNick);
            GameController.calculateBestUser(numberLevel);
            mw.timer.Content = "Czas: " + 0 + " s";
            Board.isBoardRestart = true;
            b.clearBoard();
            b.readBoard(numberLevel);
            b.drawWalls();
            b.drawBoxes();
            b.drawGLocation();
            b.drawPlayer();
            MainWindow.dispatcherTimer.Start();
            mw.levelNumber.Content = "Level " + numberLevel;
            mw.restartLevel.IsEnabled = false;
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
}
